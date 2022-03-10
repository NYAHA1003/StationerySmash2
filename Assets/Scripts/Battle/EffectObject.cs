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
    /// ����Ʈ ���� �� ����
    /// </summary>
    /// <param name="pos">����Ʈ ��ġ</param>
    /// <param name="startLifeTime">����Ʈ �����ð�</param>
    /// <param name="isSetLifeTime">����Ʈ�� �����ð��� �ٲ� ������</param>
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
