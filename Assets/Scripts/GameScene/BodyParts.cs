using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyParts : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    SpriteRenderer rdr;
    bool isBlinkHigh;
    float colorA;
    // Start is called before the first frame update
    void Start()
    {
        colorA = 1.0f;
    }

    public void JumpAnim(bool state)
	{
        animator.SetBool("IsJump", state);
	}

    public void HitAnim(bool state)
	{
        animator.SetBool("IsHit", state);
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
}

