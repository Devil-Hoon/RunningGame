using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer[] tiles;
	[SerializeField]
	Sprite[] plateImg;
	Player player;
    // Start is called before the first frame update
    void Start()
    {
		player = GameObject.FindWithTag("Player").GetComponent<Player>();
		temp = tiles[0];
    }
	SpriteRenderer temp;
    // Update is called once per frame
    void Update()
    {
		if (GameManager.instance.mapMove && !GameManager.instance.isPause)
		{
			for (int i = 0; i < tiles.Length; i++)
			{
				if (-14.24 >= tiles[i].transform.position.x)
				{
					for (int q = 0; q < tiles.Length; q++)
					{
						if (temp.transform.position.x < tiles[q].transform.position.x)
						{
							temp = tiles[q];
						}
					}
					if (GameManager.instance.isPlay)
					{
						GameManager.instance.score += 2;
					}
					tiles[i].transform.position = new Vector2(temp.transform.position.x + 3.45f, -6.7f);
					tiles[i].sprite = plateImg[Random.Range(0, plateImg.Length)];
				}
			}
			for (int i = 0; i < tiles.Length; i++)
			{
				if (GameManager.instance.boosterOn)
				{
					tiles[i].transform.Translate(new Vector2(-1, 0) * Time.deltaTime * (GameManager.instance.gameSpeed + player.itemSpeed) * 1.5f);
				}
				else
				{
					tiles[i].transform.Translate(new Vector2(-1, 0) * Time.deltaTime * (GameManager.instance.gameSpeed + player.itemSpeed));
				}
			}
		}
    }
}
