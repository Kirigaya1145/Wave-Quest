using UnityEngine;
using UnityEngine.UI;

public class NotePickup : MonoBehaviour
{
    public PlayerMovement player;
    public GameObject notePanel;
    public TMPro.TMP_Text noteText;
    public Button btnClose;

    [TextArea(4, 10)]
    public string isiNote = "Catatan Riste - Entri #1...";
    
    void Start()
    {
        if (notePanel != null) notePanel.SetActive(false);
        if (btnClose != null) btnClose.onClick.AddListener(CloseNote);
    }
    public void OpenNote()
    {
        noteText.text = isiNote;
        notePanel.SetActive(true);
        if (player != null) player.SetInputLocked(true);
    }
    public void CloseNote()
    {
        notePanel.SetActive(false);
        if (player != null) player.SetInputLocked(false);
    }
}
