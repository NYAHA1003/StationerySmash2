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
	private TextMeshProUGUI _lastPlayStageText = null; //������ �÷����� �������� �ؽ�Ʈ
	[SerializeField]
	private TextMeshProUGUI _winCountText = null; //�¸��� Ƚ�� �ؽ�Ʈ
	[SerializeField]
	private TextMeshProUGUI _winningStreakCountText = null; //���� ũ�� ������ Ƚ�� �ؽ�Ʈ
	[SerializeField]
	private TextMeshProUGUI _loseCountText = null; //�й��� Ƚ�� �ؽ�Ʈ

	private void Awake()
	{
		UserSaveManagerSO.AddObserver(this);
	}

	public void Notify()
	{
		SetEtcData();
	}

	/// <summary>
	/// ��Ÿ ������ ����
	/// </summary>
	private void SetEtcData()
	{
		_lastPlayStageText.text = $"���丮 {System.Enum.GetName(typeof(BattleStageType), UserSaveManagerSO.UserSaveData._lastPlayStage)}";
		_winCountText.text = $"�¸� {UserSaveManagerSO.UserSaveData._winCount}";
		_winningStreakCountText.text = $"����{UserSaveManagerSO.UserSaveData._winningStreakCount}";
		_loseCountText.text = $"�й� {UserSaveManagerSO.UserSaveData._loseCount}";
	}

}
