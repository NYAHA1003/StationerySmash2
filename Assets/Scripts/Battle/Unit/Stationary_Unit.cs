using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utill;

public class Stationary_Unit : Unit
{
    protected List<Eff_State> statEffList = new List<Eff_State>();


    [SerializeField]
    protected Canvas canvas;
    [SerializeField]
    protected Image delayBar;
    public float attack_Cur_Delay { get; private set; }
    public float[] unitablityData;
    private Camera mainCam;


    private void Start()
    {
        mainCam = Camera.main;
        canvas.worldCamera = mainCam;
    }


    protected override void Update()
    {
        if (!isSettingEnd) return;

        unitState = unitState.Process();

        for (int i = 0; i < statEffList.Count; i++)
        {
            statEffList[i] = statEffList[i].Process();
        }
    }

    public override void Set_UnitData(DataBase dataBase, TeamType eTeam, BattleManager battleManager, int id)
    {
        this.unitData = dataBase.unitData;
        this.unitablityData = dataBase.unitData.unitablityData;

        //딜레이시스템
        attack_Cur_Delay = 0;
        Update_DelayBar(attack_Cur_Delay);
        delayBar.rectTransform.anchoredPosition = eTeam == TeamType.MyTeam ? new Vector2(-960.15f, -540.15f) : new Vector2(-959.85f, -540.15f);
        Set_IsInvincibility(false);
        Show_Canvas();

        switch (dataBase.unitData.unitType)
        {
            default:
            case UnitType.None:
            case UnitType.Pencil:
            case UnitType.Eraser:
            case UnitType.Sharp:
                unitState = new Pencil_Idle_State(transform, spr.transform, this, stageData);
                break;

            case UnitType.BallPen:
                unitState = new BallPen_Idle_State(transform, spr.transform, this, stageData);
                break;
        }

        base.Set_UnitData(dataBase, eTeam, battleManager, id);

    }


    public void Set_AttackDelay(float delay)
    {
        attack_Cur_Delay = delay;
    }
    public void Update_DelayBar(float delay)
    {
        delayBar.fillAmount = delay;
    }
    public override void Run_Damaged(AtkData atkData)
    {
        if (atkData.damageId == -1)
        {
            //무조건 무시해야할 공격
            return;
        }
        if (atkData.damageId == myDamagedId)
        {
            //똑같은 공격 아이디를 지닌 공격은 무시함
            return;
        }
        unitState.nextState = unitState.stateChange.Return_Damaged(unitState as Stationary_UnitState, atkData);
    }

    public override Unit Pull_Unit()
    {
        if(unitState.curState == eState.DAMAGED)
        {
            return null;
        }

        unitState.nextState = unitState.stateChange.Return_Wait(unitState as Stationary_UnitState, 2);
        return this;
    }

    public override Unit Pulling_Unit()
    {
        if (unitState.curState == eState.DAMAGED)
        {
            return null;
        }

        return this;
    }

    public override void Throw_Unit()
    {
        unitState.nextState = unitState.stateChange.Return_Throw(unitState as Stationary_UnitState);
    }

    public void Show_Canvas()
    {
        canvas.gameObject.SetActive(true);
    }
    public void NoShow_Canvas()
    {
        canvas.gameObject.SetActive(false);
    }

    public override void Add_StatusEffect(AtkType atkType, params float[] value)
    {
        Eff_State statEffState = statEffList.Find(x => x.statusEffect == atkType);
        if (statEffState != null)
        {
            statEffState.Set_EffValue(value);
            return;
        }
        statEffState = statEffList.Find(x => x.statusEffect == AtkType.Normal);
        if (statEffState != null)
        {
            statEffState.Set_EffType(atkType, value);
            return;
        }

        statEffList.Add(new Stationary_Unit_Eff_State(transform, spr.transform, this, atkType, value));

        return;
    }

    #region 스탯 반환


    #endregion
}
