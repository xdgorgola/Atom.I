using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkController : MonoBehaviour
{
	public float flash_duration;
    public float light_step;

    Renderer renderer;

    private IEnumerator flash_coroutine;

	private void Start()
	{
		renderer = gameObject.GetComponent<Renderer>();
	}

    private void Update() {
		InvokeRepeating("Flash", 0f, light_step);
	}

	private void Flash(){
		if (flash_coroutine != null)
		    StopCoroutine(flash_coroutine);
		
		flash_coroutine = DoFlash();
        StartCoroutine(flash_coroutine);
    }

   private IEnumerator DoFlash()
    {
        while (true)
        {
            float lerp_time = 0;

            while (lerp_time < flash_duration)
            {
                lerp_time += Time.deltaTime;
                float perc = lerp_time / flash_duration;

                SetFlashAmount(1f - perc);
            }
            SetFlashAmount(0.1f);
            yield return new WaitForSeconds(light_step);
        }
    }
	
    private void SetFlashAmount(float flash_amount)
    {
        renderer.material.SetFloat("_Littness", flash_amount);
    }
}
