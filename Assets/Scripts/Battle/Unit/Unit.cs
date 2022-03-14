using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;

public class Unit : MonoBehaviour
{

    public UnitData unitData;
    public UnitState unitState { get; protected set; }

    public List<Eff_State> statEffList = new List<Eff_State>();

    [SerializeField]
    protected Canvas canvas;
    [SerializeField]
    protected Image delayBar;
    [SerializeField]
    protected SpriteRenderer spr;
    
    public TeamType eTeam;

    public float attack_Cur_Delay { get; protected set; }
    public float[] unitablityData;
    protected Camera mainCam;

    public int myDamagedId { get; protected set; } = 0;
    public int damageCount { get; set; } = 0;
    public int myUnitId { get; protected set; } = 0;
    public int hp { get; protected set; }
    public int maxhp { get; protected set; }
    public int weight { get; protected set; }

    //스탯 퍼센트
    public int damagePercent = 100;
    public int moveSpeedPercent = 100;
    public int attackSpeedPercent = 100;
    public int rangePercent = 100;
    public int accuracyPercent = 100;
    public int weightPercent = 100;
    public int knockbackPercent = 100;
    public bool isInvincibility { get; protected set; }
    public bool isDontThrow { get; protected set; }

    //유닛 설정 여부
    protected bool isSettingEnd;

    public BattleManager battleManager { get; protected set; }
    
    protected StageData stageData;
    protected IStateManager stateManager;

    private void Start()
    {
        mainCam = Camera.main;
        canvas.worldCamera = mainCam;
    }

    /// <summary>
    /// 유닛 생성
    /// </summary>
    /// <param name="dataBase">유닛 데이터</param>
    /// <param name="eTeam">팀 변수</param>
    /// <param name="battleManager">배틀매니저</param>
    /// <param name="id"></param>
    public virtual void Set_UnitData(DataBase dataBase, TeamType eTeam, BattleManager battleManager, int id)
    {
        this.unitData = dataBase.unitData;
        this.unitablityData = dataBase.unitData.unitablityData;

        //딜레이시스템
        attack_Cur_Delay = 0;
        Update_DelayBar(attack_Cur_Delay);
        delayBar.rectTransform.anchoredPosition = eTeam.Equals(TeamType.MyTeam) ? new Vector2(-960.15f, -540.15f) : new Vector2(-959.85f, -540.15f);
        Set_IsInvincibility(false);
        Show_Canvas();

        this.isInvincibility = true;
        this.isSettingEnd = false;

        //팀, 이름 설정
        this.eTeam = eTeam;
        transform.name = dataBase.card_Name + this.eTeam;

        switch (this.eTeam)
        {
            case TeamType.Null:
                spr.color = Color.white;
                break;
            case TeamType.MyTeam:
                spr.color = Color.red;
                break;
            case TeamType.EnemyTeam:
                spr.color = Color.blue;
                break;
        }
        
        
        this.spr.sprite = dataBase.card_Sprite;
        this.battleManager = battleManager;
        this.stageData = battleManager.currentStageData;
        this.maxhp = dataBase.unitData.unit_Hp;
        this.hp = dataBase.unitData.unit_Hp;
        this.weight = dataBase.unitData.unit_Weight;
        this.myUnitId = id;

        //스테이트 설정
        Add_state();

        unitState = stateManager.Return_CurrentUnitState();

        this.isInvincibility = false;
        this.isSettingEnd = true;
    }

    /// <summary>
    /// 유닛 상태 업데이트
    /// </summary>
    protected virtual void Update()
    {
        if (!isSettingEnd) return;

        unitState = unitState.Process();

        for (int i = 0; i < statEffList.Count; i++)
        {
            statEffList[i] = statEffList[i].Process();
        }
    }

    /// <summary>
    /// 유닛 사망시 삭제
    /// </summary>
    public virtual void Delete_Unit()
    {
        battleManager.Pool_DeleteUnit(this);
        Delete_state();
        unitState = null;

        switch (eTeam)
        {
            case TeamType.Null:
                break;
            case TeamType.MyTeam:
                battleManager.unit_MyDatasTemp.Remove(this);
                break;
            case TeamType.EnemyTeam:
                battleManager.unit_EnemyDatasTemp.Remove(this);
                break;
        }
    }

