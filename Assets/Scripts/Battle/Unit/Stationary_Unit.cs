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
        delayBar.rectTransform.anchoredPosition = eTeam.Equals(TeamType.MyTeam) ? new Vector2(-960.15f, -540.15f) : new Vector2(-959.85f, -540.15f);
        Set_IsInvincibility(false);
        Show_Canvas();
        base.Set_UnitData(dataBase, eTeam, battleManager, id);

        switch (dataBase.unitData.unitType)
        {
            default:
            case UnitType.None:
            case UnitType.Pencil:
            case UnitType.Eraser:
            case UnitType.Sharp:
                unitState = new Pencil_Idle_State(transform, spr.transform, this, stageData);
                unitState.stateChange = new PencilStateChange();
                break;

            case UnitType.BallPen:
                unitState = new BallPen_Idle_State(transform, spr.transform, this, stageData);
                unitState.stateChange = new BallPenStateChange();
                break;
        }
        unitState.stateChange.Set_State(unitState as Stationary_UnitState);


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
        if (atkData.damageId.Equals(-1))
        {
            //무조건 무시해야할 공격
            return;
        }
        if (atkData.damageId.Equals(myDamagedId))
        {
            //똑같은 공격 아이디를 지닌 공격은 무시함
            return;
        }
        unitState.stateChange.Return_Damaged(atkData);
    }

    public override Unit Pull_Unit()
    {
        if(unitState.curState.Equals(eState.DAMAGED))
        {
            return null;
        }

        unitState.stateChange.Return_Wait(2);
        return this;
    }

    public override Unit Pulling_Unit()
    {
        if (unitState.curState.Equals(eState.DAMAGED))
        {
            return null;
        }

        return this;
    }

    public override void Throw_Unit()
    {
        unitState.stateChange.Return_Throw();
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
        Eff_State statEffState = statEffList.Find(x => x.statusEffect.Equals(atkType));
        if (statEffState != null)
        {
            statEffState.Set_EffValue(value);
            return;
        }
        statEffState = statEffList.Find(x => x.statusEffect.Equals(AtkType.Normal));
        if (statEffState != null)
        {
            statEffState.Set_EffType(atkType, value);
            return;
        }

        statEffList.Add(new Stationary_Unit_Eff_State(battleManager, transform, spr.transform, this, atkType, value));

        return;
    }

    #region 스탯 반환


    #endregion
}
