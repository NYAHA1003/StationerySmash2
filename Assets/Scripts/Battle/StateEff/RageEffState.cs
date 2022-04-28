using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
namespace Battle.StateEff
{


    public class RageEffState : EffState
    {
        private float _originrageTime = 0.0f; //초기 분노 시간
        private float _rageTime = 0.0f; // 스턴 시간
        private float attackSubtractPercent = 0; //공격력 퍼센트가 얼마나 줄어들것인가
        private float moveSpeedSubtractPercent = 0;//이동속도 퍼센트가 얼마나 줄어들것인가

        public override void Enter()
        {
            SprTrm.GetComponent<SpriteRenderer>().color = Color.green;
            _myUnit.UnitStat.IncreaseAttackPercent((int)attackSubtractPercent);
            _myUnit.UnitStat.IncreaseMoveSpeedPercent((int)moveSpeedSubtractPercent);

            //이펙트 오브젝트 가져오기
            //_effectObj = _battleManager.CommandEffect.SetEffect(EffectType.Stun, new EffData(new Vector2(Trm.position.x, Trm.position.y + 0.1f), stunTime, Trm));

            base.Enter();
        }

        public override void Update()
        {
            RageTimer();
        }

        public override void Exit()
        {
            _myUnit.UnitStat.IncreaseAttackPercent(-(int)attackSubtractPercent);
            _myUnit.UnitStat.IncreaseMoveSpeedPercent(-(int)moveSpeedSubtractPercent);
            DeleteEffectObject();
            base.Exit();
        }

        public override void SetEffValue(params float[] value)
        {
            if (value[1] + value[2] > attackSubtractPercent + moveSpeedSubtractPercent)
            {
                _rageTime = value[0];
                attackSubtractPercent = value[1];
                moveSpeedSubtractPercent = value[2];
                _originrageTime = value[0];
            }
            else
            {
                _rageTime = _originrageTime;
            }

        }


        /// <summary>
        /// 분노 유지 시간
        /// </summary>
        private void RageTimer()
        {
            if (_rageTime > 0)
            {
                _rageTime -= Time.deltaTime;
                return;
            }
            _curEvent = eEvent.EXIT;
        }

    }
}