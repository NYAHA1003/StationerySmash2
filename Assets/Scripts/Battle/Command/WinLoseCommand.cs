using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Battle
{
    [System.Serializable]
    public class WinLoseCommand
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

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="winLoseCanvas"></param>
        /// <param name="winPanel"></param>
        /// <param name="losePanel"></param>
        public void SetInitialization(BattleManager battleManager)
        {

        }

        /// <summary>
        /// 승리,패배 패널 키기
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