
using System;
using System.Collections;
using UnityEngine;

/**
 * @author 汪松民
 */
public class ByteBuffer : ICloneable, IPoolObject<ByteBuffer>
{
	/* static fields */
	/** 默认的初始容量大小 */
	public const int CAPACITY = 32;
	/** 默认的动态数据或文字的最大长度，400k */
	public const int MAX_DATA_LENGTH = 400 * 1024;
    /** 对象缓存池 */
    public static ObjectPool<ByteBuffer> pool = new ObjectPool<ByteBuffer>();

    /* static methods */
    /** 获取一个字节缓存 */
    public static ByteBuffer GetByteBuffer()
    {
        return pool.Get();
    }
    /** 获取一个字节缓存指定空闲长度 */
    public static ByteBuffer GetByteBuffer(int freeLength)
    {
        ByteBuffer data=pool.Get();
        data.clear(freeLength);
        return data;
    }

	/* fields */
	/** 字节数组 */
	byte[] array;
	/** 字节缓存的长度 */
	int _top;
	/** 字节缓存的偏移量 */
	int _position;

    /* properties */
    /** 字节缓存的长度 */
	public int top 
    {
		get 
        {
			return _top;
		}
		set 
        { 
			_top = value;
		} 
	}
    /** 字节缓存的偏移量 */
	public int position 
    { 
		get 
        {
			return _position;
		}
		set 
        { 
			_position = value;
		} 
	}
    /** 标记 */
    public int mark
    {
        get;
        set;
    }

	/* constructors */
	/** 按默认的大小构造一个字节缓存对象 */
	public ByteBuffer ():this(CAPACITY)
	{
	}
	/** 按指定的大小构造一个字节缓存对象 */
	public ByteBuffer (int capacity)
	{
		if (capacity < 1)
			throw new Exception (this
				+ " <init>, invalid capatity:" + capacity);
		array = new byte[capacity];
		top = 0;
		position = 0;
	}
	/** 按指定的字节数组构造一个字节缓存对象 */
	public ByteBuffer (byte[] data)
	{
		if (data == null)
			throw new Exception (this
				+ " <init>, null data");
		array = data;
		top = data.Length;
		position = 0;
	}
	/** 按指定的字节数组构造一个字节缓存对象 */
	public ByteBuffer (byte[] data, int index, int length)
	{
		if (data == null)
			throw new Exception (this
				+ " <init>, null data");
		if (index < 0 || index > data.Length)
			throw new Exception (this
				+ " <init>, invalid index:" + index);
		if (length < 0 || data.Length < index + length)
			throw new Exception (this
				+ " <init>, invalid length:" + length);
		array = data;
		top = index + length;
		position = index;
	}

	/* properties */
	/** 得到字节缓存的容积 */
	public int capacity ()
	{
		return array.Length;
	}
	/** 设置字节缓存的容积，只能扩大容积 */
	public void setCapacity (int len)
	{
		int c = array.Length;
		if (len <= c)
			return;
		for (; c<len; c=(c<<1)+1)
			;
		byte[] temp = new byte[c];
		Buffer.BlockCopy (array, 0, temp, 0, top);
		array = temp;
	}
 
	/** 设置字节缓存的长度 */
	public void setTop (int top)
	{
		if (top < position)
			throw new Exception (this + " setTop, invalid top:"
				+ top);
		if (top > array.Length)
			setCapacity (top);
		this.top = top;
	}
	/** 得到字节缓存的偏移量 */
	public int offset ()
	{
		return position;
	}
	/** 设置字节缓存的偏移量 */
	public void setOffset (int offset)
	{
		if (offset < 0 || offset > top)
			throw new Exception (this
				+ " setOffset, invalid offset:" + offset);
		this.position = offset;
	}
	/** 得到字节缓存的使用长度 */
	public int length ()
	{
		return top - position;
	}
	/** 得到字节缓存的字节数组，一般使用toArray()方法 */
	public byte[] getArray ()
	{
		return array;
	}

