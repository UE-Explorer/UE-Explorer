using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Eliot.Utilities
{
	public sealed class MRUManager
	{
		[XmlIgnore]
		private static string _StoragePath = String.Empty;
		private const string _StorageFileName = "MRU.xml";
		public readonly List<String> Files = new List<string>();

		private MRUManager()
		{
		}

		public static void SetStoragePath( string relativePath )
		{
			_StoragePath = Path.Combine( relativePath, _StorageFileName );	
		}

		public delegate void RefreshEventHandler();
		public event RefreshEventHandler RefreshEvent = null;
		private void OnRefreshEvent()
		{
			if( RefreshEvent != null )
			{
				RefreshEvent.Invoke();
			}
		}

		public void AddFile( string path )
		{
			if( File.Exists( path ) )
			{
				Files.Remove( path );
			}

			Files.Add( path );	
			OnRefreshEvent();
		}

		public void RemoveFile( int index )
		{
			Files.Remove( Files[index] );	
			OnRefreshEvent();
		}

		public void Save()
		{
			using( var w = new XmlTextWriter( _StoragePath, Encoding.ASCII ) )
			{
				var xser = new XmlSerializer( typeof(MRUManager) );
				xser.Serialize( w, this );
			}
		}

		public static MRUManager Load()
		{
			MRUManager manager;
			if( !File.Exists( _StoragePath ) )
			{
				manager = new MRUManager();
				manager.Save();	
				return manager;
			}

			using( var r = new XmlTextReader( _StoragePath ) )
			{
				var xser = new XmlSerializer( typeof(MRUManager) );
				manager = (MRUManager)xser.Deserialize( r );
			}
			return manager;
		}
	}
}
