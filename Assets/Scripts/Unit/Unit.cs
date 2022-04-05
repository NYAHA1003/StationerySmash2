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
    public UnitStateChanger UnitStateChanger => _unitStateChanger;
    public UnitData UnitData => _unitData;
    public TeamType ETeam => _eTeam;
    public CollideData _collideData;
    public BattleManager _battleManager { get; protected set; } = null;
    public int myDamagedId { get; protected set; } = 0;
    public int damageCount { get; set; } = 0;
    public int _myUnitId { get; protected set; } = 0;
    public bool _isInvincibility { get; protected set; } = false;
    public bool isDontThrow { get; protected set; } = false;
    
    //변수
    private UnitStat _unitStat = new UnitStat();
    private UnitStateChanger _unitStateChanger = new UnitStateChanger();
    private TeamType _eTeam = TeamType.Null;

    //참조 변수
    private UnitData _unitData= null;
    private StageData _stageData = null;
    private Camera mainCam = null;

    //유닛 설정 여부
    protected bool _isSettingEnd;
    
    //인스펙터 참조 변수
    [SerializeField]
    private UnitSprite _unitSprite = null;
    private UnitStateEff _unitStateEff = new UnitStateEff();

    protected virtual void Start()
    {
        mainCam = Camera.main;
    }

    /// <summary>
    /// 배틀매니저 설정
    /// </summary>
    /// <param name="battleManager"></param>
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
        _unitData = dataBase.unitData;
        _eTeam = eTeam;
        _collideData = new CollideData();
        _collideData.originpoints = dataBase.unitData.colideData.originpoints;
        _unitSprite.SetUIAndSprite(eTeam, dataBase.card_Sprite);

        //딜레이시스템
        _unitStat.ResetAttackDelay();
        _unitSprite.Update_DelayBar(_unitStat._attackDelay);
        Set_IsInvincibility(false);
        Set_IsDontThrow(false);
        _unitSprite.Show_Canvas(true);

        _isInvincibility = true;
        _isSettingEnd = false;

        //팀, 이름 설정
        _unitSprite.SetTeamColor(eTeam);
        transform.name = dataBase.card_Name + _eTeam;
        
        //스탯 설정
        _stageData = stageData;
        _unitStat.SetUnitData(_unitData);
        _unitStat.SetGradeStat(grade);
        _unitStat.SetWeight();
        _myUnitId = id;

        //깨짐 이미지
        _unitSprite.Set_HPSprite(_unitStat._hp, _unitStat._maxHp);

        //스테이트 설정
        _unitStateChanger.StateManager.SetStageData(_stageData);
        _unitStateChanger.SetStateManager(dataBase.unitData.unitType, transform, _unitSprite.SpriteRenderer.transform, this); ;
        _unitStateChanger.SetUnitState();

        _isInvincibility = false;
        _isSettingEnd = true;
    }


    /// <summary>
    /// 유닛 상태 업데이트
    /// </summary>
    protected virtual void Update()
    {
        if (!_isSettingEnd)
        {
            return;
        }
        _unitStateChanger.ProcessState();
        _unitStateEff.ProcessEff();
    }

    /// <summary>
    /// 유닛 사망시 삭제
    /// </summary>
    public virtual void Delete_Unit()
    {
        _battleManager.PoolDeleteUnit(this);
        _unitStateChanger.DeleteState(_unitData.unitType);
        _unitStateChanger.StateNull();
        _unitStateEff.DeleteEffStetes();

        switch (_eTeam)
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
    /// 공격 맞음
    /// </summary>
    /// <param name="atkData">공격 데이터</param>
    public void Run_Damaged(AtkData atkData)
    {
        _unitStateChanger.UnitState.Run_Damaged(atkData);
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
        return _unitStateChanger.UnitState.Pull_Unit();
    }

    /// <summary>
    /// 유닛을 당기고 있을 때
    /// </summary>
    /// <returns></returns>
    public Unit Pulling_Unit()
    {
        return _unitStateChanger.UnitState.Pulling_Unit();
    }

    /// <summary>
    /// 유닛을 던졌을 때
    /// </summary>
    public void Throw_Unit(Vector2 pos)
    {
        _unitStateChanger.UnitState.Throw_Unit(pos);
    }


    /// <summary>
    /// 무적 여부 설정
    /// </summary>
    /// <param name="isboolean">True면 무적, False면 비무적</param>
    public void Set_IsInvincibility(bool isboolean)
    {
        _isInvincibility = isboolean;
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

}
