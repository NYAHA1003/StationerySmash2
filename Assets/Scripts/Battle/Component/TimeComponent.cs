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
        //�ν����� ���� ����
        [SerializeField]
        private TextMeshProUGUI _timeText;

        //���� ����
        private UnitComponent _unitCommand = null;
        private CardComponent _cardCommand = null;
        private CostComponent _costCommand = null;
        private Unit _playerPencilCase = null;
        private Unit _enemyPencilCase = null;
        private StageData _stageData;

        //����
        private float _timer = 0;
        private float _bonustime = 0;
        private bool _isSuddenDeath;
        private bool _isFinallyEnd;

        /// <summary>
        /// �ʱ�ȭ
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
        /// �ð� ����
        /// </summary>
        /// <param name="time"></param>
        public void SetTime(float time)
        {
            _timer = time;
        }

        /// <summary>
        /// �ð� �߰� ����
        /// </summary>
        /// <param name="time"></param>
        public void IncreaseTime(float time)
        {
            _bonustime = time;
        }

        /// <summary>
        /// �ð� ������Ʈ
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
        /// ���絥�� ����
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

            //ü�� ��
            if (_playerPencilCase.UnitStat.Hp > _enemyPencilCase.UnitStat.Hp)
            {
                Debug.Log("���絥�� �¸�");
                _isFinallyEnd = true;
                return;
            }
            if (_playerPencilCase.UnitStat.Hp < _enemyPencilCase.UnitStat.Hp)
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