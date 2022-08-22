using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
	[SerializeField]
	public Vector2 startPosition;
	[SerializeField]
	float speed;
	[SerializeField]
	Transform coinImg;
	Vector3 rot;
	Player player;
	private void OnEnable()
	{
		transform.position = startPosition;
		rot = Vector3.up;
		coinImg.Rotate(Vector3.zero);
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
			rot += Vector3.up * Time.deltaTime;
			coinImg.Rotate(rot);
			if (transform.position.x < -13)
			{
				gameObject.SetActive(false);
			}
		}
	}
}
