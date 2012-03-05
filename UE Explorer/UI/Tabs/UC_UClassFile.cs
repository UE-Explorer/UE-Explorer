using System;
using System.IO;
using System.Windows.Forms;
using UELib;

namespace UEExplorer.UI.Tabs
{
	using UEExplorer.UI.Dialogs;

	public partial class UC_UClassFile : UserControl_Tab, IHasFileName
	{
		public string FileName{ get; set; }

		private System.Windows.Forms.Integration.ElementHost WPFHost;
		private MyTextEditor myTextEditor1;
		private TableLayoutPanel EditorLayout;

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
					myTextEditor1.textEditor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load( 
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
			myTextEditor1.textEditor.IsReadOnly = false;
			myTextEditor1.textEditor.Load( FileName );
		}

		protected override void InitializeComponent()
		{
			this.WPFHost = new System.Windows.Forms.Integration.ElementHost();
			this.myTextEditor1 = new UEExplorer.UI.Tabs.MyTextEditor();
			this.EditorLayout = new System.Windows.Forms.TableLayoutPanel();
			this.EditorLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// WPFHost
			// 
			this.WPFHost.Dock = System.Windows.Forms.DockStyle.Fill;
			this.WPFHost.Location = new System.Drawing.Point( 3, 20 );
			this.WPFHost.Name = "WPFHost";
			this.WPFHost.Size = new System.Drawing.Size( 1306, 561 );
			this.WPFHost.TabIndex = 21;
			this.WPFHost.Text = "elementHost1";
			this.WPFHost.Child = this.myTextEditor1;
			// 
			// EditorLayout
			// 
			this.EditorLayout.ColumnCount = 1;
			this.EditorLayout.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 100F ) );
			this.EditorLayout.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Absolute, 20F ) );
			this.EditorLayout.Controls.Add( this.WPFHost, 0, 1 );
			this.EditorLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.EditorLayout.Location = new System.Drawing.Point( 0, 0 );
			this.EditorLayout.Name = "EditorLayout";
			this.EditorLayout.RowCount = 2;
			this.EditorLayout.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 2.910959F ) );
			this.EditorLayout.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 97.08904F ) );
			this.EditorLayout.Size = new System.Drawing.Size( 1312, 584 );
			this.EditorLayout.TabIndex = 23;
			// 
			// UC_UClassFile
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.Controls.Add( this.EditorLayout );
			this.Name = "UC_UClassFile";
			this.Size = new System.Drawing.Size( 1312, 584 );
			this.EditorLayout.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		public override void TabSave()
		{
			myTextEditor1.textEditor.Save( FileName );
		}

		public override void TabFind()
		{
			new FindDialog( myTextEditor1 ).ShowDialog();
		}
	}

	public static class EditorUtil
	{
		private static int _CurrentIndex;

		public static void FindText( MyTextEditor editor, string text )
		{
			int fails = 0;
			searchAgain:
			int textIndex = editor.textEditor.Text.IndexOf( text, _CurrentIndex );

			if( textIndex == -1 )
			{
				_CurrentIndex = 0;
				if( fails > 0 )
					return;

				++ fails;
				goto searchAgain;
			}
			else
			{
				var line = editor.textEditor.TextArea.Document.GetLocation( textIndex );
	
				editor.textEditor.ScrollTo( line.Line, line.Column );
				editor.textEditor.Select( textIndex, text.Length );

				_CurrentIndex = textIndex + 1;
			}
		}
	}
}
