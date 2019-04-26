using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUtility : UnityEngine.Object {

	public static Quaternion MakeQuaternion(Vector3 standard_vec, Vector3 target_vec)
    {
        float angle = Vector3.Angle(new Vector3(0.0f, 1.0f, 0.0f), target_vec - standard_vec);
        float x = target_vec.x - standard_vec.x;
        angle = 0.0f < x ? 360.0f - angle : angle;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public static Vector3 RoundVector3(Vector3 InVector)
    {
        Vector3 new_vec3 = new Vector3();
        for(int i=0; i<3; ++i)
            new_vec3[i] = Mathf.Round(InVector[i]);
        return new_vec3;
    }

    public static Vector3 CeilVector3(Vector3 InVector)
    {
        Vector3 new_vec3 = new Vector3();
        for (int i = 0; i < 3; ++i)
            new_vec3[i] = Mathf.Ceil(InVector[i]);
        return new_vec3;
    }
}
