using Cosmos.System.FileSystem;

namespace Clios
{
    public static class Global
    {
        public const string STARTING_PATH = @"0:\";
        public static string CurrentPath = STARTING_PATH;
        public static bool ConsoleEcho = true;
        public static CosmosVFS FileSystem = new CosmosVFS();
    }
}
