using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
	public class SudenDeathComponent : BattleComponent
    {
        //참조 변수
        private TimeComponent _timeComponent = null;
        private UnitComponent _unitCommand = null;
        private CardComponent _cardCommand = null;
        private CostComponent _costCommand = null;
        private Unit _playerPencilCase = null;
        private Unit _enemyPencilCase = null;

        //변수
        private bool _isSuddenDeath;

        public void SetInitialization(TimeComponent timeComponent, UnitComponent unitComponent, CardComponent cardComponent, CostComponent costComponent)
		{
            _timeComponent = timeComponent;
            _unitCommand = unitComponent;
            _cardCommand = cardComponent;
            _costCommand = costComponent;
        }

        /// <summary>
        /// 서든데스 시작
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

            //체력 비교
            if (_playerPencilCase.UnitStat.Hp > _enemyPencilCase.UnitStat.Hp)
            {
                Debug.Log("서든데스 승리");
                _timeComponent.SetFinallyEnd(true);
                return;
            }
            if (_playerPencilCase.UnitStat.Hp < _enemyPencilCase.UnitStat.Hp)
            {
                Debug.Log("서든데스 패배");
                _timeComponent.SetFinallyEnd(true);
                return;
            }

            Debug.Log("서든데스 무승부");
            _timeComponent.SetFinallyEnd(true);
        }
    }
}
