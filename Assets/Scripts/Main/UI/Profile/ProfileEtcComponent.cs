using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Main.Deck;
using Utill.Data;
using Utill.Load;
using Utill.Tool;

public class ProfileEtcComponent : MonoBehaviour, IUserData
{
	[SerializeField]
	private TextMeshProUGUI _lastPlayStageText = null; //마지막 플레이한 스테이지 텍스트
	[SerializeField]
	private TextMeshProUGUI _winCountText = null; //승리한 횟수 텍스트
	[SerializeField]
	private TextMeshProUGUI _winningStreakCountText = null; //가장 크게 연승한 횟수 텍스트
	[SerializeField]
	private TextMeshProUGUI _loseCountText = null; //패배한 횟수 텍스트

	private void Awake()
	{
		UserSaveManagerSO.AddObserver(this);
	}

	public void Notify()
	{
		SetEtcData();
	}

	/// <summary>
	/// 기타 정보들 수정
	/// </summary>
	private void SetEtcData()
	{
		_lastPlayStageText.text = $"스토리 {System.Enum.GetName(typeof(BattleStageType), UserSaveManagerSO.UserSaveData._lastPlayStage)}";
		_winCountText.text = $"승리 {UserSaveManagerSO.UserSaveData._winCount}";
		_winningStreakCountText.text = $"연승{UserSaveManagerSO.UserSaveData._winningStreakCount}";
		_loseCountText.text = $"패배 {UserSaveManagerSO.UserSaveData._loseCount}";
	}

}
