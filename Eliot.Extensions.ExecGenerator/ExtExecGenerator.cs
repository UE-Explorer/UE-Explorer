using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eliot.Extensions.ExecGenerator
{
	using UEExplorer.Development;
	using UEExplorer.UI;

	[ExtensionTitle( "Exec Generator" )]
	public class ExtExecGen : IExtension
	{
		private UEExplorer_Form _Owner;

		/// <summary>
		/// Called after UEExplorer_Form is initialized.
		/// </summary>
		/// <param name="form"></param>
		public void Initialize( UEExplorer_Form form )
		{
			_Owner = form;
		}

		/// <summary>
		/// Called when activated by end-user.
		/// </summary>
		public void OnActivate( object sender, EventArgs e )
		{
			_Owner.AddTabComponent( typeof(UC_ExecGenerator), "Unreal Exec Commands Generator" );
		}
	}
}
