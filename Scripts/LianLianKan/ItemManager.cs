using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour {
	/**item预设*/
	public GameObject itemPrefab;
	/**行号*/
	public int row = 12;
	/**列号*/
	public int col = 8;
	/**item类型列表*/
	private List<int> typeList;
	/**item列表*/
	private List<List<Item>> itemList;

	//public UILabel scoreTxt;
	// Use this for initialization
	void Start () {
		typeList = new List<int>();
		for(int i = 0; i < row * col * 0.5; i++)
		{
			int type = Random.Range(1,6);
			typeList.Add(type);
			typeList.Add(type);
		}
		RandomSort1 (typeList);
		createItems ();

		//scoreTxt.text = "得分:" + _score;
	}

	private void RandomSort1(List<int> list)
	{
		int count = list.Count;
		for(int i = 0; i < count - 1; i++)
		{
			int r = Random.Range(i + 1,count - 1);
			swap(list, i, r);
		} 
	}

	private void swap(List<int> list, int i, int j)
	{
		
		int tmp = (int)list[i];
		list[i] = list[j];
		list[j] = tmp;
	}
	
	/**创建item*/
	private void createItems()
	{
		int index = 0;
		itemList= new List<List<Item>>();
		for (int i = 0; i < row; i++) 
		{
			List<Item> tmp = new List<Item>();
			for(int j = 0; j < col; j++)
			{
                GameObject item = Instantiate(itemPrefab) as GameObject;
				item.transform.localPosition = new Vector3(-245 + j * 70,320 - i * 70,0);
				int type = (int)typeList[index];
				Item itemScript = item.GetComponent<Item>();
				itemScript.SetItemType(type);
				itemScript.itemManager = this;
				itemScript.pos = new LPoint(i,j);
				itemScript.hasItem = 1;
				tmp.Add(itemScript);
				index++;
			}
			itemList.Add(tmp);
		}
	}

    private List<LPoint> pointList = new List<LPoint>();

	private List<Item> clickList = new List<Item> ();
	public void ClickItem(Item item)
	{
		clickList.Add (item);
		if (clickList.Count == 2) {
			Item item1 = clickList [0];
			Item item2 = clickList [1];
			if (item1.itemType == item2.itemType) {
				print ("一样可以消除了");
				bool isClear = checkLink (item1.pos, item2.pos);
				if (isClear) {
					hideTwoItem ();
				}
				else
				{
					clickList.Clear ();
				}
			} else {
				clickList.Clear ();
				print ("不一样清空" + clickList.Count);
			}
		} else {
			print("怎么了" + clickList.Count);
		}
	}
	private int _score;
	private void hideTwoItem()
	{
		Item item1 = clickList[0];
		Item item2 = clickList[1];
		item1.hasItem = 0;
		item2.hasItem = 0;
		_score++;
		//scoreTxt.text = "得分:" + _score;
		//item1.fly (scoreTxt.transform.localPosition);
		//item2.fly (scoreTxt.transform.localPosition);
		//Destroy (item1.gameObject);
		//Destroy (item2.gameObject);
		clickList.Clear ();
		pointList.Clear ();
	}
	
	/**横向*/
    private bool horizon(LPoint a, LPoint b) 
	{
		if (a.x == b.x && a.y == b.y) return false;  //如果点击的是同一个图案，直接返回false;
		int x_start = a.y < b.y ? a.y : b.y;        //获取a,b中较小的y值
		int x_end = a.y < b.y ? b.y : a.y;          //获取a,b中较大的值
		//遍历a,b之间是否通路，如果一个不是就返回false;
		for (int i = x_start + 1; i < x_end;i ++ ) 
		{
			if (itemList[a.x][i].hasItem == 1) //是否有item
			{
				return false;
			}
		}
		return true;
	}
	
	/**纵向*/
    private bool vertical(LPoint a, LPoint b) 
	{
		if (a.x == b.x && a.y == b.y) return false;
		int y_start = a.x < b.x ? a.x : b.x;
		int y_end = a.x < b.x ? b.x : a.x;
		for (int i = y_start + 1; i < y_end; i ++ ) 
		{
			if (itemList[i][a.y].hasItem == 1) 
			{
				return false;
			}
		}
		return true;
	}
	
	/**一条折线*/
    private bool oneCorner(LPoint a, LPoint b) 
	{
		pointList.Clear();
        LPoint c = new LPoint(b.x, a.y);
        LPoint d = new LPoint(a.x, b.y);
		//判断C点是否有元素                
		if (itemList[c.x][c.y].hasItem == 0) 
		{
			bool path1 = horizon(b, c) && vertical(a, c);
			if(path1)
			{
				pointList.Add(a);
				pointList.Add(c);
				pointList.Add(b);
			}
			return path1;
		}
		//判断D点是否有元素
		if (itemList[d.x][d.y].hasItem == 0) 
		{
			bool path2 = horizon(a, d) && vertical(b, d);
			if(path2)
			{
				pointList.Add(a);
				pointList.Add(d);
				pointList.Add(b);
			}
			return path2;
		}else 
		{
			return false;
		}
		
	}
	
	/**两条折线*/
    private bool twoCorner(LPoint a, LPoint b) 
	{
		pointList.Clear();
		List<Line> ll = scan(a, b);
		if (ll.Count == 0) 
		{
			return false;
		}
		for (int i = 0; i < ll.Count; i ++ ) 
		{
			Line tmpLine = ll[i];
			if (tmpLine.direct == 1) 
			{
				
				if (vertical(a,tmpLine.a) && vertical(b,tmpLine.b)) 
				{
					pointList.Add(a);
					pointList.Add(tmpLine.a);
					pointList.Add(tmpLine.b);
					pointList.Add(b);
					return true;
				}
			}else if (tmpLine.direct == 0) 
			{
				if (horizon(a, tmpLine.a) && horizon(b, tmpLine.b)) 
				{
					pointList.Add(a);
					pointList.Add(tmpLine.a);
					pointList.Add(tmpLine.b);
					pointList.Add(b);
					return true;
				}
			}
		}
		return false;
	}

    private List<Line> scan(LPoint a, LPoint b) 
	{
		List<Line> linkList = new List<Line>();
		//检测a点,b点的左侧是否能够垂直直连
		for (int i = a.y; i >= 0; i -- ) 
		{
            if (itemList[a.x][i].hasItem == 0 && itemList[b.x][i].hasItem == 0 && vertical(new LPoint(a.x, i), new LPoint(b.x, i))) 
			{
                linkList.Add(new Line(new LPoint(a.x, i), new LPoint(b.x, i), 0));
			}
		}
		//检测a点,b点的右侧是否能够垂直直连
		for (int i = a.y; i < col;i ++ ) 
		{
            if (itemList[a.x][i].hasItem == 0 && itemList[b.x][i].hasItem == 0 && vertical(new LPoint(a.x, i), new LPoint(b.x, i))) 
			{
                linkList.Add(new Line(new LPoint(a.x, i), new LPoint(b.x, i), 0));
			}
		}
		//检测a点,b点的上侧是否能够水平直连
		for (int j = a.x; j >= 0; j -- ) 
		{
            if (itemList[j][a.y].hasItem == 0 && itemList[j][b.y].hasItem == 0 && horizon(new LPoint(j, a.y), new LPoint(j, b.y))) 
			{
                linkList.Add(new Line(new LPoint(j, a.y), new LPoint(j, b.y), 1));
			}
		}
		//检测a点,b点的下侧是否能够水平直连
		for (int j = a.x; j < row; j ++ ) 
		{
            if (itemList[j][a.y].hasItem == 0 && itemList[j][b.y].hasItem == 0 && horizon(new LPoint(j, a.y), new LPoint(j, b.y))) 
			{
                linkList.Add(new Line(new LPoint(j, a.y), new LPoint(j, b.y), 1));
			}
		}
		
		return linkList;
	}
	
	
	//总函数
    private bool checkLink(LPoint a, LPoint b) 
	{
		if (a.x == b.x && horizon(a, b)) 
		{
			pointList.Add(a);
			pointList.Add(b);
			return true; 
		} 
		if (a.y == b.y && vertical(a, b))
		{
			pointList.Add(a);
			pointList.Add(b);
			return true;
		}  
		if (oneCorner(a, b))
		{
			return true;  
		}
		else 
		{
			return twoCorner(a, b);
		}
	}

	public void clickBg()
	{
		print ("点击背景了");
	}

}
