﻿using System;
using System.Windows.Forms;
using System.IO;
using UEExplorer.UI.Tabs;
using UELib.Cache;

namespace UEExplorer
{
    // TODO: Save cache entries.
    [System.Runtime.InteropServices.ComVisible( false )]
    public partial class UC_CacheExtractor : UserControl_Tab
    {
        private UnrealCache _CurCache;

        public UC_CacheExtractor()
        {
            InitializeComponent();
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
                DataGridViewRowEx r = new DataGridViewRowEx{CacheIndex = i};
                r.CreateCells( _CacheData );
                r.SetValues( _CurCache.CacheEntries[i].FileName, _CurCache.CacheEntries[i].Extension, _CurCache.CacheEntries[i].Guid );
                _CacheData.Rows.Add( r );
            }
        }

        private void ExtractToolStripMenuItem1_Click( object sender, EventArgs e )
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

        private void DeleteToolStripMenuItem1_Click( object sender, EventArgs e )
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

        private void RemoveToolStripMenuItem1_Click( object sender, EventArgs e )
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
            _CacheData.Rows.RemoveAt( index );
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

        private void Button1_Click( object sender, EventArgs e )
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

        private void UC_CacheExtractor_Load(object sender, EventArgs e)
        {
            _CacheData.DataSource = null;
            if (Directory.Exists(Program.Options.InitialCachePath))
            {
                LoadCache(Program.Options.InitialCachePath);
            }
        }
    }

    public class DataGridViewRowEx : DataGridViewRow
    {
        public int CacheIndex;
    }
}
