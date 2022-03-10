using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StarMove : MonoBehaviour
{
    void Update()
    {
        Star_Move();
    }
    
    private float angle = 0, starX = 0, starY = 0, speed = 5f, width = 0.3f, height = 0.2f;
    
    public void Star_Move()
    {
        angle += Time.deltaTime * speed;
        starX = Mathf.Cos(angle) * width + 2;
        starY = Mathf.Sin(angle) * height + 3;
        transform.position = new Vector3(starX,starY,0);
    }
}