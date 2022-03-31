using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using TMPro;

namespace Battle
{
    public class TimeCommand : BattleCommand
    {

        private StageData _stageData;
        private float _timer;
        private TextMeshProUGUI _timeText;
        private bool _isSuddenDeath;
        private bool _isFinallyEnd;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="timeText"></param>
        public void SetInitialization(BattleManager battleManager, TextMeshProUGUI timeText)
        {
            _stageData = battleManager.CurrentStageData;
            _timer = _stageData.timeValue;
            this._timeText = timeText;
            battleManager.AddUpdateAction(UpdateTime);
        }

        /// <summary>
        /// 시간 업데이트
        /// </summary>
        public void UpdateTime()
        {
            if (_isFinallyEnd) return;

            if (_stageData.timeType == TimeType.DisabledTime)
                return;

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
        public void SetSuddenDeath()
        {
            _battleManager.CommandCard.ClearCards();
            _battleManager.CommandUnit.ClearUnit();

            if (!_isSuddenDeath)
            {
                _battleManager.CommandCard.SetMaxCard(8);
                _battleManager.CommandCost.SetCostSpeed(500);
                _isSuddenDeath = true;
                _timer = 60;
                return;
            }

            //체력 비교
            if (_battleManager._myUnitDatasTemp[0].hp > _battleManager._enemyUnitDatasTemp[0].hp)
            {
                Debug.Log("서든데스 승리");
                _isFinallyEnd = true;
                return;
            }
            if (_battleManager._myUnitDatasTemp[0].hp < _battleManager._enemyUnitDatasTemp[0].hp)
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