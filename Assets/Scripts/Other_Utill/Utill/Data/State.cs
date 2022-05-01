using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill.Data
{
    public enum eState  // ���� �� �ִ� ���� ����
    {
        IDLE, MOVE, ATTACK, WAIT, DAMAGED, DIE, THROW, NONE,
    };

    public enum eEvent  // �̺�Ʈ ����
    {
        ENTER, UPDATE, EXIT, NOTHING
    };
}
