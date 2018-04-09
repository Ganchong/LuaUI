using UnityEngine;
using System.Collections;
/// <summary>
/// 加密解密数据校验工具
/// </summary>
public class CodecKit  
{
	/* static fields */
	/** CRC字典 */
	private static uint[] CRCTABLE = MakeCRCTable ();
	
	/* static methods */
	/** 创建CRC字典 */
	private static uint[] MakeCRCTable ()
	{
		uint[] table = new uint[256];
		
		uint i;
		uint j;
		uint c;
		for (i = 0; i < 256; i++) {
			c = i;
			for (j = 0; j < 8; j++) {
				if ((c & 1) == 1) {
					c = 0xEDB88320 ^ (c >> 1);
				} else {
					c >>= 1;
				}
			}
			
			table [i] = c;
		}
		return table;
	}
	
	/** 计算CRC32校验码 */
	public static uint CRC32 (ByteBuffer data, uint start, uint len)
	{
		if (start >= data.top) {
			start = (uint)data.top;
		}
		if (len == 0) {
			len = (uint)data.top - start;
		}
		if (len + start > (uint)data.top) {
			len = (uint)data.top - start;
		}
		uint i;
		uint c = 0xffffffff;
		for (i = start; i < len; i++) {
			c = (uint)CRCTABLE [(c ^ data.getArray () [i]) & 0xff] ^ (c >> 8);
		}
		return (c ^ 0xffffffff);
	}
	/** 计算adler32校验码 */ 
	public static uint Adler32 (ByteBuffer data)
	{
		return	Adler32 (data, 0, 0);
	}
	public static uint Adler32 (ByteBuffer data, uint start, uint len)
	{
		if (start >= (uint)data.length ()) {
			start = (uint)data.length ();
		}
		if (len == 0) {
			len = (uint)data.length () - start;
		}
		if (len + start > (uint)data.length ()) {
			len = (uint)data.length () - start;
		}
		uint i = start;
		uint a = 1;
		uint b = 0; 
		
		while (i < (start + len)) {
			a = (a + data.toArray () [i]) % 65521;
			b = (a + b) % 65521;
			i++;
		}
		return (b << 16) | a;
	}

	/**
	 * 字节数组加密
	 * 
	 * @param bytes 源数据
	 * @param keys 密钥
	 * @return byte[] 加密后的数据
	 */
	public static byte[] EncodeXor(byte[] bytes,byte[] keys)
	{
		if(bytes==null||bytes.Length<1||keys==null
			||keys.Length<1)
			return null;
		
		int blength=bytes.Length;
		int klength=keys.Length;
		byte[] result=new byte[blength];
		int j=0;
		for(int i=0;i<blength;i++)
		{
			if(j==klength) j=0;
			int k=(bytes[i]^keys[j]);
			k<<=24;
			k>>=24;
			result[i]=(byte)k;
			j++;
		}
		return result;
	}
}
