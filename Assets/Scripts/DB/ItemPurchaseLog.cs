public class ItemPurchaseLog
{
    public int Idx { get; set; }
    public int ItemIdx { get; set; }
    public int UserIdx { get; set; }
    public string Action { get; set; }

    public ItemPurchaseLog()
    {

    }

    public ItemPurchaseLog(int idx, int itemIdx, int userIdx, string action)
    {
        Idx = idx;
        ItemIdx = itemIdx;
        UserIdx = userIdx;
        Action = action;
    }


}
