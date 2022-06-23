using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utill.Data
{
    public enum AttackType
    {
        Normal = 0,
        Range,
    }
    public enum EffAttackType
    {
        Normal = 0,
        Stun,
        Ink,
        SlowDown,
        Rage,
        Rtac,
        Blind,
        Sick,
        Exch,
        Scratch,

        Inherence = 1000,
        PCKill,

    }
    public class AtkData
    {

        public Unit attacker;

        public static int damageCount;
        public int damage;

        //데미지 아이디
        public int damageId;


        //넉백 데이터
        public float baseKnockback;
        public float extraKnockback;
        public float direction; // 디그리값. 라디안으로 주지 마셈

        //공격 속성
        public EffAttackType atkType;
        public float[] value;

        //이펙트 타입
        public EffectType _effectType;

        public AtkData(Unit attacker, int damage, float baseKnockback, float extraKnockback, float direction, bool isMyTeam, int damageId = 0, EffAttackType atkType = EffAttackType.Normal, EffectType effectType = EffectType.Attack, params float[] value)
        {
            this.attacker = attacker;
            this.damage = damage;
            Set_DamageId(damageId);
            this.baseKnockback = baseKnockback;
            this.extraKnockback = extraKnockback;
            this.direction = (isMyTeam ? direction : 180 - direction) * Mathf.Deg2Rad;
            this.atkType = atkType;
            this._effectType = effectType;
            this.value = value;
        }
        public float Caculated_Knockback(int weight, int hp, int maxhp, bool isMyTeam)
        {
            float percent = ((float)hp / maxhp) + 0.01f;

            return (baseKnockback + extraKnockback) / (weight * 0.8f * percent) * (isMyTeam ? 1 : -1);
        }

        public void Reset_Kncockback(float baseKnockback, float extraKnockback, float direction, bool isMyTeam)
        {
            this.baseKnockback = baseKnockback;
            this.extraKnockback = extraKnockback;
            this.direction = (isMyTeam ? direction : 180 - direction) * Mathf.Deg2Rad;
        }

        public void Reset_Type(EffAttackType atkType)
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
            else
            {
                if(attacker != null)
                {
                    attacker.DamageCount++;
                    this.damageId = attacker.MyUnitId * 10000 + attacker.DamageCount;
                }
                else
				{
                    this.damageId = 30000 + damageCount++;
				}
            }

        }

        /// <summary>
        /// 고유 효과 적용
        /// </summary>
        /// <param name="unit"></param>
        public void RunIncrease(Unit unit)
        {
            switch (atkType)
            {
                default:
                case EffAttackType.Inherence:
                    break;
                case EffAttackType.PCKill:
                    if (unit.UnitData._unitType == UnitType.PencilCase)
                    {
                        Reset_Damage(damage * 2);
                    }
                    break;
            }
        }
    }


}

