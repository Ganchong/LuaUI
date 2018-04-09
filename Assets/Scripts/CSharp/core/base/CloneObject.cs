using System;
/// <summary>
/// 克隆对象
/// 汪松民
/// </summary>
public class CloneObject : ICloneable
{
	/** copy方法 */
	public virtual object copy (object obj)
	{
		return obj;
	}
	/* common methods */
	public object Clone ()
	{
		return copy (base.MemberwiseClone ());
	}
}