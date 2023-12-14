#include <urlmon.h>
#include <comdef.h>
#include <time.h>

#include "shared.h"

#include "../resource.h"

extern "C" __declspec(dllexport) void __stdcall PluginStart(void* aOwner);
extern "C" __declspec(dllexport) void __stdcall AccessSystemVariable(unsigned short index, float* value, bool* write);
extern "C" __declspec(dllexport) void __stdcall PluginFinalize();

BOOL APIENTRY DllMain(HMODULE instance, DWORD, LPVOID)
{
	dll_instance = instance;
    return TRUE;
}

bool VersionCheck()
{
	if (!strncmp(reinterpret_cast<char*>(Offsets::version_check_address), Offsets::version_check_string, Offsets::version_check_length))
	{
		Log(LT_INFO, "Detected version " OMSI_VERSION);
		return true;
	}

	Error("This version of OMSITextureManager does not support this version of OMSI 2.");
	return false;
}

bool OplCheck()
{
	// OMSITextureManager.opl in this project is linked to OMSITextureManager.rc and its resource will be used for the consistency check
	HRSRC resource = FindResource(dll_instance, MAKEINTRESOURCE(IDR_OPL1), "OPL");
	HGLOBAL global = LoadResource(dll_instance, resource);
	LPVOID pointer = LockResource(global);

	char* opl = reinterpret_cast<char*>(pointer);
	int length = SizeofResource(dll_instance, resource);

	FILE* file = nullptr;
	fopen_s(&file, "plugins/OMSITextureManager.opl", "rb");

	if (!file)
	{
		char error[64];
		strerror_s(error, errno);
		Error("Failed to start a consistency check on OMSITextureManager.opl - error opening file. OMSITextureManager cannot continue.\n\nError code %d: %s", errno, error);
		return false;
	}

	// Calculate the size of the OPL in our OMSI/plugins folder
	fseek(file, 0, SEEK_END);
	int offset = ftell(file);
	fseek(file, 0, SEEK_SET);

	Log(LT_INFO, "OPL length: expected %d, got %d", length, offset);

	if (offset == length)
	{
		char* contents = new char[length];
		fread_s(contents, length, 1, length, file);

		// OPL files are of identical lengths and their contents match
		if (!strncmp(opl, contents, length))
		{
			Log(LT_INFO, "OPL consistency check was successful");

			fclose(file);
			delete[] contents;
			return true;
		}

		Log(LT_WARN, "OPL file contents do not match");
		delete[] contents;
	}
	fclose(file);

	// Consistency check failed. Try to revert the OPL file to its defaults from our resource
	fopen_s(&file, "plugins/OMSITextureManager.opl", "wb");
	if (!file)
	{
		char error[64];
		strerror_s(error, errno);
		Error("Inconsistencies in the OMSITextureManager.opl file have been detected. OMSITextureManager cannot continue.\n\n"
			"Failed to revert the file's contents to their defaults - error opening file.\n\nError code %d: %s", errno, error);
		return false;
	}

	fwrite(opl, 1, length, file);
	fclose(file);

	Error("Inconsistencies in the OMSITextureManager.opl file have been detected. OMSITextureManager cannot continue.\n\n"
		"The file's contents have been reverted to their defaults. OMSI 2 must be restarted for these changes to take effect.");
	return false;
}

