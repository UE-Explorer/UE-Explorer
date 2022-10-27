using System;
using UEExplorer.Development;
using UEExplorer.UI.Main;

namespace Eliot.Extensions.NativesTableListGenerator
{
    [ExtensionTitle("NTL Generator")]
    public class ExtNativeGen : IExtension
    {
        private ProgramForm _Form;

        /// <summary>
        ///     Called after UEExplorer_Form is initialized.
        /// </summary>
        /// <param name="form"></param>
        public void Initialize(ProgramForm form)
        {
            _Form = form;
        }

        /// <summary>
        ///     Called when activated by end-user.
        /// </summary>
        public void OnActivate(object sender, EventArgs e)
        {
            _Form.Tabs.InsertTab(typeof(UC_NativeGenerator), "Natives Table List Generator");
        }
    }
}