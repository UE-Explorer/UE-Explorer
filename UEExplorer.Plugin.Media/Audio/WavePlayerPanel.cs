using System;
using System.Diagnostics;
using System.Windows.Forms;
using UELib.Core;

namespace UEExplorer.Plugin.Media.Audio
{
    public partial class WavePlayerPanel : UserControl
    {
        private USound _WaveSource;

        public WavePlayerPanel()
        {
            InitializeComponent();
        }

        public USound WaveSource
        {
            get => _WaveSource;
            set
            {
                _WaveSource = value;
                
                if (value == null)
                {
                    wavePlayer.Stop();
                }

                if (value != null)
                {
                    PlayFromObject(value);
                }
            }
        }

        private void PlayFromObject(USound uSound)
        {
            Debug.Assert(uSound != null);

            // FIXME: Deal with this elsewhere
            uSound.BeginDeserializing();
            uSound.RawData.LoadData(uSound.GetBuffer());
            var waveEvent = wavePlayer.Play(uSound.RawData.ElementData, uSound.FileType);
            waveEvent.PlaybackStopped += (sender, args) => { wavePlayer.Stop(); };
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (WaveSource == null)
            {
                return;
            }

            PlayFromObject((dynamic)WaveSource);
        }
    }
}
