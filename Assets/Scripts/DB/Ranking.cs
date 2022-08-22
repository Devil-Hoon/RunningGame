public class Ranking
{
    public int Idx { get; set; }
    public int Num { get; set; }
    public int UserIdx { get; set; }
    public string Name { get; set; }
    public int Score { get; set; }

    public Ranking()
    {

    }

    public Ranking(int num, int idx, int userIdx, string name, int score)
    {
        Num = num;
        Idx = idx;
        UserIdx = userIdx;
        Name = name;
        Score = score;
    }


}
