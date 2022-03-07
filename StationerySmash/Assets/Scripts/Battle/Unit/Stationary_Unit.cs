using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utill;

public class Stationary_Unit : Unit
{

    public UnitData unitData;
    protected List<Stationary_Unit_Eff_State> statEffList = new List<Stationary_Unit_Eff_State>();

    [SerializeField]
    protected Canvas canvas;
    [SerializeField]
    protected Image delayBar;
    public float attack_Cur_Delay { get; private set; }

    //public Ease ease;
    public AnimationCurve curve;

    private Camera mainCam;


    private void Awake()
    {
        mainCam = Camera.main;
        canvas.worldCamera = mainCam;
    }

    protected override void Update()
    {
        base.Update();
        
        for (int i = 0; i < statEffList.Count; i++)
        {
            statEffList[i] = statEffList[i].Process();
        }
    }

    public virtual void Set_Stationary_UnitData(UnitData unitData, TeamType eTeam, BattleManager battleManager, int id)
    {
        this.unitData = unitData;
        unitState = new Pencil_Idle_State(transform, spr.transform, this);

        //딜레이시스템
        attack_Cur_Delay = 0;
        Update_DelayBar(attack_Cur_Delay);
        delayBar.rectTransform.anchoredPosition = eTeam == TeamType.MyTeam ? new Vector2(-960.15f, -540.15f) : new Vector2(-959.85f, -540.15f);

        Set_UnitData(unitData, eTeam, battleManager, id);
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
        unitState.Set_Damaged(atkData);
    }

    public override Unit Pull_Unit()
    {
        if(unitState.curState == UnitState.eState.DAMAGED)
        {
            return null;
        }

        unitState.Set_Wait(2);
        return this;
    }

    public override Unit Pulling_Unit()
    {
        if (unitState.curState == UnitState.eState.DAMAGED)
        {
            return null;
        }

        return this;
    }

    public override void Throw_Unit()
    {
        unitState = new Pencil_Throw_State(transform, spr.transform, this);
    }
    public void Add_StatusEffect(AtkType atkType, float value = 0)
    {
        Stationary_Unit_Eff_State statEffState = statEffList.Find(x => x.statusEffect == atkType);
        if (statEffState != null)
        {
            statEffState.Set_EffSetting(value);
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
}
