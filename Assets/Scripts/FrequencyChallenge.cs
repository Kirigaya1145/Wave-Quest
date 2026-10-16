using TreeEditor;
using UnityEngine;
using UnityEngine.UI;

public class FrequencyChallenge : MonoBehaviour
{
    public WaveAnimator targetWave;
    public WaveAnimator playerWave;

    public Slider frekuensiSlider;
    public Button btnCheck;
    public Button btnClosePanel;
    public TMPro.TextMeshProUGUI textKecepatan;
    public GameObject panel;
    public GameObject rewardPanel;
    public GameObject retryPanel;
    public PlayerMovement player;

    public float tolerance = 0.07f;
    private bool solved = false;

    public float nilaiMin = 0.4f;
    public float nilaiMax = 4f;
    private bool afterRandom = false;
    void Start()
    {
        frekuensiSlider.minValue = nilaiMin;
        frekuensiSlider .maxValue = nilaiMax;
        frekuensiSlider.value = playerWave.kecepatan;
        frekuensiSlider.onValueChanged.AddListener(OnSliderChange);
        btnCheck.onClick.AddListener(CheckAnswer);
        btnClosePanel.gameObject.SetActive(false);
        panel.SetActive(false);
    }
    public void OpenPanel()
    {
        panel.SetActive(true);
        player.SetInputLocked(true);
        solved = false;
        if (!afterRandom)
        {
            float randomValue = Random.Range(nilaiMin, nilaiMax);
            randomValue = Mathf.Round(randomValue * 10f) / 10f;
            targetWave.kecepatan = randomValue;
            afterRandom = true;
        }
        frekuensiSlider.value = playerWave.kecepatan;
    }

    public void ClosePanel()
    {
        CancelInvoke(nameof(ClosePanel));
        panel.SetActive(false);
        player.SetInputLocked(false);
        rewardPanel.SetActive(false);
        retryPanel.SetActive(false);
    }
    void OnSliderChange(float value)
    {
        playerWave.kecepatan = value;
        if (textKecepatan != null)  textKecepatan.text = value.ToString("F1");
    }
    void CheckAnswer()
    {
        if (solved) return;
        float error = Mathf.Abs(playerWave.kecepatan - targetWave.kecepatan) / targetWave.kecepatan;
        if (error < tolerance)
        {
            solved = true;
            rewardPanel.SetActive(true);
            Level2Manager.Instance.TandaSelesai("Frekuensi");
            Invoke(nameof(ClosePanel), 1.5f);
        }
        else
        {
            retryPanel.SetActive(true) ;
            Level2Manager.Instance.PenaltiWaktu(10f);
        }
    }
}
