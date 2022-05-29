using System;

namespace UEExplorer.Development
{
    using UI;

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

    public class ExtensionTitleAttribute : Attribute
    {
        public readonly string Title;

        public ExtensionTitleAttribute(string title)
        {
            Title = title;
        }
    }
}