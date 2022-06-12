using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadToMainLoading : MonoBehaviour
{
	/// <summary>
	/// 메인 로딩씬으로 이동하는 함수
	/// </summary>
	public void LoadToMainLoadingScene()
	{
		SceneManager.LoadScene("MainLoading");
	}

}
