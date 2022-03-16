using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Battle_WinLose : BattleCommand
{
    private Canvas winLoseCanvas;
    private RectTransform winPanel;
    private RectTransform losePanel;

    private RectTransform winText;
    private RectTransform loseText;
    public Battle_WinLose(BattleManager battleManager, Canvas winLoseCanvas, RectTransform winPanel, RectTransform losePanel) : base(battleManager)
    {
        this.winLoseCanvas = winLoseCanvas;
        this.winPanel = winPanel;
        this.losePanel = losePanel;

        winText = winPanel.GetChild(0).GetComponent<RectTransform>();
        loseText = losePanel.GetChild(0).GetComponent<RectTransform>();
    }

    public void Set_WinLosePanel(bool isWin)
    {
        winLoseCanvas.gameObject.SetActive(true);

        winPanel.gameObject.SetActive(isWin);
        losePanel.gameObject.SetActive(!isWin);
        if (isWin)
        {
            winPanel.sizeDelta = new Vector2(3, 3);
            winPanel.DOScale(1, 0.3f).SetEase(Ease.OutExpo).
                OnComplete(() =>
                {
                    winText.DOScale(1.5f, 0.3f).SetLoops(-1, LoopType.Yoyo);
                });
            return;
        }
        losePanel.sizeDelta = new Vector2(3, 3);
        losePanel.DOScale(1, 0.3f).SetEase(Ease.OutExpo).
                OnComplete(() =>
                {
                    loseText.DOScale(1.5f, 0.3f).SetLoops(-1, LoopType.Yoyo);
                });

    }
}
