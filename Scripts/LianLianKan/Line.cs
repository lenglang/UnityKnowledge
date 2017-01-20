using UnityEngine;
using System.Collections;

public class Line {

    public LPoint a;
    public LPoint b;
	/**1:水平直连 0:垂直直连*/
	public int direct;

    public Line(LPoint aa, LPoint bb, int dir)
	{
		a = aa;
		b = bb;
		direct = dir;
	}

}
