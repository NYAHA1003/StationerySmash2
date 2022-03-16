using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_AI : BattleCommand
{
    public int str_Grade { get; private set; } = 1;

    private int current;
    public List<DataBase> cardDataList;
    public List<Vector2> pos;
    public List<float> max_Delay;
    
    private float cur_Delay;
    private float throw_Speed;
    private float throw_CurDelay;
    private float throw_MaxDelay = 100;
    private bool isAIOn;

    public Battle_AI(BattleManager battleManager, AIDataSO aIDataSO, bool isAIOn) : base(battleManager)
    {
        this.cardDataList = aIDataSO.cardDataList;
        this.pos = aIDataSO.pos;
        this.max_Delay = aIDataSO.max_Delay;
        this.throw_Speed = aIDataSO.throwSpeed;
        this.isAIOn = isAIOn;
    }

    public void Update_AIThrow()
    {
        if (!isAIOn)
            return;
        if (battleManager.unit_EnemyDatasTemp.Count < 3)
            return;
        if (throw_CurDelay < throw_MaxDelay)
        {
            throw_CurDelay += throw_Speed * Time.deltaTime;
            return;
        }
        int selectUnit = Random.Range(2, battleManager.unit_EnemyDatasTemp.Count - 1);
        Vector2 pos = battleManager.unit_EnemyDatasTemp[selectUnit].transform.position;
        pos.x += Random.Range(2.0f, 4.0f);
        pos.y -= Random.Range(2.0f, 4.0f);
        battleManager.unit_EnemyDatasTemp[selectUnit].Throw_Unit(pos);
        throw_CurDelay = 0;
    }
    public void Update_AICard()
    {
        if (!isAIOn)
            return;
        if(cur_Delay < max_Delay[current])
        {
            cur_Delay += Time.deltaTime;
            return;
        }
        battleManager.battle_Unit.Summon_Unit(cardDataList[current], new Vector3(pos[current].x, 0, 0), Utill.TeamType.EnemyTeam);
        current++;
        if(current == max_Delay.Count)
        {
            current = 0;
        }
        cur_Delay = 0;
    }
}
