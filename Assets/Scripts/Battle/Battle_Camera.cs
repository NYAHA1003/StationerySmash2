using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;
public class Battle_Camera : BattleCommand
{
    private Camera camera;
    
    private Vector3 click_Pos;
    private Vector3 cur_Pos;
    private Vector3 mouse_Pos;

    private bool isCameraMove = false;
    private bool isEffect = false;

    public float perspectiveZoomSpeed = 0.5f;       // perspective mode.
    public float orthoZoomSpeed = 0.5f;        //  orthographic mode.


    public Battle_Camera(BattleManager battleManager, Camera camera) : base(battleManager)
    {
        this.camera = camera;
    }

    /// <summary>
    /// ī�޶� ������ �� �ִ� ��������
    /// </summary>
    /// <param name="isCameraMove">True�� ������ �� ����</param>
    public void Set_CameraIsMove(bool isCameraMove)
    {
        this.isCameraMove = isCameraMove;
    }

    /// <summary>
    /// ī�޶� ũ�� ����
    /// </summary>
    public void Update_CameraScale()
    {
        if (isEffect)
            return;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (battleManager.currentStageData.max_Range - 1 < camera.orthographicSize)
                return;
            camera.orthographicSize += Time.deltaTime * 10;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (1 > camera.orthographicSize)
                return;
            camera.orthographicSize -= Time.deltaTime * 10;
        }

        //�Ʒ��� ����Ϸ� �׽�Ʈ

        // �հ������� ����/�ƿ��� ��� ������ 2�հ����� ��ġ�� �Ǿ�� �ϱ� ������ Count = 2�� ��츸 ����
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0); //ù��° �հ��� ��ǥ
            Touch touchOne = Input.GetTouch(1); //�ι�° �հ��� ��ǥ

            // deltaposition�� deltatime�� �����ϰ� delta��ŭ �ð����� ������ �Ÿ��� ���Ѵ�.

            // ���� position���� ���� delta���� ���ָ� �����̱� ���� �հ��� ��ġ�� �ȴ�.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // ����� ���Ű��� �������� ũ�⸦ ���Ѵ�.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // �� ���� ���̴� �� Ȯ��/����Ҷ� ��ŭ ���� Ȯ��/��Ұ� ����Ǿ�� �ϴ����� �����Ѵ�.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // If the camera is orthographic...
            if (camera.orthographic)
            {
                // ... change the orthographic size based on the change in distance between the touches.
                camera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                // Make sure the orthographic size never drops below zero.
                camera.orthographicSize = Mathf.Max(camera.orthographicSize, 0.1f);
            }
            else
            {
                // Otherwise change the field of view based on the change in distance between the touches.
                camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                // Clamp the field of view to make sure it's between 0 and 180.
                camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 0.1f, 179.9f);
            }
        }
    }

    /// <summary>
    /// ī�޶� ��ġ ����
    /// </summary>
    public void Update_CameraPos()
    {
        if (isEffect)
            return;
        //if(Input.touchCount != 1)
        //{
        //    return;
        //}

        //ī�带 Ŭ���� ���¶��
        if (battleManager.battle_Card.isCardDown)
        {
            isCameraMove = false;
            return;
        }

        mouse_Pos = Input.mousePosition * 0.005f;

        if (Input.GetMouseButtonDown(0) && Input.mousePosition.y > camera.pixelHeight * 0.3f)
        {
            click_Pos = mouse_Pos;
            cur_Pos = camera.transform.position;
            isCameraMove = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isCameraMove = false;
        }

        if (isCameraMove)
        {
            camera.transform.position = new Vector3(cur_Pos.x + (click_Pos.x + -mouse_Pos.x), 0, -10);
            if (battleManager.currentStageData.max_Range + 1f < camera.transform.position.x)
            {
                camera.transform.DOMoveX(battleManager.currentStageData.max_Range, 0.1f);
            }
            if (-battleManager.currentStageData.max_Range - 1f > camera.transform.position.x)
            {
                camera.transform.DOMoveX(-battleManager.currentStageData.max_Range, 0.1f);
            }
        }
    }

    /// <summary>
    /// �¸� ī�޶� ����Ʈ
    /// </summary>
    public void Win_CamEffect(Vector2 pos, bool isWin)
    {
        if (isEffect)
            return;
        isEffect = true;
        float time = Vector2.Distance(camera.transform.position, pos) / 5;
        camera.transform.DOMove(new Vector3(pos.x,pos.y,-10), time);

        DOTween.To(() => camera.orthographicSize, x => camera.orthographicSize = x, 1f, time);
        camera.transform.DORotate(new Vector3(0,0,Random.Range(-30f,-10f)), 0.07f).SetDelay(0.2f).OnComplete(() =>
        {
            DOTween.To(() => camera.orthographicSize, x => camera.orthographicSize = x, 0.8f, 0.05f).SetDelay(0.2f);
            camera.transform.DORotate(new Vector3(0, 0, Random.Range(10f, 30f)), 0.07f).SetDelay(0.2f).OnComplete(() =>
            {
                DOTween.To(() => camera.orthographicSize, x => camera.orthographicSize = x, 0.6f, 0.05f).SetDelay(0.2f); ;
                camera.transform.DORotate(new Vector3(0, 0, Random.Range(-30f, -10f)), 0.07f).SetDelay(0.2f).OnComplete(() =>
                {
                    battleManager.battle_WinLose.Set_WinLosePanel(isWin);
                });
            });
        });
    }
}
