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
		private ProgramForm _Owner;

		/// <summary>
		/// Called after UEExplorer_Form is initialized.
		/// </summary>
		/// <param name="form"></param>
		public void Initialize( ProgramForm form )
		{
			_Owner = form;
		}

		/// <summary>
		/// Called when activated by end-user.
		/// </summary>
		public void OnActivate( object sender, EventArgs e )
		{
			_Owner.TManager.AddTabComponent( typeof(UC_ExecGenerator), "Unreal Exec Commands Generator" );
		}
	}
}
