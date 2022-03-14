using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill
{
    public enum eState  // ���� �� �ִ� ���� ����
    {
        IDLE, MOVE, ATTACK, WAIT, DAMAGED, DIE, PULL, THROW, NONE,
    };

    public enum eEvent  // �̺�Ʈ ����
    {
        ENTER, UPDATE, EXIT, NOTHING
    };
}
