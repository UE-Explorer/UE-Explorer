using System;
using System.Windows.Forms;
using Krypton.Navigator;

namespace UEExplorer.Framework.UI.Pages
{
    public static class PageFactory
    {
        public static T CreateTrackingPage<T>(bool isTracking, string uniqueName = null)
            where T : KryptonPage, ITrackingContext =>
            CreateTrackingPage<T>(isTracking, uniqueName, typeof(T));

        public static T CreateTrackingPage<T>(bool isTracking, string uniqueName, Type pageType)
            where T : KryptonPage, ITrackingContext
        {
            //var header = new PageHeader();
            //header.ClientSize = new Size(100, 16);
            //header.Dock = DockStyle.Top;

            var page = (T)Activator.CreateInstance(pageType);
            page.IsTracking = isTracking;
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden);
            if (!string.IsNullOrEmpty(uniqueName))
            {
                page.UniqueName = uniqueName;
            }

            //page.Controls.AddRange(new Control[]{
            //    header
            //});

            return page;
        }

        public static KryptonPage CreatePage<T>(string text, string uniqueName)
            where T : Control, new()
        {
            var content = new T();
            return CreatePage(text, uniqueName, content);
        }

        public static KryptonPage CreatePage<T>(string text, string uniqueName, T content)
            where T : Control
        {
            content.Dock = DockStyle.Fill;

            var page = new Page(text, uniqueName, content.MinimumSize);
            page.ClientSize = content.ClientSize;
            page.Controls.Add(content);
            return page;
        }
    }
}
