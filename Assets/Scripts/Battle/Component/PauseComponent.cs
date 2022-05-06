using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace Battle
{
    [System.Serializable]
    public class PauseComponent : BattleComponent
    {
        private bool _isActive = false;

        [SerializeField]
        private RectTransform _pauseUI;
        [SerializeField]
        private Canvas _pauseCanvas = null;
        [SerializeField]
        private Button _puaseButton = null;
        [SerializeField]
        private Button _puaseBackgroundButton = null;
        [SerializeField]
        private Button _retryButton = null;
        [SerializeField]
        private Button _backhomeButton = null;
        [SerializeField]
        private SceneLoadComponenet _sceneLoadComponent;

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="pauseUI"></param>
        /// <param name="pauseCanvas"></param>
        public void SetInitialization()
        {
            _puaseButton.onClick.AddListener(() => OnPause());
            _puaseBackgroundButton.onClick.AddListener(() => OnPause());
            _retryButton.onClick.AddListener(() => _sceneLoadComponent.SceneLoadBattle());
            _backhomeButton.onClick.AddListener(() => _sceneLoadComponent.SceneLoadMain());

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

        /// <summary>
        /// Ŭ���ϸ� �Ͻ�������
        /// </summary>
        public void OnPause()
        {
            SetPause();
        }
    }

}