void UpdateCheck()
{
	Log(LT_INFO, "Checking for updates...");

	/*
	// HACK: Append current unix time as an unused query string to avoid caching
	char url[128] = { 0 };
	sprintf_s(url, 128, "https://api.github.com/repos/brokenphilip/OMSITextureManager/tags?%lld", time(NULL));
	*/

	// Attempt to connect
	IStream* stream = nullptr;
	HRESULT result = URLOpenBlockingStreamA(0, "https://api.github.com/repos/brokenphilip/OMSITextureManager/tags", &stream, 0, 0);
	if (FAILED(result))
	{
		Log(LT_WARN, "Connection failed with HRESULT %lX: %s", result, _com_error(result).ErrorMessage());
		return;
	}

	// Read first 32 bytes from the result, we don't need any more
	char buffer[32] = { 0 };
	stream->Read(buffer, 31, nullptr);
	stream->Release();

	// Parse the response using tokens
	auto seps = "\"";
	char* next_token = nullptr;
	char* token = strtok_s(buffer, seps, &next_token);

	for (int i = 0; token != nullptr && i < 3; i++)
	{
		token = strtok_s(NULL, seps, &next_token);

		// First should just be "name"
		if (i == 0 && strncmp(token, "name", 4))
		{
			break;
		}

		// Third should be the latest released version
		if (i == 2)
		{
			Log(LT_INFO, "Version check complete, latest version is %s", token);

			constexpr size_t len = std::char_traits<char>::length(PROJECT_VERSION);
			size_t len2 = strlen(token);

			// If versions do not match, prompt to be taken to the OMSITextureManager page
			if (strncmp(token, PROJECT_VERSION, ((len > len2) ? len : len2)))
			{
				char msg[256];
				sprintf_s(msg, 256, "An update to OMSITextureManager is available!\n\nYour version: " PROJECT_VERSION "\nLatest version: %s\n\n"
					"Would you like to go to the OMSITextureManager page now?", token);

				if (MessageBoxA(NULL, msg, "OMSITextureManager " PROJECT_VERSION, MB_ICONINFORMATION | MB_YESNO) == IDYES)
				{
					ShellExecuteA(NULL, NULL, "https://github.com/brokenphilip/OMSITextureManager", NULL, NULL, SW_SHOW);
				}
			}

			return;
		}
	}

	Log(LT_WARN, "Version check failed to parse the response");
}

void LoadSettings()
{

}

void __stdcall PluginStart(void* aOwner)
{
	bool console_exists = false;

	debug = strstr(GetCommandLineA(), "-omsitexman_debug");
	if (debug)
	{
		FILE* console;
		if (!AllocConsole())
		{
			console_exists = true;
		}
		SetConsoleTitle("OMSITextureManager " PROJECT_VERSION " - Debug");

		freopen_s(&console, "CONIN$", "r", stdin);
		freopen_s(&console, "CONOUT$", "w", stdout);
		freopen_s(&console, "CONOUT$", "w", stderr);

		HANDLE handle = GetStdHandle(STD_OUTPUT_HANDLE);

		DWORD mode;
		GetConsoleMode(handle, &mode);
		SetConsoleMode(handle, mode | ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN);
	}

	Log(LT_INFO, "Plugin has started (%sversion " PROJECT_VERSION ")", (debug ? "in DEBUG mode, " : ""));
	if (console_exists)
	{
		Log(LT_WARN, "OMSITextureManager has detected that there is already a console allocated for this program. "
			"Please note that this may disrupt console logging output - consider disabling other plugins which use the console.");
	}

	if (!VersionCheck())
	{
		Log(LT_WARN, "Version check failed. OMSITextureManager will remain dormant");
		return;
	}

	if (!OplCheck())
	{
		Log(LT_WARN, "OPL consistency check failed. OMSITextureManager will remain dormant");
		return;
	}

	if (strstr(GetCommandLineA(), "-omsitexman_noupdate"))
	{
		Log(LT_WARN, "Skipping update check due to launch option. "
			"It is highly recommended you stay up-to-date, so please only use this option if you're having issues");
	}
	else
	{
		UpdateCheck();
	}

	Log(LT_INFO, "Loading settings...");
	LoadSettings();
	loaded = true;
}

