using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoosterGage : MonoBehaviour
{
	public Slider gage;
	public Image fill;
	public Image effect;
	public float boosterCount;
	public float time;
	// Start is called before the first frame update
	private void OnEnable()
	{
		SetTimeBarValue(0);
		boosterCount = gage.maxValue / time;
	}

	private void FixedUpdate()
	{
		if (GameManager.instance.boosterOn)
		{
			float value = gage.value - Time.deltaTime * boosterCount;
			SetTimeBarValue(value);
			if (gage.value <= 0)
			{
				GameManager.instance.BoosterOFF();
			}
		}
		
	}
	public void SetTimeBarValue(float value)
	{
		gage.value = value;
	}


	public void EffectOn()
	{
		effect.gameObject.SetActive(true);
	}
	public void EffectOff()
	{
		effect.gameObject.SetActive(false);
	}
}
