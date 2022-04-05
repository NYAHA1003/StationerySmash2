using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;
using Battle;
public class Unit : MonoBehaviour
{
    //������Ƽ
    public UnitSprite UnitSprite => _unitSprite;
    public UnitStateEff UnitStateEff => _unitStateEff;
    public UnitStat UnitStat => _unitStat;
    public UnitState unitState { get; protected set; }
    public float attack_Cur_Delay { get; protected set; }

    //����
    private UnitStat _unitStat = new UnitStat();

    //���� ����
    public UnitData unitData;
    public CollideData collideData;
    protected StageData _stageData;
    protected IStateManager stateManager;

    public TeamType eTeam;

    protected Camera mainCam;

    public int myDamagedId { get; protected set; } = 0;
    public int damageCount { get; set; } = 0;
    public int myUnitId { get; protected set; } = 0;

    //���� �ۼ�Ʈ
    public bool isInvincibility { get; protected set; }
    public bool isDontThrow { get; protected set; }

    //���� ���� ����
    protected bool isSettingEnd;

    public BattleManager _battleManager { get; protected set; }
    

    //�ν����� ���� ����
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
    /// ���� ����
    /// </summary>
    /// <param name="dataBase">���� ������</param>
    /// <param name="eTeam">�� ����</param>
    /// <param name="battleManager">��Ʋ�Ŵ���</param>
    /// <param name="id"></param>
    public virtual void SetUnitData(DataBase dataBase, TeamType eTeam, StageData stageData, int id, int grade)
    {
        _unitStateEff.SetStateEff(this, _unitSprite.SpriteRenderer);
        this.unitData = dataBase.unitData;
        this.eTeam = eTeam;
        collideData = new CollideData();
        collideData.originpoints = dataBase.unitData.colideData.originpoints;
        _unitSprite.SetUIAndSprite(eTeam, dataBase.card_Sprite);

        //�����̽ý���
        attack_Cur_Delay = 0;
        _unitSprite.Update_DelayBar(attack_Cur_Delay);
        Set_IsInvincibility(false);
        Set_IsDontThrow(false);
        _unitSprite.Show_Canvas(true);

        this.isInvincibility = true;
        this.isSettingEnd = false;

        //��, �̸� ����
        _unitSprite.SetTeamColor(eTeam);
        transform.name = dataBase.card_Name + this.eTeam;
        
        
        this._stageData = stageData;
        _unitStat.SetUnitData(unitData);
        _unitStat.SetGradeStat(grade);
        _unitStat.SetWeight();
        this.myUnitId = id;

        //���� �̹���
        _unitSprite.Set_HPSprite(_unitStat._hp, _unitStat._maxHp);

        //������Ʈ ����
        Add_state();

        unitState = stateManager.Return_CurrentUnitState();

        this.isInvincibility = false;
        this.isSettingEnd = true;
    }


    /// <summary>
    /// ���� ���� ������Ʈ
    /// </summary>
    protected virtual void Update()
    {
        if (!isSettingEnd) return;

        unitState = unitState.Process();
        _unitStateEff.ProcessEff();
    }

    /// <summary>
    /// ���� ����� ����
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
    /// ������Ʈ �߰�
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
    /// ������Ʈ ����
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
    /// ���� ����
    /// </summary>
    /// <param name="atkData">���� ������</param>
    public void Run_Damaged(AtkData atkData)
    {
        unitState.Run_Damaged(atkData);
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
        return unitState.Pull_Unit();
    }

    /// <summary>
    /// ������ ���� ���� ��
    /// </summary>
    /// <returns></returns>
    public Unit Pulling_Unit()
    {
        return unitState.Pulling_Unit();
    }

    /// <summary>
    /// ������ ������ ��
    /// </summary>
    public void Throw_Unit(Vector2 pos)
    {
        unitState.Throw_Unit(pos);
    }


    /// <summary>
    /// ���� ���� ����
    /// </summary>
    /// <param name="isboolean">True�� ����, False�� ����</param>
    public void Set_IsInvincibility(bool isboolean)
    {
        isInvincibility = isboolean;
    }

    /// <summary>
    /// ������ ���� ����
    /// </summary>
    /// <param name="isboolean">True�� ������ �Ұ���, False�� ������ ����</param>
    public void Set_IsDontThrow(bool isboolean)
    {
        isDontThrow = isboolean;
    }

    /// <summary>
    /// ü�� ����
    /// </summary>
    /// <param name="damage">�پ�� ü��</param>
    public void SubtractHP(int damage)
    {
        _unitStat.SubtractHP(damage);
        _unitSprite.Set_HPSprite(_unitStat._hp, _unitStat._maxHp);
    }


    /// <summary>
    /// ���� ������ ����
    /// </summary>
    /// <param name="delay"></param>
    public void Set_AttackDelay(float delay)
    {
        attack_Cur_Delay = delay;
    }
}
