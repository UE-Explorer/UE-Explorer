using System;

namespace UEExplorer.Framework.Commands
{
    public class CommandCategoryAttribute : Attribute
    {
        public string Category { get; }

        public CommandCategoryAttribute(string category)
        {
            Category = category;
        }
    }
}
