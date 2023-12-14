using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace OMSITexManSettings
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);

        private float LoadedTextureMemory = .0f;
        private bool FirstUpdate = true;
        private bool UnsavedChanges = false;
        private DateTime LastUpdateTime;

        private struct CopyDataStruct
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;
        }

        // !!! Make sure this struct PERFECTLY matches its C++ counterpart !!!
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        private struct SharedTextureInfo
        {
            public ushort width;
            public ushort height;
            public float memory;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] public string path;
            [MarshalAs(UnmanagedType.I1)] public bool loaded;
            [MarshalAs(UnmanagedType.I1)] public bool first_texman;
            [MarshalAs(UnmanagedType.I1)] private bool pad_1;
            [MarshalAs(UnmanagedType.I1)] private bool pad_2;
        }

        private enum ColumnSort
        {
            Descending_Name,
            Descending_Resolution,
            Descending_Memory,
            Ascending_Name,
            Ascending_Resolution,
            Ascending_Memory
        }
        private static ColumnSort SortType = ColumnSort.Descending_Memory;

        public class TextureListComparer : IComparer
        {
            public int Compare(object a, object b)
            {
                ListViewItem lviA = (ListViewItem)a;
                ListViewItem lviB = (ListViewItem)b;

                if (SortType == ColumnSort.Descending_Name)
                {
                    return string.Compare(lviA.Text, lviB.Text);
                }
                else if (SortType == ColumnSort.Descending_Resolution)
                {
                    if (int.Parse(lviA.SubItems[1].Tag.ToString()) < int.Parse(lviB.SubItems[1].Tag.ToString()))
                    {
                        return 1;
                    }
                    else if (int.Parse(lviA.SubItems[1].Tag.ToString()) > int.Parse(lviB.SubItems[1].Tag.ToString()))
                    {
                        return -1;
                    }
                }
                else if (SortType == ColumnSort.Descending_Memory)
                {
                    if (float.Parse(lviA.SubItems[2].Tag.ToString()) < float.Parse(lviB.SubItems[2].Tag.ToString()))
                    {
                        return 1;
                    }
                    else if (float.Parse(lviA.SubItems[2].Tag.ToString()) > float.Parse(lviB.SubItems[2].Tag.ToString()))
                    {
                        return -1;
                    }
                }
                else if (SortType == ColumnSort.Ascending_Name)
                {
                    return string.Compare(lviB.Text, lviA.Text);
                }
                else if (SortType == ColumnSort.Ascending_Resolution)
                {
                    if (int.Parse(lviB.SubItems[1].Tag.ToString()) < int.Parse(lviA.SubItems[1].Tag.ToString()))
                    {
                        return 1;
                    }
                    else if (int.Parse(lviB.SubItems[1].Tag.ToString()) > int.Parse(lviA.SubItems[1].Tag.ToString()))
                    {
                        return -1;
                    }
                }
                else if (SortType == ColumnSort.Ascending_Memory)
                {
                    if (float.Parse(lviB.SubItems[2].Tag.ToString()) < float.Parse(lviA.SubItems[2].Tag.ToString()))
                    {
                        return 1;
                    }
                    else if (float.Parse(lviB.SubItems[2].Tag.ToString()) > float.Parse(lviA.SubItems[2].Tag.ToString()))
                    {
                        return -1;
                    }
                }

                return 0;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (!updateCheckBox.Checked && m.Msg == 0x4A /* WM_COPYDATA */)
            {
                CopyDataStruct cds = (CopyDataStruct)Marshal.PtrToStructure(m.LParam, typeof(CopyDataStruct));

                if (cds.dwData == (IntPtr)0x6D656F77 /* meow :3 */) 
                {
                    updateCheckBox.Text = "Pause updates (parsing information, please wait...)";
                    updateCheckBox.Show();
                    updateCheckBox.Update();

                    textureListView.BeginUpdate();
                    textureListView.Items.Clear();

                    int sti_size = Marshal.SizeOf(typeof(SharedTextureInfo));
                    int count = cds.cbData / sti_size;
                    SharedTextureInfo[] stis = new SharedTextureInfo[count];

                    int loaded_count = 0;
                    float memory = .0f;
                    for (int i = 0; i < count; i++)
                    {
                        // Copy the actual data
                        stis[i] = (SharedTextureInfo)Marshal.PtrToStructure(cds.lpData + sti_size * i, typeof(SharedTextureInfo));

                        // Create a LVI with the specified first column argument
                        // We then create sub-items for it which will represent arguments for other columns
                        // We make use of each sub-item's "Tag" property for user-defined values, for later sorting
                        ListViewItem lvi = new ListViewItem(stis[i].path);

                        // Resolution is formated like so: 1024x1024 (blank if it's not loaded)
                        // Memory is formatted like so: 1234.56 MB (blank if it's not loaded)
                        if (stis[i].loaded)
                        {
                            memory += stis[i].memory;
                            loaded_count++;

                            int size = stis[i].width * stis[i].height;
                            lvi.SubItems.Add(stis[i].width.ToString() + "x" + stis[i].height.ToString()).Tag = size.ToString();

                            bool over1mb = stis[i].memory > 1048576.0f;
                            lvi.SubItems.Add(
                                (over1mb ? (stis[i].memory / 1048576.0f).ToString("0.00") : (stis[i].memory / 1024.0f).ToString("0.00")) +
                                (over1mb ? " MB" : " KB")).Tag = (stis[i].memory).ToString("0.00");
                        }
                        else
                        {
                            lvi.SubItems.Add("").Tag = 0;
                            lvi.SubItems.Add("").Tag = 0.0f;
                        }

                        textureListView.Items.Add(lvi);
                    }

                    UpdateMemoryInfo(memory, count, loaded_count);
                    textureListView.EndUpdate();
                    return;
                }
            }

            base.WndProc(ref m);
        }

        private void UpdateMemoryInfo(float memory_bytes, int count, int loaded_count)
        {
            LoadedTextureMemory = memory_bytes;
            float memory = memory_bytes / 1048576.0f;

            // For our first memory update, add the missing Game Textures page
            if (FirstUpdate)
            {
                tabControl.TabPages.Add(texturesPage);
                FirstUpdate = false;
            }

            // Update our last update time, as well as memory counts, and restart the timer
            updateCheckBox.Text = "Pause updates (last updated 00:00 ago)";
            LastUpdateTime = DateTime.Now;
            updateTimer.Stop();
            updateTimer.Start();

            memUsageLabel.Text = "Loaded texture memory: " + memory.ToString("0.00") + " MB" +
                Environment.NewLine + "Texture count: " + count + " (" + loaded_count + " loaded)";

            // Change our progress bar color depending on how much memory is used
            memUsageBar.Value = (int)memory;
            int state = 1; // green
            if (memUsageBar.Value > 1536)
            {
                state = 2; // red
            }
            else if (memUsageBar.Value > 1024)
            {
                state = 3; // yellow
            }
            SendMessage(memUsageBar.Handle, 0x410 /* PBM_SETSTATE */, (IntPtr)state, IntPtr.Zero);
            memUsageBar.Update();

            textureListView.Sort();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Don't show the Game Textures page until we receive an update
            tabControl.TabPages.Remove(texturesPage);

            textureListView.ListViewItemSorter = new TextureListComparer();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (UnsavedChanges)
            {
                DialogResult dr = MessageBox.Show("You have unsaved changes - do you want to save them?",
                    "OMSITextureManager Settings - Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (dr == DialogResult.Cancel)
                {
                    // Cancel the form closing procedure
                    e.Cancel = true;
                }
                else if (dr == DialogResult.Yes)
                {
                    // Save changes. If we pressed No, or X, or on any other irregular exit, close without saving
                }
            }
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            updateCheckBox.Text = "Pause updates (last updated " + (DateTime.Now - LastUpdateTime).ToString("mm\\:ss") + " ago)";
        }

        private void textureListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Sort based on the column we've selected, in descending order
            // If we've selected the same column, switch between ascending and descending order
            if (e.Column == 0)
            {
                if (SortType == ColumnSort.Descending_Name)
                {
                    SortType = ColumnSort.Ascending_Name;
                }
                else
                {
                    SortType = ColumnSort.Descending_Name;
                }
            }
            else if (e.Column == 1)
            {
                if (SortType == ColumnSort.Descending_Resolution)
                {
                    SortType = ColumnSort.Ascending_Resolution;
                }
                else
                {
                    SortType = ColumnSort.Descending_Resolution;
                }
            }
            else if (e.Column == 2)
            {
                if (SortType == ColumnSort.Descending_Memory)
                {
                    SortType = ColumnSort.Ascending_Memory;
                }
                else
                {
                    SortType = ColumnSort.Descending_Memory;
                }
            }
            textureListView.Sort();
        }

        private void textureListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Recalculate selected item count and total memory usage and display to the user (hide if no selected items)
            if (textureListView.SelectedItems.Count > 0)
            {
                float memory = .0f;
                foreach (ListViewItem lvi in textureListView.SelectedItems)
                {
                    memory += float.Parse(lvi.SubItems[2].Tag.ToString());
                }

                bool over1mb = memory > 1024.0f;
                selectedItemsLabel.Text = "Total memory of " + textureListView.SelectedItems.Count + " selected items: " + 
                    (over1mb ? (memory / 1048576.0f).ToString("0.00") : (memory / 1024.0f).ToString("0.00")) + (over1mb ? " MB" : " KB") +
                    " (" + ((memory / LoadedTextureMemory) * 100.0f).ToString("0.00") + "% of loaded texture memory)";

                selectedItemsLabel.Show();
            }
            else
            {
                selectedItemsLabel.Hide();
            }
        }

        private void alwaysOnTopCB_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = alwaysOnTopCB.Checked;
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            // Make sure not to capture our click, otherwise we've requested help on the help button
            helpButton.Capture = false;
            SendMessage(Handle, 0x112 /* WM_SYSCOMMAND */, (IntPtr)0xF180 /* SC_CONTEXTHELP */, IntPtr.Zero);
        }
    }
}
