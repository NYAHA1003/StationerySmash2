using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;
using Battle;
public class Unit : MonoBehaviour
{
    //프로퍼티
    public UnitSprite UnitSprite => _unitSprite;
    public UnitStateEff UnitStateEff => _unitStateEff;
    public UnitStat UnitStat => _unitStat;
    public UnitState unitState { get; protected set; }
    public float attack_Cur_Delay { get; protected set; }

    //변수
    private UnitStat _unitStat = new UnitStat();

    //참조 변수
    public UnitData unitData;
    public CollideData collideData;
    protected StageData _stageData;
    protected IStateManager stateManager;

    public TeamType eTeam;

    protected Camera mainCam;

    public int myDamagedId { get; protected set; } = 0;
    public int damageCount { get; set; } = 0;
    public int myUnitId { get; protected set; } = 0;

    //스탯 퍼센트
    public bool isInvincibility { get; protected set; }
    public bool isDontThrow { get; protected set; }

    //유닛 설정 여부
    protected bool isSettingEnd;

    public BattleManager _battleManager { get; protected set; }
    

    //인스펙터 참조 변수
    [SerializeField]
    private UnitSprite _unitSprite = null;
    private UnitStateEff _unitStateEff = new UnitStateEff();

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
        _unitStateEff.SetStateEff(this, _unitSprite.SpriteRenderer);
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
        _unitStat.SetUnitData(unitData);
        _unitStat.SetGradeStat(grade);
        _unitStat.SetWeight();
        this.myUnitId = id;

        //깨짐 이미지
        _unitSprite.Set_HPSprite(_unitStat._hp, _unitStat._maxHp);

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
        _unitStateEff.ProcessEff();
    }

    /// <summary>
    /// 유닛 사망시 삭제
    /// </summary>
    public virtual void Delete_Unit()
    {
        _battleManager.PoolDeleteUnit(this);
        Delete_state();
        _unitStateEff.DeleteEffStetes();

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
                //stateManager = PoolManager.GetItem<BallpenStateManager>(transform, _unitSprite.SpriteRenderer.transform, this);
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
                //PoolManager.AddItem((BallpenStateManager)stateManager);
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
    public virtual void AddStatusEffect(AtkType atkType, params float[] value)
    {
        _unitStateEff.AddStatusEffect(atkType, value);
    }


    /// <summary>
    /// 당길 유닛을 선택했을 때
    /// </summary>
    /// <returns></returns>
    public Unit Pull_Unit()
    {
        return unitState.Pull_Unit();
    }

    /// <summary>
    /// 유닛을 당기고 있을 때
    /// </summary>
    /// <returns></returns>
    public Unit Pulling_Unit()
    {
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
    public void SubtractHP(int damage)
    {
        _unitStat.SubtractHP(damage);
        _unitSprite.Set_HPSprite(_unitStat._hp, _unitStat._maxHp);
    }


    /// <summary>
    /// 공격 딜레이 설정
    /// </summary>
    /// <param name="delay"></param>
    public void Set_AttackDelay(float delay)
    {
        attack_Cur_Delay = delay;
    }
}
