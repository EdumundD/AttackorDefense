//
// @brief: Point类
// @version: 1.0.0
// @author lhy
// @date: 2020/2/15
// 
// 
//


public class Point
    {
    public Point ParentPoint { get; set; }
    public int F { get; set; }  //F=G+H
    public int G { get; set; }
    public int H { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
    public void CalcF()
    {
        this.F = this.G + this.H;
    }
}



