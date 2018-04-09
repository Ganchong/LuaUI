/**
 * 类说明：基于整数键的哈希表
 * 
 * @version 1.0
 * @author zminleo <zmin@seasky.cn>
 */
using System;

public class IntKeyHashMap<E> : ISelectable
{

	/* static fields */
	/** 默认的初始容量大小 */
	public static readonly int CAPACITY = 16;
	/** 默认的加载因子 */
	public static readonly float LOAD_FACTOR = 0.75f;

	/* fields */
	/** 节点数组 */
	IntKeyHashMapEntry[] array;
	/** 节点数组 */
	int _size;
	/** 加载因子 */
	readonly float loadFactor;
	/** 实际最大数量（容量X加载因子） */
	int threshold;

	/* constructors */
	/** 构造一个表 */
	public IntKeyHashMap ():this(CAPACITY,LOAD_FACTOR)
	{
	}
	/** 按指定的大小构造一个表 */
	public IntKeyHashMap (int capacity):this(capacity,LOAD_FACTOR)
	{
		
	}
	/** 按指定的大小和加载因子构造一个表 */
	public IntKeyHashMap (int capacity, float loadFactor)
	{
		if (capacity < 1)
			throw new ArgumentException (GetType()
				+ " <init>, invalid capatity:" + capacity);
		if (loadFactor <= 0)
			throw new ArgumentException (GetType()
				+ " <init>, invalid loadFactor:" + loadFactor);
		threshold = (int)(capacity * loadFactor);
		if (threshold <= 0)
			throw new ArgumentException (GetType()
				+ " <init>, invalid threshold:" + capacity + " " + loadFactor);
		this.loadFactor = loadFactor;
		array = new IntKeyHashMapEntry[capacity];
	}
	/* properties */
	/** 获得表的大小 */
	public int size ()
	{
		return _size;
	}
	/** 获取值 */
	public E this[int key]
	{
		get {
			return get(key);
		}
		set {
			put(key,value);
		}
	}

	/* methods */
	/** 获取映射到指定键的值 */
	public E get (int key)
	{
		IntKeyHashMapEntry[] array = this.array;
		IntKeyHashMapEntry n = array [(key & 0x7fffffff) % array.Length];
		while (n!=null) {
			if (n.key == key)
				return n.val;
			n = n.next;
		}
		return default(E);
	}
	/** 是否包含Key */
	public bool containsKey (int key)
	{
		IntKeyHashMapEntry[] array = this.array;
		IntKeyHashMapEntry n = array [(key & 0x7fffffff) % array.Length];
		while (n!=null) {
			if (n.key == key)
				return true;
			n = n.next;
		}
		return false;
	}
	/** 设置映射到指定键的值 */
	public E put (int key, E val)
	{
		IntKeyHashMapEntry[] array = this.array;
		int i = (key & 0x7fffffff) % array.Length;
		IntKeyHashMapEntry n = array [i];
		if (n != null) {
			while (true) {
				if (n.key == key) {
					E old = n.val;
					n.val = val;
					return old;
				}
				if (n.next == null)
					break;
				n = n.next;
			}
			n.next = new IntKeyHashMapEntry (key, val);
		} else
			array [i] = new IntKeyHashMapEntry (key, val);
		_size++;
		if (_size >= threshold)
			rehash (array.Length + 1);
		return default(E);
	}
	/** 移除映射到指定键的值 */
	public E remove (int key)
	{
		IntKeyHashMapEntry[] array = this.array;
		int i = (key & 0x7fffffff) % array.Length;
		IntKeyHashMapEntry n = array [i];
		IntKeyHashMapEntry parent = null;
		while (n!=null) {
			if (n.key == key) {
				E old = n.val;
				if (parent != null)
					parent.next = n.next;
				else
					array [i] = n.next;
				_size--;
				return old;
			}
			parent = n;
			n = n.next;
		}
		return default(E);
	}
	/** 根据新的容量，重新分布哈希码 */
	public void rehash (int capacity)
	{
		IntKeyHashMapEntry[] array = this.array;
		int len = array.Length;
		if (capacity <= len)
			return;
		int c = len;
		for (; c<capacity; c=(c<<1)+1)
			;
		IntKeyHashMapEntry[] temp = new IntKeyHashMapEntry[c];
		IntKeyHashMapEntry n, next, old;
		for (int i=len-1,j=0; i>=0; i--) {
			n = array [i];
			// 将哈希条目从旧数组中挪到新数组中，链表中条目的次序被反转
			while (n!=null) {
				next = n.next;
				j = (n.key & 0x7fffffff) % c;
				old = temp [j];
				temp [j] = n;
				n.next = old;
				n = next;
			}
		}
		this.array = temp;
		threshold = (int)(c * loadFactor);
	}
	/** 选择方法，用指定的选择器对象选出表中的元素，返回值参考常量定义 */
	public int select (Selector selector)
	{
		IntKeyHashMapEntry[] array = this.array;
		IntKeyHashMapEntry n, next;
		IntKeyHashMapEntry parent = null;
		int t;
		int r = SelectorKit.FALSE;
		for (int i=array.Length-1; i>=0; i--) {
			n = array [i];
			while (n!=null) {
				t = selector.select (n);
				next = n.next;
				if (t == SelectorKit.FALSE) {
					n = next;
					continue;
				}
				if (t == SelectorKit.TRUE) {
					if (parent != null)
						parent.next = next;
					else
						array [i] = next;
					_size--;
					r = t;
					n = next;
					continue;
				}
				if (t == SelectorKit.TRUE_BREAK) {
					if (parent != null)
						parent.next = next;
					else
						array [i] = next;
					_size--;
				}
				return t;
			}
		}
		return r;
	}
	/** 清理方法 */
	public void clear ()
	{
		IntKeyHashMapEntry[] array = this.array;
		for (int i=array.Length-1; i>=0; i--)
			array [i] = null;
		_size = 0;
	}
	/** 获得键数组 */
	public int[] keyArray ()
	{
		IntKeyHashMapEntry[] array = this.array;
		int[] temp = new int[_size];
		IntKeyHashMapEntry n;
		for (int i=array.Length-1,j=0; i>=0; i--) {
			n = array [i];
			while (n!=null) {
				temp [j++] = n.key;
				n = n.next;
			}
		}
		return temp;
	}
	/** 获得值元素数组 */
	public E[] valueArray ()
	{
		IntKeyHashMapEntry[] array = this.array;
		E[] temp = new E[_size];
		IntKeyHashMapEntry n;
		for (int i=array.Length-1,j=0; i>=0; i--) {
			n = array [i];
			while (n!=null) {
				temp [j++] = n.val;
				n = n.next;
			}
		}
		return temp;
	}
	/** 将值元素拷贝到指定的数组 */
	public int valueArray (E[] temp)
	{
		IntKeyHashMapEntry[] array = this.array;
		int len = (temp.Length > _size) ? _size : temp.Length;
		if (len == 0)
			return 0;
		IntKeyHashMapEntry n;
		int j = 0;
		for (int i=array.Length-1; i>=0; i--) {
			n = array [i];
			while (n!=null) {
				temp [j++] = n.val;
				if (j >= len)
					return j;
				n = n.next;
			}
		}
		return j;
	}
	/** 遍历 */
	public void Foreach(Action<E> action)
	{
		IntKeyHashMapEntry[] array = this.array;
		IntKeyHashMapEntry n;
		for (int i=array.Length-1,j=0; i>=0; i--) 
		{
			n = array [i];
			while (n!=null) 
			{
				action(n.val);
				n = n.next;
			}
		}
	}
	/** 获取迭代器 */
	public Iterator getIterator()
	{
		return new Iterator (array);
	}

