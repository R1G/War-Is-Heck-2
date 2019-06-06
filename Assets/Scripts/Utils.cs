using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    private static float maxViewRange = 15f;
    public static bool IsInLoS(GameObject go1, GameObject go2) {
        //Check In Range
        if(GetDistanceFrom(go1, go2)>maxViewRange) {
            return false;
        }
        //Check Field of View
        if(Vector3.Angle(GetDirectionTo(go1, go2), go1.transform.forward) > 45.0) {
            return false;
        }
        //Check Obstacles
        RaycastHit hit;
        if(Physics.Raycast(go1.transform.position, GetDirectionTo(go1, go2), out hit, GetDistanceFrom(go1, go2)-2f)) {
            return false;
        }
        return true;
    }

    private static float GetDistanceFrom(GameObject go1, GameObject go2) {
        return go2 ? Vector3.Distance(go1.transform.position, go2.transform.position) : Mathf.Infinity;
    }

    private static Vector3 GetDirectionTo(GameObject go1, GameObject go2) {
        return go2.transform.position-go1.transform.position;
    }


}
