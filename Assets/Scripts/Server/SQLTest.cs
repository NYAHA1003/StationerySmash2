using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Main.Deck;
using Utill.Data;
using Utill.Tool;

public class SQLTest : MonoBehaviour
{
	const string endPoint = "http://testsmash.kro.kr";
	const string PostUserSaveData = "/post/json";
	const string GetUserSaveData = "/get/json";

	public Post post;

	[System.Serializable]
	public class tempData
	{
		public string name;
	}

	[System.Serializable]
	public class Post
	{
		public UserSaveData post;
	}


	public class find
	{
		public string _userID;
	}

	[ContextMenu("테스트 포스트")]
	public void TestPost()
	{
		UserSaveData userSaveData = new UserSaveData
		{
			_userID = "Test10000",
			_haveCardSaveDatas =
			{
				new CardSaveData { _level = 0, stickerType = Utill.Data.StickerType.None, _cardNamingType = Utill.Data.CardNamingType.Pencil, _cardType = Utill.Data.CardType.SummonUnit, _count = 0, _skinType = Utill.Data.SkinType.PencilNormal, _strategicType = Utill.Data.StrategyType.None, _unitType = Utill.Data.UnitType.None },
				new CardSaveData { _level = 1, stickerType = Utill.Data.StickerType.None, _cardNamingType = Utill.Data.CardNamingType.Pencil, _cardType = Utill.Data.CardType.SummonUnit, _count = 0, _skinType = Utill.Data.SkinType.PencilNormal, _strategicType = Utill.Data.StrategyType.None, _unitType = Utill.Data.UnitType.None },
				new CardSaveData { _level = 2, stickerType = Utill.Data.StickerType.None, _cardNamingType = Utill.Data.CardNamingType.Pencil, _cardType = Utill.Data.CardType.SummonUnit, _count = 0, _skinType = Utill.Data.SkinType.PencilNormal, _strategicType = Utill.Data.StrategyType.None, _unitType = Utill.Data.UnitType.None },
				new CardSaveData { _level = 3, stickerType = Utill.Data.StickerType.None, _cardNamingType = Utill.Data.CardNamingType.Pencil, _cardType = Utill.Data.CardType.SummonUnit, _count = 0, _skinType = Utill.Data.SkinType.PencilNormal, _strategicType = Utill.Data.StrategyType.None, _unitType = Utill.Data.UnitType.None },
			},
			_ingameSaveDatas =
			{
				new CardSaveData { _level = 0, stickerType = Utill.Data.StickerType.Armor, _cardNamingType = Utill.Data.CardNamingType.Pencil, _cardType = Utill.Data.CardType.SummonUnit, _count = 0, _skinType = Utill.Data.SkinType.PencilNormal, _strategicType = Utill.Data.StrategyType.None, _unitType = Utill.Data.UnitType.None },
				new CardSaveData { _level = 1, stickerType = Utill.Data.StickerType.Armor, _cardNamingType = Utill.Data.CardNamingType.Pencil, _cardType = Utill.Data.CardType.SummonUnit, _count = 0, _skinType = Utill.Data.SkinType.PencilNormal, _strategicType = Utill.Data.StrategyType.None, _unitType = Utill.Data.UnitType.None },
				new CardSaveData { _level = 2, stickerType = Utill.Data.StickerType.Armor, _cardNamingType = Utill.Data.CardNamingType.Pencil, _cardType = Utill.Data.CardType.SummonUnit, _count = 0, _skinType = Utill.Data.SkinType.PencilNormal, _strategicType = Utill.Data.StrategyType.None, _unitType = Utill.Data.UnitType.None },
				new CardSaveData { _level = 3, stickerType = Utill.Data.StickerType.Armor, _cardNamingType = Utill.Data.CardNamingType.Pencil, _cardType = Utill.Data.CardType.SummonUnit, _count = 0, _skinType = Utill.Data.SkinType.PencilNormal, _strategicType = Utill.Data.StrategyType.None, _unitType = Utill.Data.UnitType.None },
			},
			_haveSkinList =
			{
				Utill.Data.SkinType.PencilNormal,
				Utill.Data.SkinType.PencilSewing,
				Utill.Data.SkinType.PostItChristmas,
				Utill.Data.SkinType.RulerHotSummer,
			},
			_haveStickerList =
			{
				new StickerSaveData { _level = 0, _stickerType = StickerType.None },
				new StickerSaveData { _level = 1, _stickerType = StickerType.None },
				new StickerSaveData { _level = 2, _stickerType = StickerType.None },
				new StickerSaveData { _level = 3, _stickerType = StickerType.None },
			},
			_havePencilCaseList =
			{
				PencilCaseType.Gold,
				PencilCaseType.Normal,
				PencilCaseType.Pencil,
				PencilCaseType.Princess,
			},
			_currentPencilCaseType = PencilCaseType.Traffic,
			_haveBadgeSaveDatas =
			{
				new BadgeSaveData { _level = 0, _BadgeType = BadgeType.None },
				new BadgeSaveData { _level = 1, _BadgeType = BadgeType.Discount },
				new BadgeSaveData { _level = 2, _BadgeType = BadgeType.Blanket },
				new BadgeSaveData { _level = 3, _BadgeType = BadgeType.Health },
			},
			_currentProfileType = ProfileType.ProNone,
			_haveProfileList =
			{
				ProfileType.ProEraser,
				ProfileType.ProMechaPencil,
				ProfileType.ProNone,
				ProfileType.ProPencil,
			},
			_materialDatas =
			{
				new MaterialData{name = "asd", _count = 0, _materialType = MaterialType.None, _sprite = null},
				new MaterialData{name = "asd1", _count = 1, _materialType = MaterialType.GlassPiece, _sprite = null},
				new MaterialData{name = "asd2", _count = 2, _materialType = MaterialType.ClothPiece, _sprite = null},
				new MaterialData{name = "asd3", _count = 3, _materialType = MaterialType.PlasticPiece, _sprite = null},
			},
			_haveCollectionDatas =
			{
				CollectionThemeType.None,
				CollectionThemeType.SewingSet,
				CollectionThemeType.HotSummerSet,
				CollectionThemeType.ChristmasSet,
			},
			_setPrestIndex = 0,
			_money = 1000,
			_dalgona = 1200,
			_name = "namesta",
			_level = 10,
			_nowExp = 100,
			_lastPlayStage = StageType.None,
			_winCount = 10,
			_winningStreakCount = 10,
			_loseCount = 20,
		};

		tempData tempData = new tempData
		{
			name = "name123",
		};

		string data = JsonUtility.ToJson(userSaveData);
		Debug.Log(data);
		StartCoroutine(IEPost(data));
	}


	/// <summary>
	/// 스티커 데이터 리스트 서버에서 가져오기
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	private IEnumerator IEPost(string jsonData)
	{
		using (UnityWebRequest www = UnityWebRequest.Post(endPoint + PostUserSaveData, jsonData))
		{
			www.method = "POST";
			www.SetRequestHeader("Content-Type", "application/json");
			var jsonBytes = Encoding.UTF8.GetBytes(jsonData);
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

	[ContextMenu("테스트 겟")]
	public void TestGet()
	{
		find data = new find
		{
			_userID = "Test10000"
		};

		string jsondata = JsonUtility.ToJson(data);
		StartCoroutine(IEGet(jsondata));
	}

	/// <summary>
	/// 스티커 데이터 리스트 서버에서 가져오기
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	private IEnumerator IEGet(string userID)
	{
		using (UnityWebRequest www = UnityWebRequest.Post(endPoint + GetUserSaveData, userID))
		{
			www.method = "POST";
			www.SetRequestHeader("Content-Type", "application/json");
			var jsonBytes = Encoding.UTF8.GetBytes(userID);
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
				post = JsonUtility.FromJson<Post>(www.downloadHandler.text);
				yield return www.downloadHandler.text;
			}
		}
	}
}
