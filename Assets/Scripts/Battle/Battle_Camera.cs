using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Battle_Camera : BattleCommand
{
    private Camera camera;
    
    private Vector3 click_Pos;
    private Vector3 cur_Pos;
    private Vector3 mouse_Pos;

    private bool isCameraMove = false;

    public float perspectiveZoomSpeed = 0.5f;       // perspective mode.
    public float orthoZoomSpeed = 0.5f;        //  orthographic mode.


    public Battle_Camera(BattleManager battleManager, Camera camera) : base(battleManager)
    {
        this.camera = camera;
    }

    /// <summary>
    /// 카메라가 움직일 수 있는 상태인지
    /// </summary>
    /// <param name="isCameraMove">True면 움직일 수 있음</param>
    public void Set_CameraIsMove(bool isCameraMove)
    {
        this.isCameraMove = isCameraMove;
    }

    /// <summary>
    /// 카메라 크기 조정
    /// </summary>
    public void Update_CameraScale()
    {
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

        //아래는 모바일로 테스트

        // 손가락으로 줌인/아웃의 경우 무조건 2손가락이 터치가 되어야 하기 때문에 Count = 2일 경우만 동작
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0); //첫번째 손가락 좌표
            Touch touchOne = Input.GetTouch(1); //두번째 손가락 좌표

            // deltaposition은 deltatime과 동일하게 delta만큼 시간동안 움직인 거리를 말한다.

            // 현재 position에서 이전 delta값을 빼주면 움직이기 전의 손가락 위치가 된다.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // 현재와 과거값의 움직임의 크기를 구한다.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // 두 값의 차이는 즉 확대/축소할때 얼만큼 많이 확대/축소가 진행되어야 하는지를 결정한다.
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
    /// 카메라 위치 조정
    /// </summary>
    public void Update_CameraPos()
    {
        //if(Input.touchCount != 1)
        //{
        //    return;
        //}

        //카드를 클릭한 상태라면
        if(battleManager.battle_Card.isCardDown)
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
}
