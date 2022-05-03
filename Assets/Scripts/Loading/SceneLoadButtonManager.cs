using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Load;
public class SceneLoadButtonManager : MonoBehaviour
{
    [SerializeField]
    private Button[] buttons;
    private LoadingBattleDataSO loadingBattleDataSO;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //private void SetBattleLoadButtons()
    //{

    //    for (int i = 0; i < System.Enum.GetValues(typeof(BattleStageType)).Length; i++)
    //    {
    //        int temp = i;
    //        buttons[temp].onClick.AddListener(() => LoadBattleData(buttons[temp].gameObject.GetComponent<LoadingBattleDataSO>()));
    //    }
    //}
    //private void LoadBattleData(LoadingBattleDataSO loadingBattleDataSO)
    //{
    //    //so데이터를 aidataSO와 PencilCaseDataSO에 넣어줌
    //}
}
