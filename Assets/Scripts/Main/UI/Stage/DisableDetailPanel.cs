using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableDetailPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject DetailPanel;
    private void OnDisable()
    {
        DetailPanel.SetActive(false);
    }
}
