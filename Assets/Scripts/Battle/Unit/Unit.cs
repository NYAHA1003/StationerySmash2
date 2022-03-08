using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;

public abstract class Unit : MonoBehaviour 
{
    public UnitState unitState { get; protected set; }

    [SerializeField]
    protected SpriteRenderer spr;


    protected bool isSettingEnd;

    public int myDamagedId = 0;
    public int damageCount = 0;
    public int myUnitId;
    public int hp { get; protected set; } = 100;
    public int weight { get; protected set; }
    public int maxhp { get; protected set; }
    public bool isInvincibility { get; protected set; }

    public TeamType eTeam;

    public BattleManager battleManager { get; protected set; }
    protected StageData stageData;


    #region �⺻ ����

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="unitData">���� ������</param>
    /// <param name="eTeam">�� ����</param>
    /// <param name="battleManager">��Ʋ�Ŵ���</param>
    /// <param name="id"></param>
    public virtual void Set_UnitData(DataBase unitData, TeamType eTeam, BattleManager battleManager, int id)
    {
        //��, �̸� ����
        this.eTeam = eTeam;
        transform.name = unitData.unitName + this.eTeam;
        switch (this.eTeam)
        {
            case TeamType.Null:
                throw new System.Exception("�� ����");
            case TeamType.MyTeam:
                spr.color = Color.red;
                break;
            case TeamType.EnemyTeam:
                spr.color = Color.blue;
                break;
        }
        
        
        spr.sprite = unitData.sprite;
        this.battleManager = battleManager;
        stageData = battleManager.currentStageData;
        maxhp = unitData.hp;
        hp = unitData.hp;
        weight = unitData.weight;
        this.myUnitId = id;

        isSettingEnd = true;
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

        if (eTeam == TeamType.MyTeam)
        {
            battleManager.unit_MyDatasTemp.Remove(this);
            return;
        }
        battleManager.unit_EnemyDatasTemp.Remove(this);
    }
    #endregion

    #region ������ ������ �ؾ���
    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="atkData">���� ������</param>
    public abstract void Run_Damaged(AtkData atkData);

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
    public void Set_IsInvincibility(bool isboolean)
    {
        isInvincibility = isboolean;
    }

    /// <summary>
    /// ü�� ����
    /// </summary>
    /// <param name="damage">�پ�� ü��</param>
    public virtual void Subtract_HP(int damage)
    {
        hp -= damage;
    }
}
