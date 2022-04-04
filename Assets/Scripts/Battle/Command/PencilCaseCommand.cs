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

        //참조 변수
        private UnitCommand _unitCommand = null;
        private StageData _stageData = null;
        private PencilCaseDataSO pencilCaseDataMy;
        private PencilCaseDataSO pencilCaseDataEnemy;
        private PencilCaseAbilityState _playerAbilityState;
        private PencilCaseAbilityState _enemyAbilityState;

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

            _playerPencilCase.SetUnitData(_playerPencilCase.PencilCaseData.PencilCasedataBase.pencilCaseData, TeamType.MyTeam, _stageData, -1, 1);
            _unitCommand._playerUnitList.Add(_playerPencilCase);
            _playerPencilCase.transform.position = new Vector2(-_stageData.max_Range, 0);
            _playerAbilityState = _playerPencilCase.AbilityState;

            _enemyPencilCase.SetUnitData(_enemyPencilCase.PencilCaseData.PencilCasedataBase.pencilCaseData, TeamType.EnemyTeam, _stageData, -2, 1);
            _unitCommand._enemyUnitList.Add(_enemyPencilCase);
            _enemyPencilCase.transform.position = new Vector2(_stageData.max_Range, 0);
            _enemyAbilityState = _enemyPencilCase.AbilityState;
        }

        /// <summary>
        /// 필통능력 사용
        /// </summary>
        public void RunPencilCaseAbility()
        {
            _playerAbilityState.RunPencilCaseAility();
        }
    }

}