using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Iinitialize
{
	/// <summary>
	/// 초기화하는 함수
	/// </summary>
	public void Initialize();

	/// <summary>
	/// 디버그 모드용 초기화 함수
	/// </summary>
	public void DebugInitialize();
}
