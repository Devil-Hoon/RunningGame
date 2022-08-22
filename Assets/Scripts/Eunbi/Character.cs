using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
  public Text nameText;
  public string name;
  public int speed;
  public int addSpeed;
  public int addHp;
  public int Hp;

  private void Start()
  {
    name = Database.nickname;
  }

  public void ItemSelect(Item item)
  {
    addSpeed = int.Parse(item.Number);
    addHp = int.Parse(item.Number);

    View();
  }

  public void View()
  {
    
  }
}
