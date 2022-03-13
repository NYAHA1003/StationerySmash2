using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class EffectObject : MonoBehaviour
{
    public EffectType effectType;
    private IEffect effectState;
    private EffData effData;
    private bool isSettingEnd;

    /// <summary>
    /// 이펙트 리셋 및 설정
    /// </summary>
    /// <param name="pos">이펙트 위치</param>
    /// <param name="startLifeTime">이펙트 유지시간</param>
    /// <param name="isSetLifeTime">이펙트를 유지시간을 바꿀 것인지</param>
    public void Set_Effect(EffData effData)
    {
        switch (effectType)
        {
            case EffectType.Attack:
                effectState ??= new Effect_Attack();
                break;
            case EffectType.Stun:
                effectState ??= new Effect_Stun();
                break;
        }

        this.effData = effData;
        effectState.Set_Effect(this, effData);

        isSettingEnd = true;
    }

    private void Update()
    {
        if (!isSettingEnd)
            return;

        effectState.Update_Effect(this, effData);
    }

    private void Delete_Effect()
    {
        gameObject.SetActive(false);
        isSettingEnd = false;
    }
}
