using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombineChallenge : MonoBehaviour
{
    //Step 1: Jenis Gelombang
    public WaveAnimator waveDisplay;
    public bool jenisGelombang = true;
    public Button btnTransversal;
    public Button btnLongitudinal;
    public GameObject panel1;

    //Step 2: Frekuensi-Periode
    public WaveAnimator targetWave;
    public WaveAnimator playerWave;
    public Slider frekuensiSlider;
    public Slider periodeSlider;
    public TextMeshProUGUI frekuensiText;
    public TextMeshProUGUI periodeText;
    public TextMeshProUGUI kerapatanText;
    public Button btnCheck;
    public GameObject panel2;

    //Final Answer
    public GameObject rewardPanel;
    public GameObject retryPanel;
    public float tolerance = 0.15f;
    public float cepatRambat = 3f;
    [SerializeField] private bool sedangUpdateSendiri = false;
    [SerializeField] private bool step1End = false;
    void Start()
    {
        panel2.SetActive(false);

        btnTransversal.onClick.AddListener(() => CekJenisGelombang(true));
        btnLongitudinal.onClick.AddListener(() => CekJenisGelombang(false));

        frekuensiSlider.value = playerWave.kecepatan;
        periodeSlider.value = 1f / playerWave.kecepatan;
        frekuensiSlider.onValueChanged.AddListener(OnFrekuensi);
        periodeSlider.onValueChanged.AddListener(OnPeriode);
        btnCheck.onClick.AddListener(CheckAnswer);
    }
    void CekJenisGelombang(bool selectedWave)
    {
        bool benar = (selectedWave == jenisGelombang);
        if (benar) 
        {
            step1End = true;
            panel1.SetActive(false);
            panel2.SetActive(true);
            UpdateTeks();
        }
        else
        {
            retryPanel.SetActive(true);
        }
    }
    void OnFrekuensi(float f)
    {
        if (sedangUpdateSendiri) return;

        sedangUpdateSendiri = true;
        periodeSlider.value = 1f / f;
        sedangUpdateSendiri = false;

        playerWave.kecepatan = f;
        playerWave.kerapatan = cepatRambat / f;
        UpdateTeks();
    }
    void OnPeriode(float T)
    {
        if (sedangUpdateSendiri) return;

        sedangUpdateSendiri = true;
        frekuensiSlider.value = 1f / T;
        sedangUpdateSendiri = false;

        playerWave.kecepatan = 1f / T;
        playerWave.kerapatan = cepatRambat * T;
        UpdateTeks();
    }
    void UpdateTeks()
    {
        frekuensiText.text = "f: " + frekuensiSlider.value.ToString("F2") + " Hz";
        periodeText.text = "T: " + periodeSlider.value.ToString("F2") + " s";
        kerapatanText.text = "λ (otomatis): " + playerWave.kerapatan.ToString("F2");
    }
    void CheckAnswer()
    {
        if(!step1End) return;
        float error = Mathf.Abs(playerWave.kerapatan - targetWave.kerapatan) / targetWave.kerapatan;
        if (error < tolerance) 
        {
            rewardPanel.SetActive(true);
        }
        else 
        {
            retryPanel.SetActive(true);
        }

    }
}
