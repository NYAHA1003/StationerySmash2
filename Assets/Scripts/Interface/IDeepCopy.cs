using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// T�� �ڷ����� ���� �����ϴ� �������̽�
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDeepCopy<T>
{
	/// <summary>
	/// T�� �ڷ����� ���� �����ؼ� ��ȯ�Ѵ�
	/// </summary>
	/// <returns></returns>
	public T DeepCopy();
}
