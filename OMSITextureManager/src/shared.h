#pragma once
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

#include <string>

/* === Main project stuff ===*/

#define PROJECT_VERSION "0.1"

/* === Shared data === */

inline HMODULE dll_instance;
inline bool debug = false;
inline bool loaded = false;

/* === Utility === */

enum LogType : char
{
	LT_PRINT = 0x0, // Prefixless log entries. Only written to logfile.txt if OMSI is started with "-logall". Not used by OMSITextureManager
	LT_INFO = 0x1, // "Information" log entries. Used by OMSITextureManager to log its standard procedures
	LT_WARN = 0x2, // "Warning" log entries. Used by OMSITextureManager to log generally unwanted (but not critical) states
	LT_ERROR = 0x3, // "Error" log entries. Used by OMSITextureManager to log illegal states, some of which unrecoverable
	LT_FATAL = 0x4, // "Fatal Error" log entries. Shows a "OMSI will be closed" message box. Not used by OMSITextureManager
};

void Log(LogType type, const char* message, ...);
void Error(const char* message, ...);

template <typename T>
inline T Read(uintptr_t address)
{
	return *reinterpret_cast<T*>(address);
}

int ListLength(uintptr_t list);

inline struct
{
	float interval = 10.0f;
} settings;

// !!! Make sure this struct PERFECTLY matches its C# counterpart !!!
struct SharedTextureInfo
{
	uint16_t width;
	uint16_t height;
	float memory;
	char path[MAX_PATH] {0};
	bool loaded;
	bool first_texman;
};

struct UnicodeString
{
	uint16_t codepage;
	uint16_t unk_1;
	uint32_t unk_2;
	uint32_t length;
	wchar_t string[1024];

	UnicodeString(const wchar_t* message, ...)
	{
		va_list va;
		va_start(va, message);
		vswprintf_s(string, 1024, message, va);

		codepage = 0x04B0;
		unk_1 = 0x0002;
		unk_2 = 0xFFFFFFFF;
		length = std::char_traits<wchar_t>::length(string);
	}
};

/* === Game structs === */

struct TTextureItem
{
	uint16_t size_x;
	uint16_t size_y;
	double mem;
	int datasize;
	bool dataready;
	void* Texture_ID3DT9;
	void* oldTexture_ID3DT9;
	char* path;
	char* justfilename;
	char* loadpath;
	char loaded;
	char load_request;
	bool managed;
	unsigned int failed;
	uint16_t used;
	uint16_t used_highres;
	bool threadloading;
	bool hasspecials;
	bool no_unload;
	bool onlyalpha;
	int NightMap;
	int WinterSnowMap;
	int WinterSnowfallMap;
	int FallMap;
	int SpringMap;
	int WinterMap;
	int SummerDryMap;
	int SurfMap;
	bool moisture;
	bool puddles;
	bool moisture_ic;
	bool puddles_ic;
	char surface;
	char surface_ic;
	bool terrainmapping;
	bool terrainmapping_alpha;
};

/* === Offsets === */

#define OMSI_VERSION "2.3.004 - Latest Steam version"
namespace Offsets
{
	// String and address of the string that will be used in the version check. Length is automatically calculated at compile time
	constexpr uintptr_t version_check_address = 0x8BBEE3;
	constexpr const char* const version_check_string = ":33333333";
	constexpr size_t version_check_length = std::char_traits<char>::length(version_check_string);

	constexpr uintptr_t TTextureMan[2] = { 0x861BC4, 0x861BC8 };

	// Location of the function AddLogEntry (custom name, xref " - Fatal Error:       " to find it)
	constexpr uintptr_t AddLogEntry = 0x8022C0;
}