using System;
using System.Windows.Forms;
using System.IO;
using UELib.Cache;

namespace UEExplorer
{
	// TODO: Save cache entries.
	[System.Runtime.InteropServices.ComVisible( false )]
	public partial class UC_CacheExtractor : UserControl
	{
		internal UnrealCache _CurCache = null;

		public UC_CacheExtractor()
		{
			InitializeComponent();

            _CacheData.DataSource = null;
	
			if( Directory.Exists( Program.Options.InitialCachePath ) )
			{
				LoadCache( Program.Options.InitialCachePath );
			}
		}

		private void Button_SelectDir_Click( object sender, EventArgs e )
		{
			if( CacheFolderDialog.ShowDialog( this ) == DialogResult.OK )
			{
				Program.Options.InitialCachePath = CacheFolderDialog.SelectedPath;
				Program.SaveConfig();

				LoadCache( Program.Options.InitialCachePath );
			}
		}

		private void LoadCache( string cacheDir )
		{
			_CurCache = new UnrealCache( cacheDir );
			try
			{
				_CurCache.LoadCacheEntries();
			}
			catch( CacheException )
			{
				return;
			}
			
			_CacheData.Rows.Clear();
			for( int i = 0; i < _CurCache.CacheEntries.Count; ++ i )
			{
                DataGridViewRowEx r = new DataGridViewRowEx();
				r.CacheIndex = i;
                r.CreateCells( _CacheData );
				r.SetValues( _CurCache.CacheEntries[i].FileName, _CurCache.CacheEntries[i].Extension, _CurCache.CacheEntries[i].Guid );
                _CacheData.Rows.Add( r );
			}
		}

		private void extractToolStripMenuItem1_Click( object sender, EventArgs e )
		{
			if( _CacheData.SelectedRows.Count == 0 )
			{
				return;
			}

			if( CacheFolderDialog.ShowDialog( this ) == DialogResult.OK )
			{
				for( int i = 0; i < _CacheData.SelectedRows.Count; ++ i )
				{
					DataGridViewRowEx r = (DataGridViewRowEx)_CacheData.SelectedRows[i];
					if( _CurCache.ExtractCacheEntry( r.CacheIndex, CacheFolderDialog.SelectedPath ) )
					{
						RemoveRow( r.Index );
						UpdateAllCacheIndex( r.CacheIndex );
					}
				}
			}
		}

		private void deleteToolStripMenuItem1_Click( object sender, EventArgs e )
		{
			for( int i = 0; i < _CacheData.SelectedRows.Count; ++ i )
			{
				DataGridViewRowEx r = (DataGridViewRowEx)_CacheData.SelectedRows[i];
				if( _CurCache.DeleteCacheEntry( r.CacheIndex ) )
				{
					RemoveRow( r.Index );
					UpdateAllCacheIndex( r.CacheIndex );
				}
			}
		}

		private void removeToolStripMenuItem1_Click( object sender, EventArgs e )
		{
			for( int i = 0; i < _CacheData.SelectedRows.Count; ++ i )
			{
				DataGridViewRowEx r = (DataGridViewRowEx)_CacheData.SelectedRows[i];
				if( _CurCache.RemoveCacheEntry( r.CacheIndex ) )
				{
					RemoveRow( r.Index );
					UpdateAllCacheIndex( r.CacheIndex );
				}
			}
		}

		private void RemoveRow( int index )
		{
			_CacheData.ClearSelection();
			_CacheData.Rows.RemoveAt( index );;
		}

		private void UpdateAllCacheIndex( int removedIndex )
		{
			for( int i = 0; i < _CacheData.Rows.Count; ++ i )
			{
				DataGridViewRowEx r = (DataGridViewRowEx)_CacheData.Rows[i];
				if( removedIndex < r.CacheIndex )
				{
					-- r.CacheIndex;
				}
			}
		}

		private void button1_Click( object sender, EventArgs e )
		{
			for( int i = 0; i < _CacheData.Rows.Count; ++ i )
			{
				string filename = (string)_CacheData.Rows[i].Cells[2].Value;
				if( !File.Exists( Path.Combine( Program.Options.InitialCachePath, filename + ".uxx" ) ) )
				{
					RemoveRow( i );
					-- i;
				}
			}	
		}
	}

	public class DataGridViewRowEx : DataGridViewRow
	{
		public int CacheIndex;
	}
}
