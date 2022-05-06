using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data; 

public class NewSticker 
{
    public DailyItemSO _stickerSheetInfo;
    public int[] _price = new int[3] { 30000, 45000, 60000 };

    public List<DailyItemInfo> currentDailyItemInfos = new List<DailyItemInfo>();

    public Grade _grade;

    public NewSticker(DailyItemSO dailyItemSO, Grade grade)
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
    /// <summary>
    /// ���� ���� ��ȯ 
    /// </summary>
    /// <returns></returns>
    
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
