using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;

public class Unit : MonoBehaviour
{

    public UnitData unitData;
    public CollideData collideData;
    public UnitState unitState { get; protected set; }

    public List<Eff_State> statEffList = new List<Eff_State>();

    [SerializeField]
    protected Canvas canvas;
    [SerializeField]
    protected Image delayBar;
    [SerializeField]
    protected SpriteMask sprMask;
    [SerializeField]
    protected SpriteRenderer spr;
    [SerializeField]
    protected SpriteRenderer hpSpr;
    [SerializeField]
    protected Sprite[] hpSprites;

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

    public BattleManager battleManager { get; protected set; }
    
    protected StageData stageData;
    protected IStateManager stateManager;

    private void Start()
    {
        mainCam = Camera.main;
        canvas.worldCamera = mainCam;
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="dataBase">���� ������</param>
    /// <param name="eTeam">�� ����</param>
    /// <param name="battleManager">��Ʋ�Ŵ���</param>
    /// <param name="id"></param>
    public virtual void Set_UnitData(DataBase dataBase, TeamType eTeam, BattleManager battleManager, int id)
    {
        this.unitData = dataBase.unitData;
        collideData = new CollideData();
        collideData.originpoints = dataBase.unitData.colideData.originpoints;

        //�����̽ý���
        attack_Cur_Delay = 0;
        Update_DelayBar(attack_Cur_Delay);
        delayBar.rectTransform.anchoredPosition = eTeam.Equals(TeamType.MyTeam) ? new Vector2(-960.15f, -540.15f) : new Vector2(-959.85f, -540.15f);
        Set_IsInvincibility(false);
        Set_IsDontThrow(false);
        Show_Canvas(true);

        this.isInvincibility = true;
        this.isSettingEnd = false;

        //��, �̸� ����
        Set_Team(eTeam);
        transform.name = dataBase.card_Name + this.eTeam;
        
        
        this.spr.sprite = dataBase.card_Sprite;
        this.battleManager = battleManager;
        this.stageData = battleManager.currentStageData;
        this.maxhp = dataBase.unitData.unit_Hp;
        this.hp = dataBase.unitData.unit_Hp;
        this.weight = dataBase.unitData.unit_Weight;
        this.myUnitId = id;

        //���� �̹���
        sprMask.sprite = dataBase.card_Sprite;
        Set_HPSprite();

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
        battleManager.Pool_DeleteUnit(this);
        Delete_state();
        Delete_EffStetes();

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
                stateManager = Battle_Unit.GetItem<PencilCaseStateManager>(transform, spr.transform, this);
                break;

            default:
            case UnitType.None:
            case UnitType.Pencil:
            case UnitType.Eraser:
            case UnitType.Sharp:
                stateManager = Battle_Unit.GetItem<PencilStateManager>(transform, spr.transform, this);
                break;
            case UnitType.BallPen:
                stateManager = Battle_Unit.GetItem<BallpenStateManager>(transform, spr.transform, this);
                break;
        }
    }

    /// <summary>
    /// ������Ʈ ����
    /// </summary>
    private void Delete_state()
    {
        switch (unitData.unitType)
        {
            case UnitType.PencilCase:
                Battle_Unit.AddItem((PencilCaseStateManager)stateManager);
                break;

            case UnitType.BallPen:
                Battle_Unit.AddItem((BallpenStateManager)stateManager);
                break;

            default:
            case UnitType.None:
            case UnitType.Pencil:
            case UnitType.Eraser:
            case UnitType.Sharp:
                Battle_Unit.AddItem((PencilStateManager)stateManager);
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
    /// �� ����
    /// </summary>
    /// <param name="eTeam"></param>
    private void Set_Team(TeamType eTeam)
    {
        this.eTeam = eTeam;
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
        Set_HPSprite();
    }

    /// <summary>
    /// ü�� ������ ���� ���� �̹���
    /// </summary>
    public void Set_HPSprite()
    {
        float percent = (float)hp / maxhp;

        if(percent > 0.5f)
        {
            hpSpr.sprite = null;
        }
        else if(percent > 0.2f)
        {
            hpSpr.sprite = hpSprites[0];
        }
        else
        {
            hpSpr.sprite = hpSprites[1];
        }
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
    /// �����̹� ������Ʈ
    /// </summary>
    /// <param name="delay"></param>
    public void Update_DelayBar(float delay)
    {
        delayBar.fillAmount = delay;
    }

    /// <summary>
    /// ĵ���� Ű�� ����
    /// </summary>
    /// <param name="isShow">True�� ĵ���� Ű�� �ƴϸ� ����</param>
    public void Show_Canvas(bool isShow)
    {
        canvas.gameObject.SetActive(isShow);
    }

    #region ���� ��ȯ

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

    #endregion

    #region �����

    [ContextMenu("����� �Լ� ����")]
    public void Debug_State()
    {
        Debug.Log(unitState.curState);
    }

    #endregion
}
