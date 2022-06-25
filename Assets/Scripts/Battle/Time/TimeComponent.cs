using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;
using TMPro;
using DG.Tweening;

namespace Battle
{
    [System.Serializable]
    public class TimeComponent : BattleComponent
    {
        //인스펙터 참조 변수
        [SerializeField]
        private RectTransform _timeHand;
        [SerializeField]
        private GameObject _suddenDeathTextObj;
        [SerializeField]
        private Image _timeDelay;
        [SerializeField]
        private GameObject _suddenDeathBackground;
        [SerializeField]
        private GameObject _suddenDeathGlow;
        [SerializeField]
        private AudioMixerGroup _bgmMixerGruop;

        //참조 변수
        private UnitComponent _unitCommand = null;
        private CardComponent _cardCommand = null;
        private CostComponent _costCommand = null;
        WinLoseComponent _winLoseComponent = null;
        private PencilCaseComponent _pencilCaseComponent = null;
        private SudenDeathComponent _sudenDeathComponent = null;
        private StageData _stageData;

        //변수
        private float _currentTimer = 0;
        private float _firstTimer = 0;
        private float _bonustime = 0;
        private bool _isFinallyEnd;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="timeText"></param>
        public void SetInitialization(ref System.Action updateAction, StageData stageData, UnitComponent unitComponent, CardComponent cardComponent, CostComponent costComponent, PencilCaseComponent pencilCaseComponent, WinLoseComponent winLoseComponent)
        {
            _sudenDeathComponent = new SudenDeathComponent();

            _stageData = stageData;
            _currentTimer = _stageData.timeValue + _bonustime;
            _firstTimer = _currentTimer;

            this._unitCommand = unitComponent;
            this._cardCommand = cardComponent;
            this._costCommand = costComponent;
            this._pencilCaseComponent = pencilCaseComponent;
            this._winLoseComponent = winLoseComponent;

            _sudenDeathComponent.SetInitialization(_suddenDeathTextObj, this, _unitCommand, _cardCommand, _costCommand, _pencilCaseComponent, _suddenDeathBackground, _suddenDeathGlow, _bgmMixerGruop);

            updateAction += UpdateTime;
        }

        /// <summary>
        /// 시간 설정
        /// </summary>
        /// <param name="time"></param>
        public void SetTime(float time)
        {
            _currentTimer = time;
            _firstTimer = time;
        }

        /// <summary>
        /// 게임이 완전히 끝났음을 설정
        /// </summary>
        /// <param name="boolean"></param>
        public void SetFinallyEnd(bool boolean)
		{
            _isFinallyEnd = boolean;
        }

        /// <summary>
        /// 시간 추가 설정
        /// </summary>
        /// <param name="time"></param>
        public void IncreaseTime(float time)
        {
            _bonustime = time;
        }
        
        /// <summary>
        /// 승리
        /// </summary>
        public void Win()
		{
            _winLoseComponent.SetWinLosePanel(true);
        }

        /// <summary>
        /// 패배
        /// </summary>
        public void Lose()
        {
            _winLoseComponent.SetWinLosePanel(false);
        }

        /// <summary>
        /// 시간 업데이트
        /// </summary>
        private void UpdateTime()
        {
            if (_isFinallyEnd)
            {
                return;
            }

            if (_stageData._timeType == TimeType.DisabledTime)
            {
                return;
            }

            if (_currentTimer > 0)
            {
                _currentTimer -= Time.deltaTime;
                float betweenValue = (_firstTimer - _currentTimer) / _firstTimer;
                _timeHand.rotation = Quaternion.Euler(new Vector3(0, 0, betweenValue * -360));
                _timeDelay.fillAmount = betweenValue;
                return;
            }

            SetSuddenDeath();
        }

        /// <summary>
        /// 서든데스 시작
        /// </summary>
        private void SetSuddenDeath()
        {
            _sudenDeathComponent.SetSuddenDeath();
        }
    }

}