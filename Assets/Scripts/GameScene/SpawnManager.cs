using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum CoinPattern
{
	Straight,
	Round
}
public class SpawnManager : MonoBehaviour
{
	[Header("Common Field")]
	[SerializeField]
	[Range(0.3f, 10f)]
	float minSpawn, maxSpawn;
	[Header("Mob Field")]
    public List<GameObject> mobPool = new List<GameObject>();
	[SerializeField]
    GameObject[] mobs;
	[SerializeField]
	int objCnt = 1;
	[Header("Coin Field")]
	public List<GameObject> coinPool = new List<GameObject>();
	[SerializeField]
	GameObject[] coins;
	float coinY = 0.0f;
	bool coinUp = true;
	[SerializeField]
	int coinCnt = 10;
	CoinPattern coin = CoinPattern.Straight;

	[Header("Use Item Field")]
	public int healRespawn;
	[SerializeField]
	GameObject healPrefab;
	GameObject heal;

	[Header("Equip Item Field")]
	public bool itemSpawn = false;
	private List<Item> allItems;
	public GameObject itemPrefab;

	[SerializeField]
	int itemCnt = 1;
	public List<GameObject> headPool = new List<GameObject>();
	public List<GameObject> backPool = new List<GameObject>();
	public List<GameObject> clothesPool = new List<GameObject>();
	public List<GameObject> shoesPool = new List<GameObject>();

	private void Awake()
	{
		allItems = new List<Item>();
		SetItems(); 
		ItemSpawnSet();
		for (int i = 0; i < mobs.Length; i++)
		{
			for (int q = 0; q < objCnt; q++)
			{
				if (mobs[i].name != "Mouse")
				{
					mobPool.Add(CreateOBJ(mobs[i], transform));
				}
				else
				{
					if (q % 2 == 1)
					{
						mobPool.Add(CreateOBJ(mobs[i], transform));
					}
				}
			}
		}
		for (int i = 0; i < coins.Length; i++)
		{
			for (int q = 0; q < coinCnt; q++)
			{
				switch (coins[i].tag)
				{
					case "SilverCoin":
						if (q < coinCnt * 0.8f)
						{
							coinPool.Add(CreateOBJ(coins[i], transform));
						}
						else
						{
							break;
						}
						break;
					case "Coin":
						if (q < coinCnt * 0.4f)
						{
							coinPool.Add(CreateOBJ(coins[i], transform));
						}
						else
						{
							break;
						}
						break;
					case "StarCoin":
						if (q < coinCnt * 0.03f)
						{
							coinPool.Add(CreateOBJ(coins[i], transform));
						}
						else
						{
							break;
						}
						break;
					default:
						break;
				}
			}
		}
		heal = CreateOBJ(healPrefab, transform);
	}
	private void Start()
	{
		GameManager.instance.onPlay += PlayGame;
	}

