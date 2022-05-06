using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data; 

public class NewBadge 
{
    public DailyItemSO _newBadgeInfo;
    public int[] _price = new int[3] { 50000, 75000, 100000 };

    public List<DailyItemInfo> currentDailyItemInfos = new List<DailyItemInfo>();
    public Grade _grade;

    public NewBadge(DailyItemSO dailyItemSO, Grade grade)
    {
        _newBadgeInfo = dailyItemSO;
        ResetDailyItemInfos();

        // ���� ��ް� ���� ����� ��ƼĿ�� ����Ʈ�� �߰� 
        for (int i = 0; i < _newBadgeInfo.dailyItemInfos.Count; i++)
        {
            DailyItemInfo dailyItemIinfo = _newBadgeInfo.dailyItemInfos[i];
            if (dailyItemIinfo._grade == grade)
            {
                currentDailyItemInfos.Add(dailyItemIinfo);
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
