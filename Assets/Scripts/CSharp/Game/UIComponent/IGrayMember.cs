using UnityEngine;

/// <summary>
/// 置灰接口，所有可以置灰的都应该继承此接口
/// </summary>
public interface IGrayMember {

	/// <summary>
	/// 是否置灰
	/// </summary>
	bool IsGray { get; }

	/// <summary>
	/// 置灰方法
	/// </summary>
	/// <param name="isGray"></param>
	void SetGray(bool isGray);
}
