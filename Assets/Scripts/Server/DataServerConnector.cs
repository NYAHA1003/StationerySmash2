using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Main.Deck;
using Utill.Data;
using Utill.Tool;

public class DataServerConnector : MonoBehaviour
{
	[System.Serializable]
	private class UserSaveDataServer
	{
		public string message; //서버에서 반환하는 메시지 혹은 내가 서버에 보내는 메시지
		public UserSaveData post; //유저 데이터값
	}
	private UserSaveDataServer postData;

	/// <summary>
	/// 유저 저장 데이터를 서버에 업데이트한다
	/// </summary>
	public void PostUserSaveData()
	{
		postData.message = "POST";
		postData.post = UserSaveManagerSO.UserSaveData;
		string jsonData = JsonUtility.ToJson(postData);
		StartCoroutine(IEPost(jsonData));
	}

	/// <summary>
	/// 유저 저장 데이터를 가져온다
	/// </summary>
	public void GetUserSaveData()
	{
		postData.message = "GET";
		string jsonData = JsonUtility.ToJson(postData);
		StartCoroutine(IEPost(jsonData));
	}

	/// <summary>
	/// 유저 데이터 관련 Get, Post
	/// </summary>
	private IEnumerator IEPost(string postData)
	{
		var link = ServerLinks.endPoint + ServerLinks.linkUserSaveData;
		using (UnityWebRequest www = UnityWebRequest.Post(link, postData))
		{
			www.method = "POST";
			www.SetRequestHeader("Content-Type", "application/json");
			var jsonBytes = Encoding.UTF8.GetBytes(postData);
			www.uploadHandler = new UploadHandlerRaw(jsonBytes);
			www.downloadHandler = new DownloadHandlerBuffer();

			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
			}
			else
			{
				Debug.Log(www.downloadHandler.text);
				UserSaveDataServer newPost = new UserSaveDataServer();
				newPost = JsonUtility.FromJson<UserSaveDataServer>(www.downloadHandler.text);
				CollbackMessageProcess(newPost);
				yield return www.downloadHandler.text;
			}
		}
	}

	/// <summary>
	/// 메시지를 처리한다
	/// </summary>
	private void CollbackMessageProcess(UserSaveDataServer newPost)
	{
		switch(newPost.message)
		{
			case "UPDATE":
				Debug.Log("업데이트");
				break;
			case "FIND":
				Debug.Log("검색");
				this.postData = newPost;
				break;
			case "NONE":
				Debug.Log("논");
				break;
			default:
				Debug.LogError("확인할 수 없는 메시지");
				break;
		}
	}
}
