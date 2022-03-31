using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Battle
{
    public class PauseCommand : BattleCommand
    {
        private bool _isActive = false;
        private RectTransform _pauseUI;
        private Canvas _pauseCanvas;

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="pauseUI"></param>
        /// <param name="pauseCanvas"></param>
        public void SetInitialization(BattleManager battleManager, RectTransform pauseUI, Canvas pauseCanvas)
        {
            this._battleManager = battleManager;
            this._pauseUI = pauseUI;
            this._pauseCanvas = pauseCanvas;
        }

        /// <summary>
        /// �Ͻ����� â�� ���� Ű�� �Լ�
        /// </summary>
        public void SetPause()
        {
            _isActive = !_isActive;
            _pauseCanvas.gameObject.SetActive(_isActive);
            if (_isActive)
            {
                Time.timeScale = 0;
                SetAnimation();
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        /// <summary>
        /// �Ͻ����� �ִϸ��̼�
        /// </summary>
        private void SetAnimation()
        {
            _pauseUI.DOKill();
            _pauseUI.anchoredPosition = new Vector2(Screen.width, 0);
            _pauseUI.DOAnchorPosX(0, 1).SetUpdate(true);
        }


    }

}