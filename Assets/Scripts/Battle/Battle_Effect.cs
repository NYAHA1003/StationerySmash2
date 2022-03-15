using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;


public class Battle_Effect : BattleCommand
{
    
    private Transform effect_PoolManager;

    public Battle_Effect(BattleManager battleManager, Transform effect_PoolManager) : base(battleManager)
    {
        this.effect_PoolManager = effect_PoolManager;
    }

    
    /// <summary>
    /// ����Ʈ ����
    /// </summary>
    /// <param name="effectType">����Ʈ Ÿ��</param>
    /// <param name="position">����Ʈ ��ġ</param>
    /// <param name="startLifeTime">����Ʈ�� �����ð�</param>
    /// <param name="isSetLifeTime">����Ʈ ������ �� ������</param>
    public IEffect Set_Effect(EffectType effectType, EffData effData)
    {
        Transform effect_Parent = effect_PoolManager.GetChild((int)effectType);
        EffectObject effect_Object = null;

        //������ �� ���� ����Ʈ ã��
        for (int i = 0; i < effect_Parent.childCount; i++)
        {
            effect_Object = effect_Parent.GetChild(i).GetComponent<EffectObject>();
            if (!effect_Object.gameObject.activeSelf)
            {
                effect_Object.Set_Effect(effData);
                return effect_Object.effectState;
            }
        }

        //������ ���� ����
        effect_Object = battleManager.Create_Object(battleManager.effect_ObjList[(int)effectType].gameObject, effData.pos, Quaternion.identity).GetComponent<EffectObject>();
        effect_Object.transform.SetParent(effect_Parent);
        effect_Object.Set_Effect(effData);
        return effect_Object.effectState;
    }
}
