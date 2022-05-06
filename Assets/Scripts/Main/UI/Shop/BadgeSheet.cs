using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data; 
public class BadgeSheet 
{
    public DailyItemSO _badgeSheetInfo;
    public int _minCount = 1;
    public int _maxCount = 4;
    public int[] _price = new int[3] { 2500, 5000, 10000 };

    public List<DailyItemInfo> currentDailyItemInfos = new List<DailyItemInfo>();
    public Grade _grade;

    public BadgeSheet(DailyItemSO dailyItemSO,Grade grade)
    {
        _badgeSheetInfo = dailyItemSO;
        ResetDailyItemInfos();

        // ���� ��ް� ���� ����� ��ƼĿ�� ����Ʈ�� �߰� 
        for (int i = 0; i < _badgeSheetInfo.dailyItemInfos.Count; i++)
        {
            DailyItemInfo dailyItemIinfo = _badgeSheetInfo.dailyItemInfos[i];
            if (dailyItemIinfo._grade == grade)
            {
                currentDailyItemInfos.Add(dailyItemIinfo);
            }
        }
    }
    public int ReturnRandomCount()
    {
        return Random.Range(_minCount, _maxCount + 1);
    }
    public int ReturnPrice(Grade grade)
    {
        _grade = grade;
        return _price[(int)_grade];
    }
    /// <summary>
    /// ���� ����� ��ƼĿ �������� �ϳ� ���� 
    /// </summary>
    /// <returns></returns>
    public DailyItemInfo ReturnRandomBadge()
    {
        int index = Random.Range(0, currentDailyItemInfos.Count);
        Debug.Log(currentDailyItemInfos.Count);
        currentDailyItemInfos[index]._itemCount = ReturnRandomCount();
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
