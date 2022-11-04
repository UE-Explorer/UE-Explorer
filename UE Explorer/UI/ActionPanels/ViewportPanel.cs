using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace UEExplorer.UI.ActionPanels
{
    public partial class ViewportPanel : UserControl
    {
        public ViewportPanel()
        {
            InitializeComponent();
        }

        private object CurrentTarget { get; set; }

        private void ViewportPanel_Load(object sender, EventArgs e)
        {
            //worldComponent.Initialize(canvas.Handle, canvas.Bounds.Width, canvas.Bounds.Height);
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            worldComponent.Resize(canvas.Handle, canvas.Bounds.Width, canvas.Bounds.Height);
            worldComponent.AddToScene((dynamic)CurrentTarget);

            timer.Enabled = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Debug.Assert(worldComponent.IsInitialized);
            worldComponent.Update();
        }

        public void SetRenderTarget(object target)
        {
            CurrentTarget = target;
        }

        private void ViewportPanel_Resize(object sender, EventArgs e)
        {
            //worldComponent.Resize(canvas.Handle, canvas.Bounds.Width, canvas.Bounds.Height);
            //worldComponent.AddToScene((dynamic)CurrentTarget);
        }
    }
}