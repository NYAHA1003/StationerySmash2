using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill
{
    public enum TimeType
    {
        ActiveTime,
        DisabledTime,
    }
    public enum TeamType
    {
        Null,
        MyTeam,
        EnemyTeam,
    }
    public enum UnitType
    {
        None,
        Pencil,
        Eraser,
        Sharp,
        BallPen,

    }
    public enum PencilCaseType
    {
        Normal,

    }
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
            if(damageId != 0)
            {
                this.damageId = damageId;
                return;
            }

            attacker.damageCount++;
            this.damageId = attacker.myUnitId * 10000 + attacker.damageCount;

        }
    }
    public class Utill : MonoBehaviour
    {
        /// <summary>
        /// �ְ��� ���
        /// </summary>
        /// <param name="v">�ʱ� ����</param>
        /// <param name="sin">���� �Լ��� ���Ȱ�</param>
        /// <param name="multiple">���</param>
        /// <returns></returns>
        static public float Caculated_Height(float v, float sin, float multiple = 1)
        {
            float height = ((v * v) * (Mathf.Sin(sin) * Mathf.Sin(sin)) / Mathf.Abs((Physics2D.gravity.y * 2))) * multiple;
            return height;
        }

        /// <summary>
        /// ���� ���� �Ÿ� ���
        /// </summary>
        /// <param name="v">�ʱ� ����</param>
        /// <param name="sin">���� �Լ��� ���� ��</param>
        /// <returns></returns>
        static public float Caculated_Width(float v, float sin)
        {
            return (v * v) * (Mathf.Sin(sin * 2)) / Mathf.Abs(Physics2D.gravity.y);
        }

        /// <summary>
        /// �ְ��� ���� �ð�
        /// </summary>
        /// <param name="v">�ʱ� ����</param>
        /// <param name="sin">���� �Լ��� ���Ȱ�</param>
        /// <param name="multiple">�ð� ���. 1�̸� �ְ��� ���� �ð�, 2�̸� ���� ���� �ð�</param>
        /// <returns></returns>
        static public float Caculated_Time(float v, float sin, float multiple = 1)
        {
            return Mathf.Abs((v * Mathf.Sin(sin) / Mathf.Abs(Physics2D.gravity.y)) * multiple);
        }

        /// <summary>
        /// t�� ���� ��ġ
        /// </summary>
        /// <param name="v">�ʱ� ����</param>
        /// <param name="sin">���� �Լ��� ���Ȱ�</param>
        /// <param name="time">�ð�</param>
        /// <returns></returns>
        static public float Caculated_TimeToPos(float v, float sin, float time)
        {
            return (v * time * Mathf.Sin(sin)) - (Mathf.Abs(Physics2D.gravity.y / 2) * (time * time));
        }
    }

}