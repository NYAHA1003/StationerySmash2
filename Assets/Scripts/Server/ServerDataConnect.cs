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
	//������ ���� enum
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
		public string message; //�������� ��ȯ�ϴ� �޽��� Ȥ�� ���� ������ ������ �޽���
		public UserSaveData post; //���� �����Ͱ�
	}
	private UserSaveDataServer postData;

	/// <summary>
	/// ������ Ÿ�Կ� ���� ���� �����Ϳ� �����ؼ� �޽��� ��ȯ
	/// </summary>
	/// <param name="action"></param>
	public void GetData<T>(Action<T> action, DataType dataType)
	{
		string link = ReturnLink(dataType, true);
		StartCoroutine(IEGETData(action, link));
	}

	/// <summary>
	/// ������ Ÿ�Կ� ���� ���� �����Ϳ� �����ؼ� �޽��� ����
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
	/// ���� ���� �����͸� ������ ������Ʈ�Ѵ�
	/// </summary>
	public void PostUserSaveData()
	{
		postData.message = "POST";
		postData.post = UserSaveManagerSO.UserSaveData;
		string jsonData = JsonUtility.ToJson(postData);
		StartCoroutine(IEUserSaveDataPost(jsonData));
	}

	/// <summary>
	/// ���� ���� �����͸� �����´�
	/// </summary>
	public void GetUserSaveData()
	{
		postData.message = "GET";
		string jsonData = JsonUtility.ToJson(postData);
		StartCoroutine(IEUserSaveDataPost(jsonData));
	}

	/// <summary>
	/// ������ Ÿ�Կ� ���� ��ũ�� ��ȯ�ϴ� �Լ�
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
	/// �ش��ϴ� ������ Ÿ���� �������� ��������
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
	/// �ش��ϴ� ������ Ÿ���� �������� ����
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
	/// ���� ������ ���� Get, Post
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
	/// ���� ���̺� �������� �ݹ� �޽����� ó���Ѵ�
	/// </summary>
	private void CollbackUserSaveDataProcess(UserSaveDataServer newPost)
	{
		switch (newPost.message)
		{
			case "UPDATE":
				Debug.Log("������Ʈ");
				break;
			case "FIND":
				Debug.Log("�˻�");
				this.postData = newPost;
				break;
			case "NONE":
				Debug.Log("��");
				break;
			default:
				Debug.LogError("Ȯ���� �� ���� �޽���");
				break;
		}
	}
}
