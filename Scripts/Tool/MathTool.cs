using UnityEngine;
using System.Collections;

public class MathTool{

	/// <summary>
	/// 获取b点相对于a点的角度，也就是说a点加上这角度就会指向b点。
	/// </summary>
	/// <returns>The angle.</returns>
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	private float GetAngle(Vector3 a, Vector3 b) 
	{
		b.x -= a.x;
		b.z -= a.z;
		float deltaAngle = 0;
		if (b.x == 0 && b.z == 0) {
			return 0;
		} else if (b.x > 0 && b.z > 0) {
			deltaAngle = 0;
		} else if (b.x > 0 && b.z == 0) {
			return 90;
		} else if (b.x > 0 && b.z < 0) {
			deltaAngle = 180;
		} else if (b.x == 0 && b.z < 0) {
			return 180;
		} else if (b.x < 0 && b.z < 0) {
			deltaAngle = -180;
		} else if (b.x < 0 && b.z == 0) {
			return -90;
		} else if (b.x < 0 && b.z > 0) {
			deltaAngle = 0;
		}
		float angle = Mathf.Atan(b.x / b.z) * Mathf.Rad2Deg + deltaAngle;
		return angle;
	}
}
