using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

namespace Battle
{
    [System.Serializable]
    public class PencilCaseCommand
    {
        public PencilCaseDataSO PencilCaseDataMy => pencilCaseDataMy;
        public PencilCaseDataSO PencilCaseDataEnemy => pencilCaseDataEnemy;
        public PencilCaseUnit PlayerPencilCase => _playerPencilCase;
        public PencilCaseUnit EnemyPencilCase => _enemyPencilCase;
        public AbstractPencilCaseAbility PlayerAbilityState => _playerAbilityState;
        public AbstractPencilCaseAbility EnemyAbilityState => _enemyAbilityState;


        //참조 변수
        private UnitCommand _unitCommand = null;
        private StageData _stageData = null;
        private PencilCaseDataSO pencilCaseDataMy = null;
        private PencilCaseDataSO pencilCaseDataEnemy = null;
        private AbstractPencilCaseAbility _playerAbilityState = null;
        private AbstractPencilCaseAbility _enemyAbilityState = null;

        //인스펙터 참조 변수
        [SerializeField]
        private PencilCaseUnit _playerPencilCase = null;
        [SerializeField]
        private PencilCaseUnit _enemyPencilCase = null;
        
        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="pencilCase_My"></param>
        /// <param name="pencilCase_Enemy"></param>
        /// <param name="pencilCaseDataMy"></param>
        /// <param name="pencilCaseDataEnemy"></param>
        public void SetInitialization(UnitCommand unitCommand, StageData stageData)
        {
            this._unitCommand = unitCommand;
            this._stageData = stageData;

            pencilCaseDataMy = _playerPencilCase.PencilCaseData;
            pencilCaseDataEnemy = _enemyPencilCase.PencilCaseData;

            _playerPencilCase.SetUnitData(pencilCaseDataMy.PencilCasedataBase.pencilCaseData, TeamType.MyTeam, _stageData, -1, 1);
            _unitCommand._playerUnitList.Add(_playerPencilCase);
            _playerPencilCase.transform.position = new Vector2(-_stageData.max_Range, 0);
            _playerAbilityState = _playerPencilCase.AbilityState;
            _playerAbilityState.SetTeam(TeamType.MyTeam);

            _enemyPencilCase.SetUnitData(pencilCaseDataEnemy.PencilCasedataBase.pencilCaseData, TeamType.EnemyTeam, _stageData, -2, 1);
            _unitCommand._enemyUnitList.Add(_enemyPencilCase);
            _enemyPencilCase.transform.position = new Vector2(_stageData.max_Range, 0);
            _enemyAbilityState = _enemyPencilCase.AbilityState;
            _enemyAbilityState.SetTeam(TeamType.EnemyTeam);
        }

        /// <summary>
        /// 플레이어 필통능력 사용
        /// </summary>
        public void RunPlayerPencilCaseAbility()
        {
            _playerAbilityState.RunPencilCaseAbility();
        }

        /// <summary>
        /// 적 필통능력 사용
        /// </summary>
        public void RunEnemyPencilCaseAbility()
        {
            _enemyAbilityState.RunPencilCaseAbility();
        }

    }

}