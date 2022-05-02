using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
namespace Battle.StateEff
{


    public class RageEffState : EffState
    {
        private float _originrageTime = 0.0f; //�ʱ� �г� �ð�
        private float _rageTime = 0.0f; // ���� �ð�
        private float attackSubtractPercent = 0; //���ݷ� �ۼ�Ʈ�� �󸶳� �پ����ΰ�
        private float moveSpeedSubtractPercent = 0;//�̵��ӵ� �ۼ�Ʈ�� �󸶳� �پ����ΰ�

        public override void Enter()
        {
            SprTrm.GetComponent<SpriteRenderer>().color = Color.green;
            _myUnit.UnitStat.IncreaseAttackPercent((int)attackSubtractPercent);
            _myUnit.UnitStat.IncreaseMoveSpeedPercent((int)moveSpeedSubtractPercent);

            //����Ʈ ������Ʈ ��������
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
        /// �г� ���� �ð�
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