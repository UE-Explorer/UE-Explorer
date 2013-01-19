using System;
using System.IO;
using System.Windows.Forms;

namespace UEExplorer.UI.Tabs
{
	using Dialogs;

	public class UC_UClassFile : UserControl_Tab
	{
		public string FileName{ get; set; }

		private System.Windows.Forms.Integration.ElementHost _WpfHost;
		private TextEditorPanel _MyTextEditor1;
		private TableLayoutPanel _EditorLayout;

		/*public class UCSyntax : ICSharpCode.AvalonEdit.Highlighting.IHighlightingDefinition
		{
		}*/

		/// <summary>
		/// Called when the Tab is added to the chain.
		/// </summary>
		protected override void TabCreated()
		{
			string langPath = Path.Combine( Application.StartupPath, "Config", "UnrealScript.xshd" );
			if( File.Exists( langPath ) )
			{
				try
				{
					_MyTextEditor1.textEditor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load( 
						new System.Xml.XmlTextReader( langPath ), 
						ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance 
					);
				}
				catch( Exception e )
				{
					ExceptionDialog.Show( e.GetType().Name, e ); 
				}
			}
			base.TabCreated();
		}

		public void PostInitialize()
		{
			_MyTextEditor1.textEditor.IsReadOnly = false;
			_MyTextEditor1.textEditor.Load( FileName );
		}

		protected override void InitializeComponent()
		{
			this._WpfHost = new System.Windows.Forms.Integration.ElementHost();
			this._MyTextEditor1 = new UEExplorer.UI.Tabs.TextEditorPanel();
			this._EditorLayout = new System.Windows.Forms.TableLayoutPanel();
			this._EditorLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// WPFHost
			// 
			this._WpfHost.Dock = System.Windows.Forms.DockStyle.Fill;
			this._WpfHost.Location = new System.Drawing.Point( 3, 20 );
			this._WpfHost.Name = "_WpfHost";
			this._WpfHost.Size = new System.Drawing.Size( 1306, 561 );
			this._WpfHost.TabIndex = 21;
			this._WpfHost.Text = "elementHost1";
			this._WpfHost.Child = this._MyTextEditor1;
			// 
			// EditorLayout
			// 
			this._EditorLayout.ColumnCount = 1;
			this._EditorLayout.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 100F ) );
			this._EditorLayout.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Absolute, 20F ) );
			this._EditorLayout.Controls.Add( this._WpfHost, 0, 1 );
			this._EditorLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._EditorLayout.Location = new System.Drawing.Point( 0, 0 );
			this._EditorLayout.Name = "_EditorLayout";
			this._EditorLayout.RowCount = 2;
			this._EditorLayout.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 2.910959F ) );
			this._EditorLayout.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 97.08904F ) );
			this._EditorLayout.Size = new System.Drawing.Size( 1312, 584 );
			this._EditorLayout.TabIndex = 23;
			// 
			// UC_UClassFile
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.Controls.Add( this._EditorLayout );
			this.Name = "UC_UClassFile";
			this.Size = new System.Drawing.Size( 1312, 584 );
			this._EditorLayout.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		public override void TabSave()
		{
			_MyTextEditor1.textEditor.Save( FileName );
		}

		public override void TabFind()
		{
			using( var findDialog = new FindDialog( _MyTextEditor1 ) )
			{
			    findDialog.ShowDialog();    
			}
		}
	}

	public static class EditorUtil
	{
		public static void FindText( TextEditorPanel editor, string text )
		{
			int fails = 0;

			var currentIndex = editor.textEditor.CaretOffset;
			if( currentIndex >= editor.textEditor.Text.Length )
				return;

			searchAgain:
			int textIndex = editor.textEditor.Text.IndexOf( text, currentIndex, StringComparison.OrdinalIgnoreCase );
			if( textIndex == -1 )
			{
				currentIndex = 0;
				if( fails > 0 )
					return;

				++ fails;
				goto searchAgain;
			}

			var line = editor.textEditor.TextArea.Document.GetLocation( textIndex );
	
			editor.textEditor.ScrollTo( line.Line, line.Column );
			editor.textEditor.Select( textIndex, text.Length );

			editor.textEditor.CaretOffset = textIndex + text.Length;
		}
	}
}
