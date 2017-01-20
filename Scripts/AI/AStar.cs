using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridType
{
    Normal,//正常
    Obstacle,//障碍物
    Start,//起点
    End//终点
}

//为了格子排序 需要继承IComparable接口实现排序
public class MapGrid : IComparable//排序接口
{
    public int x;//记录坐标
    public int y;

    public int f;//总消耗
    public int g;//当前点到起点的消耗
    public int h;//当前点到终点的消耗


    public GridType type;//格子类型
    public MapGrid fatherNode;//父节点


    //排序
    public int CompareTo(object obj)	 //排序比较方法 ICloneable的方法
    {
        //升序排序
        MapGrid grid = (MapGrid)obj;
        if (this.f < grid.f)
        {
            return -1;					//升序
        }
        if (this.f > grid.f)
        {
            return 1;					//降序
        }
        return 0;
    }

}




public class AStar : MonoBehaviour
{
    //格子大小
    public int row = 5;
    public int col = 10;
    public int size = 70;				//格子大小

    public MapGrid[,] grids;			//格子数组

    public ArrayList openList;			//开启列表
    public ArrayList closeList;			//结束列表

    //开始,结束点位置
    private int xStart = 2;
    private int yStart = 1;

    private int xEnd = 2;
    private int yEnd = 5;
    private Stack<string> fatherNodeLocation;

    void Init()
    {
        grids = new MapGrid[row, col];	//初始化数组
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                grids[i, j] = new MapGrid();
                grids[i, j].x = i;
                grids[i, j].y = j;		//初始化格子,记录格子坐标
            }
        }
        grids[xStart, yStart].type = GridType.Start;
        grids[xStart, yStart].h = Manhattan(xStart, yStart);	//起点的 h 值

        grids[xEnd, yEnd].type = GridType.End;					//结束点
        fatherNodeLocation = new Stack<string>();

        //生成障碍物
        for (int i = 1; i <= 3; i++)
        {
            grids[i, 3].type = GridType.Obstacle;
        }

        openList = new ArrayList();
        openList.Add(grids[xStart, yStart]);
        closeList = new ArrayList();
    }

    int Manhattan(int x, int y)					//计算算法中的 h
    {
        return (int)(Mathf.Abs(xEnd - x) + Mathf.Abs(yEnd - y)) * 10;
    }


    // Use this for initialization
    void Start()
    {
        Init();
    }

    void DrawGrid()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Color color = Color.yellow;
                if (grids[i, j].type == GridType.Start)
                {
                    color = Color.green;
                }
                else if (grids[i, j].type == GridType.End)
                {
                    color = Color.red;
                }
                else if (grids[i, j].type == GridType.Obstacle)	//障碍颜色
                {
                    color = Color.blue;
                }
                else if (closeList.Contains(grids[i, j]))		//关闭列表颜色  如果当前点包含在closList里
                {
                    color = Color.yellow;
                }
                else { color = Color.gray; }

                GUI.backgroundColor = color;
                GUI.Button(new Rect(j * size, i * size, size, size), FGH(grids[i, j]));
            }
        }
    }

    //每个格子显示的内容
    string FGH(MapGrid grid)
    {
        string str = "F" + grid.f + "\n";
        str += "G" + grid.g + "\n";
        str += "H" + grid.h + "\n";
        str += "(" + grid.x + "," + grid.y + ")";
        return str;
    }
    void OnGUI()
    {
        DrawGrid();
        for (int i = 0; i < openList.Count; i++)
        {
            //生成一个空行,存放开启数组
            GUI.Button(new Rect(i * size, (row + 1) * size, size, size), FGH((MapGrid)openList[i]));
        }
        //生成一个空行,存放关闭数组
        for (int j = 0; j < closeList.Count; j++)
        {
            GUI.Button(new Rect(j * size, (row + 2) * size, size, size), FGH((MapGrid)closeList[j]));
        }

        if (GUI.Button(new Rect(col * size, size, size, size), "next"))
        {
            NextStep();//点击到下一步
        }
    }

    void NextStep()
    {
        if (openList.Count == 0)				//没有可走的点
        {
            print("Over !");
            return;
        }
        MapGrid grid = (MapGrid)openList[0];	//取出openList数组中的第一个点
        if (grid.type == GridType.End)			//找到终点
        {
            print("Find");
            FindFatherNode(grid);				//找到父节点,打印路线
            return;
        }

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (!(i == 0 && j == 0))
                {
                    int x = grid.x + i;
                    int y = grid.y + j;
                    //x,y不超过边界,不是障碍物,不在closList里面
                    if (x >= 0 && x < row && y >= 0 && y < col && grids[x, y].type != GridType.Obstacle && !closeList.Contains(grids[x, y]))
                    {


                        //到起点的消耗
                        int g = grid.g + (int)(Mathf.Sqrt((Mathf.Abs(i) + Mathf.Abs(j))) * 10);
                        if (grids[x, y].g == 0 || grids[x, y].g > g)
                        //如果g=0(新点)或者  旧值g值大于新值 则用新值替换旧值
                        {
                            grids[x, y].g = g;
                            grids[x, y].fatherNode = grid;		//更新父节点
                        }
                        //到终点的消耗
                        grids[x, y].h = Manhattan(x, y);
                        grids[x, y].f = grids[x, y].g + grids[x, y].h;
                        if (!openList.Contains(grids[x, y]))
                        {
                            openList.Add(grids[x, y]);			//如果没有则加入到openlist
                        }
                        openList.Sort();						//排序
                    }
                }
            }
        }
        //添加到关闭数组
        closeList.Add(grid);
        //从open数组删除
        openList.Remove(grid);
    }


    //回溯法 递归父节点
    void FindFatherNode(MapGrid grid)
    {
        if (grid.fatherNode != null)
        {
            //print(grid.fatherNode.x + "," + grid.fatherNode.y);	//逆序打印
            string str = "(" + grid.fatherNode.x + "," + grid.fatherNode.y + ")";
            fatherNodeLocation.Push(str);
            //此处的结构体只是为调节打印次序,用下面的print可以直接实现
            FindFatherNode(grid.fatherNode);
            //print(grid.fatherNode.x + "," + grid.fatherNode.y);	//当所有的父节点均遍历完了再打印(顺序打印)
        }
        if (fatherNodeLocation.Count != 0)
        {
            print(fatherNodeLocation.Pop());
        }
    }
}
