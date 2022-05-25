using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

namespace Battle.PCAbility
{


    public class TrafficAbility : AbstractPencilCaseAbility
    {
        private enum TrafficType
        {
            Red,
            Yellow,
            Green,
        }

        private TrafficType _trafficType = TrafficType.Red;
        private CardData _redCarData = null;
        private CardData _yellowCarData = null;
        private CardData _greenCarData = null;

        //Ai ����
        private float _delay = 0f;

        public override void SetState(BattleManager battleManager)
        {
            base.SetState(battleManager);
            SetDatas();
        }

        /// <summary>
        /// ���� ���������Ʈ �����ϼ�, ���� ��� �ʷ� �� ��ȯ
        /// </summary>
        public override void RunPencilCaseAbility()
        {
            Vector2 pos = _teamType == TeamType.MyTeam ? _battleManager.PencilCaseComponent.PlayerPencilCase.transform.position : _battleManager.PencilCaseComponent.EnemyPencilCase.transform.position;

            switch (_trafficType)
            {
                case TrafficType.Red:
                    //������ ��ȯ
                    _battleManager.UnitComponent.SummonUnit(_redCarData, pos, 1, _teamType);
                    break;
                case TrafficType.Yellow:
                    //����� ����
                    _battleManager.UnitComponent.SummonUnit(_yellowCarData, pos, 1, _teamType);
                    break;
                case TrafficType.Green:
                    _battleManager.UnitComponent.SummonUnit(_greenCarData, pos, 1, _teamType);
                    //�ʷ��� ��ȯ
                    break;
            }

            //���� ��ȣ������ �Ѿ
            _trafficType++;
            if (_trafficType > TrafficType.Green)
            {
                _trafficType = TrafficType.Red;
            }
        }


        /// <summary>
        /// �ڵ��� �����͵� ����
        /// </summary>
        private async void SetDatas()
        {
            //AsyncOperationHandle<UnitDataSO> handle = Addressables.LoadAssetAsync<UnitDataSO>("ProjectileUnitSO");
            //await handle.Task;
            //_unitDataSO = handle.Result;
            SetRedCar();
            SetYellowCar();
            SetGreenCar();
        }



        /// <summary>
        /// ������ ������ ����
        /// </summary>
        private void SetRedCar()
        {
            //_redCarData = UnitDataManagerSO.FindUnitData(UnitType.RedCar);
        }

        /// <summary>
        /// ����� ������ ����
        /// </summary>
        private void SetYellowCar()
        {
            //_yellowCarData = UnitDataManagerSO.FindUnitData(UnitType.YellowCar);
        }

        /// <summary>
        /// �ʷ��� ������ ����
        /// </summary>
        private void SetGreenCar()
        {
            //_greenCarData = UnitDataManagerSO.FindUnitData(UnitType.GreenCar);
        }

        public override bool AIAbilityCondition()
        {
            if (_delay < 3f)
            {
                _delay += Time.deltaTime;
                return false;
            }
            _delay = 0f;
            return true;
        }
    }
}