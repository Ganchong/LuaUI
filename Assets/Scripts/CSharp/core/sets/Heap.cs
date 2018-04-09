using System;
/// <summary>
/// 堆
/// 汪松民
/// </summary>
public class Heap<E> : Container
{
	
	/* static fields */
	/** 默认的初始容量大小 */
	public const int CAPACITY=10;
	
	/* fields */
	/** 队列的对象数组 */
	private object[] array;
	/** 队列的长度 */
	private int _size;
	/** 对象比较器 */
	private Comparator comparator;
	/** 由升降序决定的比较参数 */
	private int comp;
	
	/* constructors */
	/** 按指定的比较器构造一个优先队列，默认为升序 */
	public Heap(Comparator comparator):this(CAPACITY,comparator,false) {}
	/** 按指定的大小和比较器构造一个优先队列，默认为升序 */
	public Heap(int capacity,Comparator comparator):this(capacity,comparator,false){}
	/** 按指定的大小和比较器及升降序构造一个优先队列 */
	public Heap(int capacity,Comparator comparator,bool descending)
	{
		if(capacity<1)
			throw new Exception(GetType().Name +" <init>, invalid capacity:"+capacity);
		if(comparator==null)
			throw new Exception(GetType().Name +" <init>, null comparator");
		array=new object[capacity];
		this.comparator=comparator;
		comp=descending?ComparatorKit.COMP_LESS:ComparatorKit.COMP_GRTR;
	}
	/* properties */
	/** 获得队列的长度 */
	public int size()
	{
		return _size;
	}
	/** 获得队列的容积 */
	public int capacity()
	{
		return array.Length;
	}
	/** 判断队列是否为空 */
	public bool isEmpty()
	{
		return _size<=0;
	}
	/** 判断队列是否已满 */
	public bool isFull()
	{
		return false;
	}
	/** 获得队列的对象比较器 */
	public Comparator getComparator()
	{
		return comparator;
	}
	/** 判断队列是否为降序 */
	public bool isDescending()
	{
		return comp==ComparatorKit.COMP_LESS;
	}
	/** 获得队列的对象数组 */
	public object[] getArray()
	{
		return array;
	}
	/* methods */
	/** 设置列表的容积，只能扩大容积 */
	public void setCapacity(int len)
	{
		object[] array=this.array;
		int c=array.Length;
		if(len<=c) return;
		for(;c<len;c=(c<<1)+1)
			;
		object[] temp=new object[c];
		Array.Copy(array,0,temp,0,_size);
		this.array=temp;
	}
	/** 判断对象是否在容器中 */
	public bool contain(object obj)
	{
		if(obj!=null)
		{
			for(int i=0;i<_size;i++)
			{
				if(obj.Equals(array[i])) return true;
			}
		}
		else
		{
			for(int i=0;i<_size;i++)
			{
				if(array[i]==null) return true;
			}
		}
		return false;
	}
	/** 将对象放入到队列中 */
	public bool add(E obj)
	{
		if(_size>=array.Length) setCapacity(_size+1);
		int i=_size++;
		// 获得堆中指定节点的父节点
		int j=(i-1)/2;
		while(i>0&&(comparator.compare(obj,(E)array[j])==comp))
		{
			array[i]=array[j];
			i=j;
			j=(i-1)/2;
		}
		array[i]=obj;
		return true;
	}
	/** 检索队列中的第一个对象 */
	public E get()
	{
		return (E)array[0];
	}
	/** 从队列中弹出第一个的对象 */
	public E remove()
	{
		object obj=array[0];
		array[0]=array[--_size];
		array[_size]=null;
		if(_size>0) heapify(0);
		return (E)obj;
	}
	/** 整堆方法，将指定位置的对象向下整理到堆中正确的位置，递归调用 */
	private void heapify(int i)
	{
		// 获得堆中指定节点的左节点
		int l=2*i+1;
		// 获得堆中指定节点的右节点
		int r=2*i+2;
		int j=i;
		if((l<_size)&&(comparator.compare((E)array[l],(E)array[j])==comp))
			j=l;
		if((r<_size)&&(comparator.compare((E)array[r],(E)array[j])==comp))
			j=r;
		if(j==i) return;
		object obj=array[i];
		array[i]=array[j];
		array[j]=obj;
		heapify(j);
	}
	/** 清除队列 */
	public void clear()
	{
		for(int i=0;i<_size;i++)
			array[i]=null;
		_size=0;
	}
	/* common methods */
	public override String ToString()
	{
		return base.ToString()+"[_size="+_size+", capacity="+array.Length+"]";
	}
}
