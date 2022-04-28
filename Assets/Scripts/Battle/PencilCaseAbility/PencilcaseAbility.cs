using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Utill;
namespace Battle.PCAbility
{
    public class PencilcaseAbility : AbstractPencilCaseAbility
    {
        int grade = 0;
        float coolTime = 5f;
        CardData pencil;
        public async override void SetState(BattleManager battleManager)
        {
            base.SetState(battleManager);
            AsyncOperationHandle<UnitDataSO> unitList = Addressables.LoadAssetAsync<UnitDataSO>("ProjectileUnitSO");
            await unitList.Task;
            pencil = unitList.Result.unitDatas.Find(x => x.unitData.unitType == Utill.UnitType.Pencil);
        }
        public override void RunPencilCaseAbility()
        {
            _battleManager.CommandUnit.SummonUnit(pencil, _battleManager.CommandPencilCase.PlayerPencilCase.gameObject.transform.position, grade, TeamType.MyTeam);
            grade = 0;
        }
        private IEnumerator gradeUp()
        {
            while (true)
            {
                yield return new WaitForSeconds(coolTime);
                grade++;
            }
        }

        public override bool AIAbilityCondition()
        {
            throw new System.NotImplementedException();
        }
    }
}