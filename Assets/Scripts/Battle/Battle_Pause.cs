using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Battle_Pause : BattleCommand
{
    private bool isActive = false;
    private RectTransform pauseUI;
    private Canvas pauseCanvas;
    public Battle_Pause(BattleManager battleManager, RectTransform pauseUI, Canvas pauseCanvas) : base(battleManager)
    {
        this.pauseUI = pauseUI;
        this.pauseCanvas = pauseCanvas;
    }
    
    /// <summary>
    /// 일시정지 창을 껐다 키는 함수
    /// </summary>
    public void Set_Pause()
    {
        isActive = !isActive;
        pauseCanvas.gameObject.SetActive(isActive);
        if (isActive)
        {
            Time.timeScale = 0;
            Set_Animation();
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void Set_Animation()
    {
        pauseUI.DOKill();
        pauseUI.anchoredPosition = new Vector2(Screen.width, 0);
        pauseUI.DOAnchorPosX(0, 1).SetUpdate(true);
    }


}
