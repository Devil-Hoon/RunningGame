using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
	[SerializeField]
	public Vector2 startPosition;
	[SerializeField]
	float speed;

	public bool crash;
	Player player;
	private void OnEnable()
	{
		transform.position = startPosition;

		crash = false;
		//sd.enabled = true;

		//obj.SetActive(true);
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
			if (GameManager.instance.boosterOn)
			{
				transform.Translate(Vector2.left * Time.deltaTime * (speed + player.itemSpeed) * 1.5f);
			}
			else
			{
				transform.Translate(Vector2.left * Time.deltaTime * (speed + player.itemSpeed));
			}
			
			if (transform.position.x < -14.75f)
			{
				gameObject.SetActive(false);
			}
		}
	}

	public void MouseDead()
	{
		gameObject.SetActive(false);
	}
}
