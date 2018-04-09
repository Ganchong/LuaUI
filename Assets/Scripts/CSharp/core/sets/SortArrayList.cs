using UnityEngine;
using System.Collections;

/**
 * @author 刘耀鑫
 */
public class SortArrayList<E> : ArrayListYK<E>
{
	
	/* fields */
	/** 对象比较器 */
	Comparator comparator;
	/** 降序 */
	bool descending;
	
	/* constructors */
	/** 按默认的大小构造一个列表 */
	public SortArrayList ():base(CAPACITY)
	{}
	/** 按指定的大小构造一个列表 */
	public SortArrayList (int capacity):base(capacity)
	{}
	/** 用指定的对象数组构造一个列表 */
	public SortArrayList(object[] array):base(array)
	{}
	/**
	 * 用指定的对象数组及长度构造一个列表， 指定长度不能超过对象数组的长度，
	 */
	public SortArrayList (object[] array, int len):base(array,len)
	{}
	/* properties */
	/** 获得对象比较器 */
	public Comparator getComparator()
	{
		return comparator;
	}
	/** 设置对象比较器 */
	public void setComparator(Comparator comparator)
	{
		this.comparator=comparator;
	}
	/** 获得对象比较器 */
	public bool isDescending()
	{
		return descending;
	}
	/** 设置对象比较器 */
	public void setDescending(bool b)
	{
		descending=b;
	}
	/* methods */
	/** 设置列表的指定位置的元素，返回原来的元素 */
	public override E set(object obj,int index)
	{
		E o=base.set(obj,index);
		if(comparator!=null)
			SetKit.sort(array,0,size(),comparator,descending);
		return o;
	}
	/** 列表添加元素，并进行排序 */
	public override bool add(object obj)
	{
		bool b=base.add(obj);
		if(!b) return false;
		if(comparator!=null)
			SetKit.sort(array,0,size(),comparator,descending);
		return true;
	}
	/**
	 * 在指定位置插入元素， 元素在数组中的顺序改变，原插入的位置上的元素移到的最后，然后排序
	 */
	public override void addAt(E obj,int index)
	{
		base.addAt(obj,index);
		if(comparator!=null)
			SetKit.sort(array,0,size(),comparator,descending);
	}
	/**
	 * 从列表移除指定的元素， 元素在数组中的顺序被改变，原来最后一项移到被移除元素的位置，然后排序
	 */
	public override bool removeAt(E obj)
	{
		bool b=base.removeAt(obj);
		if(!b) return false;
		if(comparator!=null&&size()>0)
			SetKit.sort(array,0,size(),comparator,descending);
		return true;
	}
	/**
	 * 移除指定位置的元素， 元素在数组中的顺序被改变，原来最后一项移到被移除元素的位置，然后排序
	 */
	public override E removeAt(int index)
	{
		object o=base.removeAt(index);
		if(o==null) return default(E);
		if(comparator!=null&&size()>0)
			SetKit.sort(array,0,size(),comparator,descending);
		return (E)o;
	}
	/** 列表排序元素 */
	public void sort()
	{
		if(comparator!=null&&size()>0)
			SetKit.sort(array,0,size(),comparator,descending);
	}
	
}

