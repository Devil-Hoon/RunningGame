using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    bool isJump = false;
    bool isTop = false;
    bool isInvincible = false;
    float invincibleCount = 0;
    public float jumpHeight = 0;
    public float jumpSpeed = 0;
    bool isHit;

    public int itemSpeed = 0;
    public float itemJump = 0;
    public float itemRecovery = 1;
    public float itemShield = 1;
    public float itemTime = 0;
    bool isBlinkHigh;
    [SerializeField]
    BodyParts[] bodys;
    [SerializeField]
    Animator animator;

    public ItemOption option = ItemOption.None;

    [SerializeField]
    SpriteRenderer rdr;
    [SerializeField]
    GameObject soil;
    public ItemParts hat;
    public ItemParts back;
    public ItemParts shoes1;
    public ItemParts shoes2;
    public ItemParts clothes;
    float colorA = 1;
    Vector2 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        colorA = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
		if (isInvincible && !GameManager.instance.boosterOn && option != ItemOption.Invincible)
		{
            invincibleCount += Time.deltaTime;
			if (invincibleCount > 1.5f)
			{
                InvincibleOff();
            }
		}

		if (itemTime > 0)
		{
            itemTime -= Time.deltaTime;
			if (itemTime <= 0)
			{
                ItemStatusClean();
			}
		}
		if (isJump)
		{
			if (transform.position.y <= (jumpHeight + itemJump) - 0.1f && !isTop)
            {
                transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, (jumpHeight + itemJump)), jumpSpeed * Time.deltaTime);
            }
			else
			{
                isTop = true;
			}
			if (transform.position.y > startPosition.y &&isTop)
			{
                transform.position = Vector2.MoveTowards(transform.position, startPosition, jumpSpeed * Time.deltaTime);
			}
		}

        if (transform.position.y <= startPosition.y)
        {
            isJump = false;
            isTop = false;
            transform.position = startPosition;
			if (animator.GetBool("IsJump"))
			{
                animator.SetBool("IsJump", false);
				for (int i = 0; i < bodys.Length; i++)
				{
                    bodys[i].JumpAnim(false);
				}
            }
        }
    }

    private void ItemStatusClean()
	{
        itemJump = 0;
        itemRecovery = 1;
        itemShield = 1;
        itemSpeed = 0;
        itemTime = 0;
        option = ItemOption.None;
        hat.Clean();
        back.Clean();
        shoes1.Clean();
        shoes2.Clean();
        clothes.Clean();
		if (isInvincible)
		{
            InvincibleOff();
		}
    }
    void IsHit()
    {
		if (option != ItemOption.Invincible && option != ItemOption.ResistDash)
		{
            ItemStatusClean();
		}
        animator.SetBool("IsHit", true);
        for (int i = 0; i < bodys.Length; i++)
        {
            bodys[i].HitAnim(true);
        }

        GameManager.instance.isPause = true;
        GameManager.instance.lifeBar.lifeBar.value -= (10 * itemShield);
        GameManager.instance.boosterBtn.interactable = false;
        GameManager.instance.jumpBtn.interactable = false;

        isHit = true;
    }
    public void SoilOn()
	{
        soil.SetActive(true);
	}
    public void SoilOff()
    {
        soil.SetActive(false);
    }
    public void HitEnd()
	{
        GameManager.instance.isPause = false;
        InvincibleOn();
        isHit = false;

        animator.SetBool("IsHit", false);
        for (int i = 0; i < bodys.Length; i++)
        {
            bodys[i].HitAnim(false);
        }
        GameManager.instance.boosterBtn.interactable = true;
        GameManager.instance.jumpBtn.interactable = true;
	}
    public void IsJump()
	{
		if (GameManager.instance.isPlay && !isHit)
		{
            isJump = true;
            animator.SetBool("IsJump",true );
            for (int i = 0; i < bodys.Length; i++)
            {
                bodys[i].JumpAnim(true);
            }
        }
    }
    public void InvincibleOn()
    {
        invincibleCount = 0;
        isInvincible = true;
        for (int i = 0; i < bodys.Length; i++)
        {
            bodys[i].InvincibleOn();
        }
        hat.InvincibleOn();
        back.InvincibleOn();
        shoes1.InvincibleOn();
        shoes2.InvincibleOn();
        clothes.InvincibleOn();
        StopCoroutine("InvincibleBlink");
        StartCoroutine("InvincibleBlink");
    }
    public void InvincibleOff()
    {
        invincibleCount = 0;
        isInvincible = false;
        StopCoroutine("InvincibleBlink");
        for (int i = 0; i < bodys.Length; i++)
        {
            bodys[i].InvincibleOff();
        }
        rdr.color = new Color(rdr.color.r, rdr.color.g, rdr.color.b, 1);

        hat.InvincibleOff();
        back.InvincibleOff();
        shoes1.InvincibleOff();
        shoes2.InvincibleOff();
        clothes.InvincibleOff();
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
    private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.CompareTag("Mouse") && !isInvincible)
        {
            if (!collision.GetComponent<Mouse>().crash)
            {
                GameManager.instance.score += 50;
                collision.GetComponent<Animator>().SetTrigger("IsDie");
            }
        }
        else if (collision.CompareTag("Obstacle") && !isInvincible && !isHit)
        {
            IsHit();
            if (collision.name == "Bird(Clone)")
            {
                collision.GetComponentInChildren<Animator>().SetTrigger("isHit");
            }

            if (collision.name == "Obstacle")
            {
                Transform parent = collision.transform.parent;
                parent.GetComponent<Mouse>().crash = true;
            }
        }
        else if (collision.CompareTag("SilverCoin"))
		{
            GameManager.instance.coin += 5;

            if (!GameManager.instance.boosterOn)
            {
                GameManager.instance.booster++;
                
                if (GameManager.instance.booster >= GameManager.instance.boosterGage.gage.maxValue)
                {
                    GameManager.instance.booster = (int)GameManager.instance.boosterGage.gage.maxValue;
                    GameManager.instance.boosterGage.EffectOn();
                }

                GameManager.instance.boosterGage.SetTimeBarValue(GameManager.instance.booster);
            }
            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Coin"))
        {
            GameManager.instance.coin += 10;

            if (!GameManager.instance.boosterOn)
            {
                GameManager.instance.booster += 2;
                if (GameManager.instance.booster >= GameManager.instance.boosterGage.gage.maxValue)
                {
                    GameManager.instance.booster = (int)GameManager.instance.boosterGage.gage.maxValue;
                    GameManager.instance.boosterGage.EffectOn();
                }

                GameManager.instance.boosterGage.SetTimeBarValue(GameManager.instance.booster);
            }
            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("StarCoin"))
		{
            GameManager.instance.coin += 50;

            if (!GameManager.instance.boosterOn)
            {
                GameManager.instance.booster+=5;
                if (GameManager.instance.booster >= GameManager.instance.boosterGage.gage.maxValue)
                {
                    GameManager.instance.booster = (int)GameManager.instance.boosterGage.gage.maxValue;
                    GameManager.instance.boosterGage.EffectOn();
                }

                GameManager.instance.boosterGage.SetTimeBarValue(GameManager.instance.booster);
            }
            collision.gameObject.SetActive(false);
        }
		else if (collision.CompareTag("Heal"))
		{
            GameManager.instance.lifeBar.lifeBar.value += 20;
            collision.gameObject.SetActive(false);
		}
		else if (collision.CompareTag("Equip"))
		{
            EquipItem item = collision.GetComponent<EquipItem>();
            itemTime = item.item.Time;

			switch (item.item.Kind)
			{
                case "head":
                    hat.Init(item.item);
                    break;
                case "back":
                    back.Init(item.item);
                    break;
                case "shoes":
                    shoes1.Init(item.item);
                    shoes2.Init(item.item);
                    break;
                case "clothes":
                    clothes.Init(item.item);
                    break;
				default:
					break;
			}
			switch (item.item.option)
			{
				case ItemOption.Invincible:
                    option = item.item.option;
                    InvincibleOn();
					break;
				case ItemOption.HighJump:
                    option = item.item.option;
                    itemJump = item.item.value1;
                    break;
				case ItemOption.FastRun:
                    option = item.item.option;
                    itemSpeed = (int)item.item.value1;
                    break;
				case ItemOption.Shield:
                    option = item.item.option;
                    itemRecovery = item.item.value1;
                    break;
				case ItemOption.DashJump:
                    option = item.item.option;
                    itemJump = item.item.value1;
                    itemSpeed = (int)item.item.value2;
                    break;
				case ItemOption.ResistDash:
                    option = item.item.option;
                    itemSpeed = item.item.Speed;
                    itemShield = item.item.value2;
                    break;
				case ItemOption.None:
                    option = item.item.option;
                    break;
				default:
					break;
			}
		}
	}
}
