using System;

/// <summary>
/// 池对象
/// </summary>
public interface IPoolObject<T> where T : IPoolObject<T>
{
	/* methods */
	/** 自身 */
	T Cast();
}