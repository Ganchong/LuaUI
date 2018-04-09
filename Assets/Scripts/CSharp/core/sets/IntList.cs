using System;
/**
 * @author 汪松民
 */
public class IntList : ICloneable
{
	
	/* static fields */
	/** 默认的初始容量大小 */
	public const int CAPACITY=10;
	
	/* fields */
	/** 整型数组 */
	int[] array;
	/** 整型列表的长度 */
	int size;
	
	/* constructors */
	/** 按默认的大小构造一个整型列表 */
	public IntList():this(CAPACITY)
	{
	}
	/** 按指定的大小构造一个整型列表 */
	public IntList(int capacity)
	{
		if(capacity<1)
			throw new Exception(GetType().Name
			                                   +" <init>, invalid capatity:"+capacity);
		array=new int[capacity];
		size=0;
	}
	/** 用指定的整型数组构造一个整型列表 */
	public IntList(int[] array):this(array,(array!=null)?array.Length:0)
	{
	}
	/**
	 * 用指定的整型数组及长度构造一个整型列表， 指定长度不能超过整型数组的长度，
	 */
	public IntList(int[] array,int len)
	{
		if(array==null)
			throw new Exception(GetType().Name
			                                   +" <init>, null array");
		if(len>array.Length)
			throw new Exception(GetType().Name
			                                   +" <init>, invalid length:"+len);
		this.array=array;
		this.size=len;
	}
	/* properties */
	/** 得到整型列表的长度 */
	public int getSize()
	{
		return size;
	}
	/** 得到整型列表的容积 */
	public int capacity()
	{
		return array.Length;
	}
	/** 判断整型列表是否是空 */
	public bool isEmpty()
	{
		return size<=0;
	}
	/** 得到整型列表的整型数组，一般使用toArray()方法 */
	public int[] getArray()
	{
		return array;
	}
	/* methods */
	/** 设置整型列表的容积，只能扩大容积 */
	public void setCapacity(int len)
	{
		int[] array=this.array;
		int c=array.Length;
		if(len<=c) return;
		for(;c<len;c=(c<<1)+1)
			;
		int[] temp=new int[c];
		Array.Copy(array, 0, temp, 0, size);
		this.array=temp;
	}
	/** 得到整型列表的指定位置的元素 */
	public int get(int index)
	{
		return array[index];
	}
	/** 得到整型列表的第一个元素 */
	public int getFirst()
	{
		return array[0];
	}
	/** 得到整型列表的最后一个元素 */
	public int getLast()
	{
		return array[size-1];
	}
	/** 判断整型列表是否包含指定的元素 */
	public bool contain(int t)
	{
		return indexOf(t,0)>=0;
	}
	/** 获得指定元素在整型列表中的位置，从开头向后查找 */
	public int indexOf(int t)
	{
		return indexOf(t,0);
	}
	/** 获得指定元素在整型列表中的位置，从指定的位置向后查找 */
	public int indexOf(int t,int index)
	{
		int top=this.size;
		if(index>=top) return -1;
		int[] array=this.array;
		for(int i=index;i<top;i++)
		{
			if(t==array[i]) return i;
		}
		return -1;
	}
	/** 获得指定元素在整型列表中的位置，从末尾向前查找 */
	public int lastIndexOf(int t)
	{
		return lastIndexOf(t,size-1);
	}
	/** 获得指定元素在整型列表中的位置，从指定的位置向前查找 */
	public int lastIndexOf(int t,int index)
	{
		if(index>=size) return -1;
		int[] array=this.array;
		for(int i=index;i>=0;i--)
		{
			if(t==array[i]) return i;
		}
		return -1;
	}
	/** 设置整型列表的指定位置的元素，返回原来的元素 */
	public int set(int t,int index)
	{
		if(index>=size)
			throw new Exception(GetType().Name+" set, invalid index="+index);
		int i=array[index];
		array[index]=t;
		return i;
	}
	/** 整型列表添加元素 */
	public bool add(int t)
	{
		if(size>=array.Length) setCapacity(size+1);
		array[size++]=t;
		return true;
	}
	/** 在指定位置插入元素，元素在数组中的顺序不变 */
	public void add(int t,int index)
	{
		if(index<size)
		{
			if(size>=array.Length) setCapacity(size+1);
			if(size>index)
				Array.Copy(array,index,array,index+1,size-index);
			array[index]=t;
			size++;
		}
		else
		{
			if(index>=array.Length) setCapacity(index+1);
			array[index]=t;
			size=index+1;
		}
	}
	/**
	 * 在指定位置插入元素， 元素在数组中的顺序改变，原插入的位置上的元素移到的最后，
	 */
	public void addAt(int t,int index)
	{
		if(index<size)
		{
			if(size>=array.Length) setCapacity(size+1);
			array[size++]=array[index];
			array[index]=t;
		}
		else
		{
			if(index>=array.Length) setCapacity(index+1);
			array[index]=t;
			size=index+1;
		}
	}
	/** 从整型列表移除指定的元素 */
	public bool remove(int t)
	{
		int i=indexOf(t,0);
		if(i<0) return false;
		removeIndex(i);
		return true;
	}
	/**
	 * 从整型列表移除指定的元素， 元素在数组中的顺序被改变，原来最后一项移到被移除元素的位置，
	 */
	public bool removeAt(int t)
	{
		int i=indexOf(t,0);
		if(i<0) return false;
		removeIndexAt(i);
		return true;
	}
	/** 移除指定位置的元素，元素在数组中的顺序不变 */
	public int removeIndex(int index)
	{
		if(index>=size)
			throw new Exception(GetType().Name+" removeIndex, invalid index="+index);
		int[] array=this.array;
		int t=array[index];
		int j=size-index-1;
		if(j>0) Array.Copy(array,index+1,array,index,j);
		array[--size]=0;
		return t;
	}
	/**
	 * 移除指定位置的元素， 元素在数组中的顺序被改变，原来最后一项移到被移除元素的位置，
	 */
	public int removeIndexAt(int index)
	{
		if(index>=size)
			throw new Exception(GetType().Name+" removeIndexAt, invalid index="+index);
		int[] array=this.array;
		int t=array[index];
		array[index]=array[--size];
		array[size]=0;
		return t;
	}
	/** 清除整型列表中的所有元素 */
	public void clear()
	{
		size=0;
	}
	/** 以整型数组的方式得到整型列表中的元素 */
	public int[] toArray()
	{
		int[] temp=new int[size];
		Array.Copy(array,0,temp,0,size);
		return temp;
	}
	/** 将整型列表中的元素拷贝到指定的数组 */
	public int toArray(int[] temp)
	{
		int len=(temp.Length>size)?size:temp.Length;
		Array.Copy(array,0,temp,0,len);
		return len;
	}
	/* common methods */
	public object Clone()
	{
		try
		{
			IntList temp=(IntList)base.MemberwiseClone ();
			int[] array=temp.array;
			temp.array=new int[temp.size];
			Array.Copy(array,0,temp.array,0,temp.size);
			return temp;
		}
		catch(Exception e)
		{
			throw new Exception (this
			                     + " clone, capacity=" + array.Length, e);
		}
	}
	public String toString()
	{
		return base.ToString()+"[size="+size+", capacity="+array.Length+"]";
	}
	
}
