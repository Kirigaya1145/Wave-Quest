using UnityEngine;

public class ExitDoors : MonoBehaviour
{
    public GameObject doorClosed;
    public GameObject doorOpen;
    void Update()
    {
        bool ends = Level2Manager.Instance.AllDone;
        doorClosed.SetActive(!ends);
        doorOpen.SetActive(ends);
    }
}
