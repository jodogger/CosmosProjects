using System;
using System.IO;

namespace Clios.Helpers
{
    public enum DirectoryEntryTypeEnum
    {
        Directory, File, Unknown
    }

    public class DirectoryEntry
    {
        public long mSize;
        public string mFullPath;
        public string mName;
        public readonly DirectoryEntry mParent;
        public DirectoryEntryTypeEnum mEntryType;

        public DirectoryEntry()
        {
        }

        public DirectoryEntry(DirectoryEntry aParent, string aFullPath, string aName, long aSize, DirectoryEntryTypeEnum aEntryType)
        {
            if (string.IsNullOrEmpty(aFullPath))
            {
                throw new ArgumentException("Argument is null or empty", nameof(aFullPath));
            }
            if (string.IsNullOrEmpty(aName))
            {
                throw new ArgumentException("Argument is null or empty", nameof(aName));
            }

            mParent = aParent;
            mEntryType = aEntryType;
            mName = aName;
            mSize = aSize;
            mFullPath = aFullPath;
        }
    }
}
