using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

namespace Battle
{
    [System.Serializable]
    public class PencilCaseCommand : BattleCommand
    {
        [SerializeField]
        private Unit myPencilCase = null;
        [SerializeField]
        private Unit enemyPencilCase;
        [SerializeField]
        private PencilCaseDataSO pencilCaseDataMy;
        public PencilCaseDataSO PencilCaseDataMy => pencilCaseDataMy;
        [SerializeField]
        private PencilCaseDataSO pencilCaseDataEnemy;
        public PencilCaseDataSO PencilCaseDataEnemy => pencilCaseDataEnemy;
        [SerializeField]
        private PencilCaseAbilityState myPencilCaseAbilityStaty;
        [SerializeField]
        private PencilCaseAbilityState enemyPencilCaseAbilityStaty;
        
        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="pencilCase_My"></param>
        /// <param name="pencilCase_Enemy"></param>
        /// <param name="pencilCaseDataMy"></param>
        /// <param name="pencilCaseDataEnemy"></param>
        public void SetInitialization(BattleManager battleManager)
        {
            this._battleManager = battleManager;

            myPencilCase.SetUnitData(pencilCaseDataMy.PencilCasedataBase.pencilCaseData, TeamType.MyTeam, battleManager, -1, 1);
            battleManager._myUnitDatasTemp.Add(myPencilCase);
            myPencilCase.transform.position = new Vector2(-battleManager.CurrentStageData.max_Range, 0);
            SetPencilCaseAbility(ref myPencilCaseAbilityStaty, pencilCaseDataMy.PencilCasedataBase);

            enemyPencilCase.SetUnitData(pencilCaseDataEnemy.PencilCasedataBase.pencilCaseData, TeamType.EnemyTeam, battleManager, -2, 1);
            battleManager._enemyUnitDatasTemp.Add(enemyPencilCase);
            enemyPencilCase.transform.position = new Vector2(battleManager.CurrentStageData.max_Range, 0);
            SetPencilCaseAbility(ref enemyPencilCaseAbilityStaty, pencilCaseDataEnemy.PencilCasedataBase);
        }

        /// <summary>
        /// 필통능력 사용
        /// </summary>
        public void RunPencilCaseAbility()
        {
            myPencilCaseAbilityStaty.RunPencilCaseAility();
        }

        /// <summary>
        /// 필통 능력 초기화
        /// </summary>
        /// <param name="ability_State"></param>
        /// <param name="pencilCaseData"></param>
        public void SetPencilCaseAbility(ref PencilCaseAbilityState ability_State, PencilCaseData pencilCaseData)
        {
            switch (pencilCaseData.pencilCaseType)
            {
                default:
                case PencilCaseType.Normal:
                    ability_State = new PencilCaseNormalAbilityState(_battleManager);
                    break;
            }
        }
    }

}