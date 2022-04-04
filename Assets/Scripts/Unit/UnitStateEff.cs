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
    /// �����̻� ����
    /// </summary>
    public void ProcessEff()
    {
        for (int i = 0; i < statEffList.Count; i++)
        {
            statEffList[i].Process();
        }
    }

    /// <summary>
    /// ��� �����̻� ����
    /// </summary>
    public void Delete_EffStetes()
    {
        //��� �����̻� ����
        for (; statEffList.Count > 0;)
        {
            statEffList[0].Delete_StatusEffect();
        }
    }
}