	/* common methods */
	public override string ToString ()
	{
		return base.ToString () + "[size=" + _size + ", capacity=" + array.Length + "]";
	}

	/* inner classes */
	public class IntKeyHashMapEntry
	{

		/* fields */
		/** 键 */
		internal int key;
		/** 后节点 */
		internal IntKeyHashMapEntry next;
		/** 值 */
		internal E val;

		/* constructors */
		/** 构造一个指定的整数键和关联元素的节点 */
		internal IntKeyHashMapEntry (int key, E val)
		{
			this.key = key;
			this.val = val;
		}
		/* properties */
		/** 获得键 */
		public int getKey ()
		{
			return key;
		}
		/** 获得值 */
		public E getValue ()
		{
			return val;
		}

	}
	/** 迭代器 */
	public struct Iterator
	{
		/* fields */
		/** 数组 */
		IntKeyHashMapEntry[] array;
		/** 当前 */
		IntKeyHashMapEntry _next;
		/** 当前数组下标 */
		int index;
		
		/* constructor */
		/** 构造方法 */
		public Iterator(IntKeyHashMapEntry[] array)
		{
			this.array=array;
			_next=null;
			index=0;
			moveNext();
		}
		
		/* methods */
		/** 是否有下一个 */
		public bool hasNext()
		{
			return _next != null;
		}
		/** 下一个 */
		public IntKeyHashMapEntry next()
		{
			IntKeyHashMapEntry oldNext = _next;
			moveNext ();
			return oldNext;
		}
		/** 获取下一个 */
		private void moveNext()
		{
			IntKeyHashMapEntry next = this._next;
			if (next != null && next.next != null) 
			{
				_next = next.next;
				return;
			}
			IntKeyHashMapEntry[] array = this.array;
			for(int index = this.index;index<array.Length;index++)
			{
				if(array[index]!=null)
				{
					_next=array[index];
					this.index=++index;
					return;
				}
			}
			_next = null;
			this.array = null;
			this.index = -1;
		}
	}
}
