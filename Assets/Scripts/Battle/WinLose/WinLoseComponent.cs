using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Utill.Tool;
using TMPro;
using Main.Setting;
namespace Battle
{
	[System.Serializable]
	public class WinLoseComponent : BattleComponent
	{
		[SerializeField]
		private Canvas _cardCanvas;
		[SerializeField]
		private Canvas _winLoseCanvas;
		[SerializeField]
		private RectTransform _winPanel;
		[SerializeField]
		private RectTransform _losePanel;
		[SerializeField]
		private RectTransform _winText;
		[SerializeField]
		private RectTransform _loseText;
		[SerializeField]
		private TextMeshProUGUI _playerHPText;
		[SerializeField]
		private TextMeshProUGUI _enemyHPText;
		[SerializeField]
		private AudioMixerGroup _bgmMixerGroup;

		private List<IWinLose> _observers = new List<IWinLose>(); //관찰자들
		private PencilCaseComponent _pencilCaseComponent = null;
		private StageData _currentStageData = null;

		/// <summary>
		/// 초기화
		/// </summary>
		/// <param name="battleManager"></param>
		/// <param name="winLoseCanvas"></param>
		/// <param name="winPanel"></param>
		/// <param name="losePanel"></param>
		public void SetInitialization(PencilCaseComponent pencilCaseComponent, StageData currentStageDataSO)
		{
			_pencilCaseComponent = pencilCaseComponent;
			_currentStageData = currentStageDataSO;
		}

		/// <summary>
		/// 관찰자들에게 게임이 종료됨을 알린다.
		/// </summary>
		public void SendEndGame(bool isWin)
		{
			int count = _observers.Count;
			for (int i = 0; i < count; i++)
			{
				//게임이 끝났음을 모든 관찰자들에게 전달
				_observers[i].Notify(isWin);
			}
		}

		/// <summary>
		/// 관찰자를 등록한다
		/// </summary>
		public void AddObservers(IWinLose observer)
		{
			_observers.Add(observer);
		}

		/// <summary>
		/// 승리,패배 패널 키기
		/// </summary>
		/// <param name="isWin"></param>
		public void SetWinLosePanel(bool isWin)
		{
			_bgmMixerGroup.audioMixer.SetFloat("BGMPitch", 1f);
			_playerHPText.text = $"내 체력: {_pencilCaseComponent.PlayerPencilCase.UnitStat.Hp}";
			_enemyHPText.text = $"상대 체력: {_pencilCaseComponent.EnemyPencilCase.UnitStat.Hp}";

			_winLoseCanvas.gameObject.SetActive(true);
			_cardCanvas.gameObject.SetActive(false);

			_winPanel.gameObject.SetActive(isWin);
			_losePanel.gameObject.SetActive(!isWin);
			if (isWin)
			{
				//마지막 스테이지 등록
				UserSaveManagerSO.SetLastClearStage(_currentStageData._stageType);
				_winPanel.sizeDelta = new Vector2(3, 3);
				_winPanel.DOScale(1, 0.3f).SetEase(Ease.OutExpo).
					OnComplete(() =>
					{

						UserSaveManagerSO.AddExp(_currentStageData._rewardExp);
						UserSaveManagerSO.AddMoney(_currentStageData._rewardMoney);
						_winText.DOScale(1.5f, 0.3f).SetLoops(-1, LoopType.Yoyo);
					});
				Sound.StopBgm(3);
				Sound.PlayBgm(7);
				return;
			}
			_losePanel.sizeDelta = new Vector2(3, 3);
			_losePanel.DOScale(1, 0.3f).SetEase(Ease.OutExpo).
					OnComplete(() =>
					{
						_loseText.DOScale(1.5f, 0.3f).SetLoops(-1, LoopType.Yoyo);
					});
			Sound.StopBgm(3);
			Sound.PlayBgm(8);
		}
		
		

	}

}