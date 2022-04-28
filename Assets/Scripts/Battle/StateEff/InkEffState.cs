using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
namespace Battle.StateEff
{


    public class InkEffState : EffState
    {
        private float inkTime = 0; //��ũ ���ӽð�
        private float damageSubtractPercent = 0; //���ݷ� �ۼ�Ʈ�� �󸶳� �پ����ΰ�
        private float accuracySubtractPercent = 0;//���߷� �ۼ�Ʈ�� �󸶳� �پ����ΰ�

        public override void Enter()
        {
            SprTrm.GetComponent<SpriteRenderer>().color = Color.green;
            _myUnit.UnitStat.IncreaseAttackPercent(-(int)damageSubtractPercent);
            _myUnit.UnitStat.IncreaseAccuracyPercent(-(int)accuracySubtractPercent);

            base.Enter();
        }
        public override void Update()
        {
            InkTimer();
        }

        public override void Exit()
        {
            _myUnit.UnitStat.IncreaseAttackPercent((int)damageSubtractPercent);
            _myUnit.UnitStat.IncreaseAccuracyPercent((int)accuracySubtractPercent);
            SprTrm.GetComponent<SpriteRenderer>().color = Color.red;

            base.Exit();
        }

        public override void SetEffValue(params float[] value)
        {
            inkTime = value[0];
            damageSubtractPercent = value[1];
            accuracySubtractPercent = value[2];
        }

        /// <summary>
        /// ��ũ ȿ�� ���ӽð�
        /// </summary>
        private void InkTimer()
        {
            if (inkTime > 0)
            {
                inkTime -= Time.deltaTime;
                return;
            }
            _curEvent = eEvent.EXIT;
        }
    }
}