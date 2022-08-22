using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemParts : MonoBehaviour
{
    public Item item;
    [SerializeField]
    SpriteRenderer rdr;
    bool isBlinkHigh;
    float colorA;
    // Start is called before the first frame update
    private void OnEnable()
    {
        colorA = 1.0f;
    }

    public void InvincibleOn()
    {
        StopCoroutine("InvincibleBlink");
        StartCoroutine("InvincibleBlink");
    }

    public void InvincibleOff()
    {
        StopCoroutine("InvincibleBlink");
        rdr.color = new Color(rdr.color.r, rdr.color.g, rdr.color.b, 1);
    }

    IEnumerator InvincibleBlink()
    {
        while (GameManager.instance.isPlay && !GameManager.instance.isPause)
        {
            if (isBlinkHigh)
            {
                colorA += Time.deltaTime * 1000;
            }
            else
            {
                colorA -= Time.deltaTime * 1000;
            }

            if (colorA > 1)
            {
                colorA = 1;
                isBlinkHigh = false;
            }
            else if (colorA < 0)
            {
                colorA = 0;
                isBlinkHigh = true;
            }

            rdr.color = new Color(rdr.color.r, rdr.color.g, rdr.color.b, colorA);

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Init(Item item)
	{
        this.item = item;
        rdr.sprite = item.img;
        isBlinkHigh = false;
        colorA = 1;
        rdr.color = new Color(rdr.color.r, rdr.color.g, rdr.color.b, colorA);
    }

    public void Clean()
	{
        this.item = null;
        rdr.sprite = null;
        isBlinkHigh = false;
        colorA = 1;
	}
}
