using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utill
{
    public enum AtkType
    {
        Normal,
        Stun,
        Ink,
        SlowDown,
    }
    public class AtkData
    {

        public Unit attacker;

        public int damage;

        //������ ���̵�
        public int damageId;


        //�˹� ������
        public float baseKnockback;
        public float extraKnockback;
        public float direction; // ��׸���. �������� ���� ����

        //���� �Ӽ�
        public AtkType atkType;
        public float[] value;

        public AtkData(Unit attacker, int damage, float baseKnockback, float extraKnockback, float direction, bool isMyTeam, int damageId = 0, AtkType atkType = AtkType.Normal, params float[] value)
        {
            this.attacker = attacker;
            this.damage = damage;
            Set_DamageId(damageId);
            this.baseKnockback = baseKnockback;
            this.extraKnockback = extraKnockback;
            this.direction = (isMyTeam ? direction : 180 - direction) * Mathf.Deg2Rad;
            this.atkType = atkType;
            this.value = value;
        }
        public float Caculated_Knockback(int weight, int hp, int maxhp, bool isMyTeam)
        {
            return ((baseKnockback + extraKnockback) / (weight * (((float)hp / maxhp) + 0.1f))) * (isMyTeam ? 1 : -1);
        }

        public void Reset_Kncockback(float baseKnockback, float extraKnockback, float direction, bool isMyTeam)
        {
            this.baseKnockback = baseKnockback;
            this.extraKnockback = extraKnockback;
            this.direction = (isMyTeam ? direction : 180 - direction) * Mathf.Deg2Rad;
        }

        public void Reset_Type(AtkType atkType)
        {
            this.atkType = atkType;
        }

        public void Reset_Damage(int damage)
        {
            this.damage = damage;
        }

        public void Reset_Value(params float[] value)
        {
            this.value = value;
        }
        public void Set_DamageId(int damageId = 0)
        {
            if (damageId != 0)
            {
                this.damageId = damageId;
                return;
            }

            attacker.damageCount++;
            this.damageId = attacker._myUnitId * 10000 + attacker.damageCount;

        }
    }
}

