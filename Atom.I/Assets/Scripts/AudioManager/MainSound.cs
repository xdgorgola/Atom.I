using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSound : MonoBehaviour
{
    [SerializeField] private AudioClip bas;

    private AudioSource audioSource { get { return GetComponent<AudioSource>(); } }

    private void Start()
    {
        gameObject.AddComponent<AudioSource>();

        audioSource.clip = bas;
        audioSource.loop = true;
        audioSource.Play();
    }
}
