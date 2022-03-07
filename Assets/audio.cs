using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio : MonoBehaviour
{
    private AudioSource _audioSource;
	private bool play;
    async void Start()
    {
        print("just true play");
        print(play);
        
        if (play)
        {
            PlayMusic();
        }
        play = false;

    }
    private void Awake()
     {

        print("on awake");
         DontDestroyOnLoad(transform.gameObject);
         _audioSource = GetComponent<AudioSource>();
        play = true;

    }
 
     public void PlayMusic()
     {
         if (_audioSource.isPlaying) return;
         _audioSource.Play();
     }
 
     public void StopMusic()
     {
         _audioSource.Stop();
     }

}

