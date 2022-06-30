using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Battle;
using Utill.Tool;
using Utill.Data;
using Utill.Load;
using Main.Event;

public class StageDetailPopupPanel : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _stageNameText = null;
	[SerializeField]
	private TextMeshProUGUI _moneyText = null;
	[SerializeField]
	private TextMeshProUGUI _expText = null;
	[SerializeField]
	private Image _previewImage = null;
	[SerializeField]
	private Button _closeButton = null;

	private WarrningComponent _warrningComponent = null;

	public void Start()
	{
		_warrningComponent = FindObjectOfType<WarrningComponent>();
		_closeButton.onClick.AddListener(() => gameObject.SetActive(false));
		EventManager.Instance.StartListening(Utill.Data.EventsType.SetNormalBattle, NormalBattle);
		EventManager.Instance.StartListening(Utill.Data.EventsType.SetHardBattle, HardBattle);
	}

	private void OnDisable()
	{
		//�θ� ������ ���� ������ ����
		gameObject.SetActive(false);
	}

	/// <summary>
	/// �������� �˾�â ����
	/// </summary>
	public void Setting()
	{
		_stageNameText.text = AIAndStageData.Instance._currentStageDatas._stageName;
		_moneyText.text = AIAndStageData.Instance._currentStageDatas._rewardMoney.ToString();
		_expText.text = AIAndStageData.Instance._currentStageDatas._rewardExp.ToString();
		gameObject.SetActive(true);
	}

	/// <summary>
	/// �Ϲ����� ����
	/// </summary>
	private void NormalBattle()
	{
		if(AIAndStageData.Instance._isEventMode)
		{
			if(UserSaveManagerSO.UserSaveData._coupon == 0)
			{
				_warrningComponent.SetWarrning("������ �����մϴ�");
				return;
			}
			UserSaveManagerSO.UserSaveData.AddCoupon(-1);
		}
		BattleManager.IsHardMode = false;
		EventManager.Instance.TriggerEvent(Utill.Data.EventsType.LoadBattleScene);
	}

	/// <summary>
	/// ����� ���� ����
	/// </summary>
	private void HardBattle()
	{
		if (AIAndStageData.Instance._isEventMode)
		{
			if (UserSaveManagerSO.UserSaveData._coupon == 0)
			{
				_warrningComponent.SetWarrning("������ �����մϴ�");
				return;
			}
			UserSaveManagerSO.UserSaveData.AddCoupon(-1);
		}
		EventManager.Instance.TriggerEvent(Utill.Data.EventsType.LoadBattleScene);
		BattleManager.IsHardMode = true;
	}
}
