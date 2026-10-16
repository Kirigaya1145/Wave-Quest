using UnityEngine;
using UnityEngine.UI;

public enum TypeMachine { Tali, Pegas}; //Tali = transversal, Pegas = longitudinal

public class MachinePanelController : MonoBehaviour
{
    public Level1Challenge challenge;
    public PlayerMovement player;
    public NPCHint npcHint;

    public TypeMachine machineType;

    public GameObject panel;
    public Slider progressBar;
    public Button closeBtn;

    public Button upBtn;
    public Button downBtn;
    public Button pushBtn;
    public WaveAnimator waveVisual;
    public TMPro.TMP_Text infoText; //info penjelasan di panel

    public float addProgress = 0.15f;
    public float holdMin = 0.2f;
    public float holdMax = 1.2f;

    public float minAmplitudo = 0.15f; //gelombang kecil saat progres 0
    public float maxAmplitudo = 1f; //gelombang besar saat progres 1
    public float maxAmplitudoPegas = 0.35f;

    private float progress = 0f;
    private bool firstInput = false;
    private bool lastInput;
    private float holdTimer = -1f;
    private bool solved = false;

    void Start()
    {
        panel.SetActive(false);
        LockProgressBar();
    }
    void LockProgressBar()
    {
        if (progressBar == null) return;
        progressBar.interactable = false;
        progressBar.transition = Selectable.Transition.None;
        progressBar.navigation = new Navigation { mode = Navigation.Mode.None };
        foreach (var g in progressBar.GetComponentsInChildren<Graphic>(true))
            g.raycastTarget = false;
    }
    public void OpenPanel()
    {
        if (panel.activeSelf) return;

        panel.SetActive(true);
        player.SetInputLocked(true);

        closeBtn.onClick.RemoveAllListeners();
        upBtn.onClick.RemoveAllListeners();
        downBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.AddListener(ClosePanel);
        upBtn.onClick.AddListener(() => InputTali(true));
        downBtn.onClick.AddListener(() => InputTali(false));

        progress = 0f;
        firstInput = false;
        holdTimer = -1f;
        solved = false;

        bool thisRope = machineType == TypeMachine.Tali;
        upBtn.gameObject.SetActive(thisRope);
        downBtn.gameObject.SetActive(thisRope);
        pushBtn.gameObject.SetActive(!thisRope);

        if (infoText != null) 
        {
            infoText.text = thisRope
                ? "Tekan naik dan turun bergantian.\nPartikel bergetar NAIK-TURUN, tegak lurus arah rambat gelombang."
                : "Tekan-tahan sebentar, lalu lepas.\nPartikel bergetar MAJU-MUNDUR, sejajar arah rambat gelombang.";
        }

        if (waveVisual != null)
        {
            waveVisual.gameObject.SetActive(true); // BARU: nyalakan saat panel dibuka
            waveVisual.isTransversal = thisRope;
        }
        UpdateViewProgress();
    }
    public void ClosePanel()
    {
        CancelInvoke(nameof(ClosePanel));
        panel.SetActive(false);
        player.SetInputLocked(false);
        holdTimer = -1f;
        if (waveVisual != null) waveVisual.gameObject.SetActive(false); // BARU: matikan saat ditutup
    }
    private void InputTali(bool upPress)
    {
        if (solved) return;
        if (!MesinBenar())
        {
            ShowWrongMachine();
            return;
        }
        if (!firstInput)
        {
            firstInput = true;
            lastInput = upPress;
            AddProgress();
            return;
        }
        if (upPress != lastInput) 
        {
            lastInput = upPress;
            AddProgress();
        }
    }
    private bool MesinBenar()
    {
        return (machineType == TypeMachine.Tali) == challenge.targetWave; ;
    }
    public void StartHold()
    {
        if (solved) return;
        if (!MesinBenar()) 
        {
            ShowWrongMachine();
            return ;
        }
        holdTimer = Time.time;
    }
    public void ReleaseHold()
    {
        if (solved || holdTimer < 0f) return;
        float lamaTahan = Time.time - holdTimer;
        holdTimer = -1f;
        if (lamaTahan < holdMin)
            Hint("Tahan sedikit lebih lama,\nlalu lepas.");
        else if (lamaTahan > holdMax)
            Hint("Terlalu lama.\nLepas lebih cepat.");
        else
            AddProgress();
    }
    private void AddProgress()
    {
        progress = Mathf.Clamp01(progress + addProgress);
        UpdateViewProgress();
        if (progress >= 1f && !solved) 
        {
            solved = true;
            challenge.OnMachineSolved(); 
            Invoke(nameof(ClosePanel), 1.5f);
        }
    }
    private void UpdateViewProgress()
    {
        if (progressBar != null) progressBar.value = progress;
        if (waveVisual != null) waveVisual.amplitudo = Mathf.Lerp(minAmplitudo, maxAmplitudo, progress);
    }
    private void ShowWrongMachine()
    {
        Hint("Itu bukan yang dimaksud tadi.\n Try Again!");
    }
    private void Hint(string msg)
    {
        if (npcHint != null) npcHint.ShowFeedback(msg);
    }
}
