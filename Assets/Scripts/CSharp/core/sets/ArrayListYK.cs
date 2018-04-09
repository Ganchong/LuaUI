using System;
/**
 *  @author 汪松民
 */
public class ArrayListYK<T>:Singleton<ArrayListYK<T>>,ICloneable,Container,ISelectable
{
	/* static fields */
	public const int CAPACITY = 10;

	/* fields */
	protected object[] array;
	int _size;

	/* constructors */
	public ArrayListYK ():this(CAPACITY)
	{
	}

	public ArrayListYK (int capacity)
	{
		if (capacity < 1)
			throw new ArgumentException (this
				+ " <init>, invalid capatity:" + capacity);
		array = new object[capacity];
	}

	public ArrayListYK (object[] array):this(array,(array!=null)?array.Length:0)
	{
	}

	public ArrayListYK (object[] array, int len)
	{
		if (array == null)
			throw new ArgumentException (this
				+ " <init>, null array");
		if (len > array.Length)
			throw new ArgumentException (this
				+ " <init>, invalid length:" + len);
		this.array = (object[])array;
		this._size = len;
	}
	/* properties */
	public int size ()
	{
		return _size;
	}

	public int capacity ()
	{
		return array.Length;
	}

	public bool isEmpty ()
	{
		return _size <= 0;
	}

	public bool isFull ()
	{
		return false;
	}

	public object[] getArray ()
	{
		return array;
	}
	/* methods */
	public void setCapacity (int len)
	{
		object[] array = this.array;
		int c = array.Length;
		if (len <= c)
			return;
		for (; c<len; c=(c<<1)+1)
			;
		object[] temp = new object[c];
	
		Array.Copy (array, 0, temp, 0, _size);
		this.array = temp;
	}

	public T get ()
	{
		return getLast ();
	}

	public T get (int index)
	{
		return (T)array [index];
	}

	public T this[int index]
	{
		get {
			return (T)array [index];
		}
		set {
			array[index] = value;
		}
	}

	public T getFirst ()
	{
		return (T)array [0];
	}

	public T getLast ()
	{
		return (T)array [_size - 1];
	}

	public bool contain (object obj)
	{
		return indexOf (obj, 0) >= 0;
	}

	public int indexOf (object obj)
	{
		return indexOf (obj, 0);
	}

	public int indexOf (object obj, int index)
	{
		int top = this._size;
		if (index >= top)
			return -1;
		object[] array = this.array;
		if (obj == null) {
			for (int i=index; i<top; i++) {
				if (array [i] == null)
					return i;
			}
		} else {
			for (int i=index; i<top; i++) {
				if (obj.Equals (array [i]))
					return i;
			}
		}
		return -1;
	}

	public int lastIndexOf (object obj)
	{
		return lastIndexOf (obj, _size - 1);
	}

	public int lastIndexOf (object obj, int index)
	{
		if (index >= _size)
			return -1;
		object[] array = this.array;
		if (obj == null) {
			for (int i=index; i>=0; i--) {
				if (array [i] == null)
					return i;
			}
		} else {
			for (int i=index; i>=0; i--) {
				if (obj.Equals (array [i]))
					return i;
			}
		}
		return -1;
	}

	public virtual T set (object obj, int index)
	{
		if (index >= _size)
			throw new Exception (this
				+ " set, invalid index=" + index);
		T o = (T)array [index];
		array [index] = obj;
		return o;
	}

	public virtual bool add (object obj)
	{
		if (_size >= array.Length)
			setCapacity (_size + 1);
		array [_size++] = obj;
		return true;
	}

	public void add (object obj, int index)
	{
		if (index < _size) {
			if (_size >= array.Length)
				setCapacity (_size + 1);
			Array.Copy (array, index, array, index + 1, _size - index);
			array [index] = obj;
			_size++;
		} else {
			if (index >= array.Length)
				setCapacity (index + 1);
			array [index] = obj;
			_size = index + 1;
		}
	}

	public virtual void addAt (T obj, int index)
	{
		if (index < _size) {
			if (_size >= array.Length)
				setCapacity (_size + 1);
			array [_size++] = array [index];
			array [index] = obj;
		} else {
			if (index >= array.Length)
				setCapacity (index + 1);
			array [index] = obj;
			_size = index + 1;
		}
	}

	public bool remove (T obj)
	{
		int i = indexOf (obj, 0);
		if (i < 0)
			return false;
		remove (i);
		return true;
	}

	public virtual bool removeAt (T obj)
	{
		int i = indexOf (obj, 0);
		if (i < 0)
			return false;
		removeAt (i);
		return true;
	}

	public T remove ()
	{
		return remove (_size - 1);
	}

	public T remove (int index)
	{
		if (index >= _size)
			throw new Exception (this
				+ " remove, invalid index=" + index);
		object[] array = this.array;
		object obj = array [index];
		int j = _size - index - 1;
		if (j > 0)
			Array.Copy (array, index + 1, array, index, j);
		array [--_size] = null;
		return (T)obj;
	}

	public virtual T removeAt (int index)
	{
		if (index >= _size)
			throw new Exception (this
				+ " removeAt, invalid index=" + index);
		object[] array = this.array;
		object obj = array [index];
		array [index] = array [--_size];
		array [_size] = null;
		return (T)obj;
	}
	public int select(Selector selector)
	{
		int t;
		object obj;
		int r=SelectorKit.FALSE;
		object[] array=this.array;
		for(int i=_size-1;i>=0;i--)
		{
			obj=array[i];
			t=selector.select(obj);
			if(t==SelectorKit.FALSE) continue;
			if(t==SelectorKit.TRUE)
			{
				array[i]=array[--_size];
				array[_size]=null;
				r=t;
				continue;
			}
			if(t==SelectorKit.TRUE_BREAK)
			{
				array[i]=array[--_size];
				array[_size]=null;
			}
			return t;
		}
		return r;
	}
	public void clear ()
	{
		object[] array = this.array;
		for (int i=_size-1; i>=0; i--)
			array [i] = null;
		_size = 0;
	}

	public T[] toArray ()
	{
		T[] temp = new T[_size];
		Array.Copy (array, 0, temp, 0, _size);
		return temp;
	}

	public int toArray (T[] temp)
	{
		int len = (temp.Length > _size) ? _size : temp.Length;
		Array.Copy (array, 0, temp, 0, len);
		return len;
	}
	/* common methods */
	public object Clone ()
	{
		try {
			ArrayListYK<T> temp = (ArrayListYK<T>)base.MemberwiseClone ();
			object[] array = temp.array;
			temp.array = new object[temp._size];
			Array.Copy (array, 0, temp.array, 0, temp._size);
			return temp;
		} catch (Exception e) {
			throw new Exception (this
				+ " clone, capacity=" + array.Length, e);
		}
	}
	public void add(ArrayListYK<T> list){
		T[] objs = list.toArray ();
		for (int i=0; i<objs.Length; i++) {
			add(objs[i]);
		}
	}

	public void add(T[] objs){
		for (int i=0; i<objs.Length; i++) {
			add(objs[i]);
		}
	}

	public override String ToString ()
	{
		return base.ToString () + "[size=" + _size + ", capacity=" + array.Length + "]";
	}
}
