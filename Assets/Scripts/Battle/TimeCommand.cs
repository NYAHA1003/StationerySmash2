using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using TMPro;

namespace Battle
{
    [System.Serializable]
    public class TimeCommand : BattleCommand
    {
        //�ν����� ���� ����
        [SerializeField]
        private TextMeshProUGUI _timeText;

        private StageData _stageData;
        private float _timer;
        private bool _isSuddenDeath;
        private bool _isFinallyEnd;

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="timeText"></param>
        public void SetInitialization(BattleManager battleManager)
        {
            _stageData = battleManager.CurrentStageData;
            _timer = _stageData.timeValue;
            battleManager.AddUpdateAction(UpdateTime);
        }

        /// <summary>
        /// �ð� ������Ʈ
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
        /// ���絥�� ����
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

            //ü�� ��
            if (_battleManager._myUnitDatasTemp[0].hp > _battleManager._enemyUnitDatasTemp[0].hp)
            {
                Debug.Log("���絥�� �¸�");
                _isFinallyEnd = true;
                return;
            }
            if (_battleManager._myUnitDatasTemp[0].hp < _battleManager._enemyUnitDatasTemp[0].hp)
            {
                Debug.Log("���絥�� �й�");
                _isFinallyEnd = true;
                return;
            }

            Debug.Log("���絥�� ���º�");
            _isFinallyEnd = true;
        }
    }

}