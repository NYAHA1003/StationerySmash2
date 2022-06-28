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
using Main.Event;

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
        [SerializeField]
        private Canvas _rouletteCanvas;

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
            EventManager.Instance.StartListening(Utill.Data.EventsType.ActiveWinCanvas, WinPanelActive);
            EventManager.Instance.StartListening(Utill.Data.EventsType.ActiveLoseCanvas, LosePanelActive);
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
            if(isWin)
			{
                _rouletteCanvas.gameObject.SetActive(true);
            }
            else
			{
                LosePanelActive();

            }
            _bgmMixerGroup.audioMixer.SetFloat("BGMPitch", 1f);
            _playerHPText.text = $"내 체력: {_pencilCaseComponent.PlayerPencilCase.UnitStat.Hp}";
            _enemyHPText.text = $"상대 체력: {_pencilCaseComponent.EnemyPencilCase.UnitStat.Hp}";
            _cardCanvas.gameObject.SetActive(false);

        }

        /// <summary>
        /// 승리패널 띄우기
        /// </summary>
        private void WinPanelActive()
        {
            _winLoseCanvas.gameObject.SetActive(true);
            _winPanel.gameObject.SetActive(true);

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
            Sound.StopBgm(Utill.Data.BGMSoundType.Stage1);
            Sound.PlayBgm(Utill.Data.BGMSoundType.Win);
        }

        /// <summary>
        /// 패배 패널 띄우기
        /// </summary>
        private void LosePanelActive()
        {
            _winLoseCanvas.gameObject.SetActive(true);
            _losePanel.gameObject.SetActive(true);

            _losePanel.sizeDelta = new Vector2(3, 3);
            _losePanel.DOScale(1, 0.3f).SetEase(Ease.OutExpo).
                    OnComplete(() =>
                    {
                        _loseText.DOScale(1.5f, 0.3f).SetLoops(-1, LoopType.Yoyo);
                    });
            Sound.StopBgm(Utill.Data.BGMSoundType.Stage1);
            Sound.PlayBgm(Utill.Data.BGMSoundType.Loose);
        }


    }

}