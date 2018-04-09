using System;
/// <summary>
/// 矩形区域
/// </summary>
public struct Rectangle
{
	/* fields */
	/** x */
	public float x;
	/** y */
	public float y;
	/** 宽 */
	public float width;
	/** 高 */
	public float height;

	/* constructor */
	/** 初始化方法 */
	public Rectangle (float x, float y, float width, float height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
	}

	/* methods */
	/** 是否重叠 */
	public static bool isHit (Rectangle r1, Rectangle r2)
	{
		return isWidthHit (r1, r2) && isHeightHit (r1, r2);
	}
	/** 宽相交 */
	public static bool isWidthHit (Rectangle r1, Rectangle r2)
	{
		return Math.Abs (r1.x - r2.x) <= (r1.width + r2.width) * 0.5;
	}
	/** 高相交 */
	public static bool isHeightHit (Rectangle r1, Rectangle r2)
	{
		return Math.Abs (r1.y - r2.y) <= (r1.height + r2.height) * 0.5;
	}

	/* common methods */
	/** 字符串 */
	public override string ToString ()
	{
		return "x=" + x + ",y=" + y + ",width=" + width + ",height=" + height;
	}
}