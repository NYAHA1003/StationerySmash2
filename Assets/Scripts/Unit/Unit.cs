using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;
using Battle;
public class Unit : MonoBehaviour
{
    //������Ƽ
    public UnitSprite UnitSprite => _unitSprite;//���� ��������Ʈ �� UI ����
    public UnitStateEff UnitStateEff => _unitStateEff; //���� �����̻� ����
    public UnitStat UnitStat => _unitStat; // ���� ���� ����
    public UnitStateChanger UnitStateChanger => _unitStateChanger; //���ֺ� ������Ʈ ����
    public UnitData UnitData => _unitData; //���� ������
    public TeamType ETeam => _eTeam; // ������ ��
    public CollideData CollideData => _collideData; // ������ �ݶ��̴� ������
    public BattleManager BattleManager => _battleManager; //��Ʋ�Ŵ��� ����
    public int MyDamagedId { get; protected set; } = 0; // ������ ���� ���� ID
    public int DamageCount { get; set; } = 0; // ����ī��Ʈ
    public int MyUnitId { get; protected set; } = 0; //������ ID
    public bool _isInvincibility { get; protected set; } = false; // ���� & ���� ����
    public bool _isDontThrow { get; protected set; } = false; // ������ ���� ����
    
    //����
    private CollideData _collideData = default; 
    private UnitStateEff _unitStateEff = new UnitStateEff();
    private UnitStat _unitStat = new UnitStat();
    private UnitStateChanger _unitStateChanger = new UnitStateChanger();
    private TeamType _eTeam = TeamType.Null;
    protected BattleManager _battleManager = null;    
    protected bool _isSettingEnd = false;

    //���� ����
    private UnitData _unitData= null;
    private StageData _stageData = null;
    private Camera _mainCam = null;

    //�ν����� ���� ����
    [SerializeField]
    private UnitSprite _unitSprite = null;

    protected virtual void Start()
    {
        _mainCam = Camera.main;
    }

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
    public virtual void SetUnitData(DataBase dataBase, TeamType eTeam, StageData stageData, int id, int grade)
    {
        //���� ������ �޾ƿ���
        _unitData = dataBase.unitData;

        //��, �̸� ����
        _eTeam = eTeam;
        transform.name = dataBase.card_Name + _eTeam;

        //�������� ����
        _collideData = new CollideData();
        _collideData.originpoints = dataBase.unitData.colideData.originpoints;

        //�����̽ý���
        Set_IsInvincibility(false);
        Set_IsDontThrow(false);
        
        //�������� ������ ��������
        _stageData = stageData;

        //���� ����
        _unitStat.ResetAttackDelay();
        _unitStat.SetUnitData(_unitData);
        _unitStat.SetGradeStat(grade);
        _unitStat.SetWeight();
        MyUnitId = id;

        //�����̻�
        _unitStateEff.SetStateEff(this, _unitSprite.SpriteRenderer);

        //��������Ʈ �ʱ�ȭ
        _unitSprite.SetUIAndSprite(eTeam, dataBase.card_Sprite);
        _unitSprite.UpdateDelayBar(_unitStat.AttackDelay);
        _unitSprite.ShowCanvas(true);
        _unitSprite.SetTeamColor(eTeam);
        _unitSprite.Set_HPSprite(_unitStat.Hp, _unitStat.MaxHp);

        //������Ʈ ����
        _unitStateChanger.SetStateManager(dataBase.unitData.unitType, transform, _unitSprite.SpriteRenderer.transform, this); ;
        _unitStateChanger.SetStageData(_stageData);
        _unitStateChanger.SetUnitState();

        //���� ��, �������� ����
        _isInvincibility = false;
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
        _unitStateChanger.ProcessState();
        _unitStateEff.ProcessEff();
    }

    /// <summary>
    /// ���� ����� ����
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
    /// ���� ����
    /// </summary>
    /// <param name="atkData">���� ������</param>
    public void Run_Damaged(AtkData atkData)
    {
        _unitStateChanger.UnitState.Run_Damaged(atkData);
    }

    /// <summary>
    /// �Ӽ� ȿ�� ����
    /// </summary>
    /// <param name="atkType"></param>
    /// <param name="value"></param>
    public virtual void AddStatusEffect(AtkType atkType, params float[] value)
    {
        _unitStateEff.AddStatusEffect(atkType, value);
    }

    /// <summary>
    /// ��� ������ �������� ��
    /// </summary>
    /// <returns></returns>
    public Unit Pull_Unit()
    {
        return _unitStateChanger.UnitState.Pull_Unit();
    }

    /// <summary>
    /// ������ ���� ���� ��
    /// </summary>
    /// <returns></returns>
    public Unit Pulling_Unit()
    {
        return _unitStateChanger.UnitState.Pulling_Unit();
    }

    /// <summary>
    /// ������ ������ ��
    /// </summary>
    public void Throw_Unit(Vector2 pos)
    {
        _unitStateChanger.UnitState.Throw_Unit(pos);
    }

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    /// <param name="isboolean">True�� ����, False�� ����</param>
    public void Set_IsInvincibility(bool isboolean)
    {
        _isInvincibility = isboolean;
    }

    /// <summary>
    /// ������ ���� ����
    /// </summary>
    /// <param name="isboolean">True�� ������ �Ұ���, False�� ������ ����</param>
    public void Set_IsDontThrow(bool isboolean)
    {
        _isDontThrow = isboolean;
    }

    /// <summary>
    /// ü�� ����
    /// </summary>
    /// <param name="damage">�پ�� ü��</param>
    public void SubtractHP(int damage)
    {
        _unitStat.SubtractHP(damage);
        _unitSprite.Set_HPSprite(_unitStat.Hp, _unitStat.MaxHp);
    }


    /// <summary>
    /// ���� ����Ʈ���� �� ������Ʈ�� ����
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
