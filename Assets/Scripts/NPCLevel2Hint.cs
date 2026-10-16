using UnityEngine;
using UnityEngine.UI;

public class NPCLevel2Hint : MonoBehaviour
{
    public PlayerMovement player;

    public GameObject dialogPanel;
    public TMPro.TMP_Text dialogText;
    public Button btnClose;

    public string dialog = "Ruangan ini dulu dipakai untuk riset besaran gelombang. Tetapi ingatan saya hilang mengenai detail lanjutan.\n" +
        "Coba cari catatan yang tertinggal di tiap stasiun ruangan ini. \nMungkin itu bisa membantumu.";
    void Start()
    {
        if (dialogPanel != null) dialogPanel.SetActive(false);
        if (btnClose != null) btnClose.onClick.AddListener(CloseDialog);
    }
    public void OpenDialog()
    {
        dialogText.text = dialog;
        dialogPanel.SetActive(true);
        if (player != null) player.SetInputLocked(true);
    }
    public void CloseDialog() 
    {
        dialogPanel.SetActive(false);
        if (player != null) player.SetInputLocked(false);
    }
}