void __stdcall AccessSystemVariable(unsigned short index, float* value, bool* write)
{
	if (loaded)
	{
		// Deltatime (Timegap) sum
		static float dt = .0f;
		dt += *value;
		if (dt > settings.interval)
		{
			dt = .0f;

			static bool settings_open = false;

			// Check if settings are open by searching for the mutex
			HANDLE mutex = CreateMutexW(0, true, L"Global\\OMSITexManSettings");
			DWORD gle = GetLastError();

			// If it already exists, OMSITextureManager Settings is likely running
			if (gle == ERROR_ALREADY_EXISTS)
			{
				// 'mutex' could be 0 - no it can't, not under ERROR_ALREADY_EXISTS
				// Adding a check here anyways to skip the annoying warning
				if (mutex)
				{
					// To prevent a "zombie mutex", we MUST close its handle in this case (but NOT release the mutex itself!!!)
					CloseHandle(mutex);
				}

				constexpr int a = offsetof(SharedTextureInfo, first_texman);

				size_t textures1_count = ListLength(Read<uintptr_t>(Read<uintptr_t>(Offsets::TTextureMan1) + 0x8));
				size_t textures2_count = ListLength(Read<uintptr_t>(Read<uintptr_t>(Offsets::TTextureMan2) + 0x8));
				size_t count = textures1_count + textures2_count;

				SharedTextureInfo* stis = new SharedTextureInfo[count];
				for (int i = 0; i < count; i++)
				{
					TTextureItem* tex = (i < textures1_count) ?
						Read<TTextureItem*>(Read<uintptr_t>(Offsets::TTextureMan1) + 0x8) + i :
						Read<TTextureItem*>(Read<uintptr_t>(Offsets::TTextureMan2) + 0x8) + i - textures1_count;

					stis[i].first_texman = (i < textures1_count);
					stis[i].loaded = tex->loaded;
					stis[i].memory = (float)tex->mem;
					stis[i].width = tex->size_x;
					stis[i].height = tex->size_y;
					if (tex->path)
					{
						memcpy(stis[i].path, tex->path, strlen(tex->path));
					}
					else
					{
						memcpy(stis[i].path, "(null)", 7);
					}
				}

				COPYDATASTRUCT cds;
				cds.dwData = (ULONG_PTR)0x6D656F77 /* meow :3 */;
				cds.cbData = sizeof(SharedTextureInfo) * count;
				cds.lpData = stis;

				HWND window = FindWindowW(0, L"OMSITextureManager Settings");
				if (window)
				{
					// NOTE: WM_COPYDATA messages can't be "Post"ed (bless your soul Raymond Chen :pray:)
					SendMessageA(window, WM_COPYDATA, (WPARAM)window, (LPARAM)&cds);
					settings_open = true;
				}
				else
				{
					// Not changing settings_open to false here, because we want to let the game update the settings if the window was open
					// The mutex dies with the window, so this state should be VERY rare - if you see it often, something has gone horribly wrong
					Log(LT_WARN, "Sending game information failed - found mutex but couldn't find settings window. "
						"Make sure OMSITextureManager Settings is running properly, OR, if not in use, make sure it's fully closed");
				}

				return;
			}
			else if (gle == ERROR_ACCESS_DENIED)
			{
				Log(LT_WARN, "Sending game information aborted - access denied trying to create mutex. "
					"Make sure you're NOT running OMSITextureManager Settings as an administrator");
			}

			// We ended up creating a mutex, release it immediately, otherwise OMSITextureManager Settings will not be able to start
			if (mutex)
			{
				ReleaseMutex(mutex);
				CloseHandle(mutex);
			}

			// Window has now been closed, reload the settings
			if (settings_open)
			{
				settings_open = false;
				Log(LT_INFO, "OMSITextureManager Settings window has been closed, reloading settings...");
				LoadSettings();
			}
		}
	}

	/*
	static bool print = false;

	if (*value > .0f && !print)
	{
		print = true;
		int i = 0;

		Log(LT_PRINT, "------------------------------------------------------------------"
			"------------------------------------------------------------------"
			"------------------------------");

		Log(LT_PRINT, "%-128s %-5s %-5s %-4s %-16s", "Filename", "W", "H", "Load", "Memory");

		Log(LT_PRINT, "------------------------------------------------------------------"
			"------------------------------------------------------------------"
			"------------------------------");

		size_t textures1_count = ListLength(Read<uintptr_t>(Read<uintptr_t>(Offsets::TTextureMan1) + 0x8));
		TTextureItem* textures1 = new TTextureItem[textures1_count];
		memcpy(textures1, Read<void*>(Read<uintptr_t>(Offsets::TTextureMan1) + 0x8), textures1_count * sizeof(TTextureItem));
		qsort(textures1, textures1_count, sizeof(TTextureItem), CompareMemory);

		for (i = 0; i < textures1_count; i++)
		{
			bool over1mb = textures1[i].mem >= 1048576.0;
			float mem = over1mb ? (float)textures1[i].mem / 1048576.0 : (float)textures1[i].mem / 1024.0;
			Log(LT_PRINT, "%-128s %-5u %-5u %-4s %-13.02f %s", textures1[i].path, textures1[i].size_x, textures1[i].size_y,
				(textures1[i].loaded ? "yes" : ""), mem, over1mb ? "MB" : "KB");
		}

		Log(LT_PRINT, "------------------------------------------------------------------"
			"------------------------------------------------------------------"
			"------------------------------");

		size_t textures2_count = ListLength(Read<uintptr_t>(Read<uintptr_t>(Offsets::TTextureMan2) + 0x8));
		TTextureItem* textures2 = new TTextureItem[textures2_count];
		memcpy(textures2, Read<void*>(Read<uintptr_t>(Offsets::TTextureMan2) + 0x8), textures2_count * sizeof(TTextureItem));
		qsort(textures2, textures2_count, sizeof(TTextureItem), CompareMemory);

		for (i = 0; i < textures2_count; i++)
		{
			bool over1mb = textures2[i].mem >= 1048576.0;
			float mem = over1mb ? (float)textures2[i].mem / 1048576.0 : (float)textures2[i].mem / 1024.0;
			Log(LT_PRINT, "%-128s %-5u %-5u %-4s %-13.02f %s", textures2[i].path, textures2[i].size_x, textures2[i].size_y,
				(textures2[i].loaded ? "yes" : ""), mem, over1mb ? "MB" : "KB");
		}

		Log(LT_PRINT, "------------------------------------------------------------------"
			"------------------------------------------------------------------"
			"------------------------------");

		float tex_mem_mb = Read<float>(Offsets::TextureMemoryKB) / 1048576.0;
		Log(LT_PRINT, "Total texture memory usage: %.02f MB - Texture counts: TTM1 = %d, TTM2 = %d", tex_mem_mb, textures1_count, textures2_count);

		Log(LT_PRINT, "------------------------------------------------------------------"
			"------------------------------------------------------------------"
			"------------------------------");
	}
	else if (*value == .0f && print)
	{
		print = false;
	}
	*/

	/*
	if (open_menu)
	{
		open_menu = false;
	}

	static float dt = .0f;
	dt += *value;
	if (dt > 5.0f)
	{
		dt = .0f;

		float tex_mem_mb = Read<float>(Offsets::TextureMemoryKB) / 1048576.0;
		Log(LT_PRINT, "TexMem: %.02f - TTexMan1: %d - TTexMan2: %d", tex_mem_mb,
			ListLength(Read<uintptr_t>(Read<uintptr_t>(Offsets::TTextureMan1) + 0x8)),
			ListLength(Read<uintptr_t>(Read<uintptr_t>(Offsets::TTextureMan2) + 0x8)));

		Log(LT_PRINT, "%-128s %-5s %-5s %-4s %-16s", "Filename", "W", "H", "Load", "Memory (MB)");
		Log(LT_PRINT, "------------------------------------------------------------------"
			"------------------------------------------------------------------"
			"------------------------------");

		TTextureItem* tex;
		int i;
		for (i = 0; i < ListLength(Read<uintptr_t>(Read<uintptr_t>(Offsets::TTextureMan1) + 0x8)); i++)
		{
			tex = Read<TTextureItem*>(Read<uintptr_t>(Offsets::TTextureMan1) + 0x8) + i;

			float mem = (float)tex->mem / 1048576.0;
			Log(LT_PRINT, "%-128s %-5u %-5u %-4s %-16.02f", tex->path, tex->size_x, tex->size_y, (tex->loaded ? "yes" : ""), mem);

			//Log(LT_PRINT, "----------------------------------------------------------------\n"
			//	"uint16_t size_x = %hu\n"
			//	"uint16_t size_y = %hu\n"
			//	"double mem = %f\n"
			//	"int datasize = %d\n"
			//	"bool dataready = %i\n"
			//	"void* Texture_ID3DT9 = 0x%08X\n"
			//	"void* oldTexture_ID3DT9 = 0x%08X\n"
			//	"char* path = %s\n"
			//	"char* justfilename = %s\n"
			//	"char* loadpath = %s\n"
			//	"char loaded = %i\n"
			//	"char load_request = %i\n"
			//	"bool managed = %i\n"
			//	"unsigned int failed = %u\n"
			//	"uint16_t used = %hu\n"
			//	"uint16_t used_highres = %hu\n"
			//	"bool threadloading = %i\n"
			//	"bool hasspecials = %i\n"
			//	"bool no_unload = %i\n"
			//	"bool onlyalpha = %i\n"
			//	"int NightMap = %d\n"
			//	"int WinterSnowMap = %d\n"
			//	"int WinterSnowfallMap = %d\n"
			//	"int FallMap = %d\n"
			//	"int SpringMap = %d\n"
			//	"int WinterMap = %d\n"
			//	"int SummerDryMap = %d\n"
			//	"int SurfMap = %d\n"
			//	"bool moisture = %i\n"
			//	"bool puddles = %i\n"
			//	"bool moisture_ic = %i\n"
			//	"bool puddles_ic = %i\n"
			//	"char surface = %i\n"
			//	"char surface_ic = %i\n"
			//	"bool terrainmapping = %i\n"
			//	"bool terrainmapping_alpha = %i\n",
			//	tex->size_x,
			//	tex->size_y,
			//	mem,
			//	tex->datasize,
			//	tex->dataready,
			//	tex->Texture_ID3DT9,
			//	tex->oldTexture_ID3DT9,
			//	tex->path,
			//	tex->justfilename,
			//	tex->loadpath,
			//	tex->loaded,
			//	tex->load_request,
			//	tex->managed,
			//	tex->failed,
			//	tex->used,
			//	tex->used_highres,
			//	tex->threadloading,
			//	tex->hasspecials,
			//	tex->no_unload,
			//	tex->onlyalpha,
			//	tex->NightMap,
			//	tex->WinterSnowMap,
			//	tex->WinterSnowfallMap,
			//	tex->FallMap,
			//	tex->SpringMap,
			//	tex->WinterMap,
			//	tex->SummerDryMap,
			//	tex->SurfMap,
			//	tex->moisture,
			//	tex->puddles,
			//	tex->moisture_ic,
			//	tex->puddles_ic,
			//	tex->surface,
			//	tex->surface_ic,
			//	tex->terrainmapping,
			//	tex->terrainmapping_alpha);
		}

		Log(LT_PRINT, "------------------------------------------------------------------"
			"------------------------------------------------------------------"
			"------------------------------");

		for (i = 0; i < ListLength(Read<uintptr_t>(Read<uintptr_t>(Offsets::TTextureMan2) + 0x8)); i++)
		{
			tex = Read<TTextureItem*>(Read<uintptr_t>(Offsets::TTextureMan2) + 0x8) + i;

			float mem = (float)tex->mem / 1048576.0;
			Log(LT_PRINT, "%-128s %-5u %-5u %-4s %-16.02f", tex->path, tex->size_x, tex->size_y, (tex->loaded ? "yes" : ""), mem);
		}
		
	}
	*/
}

