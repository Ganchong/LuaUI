using System;
using System.Collections.Generic;

/// <summary>
/// 对象池
/// </summary>
public class ObjectPool<T> where T : IPoolObject<T>,new()
{
	/* static fields */
	/** 最大对象数量 */
	public const int MAXNUM=100;

	/* fields */
	/** 未使用列表 */
	protected Queue<T> unuseQueue=new Queue<T>();
	/** 最大对象池 */
	protected int maxNum=MAXNUM;

	/* constructors */
	/** 构造方法 */
	public ObjectPool()
	{
	}
	/** 构造方法,指定池子大小 */
	public ObjectPool(int maxNum)
	{
		this.maxNum = maxNum;
	}

	/* properties */
	/** 数量 */
	public int Count
	{
		get
		{ 
			return unuseQueue.Count;
		}
	}

	/* methods */
	/** 回收 */
	public void Recycle(IPoolObject<T> obj)
	{
		if (obj != null&&unuseQueue.Count<maxNum) 
		{
			lock (unuseQueue) 
			{
				if(unuseQueue.Count<maxNum)
					unuseQueue.Enqueue (obj.Cast ());
			}
		}
	}
	/** 获取 */
	public T Get()
	{
		lock (unuseQueue) 
		{
			if (unuseQueue.Count <= 0)
				return new T();
			return unuseQueue.Dequeue ();
		}
	}
}