	/* methods */
	/* byte methods */
	/** 得到指定偏移位置的字节 */
	public byte read (int pos)
	{
		return array [pos];
	}
	/** 设置指定偏移位置的字节 */
	public void write (int b, int pos)
	{
		array [pos] = (byte)b;
	}
	/* read methods */
	/**
	 * 按当前偏移位置读入指定的字节数组
	 * 
	 * @param data 指定的字节数组
	 * @param pos 指定的字节数组的起始位置
	 * @param len 读入的长度
	 */
	public void read (byte[] data, int pos, int len)
	{
		Buffer.BlockCopy (array, position, data, pos, len);
		position += len;
	}
	/** 读出一个布尔值 */
	public bool readBoolean ()
	{
		return (array [position++] != 0);
	}
	/** 读出一个字节 */
	public sbyte readByte ()
	{
		return unchecked((sbyte)array [position++]);
	}
	/** 读出一个无符号字节 */
	public int readUnsignedByte ()
	{
		return array [position++] & 0xff;
	}
	/** 读出一个字符 */
	public char readChar ()
	{
		return (char)readUnsignedShort ();
	}
	/** 读出一个短整型数值 */
	public short readShort ()
	{
		return (short)readUnsignedShort ();
	}
	/** 读出一个无符号的短整型数值 */
	public int readUnsignedShort ()
	{
		int pos = position;
		position += 2;
		return (array [pos + 1] & 0xff) + ((array [pos] & 0xff) << 8);
	}
	/** 读出一个整型数值 */
	public int readInt ()
	{
		int pos = position;
		position += 4;
		return (array [pos + 3] & 0xff) + ((array [pos + 2] & 0xff) << 8)
			+ ((array [pos + 1] & 0xff) << 16) + ((array [pos] & 0xff) << 24);
	}
	/** 读出一个浮点数值 */
	public float readFloat ()
	{
        return FastBitConverter.IntToFloat(readInt());
	}
	/** 读出一个长整型数值 */
	public long readLong ()
	{
		int pos = position;
		position += 8;
		return (array [pos + 7] & 0xffL) + ((array [pos + 6] & 0xffL) << 8)
			+ ((array [pos + 5] & 0xffL) << 16) + ((array [pos + 4] & 0xffL) << 24)
			+ ((array [pos + 3] & 0xffL) << 32) + ((array [pos + 2] & 0xffL) << 40)
			+ ((array [pos + 1] & 0xffL) << 48) + ((array [pos] & 0xffL) << 56);
	}
	/** 读出一个双浮点数值 */
	public double readDouble ()
	{
		return BitConverter.Int64BitsToDouble (readLong ());
	}
	/**
	 * 读出动态长度， 数据大小采用动态长度，整数类型下，最大为512M
	 * <li>1xxx,xxxx表示（0~0x80） 0~128B</li>
	 * <li>01xx,xxxx,xxxx,xxxx表示（0~0x4000）0~16K</li>
	 * <li>001x,xxxx,xxxx,xxxx,xxxx,xxxx,xxxx,xxxx表示（0~0x20000000）0~512M</li>
	 */
	public int readLength ()
	{
		int n = array [position] & 0xff;
		if (n >= 0x80) {
			position++;
			return n - 0x80;
		}
		if (n >= 0x40)
			return readUnsignedShort () - 0x4000;
		if (n >= 0x20)
			return readInt () - 0x20000000;
		throw new Exception (this
			+ " readLength, invalid number:" + n);
	}
	/** 读出一个指定长度的字节数组，可以为null */
	public byte[] readData ()
	{
		int len = readLength () - 1;
		if (len < 0)
			return null;
		if (len > MAX_DATA_LENGTH)
			throw new Exception (this
				+ " readData, data overflow:" + len);
		if (len == 0) return Empty<byte>.ARRAY;
		byte[] data = new byte[len];
		read (data, 0, len);
		return data;
	}
	/** 读出一个指定长度的字符串 */
	public string readString ()
	{
		return readString (null);
	}
	/** 读出一个指定长度和编码类型的字符串 */
	public string readString (string charsetName)
	{
		int len = readLength () - 1;
		if (len < 0)
			return null;
		if (len > MAX_DATA_LENGTH)
			throw new Exception (this
				+ " readString, data overflow:" + len);
		if (len == 0)
			return string.Empty;
		byte[] data = new byte[len];
		read (data, 0, len);
		if (charsetName == null)
			return System.Text.Encoding.Default.GetString (data);
		try {
			return System.Text.Encoding.GetEncoding (charsetName).GetString (data);
		} catch (Exception e) { 
			throw new Exception (this
				+ " readString, invalid charsetName:" + charsetName + " " + e.ToString ());
		}
	}
	/** 读出一个指定长度的utf字符串 */
	public string readUTF ()
	{
		int len = readLength () - 1;
		if (len < 0)
			return null;
		if (len == 0) return string.Empty;
		if (len > MAX_DATA_LENGTH)
			throw new Exception (this
				+ " readUTF, data overflow:" + len);
		char[] temp = new char[len];
		int n = ByteKit.readUTF (array, position, len, temp);
		if (n < 0)
			throw new Exception (this
				+ " readUTF, format err, len=" + len);
		position += len;
		return new String (temp, 0, n);
	}
	/* write methods */
	/**
	 * 写入指定字节数组
	 * 
	 * @param data 指定的字节数组
	 * @param pos 指定的字节数组的起始位置
	 * @param len 写入的长度
	 */
	public void write (byte[] data, int pos, int len)
	{
		if (len <= 0)
			return;
		if (array.Length < top + len)
			setCapacity (top + len);
		Buffer.BlockCopy (data, pos, array, top, len); 
		top += len;
	}
	/** 写入一个布尔值 */
	public void writeBoolean (bool b)
	{
		if (array.Length < top + 1)
			setCapacity (top + CAPACITY);
		array [top++] = (byte)(b ? 1 : 0);
	}
	/** 写入一个字节 */
	public void writeByte (int b)
	{
		if (array.Length < top + 1)
			setCapacity (top + CAPACITY);
		array [top++] = (byte)b;
	}
	/** 写入一个字符 */
	public void writeChar (int c)
	{
		writeShort (c);
	}
	/** 写入一个短整型数值 */
	public void writeShort (int s)
	{
		int pos = top;
		if (array.Length < pos + 2)
			setCapacity (pos + CAPACITY);
		ByteKit.writeShort((short)s,array,pos);
		top += 2;
	}
	/** 写入一个整型数值 */
	public void writeInt (int i)
	{
		int pos = top;
		if (array.Length < pos + 4)
			setCapacity (pos + CAPACITY);
		ByteKit.writeInt(i,array,pos);
		top += 4;
	}
	/** 写入一个浮点数值 */
	public void writeFloat (float f)
	{
        writeInt(FastBitConverter.FloatToInt(f));
	}
	/** 写入一个长整型数值 */
	public void writeLong (long l)
	{
		int pos = top;
		if (array.Length < pos + 8)
			setCapacity (pos + CAPACITY);
		ByteKit.writeLong(l,array,pos);
		top += 8;
	}
	/** 写入一个双浮点数值 */
	public void writeDouble (double d)
	{
		writeLong (BitConverter.DoubleToInt64Bits (d));
	}
	/** 写入动态长度 */
	public void writeLength (int len)
	{
		if (len >= 0x20000000 || len < 0)
			throw new Exception (this
				+ " writeLength, invalid len:" + len);
		if (len < 0x80)
			writeByte (len + 0x80);
		else if (len < 0x4000)
			writeShort (len + 0x4000);
		else
			writeInt (len + 0x20000000);
	}
	/** 写入一个字节数组，可以为null */
	public void writeData (byte[] data)
	{
		writeData (data, 0, (data != null) ? data.Length : 0);
	}
	/** 写入一个字节数组，可以为null */
	public void writeData (byte[] data, int pos, int len)
	{
		if (data == null) {
			writeLength (0);
			return;
		}
		writeLength (len + 1);
		write (data, pos, len);
	}
	/** 写入一个字符串，可以为null */
	public void writeString (string str)
	{
		writeString (str, null);
	}
	/** 写入一个字符串，以指定的字符进行编码 */
	public void writeString (string str, string charsetName)
	{
		if (str == null) {
			writeLength (0);
			return;
		}
		if (str.Length <= 0) {
			writeLength (1);
			return;
		}
		byte[] data;
		if (charsetName != null) {
			try {
				data = System.Text.Encoding.GetEncoding (charsetName).GetBytes (str);
			} catch (Exception e) {
				throw new Exception (this
					+ " writeString, invalid charsetName:" + charsetName + " " + e.ToString ());
			}
		} else
			data = System.Text.Encoding.Default.GetBytes (str);
		writeLength (data.Length + 1);
		write (data, 0, data.Length);
	}
	/** 写入一个utf字符串，可以为null */
	public void writeUTF (string str)
	{
		writeUTF (str, 0, (str != null) ? str.Length : 0);
	}
	/** 写入一个utf字符串中指定的部分，可以为null */
	public void writeUTF (string str, int index, int length)
	{
		if (str == null) {
			writeLength (0);
			return;
		}
		int len = ByteKit.getUTFLength (str, index, length);
		writeLength (len + 1);
		if (len <= 0)
			return;
		int pos = top;
		if (array.Length < pos + len)
			setCapacity (pos + len);
		ByteKit.writeUTF (str, index, length, array, pos);
		top += len;
	}
	/** 归零偏移量 */
	public void zeroOffset ()
	{
		int pos = position;
		if (pos <= 0)
			return;
		int t = top - pos;
		Buffer.BlockCopy (array, pos, array, 0, t);
		top = t;
		position = 0;
	}
	/** 得到字节缓存当前长度的字节数组 */
	public byte[] toArray ()
	{
		byte[] data = new byte[top - position];
		Buffer.BlockCopy (array, position, data, 0, data.Length);
		return data;
	}
	/** 清除字节缓存对象 */
	public void clear ()
	{
		top = 0;
		position = 0;
	}
    /** 清除字节缓存对象 */
    public void clear(int position)
    {
        this.top = position;
        this.position = position;
    }
	/** 从字节缓存中反序列化获得对象的域 */
	public object bytesRead (ByteBuffer data)
	{
		int len = data.readLength () - 1;
		if (len < 0)
			return null;
		if (len > MAX_DATA_LENGTH)
			throw new Exception (this
				+ " bytesRead, data overflow:" + len);
		if (array.Length < len)
			array = new byte[len];
		if (len > 0)
			data.read (array, 0, len);
		top = len;
		position = 0;
		return this;
	}
	/** 将对象的域序列化到字节缓存中 */
	public void bytesWrite (ByteBuffer data)
	{
		data.writeData (array, position, top - position);
	}
    /** 回收一个缓存 */
    public void recycle()
    {
        pool.Recycle(this);
    }
    /** 确保缓存字节有指定字节的前置空位 */
    public ByteBuffer ensureFreeAndRecycle(int freeLength)
    {
        if (position >= freeLength) return this;
        ByteBuffer data = GetByteBuffer(freeLength);
        data.write(this.array, _position, this.length());
        recycle();
        return data;
    }

