using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
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
        private UnitDataSO _unitDataSO = null;

        //Ai 변수
        private float _delay = 0f;

        public override void SetState(BattleManager battleManager)
        {
            base.SetState(battleManager);
            SetDatas();
        }

        /// <summary>
        /// 구글 스프레드시트 참고하셈, 빨강 노랑 초록 차 소환
        /// </summary>
        public override void RunPencilCaseAbility()
        {
            Vector2 pos = _teamType == TeamType.MyTeam ? _battleManager.CommandPencilCase.PlayerPencilCase.transform.position : _battleManager.CommandPencilCase.EnemyPencilCase.transform.position;

            switch (_trafficType)
            {
                case TrafficType.Red:
                    //빨간차 소환
                    _battleManager.CommandUnit.SummonUnit(_redCarData, pos, 1, _teamType);
                    break;
                case TrafficType.Yellow:
                    //노란차 소한
                    _battleManager.CommandUnit.SummonUnit(_yellowCarData, pos, 1, _teamType);
                    break;
                case TrafficType.Green:
                    _battleManager.CommandUnit.SummonUnit(_greenCarData, pos, 1, _teamType);
                    //초록차 소환
                    break;
            }

            //다음 신호등으로 넘어감
            _trafficType++;
            if (_trafficType > TrafficType.Green)
            {
                _trafficType = TrafficType.Red;
            }
        }


        /// <summary>
        /// 자동차 데이터들 설정
        /// </summary>
        private async void SetDatas()
        {
            AsyncOperationHandle<UnitDataSO> handle = Addressables.LoadAssetAsync<UnitDataSO>("ProjectileUnitSO");
            await handle.Task;
            _unitDataSO = handle.Result;
            SetRedCar();
            SetYellowCar();
            SetGreenCar();
        }



        /// <summary>
        /// 빨간차 데이터 설정
        /// </summary>
        private void SetRedCar()
        {
            _redCarData = _unitDataSO.unitDatas.Find(x => x.unitData.unitType == Utill.UnitType.RedCar);
        }

        /// <summary>
        /// 노란차 데이터 설정
        /// </summary>
        private void SetYellowCar()
        {
            _yellowCarData = _unitDataSO.unitDatas.Find(x => x.unitData.unitType == Utill.UnitType.YellowCar);
        }

        /// <summary>
        /// 초록차 데이터 설정
        /// </summary>
        private void SetGreenCar()
        {
            _greenCarData = _unitDataSO.unitDatas.Find(x => x.unitData.unitType == Utill.UnitType.GreenCar);
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