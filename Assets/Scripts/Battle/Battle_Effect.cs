using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    Attack,
    Stun,
}

public class Battle_Effect : BattleCommand
{
    
    private Transform effect_PoolManager;

    public Battle_Effect(BattleManager battleManager, Transform effect_PoolManager) : base(battleManager)
    {
        this.effect_PoolManager = effect_PoolManager;
    }

    
    /// <summary>
    /// 이펙트 설정
    /// </summary>
    /// <param name="effectType">이펙트 타입</param>
    /// <param name="position">이펙트 위치</param>
    /// <param name="startLifeTime">이펙트의 유지시간</param>
    /// <param name="isSetLifeTime">이펙트 설정을 할 것인지</param>
    public void Set_Effect(EffectType effectType, Vector2 position, float startLifeTime = 0f, bool isSetLifeTime = false)
    {
        Transform effect_Parent = effect_PoolManager.GetChild((int)effectType);
        EffectObject effect_Object = null;
        for (int i = 0; i < effect_Parent.childCount; i++)
        {
            effect_Object = effect_Parent.GetChild(i).GetComponent<EffectObject>();
            if (!effect_Object.gameObject.activeSelf)
            {
                effect_Object.Set_Effect(position, startLifeTime, isSetLifeTime);
                return;
            }
        }

        effect_Object = effect_Parent.GetChild(0).GetComponent<EffectObject>();
        effect_Object.Set_Effect(position);
    }
}
