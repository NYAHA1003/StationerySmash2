using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingUnit : MonoBehaviour
{
    int plus;
    float rot;
    [SerializeField] private float rotateMax;
    [SerializeField] private float rotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rot = transform.rotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        float rotation = rot;
        rotation = rotateMax * Mathf.Sin(Time.time * rotateSpeed * Time.deltaTime);
        plus*=-1;
        Vector3 vec = new Vector3(0, 0, rotation);
        transform.Rotate(vec);
    }
}
