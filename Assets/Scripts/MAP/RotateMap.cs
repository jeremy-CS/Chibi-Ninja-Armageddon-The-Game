using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMap : MonoBehaviour
{
    [SerializeField] GameObject map;
    [SerializeField] Vector3 rotationSpeed;
    private void Start()
    {
        if (Time.timeScale == 0) Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        map.transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
