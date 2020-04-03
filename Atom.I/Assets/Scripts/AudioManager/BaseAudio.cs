using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAudio : MonoBehaviour
{
    [SerializeField] private AudioClip clik;
    private AudioSource audioSource { get { return GetComponent<AudioSource>(); } }

    private void Start()
    {
        gameObject.AddComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.clip = clik;

        GameManagerScript.Manager.onGameStarted.AddListener(() => clikSound());
        GameManagerScript.Manager.onResume.AddListener(() => clikSound());
        GameManagerScript.Manager.onPause.AddListener(() => clikStop());
        GameManagerScript.Manager.onFinishedGame.AddListener(() => clikStop());
    }
    private void clikSound()
    {
        audioSource.Play();
    }
    private void clikStop()
    {
        audioSource.Stop();
    }

}
