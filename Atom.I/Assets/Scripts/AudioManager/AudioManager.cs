using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip isolation;
    [SerializeField] private AudioClip fail;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (BoxManager.Container != null)
        {
            BoxManager.Container.onFailedIsolation.AddListener(failSound);
            BoxManager.Container.onSucessfullIsolation.AddListener(isoSound);
        }
    }

    private void failSound()
    {
        audioSource.clip = fail;
        audioSource.Play();
    }

     private void isoSound()
    {
        audioSource.clip = isolation;
        audioSource.Play();
    }
}
