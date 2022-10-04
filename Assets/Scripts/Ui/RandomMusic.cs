using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ui
{
    [RequireComponent(typeof(AudioSource))]
    public class RandomMusic : MonoBehaviour
    {
        public AudioClip[] musicClips;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            StartCoroutine(PlayRandomMusicClips());
        }

        private IEnumerator PlayRandomMusicClips()
        {
            while (Application.isPlaying)
            {
                _audioSource.clip = musicClips[Random.Range(0, musicClips.Length)];
                _audioSource.Play();

                yield return new WaitForSeconds(_audioSource.clip.length);
            }

            yield return new WaitForEndOfFrame();
        }
    }
}