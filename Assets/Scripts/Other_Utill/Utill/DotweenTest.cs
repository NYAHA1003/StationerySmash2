using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DotweenTest : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            transform.DOJump(new Vector3(2, 0, 0), 1, 1, 1);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            transform.DOKill();
        }
    }
}
