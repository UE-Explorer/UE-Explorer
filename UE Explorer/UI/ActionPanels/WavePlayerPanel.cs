using UEExplorer.UI.Tabs;
using UELib.Core;

namespace UEExplorer.UI.ActionPanels
{
    public partial class WavePlayerPanel : ActionPanel, IActionPanel<object>
    {
        public WavePlayerPanel()
        {
            InitializeComponent();
        }

        public void RestoreState(ref ActionState state)
        {
        }

        public void StoreState(ref ActionState state)
        {
        }

        protected override void UpdateOutput(object target)
        {
            switch (target)
            {
                case null:
                    wavePlayer.Stop();
                    break;

                case USound uSound:
                    PlayFromObject(uSound);
                    break;
            }
        }

        private void PlayFromObject(USound uSound)
        {
            var waveEvent = wavePlayer.Play(uSound.Data, uSound.FileType);
            waveEvent.PlaybackStopped += (sender, args) =>
            {
                wavePlayer.Stop();
            };
        }
        
        private void playButton_Click(object sender, System.EventArgs e)
        {
            if (Object == null) return;
            
            PlayFromObject((dynamic)Object);
        }
    }
}