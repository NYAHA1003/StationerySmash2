using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using TMPro;

namespace Battle
{
    [System.Serializable]
    public class TimeCommand
    {
        //�ν����� ���� ����
        [SerializeField]
        private TextMeshProUGUI _timeText;

        //���� ����
        private UnitCommand _unitCommand = null;
        private CardCommand _cardCommand = null;
        private CostCommand _costCommand = null;
        private Unit _playerPencilCase = null;
        private Unit _enemyPencilCase = null;

        private StageData _stageData;
        private float _timer;
        private bool _isSuddenDeath;
        private bool _isFinallyEnd;

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="timeText"></param>
        public void SetInitialization(ref System.Action updateAction, StageData stageData)
        {
            _stageData = stageData;
            _timer = _stageData.timeValue;
            updateAction += UpdateTime;
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
            if (_playerPencilCase.hp > _enemyPencilCase.hp)
            {
                Debug.Log("���絥�� �¸�");
                _isFinallyEnd = true;
                return;
            }
            if (_playerPencilCase.hp < _enemyPencilCase.hp)
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