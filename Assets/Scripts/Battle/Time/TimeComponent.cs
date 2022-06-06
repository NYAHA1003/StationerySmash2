using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        private RectTransform _timeHand;
        [SerializeField]
        private Image _timeDelay;

        //���� ����
        private UnitComponent _unitCommand = null;
        private CardComponent _cardCommand = null;
        private CostComponent _costCommand = null;
        private PencilCaseComponent _pencilCaseComponent = null;
        private SudenDeathComponent _sudenDeathComponent = null;
        private StageData _stageData;

        //����
        private float _currentTimer = 0;
        private float _firstTimer = 0;
        private float _bonustime = 0;
        private bool _isFinallyEnd;

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="timeText"></param>
        public void SetInitialization(ref System.Action updateAction, StageData stageData, UnitComponent unitComponent, CardComponent cardComponent, CostComponent costComponent, PencilCaseComponent pencilCaseComponent)
        {
            _sudenDeathComponent = new SudenDeathComponent();

            _stageData = stageData;
            _currentTimer = _stageData.timeValue + _bonustime;
            _firstTimer = _currentTimer;

            this._unitCommand = unitComponent;
            this._cardCommand = cardComponent;
            this._costCommand = costComponent;
            this._pencilCaseComponent = pencilCaseComponent;

            _sudenDeathComponent.SetInitialization(this, _unitCommand, _cardCommand, _costCommand, _pencilCaseComponent);

            updateAction += UpdateTime;
        }

        /// <summary>
        /// �ð� ����
        /// </summary>
        /// <param name="time"></param>
        public void SetTime(float time)
        {
            _currentTimer = time;
        }

        /// <summary>
        /// ������ ������ �������� ����
        /// </summary>
        /// <param name="boolean"></param>
        public void SetFinallyEnd(bool boolean)
		{
            _isFinallyEnd = boolean;
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
        /// ���絥�� ����
        /// </summary>
        private void SetSuddenDeath()
        {
            _sudenDeathComponent.SetSuddenDeath();
        }
    }

}