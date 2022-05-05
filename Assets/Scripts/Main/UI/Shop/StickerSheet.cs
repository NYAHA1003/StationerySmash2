using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data; 

public class StickerSheet 
{
    public DailyItemSO _stickerSheetInfo;
    public int _minCount = 1;
    public int _maxCount = 10;
    public int[] _price = new int[3] { 1000, 2000, 4000 };

    public List<DailyItemInfo> currentDailyItemInfos = new List<DailyItemInfo>(); 

    public Grade _grade;

    public StickerSheet(DailyItemSO dailyItemSO, Grade grade)
    {
        _stickerSheetInfo = dailyItemSO;
        ResetDailyItemInfos(); 
        
        // ���� ��ް� ���� ����� ��ƼĿ�� ����Ʈ�� �߰� 
        for (int i = 0; i < _stickerSheetInfo.dailyItemInfos.Count; i++)
        {
            DailyItemInfo dailyItemIinfo = _stickerSheetInfo.dailyItemInfos[i]; 
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
    public DailyItemInfo ReturnRandomSticker()
    {
        int index =  Random.Range(0, currentDailyItemInfos.Count);
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
    /// <summary>
    /// ���� ���� ��ȯ 
    /// </summary>
    /// <returns></returns>
    public int ReturnRandomCount()
    {
        return Random.Range(_minCount, _maxCount + 1);
    }
    /// <summary>
    /// ���� ���� 
    /// </summary>
    /// <param name="grade"></param>
    /// <returns></returns>
    public int ReturnPrice(Grade grade)
    {
        _grade = grade;
        return _price[(int)_grade];
    }

}
