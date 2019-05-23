using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Manager : MonoBehaviour
{
    Animator m_Animator;

    void Start() {
        m_Animator = gameObject.GetComponent<Animator>();
    }

    private void OnAnimatorIK(){
        if(m_Animator) {
			RaycastHit hit;
			Ray lookRay = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0.5f));
			if(Physics.Raycast(lookRay, out hit, Mathf.Infinity)) {
				
				m_Animator.SetLookAtWeight(1,1,1);
                m_Animator.SetLookAtPosition(hit.point);
			} else {          
            	m_Animator.SetLookAtWeight(0);
        	}
        }
    }
}
