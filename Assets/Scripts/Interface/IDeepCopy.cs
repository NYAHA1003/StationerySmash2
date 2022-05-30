using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// T형 자료형을 깊은 복사하는 인터페이스
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDeepCopy<T>
{
	/// <summary>
	/// T형 자료형을 깊은 복사해서 반환한다
	/// </summary>
	/// <returns></returns>
	public T DeepCopy();
}
