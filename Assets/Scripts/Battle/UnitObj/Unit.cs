using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;
using Battle;
using DG.Tweening;
public class Unit : MonoBehaviour
{
    //프로퍼티
    public UnitSprite UnitSprite => _unitSprite;//유닛 스프라이트 및 UI 관리
    public UnitStateEff UnitStateEff => _unitStateEff; //유닛 상태이상 관리
    public UnitSticker UnitSticker => _unitSticker; //유닛 스티커
    public UnitStat UnitStat => _unitStat; // 유닛 스탯 관리
    public UnitStateChanger UnitStateChanger => _unitStateChanger; //유닛별 스테이트 관리
    public UnitData UnitData => _unitData; //유닛 데이터
    public SkinData SkinData => _skinData; //스킨 데이터
    public TeamType ETeam => _eTeam; // 유닛의 팀
    public CollideData CollideData => _collideData; // 유닛의 콜라이더 데이터
    public BattleManager BattleManager => _battleManager; //배틀매니저 참조
    public int MyDamagedId { get; protected set; } = 0; // 유닛의 현재 공격 ID
    public int DamageCount { get; set; } = 0; // 공격카운트
    public int MyUnitId { get; protected set; } = 0; //유닛의 ID
    public bool IsInvincibility { get; protected set; } = false; // 무적 & 무시 여부
    public bool IsNeverDontThrow { get; protected set; } = false; // 절대 던지기 가능 여부
    public bool IsDontThrow { get; protected set; } = false; // 던지기 가능 여부
    public Sequence KnockbackTweener => _knockbackTweener; //넉백에 사용하는 시퀀스
    public int OrderIndex { get; set; } = 0;
    public int ViewIndex => _viewIndex; //뷰 인덱스
    public Animator Animator => _animator; //애니메이터

    //변수
    private CollideData _collideData = default; 
    private UnitStateEff _unitStateEff = new UnitStateEff();
    private UnitStat _unitStat = new UnitStat();
    private TeamType _eTeam = TeamType.Null;
    private bool isThrowring = false; //던져지는 중인가
    protected UnitStateChanger _unitStateChanger = new UnitStateChanger();
    protected BattleManager _battleManager = null;    
    protected bool _isSettingEnd = false;
    protected Sequence _knockbackTweener;
    private int _viewIndex = 0;

    //참조 변수
    private UnitData _unitData= null;
    private SkinData _skinData= null;
    private StageData _stageData = null;

    //인스펙터 참조 변수
    [SerializeField]
    private UnitSprite _unitSprite = null;
    [SerializeField]
    private UnitSticker _unitSticker = null;
    [SerializeField]
    private Animator _animator = null;

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
    public virtual void SetUnitData(CardData dataBase, TeamType eTeam, StageData stageData, int id, int grade, int orderIndex)
    {
        isThrowring = false;

        _knockbackTweener = DOTween.Sequence();

        //순서 인덱스
        OrderIndex = orderIndex;

        //유닛 데이터 받아오기
        UnitData unitData = UnitDataManagerSO.FindUnitData(dataBase._unitType);
        _unitData = unitData;

        //스킨 데이터 받아오기
        _skinData = dataBase._skinData;

        //팀, 이름 설정
        _eTeam = eTeam;
        if(_eTeam == TeamType.MyTeam)
		{
            transform.localScale = new Vector3(1, 1, 1);
		}
        else if(_eTeam == TeamType.EnemyTeam)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        transform.name = dataBase._name + _eTeam;

        //물리판정 설정
        _collideData = new CollideData();
        _collideData.originpoints = unitData._colideData.originpoints;

        //딜레이시스템
        SetIsInvincibility(false);
        SetIsDontThrow(false);
        
        //스테이지 데이터 가져오기
        _stageData = stageData;

        //스탯 설정
        _unitStat.ResetStat(_unitData, grade);
        MyUnitId = id;

        //상태이상
        _unitStateEff.SetStateEff(this, _unitSprite.SpriteRenderer);

        //스프라이트 초기화
        _unitSprite.ResetSprite(eTeam, dataBase, _unitStat, orderIndex);

        //스테이트 설정
        _unitStateChanger.ResetUnitStateChanger(dataBase, transform, stageData, _unitSprite, this);

        //스티커 설정
        _unitSticker.SetSticker(this);

        //설정 끝, 무적판정 제거
        IsInvincibility = false;
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
        CheckOrder();
        _unitStateChanger.ProcessState();
        _unitStateEff.ProcessEff();
    }

    /// <summary>
    /// 오더 확인
    /// </summary>
    public void CheckOrder()
    {
        if(ETeam == TeamType.MyTeam)
        {
            if (OrderIndex + 1 < _battleManager.UnitComponent._playerUnitList.Count)
            {
                if(transform.position.x > _battleManager.UnitComponent._playerUnitList[OrderIndex + 1].transform.position.x)
                {
                    _battleManager.UnitComponent.SortPlayerUnitList();
                }
            }
        }
        else if (ETeam == TeamType.EnemyTeam)
        {
            if (OrderIndex + 1 < _battleManager.UnitComponent._enemyUnitList.Count)
            {
                if (-transform.position.x > -_battleManager.UnitComponent._enemyUnitList[OrderIndex + 1].transform.position.x)
                {
                    _battleManager.UnitComponent.SortEnemyUnitList();
                }
            }
        }
    }

