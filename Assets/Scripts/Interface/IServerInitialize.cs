using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IServerInitialize
{
	/// <summary>
	/// �������� �����͸� �޾� �ʱ�ȭ�ϴ� �Լ�
	/// </summary>
	public void ServerInitialize(ServerDataConnect serverDataConnect);
}
