using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
	public class SudenDeathComponent : BattleComponent
    {
        //���� ����
        private TimeComponent _timeComponent = null;
        private UnitComponent _unitCommand = null;
        private CardComponent _cardCommand = null;
        private CostComponent _costCommand = null;
        private Unit _playerPencilCase = null;
        private Unit _enemyPencilCase = null;

        //����
        private bool _isSuddenDeath;

        public void SetInitialization(TimeComponent timeComponent, UnitComponent unitComponent, CardComponent cardComponent, CostComponent costComponent)
		{
            _timeComponent = timeComponent;
            _unitCommand = unitComponent;
            _cardCommand = cardComponent;
            _costCommand = costComponent;
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
                _timeComponent.SetTime(60);
                return;
            }

            //ü�� ��
            if (_playerPencilCase.UnitStat.Hp > _enemyPencilCase.UnitStat.Hp)
            {
                Debug.Log("���絥�� �¸�");
                _timeComponent.SetFinallyEnd(true);
                return;
            }
            if (_playerPencilCase.UnitStat.Hp < _enemyPencilCase.UnitStat.Hp)
            {
                Debug.Log("���絥�� �й�");
                _timeComponent.SetFinallyEnd(true);
                return;
            }

            Debug.Log("���絥�� ���º�");
            _timeComponent.SetFinallyEnd(true);
        }
    }
}
