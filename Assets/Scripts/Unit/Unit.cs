using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;
using Battle;
public class Unit : MonoBehaviour
{
    //프로퍼티
    public UnitSprite UnitSprite => _unitSprite;//유닛 스프라이트 및 UI 관리
    public UnitStateEff UnitStateEff => _unitStateEff; //유닛 상태이상 관리
    public UnitStat UnitStat => _unitStat; // 유닛 스탯 관리
    public UnitStateChanger UnitStateChanger => _unitStateChanger; //유닛별 스테이트 관리
    public UnitData UnitData => _unitData; //유닛 데이터
    public TeamType ETeam => _eTeam; // 유닛의 팀
    public CollideData CollideData => _collideData; // 유닛의 콜라이더 데이터
    public BattleManager BattleManager => _battleManager; //배틀매니저 참조
    public int MyDamagedId { get; protected set; } = 0; // 유닛의 현재 공격 ID
    public int DamageCount { get; set; } = 0; // 공격카운트
    public int MyUnitId { get; protected set; } = 0; //유닛의 ID
    public bool _isInvincibility { get; protected set; } = false; // 무적 & 무시 여부
    public bool _isDontThrow { get; protected set; } = false; // 던지기 가능 여부
    
    //변수
    private CollideData _collideData = default; 
    private UnitStateEff _unitStateEff = new UnitStateEff();
    private UnitStat _unitStat = new UnitStat();
    private UnitStateChanger _unitStateChanger = new UnitStateChanger();
    private TeamType _eTeam = TeamType.Null;
    protected BattleManager _battleManager = null;    
    protected bool _isSettingEnd = false;

    //참조 변수
    private UnitData _unitData= null;
    private StageData _stageData = null;
    private Camera _mainCam = null;

    //인스펙터 참조 변수
    [SerializeField]
    private UnitSprite _unitSprite = null;

    protected virtual void Start()
    {
        _mainCam = Camera.main;
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
        //유닛 데이터 받아오기
        _unitData = dataBase.unitData;

        //팀, 이름 설정
        _eTeam = eTeam;
        transform.name = dataBase.card_Name + _eTeam;

        //물리판정 설정
        _collideData = new CollideData();
        _collideData.originpoints = dataBase.unitData.colideData.originpoints;

        //딜레이시스템
        Set_IsInvincibility(false);
        Set_IsDontThrow(false);
        
        //스테이지 데이터 가져오기
        _stageData = stageData;

        //스탯 설정
        _unitStat.ResetAttackDelay();
        _unitStat.SetUnitData(_unitData);
        _unitStat.SetGradeStat(grade);
        _unitStat.SetWeight();
        MyUnitId = id;

        //상태이상
        _unitStateEff.SetStateEff(this, _unitSprite.SpriteRenderer);

        //스프라이트 초기화
        _unitSprite.SetUIAndSprite(eTeam, dataBase.card_Sprite);
        _unitSprite.UpdateDelayBar(_unitStat.AttackDelay);
        _unitSprite.ShowCanvas(true);
        _unitSprite.SetTeamColor(eTeam);
        _unitSprite.Set_HPSprite(_unitStat.Hp, _unitStat.MaxHp);

        //스테이트 설정
        _unitStateChanger.SetStateManager(dataBase.unitData.unitType, transform, _unitSprite.SpriteRenderer.transform, this); ;
        _unitStateChanger.SetStageData(_stageData);
        _unitStateChanger.SetUnitState();

        //설정 끝, 무적판정 제거
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
        RemoveUnitList();
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
        _isDontThrow = isboolean;
    }

    /// <summary>
    /// 체력 감소
    /// </summary>
    /// <param name="damage">줄어들 체력</param>
    public void SubtractHP(int damage)
    {
        _unitStat.SubtractHP(damage);
        _unitSprite.Set_HPSprite(_unitStat.Hp, _unitStat.MaxHp);
    }


    /// <summary>
    /// 유닛 리스트에서 이 오브젝트를 제거
    /// </summary>
    private void RemoveUnitList()
    {
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

}
