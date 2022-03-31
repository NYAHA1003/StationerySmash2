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

        public void SetInitialization(BattleManager battleManager, TextMeshProUGUI timeText)
        {
            _stageData = battleManager.CurrentStageData;
            _timer = _stageData.timeValue;
            this._timeText = timeText;
        }

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

        public void SetSuddenDeath()
        {
            battleManager.CommandCard.ClearCards();
            battleManager.CommandUnit.ClearUnit();

            if (!_isSuddenDeath)
            {
                battleManager.CommandCard.SetMaxCard(8);
                battleManager.CommandCost.SetCostSpeed(500);
                _isSuddenDeath = true;
                _timer = 60;
                return;
            }

            //체력 비교
            if (battleManager._myUnitDatasTemp[0].hp > battleManager._enemyUnitDatasTemp[0].hp)
            {
                Debug.Log("서든데스 승리");
                _isFinallyEnd = true;
                return;
            }
            if (battleManager._myUnitDatasTemp[0].hp < battleManager._enemyUnitDatasTemp[0].hp)
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