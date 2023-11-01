using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using NAudio.Wave;

namespace UEExplorer.Render.Components.Audio
{
    public class WavePlayerComponent : IComponent
    {
        private WaveOutEvent _OutputDevice;
        private Stream _WaveStream;

        public void Dispose()
        {
            Stop();
            _OutputDevice?.Dispose();
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public ISite Site { get; set; }
        public event EventHandler Disposed;

        public WaveOutEvent Play(byte[] rawData, string fileType)
        {
            // We'll do a complete reboot on every play, just to be safe
            Stop();

            Debug.Assert(_OutputDevice == null);
            _OutputDevice = new WaveOutEvent();

            // Expected to be disposed by the WaveFileReader
            var stream = new MemoryStream(rawData);

            IWaveProvider provider;
            switch (fileType.ToUpperInvariant())
            {
                case "WAV":
                    provider = new WaveFileReader(stream);
                    break;

                default:
                    throw new NotSupportedException($"Audio input '{fileType}' is not supported");
            }

            Debug.Assert(_WaveStream == null);
            _WaveStream = (Stream)provider;

            _OutputDevice.Init(provider);
            _OutputDevice.Play();

            return _OutputDevice;
        }

        public void Stop()
        {
            if (_OutputDevice == null)
            {
                return;
            }

            _OutputDevice.Stop();
            _OutputDevice.Dispose();
            _OutputDevice = null;

            if (_WaveStream == null)
            {
                return;
            }

            _WaveStream.Dispose();
            _WaveStream = null;
        }
    }
}
