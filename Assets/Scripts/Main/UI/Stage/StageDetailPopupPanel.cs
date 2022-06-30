using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Battle;
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

	public void Start()
	{
		_closeButton.onClick.AddListener(() => gameObject.SetActive(false));
		EventManager.Instance.StartListening(Utill.Data.EventsType.SetNormalBattle, NormalBattle);
		EventManager.Instance.StartListening(Utill.Data.EventsType.SetHardBattle, HardBattle);
	}

	private void OnDisable()
	{
		//부모가 꺼지면 같이 꺼지게 만듦
		gameObject.SetActive(false);
	}

	/// <summary>
	/// 스테이지 팝업창 설정
	/// </summary>
	public void Setting()
	{
		_stageNameText.text = AIAndStageData.Instance._currentStageDatas._stageName;
		_moneyText.text = AIAndStageData.Instance._currentStageDatas._rewardMoney.ToString();
		_expText.text = AIAndStageData.Instance._currentStageDatas._rewardExp.ToString();
		gameObject.SetActive(true);
	}

	/// <summary>
	/// 일반전투 돌입
	/// </summary>
	private void NormalBattle()
	{
		BattleManager.IsHardMode = false;
	}

	/// <summary>
	/// 어려운 전투 돌입
	/// </summary>
	private void HardBattle()
	{
		BattleManager.IsHardMode = true;
	}
}
