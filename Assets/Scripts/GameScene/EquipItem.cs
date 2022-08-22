using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItem : MonoBehaviour
{
	Player player;
	SpawnManager spawnManager;
	public SpriteRenderer rdr;
	public Item item = new Item();
	public Vector2 startPosition1;
	public Vector2 startPosition2;
	[SerializeField]
	float speed;
	private void OnEnable()
	{
		player = GameObject.FindWithTag("Player").GetComponent<Player>();
		int i = Random.Range(0,2);
		if (i > 0)
		{
			transform.position = startPosition1;
		}
		else
		{
			transform.position = startPosition2;
		}
	}
	// Start is called before the first frame update
	void Start()
	{
		spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
	}

	// Update is called once per frame
	void Update()
	{
		if (GameManager.instance.isPlay && !GameManager.instance.isPause)
		{
			if (GameManager.instance.boosterOn)
			{
				transform.Translate(Vector2.left * Time.deltaTime * speed * 1.5f);
			}
			else
			{
				transform.Translate(Vector2.left * Time.deltaTime * speed);
			}

			if (transform.position.x < -13)
			{
				gameObject.SetActive(false);
				spawnManager.itemSpawn = false;
			}
		}
	}

	public void RendererSet()
	{
		rdr.sprite = item.packImg;
	}
}
