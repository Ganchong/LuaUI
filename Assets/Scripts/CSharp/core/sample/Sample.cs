using System;
using System.Collections.Generic;
/// <summary>
/// 模板
/// 汪松民
/// </summary>
public class Sample:ICloneable
{
	/* fields */
	/** 源对象 */
	Object source;
	/** 模板sid */
	int sid;

	/* constructors */
	/** 构造方法 */
	protected Sample ()
	{
	}
	/** methods */
	/** 初始化 */
	public virtual void initSample(){}
	/** 序列化方法 */
	public virtual object bytesRead(ByteBuffer data)
	{
		return this;
	}
	/* properties */
	/** 获得源对象 */
	public object Source {
		get {
			return source;
		}
		set {
			source = value;
		}
	}
	public int Sid {
		get {
			return sid;
		}
		set
		{
			this.sid = value;
		}
	}
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

	public override string ToString ()
	{
		return base.ToString () + "[sid=" + sid + "]";
	}
}