	private void ItemSpawnSet()
	{
		for (int i = 0; i < allItems.Count; i++)
		{
			switch (allItems[i].Kind)
			{
				case "head":
					{
						GameObject obj = CreateOBJ(itemPrefab, transform);
						obj.GetComponent<EquipItem>().item.SetItem(allItems[i]);
						obj.GetComponent<EquipItem>().RendererSet();
						headPool.Add(obj);
					}
					break;
				case "back":
					{
						GameObject obj = CreateOBJ(itemPrefab, transform);
						obj.GetComponent<EquipItem>().item.SetItem(allItems[i]);
						obj.GetComponent<EquipItem>().RendererSet();
						backPool.Add(obj);
					}
					break;
				case "clothes":
					{
						GameObject obj = CreateOBJ(itemPrefab, transform);
						obj.GetComponent<EquipItem>().item.SetItem(allItems[i]);
						obj.GetComponent<EquipItem>().RendererSet();
						clothesPool.Add(obj);
					}
					break;
				case "shoes":
					{
						GameObject obj = CreateOBJ(itemPrefab, transform);
						obj.GetComponent<EquipItem>().item.SetItem(allItems[i]);
						obj.GetComponent<EquipItem>().RendererSet();
						shoesPool.Add(obj);
					}
					break;
				default:
					break;
			}
		}
	}
	void PlayGame(bool isPlay)
	{
		if (isPlay)
		{
			for (int i = 0; i < mobPool.Count; i++)
			{
				if (mobPool[i].activeSelf)
				{
					mobPool[i].SetActive(false);
				}
			}
			StartCoroutine(CreateMob());
			StartCoroutine(CreateCoin());
			StartCoroutine(CoinPatternChange());
			StartCoroutine(SpawnHealPack());
			StartCoroutine(CreateEquipItem());
		}
		else
		{
			StopAllCoroutines();
		}
	}
	IEnumerator CreateMob()
	{
		//yield return new WaitForSeconds(0.5f);
		while (GameManager.instance.isPlay)
		{
			if (!GameManager.instance.isPause)
			{
				mobPool[DetactiveMob()].SetActive(true);
			}
			yield return new WaitForSeconds(Random.Range(minSpawn, maxSpawn));
		}
	}
	private int DistanceCheck(int distance)
	{
		if (distance >= 1000)
		{
			int temp = distance / 1000;
			if (distance - (temp * 1000) == 0)
			{
				return 1000;
			}
			return distance - (temp * 1000);
		}
		else
		{
			int temp = distance / 1000;
			return distance - (temp * 1000);
		}
	}
	IEnumerator CreateEquipItem()
	{
		while (GameManager.instance.isPlay)
		{
			if (!itemSpawn)
			{
				if (DistanceCheck(GameManager.instance.score) == 400)
				{
					int rand = Random.Range(0, 3);
					if (rand == 0)
					{
						Debug.Log("Shoes spawn");
						shoesPool[DetactiveEquipItem("shoes")].SetActive(true);
						itemSpawn = true;
					}
					else if (rand == 1)
					{
						Debug.Log("clothes spawn");
						clothesPool[DetactiveEquipItem("clothes")].SetActive(true);
						itemSpawn = true;
					}
					else
					{
						Debug.Log("back spawn");
						backPool[DetactiveEquipItem("back")].SetActive(true);
						itemSpawn = true;
					}
				}
				else if (DistanceCheck(GameManager.instance.score) == 700)
				{
					int rand = Random.Range(0, 3);
					if (rand == 0)
					{
						Debug.Log("Shoes spawn");
						shoesPool[DetactiveEquipItem("shoes")].SetActive(true);
						itemSpawn = true;
					}
					else if (rand == 1)
					{
						Debug.Log("head spawn");
						headPool[DetactiveEquipItem("head")].SetActive(true);
						itemSpawn = true;
					}
					else
					{
						Debug.Log("back spawn");
						backPool[DetactiveEquipItem("back")].SetActive(true);
						itemSpawn = true;
					}
				}
				else if (DistanceCheck(GameManager.instance.score) == 1000)
				{
					int rand = Random.Range(0, 3);
					if (rand == 0)
					{
						Debug.Log("clothes spawn");
						clothesPool[DetactiveEquipItem("clothes")].SetActive(true);
						itemSpawn = true;
					}
					else if (rand == 1)
					{
						Debug.Log("head spawn");
						headPool[DetactiveEquipItem("head")].SetActive(true);
						itemSpawn = true;
					}
					else
					{
						Debug.Log("back spawn");
						backPool[DetactiveEquipItem("back")].SetActive(true);
						itemSpawn = true;
					}
				}
			}
			
			yield return new WaitForSeconds(0.01f);
		}
	}
	IEnumerator CreateCoin()
	{
		while (GameManager.instance.isPlay)
		{
			if (!GameManager.instance.isPause)
			{
				switch (coin)
				{
					case CoinPattern.Straight:
						coinY = 0;
						coinUp = true;
						coinPool[DetactiveCoin()].SetActive(true);
						break;
					case CoinPattern.Round:
						GameObject obj = coinPool[DetactiveCoin()];
						obj.SetActive(true);
						if (coinUp)
						{
							coinY += 1.4f;
						}
						else
						{
							coinY -= 1.4f;
						}
						if (coinY >= 4.0f)
						{
							coinUp = false;
						}
						if (coinY <= 0)
						{
							coinUp = true;
						}
						obj.transform.Translate(new Vector2(0, coinY));
						break;
					default:
						break;
				}
			}
			yield return new WaitForSeconds(0.1f);

			//if (!GameManager.instance.isPause)
			//{
			//	int num = (int)coin;
			//	num++;
			//	if (num > 1)
			//	{
			//		num = 0;
			//	}
			//	coin = (CoinPattern)num;
			//}
			//yield return new WaitForSeconds(2.0f);
		}
	}

