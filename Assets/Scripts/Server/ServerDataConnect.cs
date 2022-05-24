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

public class ServerDataConnect : MonoBehaviour
{
	const string endPoint = "http://testsmash.kro.kr";
	const string GetSticker = "/smash/GetSticker";
	const string GetUnitData = "/smash/UnitData";
	const string GetStrategyData = "/smash/StrategyData";
	const string GetBadgeData = "/smash/BadgeData";
	const string GetPencilCaseData = "/smash/PencilCaseData";
	const string GetUserSaveData = "/smash/UserSaveData";

	/// <summary>
	/// ��ƼĿ ���� ������ ��������
	/// </summary>
	/// <param name="action"></param>
	public void GetStandardStickerData(Action<List<StickerData>> action)
	{
		StartCoroutine(IEGETSticker(action));
	}

	/// <summary>
	/// ��ƼĿ ������ ����Ʈ �������� ��������
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	private IEnumerator IEGETSticker(Action<List<StickerData>> action)
	{
		using (UnityWebRequest www = UnityWebRequest.Get(endPoint + GetSticker))
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
				action.Invoke(JsonUtility.FromJson<List<StickerData>>(www.downloadHandler.text));
			}
		}
	}

	/// <summary>
	/// ���� ���� ������ ��������
	/// </summary>
	/// <param name="action"></param>
	public void GetStandardUnitData(Action<List<UnitData>> action)
	{
		StartCoroutine(IEGETUnitData(action));
	}

	/// <summary>
	/// ���� ������ ����Ʈ �������� ��������
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	private IEnumerator IEGETUnitData(Action<List<UnitData>> action)
	{
		using (UnityWebRequest www = UnityWebRequest.Get(endPoint + GetUnitData))
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
				action.Invoke(JsonUtility.FromJson<List<UnitData>>(www.downloadHandler.text));
			}
		}
	}

	/// <summary>
	/// ���� ���� ������ ��������
	/// </summary>
	/// <param name="action"></param>
	public void GetStandardBadgeData(Action<List<BadgeData>> action)
	{
		StartCoroutine(IEGETBadgeData(action));
	}

	/// <summary>
	/// ���� ������ ����Ʈ �������� ��������
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	private IEnumerator IEGETBadgeData(Action<List<BadgeData>> action)
	{
		using (UnityWebRequest www = UnityWebRequest.Get(endPoint + GetBadgeData))
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
				action.Invoke(JsonUtility.FromJson<List<BadgeData>>(www.downloadHandler.text));
			}
		}
	}


	/// <summary>
	/// ���� ���� ������ ��������
	/// </summary>
	/// <param name="action"></param>
	public void GetStandardStrategyData(Action<List<StrategyData>> action)
	{
		StartCoroutine(IEGETStrategyData(action));
	}

	/// <summary>
	/// ���� ������ ����Ʈ �������� ��������
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	private IEnumerator IEGETStrategyData(Action<List<StrategyData>> action)
	{
		using (UnityWebRequest www = UnityWebRequest.Get(endPoint + GetBadgeData))
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
				action.Invoke(JsonUtility.FromJson<List<StrategyData>>(www.downloadHandler.text));
			}
		}
	}


	/// <summary>
	/// ���� ���� ������ ��������
	/// </summary>
	/// <param name="action"></param>
	public void GetStandardPencilCaseData(Action<List<PencilCaseData>> action)
	{
		StartCoroutine(IEGETPencilCaseData(action));
	}

	/// <summary>
	/// ���� ������ ����Ʈ �������� ��������
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	private IEnumerator IEGETPencilCaseData(Action<List<PencilCaseData>> action)
	{
		using (UnityWebRequest www = UnityWebRequest.Get(endPoint + GetPencilCaseData))
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
				action.Invoke(JsonUtility.FromJson<List<PencilCaseData>>(www.downloadHandler.text));
			}
		}
	}


	/// <summary>
	/// ���� ���̺� ������ ��������
	/// </summary>
	/// <param name="action"></param>
	public void GetStandardUserSaveData(Action<UserSaveData> action)
	{
		StartCoroutine(IEGETUserSaveData(action));
	}

	/// <summary>
	/// ���� ���̺� ������  �������� ��������
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	private IEnumerator IEGETUserSaveData(Action<UserSaveData> action)
	{
		using (UnityWebRequest www = UnityWebRequest.Get(endPoint + GetPencilCaseData))
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
				action.Invoke(JsonUtility.FromJson<UserSaveData>(www.downloadHandler.text));
			}
		}
	}
}
