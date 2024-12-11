using UnityEngine;
using System.Collections;

namespace SH
{
    [System.Serializable]
    public abstract class AbstractData
    {

        public abstract Save mySaveData { get; }

        public bool onResources;
        public bool encode;

        public abstract void SaveData();
        public abstract void LoadData();
    }

    public class Save : AbstractType
    {
        public string folder;
        public string fileName;
        public bool onResources = false;
        public bool onStreamingAssets = false;
        public bool encode = false;
        public bool useFileNameAsPath = false;
        public bool ignoreExtension = false;
        public bool keepItInMemory = true;

        // force load is used when we load from files, and not from the same loaded scene
        public bool forceLoad = true;

        public Save()
        {
            folder = "";
            fileName = "";
        }

        public Save(string newFilename, bool onResources = false)
        {
            folder = "";
            fileName = newFilename;
            this.onResources = onResources;
        }

        public Save(string newFilename, bool onResources, bool encode)
        {
            folder = "";
            fileName = newFilename;
            this.onResources = onResources;
            this.encode = encode;
        }

        public Save(string newFolder, string newFilename, bool onResources = false)
        {
            folder = newFolder;
            fileName = newFilename;
            this.onResources = onResources;
        }
    }
}