using System;
using UEExplorer.Development;
using UEExplorer.UI.Main;

namespace Eliot.Extensions.ExecGenerator
{
    [ExtensionTitle("Exec Generator")]
    public class ExtExecGen : IExtension
    {
        private ProgramForm _Owner;

        /// <summary>
        ///     Called after UEExplorer_Form is initialized.
        /// </summary>
        /// <param name="form"></param>
        public void Initialize(ProgramForm form)
        {
            _Owner = form;
        }

        /// <summary>
        ///     Called when activated by end-user.
        /// </summary>
        public void OnActivate(object sender, EventArgs e)
        {
            _Owner.Tabs.InsertTab(typeof(UC_ExecGenerator), "Unreal Exec Commands Generator");
        }
    }
}