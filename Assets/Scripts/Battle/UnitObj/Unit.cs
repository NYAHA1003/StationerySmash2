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
    //������Ƽ
    public UnitSprite UnitSprite => _unitSprite;//���� ��������Ʈ �� UI ����
    public UnitStateEff UnitStateEff => _unitStateEff; //���� �����̻� ����
    public UnitSticker UnitSticker => _unitSticker; //���� ��ƼĿ
    public UnitStat UnitStat => _unitStat; // ���� ���� ����
    public UnitStateChanger UnitStateChanger => _unitStateChanger; //���ֺ� ������Ʈ ����
    public UnitData UnitData => _unitData; //���� ������
    public SkinData SkinData => _skinData; //��Ų ������
    public TeamType ETeam => _eTeam; // ������ ��
    public CollideData CollideData => _collideData; // ������ �ݶ��̴� ������
    public BattleManager BattleManager => _battleManager; //��Ʋ�Ŵ��� ����
    public int MyDamagedId { get; protected set; } = 0; // ������ ���� ���� ID
    public int DamageCount { get; set; } = 0; // ����ī��Ʈ
    public int MyUnitId { get; protected set; } = 0; //������ ID
    public bool IsInvincibility { get; protected set; } = false; // ���� & ���� ����
    public bool IsNeverDontThrow { get; protected set; } = false; // ���� ������ ���� ����
    public bool IsDontThrow { get; protected set; } = false; // ������ ���� ����
    public Sequence KnockbackTweener => _knockbackTweener; //�˹鿡 ����ϴ� ������
    public int OrderIndex { get; set; } = 0;
    public int ViewIndex => _viewIndex; //�� �ε���
    public Animator Animator => _animator; //�ִϸ�����

    //����
    private CollideData _collideData = default; 
    private UnitStateEff _unitStateEff = new UnitStateEff();
    private UnitStat _unitStat = new UnitStat();
    private TeamType _eTeam = TeamType.Null;
    private bool isThrowring = false; //�������� ���ΰ�
    protected UnitStateChanger _unitStateChanger = new UnitStateChanger();
    protected BattleManager _battleManager = null;    
    protected bool _isSettingEnd = false;
    protected Sequence _knockbackTweener;
    private int _viewIndex = 0;

    //���� ����
    private UnitData _unitData= null;
    private SkinData _skinData= null;
    private StageData _stageData = null;

    //�ν����� ���� ����
    [SerializeField]
    private UnitSprite _unitSprite = null;
    [SerializeField]
    private UnitSticker _unitSticker = null;
    [SerializeField]
    private Animator _animator = null;

    /// <summary>
    /// ��Ʋ�Ŵ��� ����
    /// </summary>
    /// <param name="battleManager"></param>
    public void SetBattleManager(BattleManager battleManager)
    {
        _battleManager = battleManager;
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="dataBase">���� ������</param>
    /// <param name="eTeam">�� ����</param>
    /// <param name="battleManager">��Ʋ�Ŵ���</param>
    /// <param name="id"></param>
    public virtual void SetUnitData(CardData dataBase, TeamType eTeam, StageData stageData, int id, int grade, int orderIndex)
    {
        isThrowring = false;

        _knockbackTweener = DOTween.Sequence();

        //���� �ε���
        OrderIndex = orderIndex;

        //���� ������ �޾ƿ���
        UnitData unitData = UnitDataManagerSO.FindUnitData(dataBase._unitType);
        _unitData = unitData;

        //��Ų ������ �޾ƿ���
        _skinData = dataBase._skinData;

        //��, �̸� ����
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

        //�������� ����
        _collideData = new CollideData();
        _collideData.originpoints = unitData._colideData.originpoints;

        //�����̽ý���
        SetIsInvincibility(false);
        SetIsDontThrow(false);
        
        //�������� ������ ��������
        _stageData = stageData;

        //���� ����
        _unitStat.ResetStat(_unitData, grade);
        MyUnitId = id;

        //�����̻�
        _unitStateEff.SetStateEff(this, _unitSprite.SpriteRenderer);

        //��������Ʈ �ʱ�ȭ
        _unitSprite.ResetSprite(eTeam, dataBase, _unitStat, orderIndex);

        //������Ʈ ����
        _unitStateChanger.ResetUnitStateChanger(dataBase, transform, stageData, _unitSprite, this);

        //��ƼĿ ����
        _unitSticker.SetSticker(this);

        //���� ��, �������� ����
        IsInvincibility = false;
        _isSettingEnd = true;
    }

    /// <summary>
    /// ���� ���� ������Ʈ
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
    /// ���� Ȯ��
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
    /// ���� ����� ����
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
    /// ���� ����
    /// </summary>
    /// <param name="atkData">���� ������</param>
    public virtual void Run_Damaged(AtkData atkData)
    {
        if (_unitStateChanger.UnitState != null)
        {
            _unitStateChanger.UnitState.RunDamaged(atkData);
        }
    }

    /// <summary>
    /// �˹� ����
    /// </summary>
    /// <param name="sequence"></param>
    public void SetKnockBack(Sequence sequence)
    {
        _knockbackTweener = sequence;
    }

    /// <summary>
    /// ���� ���������� ���� ������ ���̵� ����
    /// </summary>
    /// <param name="id"></param>
    public void SetDamagedId(int id)
    {
        MyDamagedId = id;
    }

    /// <summary>
    /// �Ӽ� ȿ�� ����
    /// </summary>
    /// <param name="atkType"></param>
    /// <param name="value"></param>
    public virtual void AddStatusEffect(EffAttackType atkType, params float[] value)
    {
        _unitStateEff.AddStatusEffect(atkType, value);
    }
    /// <summary>
    /// �Ӽ� ����ȿ�� ����
    /// </summary>
    /// <param name="atkType"></param>
    /// <param name="value"></param>
    public virtual void AddInherence(AtkData atkData)
    {
        atkData.RunIncrease(this);
    }

    /// <summary>
    /// ��� ������ �������� ��
    /// </summary>
    /// <returns></returns>
    public Unit Pull_Unit()
    {
        return _unitStateChanger?.UnitState?.PullUnit();
    }

    /// <summary>
    /// ������ ���� ���� ��
    /// </summary>
    /// <returns></returns>
    public Unit Pulling_Unit()
    {
        return _unitStateChanger.UnitState.PullingUnit();
    }

    /// <summary>
    /// ������ ������ ��
    /// </summary>
    public void Throw_Unit(Vector2 pos)
    {
        isThrowring = false;
        UnitSprite.OrderDraw(_viewIndex);
        UnitSticker.OrderDraw(_viewIndex);
        _unitStateChanger.UnitState.ThrowUnit(pos);
    }

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    /// <param name="isboolean">True�� ����, False�� ����</param>
    public void SetIsInvincibility(bool isboolean)
    {
        IsInvincibility = isboolean;
    }

    /// <summary>
    /// ������ ���� ����
    /// </summary>
    /// <param name="isboolean">True�� ������ �Ұ���, False�� ������ ����</param>
    public void SetIsDontThrow(bool isboolean)
    {
        IsDontThrow = isboolean;
    }
    /// <summary>
    /// ���� ������ ���� ����
    /// </summary>
    /// <param name="isboolean">True�� ������ �Ұ���, False�� ������ ����</param>
    public void SetIsNeverDontThrow(bool isboolean)
    {
        IsNeverDontThrow = isboolean;
    }

    /// <summary>
    /// ü�� ����
    /// </summary>
    /// <param name="damage">�پ�� ü��</param>
    public void SubtractHP(int damage)
    {
        _unitStat.SubtractHP(damage);
        _unitSprite.SetHPSprite(_unitStat.Hp, _unitStat.MaxHp);
    }

    /// <summary>
    /// ���̱� ���� ����
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
    /// ���� ����Ʈ���� �� ������Ʈ�� ����
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
    /// �����Ⱑ �������� üũ
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
    /// ���׸����� �����Ѵ�
    /// </summary>
    public void ChangeMaterial(Material material)
	{
        //_unitSprite.ChangeMaterial(material);
	}
}