    /// <summary>
    /// 유닛 사망시 삭제
    /// </summary>
    public virtual void Delete_Unit()
    {
        RemoveUnitList();
        _battleManager.UnitComponent.DeletePoolUnit(this);
        _unitStateChanger.DeleteState(_unitData._unitType);
        _unitStateChanger.StateNull();
        _unitStateEff.DeleteEffStetes();
        _unitSticker.DeleteSticekr();
    }

    /// <summary>
    /// 공격 맞음
    /// </summary>
    /// <param name="atkData">공격 데이터</param>
    public virtual void Run_Damaged(AtkData atkData)
    {
        if (_unitStateChanger.UnitState != null)
        {
            _unitStateChanger.UnitState.RunDamaged(atkData);
        }
    }

    /// <summary>
    /// 넉백 설정
    /// </summary>
    /// <param name="sequence"></param>
    public void SetKnockBack(Sequence sequence)
    {
        _knockbackTweener = sequence;
    }

    /// <summary>
    /// 가장 마지막으로 맞은 데미지 아이디 설정
    /// </summary>
    /// <param name="id"></param>
    public void SetDamagedId(int id)
    {
        MyDamagedId = id;
    }

    /// <summary>
    /// 속성 효과 적용
    /// </summary>
    /// <param name="atkType"></param>
    /// <param name="value"></param>
    public virtual void AddStatusEffect(EffAttackType atkType, params float[] value)
    {
        _unitStateEff.AddStatusEffect(atkType, value);
    }
    /// <summary>
    /// 속성 고유효과 적용
    /// </summary>
    /// <param name="atkType"></param>
    /// <param name="value"></param>
    public virtual void AddInherence(AtkData atkData)
    {
        atkData.RunIncrease(this);
    }

    /// <summary>
    /// 당길 유닛을 선택했을 때
    /// </summary>
    /// <returns></returns>
    public Unit Pull_Unit()
    {
        return _unitStateChanger?.UnitState?.PullUnit();
    }

    /// <summary>
    /// 유닛을 당기고 있을 때
    /// </summary>
    /// <returns></returns>
    public Unit Pulling_Unit()
    {
        return _unitStateChanger.UnitState.PullingUnit();
    }

    /// <summary>
    /// 유닛을 던졌을 때
    /// </summary>
    public void Throw_Unit(Vector2 pos)
    {
        isThrowring = false;
        UnitSprite.OrderDraw(_viewIndex);
        UnitSticker.OrderDraw(_viewIndex);
        _unitStateChanger.UnitState.ThrowUnit(pos);
    }

    /// <summary>
    /// 무적 여부 설정
    /// </summary>
    /// <param name="isboolean">True면 무적, False면 비무적</param>
    public void SetIsInvincibility(bool isboolean)
    {
        IsInvincibility = isboolean;
    }

    /// <summary>
    /// 던지기 가능 설정
    /// </summary>
    /// <param name="isboolean">True면 던지기 불가능, False면 던지기 가능</param>
    public void SetIsDontThrow(bool isboolean)
    {
        IsDontThrow = isboolean;
    }
    /// <summary>
    /// 절대 던지기 가능 설정
    /// </summary>
    /// <param name="isboolean">True면 던지기 불가능, False면 던지기 가능</param>
    public void SetIsNeverDontThrow(bool isboolean)
    {
        IsNeverDontThrow = isboolean;
    }

    /// <summary>
    /// 체력 감소
    /// </summary>
    /// <param name="damage">줄어들 체력</param>
    public void SubtractHP(int damage)
    {
        _unitStat.SubtractHP(damage);
        _unitSprite.SetHPSprite(_unitStat.Hp, _unitStat.MaxHp);
    }

    /// <summary>
    /// 보이기 순서 설정
    /// </summary>
    /// <param name="index"></param>
    public void SetOrderIndex(int index)
    {
        OrderIndex = index;
        if(ETeam == TeamType.MyTeam)
        {
            if (OrderIndex == _battleManager.UnitComponent._playerUnitList.Count - 1)
            {
                _viewIndex = 0;
            }
        }
        else if(ETeam == TeamType.EnemyTeam)
        {
            if (OrderIndex == _battleManager.UnitComponent._enemyUnitList.Count - 1)
            {
                _viewIndex = 0;
            }
        }
        if(isThrowring)
        {
            _unitSprite.OrderDraw(_viewIndex);
            _unitSticker.OrderDraw(_viewIndex);
        }
    }

    /// <summary>
    /// 유닛 리스트에서 이 오브젝트를 제거
    /// </summary>
    public void RemoveUnitList()
    {
        switch (_eTeam)
        {
            case TeamType.Null:
                break;
            case TeamType.MyTeam:
                _battleManager.UnitComponent._playerUnitList.Remove(this);
                break;
            case TeamType.EnemyTeam:
                _battleManager.UnitComponent._enemyUnitList.Remove(this);
                break;
        }
    }

    /// <summary>
    /// 던지기가 가능한지 체크
    /// </summary>
    /// <returns></returns>
    public bool CheckCanThrow()
    {
        if (!isThrowring && !IsDontThrow && !IsNeverDontThrow)
		{
            return true;
		}
        else
		{
            return false;
		}
    }

    /// <summary>
    /// 머테리얼을 변경한다
    /// </summary>
    public void ChangeMaterial(Material material)
	{
        //_unitSprite.ChangeMaterial(material);
	}
}
