using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
	[SerializeField]
	SpriteRenderer[] maps;
	[SerializeField]
	float speed;
	// Start is called before the first frame update
	void Start()
	{
		temp = maps[0];
	}
	SpriteRenderer temp;
	// Update is called once per frame
	void Update()
	{
		if (GameManager.instance.mapMove && !GameManager.instance.isPause)
		{
			for (int i = 0; i < maps.Length; i++)
			{
				if (-24.95f >= maps[i].transform.position.x)
				{
					for (int q = 0; q < maps.Length; q++)
					{
						if (temp.transform.position.x < maps[q].transform.position.x)
						{
							temp = maps[q];
						}
					}

					maps[i].transform.position = new Vector2(temp.transform.position.x + 24.95f, 0);
				}
			}
			for (int i = 0; i < maps.Length; i++)
			{
				if (GameManager.instance.boosterOn)
				{
					maps[i].transform.Translate(Vector2.left * Time.deltaTime * speed * 1.5f);
				}
				else
				{
					maps[i].transform.Translate(Vector2.left * Time.deltaTime * speed);
				}
			}
		}
	}
	
}
