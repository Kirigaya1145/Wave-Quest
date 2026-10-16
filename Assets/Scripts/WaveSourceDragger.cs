using UnityEngine;

public class WaveSourceDragger : MonoBehaviour
{
    public WaveAnimator playerWave;
    public float maxRange = 0.5f;
    public float threshold = 0.15f;

    private Vector3 startPose;
    private bool dragging;
    
    void Start()
    {
        startPose = transform.position;
    }
    private void OnMouseDown()
    {
        dragging = true;
        Debug.Log("Handle diklik!");
    }
    private void OnMouseDrag()
    {
        if (!dragging) return;
        Debug.Log("Sedang drag..!");

        Vector3 mouseW = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseW.z = startPose.z;
        Vector2 delta = (Vector2)(mouseW - startPose);
        transform.position = startPose + (Vector3)Vector2.ClampMagnitude(delta, maxRange);
        if(delta.magnitude > threshold)
        {
            bool vertikal = Mathf.Abs(delta.y) > Mathf.Abs(delta.x);
            playerWave.isTransversal = vertikal;
        }
    }
    private void OnMouseUp()
    {
        dragging = false;
        transform.position = startPose;
    }
}
