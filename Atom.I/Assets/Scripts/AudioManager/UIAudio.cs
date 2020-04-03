using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAudio : MonoBehaviour
{
    [SerializeField] private AudioClip clik;
    [SerializeField] private Button[] buttons;
    private AudioSource audioSource { get { return GetComponent<AudioSource>(); } }

    private void Start()
    {
        gameObject.AddComponent<AudioSource>();

       foreach (Button item in buttons)
       {
            item.onClick.AddListener(() => clikSound());
       }
    }

    private void clikSound()
    {
        audioSource.PlayOneShot(clik);
    }

}
