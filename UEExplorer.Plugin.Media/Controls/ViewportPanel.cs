using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace UEExplorer.Plugin.Media.Controls
{
    public partial class ViewportPanel : UserControl
    {
        private object _CurrentTarget;

        public ViewportPanel()
        {
            InitializeComponent();
        }

        public object CurrentTarget
        {
            get => _CurrentTarget;
            set
            {
                if (value != null)
                {
                    if (!worldComponent.IsInitialized)
                    {
                        worldComponent.Initialize(canvas.Handle, canvas.Bounds.Width, canvas.Bounds.Height);
                    }
                    worldComponent.AddToScene((dynamic)value);
                    timer.Enabled = true;
                }
                else
                {
                    worldComponent.Dispose();
                    timer.Enabled = false;
                }

                _CurrentTarget = value;
            }
        }

        public object DesiredTarget = null;
        
        private void ViewportPanel_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            if (DesiredTarget != null)
            {
                CurrentTarget = DesiredTarget;
            }
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

        private void ViewportPanel_Resize(object sender, EventArgs e)
        {
            //worldComponent.Resize(canvas.Handle, canvas.Bounds.Width, canvas.Bounds.Height);
            //worldComponent.AddToScene((dynamic)CurrentTarget);
        }
    }
}