    /* override methods */
    /** 回收 */
    public ByteBuffer Cast()
    {
        this.clear();
        return this;
    }

	/* common methods */
    /** 复制 */
	public object Clone ()
	{
		try {
			ByteBuffer bb = (ByteBuffer)base.MemberwiseClone ();
			byte[] array = bb.array;
			bb.array = new byte[bb.top];
			Buffer.BlockCopy (array, 0, bb.array, 0, bb.top);
			return bb;
		} catch (Exception e) {
			throw new Exception (this
				+ " clone, capacity=" + array.Length, e);
		}
	}
    /** 是否相等 */
	public override bool Equals(object obj)
	{
		if (this == obj)
			return true;
		if (!(obj is ByteBuffer))
			return false;
		ByteBuffer bb = (ByteBuffer)obj;
		if (bb.top != top)
			return false;
		if (bb.position != position)
			return false;
		for (int i=top-1; i>=0; i--)
			if (bb.array [i] != array [i])
				return false;
		return true;
	}
    /** 哈希值 */
    public override int GetHashCode()
    {
        int hash = 17;
        for (int i = top - 1; i >= 0; i--)
            hash = 65537 * hash + array[i];
        return hash;
    }
    /** 字符串 */
	public override string ToString ()
	{
		return base.ToString () + "[" + top + "," + position + "," + array.Length + "]";
	}
}
