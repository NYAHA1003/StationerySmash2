using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;



namespace Battle
{
	public class ThrowSelectComponent : BattleComponent
    {
        private UnitComponent _unitCommand = null;
        private ThrowComponent _throwComponent = null;
        public void SetInitialization(ThrowComponent throwComponent, UnitComponent unitComponent)
        {
            _throwComponent = throwComponent;
            _unitCommand = unitComponent;
        }

        /// <summary>
        /// 던질 유닛 선택
        /// </summary>
        /// <param name="pos"></param>
        public void SelectThrowUnit(Vector2 pos)
        {
            int firstNum = 0;
            int lastNum = _unitCommand._playerUnitList.Count - 1;
            int loopnum = 0;
            int count = _unitCommand._playerUnitList.Count;
            List<Unit> list = _unitCommand._playerUnitList;
            float targetPosX = 0;
            Unit throwedUnit = null;
            
            _throwComponent.SetThrowedUnit(null);
            
            if (list.Count == 0)
            {
                return;
            }

            if (pos.x >= list[lastNum].transform.position.x - 0.3f)
            {
                throwedUnit = list[lastNum];
            }
            else if (pos.x <= list[firstNum].transform.position.x)
            {
                throwedUnit = list[firstNum];
            }

            while (throwedUnit == null)
            {
                if (count == 0)
                {
                    throwedUnit = null;
                    _throwComponent.SetThrowedUnit(throwedUnit);
                    return;
                }

                int find = (lastNum + firstNum) / 2;
                targetPosX = list[find].transform.position.x;

                if (pos.x == targetPosX)
                {
                    throwedUnit = list[find];
                    break;
                }

                if (pos.x > targetPosX)
                {
                    firstNum = find;
                }
                else if (pos.x < targetPosX)
                {
                    lastNum = find;
                }

                if (lastNum - firstNum <= 1)
                {
                    throwedUnit = list[lastNum];
                    break;
                }

                loopnum++;
                if (loopnum > 10000)
                {
                    throw new System.Exception("Infinite Loop");
                }
            }

            if (throwedUnit != null)
            {
                if (throwedUnit.UnitData.unitType == UnitType.PencilCase)
                {
                    throwedUnit = null;
                    _throwComponent.SetThrowedUnit(throwedUnit);
                    return;
                }
                Vector2[] points = throwedUnit.CollideData.GetPoint(throwedUnit.transform.position);

                if (CheckPoints(points, pos))
                {
                    throwedUnit = throwedUnit.Pull_Unit();

                    _throwComponent.SetThrowedUnit(throwedUnit);
                }
            }
        }

        /// <summary>
        /// 인포인트가 아웃 포인트 안에 있는지 체크
        /// </summary>
        /// <param name="outPoint"></param>
        /// <param name="inPoint"></param>
        /// <returns></returns>
        private bool CheckPoints(Vector2[] box, Vector2 inPoint)
        {
            if (box[0].x - 0.2f > inPoint.x)
            {
                return false;
            }
            if (box[1].x + 0.2f < inPoint.x)
            {
                return false;
            }
            if (box[2].x - 0.2f > inPoint.x)
            {
                return false;
            }
            if (box[3].x + 0.2f < inPoint.x)
            {
                return false;
            }
            if (box[0].y + 0.15f < inPoint.y)
            {
                return false;
            }
            if (box[1].y + 0.15f < inPoint.y)
            {
                return false;
            }
            if (box[2].y - 0.1f > inPoint.y)
            {
                return false;
            }
            if (box[3].y - 0.1f > inPoint.y)
            {
                return false;
            }
            return true;

        }
    }

}