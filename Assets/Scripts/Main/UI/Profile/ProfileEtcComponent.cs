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

public class ProfileEtcComponent : MonoBehaviour, IUserData
{
	[SerializeField]
	private TextMeshProUGUI _lastPlayStageText = null; //������ �÷����� �������� �ؽ�Ʈ
	[SerializeField]
	private TextMeshProUGUI _winCountText = null; //�¸��� Ƚ�� �ؽ�Ʈ
	[SerializeField]
	private TextMeshProUGUI _winningStreakCountText = null; //���� ũ�� ������ Ƚ�� �ؽ�Ʈ
	[SerializeField]
	private TextMeshProUGUI _loseCountText = null; //�й��� Ƚ�� �ؽ�Ʈ

	private void Awake()
	{
		SaveManager._instance.SaveData.AddObserver(this);
	}

	public void Notify(ref UserSaveData userSaveData)
	{
		SetEtcData(ref userSaveData);
	}

	/// <summary>
	/// ��Ÿ ������ ����
	/// </summary>
	private void SetEtcData(ref UserSaveData userSaveData)
	{
		_lastPlayStageText.text = $"���丮 {System.Enum.GetName(typeof(StageType), userSaveData._lastPlayStage)}";
		_winCountText.text = $"�¸� {userSaveData._winCount}";
		_winningStreakCountText.text = $"����{userSaveData._winningStreakCount}";
		_loseCountText.text = $"�й� {userSaveData._loseCount}";
	}

}
