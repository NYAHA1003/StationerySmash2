using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;

public abstract class Unit : MonoBehaviour
{
    public UnitData unitData;
    public UnitState unitState { get; protected set; }

    [SerializeField]
    protected SpriteRenderer spr;
    
    public TeamType eTeam;


    public int myDamagedId = 0;
    public int damageCount = 0;
    public int myUnitId;
    public int hp { get; protected set; }
    public int maxhp { get; protected set; }
    public int weight { get; protected set; }

    public int damagePercent = 100;
    public int moveSpeedPercent = 100;
    public int attackSpeedPercent = 100;
    public int rangePercent = 100;
    public int accuracyPercent = 100;
    public int weightPercent = 100;
    public int knockbackPercent = 100;
    public bool isInvincibility { get; protected set; }
    public bool isDontThrow { get; protected set; }

    protected bool isSettingEnd;

    public BattleManager battleManager { get; protected set; }
    
    protected StageData stageData;

    #region �⺻ ����

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="dataBase">���� ������</param>
    /// <param name="eTeam">�� ����</param>
    /// <param name="battleManager">��Ʋ�Ŵ���</param>
    /// <param name="id"></param>
    public virtual void Set_UnitData(DataBase dataBase, TeamType eTeam, BattleManager battleManager, int id)
    {
        this.isSettingEnd = false;

        //��, �̸� ����
        this.eTeam = eTeam;
        transform.name = dataBase.card_Name + this.eTeam;

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
        
        
        this.spr.sprite = dataBase.card_Sprite;
        this.battleManager = battleManager;
        this.stageData = battleManager.currentStageData;
        this.maxhp = dataBase.unitData.unit_Hp;
        this.hp = dataBase.unitData.unit_Hp;
        this.weight = dataBase.unitData.unit_Weight;
        this.myUnitId = id;

        this.isSettingEnd = true;
    }

    /// <summary>
    /// ���� ������Ʈ ���� ����
    /// </summary>
    protected virtual void Update()
    {
        if (!isSettingEnd) return;

        unitState = unitState.Process();

    }

    /// <summary>
    /// ���� ����� ����
    /// </summary>
    public virtual void Delete_Unit()
    {
        battleManager.Pool_DeleteUnit(this);

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

    #endregion

    #region ������ ������ �ؾ���

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="atkData">���� ������</param>
    public abstract void Run_Damaged(AtkData atkData);

    /// <summary>
    /// �Ӽ� ȿ�� ����
    /// </summary>
    /// <param name="atkType"></param>
    /// <param name="value"></param>
    public abstract void Add_StatusEffect(AtkType atkType, params float[] value);

    #endregion

    #region ������ �ý���

    /// <summary>
    /// ��� ������ �������� ��
    /// </summary>
    /// <returns></returns>
    public virtual Unit Pull_Unit()
    {
        //��� ���� ����
        return null;
    }

    /// <summary>
    /// ������ ���� ���� ��
    /// </summary>
    /// <returns></returns>
    public virtual Unit Pulling_Unit()
    {
        //���� ���� ��
        return null;
    }

    /// <summary>
    /// ������ ������ ��
    /// </summary>
    public virtual void Throw_Unit()
    {

    }

    #endregion

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    /// <param name="isboolean">True�� ����, False�� ����</param>
    public virtual void Set_IsInvincibility(bool isboolean)
    {
        isInvincibility = isboolean;
    }

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    /// <param name="isboolean">True�� ������ �Ұ���, False�� ������ ����</param>
    public virtual void Set_IsDontThrow(bool isboolean)
    {
        isDontThrow = isboolean;
    }

    /// <summary>
    /// ü�� ����
    /// </summary>
    /// <param name="damage">�پ�� ü��</param>
    public virtual void Subtract_HP(int damage)
    {
        hp -= damage;
    }

    #region ���� ��ȯ
    public int Return_Damage()
    {
        return Mathf.RoundToInt(unitData.damage * (float)damagePercent / 100);
    }
    public float Return_MoveSpeed()
    {
        return unitData.moveSpeed * (float)moveSpeedPercent / 100;
    }
    public float Return_AttackSpeed()
    {
        return unitData.attackSpeed * (float)attackSpeedPercent / 100;
    }
    public float Return_Range()
    {
        return unitData.range * (float)rangePercent / 100;
    }
    public int Return_Weight()
    {
        return Mathf.RoundToInt(unitData.unit_Weight * (float)weightPercent / 100);
    }
    public float Return_Accuracy()
    {
        return unitData.accuracy * (float)accuracyPercent / 100;
    }
    public int Return_Knockback()
    {
        return Mathf.RoundToInt(unitData.knockback * (float)knockbackPercent / 100);
    }

    #endregion
}
