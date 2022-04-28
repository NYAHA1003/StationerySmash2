using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
namespace Battle.StateEff
{


    public class RtacEffState : EffState
    {
        private float _originRtacTime = 0.0f; //�ʱ� ���� �ð�
        private float _rtacTime = 0.0f; // ���� �ð�
        private float damagedSubtractPercent = 0; //�������޴·� �ۼ�Ʈ�� �󸶳� �پ����ΰ�

        public override void Enter()
        {
            SprTrm.GetComponent<SpriteRenderer>().color = Color.green;
            _myUnit.UnitStat.IncreaseDamagedPercent(-(int)damagedSubtractPercent);

            //����Ʈ ������Ʈ ��������
            //_effectObj = _battleManager.CommandEffect.SetEffect(EffectType.Stun, new EffData(new Vector2(Trm.position.x, Trm.position.y + 0.1f), stunTime, Trm));

            base.Enter();
        }

        public override void Update()
        {
            RtacTimer();
        }

        public override void Exit()
        {
            _myUnit.UnitStat.IncreaseDamagedPercent((int)damagedSubtractPercent);
            DeleteEffectObject();
            base.Exit();
        }

        public override void SetEffValue(params float[] value)
        {
            if (value[1] > damagedSubtractPercent)
            {
                _rtacTime = value[0];
                damagedSubtractPercent = value[1];
                _originRtacTime = value[0];
            }
            else
            {
                _rtacTime = _originRtacTime;
            }

        }


        /// <summary>
        /// ���� ���� �ð�
        /// </summary>
        private void RtacTimer()
        {
            if (_rtacTime > 0)
            {
                _rtacTime -= Time.deltaTime;
                return;
            }
            _curEvent = eEvent.EXIT;
        }
    }
}