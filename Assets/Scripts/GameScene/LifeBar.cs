using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LifeBar : MonoBehaviour
{
	public Slider lifeBar;
	public Image fill;
	Player player;
	// Start is called before the first frame update

	private void Start()
	{
		player = GameObject.FindWithTag("Player").GetComponent<Player>();
	}
	private void OnEnable()
	{
		SetTimeBarMaxRange(300);
	}
	private void Update()
	{
		if (GameManager.instance.isPlay && !GameManager.instance.isPause)
		{
			SetTimeBarValue(lifeBar.value - (Time.deltaTime * player.itemRecovery));

			if (lifeBar.value <= 0.0f)
			{
				GameManager.instance.GameOver();
			}
		}
	}
	public void SetTimeBarMaxRange(float range)
	{
		lifeBar.maxValue = range;
		lifeBar.value = range;
	}

	public void SetTimeBarValue(float value)
	{
		lifeBar.value = value;
	}
}
