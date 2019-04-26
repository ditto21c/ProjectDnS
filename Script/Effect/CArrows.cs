using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CArrows : CBaseEffect
{
    void Start ()
    {
        float each_angle = max_angle / (max_count - 1);
        float half_angle = max_angle / 2.0f;

        Vector3 center_vec = end_pos - gameObject.transform.position;
        Vector3 left_vec = Quaternion.AngleAxis(half_angle, Vector3.back) * center_vec;

        for (int i = 0; i < max_count; ++i)
        {
            Vector3 rotated_end_pos = Quaternion.AngleAxis(each_angle * i, Vector3.forward) * left_vec;

            Quaternion newRotation = CUtility.MakeQuaternion(gameObject.transform.position, rotated_end_pos + gameObject.transform.position);
            // 이미지를 회전 시켜서 위로 바라 보게 만든다.
            Vector3 eulerAngles = newRotation.eulerAngles;
            eulerAngles.z += 90.0f;
            newRotation = Quaternion.Euler(eulerAngles);

            GameObject LoadedObj = Instantiate<GameObject>(CResourceMgr.LoadEffect("Arrow"), gameObject.transform.position, newRotation);
            CArrow Arrow = LoadedObj.GetComponent<CArrow>();
            Arrow.Speed = speed;
            Arrow.Owner = Owner;
            Arrow.EndPos = rotated_end_pos + gameObject.transform.position;
            Arrow.HitAniType = HitAniType;
        }

        Destroy(gameObject);
    }

    public Vector3 end_pos { get; set; }
    public float speed;
    public int max_count;
    public float max_angle;
    public EHitAniType HitAniType = EHitAniType.Type1;

}
