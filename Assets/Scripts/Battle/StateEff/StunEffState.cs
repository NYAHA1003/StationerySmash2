using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;

namespace Battle.StateEff
{


    public class StunEffState : EffState
    {
        private float stunTime = 0.0f; // ���� �ð�

        public override void Enter()
        {
            stunTime = stunTime + (stunTime * (((float)_myUnit.UnitStat.MaxHp / (_myUnit.UnitStat.Hp + 70)) - 1));
            _stateManager.Set_Wait(stunTime);
            _stateManager.Set_WaitExtraTime(stunTime);

            //����Ʈ ������Ʈ ��������
            _effectObj = _battleManager.CommandEffect.SetEffect(EffectType.Stun, new EffData(new Vector2(Trm.position.x, Trm.position.y + 0.1f), stunTime, Trm));

            base.Enter();
        }

        public override void Update()
        {
            StunTimer();
        }

        public override void Exit()
        {
            DeleteEffectObject();
            base.Exit();
        }

        public override void SetEffValue(params float[] value)
        {
            if (stunTime < value[0])
            {
                stunTime = value[0];
                stunTime = stunTime + (stunTime * (((float)_myUnit.UnitStat.MaxHp / (_myUnit.UnitStat.Hp + 70)) - 1));
            }
        }

        /// <summary>
        /// ����Ÿ�� ���� �ð�
        /// </summary>
        private void StunTimer()
        {
            if (stunTime > 0)
            {
                stunTime -= Time.deltaTime;
                _stateManager.Set_WaitExtraTime(stunTime);
                return;
            }
            _curEvent = eEvent.EXIT;
        }
    }
}