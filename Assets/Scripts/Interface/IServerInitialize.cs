using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IServerInitialize
{
	/// <summary>
	/// 서버에서 데이터를 받아 초기화하는 함수
	/// </summary>
	public void ServerInitialize(ServerDataConnect serverDataConnect);
}
