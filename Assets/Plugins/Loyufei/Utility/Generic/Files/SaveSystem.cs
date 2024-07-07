using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei
{
    public interface ISaveable
    {

    }

    [Serializable]
    public enum EUnitySavePath
    {
        None = 0,
        PersistanceDataPath = 1,
        DataPath = 2,
    }

    [Serializable]
    public abstract class SaveSystem
    {
        [SerializeField]
        protected EUnitySavePath _UnitySavePath;
        [SerializeField]
        protected string         _SavePath;
        [SerializeField]
        protected string         _FileName;

        public string SavePath
        {
            get
            {
                var unitySavePath = string.Empty;

                switch (_UnitySavePath)
                {
                    case EUnitySavePath.PersistanceDataPath:
                        unitySavePath = Application.persistentDataPath;
                        break;
                    case EUnitySavePath.DataPath:
                        unitySavePath = Application.dataPath;
                        break;
                    default:
                        unitySavePath = string.Empty;
                        break;
                }

                return Path.Combine(unitySavePath, _SavePath);
            }
        }

        public string FileName => _FileName + ".json";

        public virtual ISaveable Saveable { get; protected set; }
    }

    public static class SaveSystemExtensions 
    {
        public static void Save(this SaveSystem self) 
        {
            if (!Directory.Exists(self.SavePath)) 
            {
                Directory.CreateDirectory(self.SavePath); 
            }

            var json = JsonUtility.ToJson(self.Saveable, true);
            var path = Path.Combine(self.SavePath, self.FileName);

            File.WriteAllText(path, json);
        }

        public static T Load<T>(this SaveSystem self) where T : ISaveable
        {
            var path  = Path.Combine(self.SavePath, self.FileName);
            var exist = File.Exists(path);
            
            if(!exist) { return default; }

            var json = File.ReadAllText(path); 
            var data = JsonUtility.FromJson<T>(json);

            return data;
        }
    }
}
