using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadToMainLoading : MonoBehaviour
{
	/// <summary>
	/// ���� �ε������� �̵��ϴ� �Լ�
	/// </summary>
	public void LoadToMainLoadingScene()
	{
		SceneManager.LoadScene("MainLoading");
	}

}
