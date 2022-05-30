using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;

public abstract class AbstractDailyItem : MonoBehaviour
{
    public DailyItemSO _newItemInfo;
    public abstract int[] _price { get; set; }

    public List<DailyItemInfo> currentDailyItemInfos = new List<DailyItemInfo>();
    public Grade _grade;
    
    public AbstractDailyItem(DailyItemSO dailyItemSO, Grade grade = Grade.Null) 
    {
        _newItemInfo = dailyItemSO;
        ResetDailyItemInfos();

        if (grade == Grade.Null) return; 

        // ���� ��ް� ���� ����� �����۵� ����Ʈ�� �߰� 
        for(int i = 0; i< _newItemInfo.dailyItemInfos.Count; i++)
        {
            DailyItemInfo dailyItemInfo = _newItemInfo.dailyItemInfos[i]; 
            if(dailyItemInfo._grade == grade)
            {
                currentDailyItemInfos.Add(dailyItemInfo); 
            }
        }

    }

    /// <summary>
    /// ���� ����� ��ƼĿ �������� �ϳ� ���� 
    /// </summary>
    /// <returns></returns>
    public DailyItemInfo ReturnRandomBadge()
    {
        int index = Random.Range(0, currentDailyItemInfos.Count);
        return currentDailyItemInfos[index];
    }

    /// <summary>
    /// ���ϻ��� ��ƼĿ���� ������ ����Ʈ �ʱ�ȭ
    /// </summary>
    public void ResetDailyItemInfos()
    {
         currentDailyItemInfos.Clear();
    }
}
