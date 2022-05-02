using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadStageData : MonoBehaviour
{
    [Header("Enemy AI Data")]
    [SerializeField]
    private int summonGrade;
    [SerializeField]
    private float throwSpeed;
    [SerializeField]
    private List<CardData> cardDataList;
    [SerializeField]
    private List<Vector2> pos;
    [SerializeField]
    private List<float> max_Delay;

    [Header("Enemy PencilCaseData")]
    [SerializeField]
    private PencilCaseData enemyPencilCaseData;

    [SerializeField]
    private AIDataSO aIDataSO;
    [SerializeField]
    private PencilCaseDataSO pencilCaseDataSO;

    public void LoadEnemyStageData()
    {
         pencilCaseDataSO.PencilCaseData = enemyPencilCaseData.maxCard;
         pencilCaseDataSO.PencilCaseData = enemyPencilCaseData.costSpeed;
         pencilCaseDataSO.PencilCaseData = enemyPencilCaseData.throwGaugeSpeed;
         pencilCaseDataSO.PencilCaseData = enemyPencilCaseData.pencilCaseType;
         pencilCaseDataSO.PencilCaseData = enemyPencilCaseData.pencilState;
         pencilCaseDataSO.PencilCaseData = enemyPencilCaseData.pencilCaseData;
         pencilCaseDataSO.PencilCaseData = enemyPencilCaseData._badgeDatas;

        aIDataSO.summonGrade = summonGrade;
        aIDataSO.throwSpeed = throwSpeed;
        aIDataSO.cardDataList = cardDataList;
        aIDataSO.pos = pos;
        aIDataSO.max_Delay = max_Delay;
    }
    public void OnClickStageButton()
    {
        DontDestroyOnLoad(gameObject);
        LoadEnemyStageData();
    }
}
