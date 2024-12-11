using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using System.Threading;
using System;
using UnityEditor.Overlays;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SH
{
    public static class StaticSaveLoad
    {
        public static List<Action> toLoad;
        public static List<Action> toSave;
        //public static Band gameData;

        // save the game data in case it's needed
        public static Dictionary<KeyValuePair<KeyValuePair<int, Type>, Type>, List<AbstractType>> scriptsData;
        // save the modules that need to update the gameData when the gamescene change
        // public static Dictionary<KeyValuePair<KeyValuePair<int, Type>, Type>, AbstractModule> modulesData;
        // all the load and save, will always pass throug here !, and here we comunicate with the static
        public static void AddToQueueLoad<T>(KeyValuePair<int, Type> subscriber, Save saveData, Action<List<T>> toAsign) where T : AbstractType
        {
            if (toLoad == null)
            {
                toLoad = new List<Action>();
            }
            toLoad.Add(() =>
            {
                toAsign(Load<T>(subscriber, saveData));


            });
        }

        public static void DataWasLoaded<T>(KeyValuePair<int, Type> id, List<T> data, Type tp, bool threaded = false) where T : AbstractType
        {
            KeyValuePair<KeyValuePair<int, Type>, Type> key = new KeyValuePair<KeyValuePair<int, Type>, Type>(id, tp);

            if (scriptsData == null)
            {
                scriptsData = new Dictionary<KeyValuePair<KeyValuePair<int, Type>, Type>, List<AbstractType>>();
            }
            if (data != null && !scriptsData.ContainsKey(key))
            {
                scriptsData.Add(key, data.Cast<AbstractType>().ToList());
            }

        }

        // return a list casted by the type
        public static List<T> GetData<T>(KeyValuePair<int, Type> instanceID, Type tp) where T : AbstractType
        {
            if (scriptsData.ContainsKey(new KeyValuePair<KeyValuePair<int, Type>, Type>(instanceID, tp)))
            {
                return scriptsData[new KeyValuePair<KeyValuePair<int, Type>, Type>(instanceID, tp)].Cast<T>().ToList();
            }
            else
            {
                Debug.Log("NOT FOUND !!!");
                return null;
            }
        }

        public static List<T> GetData<T>(KeyValuePair<int, Type> instanceID) where T : AbstractType
        {
            //Debug.Log("type ? " + typeof(T));
            return scriptsData[new KeyValuePair<KeyValuePair<int, Type>, Type>(instanceID, typeof(T))].Cast<T>().ToList();
        }

        public static void AddToQueueSave<T>(KeyValuePair<int, Type> subscriber, List<T> data, Save saveData) where T : AbstractType
        {
            if (toSave == null)
            {
                toSave = new List<Action>();
            }
            toSave.Add(() =>
            {
                Save<T>(subscriber, data, saveData);
            });
        }

        public static bool CheckIfSaveExist(Save saveData)
        {
            string filePath = "";
            string folderToFile = saveData.folder != "" ? Path.Combine(saveData.folder, saveData.fileName) : saveData.fileName;
            // path location
            if (!saveData.useFileNameAsPath)
            {
                if (saveData.onStreamingAssets)
                {
                    filePath = Path.Combine(Application.streamingAssetsPath, folderToFile + ".xml");
                }
                else if (saveData.onResources)
                {
                    filePath = Path.Combine(Path.Combine(Application.dataPath, "Resources"), folderToFile + ".xml");
                }
                else
                {
                    filePath = Path.Combine(Application.persistentDataPath, folderToFile + ".xml");
                }
            }
            else
            {
                if (saveData.ignoreExtension)
                {
                    filePath = folderToFile;
                }
                else
                {
                    filePath = folderToFile + ".xml";
                }

            }

            return File.Exists(filePath);
        }

        public static void Save<T>(List<T> data, Save saveData) where T : AbstractType
        {
            Save<T>(new KeyValuePair<int, Type>(0, typeof(int)), data, saveData);
        }

        public static void Save<T>(KeyValuePair<int, Type> subscriber, List<T> data, Save saveData) where T : AbstractType
        {
            //float timeStart = Time.time;
            string newData = Serialize(data);
            string filePath = "";
            string folderToFile = saveData.folder != "" ? Path.Combine(saveData.folder, saveData.fileName) : saveData.fileName;

            // Encode the data ?
            if (saveData.encode)
            {
                newData = Encode(newData);
            }

            // path location
            if (!saveData.useFileNameAsPath)
            {
                if (saveData.onStreamingAssets)
                {
                    filePath = Path.Combine(Application.streamingAssetsPath, folderToFile + ".xml");
                }
                else if (saveData.onResources)
                {
                    filePath = Path.Combine(Path.Combine(Application.dataPath, "Resources"), folderToFile + ".xml");
                }
                else
                {
                    filePath = Path.Combine(Application.persistentDataPath, folderToFile + ".xml");
                }
            }
            else
            {
                if (saveData.ignoreExtension)
                {
                    filePath = folderToFile;
                }
                else
                {
                    filePath = folderToFile + ".xml";
                }

            }

            // create it if it does not exist
            if (File.Exists(filePath))
            {
                //Debug.Log("create / dispose");
                File.Create(filePath).Dispose();
            }

            // use the serializer to arrange the data so it's not a single line
            // Use a 'using' statement to ensure the file is properly closed after writing
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                using (TextWriter textWriter = new StreamWriter(fs))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                    // Serialize the data to the file
                    serializer.Serialize(textWriter, data);

                    // Flushing is usually unnecessary as 'using' will take care of it, but if you need it:
                    textWriter.Flush();

                    // check FlushAsync() if this is too slow !
                    textWriter.Close();
                }
            }
            //Debug.Log("Finished Time ... " + (timeStart - Time.time));
#if UNITY_EDITOR
            if (saveData.onResources == true)
            {
                AssetDatabase.Refresh();
            }
#endif
        }

        #region Asyn save ...
        public static void SaveAsync<T>(KeyValuePair<int, Type> subscriber, List<T> data, Save saveData) where T : AbstractType
        {
            Debug.Log("saving ");
            //float timeStart = Time.time;
            string filePath = "";
            string folderToFile = saveData.folder != "" ? Path.Combine(saveData.folder, saveData.fileName) : saveData.fileName;

            // Encode the data ?
            if (saveData.encode)
            {
                //newData = Encode(newData);
                //yield return 0;
            }

            // path location
            if (!saveData.useFileNameAsPath)
            {
                if (saveData.onStreamingAssets)
                {
                    filePath = Path.Combine(Application.streamingAssetsPath, folderToFile + ".xml");
                }
                else if (saveData.onResources)
                {
                    filePath = Path.Combine(Path.Combine(Application.dataPath, "Resources"), folderToFile + ".xml");
                }
                else
                {
                    filePath = Path.Combine(Application.persistentDataPath, folderToFile + ".xml");
                }
            }
            else
            {
                if (saveData.ignoreExtension)
                {
                    filePath = folderToFile;
                }
                else
                {
                    filePath = folderToFile + ".xml";
                }

            }
            //yield return 0;

            
            // create it if it does not exist
            if (File.Exists(filePath))
            {
                //Debug.Log("create / dispose");
                File.Create(filePath).Dispose();
            }
            //yield return 0;

            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));

            Stream s = new FileStream(filePath, FileMode.Create, FileAccess.Write,
                    FileShare.None);
            //FileShare.None, 2048, true);
            StreamWriter sr = new StreamWriter(s);

            //serializer.Serialize(s, data);

            TextWriter textWriter = new StreamWriter(s);
            serializer.Serialize(textWriter, data);
            textWriter.Flush();
            textWriter.Close();

            Debug.Log(filePath /*+  "[" + (Time.time - timeStart) + "]"*/);
        }

        #endregion Asyn save ...

        public static int GetNextFolderID(string path, string folderName)
        {
            var directories = Directory.GetDirectories(path);
            int toRemove = folderName.Length;
            int pathRemove = path.Length;
            List<int> values = new List<int>();
            bool found = false;
            int newID = 1;

            for (int i = 0, length = directories.Length; i < length; i++)
            {
                string folder = directories[i].Substring(pathRemove + 1);
                //Debug.Log("Thigns ? [ " + folder + "] " + folderName);
                if (folder.Contains(folderName)) {
                    //Debug.Log("Find value " + folder + " saved ? " + folder.Substring(toRemove));
                    values.Add(int.Parse(folder.Substring(toRemove)));
                }
            }

            while(found == false)
            {
                if (values.Contains(newID))
                {
                    values.Remove(newID);
                    newID++;
                }
                else
                {
                    found = true;
                }
            }

            return newID;
        }

        public static void CheckCreateFolder(string path)
        {
            if (Directory.Exists(path))
            {
                Debug.Log("Path Exist");
            }
            DirectoryInfo di = Directory.CreateDirectory(path);
        }

        public static void RemoveFolder(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public static void RemoveFile(string path)
        {
            string filePath = Path.Combine(Application.persistentDataPath, path + ".xml");
            if (File.Exists(filePath))
            {
                Debug.Log("Found file location, removing it");
                File.Delete(filePath);
            }
        }

        public static void RemoveFile(Save saveData)
        {
            string filePath = "";
            string folderToFile = saveData.folder != "" ? Path.Combine(saveData.folder, saveData.fileName) : saveData.fileName;

            // path location
            if (!saveData.useFileNameAsPath)
            {
                if (saveData.onStreamingAssets)
                {
                    filePath = Path.Combine(Application.streamingAssetsPath, folderToFile + ".xml");
                }
                else if (saveData.onResources)
                {
                    filePath = Path.Combine(Path.Combine(Application.dataPath, "Resources"), folderToFile + ".xml");
                }
                else
                {
                    filePath = Path.Combine(Application.persistentDataPath, folderToFile + ".xml");
                }
            }
            else
            {
                if (saveData.ignoreExtension)
                {
                    filePath = folderToFile;
                }
                else
                {
                    filePath = folderToFile + ".xml";
                }

            }
            File.Delete(filePath);


        }

        

        public static string GetpathBase(Save saveData, bool useFolder = true)
        {
            string filePath = "";

            // path location
            if (!saveData.useFileNameAsPath)
            {
                if (saveData.onStreamingAssets)
                {
                    filePath = Application.streamingAssetsPath;
                }
                else if (saveData.onResources)
                {
                    filePath = Path.Combine(Application.dataPath, "Resources");
                }
                else
                {
                    filePath = Application.persistentDataPath;
                }
            }
            else
            {

            }

            return useFolder ? Path.Combine(filePath, saveData.folder): filePath;
        }

        public static string GetFileBase(Save saveData)
        {
            string filePath = "";
            string folderToFile = saveData.folder != "" ? Path.Combine(saveData.folder, saveData.fileName) : saveData.fileName;

            // path location
            if (!saveData.useFileNameAsPath)
            {
                if (saveData.onStreamingAssets)
                {
                    filePath = Path.Combine(Application.streamingAssetsPath, folderToFile + ".xml");
                }
                else if (saveData.onResources)
                {
                    filePath = Path.Combine(Path.Combine(Application.dataPath, "Resources"), folderToFile + ".xml");
                }
                else
                {
                    filePath = Path.Combine(Application.persistentDataPath, folderToFile + ".xml");
                }
            }
            else
            {
                if (saveData.ignoreExtension)
                {
                    filePath = folderToFile;
                }
                else
                {
                    filePath = folderToFile + ".xml";
                }

            }

            return filePath;
        }

        // easy load from other static files
        public static List<T> Load<T>(Save saveData, bool fromScriptableObject = false) where T : AbstractType
        {
            // Debug.Log(saveData.fileName);
            KeyValuePair<int, Type> justLoad = new KeyValuePair<int, Type>(0, null);
            return Load<T>(justLoad, saveData, fromScriptableObject);
        }

        // loas from xml, resources/persistentData (ours) mods (theirs) 
        // check if exist, and return the data !
        public static List<T> Load<T>(KeyValuePair<int, Type> subscriber, Save saveData, bool fromScriptableObject = false) where T : AbstractType
        {
            string _info = "";
            string folderToFile = saveData.folder != null && saveData.folder != "" ? Path.Combine(saveData.folder, saveData.fileName) : saveData.fileName;
            // Debug.Log("Loading dhis " + folderToFile + ", " + saveData.folder + ", " + saveData.fileName);

            // check if the data already existe
            if (scriptsData != null && subscriber.Value != null && scriptsData.ContainsKey(new KeyValuePair<KeyValuePair<int, Type>, Type>(subscriber, typeof(T))))
            {
                return GetData<T>(subscriber);
            }
            List<T> newdata = new List<T>();
            if (saveData.onStreamingAssets)
            {
                _info = ReadLines(System.IO.Path.Combine(Application.streamingAssetsPath, folderToFile + ".xml"));
                
            }
            else if (saveData.useFileNameAsPath)
            {
                if (saveData.ignoreExtension)
                {
                    _info = ReadLines(folderToFile);
                }
                else
                {
                    _info = ReadLines(folderToFile + ".xml");
                }

            }
            else
            {
                // resource load
                if (saveData.onResources)
                {
                    //Debug.Log("Load file from res ? " + folderToFile);
                    TextAsset txt = (TextAsset)Resources.Load(folderToFile, typeof(TextAsset));
                    if (txt != null)
                    {
                        _info = txt.text;
                    }
                }
                // persisten data load
                else
                {
                    Debug.Log(System.IO.Path.Combine(Application.persistentDataPath, folderToFile + ".xml"));
                    if (System.IO.File.Exists(System.IO.Path.Combine(Application.persistentDataPath, folderToFile + ".xml")))
                    {
                        _info = ReadLines(System.IO.Path.Combine(Application.persistentDataPath, folderToFile + ".xml"));
                    }
                }
            }

            // transform to the info to string
            if (_info.Length > 0)
            {
                if (saveData.encode)
                {
                    _info = Decode(_info);
                }
                newdata = Deserialize<List<T>>(_info);
            }
            else
            {
                //Debug.Log(folderToFile + " : data not found :c, or empty XP");
            }

            // finally we save it on the gameData
            if (saveData.keepItInMemory)
            {
                DataWasLoaded(subscriber, newdata, typeof(T));
            }

            return newdata;
        }

        private static string ReadLines(string filePath)
        {
            string fileContent = "";
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(fs))
                {
                    fileContent = reader.ReadToEnd();
                }
            }
            return fileContent;//File.ReadAllText(location);
        }

        public static string GetFolder(Save saveData)
        {

            string filePath = "";
            string folderToFile = saveData.folder != "" ? Path.Combine(saveData.folder, saveData.fileName) : saveData.fileName;

            // path location
            if (!saveData.useFileNameAsPath)
            {
                if (saveData.onStreamingAssets)
                {
                    filePath = Path.Combine(Application.streamingAssetsPath, folderToFile);
                }
                else if (saveData.onResources)
                {
                    filePath = Path.Combine(Path.Combine(Application.dataPath, "Resources"), folderToFile);
                }
                else
                {
                    filePath = Path.Combine(Application.persistentDataPath, folderToFile);
                }
            }
            else
            {
                filePath = folderToFile;
                Debug.Log(filePath);
            }

            return filePath;
        }

        public static void RemoveFileData(KeyValuePair<int, Type> subscriber, Save saveData)
        {

            //GetID(), new Save(fileName, onResources)){
        
        }

        public static string Encode(string strToEncode)
        {

            byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(strToEncode);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            // http://msdn.microsoft.com/en-us/library/system.security.cryptography.ciphermode.aspx
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return System.Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decode(string strToDecode)
        {

            byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            byte[] toEncryptArray = System.Convert.FromBase64String(strToDecode);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            // http://msdn.microsoft.com/en-us/library/system.security.cryptography.ciphermode.aspx
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }


        // XML Serialization
        public static T Deserialize<T>(string myString)
        {
            XmlSerializer xs;
            xs = new XmlSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(myString));
            return (T)xs.Deserialize(memoryStream);
        }

        public static string Serialize<T>(T myObject)
        {
            string xmlizedString = null;
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xs;
            xs = new XmlSerializer(typeof(T));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            xs.Serialize(xmlTextWriter, myObject);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            xmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
            return xmlizedString;
        }

        // XML Stuff
        public static string UTF8ByteArrayToString(byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        public static byte[] StringToUTF8ByteArray(string pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }

        // reading from streaming assets ?

        // reading any lua file on streaming assets
        public static string LoadLuaStreaming(string fileName, string folder)
        {
            string finalLocation = System.IO.Path.Combine(Application.streamingAssetsPath, folder);
            finalLocation = System.IO.Path.Combine(finalLocation, fileName);
            return ReadLines(finalLocation);
        }

        // find all files in
        public static string[] SearchForMods(string modContainer)
        {
            if (Directory.Exists(modContainer))
            {
                return SearchForFolders(System.IO.Path.Combine(Application.streamingAssetsPath, modContainer));
            }
            return new string[0];
        }

        // find all files names !
        public static string[] SearchForFiles(string fatherFolder)
        {
            return Directory.GetFiles(fatherFolder);
        }

        // find all folders in
        public static string[] SearchForFolders(string fatherFolder)
        {
            return Directory.GetDirectories(fatherFolder);
        }

        // get bytes for images
        public static byte[] GetImage(string filePath)
        {
            return System.IO.File.ReadAllBytes(filePath);
        }

        [System.Serializable]
        public class NameAndDate
        {
            public string fileName;
            public DateTime time;
            public NameAndDate() { }
            public NameAndDate(string fileName, DateTime time)
            {
                this.fileName = fileName;
                this.time = time;
            }
        }

        // get the files with the name X and the extension Y
        public static List<NameAndDate> GetFilesWithName(string fileDirection, string name, string extension)
        {
            string[] files = new string[0];

            List<NameAndDate> filesOrganized = new List<NameAndDate>();
            //List<string> correctFiles = new List<string>();
            //Debug.Log(fileDirection);
            string _info = fileDirection;

            files = SearchForFiles(_info);

            for (int i = 0, len = files.Length; i < len; i++)
            {
                if (files[i].Contains(Path.Combine("", name)) && files[i].EndsWith(extension))
                {
                    Debug.Log("Check info of ..." + files[i]);
                    var fi1 = new FileInfo(files[i]);
                    
                    string[] parts = files[i].Split(Path.DirectorySeparatorChar);
                    string fileName = parts[parts.Length - 1];
                    //correctFiles.Add(fileName.Split('.')[0]);
                    CheckDateAndAdd(filesOrganized, fileName.Split('.')[0], fi1.LastWriteTime);
                }
            }

            //correctFiles = FE.Helpers.ListHelper.GetListOf(filesOrganized, (e) => { return e.fileName; });
            return filesOrganized;
        }

        public static List<NameAndDate> GetFilesWithNameWhithin(Save fileDirection, string name, string extension)
        {
            string[] files = new string[0];
            List<int> correctFiles = new List<int>();

            return GetFilesWithNameWhithin(GetPath(fileDirection), name, extension);
        }

        public static List<NameAndDate> GetFilesWithNameWhithin(string fileDirection, string name, string extension)
        {
            string[] files = new string[0];

            List<NameAndDate> filesOrganized = new List<NameAndDate>();
            //List<string> correctFiles = new List<string>();

            string _info = fileDirection;

            files = SearchForFiles(_info);

            for (int i = 0, len = files.Length; i < len; i++)
            {
                // Debug.Log("Files ... " + files[i]);
                if (files[i].Contains(Path.Combine("", name)) && files[i].EndsWith(extension))
                {
                    //Debug.Log("Check info of ..." + files[i]);
                    var fi1 = new FileInfo(files[i]);

                    string[] parts = files[i].Split(Path.DirectorySeparatorChar);
                    string fileName = parts[parts.Length - 1];
                    //correctFiles.Add(fileName.Split('.')[0]);
                    CheckDateAndAdd(filesOrganized, fileName.Split('.')[0], fi1.LastWriteTime);
                }
            }
            // Debug.Log("files ? " + filesOrganized + ", " + filesOrganized.Count);

            //correctFiles = FE.Helpers.ListHelper.GetListOf(filesOrganized, (e) => { return e.fileName; });
            return filesOrganized;
        }

        // Can be optimized if check from the center to sides !
        public static void CheckDateAndAdd(List<NameAndDate> org, string name, DateTime time)
        {
            bool added = false;
            for (int i = 0, length = org.Count; i < length; i++)
            {
                if(org[i].time < time)
                {
                    added = true;
                    NameAndDate last = org.ElementAt(i);
                    org[i] = new NameAndDate(name, time);
                    org.Add(last);
                    break;
                }
            }
            if(added == false)
            {
                org.Add(new NameAndDate(name, time));
            }
            
        }

        public static List<int> GetFilesNumberWithName(Save fileDirection, string name, string extension)
        {
            string[] files = new string[0];
            List<int> correctFiles = new List<int>();

            string _info = GetPath(fileDirection);

            files = SearchForFiles(_info);

            for (int i = 0, len = files.Length; i < len; i++)
            {
                if (files[i].Contains(Path.Combine("", name)) && files[i].EndsWith(extension))
                {
                    string[] parts = files[i].Split(Path.DirectorySeparatorChar);
                    string folderName = parts[parts.Length - 1].Remove(0, name.Length);
                    int number = 0;

                    folderName = folderName.Split('.')[0];
                    // if there's number
                    int.TryParse(folderName, out number);
                    correctFiles.Add(number);
                }
            }
            return correctFiles;
        }

        public static string GetPath(Save fileDirection)
        {
            string _info = "";
            string folderToFile = fileDirection.folder;

            if (!fileDirection.useFileNameAsPath)
            {
                if (fileDirection.onStreamingAssets)
                {
                    _info = Path.Combine(Application.streamingAssetsPath, folderToFile);
                }
                else if (fileDirection.onResources)
                {
                    _info = Path.Combine(Path.Combine(Application.dataPath, "Resources"), folderToFile);
                }
                else
                {
                    _info = Path.Combine(Application.persistentDataPath, folderToFile);
                }
            }
            return _info;
        }
    }
}

