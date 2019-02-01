using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.IO;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using System.Collections.Generic;
using System.Linq;


namespace SquareGrid.Audio
{
    public class AudioFx
    {
        internal BaseGame Game;
        private MasteringVoice _effectsVoice;
        internal XAudio2 Effects;
        private readonly List<Cue> _effectCues;

        private MasteringVoice _musicVoice;
        internal XAudio2 Music;
        private readonly List<Cue> _musicCues;

        public bool EnableAudio
        {
            get
            {
                
                var localSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
                if (!localSettings.Values.ContainsKey("AudioState"))
                    localSettings.Values["AudioState"] = true.ToString();
                var v = bool.Parse((string)localSettings.Values["AudioState"]);
                return v;
            }
            set
            {
                Windows.Storage.ApplicationData.Current.RoamingSettings.Values["AudioState"] = value.ToString();

                if (!EnableAudio)
                {
                    _musicVoice.SetVolume(0);
                    _effectsVoice.SetVolume(0);
                }
                else
                {
                    _musicVoice.SetVolume(MusicVolume);
                    _effectsVoice.SetVolume(EffectVolume);
                }
            }
        }

        public float EffectVolume
        {
            get
            {
                var localSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
                if (!localSettings.Values.ContainsKey("EffectVolume"))
                    localSettings.Values["EffectVolume"] = 100.ToString();
                var v = float.Parse((string)localSettings.Values["EffectVolume"]);
                return MathHelper.Clamp(v/100f, 0f, 1f);
            }
            set
            {
                Windows.Storage.ApplicationData.Current.RoamingSettings.Values["EffectVolume"] = MathHelper.Clamp(value * 100f, 0, 100).ToString();
                if (EnableAudio)
                {
                    _effectsVoice.SetVolume(float.Parse((string)Windows.Storage.ApplicationData.Current.RoamingSettings.Values["EffectVolume"]) / 100f);
                }
                else
                {
                    _effectsVoice.SetVolume(0f);
                }
            }
        }
        public float MusicVolume
        {
            get
            {
                var localSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
                    if (!localSettings.Values.ContainsKey("MusicVolume"))
                        localSettings.Values["MusicVolume"] = 100.ToString();
                    var v = float.Parse((string)localSettings.Values["MusicVolume"]);
                    return MathHelper.Clamp(v/100f,0f,1f);
            }
            set
            {
                Windows.Storage.ApplicationData.Current.RoamingSettings.Values["MusicVolume"] = MathHelper.Clamp(value * 100f, 0, 100).ToString();
                if (EnableAudio)
                {
                    _musicVoice.SetVolume(float.Parse((string)Windows.Storage.ApplicationData.Current.RoamingSettings.Values["MusicVolume"]) / 100f);
                }
                else
                {
                    _musicVoice.SetVolume(0f);
                }
            }
        }

        public AudioFx(BaseGame game)
        {
            Game = game;
            Effects = new XAudio2();
            Task.Run(() =>
            {
                Effects.StartEngine();
                _effectsVoice = new MasteringVoice(Effects);
                _effectsVoice.SetVolume(EffectVolume);
                if (!EnableAudio) _effectsVoice.SetVolume(0);
            });
            _effectCues = new List<Cue>();

            Music = new XAudio2();
            Task.Run(() =>
            {
                Music.StartEngine();
                _musicVoice = new MasteringVoice(Music);
                _musicVoice.SetVolume(MusicVolume);
                if (!EnableAudio) _musicVoice.SetVolume(0);
            });
            _musicCues = new List<Cue>();
        }

        public Cue Play(string sound, AudioChannels channel = AudioChannels.Effect, bool loop = false)
        {
            var any = ((channel == AudioChannels.Effect) ? _effectCues : _musicCues).Any(p => p.Sound == sound);
            if (any)
            {
                var c = ((channel == AudioChannels.Effect) ? _effectCues : _musicCues).First(p => p.Sound == sound);
                c.Loop = loop;
                c.Start();
                return c;
            }
            else
            {
                var c = new Cue(this, channel, sound);
                ((channel == AudioChannels.Effect) ? _effectCues : _musicCues).Add(c);
                c.Loop = loop;
                c.Start();
                return c;
            }
        }
    }
}
