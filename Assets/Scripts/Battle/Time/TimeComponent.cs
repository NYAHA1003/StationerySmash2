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

        //참조 변수
        private UnitComponent _unitCommand = null;
        private CardComponent _cardCommand = null;
        private CostComponent _costCommand = null;
        private Unit _playerPencilCase = null;
        private Unit _enemyPencilCase = null;
        private StageData _stageData;

        //변수
        private float _timer = 0;
        private float _bonustime = 0;
        private bool _isSuddenDeath;
        private bool _isFinallyEnd;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="timeText"></param>
        public void SetInitialization(ref System.Action updateAction, StageData stageData, UnitComponent unitComponent, CardComponent cardComponent, CostComponent costComponent)
        {
            _stageData = stageData;
            _timer = _stageData.timeValue + _bonustime;

            this._unitCommand = unitComponent;
            this._cardCommand = cardComponent;
            this._costCommand = costComponent;

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
            _cardCommand.ClearCards();
            _unitCommand.ClearUnit();

            if (!_isSuddenDeath)
            {
                _cardCommand.SetMaxCard(8);
                _costCommand.SetCostSpeed(500);
                _isSuddenDeath = true;
                _timer = 60;
                return;
            }

            //체력 비교
            if (_playerPencilCase.UnitStat.Hp > _enemyPencilCase.UnitStat.Hp)
            {
                Debug.Log("서든데스 승리");
                _isFinallyEnd = true;
                return;
            }
            if (_playerPencilCase.UnitStat.Hp < _enemyPencilCase.UnitStat.Hp)
            {
                Debug.Log("서든데스 패배");
                _isFinallyEnd = true;
                return;
            }

            Debug.Log("서든데스 무승부");
            _isFinallyEnd = true;
        }
    }

}