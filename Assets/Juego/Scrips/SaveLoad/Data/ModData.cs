using UnityEngine;
using System.Collections.Generic;

namespace SH
{   
    /// <summary>
    /// load the xml structure tied to le lua functions of the mod
    /// modStructure.xml inside a mod
    /// </summary>

    [System.Serializable]
    public class ModData : AbstractData
    {
        // tell as were the mods are on the game and witch ones are active !

        public List<Mod> modList;

        public struct Mod
        {
            public string modName;
            public bool active;
        }

        public override Save mySaveData
        {
            get
            {
                Save newSave = new Save();
                // TODO search for the files and check the active mods!
                // name of the mod and description of the mod
                // folderMod -> modStructure.xml
                // the modStructure tell as were the LUA files and xml that it instanciate are contained
                // it means it will tell were the pawn or other stuff is on the mod !
                newSave.folder = "";
                newSave.fileName = "modStructure";
                newSave.onStreamingAssets = true;
                newSave.encode = encode;
                return newSave;
            }
        }

        /// <summary>
        /// whats gonna be saved !
        /// the mod data and if it's active on the game or save !
        /// </summary>
        [System.Serializable]
        public class ModsOnGame
        {
            public string modName;
            public bool active;
            public string[] dependsOn;
        }

        /// <summary>
        /// Read the structure inside this data class and send it to the mods 
        /// this classes might me accesed as members of the load module !
        /// </summary>
        public override void SaveData()
        {
            //ModsOnGame testingSave = new ModsOnGame();
            //StaticSaveLoad.Save(testingSave, mySaveData);
        }

        public override void LoadData()
        {
            // on the start on the load we check the direction of streaming assets / LUA / modFileName

        }
    }
}