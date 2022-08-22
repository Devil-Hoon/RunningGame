public class User
{

    public int Idx { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public int Score { get; set; }
    public int Item1 { get; set; }
    public int Item2 { get; set; }
    public int Item3 { get; set; }
    public string ItemName1 { get; set; }
    public string ItemName2 { get; set; }
    public string ItemName3 { get; set; }
    public string Status { get; set;  }
    public bool anonymous { get; set; }

    public User()
    {

    }
    public User(int idx, string userId, string name, int score, int item1, int item2, int item3)
    {
        Idx = idx;
        UserId = userId;
        Name = name;
        Score = score;
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
    }

    public User(int idx, string userId, string name, string status, int score, int item1, int item2, int item3, string itemName1, string itemName2, string itemName3)
    {
        Idx = idx;
        UserId = userId;
        Name = name;
        Status = status;
        Score = score;
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
        ItemName1 = itemName1;
        ItemName2 = itemName2;
        ItemName3 = itemName3;
    }

}