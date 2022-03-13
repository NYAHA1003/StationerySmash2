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
    //����� ī�� �̸�
    public string log_Name;
    //����� ī���� �ڽ�Ʈ
    public int log_Cost;
    //ī�带 ����� ��ġ
    public Vector2 log_Pos;
    //����� ī���� ���� �ܰ�
    public int log_Grade;
    //(�� ī�带 ����� ��) ī�带 ����� �ӵ�
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