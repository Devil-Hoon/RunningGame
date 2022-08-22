using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum ItemOption
{
    Invincible,
    HighJump,
    FastRun,
    Shield,
    DashJump,
    ResistDash,
    None
}
[System.Serializable]
public class Item
{
  public Item(string _Type, string _Name, string _Explain, string _Number, bool _isUsing, string _Index, Sprite _img = null, Sprite _packImg = null)
  { Type = _Type; Name = _Name; Explain = _Explain; Number = _Number; isUsing = _isUsing; Index = _Index; img = _img; packImg = _packImg; }
    public Item() { }

    public void SetItem(Item item) { Name = item.Name; Index = item.Index; Kind = item.Kind; Speed = item.Speed; option = item.option;
        Time = item.Time; Jump = item.Jump; value1 = item.value1; value2 = item.value2; img = item.img; packImg = item.packImg;
        Explain = item.Explain;
    }
  public string Type, Name, Explain, Number, Index, Kind;
    public int Speed;
    public float Time, Jump,  value1, value2;
    public ItemOption option;
  public bool isUsing;
  public Sprite img;
    public Sprite packImg;
}

public class ItemManager : MonoBehaviour
{
  
  public TextAsset ItemDatabase;
  public List<Item> AllItemList, MyItemList, CurItemList;
  public string curType;
  public GameObject[] Slot, UsingImage;
  public Image[] TabImage, ItemImage;
  // 눌러져 있는 스프라이트와 안눌러져 있는 스프라이트
  public Sprite TabIdleSprite, TabSelectSprite;
  public Sprite[] ItemSprite;
  public GameObject ExplainPanel;
  public RectTransform[] SlotPos;
  public RectTransform CanvasRect;
  public InputField ItemNameInput, ItemNumberInput;
  IEnumerator PointerCoroutine;
  RectTransform ExplainRect;
  bool EquipITem;
  public Image[] ShowedEquipItem;

  public List<Sprite> items = new List<Sprite>();


