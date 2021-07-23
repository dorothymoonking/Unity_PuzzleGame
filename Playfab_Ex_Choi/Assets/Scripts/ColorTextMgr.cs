using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

enum ColorNum
{
    Red = 0,
    Yellow,
    Black,
    Green,
    Blue
}

class ColorText
{
    public ColorNum m_ColorName = ColorNum.Red;
    public Color m_Color = Color.white;
    public string m_Text = "";
}

public class ColorTextMgr : MonoBehaviour
{
    public Text ColorTxt = null;
    public Text m_UserScore;
    public Text m_TimerTxt;
    public Text m_GameTimeTxt; 
    public Text Ranking_Text = null;
    public Text EndScoreTxt = null;
    public Button DoneBtn = null;
    public GameObject GameOverPanel = null;
    public Button[] ColorBtn;
    ColorText a_Text = new ColorText();
    public Button CloseBtn = null;

    Color[] m_ColorList = { Color.red, Color.yellow, Color.black,
                            Color.green, Color.white, Color.blue};
    string[] m_ColorText = { "빨강", "노랑", "검정", "초록", "흰색", "파랑" };

    float m_Time = 0;
    float m_GameTime = 30.0f;
    int m_Score;
    // Start is called before the first frame update
    void Start()
    {
        for (int ii = 0; ii < ColorBtn.Length; ii++)
        {
            int Index = ii;
            if (ColorBtn[ii] != null)
                ColorBtn[ii].onClick.AddListener(() =>
                {
                    Judgment((ColorNum)Index);
                });
        }

        if (GameOverPanel != null)
            GameOverPanel.SetActive(false);

        if (DoneBtn != null)
            DoneBtn.onClick.AddListener(() => 
            {
                Time.timeScale = 1;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
            });

        if (CloseBtn != null)
            CloseBtn.onClick.AddListener(() =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
            });

        m_GameTime = 30.0f;
        m_UserScore.text = "User점수 : " + GlobalValue.g_BestScore.ToString();
        ChangeColorText();
        GetLeaderboard();
    }

    // Update is called once per frame
    void Update()
    {
        if (0.0f < m_Time)
        {
            int TimeNum = (int)m_Time;
            m_TimerTxt.text = TimeNum.ToString();

            m_Time -= Time.deltaTime;
            if (m_Time <= 0.0f)
            {
                m_Time = 0.0f;
                m_Score -= 50;
                if (m_Score < 0)
                    m_Score = 0;
                m_UserScore.text = "User점수 : " + m_Score.ToString();
                ChangeColorText();
            }
        }

        if (0.0f < m_GameTime)
        {
            m_GameTime -= Time.deltaTime;
            SetTimeText();
            if (m_GameTime <= 0.0f)
            {
                m_GameTime = 0.0f;
                EndGame();
            }
        }
    }

    void ChangeColorText()
    {
        m_Time = 3.0f;
        int RanText = Random.Range(0, 6);
        int RanColor = Random.Range(0, 6);
        a_Text.m_Text = m_ColorText[RanText];
        a_Text.m_Color = m_ColorList[RanColor];
        a_Text.m_ColorName = (ColorNum)RanColor;
        SetText();
    }

    void SetText()
    {
        ColorTxt.text = a_Text.m_Text;
        ColorTxt.color = a_Text.m_Color;
    }

    void SetTimeText()
    {
        int TimeDateChangeInt = (int)m_GameTime;
        if (m_GameTimeTxt != null)
            m_GameTimeTxt.text = TimeDateChangeInt.ToString();
    }

    private void Judgment(ColorNum colorNum)
    {
        if (colorNum == a_Text.m_ColorName)
        {
            m_Score += 100;
        }
        else
        {
            m_Score -= 50;
            if (m_Score < 0)
                m_Score = 0;
        }
        ChangeColorText();
        m_UserScore.text = "User점수 : " + m_Score.ToString();
    }

    void ScoreSet(int _Score)
    {
        if (GlobalValue.g_BestScore < _Score)
            GlobalValue.g_BestScore = _Score;

        GlobalValue.g_TotalScore += _Score;
        GlobalValue.g_UserGold += _Score;
        UpdateScoreCo();
        UpdateGoldCo();
    }

    void EndGame()
    {
        ScoreSet(m_Score);
        Time.timeScale = 0;
        if (GameOverPanel != null)
            GameOverPanel.SetActive(true);

        EndScoreTxt.text = "Score : " + m_Score;
    }

    void UpdateScoreCo()
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            //BestScore, BestLevel
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "ToTalScore",
                                       Value = GlobalValue.g_TotalScore },
                new StatisticUpdate { StatisticName = "BestScore",
                                       Value = GlobalValue.g_BestScore }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(
                request,

                (result) =>
                {
                    //업데이트 성공 처리
                    Debug.Log("점수 저장 성공");
                },

                (error) =>
                {
                    //업데이트 실패시 응답 함수
                    Debug.Log("점수 저장 실패");
                }
            );
    }

    void UpdateGoldCo()
    {
        Dictionary<string, string> a_Gold = new Dictionary<string, string>();
        string a_MkKey = "";
        a_Gold.Clear();
        a_Gold.Add("UserGold", GlobalValue.g_UserGold.ToString()); //Dictionary 노드 추가 방법
        var request = new UpdateUserDataRequest
        {
            Data = a_Gold
        };

        PlayFabClientAPI.UpdateUserData(
                request,
                (result) =>
                {
                    //업데이트 성공 처리
                    Debug.Log("골드 저장 성공");
                },

                (error) =>
                {
                    //업데이트 실패시 응답 함수
                    Debug.Log("골드 저장 실패");
                }
            );
    }

    public void GetLeaderboard() //순위 불러오기...
    {
        var request = new GetLeaderboardRequest
        {
            StartPosition = 0,      //0번인덱스 즉 1등 부터
            StatisticName = "BestScore",
            MaxResultsCount = 10,   //10명까지
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true //닉네임
            }
        };


        PlayFabClientAPI.GetLeaderboard(request,
            (result) =>
            {
                //랭킹 리스트 받아오기 성공
                if (Ranking_Text == null)
                    return;

                //랭킹 리스트 받아오기 성공
                Ranking_Text.text = "";

                for (int i = 0; i < result.Leaderboard.Count; i++)
                {
                    var curBoard = result.Leaderboard[i];

                    //등수 안에 내가 있다면 색 표시
                    if (curBoard.PlayFabId == GlobalValue.g_Unique_ID)
                        Ranking_Text.text += "<color=#00ff00>";

                    Ranking_Text.text += (i + 1).ToString() + "등 : " +
                    curBoard.DisplayName + " : " +
                    curBoard.StatValue + "점\n";
                    // " : " + curBoard.PlayFabId + "\n"; 

                    //등수 안에 내가 있다면 색 표시
                    if (curBoard.PlayFabId == GlobalValue.g_Unique_ID)
                        Ranking_Text.text += "</color>";
                }//for(int i = 0; i < result.Leaderboard.Count; i++)

            },
            (error) => Debug.Log("리더보드 불러오기 실패")
        );
    }
}
