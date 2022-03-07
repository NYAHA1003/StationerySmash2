using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill
{
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
    }
    public enum PencilCaseType
    {
        Normal,

    }
    public enum AtkType
    {
        Normal,
        Stun,
    }
    public class AtkData
    {

        public Unit attacker;

        public int damage;

        //데미지 아이디
        public int damageId;


        //넉백 데이터
        public float baseKnockback;
        public float extraKnockback;
        public float direction; // 디그리값. 라디안으로 주지 마셈

        //공격 속성
        public AtkType atkType;
        public float value;

        public AtkData(Unit attacker, int damage, float baseKnockback, float extraKnockback, float direction, bool isMyTeam, AtkType atkType, float value = 0, int damageId = 0)
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

        public void Reset_Value(float value)
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
        /// 최고점 계산
        /// </summary>
        /// <param name="v">초기 벡터</param>
        /// <param name="sin">사인 함수의 라디안값</param>
        /// <param name="multiple">배수</param>
        /// <returns></returns>
        static public float Caculated_Height(float v, float sin, float multiple = 1)
        {
            float height = ((v * v) * (Mathf.Sin(sin) * Mathf.Sin(sin)) / Mathf.Abs((Physics2D.gravity.y * 2))) * multiple;
            return height;
        }

        /// <summary>
        /// 수평 도달 거리 계산
        /// </summary>
        /// <param name="v">초기 벡터</param>
        /// <param name="sin">사인 함수의 라디안 값</param>
        /// <returns></returns>
        static public float Caculated_Width(float v, float sin)
        {
            return (v * v) * (Mathf.Sin(sin * 2)) / Mathf.Abs(Physics2D.gravity.y);
        }

        /// <summary>
        /// 최고점 도달 시간
        /// </summary>
        /// <param name="v">초기 벡터</param>
        /// <param name="sin">사인 함수의 라디안값</param>
        /// <param name="multiple">시간 배수. 1이면 최고점 도달 시간, 2이면 수평 도달 시간</param>
        /// <returns></returns>
        static public float Caculated_Time(float v, float sin, float multiple = 1)
        {
            return Mathf.Abs((v * Mathf.Sin(sin) / Mathf.Abs(Physics2D.gravity.y)) * multiple);
        }

        /// <summary>
        /// t초 후의 위치
        /// </summary>
        /// <param name="v">초기 벡터</param>
        /// <param name="sin">사인 함수의 라디안값</param>
        /// <param name="time">시간</param>
        /// <returns></returns>
        static public float Caculated_TimeToPos(float v, float sin, float time)
        {
            return (v * time * Mathf.Sin(sin)) - (Mathf.Abs(Physics2D.gravity.y / 2) * (time * time));
        }
    }

}