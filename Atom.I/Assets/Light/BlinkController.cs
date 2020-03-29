using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkController : MonoBehaviour
{
	public float flashDuration;

    Renderer renderer;

    private IEnumerator flashCoroutine;

	private void Start()
	{
		renderer = gameObject.GetComponent<Renderer>();
	}

    private void Update() {
		if(Input.GetKeyDown(KeyCode.Space))
		    Flash();
	}

	private void Flash(){
		if (flashCoroutine != null)
		    StopCoroutine(flashCoroutine);
		
		flashCoroutine = DoFlash();
        StartCoroutine(flashCoroutine);
    }

   private IEnumerator DoFlash()
    {
        float lerpTime = 0;

        while (lerpTime < flashDuration)
        {
            lerpTime += Time.deltaTime;
            float perc = lerpTime / flashDuration;

            SetFlashAmount(1f - perc);
            yield return null;
        }
        SetFlashAmount(0);
    }
	
    private void SetFlashAmount(float flashAmount)
    {
        renderer.material.SetFloat("_FlashAmount", flashAmount);
    }
}
