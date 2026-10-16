using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Manager : MonoBehaviour
{
    public static Level2Manager Instance;

    //Waktu Timer 3 menit
    public float totalTimer = 180f;
    private float sisaWaktu;
    private bool timerUp = false;
    public GameObject panelTimeUp;

    //Progression Sub Level
    public bool amplitudoDone = false;
    public bool frekuensiDone = false;
    public bool panjangDone = false;
    public bool AllDone => amplitudoDone && frekuensiDone && panjangDone;

    public TMPro.TMP_Text timerText;
    public GameObject panelSelesai;
    public PlayerMovement player;

    public string sceneLevel3 = "";
    public string sceneFail = "";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        sisaWaktu = totalTimer;
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += CariTimerText;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= CariTimerText;    
    }
    void CariTimerText(Scene scene, LoadSceneMode mode)
    {
        GameObject obj = GameObject.Find("Timer Text");
        if (obj != null)
        {
            timerText = obj.GetComponent<TMPro.TMP_Text>();
        } 
        else
        {
            timerText = null;
        }
        GameObject panelObj = GameObject.Find("PanelLevel2Selesai");
        if (panelObj != null) 
        {
            panelSelesai = panelObj;
            panelSelesai.SetActive(false);
            if (AllDone) panelSelesai.SetActive(true);
        }
        else 
        {
            panelSelesai = null;
        }
        GameObject panelTimeObj = GameObject.Find("Panel Waktu Habis");
        if (panelTimeObj != null)
        {
            panelTimeUp = panelTimeObj;
            panelTimeUp.SetActive(false);
            if (timerUp && !AllDone) panelTimeUp.SetActive(true);
        }
        else
        {
            panelTimeUp = null;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.GetComponent<PlayerMovement>();
        else player = null;
        if (timerUp && !AllDone && player != null) player.SetInputLocked(true);

    }
    void Update()
    {
        if (timerUp) return;
        sisaWaktu -= Time.deltaTime;
        if (timerText != null)
        {
            int menit = Mathf.Max(0, Mathf.FloorToInt(sisaWaktu / 60f));
            int detik = Mathf.Max(0, Mathf.FloorToInt(sisaWaktu % 60f));
            timerText.text = menit.ToString("00") + ":" + detik.ToString("00");
        }
        if (sisaWaktu <= 0f)
        {
            timerUp = true;
            Debug.Log("Waktu Level 2 habis");
            OpenFailPanel();
        }
    }
    public void TandaSelesai(string jenis)
    {
        if (jenis == "Amplitudo") amplitudoDone = true;
        if (jenis == "Frekuensi") frekuensiDone = true;
        if (jenis == "Panjang") panjangDone = true;
        if (AllDone)
        {
            timerUp = true;
            Debug.Log("Level 2 selesai");
        }
    }
    public void OpenEndPanel()
    {
        if (panelSelesai != null) panelSelesai.SetActive(true);
        if (player != null) player.SetInputLocked(true);
    }
    public void OpenFailPanel()
    {
        if (panelTimeUp != null) panelTimeUp.SetActive(true);
        if (player != null) player.SetInputLocked(true);
    }
    public void ContinueLevel3()
    {
        Debug.Log("Continue");
    }
    public void UlangiLevel2()
    {
        Debug.Log("Ulangi ditekan");
        SceneManager.LoadScene("Level2");
    }
    public void PenaltiWaktu(float detik)
    {
        sisaWaktu -= detik;
        if (sisaWaktu < 0f) sisaWaktu = 0f;
    }
}
