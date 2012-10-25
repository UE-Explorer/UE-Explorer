using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UEExplorer.UI;
using UEExplorer.UI.Tabs;
using System.ComponentModel;

namespace UEExplorer.Development
{
	public interface IExtension
	{
		/// <summary>
		/// Called after UEExplorer_Form is initialized.
		/// </summary>
		/// <param name="form"></param>
		void Initialize( UEExplorer.UI.ProgramForm form );

		/// <summary>
		/// Called when activated by end-user.
		/// </summary>
		void OnActivate( object sender, EventArgs e );
	}

	public class ExtensionTitleAttribute : Attribute
	{
		public string Title;

		public ExtensionTitleAttribute( string title )
		{
			Title = title;
		}
	}
}
