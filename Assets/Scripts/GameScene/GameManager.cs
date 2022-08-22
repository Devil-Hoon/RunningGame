using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	#region instance;
	public static GameManager instance;
	private void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
	}
	#endregion

	public Dictionary<string, Item> items = new Dictionary<string, Item>();
	public SpawnManager spawnManager;
	public GameObject startAnimation;
	public GameObject continueAnimation;
	public float gameSpeed = 1;
	// Use this for initialization
	public bool mapMove = false;
	public bool isPlay = false;
	//public GameObject playBtn;
	public float maxHP = 0;
	public bool isPause = false;
	public int score = 0;
	public int coin = 0;
	public delegate void OnPlay(bool isPlay);
	public OnPlay onPlay;
	public Player player;
	public LifeBar lifeBar;
	public BoosterGage boosterGage;
	public Button boosterBtn;
	public Button jumpBtn;
	public Text scoreText;
	public Text endScoreText;
	public Text distanceText;
	public Text coinText;
	public GameObject gameOverPanel;
	public RewardedAD adManager;
	private int life;
	public int booster;
	public bool boosterOn;
	private void Start()
	{
		int setWidth = 1920; // 사용자 설정 너비
		int setHeight = 1080; // 사용자 설정 높이

		int deviceWidth = Screen.width; // 기기 너비 저장
		int deviceHeight = Screen.height; // 기기 높이 저장

		Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기
		if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
		{
			float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
			Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
		}
		else // 게임의 해상도 비가 더 큰 경우
		{
			float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
			Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
		}
		life = 1;
		maxHP = 300;
		mapMove = true;
		startAnimation.SetActive(true);
	}
	private void Update()
	{
		scoreText.text = (score + coin).ToString();
	}
	//IEnumerator AddScore()
	//{
	//	while (isPlay)
	//	{
	//		if (!isPause)
	//		{
	//			score++;
	//			scoreText.text = score.ToString();
	//			gameSpeed = gameSpeed + 0.01f;
	//		}
	//		yield return new WaitForSeconds(0.1f);
	//	}
	//}
	public void PlayStart()
	{
		isPlay = true;
		isPause = false;
		onPlay.Invoke(isPlay);
		score = 0;
		scoreText.text = score.ToString();
		//StartCoroutine(AddScore());
	}
	public void UseBooster()
	{
		if (booster == boosterGage.gage.maxValue)
		{
			booster = 0;
			player.InvincibleOn();
			player.SoilOn();
			boosterOn = true;
		}
	}
	public void BoosterOFF()
	{
		boosterOn = false;
		boosterGage.EffectOff();
		if (player.option != ItemOption.Invincible)
		{
			player.InvincibleOff();
		}
		player.SoilOff();
	}
	public void PlayPauseSet(bool b)
	{
		isPause = b;
	}
	public void GameOver()
	{
		if (life > 0)
		{
			mapMove = false;
			isPlay = false;
			//StopCoroutine(AddScore());
			spawnManager.AllClean();
			gameOverPanel.SetActive(true);
			adManager.continueADBtn.interactable = true;
			adManager.continueBtn.gameObject.SetActive(false);
			endScoreText.text = (score + coin).ToString();
			distanceText.text = score.ToString();
			coinText.text = coin.ToString();
		}
		else
		{
			mapMove = false;
			isPlay = false;
			//StopCoroutine(AddScore());
			spawnManager.AllClean();
			gameOverPanel.SetActive(true);
			adManager.continueADBtn.interactable = false;
			adManager.continueBtn.gameObject.SetActive(false);
			endScoreText.text = (score + coin).ToString();
			distanceText.text = score.ToString();
			coinText.text = coin.ToString();
		}
	}
	public void Restart()
	{
		SceneManager.LoadScene("GamePlay");
	}
	public void ContinueAnim()
	{
		gameOverPanel.SetActive(false);
		maxHP = 300;
		lifeBar.SetTimeBarMaxRange(300);
		lifeBar.SetTimeBarValue(300);
		continueAnimation.SetActive(true);
	}
	public void ContinueRun()
	{
		mapMove = true;
		//playBtn.SetActive(false);
		isPlay = true;
		isPause = false;
		onPlay.Invoke(isPlay);
		spawnManager.healRespawn = 0;
		scoreText.text = score.ToString();
	}
	public void Main()
	{
		SceneManager.LoadScene("Main");
	}
}
