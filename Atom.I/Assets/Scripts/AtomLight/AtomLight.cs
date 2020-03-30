using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AtomState { InBox, OutBox }
public class AtomLight : MonoBehaviour
{
    // Public
    public Atoms atomKind = Atoms.Atom;
	public float flash_duration;
    public float light_step;

    [HideInInspector()]
    public AtomState state = AtomState.OutBox;

    Renderer render;

    private IEnumerator flash_coroutine;

    private void awake()
    {
        render = gameObject.GetComponent<Renderer>();
    }

	private void Start()
	{
        //flash();

        BoxAtomContainer.Container.onStartIsolation.AddListener(updateInIsolation);
        BoxAtomContainer.Container.onSucessfullIsolation.AddListener(synchronicity);
        BoxAtomContainer.Container.onFailedIsolation.AddListener(synchronicity);

        GameManagerScript.Manager.onGameOver.AddListener(lightsOn);
        GameManagerScript.Manager.onFinishedGame.AddListener(lightsOff);
	}

    private void updateInIsolation()
    {
        if (state == AtomState.InBox) lightsOn();
        else lightsOff();
    }

    private void synchronicity()
    {
        state = AtomState.OutBox;
        lightsOff();
        flash();
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