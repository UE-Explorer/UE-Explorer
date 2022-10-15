using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Eliot.Utilities
{
    public sealed class MRUManager
    {
        public delegate void RefreshEventHandler();

        private const string _StorageFileName = "MRU.xml";

        [XmlIgnore] private static string _StoragePath = string.Empty;

        public readonly List<string> Files = new List<string>();

        private MRUManager()
        {
        }

        [XmlIgnore] public int MaxCount { get; set; } = 15;

        public static void SetStoragePath(string relativePath)
        {
            _StoragePath = Path.Combine(relativePath, _StorageFileName);
        }

        public event RefreshEventHandler RefreshEvent;

        private void OnRefreshEvent()
        {
            RefreshEvent?.Invoke();
        }

        public void AddFile(string path)
        {
            if (File.Exists(path))
            {
                Files.Remove(path);
            }

            Files.Add(path);
            if (Files.Count > MaxCount)
            {
                Files.RemoveAt(0);
            }

            OnRefreshEvent();
        }

        public void RemoveFile(int index)
        {
            Files.Remove(Files[index]);
            OnRefreshEvent();
        }

        public void Save()
        {
            using (var w = new XmlTextWriter(_StoragePath, Encoding.ASCII))
            {
                var xser = new XmlSerializer(typeof(MRUManager));
                xser.Serialize(w, this);
            }
        }

        public static MRUManager Load()
        {
            MRUManager manager;
            if (!File.Exists(_StoragePath))
            {
                manager = new MRUManager();
                manager.Save();
                return manager;
            }

            using (var r = new XmlTextReader(_StoragePath))
            {
                var xser = new XmlSerializer(typeof(MRUManager));
                manager = (MRUManager)xser.Deserialize(r);
            }

            return manager;
        }
    }
}