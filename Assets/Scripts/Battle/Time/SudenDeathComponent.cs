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
        private PencilCaseComponent _pencilCaseComponent = null;
        private Unit _playerPencilCase = null;
        private Unit _enemyPencilCase = null;

        //변수
        private bool _isSuddenDeath;

        public void SetInitialization(TimeComponent timeComponent, UnitComponent unitComponent, CardComponent cardComponent, CostComponent costComponent, PencilCaseComponent pencilCaseComponent)
		{
            _timeComponent = timeComponent;
            _unitCommand = unitComponent;
            _cardCommand = cardComponent;
            _costCommand = costComponent;
            _pencilCaseComponent = pencilCaseComponent;

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
                _timeComponent.SetTime(1);
                return;
            }

            //체력 비교
            if (_pencilCaseComponent.PlayerPencilCase.UnitStat.Hp > _pencilCaseComponent.EnemyPencilCase.UnitStat.Hp)
            {
                _timeComponent.Win();
                _timeComponent.SetFinallyEnd(true);
                return;
            }
            else if (_pencilCaseComponent.PlayerPencilCase.UnitStat.Hp < _pencilCaseComponent.EnemyPencilCase.UnitStat.Hp)
            {
                _timeComponent.Lose();
                _timeComponent.SetFinallyEnd(true);
                return;
            }
            else
            {
                _timeComponent.Lose();
                _timeComponent.SetFinallyEnd(true);
            }
        }
    }
}
