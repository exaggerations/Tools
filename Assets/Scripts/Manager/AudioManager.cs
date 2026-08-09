using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace XFramework.AudioCtr
{
    [System.Serializable]
    public class Sound
    {
        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 1f;
        public bool loop = false;

        public bool playOnAwake = false;

    }


    public class AudioManager : MonoBehaviour
    {
      private static AudioManager _instance;

        public List<Sound> sounds;

        private Dictionary<string, AudioSource> audioSources;


        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
              Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
            audioSources = new Dictionary<string, AudioSource>();
        }

        private void Start()
        {
            GameObject obj = new GameObject("AudioManager");
            obj.transform.SetParent(transform);

            foreach (Sound sound in sounds)
            {
                AudioSource source = obj.AddComponent<AudioSource>();
                source.clip = sound.clip;
                source.volume = sound.volume;
                source.loop = sound.loop;
                source.playOnAwake = sound.playOnAwake;

                if(sound.playOnAwake)
                {
                    source.Play();
                }
                audioSources.Add(sound.clip.name, source);
            }
        }

        public void PlayAudio(string clipName,bool wait = false)
        {
           if(!_instance.audioSources.ContainsKey(clipName))
            {
                Debug.LogWarning($"AudioManager: Audio clip '{clipName}' not found!");
                return;
            }

           if(wait)
            {
                if(!audioSources[clipName].isPlaying)
                    audioSources[clipName].Play();
            }
            else
            {
                audioSources[clipName].Play();
            }
        }

        public void StopAudio(string clipName)
            {
                if(!audioSources.ContainsKey(clipName))
                {
                    Debug.LogWarning($"AudioManager: Audio clip '{clipName}' not found!");
                    return;
                }
    
                audioSources[clipName].Stop();
        }

    }
}