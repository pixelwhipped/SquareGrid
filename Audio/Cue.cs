using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Multimedia;
using SharpDX.XAudio2;

namespace SquareGrid.Audio
{
    public class Cue
    {
        internal string Sound;
        private readonly AudioFx _audio;
        public readonly AudioChannels Channel;
        private XAudio2 Device { get { return (Channel==AudioChannels.Effect)?_audio.Effects:_audio.Music; } }
        private readonly byte[] _byteStream;
        private readonly List<SourceVoice> _voices;
        public bool Loop;
        private SoundStream Stream
        {
            get
            {
                var m = new MemoryStream(_byteStream);
                return new SoundStream(m);
            }
        }

        public Cue(AudioFx audio, AudioChannels channel, string path)
        {
            Channel = channel;
            Sound = path;
            _audio = audio;
            var s = Windows.ApplicationModel.Package.Current.InstalledLocation.OpenStreamForReadAsync(Path.Combine(_audio.Game.Content.RootDirectory, path));
            s.Wait();
            var stream = s.Result;
            _byteStream = ReadFully(stream);
            _voices = new List<SourceVoice>();
        }
        public static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        internal void Start()
        {
            SourceVoice s;
            if (_voices.Any(v => v.State.BuffersQueued <= 0))
            {
                s = _voices.First(v => v.State.BuffersQueued <= 0);
            }
            else
            {
                s = new SourceVoice(Device, Stream.Format, true);
                _voices.Add(s);
            }
            var b = new AudioBuffer
            {
                Stream = Stream.ToDataStream(),
                AudioBytes = (int)Stream.Length,
                LoopCount = Loop ? AudioBuffer.LoopInfinite : 1,
                Flags = BufferFlags.EndOfStream
            };
            s.SubmitSourceBuffer(b, Stream.DecodedPacketsInfo);
            s.Start();
        }
    }
}
