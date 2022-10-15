using System;
using UEExplorer.UI.Main;
using UELib.Annotations;

namespace UEExplorer.Development
{
    public interface IExtension
    {
        /// <summary>
        /// Called after main form is initialized.
        /// </summary>
        /// <param name="form"></param>
        void Initialize(ProgramForm form);

        /// <summary>
        /// Called when activated by end-user.
        /// </summary>
        void OnActivate(object sender, EventArgs e);
    }

    [MeansImplicitUse]
    public class ExtensionTitleAttribute : Attribute
    {
        public readonly string Title;

        public ExtensionTitleAttribute(string title)
        {
            Title = title;
        }
    }
}