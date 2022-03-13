using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLog : MonoBehaviour
{
    private float time;

    public List<LogData> logDatas;

    public void Add_Log(DataBase dataBase, int grade)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        logDatas.Add(new LogData(dataBase.card_Name, dataBase.card_Cost, mousePos, grade, time));
    }

    public void Update()
    {
        time += Time.deltaTime;    
    }
}

[System.Serializable]
public class LogData
{
    //사용한 카드 이름
    public string log_Name;
    //사용한 카드의 코스트
    public int log_Cost;
    //카드를 사용한 위치
    public Vector2 log_Pos;
    //사용한 카드의 융합 단계
    public int log_Grade;
    //(전 카드를 사용한 후) 카드를 사용한 속도
    public float log_Speed;

    public LogData (string name, int cost, Vector2 pos, int grade, float speed)
    {
        this.log_Name = name;
        this.log_Cost = cost;
        this.log_Pos = pos;
        this.log_Grade = grade;
        this.log_Speed = speed;
    }
}