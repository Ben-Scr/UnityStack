using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BenScr.UnityStack
{
    /*
     * Used for playing music and sound effects, with support for multiple channels and volume control. Uses an object pool to manage audio sources and prevent performance issues from creating/destroying them frequently.
     */

    public class AudioManager : MonoBehaviour
    {
        public enum AudioChannel
        {
            Music = 0,
            Sound = 1,
        }

        [SerializeField] private GameObject audioSourcePrefab;
        private static DynamicObjectPool<int> audioPool = new DynamicObjectPool<int>();
        private static List<GameObject> activeMusic = new List<GameObject>();
        private static List<GameObject> activeSounds = new List<GameObject>();

        [Range(0, 1)][SerializeField] private float masterVolume = 1.0f;
        [Range(0, 1)][SerializeField] private float musicVolume = 1.0f;
        [Range(0, 1)][SerializeField] private float sfxVolume = 1.0f;

        public static AudioManager instance;

        private void Awake()
        {
            instance = this;
            audioPool = new DynamicObjectPool<int>();
            activeMusic = new List<GameObject>();
            activeSounds = new List<GameObject>();
        }

        public static AudioClip GetClipByName_Sfx(string name)
        {
            return GameAssets.instance.sfxTracks.FirstOrDefault(s => s.name == name)?.clip ?? null;
        }

        public static AudioSource PlaySongData_MusicTrack(SongData songData, float offset = 0)
        {
            GameObject audioSourceObj = audioPool.Get((int)AudioChannel.Music, instance.audioSourcePrefab, instance.transform);
            var source = audioSourceObj.GetComponent<AudioSource>();

            source.clip = songData.clip;
            source.loop = LevelController.activeLevel.mapSettings.loopSong;
            source.time = offset;
            source.volume = songData.baseVolume * instance.musicVolume * instance.masterVolume;
            source.Play();
            activeMusic.Add(audioSourceObj);

            return source;
        }
        public static AudioSource PlaySongData_Music(SongData songData, bool shouldLoop = false, float offset = 0)
        {
            if(songData == null) return null;

            GameObject audioSourceObj = audioPool.Get((int)AudioChannel.Music, instance.audioSourcePrefab, instance.transform);
            var source = audioSourceObj.GetComponent<AudioSource>();

            source.clip = songData. clip;
            source.loop = shouldLoop;
            source.time = offset;
            source.volume = songData.baseVolume * instance.musicVolume * instance.masterVolume;
            source.Play();
            activeMusic.Add(audioSourceObj);

            return source;
        }

        public static AudioSource PlayAudioClip_Sfx(AudioClip clip, float offset = 0)
        {
            GameObject audioSourceObj = audioPool.Get((int)AudioChannel.Sound, instance.audioSourcePrefab, instance.transform);
            var source = audioSourceObj.GetComponent<AudioSource>();

            source.clip = clip;
            source.loop = LevelController.activeLevel.mapSettings.loopSong;
            source.time = offset;
            source.volume = 0.1f * instance.musicVolume * instance.masterVolume;
            source.Play();
            activeMusic.Add(audioSourceObj);

            return source;
        }

        public static bool StopAudioClip_Sfx(AudioClip clip)
        {
            var source = GetAudioSourceByClip_Sfx(clip);

            if (source == null) return false;

            activeSounds.Remove(source.gameObject);
            source.Stop();
            audioPool.Release((int)AudioChannel.Sound, source.gameObject);
            return true;
        }

        public static bool StopAudioClip_Music(AudioClip clip)
        {
            var source = GetAudioSourceByClip_Music(clip);

            if (source == null) return false;

            activeMusic.Remove(source.gameObject);
            source.Stop();
            audioPool.Release((int)AudioChannel.Music, source.gameObject);
            return true;
        }
        public static bool StopSongData_Music(SongData songData)
        {
            if (songData == null) return false;

            var source = GetAudioSourceByClip_Music(songData.clip);

            if (source == null) return false;

            activeMusic.Remove(source.gameObject);
            source.Stop();
            audioPool.Release((int)AudioChannel.Music, source.gameObject);
            return true;
        }

        public static AudioSource GetAudioSourceByClip_Music(AudioClip clip)
        {
            try
            {
                var obj = activeMusic.Where(instance => instance.GetComponent<AudioSource>().clip == clip).FirstOrDefault();
                return obj.GetComponent<AudioSource>();
            }
            catch
            {
                return null;
            }
        }
        public static AudioSource GetAudioSourceByClip_Sfx(AudioClip clip)
        {
            try
            {
                var obj = activeSounds.Where(instance => instance.GetComponent<AudioSource>().clip == clip).FirstOrDefault();
                return obj.GetComponent<AudioSource>();
            }
            catch
            {
                return null;
            }
        }
    }

    [System.Serializable]
    public class SongData
    {
        public string name;
        public AudioClip clip;
        public float baseVolume = 0.1f;

        public AudioSource AudioSource()
        {
            return AudioManager.GetAudioSourceByClip_Music(clip);
        }
        public void Play(float offset = 0)
        {
            AudioManager.PlaySongData_MusicTrack(this, offset);
        }
        public void Stop()
        {
            AudioManager.StopAudioClip_Music(clip);
        }
        public void Pause()
        {
            var source = AudioManager.GetAudioSourceByClip_Music(clip);

            if (source != null)
            {
                source.Pause();
            }
        }
        public void UnPause()
        {
            var source = AudioManager.GetAudioSourceByClip_Music(clip);

            if (source != null)
            {
                source.UnPause();
            }
        }
    }
}