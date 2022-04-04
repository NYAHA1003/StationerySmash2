using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;
using Battle;
public class Unit : MonoBehaviour
{
    //인스펙터 참조 변수
    //참조 변수
    //변수
    public UnitData unitData;
    public CollideData collideData;
    public UnitState unitState { get; protected set; }

    public List<Eff_State> statEffList = new List<Eff_State>();
    public TeamType eTeam;

    public float attack_Cur_Delay { get; protected set; }
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

    public BattleManager _battleManager { get; protected set; }
    
    protected StageData _stageData;
    protected IStateManager stateManager;

    [SerializeField]
    private UnitSprite _unitSprite = null;
    public UnitSprite UnitSprite => _unitSprite;

    protected virtual void Start()
    {
        mainCam = Camera.main;
    }

    public void SetBattleManager(BattleManager battleManager)
    {
        _battleManager = battleManager;
    }

    /// <summary>
    /// 유닛 생성
    /// </summary>
    /// <param name="dataBase">유닛 데이터</param>
    /// <param name="eTeam">팀 변수</param>
    /// <param name="battleManager">배틀매니저</param>
    /// <param name="id"></param>
    public virtual void SetUnitData(DataBase dataBase, TeamType eTeam, StageData stageData, int id, int grade)
    {
        this.unitData = dataBase.unitData;
        this.eTeam = eTeam;
        collideData = new CollideData();
        collideData.originpoints = dataBase.unitData.colideData.originpoints;
        _unitSprite.SetUIAndSprite(eTeam, dataBase.card_Sprite);

        //딜레이시스템
        attack_Cur_Delay = 0;
        _unitSprite.Update_DelayBar(attack_Cur_Delay);
        Set_IsInvincibility(false);
        Set_IsDontThrow(false);
        _unitSprite.Show_Canvas(true);

        this.isInvincibility = true;
        this.isSettingEnd = false;

        //팀, 이름 설정
        _unitSprite.SetTeamColor(eTeam);
        transform.name = dataBase.card_Name + this.eTeam;
        
        
        this._stageData = stageData;
        this.maxhp = dataBase.unitData.unit_Hp * grade;
        this.hp = dataBase.unitData.unit_Hp;
        moveSpeedPercent = 100 * grade;
        attackSpeedPercent = 100 * grade;
        damagePercent = 100 * grade;
        this.weight = dataBase.unitData.unit_Weight;
        this.myUnitId = id;

        //깨짐 이미지
        _unitSprite.Set_HPSprite(hp, maxhp);

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
            statEffList[i].Process();
        }
    }

    /// <summary>
    /// 유닛 사망시 삭제
    /// </summary>
    public virtual void Delete_Unit()
    {
        _battleManager.PoolDeleteUnit(this);
        Delete_state();
        Delete_EffStetes();

        unitState = null;

        switch (eTeam)
        {
            case TeamType.Null:
                break;
            case TeamType.MyTeam:
                _battleManager.CommandUnit._playerUnitList.Remove(this);
                break;
            case TeamType.EnemyTeam:
                _battleManager.CommandUnit._enemyUnitList.Remove(this);
                break;
        }
    }

    /// <summary>
    /// 모든 상태이상 삭제
    /// </summary>
    public void Delete_EffStetes()
    {
        //모든 상태이상 삭제
        for (; statEffList.Count > 0;)
        {
            statEffList[0].Delete_StatusEffect();
        }
    }
    
    /// <summary>
    /// 스테이트 추가
    /// </summary>
    private void Add_state()
    {
        switch (unitData.unitType)
        {
            case UnitType.PencilCase:
                stateManager = PoolManager.GetItem<PencilCaseStateManager>(transform, _unitSprite.SpriteRenderer.transform, this);
                break;

            default:
            case UnitType.None:
            case UnitType.Pencil:
            case UnitType.Eraser:
            case UnitType.Sharp:
                stateManager = PoolManager.GetItem<PencilStateManager>(transform, _unitSprite.SpriteRenderer.transform, this);
                break;
            case UnitType.BallPen:
                stateManager = PoolManager.GetItem<BallpenStateManager>(transform, _unitSprite.SpriteRenderer.transform, this);
                break;
        }
        stateManager.SetStageData(_stageData);
    }

    /// <summary>
    /// 스테이트 삭제
    /// </summary>
    private void Delete_state()
    {
        switch (unitData.unitType)
        {
            case UnitType.PencilCase:
                PoolManager.AddItem((PencilCaseStateManager)stateManager);
                break;

            case UnitType.BallPen:
                PoolManager.AddItem((BallpenStateManager)stateManager);
                break;

            default:
            case UnitType.None:
            case UnitType.Pencil:
            case UnitType.Eraser:
            case UnitType.Sharp:
                PoolManager.AddItem((PencilStateManager)stateManager);
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
    public void Throw_Unit(Vector2 pos)
    {
        unitState.Throw_Unit(pos);
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
        _unitSprite.Set_HPSprite(hp, maxhp);
    }


    /// <summary>
    /// 공격 딜레이 설정
    /// </summary>
    /// <param name="delay"></param>
    public void Set_AttackDelay(float delay)
    {
        attack_Cur_Delay = delay;
    }



    /// <summary>
    /// 공격력 스탯 반환
    /// </summary>
    /// <returns></returns>
    public int Return_Damage()
    {
        return Mathf.RoundToInt(unitData.damage * (float)damagePercent / 100);
    }
    /// <summary>
    /// 이동속도 스탯 반환
    /// </summary>
    /// <returns></returns>
    public float Return_MoveSpeed()
    {
        return unitData.moveSpeed * (float)moveSpeedPercent / 100;
    }
    /// <summary>
    /// 공격속도 스탯 반환
    /// </summary>
    /// <returns></returns>
    public float Return_AttackSpeed()
    {
        return unitData.attackSpeed * (float)attackSpeedPercent / 100;
    }
    /// <summary>
    /// 사거리 스탯 반환
    /// </summary>
    /// <returns></returns>
    public float Return_Range()
    {
        return unitData.range * (float)rangePercent / 100;
    }
    /// <summary>
    /// 무게 스탯 반환
    /// </summary>
    /// <returns></returns>
    public int Return_Weight()
    {
        return Mathf.RoundToInt(unitData.unit_Weight * (float)weightPercent / 100);
    }
    /// <summary>
    /// 명중률 스탯 반환
    /// </summary>
    /// <returns></returns>
    public float Return_Accuracy()
    {
        return unitData.accuracy * (float)accuracyPercent / 100;
    }
    /// <summary>
    /// 넉백 스탯 반환
    /// </summary>
    /// <returns></returns>
    public int Return_Knockback()
    {
        return Mathf.RoundToInt(unitData.knockback * (float)knockbackPercent / 100);
    }
}
