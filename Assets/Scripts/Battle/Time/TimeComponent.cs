using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using TMPro;

namespace Battle
{
    [System.Serializable]
    public class TimeComponent : BattleComponent
    {
        //인스펙터 참조 변수
        [SerializeField]
        private TextMeshProUGUI _timeText;
        [SerializeField]
        private Unit _playerPencilCase = null;
        [SerializeField]
        private Unit _enemyPencilCase = null;

        //참조 변수
        private UnitComponent _unitCommand = null;
        private CardComponent _cardCommand = null;
        private CostComponent _costCommand = null;
        private SudenDeathComponent _sudenDeathComponent = null;
        private StageData _stageData;

        //변수
        private float _timer = 0;
        private float _bonustime = 0;
        private bool _isFinallyEnd;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="timeText"></param>
        public void SetInitialization(ref System.Action updateAction, StageData stageData, UnitComponent unitComponent, CardComponent cardComponent, CostComponent costComponent)
        {
            _sudenDeathComponent = new SudenDeathComponent();

            _stageData = stageData;
            _timer = _stageData.timeValue + _bonustime;

            this._unitCommand = unitComponent;
            this._cardCommand = cardComponent;
            this._costCommand = costComponent;

            _sudenDeathComponent.SetInitialization(this, _unitCommand, _cardCommand, _costCommand);

            updateAction += UpdateTime;
        }

        /// <summary>
        /// 시간 설정
        /// </summary>
        /// <param name="time"></param>
        public void SetTime(float time)
        {
            _timer = time;
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
        /// 시간 업데이트
        /// </summary>
        private void UpdateTime()
        {
            if (_isFinallyEnd)
            {
                return;
            }

            if (_stageData.timeType == TimeType.DisabledTime)
            {
                return;
            }

            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                _timeText.text = $"{(int)_timer / 60}:{(int)_timer % 60}";
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