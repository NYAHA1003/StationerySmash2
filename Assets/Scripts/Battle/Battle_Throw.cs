using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
public class Battle_Throw : BattleCommand
{
    private Unit throw_Unit;
    private LineRenderer parabola;
    private Transform arrow;
    private StageData stageData;

    private List<Vector2> lineZeroPos;
    private Vector2 direction;
    private float force;
    private float pullTime;


    public Battle_Throw(BattleManager battleManager, LineRenderer parabola, Transform arrow, StageData stageData) : base(battleManager)
    {
        this.parabola = parabola;
        this.stageData = stageData;
        this.arrow = arrow;
        lineZeroPos = new List<Vector2>(parabola.positionCount);
        for(int i = 0; i < parabola.positionCount; i++)
        {
            lineZeroPos.Add(Vector2.zero);
        }
    }

    public void Pull_Unit(Vector2 pos)
    {
        float targetRange = float.MaxValue;
        for (int i = 0; i < battleManager.unit_MyDatasTemp.Count; i++)
        {
            if (Vector2.Distance(pos, battleManager.unit_MyDatasTemp[i].transform.position) < targetRange)
            {
                throw_Unit = battleManager.unit_MyDatasTemp[i];
                targetRange = Vector2.Distance(pos, throw_Unit.transform.position);
            }
        }

        if (throw_Unit != null)
        {
            if (Vector2.Distance(pos, throw_Unit.transform.position) < 0.1f)
            {
                throw_Unit = throw_Unit.Pull_Unit();
                if(throw_Unit == null)
                {
                    battleManager.battle_Camera.Set_CameraIsMove(false);
                }
                pullTime = 2f;
                return;
            }
            throw_Unit = null;
        }
    }

    public void Draw_Parabola(Vector2 pos)
    {
        UnDraw_Parabola();
        if (throw_Unit != null)
        {
            //�ð��� ������ ���
            pullTime -= Time.deltaTime;
            if (pullTime < 0)
            {
                throw_Unit = null;
                UnDraw_Parabola();
                return;
            }

            //������ �ٸ� �ൿ�� ���ϰ� �Ǹ� ���
            throw_Unit = throw_Unit.Pulling_Unit();
            battleManager.battle_Camera.Set_CameraIsMove(false);

            if (throw_Unit == null)
            {
                UnDraw_Parabola();
                return;
            }

            //����
            direction = (Vector2)throw_Unit.transform.position - pos;
            float dir = Mathf.Atan2(direction.y, direction.x);
            float dirx = Mathf.Atan2(direction.y, -direction.x);
            
            //������ ���⿡ ���� �������� �� ���̰� ��
            if(dir < 0)
            {
                UnDraw_Parabola();
                return;
            }

            //ȭ��ǥ
            arrow.transform.position = throw_Unit.transform.position;
            arrow.transform.eulerAngles = new Vector3(0, 0, dir * Mathf.Rad2Deg);
            
            //�ʱ� ����
            force = Mathf.Clamp(Vector2.Distance(throw_Unit.transform.position, pos), 0, 1) * 4 * (100.0f / throw_Unit.weight);

            //�ְ���
            float height = Utill.Parabola.Caculated_Height(force, dirx);
            //���� ���� �Ÿ�
            float width = Utill.Parabola.Caculated_Width(force, dirx) ;
            //���� ���� �ð�
            float time = Utill.Parabola.Caculated_Time(force, dir, 2);
            
            List<Vector2> linePos = Set_ParabolaPos(parabola.positionCount, width, force, dir, time);

            Set_ParabolaPos(linePos);

            return;
        }
    }

    public void UnDraw_Parabola()
    {
        Set_ParabolaPos(lineZeroPos);
    }

    private void Set_ParabolaPos(List<Vector2> linePos)
    {
        for (int i = 0; i < parabola.positionCount; i++)
        {
            parabola.SetPosition(i, linePos[i]);
        }
    }


    private List<Vector2> Set_ParabolaPos(int count, float width, float force, float dir_rad, float time)
    {
        List<Vector2> results = new List<Vector2>(count);
        float[] objLerps = new float[count];
        float[] timeLerps = new float[count];
        float interbal = 1f / (count - 1 > 0 ? count - 1 : 1);
        float timeInterbal = time / (count - 1 > 0 ? count - 1 : 1);
        for (int i = 0; i < count; i++)
        {
            objLerps[i] = interbal * i;
            timeLerps[i] = timeInterbal * i;
        }

        for(int i = 0; i < count; i ++)
        {
            Vector3 pos = Vector3.Lerp((Vector2)throw_Unit.transform.position, new Vector2(throw_Unit.transform.position.x - width, 0), objLerps[i]);
            pos.y = Utill.Parabola.Caculated_TimeToPos(force, dir_rad, timeLerps[i]);
            
            if(i > 0)
            {
                if(pos.x >= stageData.max_Range || pos.x <= -stageData.max_Range)
                {
                    pos = results[i - 1];
                }
            }

            results.Add(pos);
        }

        return results;
    }

    public void Throw_Unit()
    {
        if (throw_Unit != null)
        {
            throw_Unit.Throw_Unit(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            throw_Unit = null;
            UnDraw_Parabola();
        }
    }
}
