using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리자 관련 코드
using UnityEngine.UI; // UI 관련 코드

// 필요한 UI에 즉시 접근하고 변경할 수 있도록 허용하는 UI 매니저
public class UIManager : MonoBehaviour {
    // 싱글톤 접근용 프로퍼티
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }

    private static UIManager m_instance; // 싱글톤이 할당될 변수

    public Text ammoText; // 탄약 표시용 텍스트
    public Text scoreText; // 점수 표시용 텍스트
    public Text waveText; // 적 웨이브 표시용 텍스트
    public Text talkboxText;
    public Text coinText;
    public Text damageText;
    public GameObject gameoverUI; // 게임 오버시 활성화할 UI 
    public GameObject clearUI;
    public GameObject NpcInteraction;
    public GameObject PotalInteraction;
    public GameObject WeaponsChange;
    public GameObject TalkBox;
    public GameObject UpgradeButton;
    public GameObject Pannel;



    // 탄약 텍스트 갱신
    public void UpdateAmmoText(int magAmmo, int remainAmmo) {
        ammoText.text = magAmmo + "/" + remainAmmo;
    }

    // 점수 텍스트 갱신
    public void UpdateScoreText(int newScore) {
        scoreText.text = "Score : " + newScore;
    }
    //코인 갱신
    public void UpdateCoinText()
    {
        coinText.text = "Coin: " + PlayerPrefs.GetInt("Coin");
    }

    // 적 웨이브 텍스트 갱신
    public void UpdateWaveText(int waves) {
        waveText.text = "Wave " + (waves+1);
    }

    public void UpdateDamageText()
    {
        damageText.text = "현재 데미지: 기본 데미지 + 업그레이드된 데미지("+ "10 x "+PlayerPrefs.GetInt("Upgrade")+")";
    }

    public void UpdateTalkBox(string NpcName)
    {
        talkboxText.text = "무기 강화를 하고 싶나?";
    }



    // 게임 오버 UI 활성화
    public void SetActiveGameoverUI(bool active) {
        gameoverUI.SetActive(active);
    }
    public void SetActiveNpcInteractionUI(bool active)
    {
        NpcInteraction.SetActive(active);
    }
    public void SetActivePotalInteractionUI(bool active)
    {
        PotalInteraction.SetActive(active);
    }
    public void SetActiveWeaponsChangeUI(bool active)
    {
        WeaponsChange.SetActive(active);
    }
    public void SetActiveNpcTalkBox(bool active)
    {
        TalkBox.SetActive(active);
    }
    public void SetActiveUpgradeButton(bool active)
    {
        UpgradeButton.SetActive(active);
    }

    public void SetActivePanel(bool active)
    {
        Pannel.SetActive(active);
    }

    public void SetActiveClearUI(bool active)
    {
        clearUI.SetActive(active);
    }
    public float GetAlpha()
    {
        Image image=Pannel.GetComponent<Image>();
        Color color = image.color;
        return color.a;
    }
    // 게임 재시작
    public void GameRestart() {
        SceneManager.LoadScene("HomeTown");
    }


}