using UnityEngine;
using UnityEngine.Events;
public class InteractZone : MonoBehaviour
{
    public string promptText = "Tekan E untuk interaksi";
    public GameObject promptUI;
    private TMPro.TMP_Text promptLabel;

    public UnityEvent onInteract;
    private bool playerInside = false;

    void Awake()
    {
        if (promptUI != null)
        {
            promptLabel = promptUI.GetComponentInChildren<TMPro.TMP_Text>(true);
            promptUI.SetActive(false);
        }    
    }
    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            if (promptUI != null) promptUI.SetActive(false);
            onInteract.Invoke();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        playerInside = true;
        if (promptUI != null)
        {
            promptUI.SetActive(true);
            if (promptLabel != null) promptLabel.text = promptText;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = false;
        if (promptUI != null) promptUI.SetActive(false);
    }
}
