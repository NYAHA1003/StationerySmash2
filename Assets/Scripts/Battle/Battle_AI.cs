using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_AI : BattleCommand
{
    //Enemy용
    private int enemy_current;
    public List<DataBase> enemy_cardDataList;
    public List<Vector2> enemy_pos;
    public List<float> enemy_max_Delay;

    private int enemy_summonGrade = 1;
    private float enemy_cur_Delay;
    private float enemy_throw_Speed;
    private float enemy_throw_CurDelay;
    private float enemy_throw_MaxDelay = 100;
    private bool isEnemyAIOn;

    //Player용
    private int player_current;
    public List<DataBase> player_cardDataList;
    public List<Vector2> player_pos;
    public List<float> player_max_Delay;

    private int player_summonGrade = 1;
    private float player_cur_Delay;
    private float player_throw_Speed;
    private float player_throw_CurDelay;
    private float player_throw_MaxDelay = 100;
    private bool isPlayerAIOn;

    public Battle_AI(BattleManager battleManager, AIDataSO aIenemyDataSO, AIDataSO aIplayerDataSO, bool isEnemyAIOn, bool isPlayerAIOn) : base(battleManager)
    {
        //적 AI
        this.enemy_summonGrade = aIenemyDataSO.summonGrade;
        this.enemy_cardDataList = aIenemyDataSO.cardDataList;
        this.enemy_pos = aIenemyDataSO.pos;
        this.enemy_max_Delay = aIenemyDataSO.max_Delay;
        this.enemy_throw_Speed = aIenemyDataSO.throwSpeed;
        this.isEnemyAIOn = isEnemyAIOn;

        //테스트용 Player AI
        this.player_summonGrade = aIplayerDataSO.summonGrade;
        this.player_cardDataList = aIplayerDataSO.cardDataList;
        this.player_pos = aIplayerDataSO.pos;
        this.player_max_Delay = aIplayerDataSO.max_Delay;
        this.player_throw_Speed = aIplayerDataSO.throwSpeed;
        this.isPlayerAIOn = isPlayerAIOn;
    }

    public void Update_EnemyAIThrow()
    {
        if (!isEnemyAIOn)
            return;
        if (battleManager.unit_EnemyDatasTemp.Count < 3)
            return;
        if (enemy_throw_CurDelay < enemy_throw_MaxDelay)
        {
            enemy_throw_CurDelay += enemy_throw_Speed * Time.deltaTime;
            return;
        }
        int selectUnit = Random.Range(2, battleManager.unit_EnemyDatasTemp.Count - 1);
        Vector2 pos = battleManager.unit_EnemyDatasTemp[selectUnit].transform.position;
        pos.x += Random.Range(2.0f, 4.0f);
        pos.y -= Random.Range(2.0f, 4.0f);
        battleManager.unit_EnemyDatasTemp[selectUnit].Throw_Unit(pos);
        enemy_throw_CurDelay = 0;
    }
    public void Update_EnemyAICard()
    {
        if (!isEnemyAIOn)
            return;
        if(enemy_cur_Delay < enemy_max_Delay[enemy_current])
        {
            enemy_cur_Delay += Time.deltaTime;
            return;
        }
        battleManager.battle_Unit.Summon_Unit(enemy_cardDataList[enemy_current], new Vector3(enemy_pos[enemy_current].x, 0, 0), enemy_summonGrade, Utill.TeamType.EnemyTeam);
        enemy_current++;
        if(enemy_current == enemy_max_Delay.Count)
        {
            enemy_current = 0;
        }
        enemy_cur_Delay = 0;
    }


    public void Update_PlayerAIThrow()
    {
        if (!isPlayerAIOn)
            return;
        if (battleManager.unit_MyDatasTemp.Count < 3)
            return;
        if (player_throw_CurDelay < player_throw_MaxDelay)
        {
            player_throw_CurDelay += player_throw_Speed * Time.deltaTime;
            return;
        }
        int selectUnit = Random.Range(2, battleManager.unit_MyDatasTemp.Count - 1);
        Vector2 pos = battleManager.unit_MyDatasTemp[selectUnit].transform.position;
        pos.x -= Random.Range(2.0f, 4.0f);
        pos.y -= Random.Range(2.0f, 4.0f);
        battleManager.unit_MyDatasTemp[selectUnit].Throw_Unit(pos);
        player_throw_CurDelay = 0;
    }
    public void Update_PlayerAICard()
    {
        if (!isPlayerAIOn)
            return;
        if (player_cur_Delay < player_max_Delay[player_current])
        {
            player_cur_Delay += Time.deltaTime;
            return;
        }
        battleManager.battle_Unit.Summon_Unit(player_cardDataList[player_current], new Vector3(player_pos[player_current].x, 0, 0), player_summonGrade, Utill.TeamType.MyTeam);
        player_current++;
        if (player_current == player_max_Delay.Count)
        {
            player_current = 0;
        }
        player_cur_Delay = 0;
    }
}