void __stdcall PluginFinalize()
{
	loaded = false;
	Log(LT_INFO, "Plugin finalized");
}

/* === Utility === */

void Log(LogType log_t, const char* message, ...)
{
	char buffer[1024];
	va_list va;
	va_start(va, message);
	vsprintf_s(buffer, 1024, message, va);

	if (debug)
	{
		char tag[15] = { 0 };
		switch (log_t)
		{
		case LT_FATAL:
		case LT_ERROR: sprintf_s(tag, 15, "\x1B[91mERROR\x1B[0m"); break;

		case LT_WARN: sprintf_s(tag, 15, "\x1B[93m WARN\x1B[0m"); break;

			/* LT_PRINT, LT_INFO */
		default: sprintf_s(tag, 15, "\x1B[97m INFO\x1B[0m"); break;
		}

		SYSTEMTIME time;
		GetLocalTime(&time);
		printf("[%02d:%02d:%02d %s] %s\n", time.wHour, time.wMinute, time.wSecond, tag, buffer);
	}

	wchar_t* log = UnicodeString(L"[OMSITextureManager] %hs", buffer).string;

	__asm
	{
		mov     eax, log
		mov     dl, log_t
		call    Offsets::AddLogEntry
	}
}

void Error(const char* message, ...)
{
	char buffer[1024];
	va_list va;
	va_start(va, message);
	vsprintf_s(buffer, 1024, message, va);

	//Log(LT_ERROR, "MBox: %s", buffer);

	MessageBoxA(NULL, buffer, "OMSITextureManager " PROJECT_VERSION, MB_ICONERROR);
}

inline int ListLength(uintptr_t list)
{
	return Read<int>(list - 4);
}