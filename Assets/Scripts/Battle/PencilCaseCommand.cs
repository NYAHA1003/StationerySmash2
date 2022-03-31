using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

namespace Battle
{
    public class PencilCaseCommand : BattleCommand
    {
        public Unit myPencilCase;
        public Unit enemy_pencilCase;
        public PencilCaseData pencilCaseDataMy;
        public PencilCaseData pencilCaseDataEnemy;
        public PencilCaseAbilityState myPencilCaseAbilityStaty;
        public PencilCaseAbilityState enemyPencilCaseAbilityStaty;
        
        public void SetInitialization(BattleManager battleManager, Unit pencilCase_My, Unit pencilCase_Enemy, PencilCaseData pencilCaseDataMy, PencilCaseData pencilCaseDataEnemy)
        {
            SetBattleManager(battleManager);
            this.myPencilCase = pencilCase_My;
            this.enemy_pencilCase = pencilCase_Enemy;
            this.pencilCaseDataMy = pencilCaseDataMy;
            this.pencilCaseDataEnemy = pencilCaseDataEnemy;
            this.myPencilCaseAbilityStaty = pencilCaseDataMy.pencilState;
            this.enemyPencilCaseAbilityStaty = pencilCaseDataEnemy.pencilState;
            this.battleManager = battleManager;

            pencilCase_My.SetUnitData(pencilCaseDataMy.pencilCaseData, TeamType.MyTeam, battleManager, -1, 1);
            battleManager._myUnitDatasTemp.Add(pencilCase_My);
            pencilCase_My.transform.position = new Vector2(-battleManager.CurrentStageData.max_Range, 0);
            SetPencilCaseAbility(ref myPencilCaseAbilityStaty, pencilCaseDataMy);

            pencilCase_Enemy.SetUnitData(pencilCaseDataEnemy.pencilCaseData, TeamType.EnemyTeam, battleManager, -2, 1);
            battleManager._enemyUnitDatasTemp.Add(pencilCase_Enemy);
            pencilCase_Enemy.transform.position = new Vector2(battleManager.CurrentStageData.max_Range, 0);
            SetPencilCaseAbility(ref enemyPencilCaseAbilityStaty, pencilCaseDataEnemy);
        }

        public void RunPencilCaseAbility()
        {
            myPencilCaseAbilityStaty.RunPencilCaseAility();
        }

        public void SetPencilCaseAbility(ref PencilCaseAbilityState ability_State, PencilCaseData pencilCaseData)
        {
            switch (pencilCaseData.pencilCaseType)
            {
                default:
                case PencilCaseType.Normal:
                    ability_State = new PencilCaseNormalAbilityState(battleManager);
                    break;
            }
        }
    }

}