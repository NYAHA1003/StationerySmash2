using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;
using Battle;
public class Unit : MonoBehaviour
{
    //�ν����� ���� ����
    //���� ����
    //����
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

    //���� �ۼ�Ʈ
    public int damagePercent = 100;
    public int moveSpeedPercent = 100;
    public int attackSpeedPercent = 100;
    public int rangePercent = 100;
    public int accuracyPercent = 100;
    public int weightPercent = 100;
    public int knockbackPercent = 100;
    public bool isInvincibility { get; protected set; }
    public bool isDontThrow { get; protected set; }

    //���� ���� ����
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
    /// ���� ����
    /// </summary>
    /// <param name="dataBase">���� ������</param>
    /// <param name="eTeam">�� ����</param>
    /// <param name="battleManager">��Ʋ�Ŵ���</param>
    /// <param name="id"></param>
    public virtual void SetUnitData(DataBase dataBase, TeamType eTeam, StageData stageData, int id, int grade)
    {
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
        this.maxhp = dataBase.unitData.unit_Hp * grade;
        this.hp = dataBase.unitData.unit_Hp;
        moveSpeedPercent = 100 * grade;
        attackSpeedPercent = 100 * grade;
        damagePercent = 100 * grade;
        this.weight = dataBase.unitData.unit_Weight;
        this.myUnitId = id;

        //���� �̹���
        _unitSprite.Set_HPSprite(hp, maxhp);

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

        for (int i = 0; i < statEffList.Count; i++)
        {
            statEffList[i].Process();
        }
    }

    /// <summary>
    /// ���� ����� ����
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
    /// ��� �����̻� ����
    /// </summary>
    public void Delete_EffStetes()
    {
        //��� �����̻� ����
        for (; statEffList.Count > 0;)
        {
            statEffList[0].Delete_StatusEffect();
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
                stateManager = PoolManager.GetItem<BallpenStateManager>(transform, _unitSprite.SpriteRenderer.transform, this);
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
    public void Add_StatusEffect(AtkType atkType, params float[] value)
    {
        unitState.Add_StatusEffect(atkType, value);
    }


    /// <summary>
    /// ��� ������ �������� ��
    /// </summary>
    /// <returns></returns>
    public Unit Pull_Unit()
    {
        //��� ���� ����
        return unitState.Pull_Unit();
    }

    /// <summary>
    /// ������ ���� ���� ��
    /// </summary>
    /// <returns></returns>
    public Unit Pulling_Unit()
    {
        //���� ���� ��
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
    public void Subtract_HP(int damage)
    {
        hp -= damage;
        _unitSprite.Set_HPSprite(hp, maxhp);
    }


    /// <summary>
    /// ���� ������ ����
    /// </summary>
    /// <param name="delay"></param>
    public void Set_AttackDelay(float delay)
    {
        attack_Cur_Delay = delay;
    }



    /// <summary>
    /// ���ݷ� ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public int Return_Damage()
    {
        return Mathf.RoundToInt(unitData.damage * (float)damagePercent / 100);
    }
    /// <summary>
    /// �̵��ӵ� ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public float Return_MoveSpeed()
    {
        return unitData.moveSpeed * (float)moveSpeedPercent / 100;
    }
    /// <summary>
    /// ���ݼӵ� ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public float Return_AttackSpeed()
    {
        return unitData.attackSpeed * (float)attackSpeedPercent / 100;
    }
    /// <summary>
    /// ��Ÿ� ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public float Return_Range()
    {
        return unitData.range * (float)rangePercent / 100;
    }
    /// <summary>
    /// ���� ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public int Return_Weight()
    {
        return Mathf.RoundToInt(unitData.unit_Weight * (float)weightPercent / 100);
    }
    /// <summary>
    /// ���߷� ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public float Return_Accuracy()
    {
        return unitData.accuracy * (float)accuracyPercent / 100;
    }
    /// <summary>
    /// �˹� ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public int Return_Knockback()
    {
        return Mathf.RoundToInt(unitData.knockback * (float)knockbackPercent / 100);
    }
}
