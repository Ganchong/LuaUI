using UnityEngine;
using System.Collections;

public class ServerComparator : Comparator
{
	/* methods */
	/** 比较方法，返回比较结果常数 */
	public int compare(object o1,object o2)
	{
		Server s1 = o1 as Server;
		Server s2 = o2 as Server;
		if (s1.ID > s2.ID)
			return ComparatorKit.COMP_LESS;
		return ComparatorKit.COMP_GRTR;
	}
}