    private void Add_state()
    {
        switch (unitData.unitType)
        {
            default:
            case UnitType.None:
            case UnitType.Pencil:
            case UnitType.Eraser:
            case UnitType.Sharp:
                stateManager = Battle_Unit.GetItem<PencilStateManager>(transform, spr.transform, this);
                break;
        }
    }
    private void Delete_state()
    {
        switch (unitData.unitType)
        {
            default:
            case UnitType.None:
            case UnitType.Pencil:
            case UnitType.Eraser:
            case UnitType.Sharp:
                Battle_Unit.AddItem<PencilStateManager>((PencilStateManager)stateManager);
                break;
        }
    }

    /// <summary>
    /// 공격 맞음
    /// </summary>
    /// <param name="atkData">공격 데이터</param>
    public void Run_Damaged(AtkData atkData)
    {
        unitState.Run_Damaged(atkData);
    }

    /// <summary>
    /// 속성 효과 적용
    /// </summary>
    /// <param name="atkType"></param>
    /// <param name="value"></param>
    public void Add_StatusEffect(AtkType atkType, params float[] value)
    {
        unitState.Add_StatusEffect(atkType, value);
    }


    /// <summary>
    /// 당길 유닛을 선택했을 때
    /// </summary>
    /// <returns></returns>
    public Unit Pull_Unit()
    {
        //당기 유닛 선택
        return unitState.Pull_Unit();
    }

    /// <summary>
    /// 유닛을 당기고 있을 때
    /// </summary>
    /// <returns></returns>
    public Unit Pulling_Unit()
    {
        //유닛 당기는 중
        return unitState.Pulling_Unit();
    }

    /// <summary>
    /// 유닛을 던졌을 때
    /// </summary>
    public void Throw_Unit()
    {
        unitState.Throw_Unit();
    }

    /// <summary>
    /// 무적 여부 설정
    /// </summary>
    /// <param name="isboolean">True면 무적, False면 비무적</param>
    public void Set_IsInvincibility(bool isboolean)
    {
        isInvincibility = isboolean;
    }

    /// <summary>
    /// 던지기 가능 설정
    /// </summary>
    /// <param name="isboolean">True면 던지기 불가능, False면 던지기 가능</param>
    public void Set_IsDontThrow(bool isboolean)
    {
        isDontThrow = isboolean;
    }

    /// <summary>
    /// 체력 감소
    /// </summary>
    /// <param name="damage">줄어들 체력</param>
    public void Subtract_HP(int damage)
    {
        hp -= damage;
    }
    public void Set_AttackDelay(float delay)
    {
        attack_Cur_Delay = delay;
    }
    public void Update_DelayBar(float delay)
    {
        delayBar.fillAmount = delay;
    }
    public void Show_Canvas()
    {
        canvas.gameObject.SetActive(true);
    }
    public void NoShow_Canvas()
    {
        canvas.gameObject.SetActive(false);
    }

    #region 스탯 반환
    public int Return_Damage()
    {
        return Mathf.RoundToInt(unitData.damage * (float)damagePercent / 100);
    }
    public float Return_MoveSpeed()
    {
        return unitData.moveSpeed * (float)moveSpeedPercent / 100;
    }
    public float Return_AttackSpeed()
    {
        return unitData.attackSpeed * (float)attackSpeedPercent / 100;
    }
    public float Return_Range()
    {
        return unitData.range * (float)rangePercent / 100;
    }
    public int Return_Weight()
    {
        return Mathf.RoundToInt(unitData.unit_Weight * (float)weightPercent / 100);
    }
    public float Return_Accuracy()
    {
        return unitData.accuracy * (float)accuracyPercent / 100;
    }
    public int Return_Knockback()
    {
        return Mathf.RoundToInt(unitData.knockback * (float)knockbackPercent / 100);
    }

    #endregion
}
