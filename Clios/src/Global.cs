using Clios.Helpers;

namespace Clios
{
    public static class Global
    {
#if COSMOS
        public const string STARTING_PATH = @"0:\";
#else
        public const string STARTING_PATH = @"f:\Clios\";
#endif    
        public static string CurrentPath = STARTING_PATH;
        public static bool ConsoleEcho = true;
#if COSMOS
        public static CosmosVFS FileSystem = new CosmosVFS();
#else
        public static FileSystemHelper FileSystem = new FileSystemHelper();
#endif
    }
}
