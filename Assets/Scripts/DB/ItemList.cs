public class ItemList
{
    public int Idx { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public string Effect { get; set; }
    public int Price { get; set; }
    public string Parts { get; set; }

    public ItemList()
    {

    }

    public ItemList(int idx, string name, string content, string effect, int price, string parts)
    {
        Idx = idx;
        Name = name;
        Content = content;
        Effect = effect;
        Price = price;
        Parts = parts;
    }


}
