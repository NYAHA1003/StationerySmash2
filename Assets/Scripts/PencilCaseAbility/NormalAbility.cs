using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NormalAbility : AbstractPencilCaseAbility
{
    /// <summary>
    /// ī�带 ��� �����Ѵ�
    /// </summary>
    public override void RunPencilCaseAbility()
    {
        _battleManager.CommandCard.ClearCards();
    }
}
