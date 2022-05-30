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
		public string message; //�������� ��ȯ�ϴ� �޽��� Ȥ�� ���� ������ ������ �޽���
		public UserSaveData post; //���� �����Ͱ�
	}
	private UserSaveDataServer postData;

	/// <summary>
	/// ���� ���� �����͸� ������ ������Ʈ�Ѵ�
	/// </summary>
	public void PostUserSaveData()
	{
		postData.message = "POST";
		postData.post = UserSaveManagerSO.UserSaveData;
		string jsonData = JsonUtility.ToJson(postData);
		StartCoroutine(IEPost(jsonData));
	}

	/// <summary>
	/// ���� ���� �����͸� �����´�
	/// </summary>
	public void GetUserSaveData()
	{
		postData.message = "GET";
		string jsonData = JsonUtility.ToJson(postData);
		StartCoroutine(IEPost(jsonData));
	}

	/// <summary>
	/// ���� ������ ���� Get, Post
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
	/// �޽����� ó���Ѵ�
	/// </summary>
	private void CollbackMessageProcess(UserSaveDataServer newPost)
	{
		switch(newPost.message)
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
