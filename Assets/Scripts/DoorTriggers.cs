using UnityEngine;

public class DoorTriggers : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (Level2Manager.Instance.AllDone)
        {
            Level2Manager.Instance.OpenEndPanel();
        }
    }
}
