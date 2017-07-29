using System.Collections.Generic;
using System.IO;

namespace Clios.Helpers
{
    public class FileSystemHelper
    {
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public void CreateFile(string path)
        {
            File.Create(path);
        }

        public void DeleteDirectory(DirectoryEntry de)
        {
            Directory.Delete(de.mFullPath);
        }

        public DirectoryEntry GetDirectory(string path)
        {
            DirectoryEntry dr = null;
            if (Directory.Exists(path))
            {
                dr = new DirectoryEntry();
            }
            return dr;
        }

        public List<DirectoryEntry> GetDirectoryListing(string path)
        {
            List<DirectoryEntry> listing = new List<DirectoryEntry>();
            
            IEnumerable<string> files = Directory.EnumerateFiles(Global.CurrentPath);
            foreach(string f in files)
            {
                string p = Path.Combine(path, f);
                long size = new System.IO.FileInfo(p).Length;
                listing.Add(new DirectoryEntry { mFullPath = p, mName = f, mSize = size, mEntryType = DirectoryEntryTypeEnum.File });
            }

            IEnumerable<string> dirs = Directory.EnumerateDirectories(Global.CurrentPath);
            foreach (string d in dirs)
            {
                string p = Path.Combine(path, d);
                listing.Add(new DirectoryEntry { mFullPath = p, mName = d, mSize = 0, mEntryType = DirectoryEntryTypeEnum.Directory });
            }

            return listing;
        }
    }
}
