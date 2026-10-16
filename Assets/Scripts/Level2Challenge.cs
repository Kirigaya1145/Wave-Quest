using UnityEngine;
using UnityEngine.UI;

public class Level2Challenge : MonoBehaviour
{
    public WaveAnimator targetWave;
    public WaveAnimator playerWave;

    public Slider amplitudoSlider;
    public Button btnCheck;
    public Button btnClose;
    public GameObject panel;
    public GameObject rewardPanel;
    public GameObject retryPanel;

    public PlayerMovement player;
    public float tolerance = 0.08f;
    private bool solved = false;

    public float nilaiMin = 0.2f;
    public float nilaiMax = 2f;
    private bool afterRandom = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        amplitudoSlider.minValue = nilaiMin;
        amplitudoSlider.maxValue = nilaiMax;
        amplitudoSlider.onValueChanged.AddListener(OnSliderChange);
        btnCheck.onClick.AddListener(CheckAnswer);
        btnClose.gameObject.SetActive(false);
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
            targetWave.amplitudo = randomValue;
            afterRandom = true;
        }
        amplitudoSlider.value = playerWave.amplitudo;
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
        playerWave.amplitudo = value;
    }
    void CheckAnswer()
    {
        if (solved) return;
        float error = Mathf.Abs(playerWave.amplitudo - targetWave.amplitudo) / targetWave.amplitudo;
        if (error < tolerance)
        {
            solved = true;
            rewardPanel.SetActive(true);
            Level2Manager.Instance.TandaSelesai("Amplitudo");
            Invoke(nameof(ClosePanel), 1.5f);
        }
        else 
        {
            retryPanel.SetActive(true);
            Level2Manager.Instance.PenaltiWaktu(10f);
        }
    }
}
