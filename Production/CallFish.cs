using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallFish : MonoBehaviour {

    public float CallRange = 10f;

	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Collider[] Fishes = Physics.OverlapSphere(transform.position, CallRange, 1 << LayerMask.NameToLayer("Fish"));

            if(Fishes.Length != 0)
            {
                for (int i = 0; i < Fishes.Length; i++)
                {
                    Fishes[i].GetComponent<FishAI>().CUR_STATE = FishAI.Fish_STATE.CALL;
                }
            }
        }
    }
}
