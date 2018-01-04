using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GlobalObstacle : MonoBehaviour {

    public Transform[] obstacles;

    void Update () {

        Vector4[] positions = new Vector4[obstacles.Length];
        for (int i = 0; i < obstacles.Length; i++) {
            positions[i] = obstacles[i].position;
        }

        Shader.SetGlobalInt("_ObstacleLength", obstacles.Length);
        Shader.SetGlobalVectorArray("_Obstacles", positions);
    }
}