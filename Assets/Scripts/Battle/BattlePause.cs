using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BattlePause : BattleCommand
{
    private bool _isActive = false;
    private RectTransform _pauseUI;
    private Canvas _pauseCanvas;
    public BattlePause(BattleManager battleManager, RectTransform pauseUI, Canvas pauseCanvas) : base(battleManager)
    {
        this._pauseUI = pauseUI;
        this._pauseCanvas = pauseCanvas;
    }
    
    /// <summary>
    /// 일시정지 창을 껐다 키는 함수
    /// </summary>
    public void SetPause()
    {
        _isActive = !_isActive;
        _pauseCanvas.gameObject.SetActive(_isActive);
        if (_isActive)
        {
            Time.timeScale = 0;
            SetAnimation();
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void SetAnimation()
    {
        _pauseUI.DOKill();
        _pauseUI.anchoredPosition = new Vector2(Screen.width, 0);
        _pauseUI.DOAnchorPosX(0, 1).SetUpdate(true);
    }


}
