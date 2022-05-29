using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;
using UnityEngine.Networking;
using Main.Deck;

public class ServerDataConnect : MonoSingleton<ServerDataConnect>
{
	//데이터 유형 enum
	public enum DataType
	{
		PencilCaseData,
		BadgeData,
		StickerData,
		UnitData,
		StrategyData,

	}

	[System.Serializable]
	private class UserSaveDataServer
	{
		public string message; //서버에서 반환하는 메시지 혹은 내가 서버에 보내는 메시지
		public UserSaveData post; //유저 데이터값
	}
	private UserSaveDataServer postData;

	/// <summary>
	/// 데이터 타입에 따라 서버 데이터에 접속해서 메시지 반환
	/// </summary>
	/// <param name="action"></param>
	public void GetData<T>(Action<T> action, DataType dataType)
	{
		string link = ReturnLink(dataType, true);
		StartCoroutine(IEGETData(action, link));
	}

	/// <summary>
	/// 데이터 타입에 따라 서버 데이터에 접속해서 메시지 전송
	/// </summary>
	/// <param name="action"></param>
	public void PostData<T>(T data, DataType dataType)
	{
		string link = ReturnLink(dataType, false);
		string jsonData = JsonUtility.ToJson(data);
		Debug.Log(jsonData);
		StartCoroutine(IEPostData(jsonData, link));
	}

	/// <summary>
	/// 유저 저장 데이터를 서버에 업데이트한다
	/// </summary>
	public void PostUserSaveData()
	{
		postData.message = "POST";
		postData.post = UserSaveManagerSO.UserSaveData;
		string jsonData = JsonUtility.ToJson(postData);
		StartCoroutine(IEUserSaveDataPost(jsonData));
	}

	/// <summary>
	/// 유저 저장 데이터를 가져온다
	/// </summary>
	public void GetUserSaveData()
	{
		postData.message = "GET";
		string jsonData = JsonUtility.ToJson(postData);
		StartCoroutine(IEUserSaveDataPost(jsonData));
	}

	/// <summary>
	/// 데이터 타입에 따라 링크를 반환하는 함수
	/// </summary>
	/// <param name="dataType"></param>
	/// <param name="isGet"></param>
	/// <returns></returns>
	private string ReturnLink(DataType dataType, bool isGet = true)
	{
		string link = ServerLinks.endPoint;
		if (isGet)
		{
			switch(dataType)
			{
				case DataType.PencilCaseData:
					link += ServerLinks.getPencilCaseData;
					break;
				case DataType.BadgeData:
					link += ServerLinks.getBadgeData;
					break;
				case DataType.StickerData:
					link += ServerLinks.getSticker;
					break;
				case DataType.UnitData:
					link += ServerLinks.getUnitData;
					break;
				case DataType.StrategyData:
					link += ServerLinks.getStrategyData;
					break;
			}
		}
		else
		{
			switch (dataType)
			{
				case DataType.PencilCaseData:
					link += ServerLinks.getPencilCaseData;
					break;
				case DataType.BadgeData:
					link += ServerLinks.getBadgeData;
					break;
				case DataType.StickerData:
					link += ServerLinks.getSticker;
					break;
				case DataType.UnitData:
					link += ServerLinks.postUnitData;
					break;
				case DataType.StrategyData:
					link += ServerLinks.getStrategyData;
					break;
			}
		}
		return link;
	}

	/// <summary>
	/// 해당하는 데이터 타입을 서버에서 가져오기
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	private IEnumerator IEGETData<T>(Action<T> action, string link)
	{
		using (UnityWebRequest www = UnityWebRequest.Get(link))
		{
			www.method = "GET";

			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
			}
			else
			{
				Debug.Log(www.downloadHandler.text);
				yield return www.downloadHandler.text;
				action.Invoke(JsonUtility.FromJson<T>(www.downloadHandler.text));
			}
		}
	}

	/// <summary>
	/// 해당하는 데이터 타입을 서버에서 전송
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	private IEnumerator IEPostData(string postData, string link)
	{
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
				yield return www.downloadHandler.text;
			}
		}
	}

	/// <summary>
	/// 유저 데이터 관련 Get, Post
	/// </summary>
	private IEnumerator IEUserSaveDataPost(string postData)
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
				CollbackUserSaveDataProcess(newPost);
				yield return www.downloadHandler.text;
			}
		}
	}

	/// <summary>
	/// 유저 세이브 데이터의 콜백 메시지를 처리한다
	/// </summary>
	private void CollbackUserSaveDataProcess(UserSaveDataServer newPost)
	{
		switch (newPost.message)
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
