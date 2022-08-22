using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    public Vector2 startPosition;
    [SerializeField]
    float speed;
	Player player;
    bool up;
	private void OnEnable()
	{
        transform.position = startPosition;
        up = false;
	}
	// Start is called before the first frame update
	void Start()
    {
		player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
		if (GameManager.instance.isPlay && !GameManager.instance.isPause)
		{
			if (gameObject.name == "Balloon1(Clone)" || gameObject.name =="Balloon2(Clone)")
			{
				if (up)
				{
					if (GameManager.instance.boosterOn)
					{
						transform.Translate(Vector2.left * Time.deltaTime * (speed + player.itemSpeed) * 1.5f + Vector2.up * Time.deltaTime);
					}
					else
					{
						transform.Translate(Vector2.left * Time.deltaTime * (speed + player.itemSpeed) + Vector2.up * Time.deltaTime);
					}
				}
				else
				{
					if (GameManager.instance.boosterOn)
					{
						transform.Translate(Vector2.left * Time.deltaTime * (speed + player.itemSpeed) * 1.5f + Vector2.down * Time.deltaTime);
					}
					else
					{
						transform.Translate(Vector2.left * Time.deltaTime * (speed + player.itemSpeed) + Vector2.down * Time.deltaTime);
					}
                }

				if (transform.position.y > startPosition.y + 0.4f)
				{
                    up = false;
				}
				else if (transform.position.y < startPosition.y - 0.4f)
				{
					up = true;
				}
			}
			else
			{
				if (GameManager.instance.boosterOn)
				{
					transform.Translate(Vector2.left * Time.deltaTime * (speed + player.itemSpeed) * 1.5f);
				}
				else
				{
					transform.Translate(Vector2.left * Time.deltaTime * (speed + player.itemSpeed));
				}
			}
            if (transform.position.x < -14.75f)
            {
                gameObject.SetActive(false);
            }
        }
    }

}
