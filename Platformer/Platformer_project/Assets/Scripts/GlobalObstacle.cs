using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalObstacle : MonoBehaviour {

    public Transform obstacle;

    void Update () {

        Shader.SetGlobalVector("_Obstacle", obstacle.position);
    }
}