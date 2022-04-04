using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;
using Battle;

public class UnitStateEff
{
    public List<Eff_State> statEffList = new List<Eff_State>();

    /// <summary>
    /// 상태이상 수행
    /// </summary>
    public void ProcessEff()
    {
        for (int i = 0; i < statEffList.Count; i++)
        {
            statEffList[i].Process();
        }
    }

    /// <summary>
    /// 모든 상태이상 삭제
    /// </summary>
    public void Delete_EffStetes()
    {
        //모든 상태이상 삭제
        for (; statEffList.Count > 0;)
        {
            statEffList[0].Delete_StatusEffect();
        }
    }
}
