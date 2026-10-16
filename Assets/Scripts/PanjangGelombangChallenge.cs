using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;

public class PanjangGelombangChallenge : MonoBehaviour
{
    public WaveAnimator targetWave;
    public WaveAnimator playerWave;
    public Slider panjangSlider;

    public TextMeshProUGUI panjangText;
    public Button btnCheck;
    public Button btnClosePanel;
    public GameObject panel;
    public GameObject rewardPanel;
    public GameObject retryPanel;
    public PlayerMovement player;

    public float tolerance = 0.10f;
    private bool solved = false;
    public float nilaiMin = 0.3f;
    public float nilaiMax = 1.2f;
    private bool afterRandom = false;

    void Start()
    {
        panjangSlider.minValue = nilaiMin;
        panjangSlider.maxValue = nilaiMax;
        panjangSlider.value = playerWave.kerapatan;
        panjangSlider.onValueChanged.AddListener(OnSliderChange);
        btnCheck.onClick.AddListener(CheckAnswer);
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
            targetWave.kerapatan = randomValue;
            afterRandom = true;
        }
        panjangSlider.value = playerWave.kerapatan;
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
        playerWave.kerapatan = value;
        panjangText.text = value.ToString("F1");
    }
    void CheckAnswer()
    {
        if (solved) return;
        float error = Mathf.Abs(playerWave.kerapatan - targetWave.kerapatan) / targetWave.kerapatan;
        if (error < tolerance)
        {
            solved = true;
            rewardPanel.SetActive(true);
            Level2Manager.Instance.TandaSelesai("Panjang");
            Invoke(nameof(ClosePanel), 1.5f);
        }
        else
        {
            retryPanel.SetActive(true);
            Level2Manager.Instance.PenaltiWaktu(10f);
        }
    }
}
