using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Main.Setting;
using DG.Tweening;
using Utill.Tool;

public class MainLoadingManager : LoadingManager
{
    
    protected async override void Start()
    {
        await Sound.AllLoadAssetAsync();
        MainSceneLoad();

        base.Start();
    }
    public static void MainSceneLoad()
    {
        nextScene ="MainSceneRework";
    }


    protected override IEnumerator Random_Tips()
    {
        StartCoroutine(base.Random_Tips());

        string[] text = new string[4];
        text[0] = tip_Text.text;
        text[1] = tip_Text.text + ".";
        text[2] = tip_Text.text + "..";
        text[3] = tip_Text.text + "...";

        int index = 0;
        while(true)
		{
            tip_Text.text = text[index++];
            if(index == 4)
			{
                index = 0;
			}
            yield return new WaitForSeconds(0.2f);
		}
    }
}
