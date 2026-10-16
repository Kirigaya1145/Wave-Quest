using UnityEngine;
using UnityEngine.UI;

public class NPCHint : MonoBehaviour
{
    public Level1Challenge challenge;
    public PlayerMovement playerM;

    public GameObject dialogPanel;
    public TMPro.TMP_Text npcDialog;
    public Button btnP;
    public TMPro.TMP_Text btnPLabel;

    [TextArea] public string dialogIntro = "\"Ya, itu dia catatannya. " +
        "Coba baca instruksinya, lalu cari mesin yang cocok.\"";
    [TextArea]
    public string clipboardTransversal =
        "Catatan Perbaikan - Stasiun Gelombang\n\n" +
        "Mesin butuh gelombang yang arahnya TEGAK LURUS dengan arah rambatnya - " +
        "seperti tali yang digoyang naik-turun sementara gelombangnya jalan ke depan.\n\n" +
        "-> Cari mesin yang cocok, lalu gerakkan sesuai arah getarnya: naik, lalu turun, bergantian.";
    [TextArea]
    public string clipboardLongitudinal =
        "Catatan Perbaikan - Stasiun Gelombang\n\n" +
        "Mesin ini butuh gelombang yang arah getarnya SEJAJAR dengan arah rambatnya - " +
        "seperti pegas yang dimampatkan lalu dilepas berulang.\n\n" +
        "-> Cari mesin yang cocok, lalu tekan-tahan sebentar, lalu lepas, berulang mengikuti ritme mampat-lepas.";
    private int page = 0;
    void Start()
    {
        if (dialogPanel != null) dialogPanel.SetActive(false);
        if (btnP != null) btnP.onClick.AddListener(OnPrimaryButtonPressed);
    }
    public void OpenDialog()
    {
        if (dialogPanel == null || challenge == null) return;
        if (dialogPanel.activeSelf) return;

        page = 0;
        ShowPage();
        dialogPanel.SetActive(true);
        if (playerM != null) playerM.SetInputLocked(true);
    }
    void ShowPage()
    {
        if (page == 0)
        {
            npcDialog.text = dialogIntro;
            if (btnPLabel != null) btnPLabel.text = "Lanjut";
        }
        else
        {
            npcDialog.text = challenge.targetWave ? clipboardTransversal : clipboardLongitudinal;
            if (btnPLabel != null) btnPLabel.text = "Tutup";
        }
    }

    public void CloseDialog()
    {
        dialogPanel.SetActive(false);
        if (playerM != null) playerM.SetInputLocked(false);
    }
    void OnPrimaryButtonPressed()
    {
        if (page == 0)
        {
            page = 1;
            ShowPage();
        }
        else
        {
            CloseDialog();
        }
    }
    public GameObject smallFeedbackBubble;
    public TMPro.TMP_Text smallFeedbackText;
    public float feedbackDuration = 2f;

    public void ShowFeedback(string text)
    {
        if (smallFeedbackBubble == null) return;
        smallFeedbackText.text = text;
        smallFeedbackBubble.SetActive(true);
        smallFeedbackBubble.transform.SetAsLastSibling();
        CancelInvoke(nameof(HideFeedback));
        Invoke(nameof(HideFeedback), feedbackDuration);
    }

    void HideFeedback()
    {
        if (smallFeedbackBubble != null) smallFeedbackBubble.SetActive(false);
    }
}