  void Start()
  {
    Sprite[] objs = Resources.LoadAll<Sprite>("Item");

    for (int i = 0; i < objs.Length; i++)
    {
      items.Add(objs[i]);
    }
    //전체 아이템 리스트 불러오기
    string[] line = ItemDatabase.text.Substring(0, ItemDatabase.text.Length - 1).Split('\n');
    for (int i = 0; i < line.Length; i++)
    {
      string[] row = line[i].Split('\t');
      AllItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4] == "TRUE", row[5]));
    }
    
    ExplainRect = ExplainPanel.GetComponent<RectTransform>();

    Load();
  }


  private void EquipItem(Item newItem) // 새로운 아이템 장착시
  {
        //Object[] obj = Resources.LoadAll("/item");
        //List<Item> l = new List<Item>();

        //l[0].img = obj[0] as Sprite;

  }

  private void Update()
  {
    RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect, Input.mousePosition, Camera.main, out Vector2 anchoredPos);
    //ExplainRect.anchoredPosition = anchoredPos + new Vector2(-210, -165);
  }



  public void GetItemClick()
  {
    Item curItem = MyItemList.Find(x => x.Name == ItemNameInput.text);
    //ItemNumberInput.text = ItemNumberInput.text == "" ? "1" : ItemNumberInput.text;
    if (curItem != null) curItem.Number = (int.Parse(curItem.Number) + int.Parse(ItemNumberInput.text)).ToString();
    else
    {
      // 전체에서 얻을 아이템을 찾아 내 아이템에 추가
      Item curAllItem = AllItemList.Find(x => x.Name == ItemNameInput.text);
      if (curAllItem != null)
      {
        curAllItem.Number = ItemNumberInput.text;
        MyItemList.Add(curAllItem);
      }
    }
    MyItemList.Sort((p1, p2) => p1.Index.CompareTo(p2.Index));
    Save();
  }

  public void RemoveItemClick()
  {
    Item curItem = MyItemList.Find(x => x.Name == ItemNameInput.text);
    if (curItem != null)
    {
      int curNumber = int.Parse(curItem.Number) - int.Parse(ItemNumberInput.text);

      if (curNumber <= 0) MyItemList.Remove(curItem);
      else curItem.Number = curNumber.ToString();
    }
    MyItemList.Sort((p1, p2) => p1.Index.CompareTo(p2.Index));
    Save();
  }

  public void ResetItemClick()
  {
    Item BasicItem = AllItemList.Find(x => x.Name == "head1");

    MyItemList = new List<Item>() { BasicItem };
    Save();
  }


  public void SlotClick(int slotNum)
  {
    Item CurItem = CurItemList[slotNum];
    Item UsingItem = CurItemList.Find(x => x.isUsing == true);
    int num = 0;
    if (curType == "Equip")
    {
      switch (UsingItem.Kind)
      {
        case "Head":
          num = 0;
          break;
        case "Wing":
          num = 1;
          break;
        case "Shoes":
          num = 2;
          break;
        case "Chararter":
          num = 3;
          break;
        default:
          num = 99999;
          break;
      }
      if (UsingItem != null)
      {
        UsingItem.isUsing = false;
        if (num != 99999)
        {
          ShowedEquipItem[num].gameObject.SetActive(false);
        }
      }
      else
      {
        CurItem.isUsing = true;
        if (num != 99999)
        {
          ShowedEquipItem[num].gameObject.SetActive(true);
          
        }
      }
    }
    else
    {
      switch (CurItem.Kind)
      {
        case "Head":
          num = 0;
          break;
        case "Wing":
          num = 1;
          break;
        case "Shoes":
          num = 2;
          break;
        case "Chararter":
          num = 3;
          break;
        default:
          num = 99999;
          break;
      }
      CurItem.isUsing = !CurItem.isUsing;
      ShowedEquipItem[num].gameObject.SetActive(CurItem.isUsing);

      if (UsingItem != null) UsingItem.isUsing = false;
      
    }
    Save();
  }


  // 버튼누르면 해당되는 아이템리스트 불러오기
  public void TabClick(string tabName)
  {
    // 현재 아이템 리스트에 클릭한 타입만 추가
    curType = tabName;
    if (tabName == "Equip")
    {
      CurItemList = MyItemList.FindAll(item => item.isUsing);
    }
    else
    {
      CurItemList = MyItemList.FindAll(x => x.Type == tabName && !x.isUsing);
    }

    

    // 선택한 버튼의 슬롯과 텍스트만 보이기
    for (int i = 0; i < Slot.Length; i++)
    {
      bool isExist = i < CurItemList.Count;
      Slot[i].SetActive(isExist);
      Slot[i].GetComponentInChildren<Text>().text = isExist ? CurItemList[i].Name + "/" + CurItemList[i].isUsing : " ";

      if (isExist)
      {
        ItemImage[i].sprite = ItemSprite[AllItemList.FindIndex(x => x.Name == CurItemList[i].Name)];
        UsingImage[i].SetActive(CurItemList[i].isUsing);
      }


    }


    // 탭 이미지************************************************************************************************************************
    int tabNum = 0;
    switch (tabName)
    {
      case "Equip": tabNum = 0; break;
      case "Head": tabNum = 1; break;
      case "Wing": tabNum = 2; break;
      case "Shoes": tabNum = 3; break;
      case "Chararter": tabNum = 4; break;
    }
    //for (int i = 0; i < TabImage.Length; i++)
     // TabImage[i].sprite = i == tabNum ? TabSelectSprite : TabIdleSprite;

  }

  public void PointerEnter(int slotNum)
  {
    // 슬롯에 마우스를 올리면 0.5초후에 설명창 띄움
    PointerCoroutine = PointerEnterDelay(slotNum);
    StartCoroutine(PointerCoroutine);

    // 설명창에 이름, 이미지, 개수, 설명 나타내기
    ExplainPanel.GetComponentInChildren<Text>().text = CurItemList[slotNum].Name;
    ExplainPanel.transform.GetChild(2).GetComponent<Image>().sprite = Slot[slotNum].transform.GetChild(1).GetComponent<Image>().sprite;
    ExplainPanel.transform.GetChild(3).GetComponent<Text>().text = CurItemList[slotNum].Number + "개";
    ExplainPanel.transform.GetChild(4).GetComponent<Text>().text = CurItemList[slotNum].Explain;
  }

  IEnumerator PointerEnterDelay(int slotNum)
  {
    yield return new WaitForSeconds(0.5f);
    ExplainPanel.SetActive(true);
  }

  public void PointerExit(int slotNum)
  {
    StopCoroutine(PointerEnterDelay(slotNum));
    ExplainPanel.SetActive(false);
  }


  void Save()
  {
    string jdata = JsonConvert.SerializeObject(MyItemList);
    File.WriteAllText(Application.dataPath + "/Resources/MyItemText.txt", jdata);

    TabClick(curType);
  }


  void Load()
  {
    string jdata = File.ReadAllText(Application.dataPath + "/Resources/MyItemText.txt");
    MyItemList = JsonConvert.DeserializeObject<List<Item>>(jdata);

    for (int i = 0; i < MyItemList.Count; i++)
    {
      if (MyItemList[i].isUsing)
      {
        switch (MyItemList[i].Kind)
        {
          case "Head":
            ShowedEquipItem[0].gameObject.SetActive(true);
            break;
          case "Wing":
            ShowedEquipItem[1].gameObject.SetActive(true);
           break;
         case "Shoes":
           ShowedEquipItem[2].gameObject.SetActive(true);
           break;
         case "Chararter":
           ShowedEquipItem[3].gameObject.SetActive(true);
           break;
         default:
           break;
        }
      }
    }
    TabClick(curType);
  }
}