	int DetactiveEquipItem(string kind)
	{
		int x = 0;
		switch (kind)
		{
			case "back":
				{
					List<int> num = new List<int>();
					for (int i = 0; i < backPool.Count; i++)
					{
						if (!backPool[i].activeSelf)
						{
							num.Add(i);
						}
					}
					if (num.Count > 0)
					{
						x = num[Random.Range(0, num.Count)];
					}
				}
				break;
			case "head":
				{
					List<int> num = new List<int>();
					for (int i = 0; i < headPool.Count; i++)
					{
						if (!headPool[i].activeSelf)
						{
							num.Add(i);
						}
					}
					if (num.Count > 0)
					{
						x = num[Random.Range(0, num.Count)];
					}
				}
				break;
			case "shoes":
				{
					List<int> num = new List<int>();
					for (int i = 0; i < shoesPool.Count; i++)
					{
						if (!shoesPool[i].activeSelf)
						{
							num.Add(i);
						}
					}
					if (num.Count > 0)
					{
						x = num[Random.Range(0, num.Count)];
					}
				}
				break;
			case "clothes":
				{
					List<int> num = new List<int>();
					for (int i = 0; i < clothesPool.Count; i++)
					{
						if (!clothesPool[i].activeSelf)
						{
							num.Add(i);
						}
					}
					if (num.Count > 0)
					{
						x = num[Random.Range(0, num.Count)];
					}
				}
				break;
			default:
				break;
		}
		
		return x;
	}
	IEnumerator CoinPatternChange()
	{
		while (GameManager.instance.isPlay)
		{
			if (!GameManager.instance.isPause)
			{
				coin = (CoinPattern)Random.Range(0, 2);
			}
			yield return new WaitForSeconds(1.0f);
		}
	}
	int DetactiveMob()
	{
		List<int> num = new List<int>();
		for (int i = 0; i < mobPool.Count; i++)
		{
			if (!mobPool[i].activeSelf)
			{
				num.Add(i);
			}
		}
		int x = 0;
		if (num.Count > 0)
		{
			x = num[Random.Range(0, num.Count)];
		}
		return x;
	}

	IEnumerator SpawnHealPack()
	{
		while (GameManager.instance.isPlay)
		{
			switch (healRespawn)
			{
				case 0:
					if ((GameManager.instance.lifeBar.lifeBar.value < GameManager.instance.lifeBar.lifeBar.maxValue - (GameManager.instance.lifeBar.lifeBar.maxValue / 3) + 10)
						&& (GameManager.instance.lifeBar.lifeBar.value > GameManager.instance.lifeBar.lifeBar.maxValue - (GameManager.instance.lifeBar.lifeBar.maxValue / 3 * 2) + 10))
					{
						heal.SetActive(true);
						healRespawn++;
					}

					break;
				case 1:
					if ((GameManager.instance.lifeBar.lifeBar.value <= GameManager.instance.lifeBar.lifeBar.maxValue - (GameManager.instance.lifeBar.lifeBar.maxValue / 3 * 2) + 10)
						&& (GameManager.instance.lifeBar.lifeBar.value > GameManager.instance.lifeBar.lifeBar.maxValue - GameManager.instance.lifeBar.lifeBar.maxValue + 10))
					{
						heal.SetActive(true);
						healRespawn++;
					}
					break;
				case 2:
					if (GameManager.instance.lifeBar.lifeBar.value <= GameManager.instance.lifeBar.lifeBar.maxValue - (GameManager.instance.lifeBar.lifeBar.maxValue) + 10)
					{
						heal.SetActive(true);
						healRespawn++;
					}
					break;
				default:
					break;
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	private void SetItems()
	{
		var temp = CSVReader.Read("RunGame");

		for (int i = 0; i < temp.Count; i++)
		{
			Item item = new Item();

			item.Index = temp[i]["id"].ToString();
			item.Name = temp[i]["name"].ToString();
			item.Time = int.Parse(temp[i]["time"].ToString());
			item.Jump = float.Parse(temp[i]["jump"].ToString());
			item.Speed = int.Parse(temp[i]["speed"].ToString());
			item.Kind = temp[i]["contents"].ToString();
			item.option = (ItemOption)int.Parse(temp[i]["option"].ToString());
			if (temp[i]["value1"].ToString() != "")
			{
				item.value1 = float.Parse(temp[i]["value1"].ToString());
			}
			if (temp[i]["value2"].ToString() != "")
			{
				item.value2 = float.Parse(temp[i]["value2"].ToString());
			}
			item.img = Resources.Load<Sprite>(temp[i]["경로"].ToString());
			item.packImg = Resources.Load<Sprite>(temp[i]["경로2"].ToString());
			item.Explain = temp[i]["descrition"].ToString();

			allItems.Add(item);
		}
	}
	int DetactiveCoin()
	{
		List<int> num = new List<int>();
		for (int i = 0; i < coinPool.Count; i++)
		{
			if (!coinPool[i].activeSelf)
			{
				num.Add(i);
			}
		}
		int x = 0;
		if (num.Count > 0)
		{
			x = num[Random.Range(0, num.Count)];
		}
		return x;
	}
	// Start is called before the first frame update
	GameObject CreateOBJ(GameObject obj, Transform parent)
	{
        GameObject copy = Instantiate(obj);
        copy.transform.SetParent(parent);
        copy.SetActive(false);
        return copy;
	}

	public void AllClean()
	{
		for (int i = 0; i < coinPool.Count; i++)
		{
			coinPool[i].SetActive(false);
		}
		for (int i = 0; i < mobPool.Count; i++)
		{
			mobPool[i].SetActive(false);
		}
		heal.SetActive(false);
	}
}
