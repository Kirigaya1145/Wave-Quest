using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class Level1Challenge : MonoBehaviour
{
    public bool targetWave;
    public NPCHint npcHint;

    public GameObject rewardPanel;
    [TextArea] public string dialogTaliDone =
        "Kerja Bagus!\n" +
        "Partikel tali bergetar naik-turun, tegak lurus arah rambat. " +
        "\nItulah gelombang transversal.";
    [TextArea] public string dialogPegasDone = 
        "Kerja Bagus!\n " + 
        "Partikel pegas bergetar maju-mundur, sejajar arah rambat." +
        "\nItulah gelombang longitudinal.";

    public int totalRound = 2;
    private int roundEnd = 0;

    void Start()
    {
        targetWave = Random.value > 0.5f;
    }
    public void OnMachineSolved()
    {
        roundEnd++;
        if (rewardPanel != null)
        {
            rewardPanel.SetActive(true);
            CancelInvoke(nameof(HideReward));
            Invoke(nameof(HideReward), 2f);
        }
        if (npcHint != null)
        {
            npcHint.ShowFeedback(targetWave ? dialogTaliDone : dialogPegasDone);
        }
        if (roundEnd >= totalRound) 
        {
            Debug.Log("Level 1 Selesai");
            //if (!string.IsNullOrEmpty(nextSceneName)) Invoke(nameof(LoadNext), 3f);
        }
        else 
        {
            targetWave = !targetWave;
        }
    }
    void HideReward() { rewardPanel.SetActive(false);}
}
