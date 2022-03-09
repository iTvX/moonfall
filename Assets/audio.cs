using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class audio : MonoBehaviour
{
    private AudioSource _audioSource;
	public static bool play = false;
   
    private void Awake()
     {

        Debug.Log("Awake:" + SceneManager.GetActiveScene().name);
        DontDestroyOnLoad(transform.gameObject);
         _audioSource = GetComponent<AudioSource>();
        if (!play)
        {
            PlayMusic();
            play = true;
        }

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

