using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using Main.Event;
using Utill.Data;

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

        public void OnDestroy()
        {
            EventManager.StopListening(EventsType.Pause, OnPause);
        }
        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="pauseUI"></param>
        /// <param name="pauseCanvas"></param>
        public void SetInitialization()
        {
            EventManager.StopListening(EventsType.Pause, OnPause);  
            EventManager.StartListening(EventsType.Pause, OnPause);
        }
       
        /// <summary>
        /// 일시정지 창을 껐다 키는 함수
        /// </summary>
        private void SetPause()
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
        /// 일시정지 애니메이션
        /// </summary>
        private void SetAnimation()
        {
            _pauseUI.DOKill();
            _pauseUI.anchoredPosition = new Vector2(Screen.width, 0);
            _pauseUI.DOAnchorPosX(0, 1).SetUpdate(true);
        }

        /// <summary>
        /// 클릭하면 일시정지함
        /// </summary>
        private void OnPause()
        {
            SetPause();
        }
    }

}