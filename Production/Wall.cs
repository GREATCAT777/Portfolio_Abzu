using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public GameManager.Stage ShiftStage;
    public GameObject wall;

    private void OnTriggerEnter(Collider other)
    {
        DoingDoor(other);
    }

    public void DoingDoor(Collider other)
    {
        if (GameManager.Instance.player.CompareTag(other.tag))
        {
            GameManager.Instance.ChangeStage(ShiftStage);
            wall.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
