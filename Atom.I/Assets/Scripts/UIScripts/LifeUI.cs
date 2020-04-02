using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    [SerializeField] private Sprite on_icon;
    [SerializeField] private Sprite off_icon;
    [SerializeField] private Image[] life_icons;

    private int items;

    private void Start()
    {
        BoxManager.Container.onFailedIsolation.AddListener(resolveItems);
        items = life_icons.Length - 1;

        foreach (Image item in life_icons)
        {
            item.sprite = on_icon;
        }
    }

    private void resolveItems()
    {
        if (items >= 0)
        {
            life_icons[items].sprite = off_icon;
            items--;
        } 
    }
}
