using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Battle
{
    [System.Serializable]
    public class WinLoseComponent : BattleComponent
    {
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
        private Button _loseRetryButton;
        [SerializeField]
        private Button _loseBackHomeButton;
        [SerializeField]
        private Button _winRetryButton;
        [SerializeField]
        private Button _winBackHomeButton;
        [SerializeField]
        private SceneLoadComponenet _sceneLoadComponent;

        private List<IWinLose> _observers = new List<IWinLose>(); //�����ڵ�

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="winLoseCanvas"></param>
        /// <param name="winPanel"></param>
        /// <param name="losePanel"></param>
        public void SetInitialization()
        {
            _winRetryButton.onClick.AddListener(() => _sceneLoadComponent.SceneLoadBattle());
            _winBackHomeButton.onClick.AddListener(() => _sceneLoadComponent.SceneLoadMain());
            _loseRetryButton.onClick.AddListener(() => _sceneLoadComponent.SceneLoadBattle());
            _loseBackHomeButton.onClick.AddListener(() => _sceneLoadComponent.SceneLoadMain());
        }

        /// <summary>
        /// �����ڵ鿡�� ������ ������� �˸���.
        /// </summary>
        public void SendEndGame(bool isWin)
		{
            int count = _observers.Count;
            for (int i = 0; i < count; i++)
			{
                //������ �������� ��� �����ڵ鿡�� ����
                _observers[i].Notify(isWin);
			}
		}

        /// <summary>
        /// �����ڸ� ����Ѵ�
        /// </summary>
        public void AddObservers(IWinLose observer)
		{
            _observers.Add(observer);
		}

        /// <summary>
        /// �¸�,�й� �г� Ű��
        /// </summary>
        /// <param name="isWin"></param>
        public void SetWinLosePanel(bool isWin)
        {
            _winLoseCanvas.gameObject.SetActive(true);

            _winPanel.gameObject.SetActive(isWin);
            _losePanel.gameObject.SetActive(!isWin);
            if (isWin)
            {
                _winPanel.sizeDelta = new Vector2(3, 3);
                _winPanel.DOScale(1, 0.3f).SetEase(Ease.OutExpo).
                    OnComplete(() =>
                    {
                        _winText.DOScale(1.5f, 0.3f).SetLoops(-1, LoopType.Yoyo);
                    });
                return;
            }
            _losePanel.sizeDelta = new Vector2(3, 3);
            _losePanel.DOScale(1, 0.3f).SetEase(Ease.OutExpo).
                    OnComplete(() =>
                    {
                        _loseText.DOScale(1.5f, 0.3f).SetLoops(-1, LoopType.Yoyo);
                    });

        }
    }

}