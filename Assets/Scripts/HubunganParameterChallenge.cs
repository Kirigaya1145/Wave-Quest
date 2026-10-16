using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HubunganParameterChallenge : MonoBehaviour
{
    public WaveAnimator targetWave;
    public WaveAnimator playerWave;

    public Slider frekuensiSlider;
    public Slider periodeSlider;
    public TextMeshProUGUI frekuensiText;
    public TextMeshProUGUI periodeText;
    public TextMeshProUGUI kerapatanText;

    public Button btnCheck;
    public GameObject rewardPanel;
    public GameObject retryPanel;

    public float tolerance = 0.15f;
    public float cepatRambat = 3f;

    public GameObject panel;
    public PlayerMovement player;

    [SerializeField] private bool sedangUpdateSendiri = false;
    void Start()
    {
        playerWave.panjangGelombang = cepatRambat / playerWave.kecepatan;

        frekuensiSlider.value = playerWave.kecepatan;
        periodeSlider.value = 1f / playerWave.kecepatan;
        UpdateTeks();

        frekuensiSlider.onValueChanged.AddListener(OnFrekuensi);
        periodeSlider.onValueChanged.AddListener(OnPeriode);
        btnCheck.onClick.AddListener(CheckAnswer);
        if (panel != null) panel.SetActive(false);
           
    }
    void OnFrekuensi(float f)
    {
        if (sedangUpdateSendiri) return;

        sedangUpdateSendiri = true;
        periodeSlider.value = 1f / f;
        sedangUpdateSendiri = false;

        playerWave.kecepatan = f;
        playerWave.panjangGelombang = cepatRambat / f;
        UpdateTeks();
    }
    void OnPeriode(float T)
    {
        if (sedangUpdateSendiri) return;

        sedangUpdateSendiri = true;
        frekuensiSlider.value = 1f / T;
        sedangUpdateSendiri = false;

        playerWave.kecepatan = 1f / T;
        playerWave.panjangGelombang = cepatRambat * T;
        UpdateTeks();
    }
    void UpdateTeks()
    {
        frekuensiText.text = "f: " + frekuensiSlider.value.ToString("F2") + " Hz";
        periodeText.text = "T: " + periodeSlider.value.ToString("F2") + " s";
        kerapatanText.text = "λ (otomatis): " + playerWave.panjangGelombang.ToString("F2");
    }
    public void OpenPanel()
    {
        panel.SetActive(true);
        player.SetInputLocked(true);
        rewardPanel.SetActive(false);
        retryPanel.SetActive(false);
    }

    public void ClosePanel()
    {
        CancelInvoke(nameof(ClosePanel));
        panel.SetActive(false);
        player.SetInputLocked(false);
        rewardPanel.SetActive(false);
        retryPanel.SetActive(false);
    }
    void CheckAnswer()
    {
        float error = Mathf.Abs(playerWave.panjangGelombang - targetWave.panjangGelombang) / targetWave.panjangGelombang;
        if (error < tolerance)
        {
            rewardPanel.SetActive(true);
            Invoke(nameof(ClosePanel), 1.5f);
        }
        else
        {
            retryPanel.SetActive(true);
        }

    }
}
