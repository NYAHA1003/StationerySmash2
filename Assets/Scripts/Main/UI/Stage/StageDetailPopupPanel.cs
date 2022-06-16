using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
	private CurrentStageDataSO _currentStageDataSO = null;
	[SerializeField]
	private Button _closeButton = null;

	public void Start()
	{
		_closeButton.onClick.AddListener(() => gameObject.SetActive(false));
	}

	/// <summary>
	/// 스테이지 팝업창 설정
	/// </summary>
	public void Setting()
	{
		_stageNameText.text = _currentStageDataSO._currentStageDatas._stageName;
		_moneyText.text = _currentStageDataSO._currentStageDatas._rewardMoney.ToString();
		_expText.text = _currentStageDataSO._currentStageDatas._rewardMoney.ToString();
		gameObject.SetActive(true);
	}
}
