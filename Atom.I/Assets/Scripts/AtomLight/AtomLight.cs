﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AtomState { InBox, OutBox }
public class AtomLight : MonoBehaviour
{
    public Atoms atomKind = Atoms.Atom;
	public float flash_duration;
    public float light_step;

    [HideInInspector()]
    public AtomState state = AtomState.OutBox;

    Renderer render;

    private IEnumerator flash_coroutine;

    private void Awake()
    {
        render = GetComponent<Renderer>();
    }

	private void Start()
	{
        BoxManager.Container.onStartIsolation.AddListener(updateInIsolation);
        BoxManager.Container.onSucessfullIsolation.AddListener(synchronicity);
        BoxManager.Container.onFailedIsolation.AddListener(synchronicity);

        GameManagerScript.Manager.onGameStarted.AddListener(flash);
        GameManagerScript.Manager.onGameOver.AddListener(() => StopCoroutine(flash_coroutine));
        GameManagerScript.Manager.onGameOver.AddListener(lightsOn);
        GameManagerScript.Manager.onFinishedGame.AddListener(() => StopCoroutine(flash_coroutine));
        GameManagerScript.Manager.onFinishedGame.AddListener(lightsOn);
	}

    private void updateInIsolation()
    {
        StopCoroutine(flash_coroutine);
        if (state == AtomState.InBox) lightsOn();
        else lightsOff();
    }

    private void synchronicity()
    {
        StopCoroutine(flash_coroutine);
        lightsOff();
        flash();
        state = AtomState.OutBox;
    }

	private void flash()
    {
		if (flash_coroutine != null)
		    StopCoroutine(flash_coroutine);
		
		flash_coroutine = doFlash();
        StartCoroutine(flash_coroutine);
    }

    private IEnumerator doFlash()
    {
        while (true)
        {
            float lerp_time = 0;
            while (lerp_time < flash_duration)
            {
                lerp_time += Time.deltaTime;
                float perc = lerp_time / flash_duration;

                setFlashAmount(1f - perc);
                yield return null;
            }
            setFlashAmount(0.005f);
            yield return new WaitForSeconds(light_step);
        }
    }

    private void lightsOn()
    {
        setFlashAmount(1f);
    }

    private void lightsOff()
    {
        setFlashAmount(0.005f);
    }

    private void setFlashAmount(float flash_amount)
    {
        render.material.SetFloat("_Littness", flash_amount);
    }
}