using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartAnimation : MonoBehaviour
{
	public void GameStart()
	{
		GameManager.instance.PlayStart();
		gameObject.SetActive(false);
	}

	public void GameContinue()
	{
		GameManager.instance.ContinueRun();
		gameObject.SetActive(false);
	}
}
