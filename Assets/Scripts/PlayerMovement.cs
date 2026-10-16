using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    public bool inputKunci = false;
    private bool pAtas, pBawah, pKiri, pKanan;

    private bool useHorizon = true; // mencegah untuk tidak dua button ditekan
    private float prevX, prevY;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    void Update()
    {
        if (inputKunci) 
        {
            moveInput = Vector2.zero;
            return;
        }

        float xBtn = 0f;
        float yBtn = 0f;
        if (pKiri) xBtn -= 1f;
        if (pKanan) xBtn += 1f;
        if (pBawah) yBtn -= 1f;
        if (pAtas) yBtn += 1f;

        float xKey = Input.GetAxisRaw("Horizontal");
        float yKey = Input.GetAxisRaw("Vertical");

        float x = xKey != 0f ? xKey : xBtn;
        float y = yKey != 0f ? yKey : yBtn;

        if (x != 0f && prevX == 0f) useHorizon = true;
        if (y != 0f && prevY == 0f) useHorizon = false;
        prevX = x; 
        prevY = y;
        if (x != 0f && y != 0f) 
        { 
            if (useHorizon) y = 0f;
            else x = 0f;
        }
        moveInput = new Vector2(x,y);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;    
    }
    public void SetInputLocked(bool locked)
    {
        inputKunci = locked;
        if (locked) 
        { 
            rb.linearVelocity = Vector2.zero;
            pAtas = pBawah = pKiri = pKanan = false;
        }
    }
    public void OnUpPressed() { pAtas = true; }
    public void OnUpReleased() { pAtas = false; }
    public void OnDownPressed() { pBawah = true; }
    public void OnDownReleased() { pBawah = false; }
    public void OnLeftPressed() { pKiri = true; }
    public void OnLeftReleased() { pKiri = false; }
    public void OnRightPressed() { pKanan = true; }
    public void OnRightReleased() { pKanan = false; }
}
