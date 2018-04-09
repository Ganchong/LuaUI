using System;
using System.Reflection;
/// <summary>
/// Domain access.
/// </summary>
public class DomainAccess
{
	/* static fields */
	/** 获取对象 */
    public static object getObject(string classStr)
    {
        Type t = getType(classStr);
        if (t == null) return null;
        return LoadObject(t);
    }
	/** 多程序集中获取类型 */
    public static Type getType(string classStr)
    {
        Assembly[] asses = AppDomain.CurrentDomain.GetAssemblies();
        Type type = null;
        foreach (Assembly item in asses)
        {
            type = item.GetType(classStr);
            if (type != null)
                break;
        }
        return type;
    }
	/** 获取对象 */
    public static object LoadObject(Type type)
    {
		if (type == null) return null;
        try
        {
			object obj = null;
			if(type == typeof(GameManager))
				obj=GameManager.Instance;
            else obj = Activator.CreateInstance(type);
            return obj;
        }
        catch (Exception e)
        {
            Log.debug("  " + e);
            return null;
        }
    }
	/** 是否是组件 */
	private static bool isComponent(Type type)
	{
		Type temp = type;
		do {
			if (temp == typeof(UnityEngine.MonoBehaviour)) {
				return true;
			} else {
				temp = temp.BaseType;
			}
		} while (temp!=null);
		return false;
	}
	/** 是否值组件 */
	public static bool IsComponent(string className,out Type type)
	{
		type = getType(className);
		if (type == null) return false;
		return isComponent(type);
	}
}