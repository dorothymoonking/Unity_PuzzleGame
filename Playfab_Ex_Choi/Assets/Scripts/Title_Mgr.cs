using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

//----- 이메일형식이 맞는지 확인하는 방법 스크립트
using System.Globalization;
using System.Text.RegularExpressions;
using System;

public class Title_Mgr : MonoBehaviour
{
    public string g_Message = "";
    
    [Header("LoginPanel")]
    public GameObject m_LoginPanelObj;
    public InputField IDInputField;
    public InputField PassInputField;
    public Button m_LoginBtn = null;
    public Button m_CreateAccOpenBtn = null;

    [Header("CreateAccountPanel")]
    public GameObject m_CreateAccPanelObj;
    public InputField New_IDInputField;
    public InputField New_PassInputField;
    public InputField New_NickInputField;
    public Button m_CreateAccountBtn = null;
    public Button m_CancelButton = null;

    private bool invalidEmailType = false;       // 이메일 포맷이 올바른지 체크
    private bool isValidFormat = false;          // 올바른 형식인지 아닌지 체크

    class Student
    {
        public int m_Kor;
        public int m_Eng;
        public int m_Math;
    }

    // Start is called before the first frame update
    void Start()
    {
        GlobalValue.InitDate();

        var a_St = new Student { m_Kor = 23, m_Math = 97 };

        if (m_CreateAccOpenBtn != null)
            m_CreateAccOpenBtn.onClick.AddListener(OpenCreateAccBtn);

        if (m_CancelButton != null)
            m_CancelButton.onClick.AddListener(CreateCancelBtn);

        if (m_CreateAccountBtn != null)
            m_CreateAccountBtn.onClick.AddListener(CreateAccountBtn);

        if (m_LoginBtn != null)
            m_LoginBtn.onClick.AddListener(LoginBtn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenCreateAccBtn()
    {
        if (m_LoginPanelObj != null)
            m_LoginPanelObj.SetActive(false);

        if (m_CreateAccPanelObj != null)
            m_CreateAccPanelObj.SetActive(true);
    }

    public void CreateCancelBtn()
    {
        if (m_LoginPanelObj != null)
            m_LoginPanelObj.SetActive(true);

        if (m_CreateAccPanelObj != null)
            m_CreateAccPanelObj.SetActive(false);
    }

    public void CreateAccountBtn()  //계정 생성 요청 함수
    {
        string a_IdStr = New_IDInputField.text;
        string a_PwStr = New_PassInputField.text;
        string a_NickStr = New_NickInputField.text;

        if (a_IdStr.Trim() == "" || a_PwStr.Trim() == "" || a_NickStr.Trim() == "")
        {
            g_Message = "ID, PW, 별명에 빈칸 없이 입력해 주셔야 합니다.";
            return;
        }

        if(!(3 <= a_IdStr.Length && a_IdStr.Length < 20))
        {
            g_Message = "ID는 3글자 이상 20글자 이하로 작성해 주세요.";
            return;
        }

        if(!(6 <= a_PwStr.Length && a_PwStr.Length < 20))
        {
            g_Message = "ID는 3글자 이상 20글자 이하로 작성해 주세요.";
            return;
        }
        
        if(!(2 <= a_NickStr.Length && a_NickStr.Length < 20))
        {
            g_Message = "ID는 3글자 이상 20글자 이하로 작성해 주세요.";
            return;
        }

        if(!CheckEmailAddress(a_IdStr))
        {
            g_Message = "Email 형식이 맞지 않습니다.";
            return;
        }

        var request = new RegisterPlayFabUserRequest
        {
            Email = New_IDInputField.text,
            Password = New_PassInputField.text,
            DisplayName = New_NickInputField.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, RegisterSuccess, RegisterFailure);

        //RequireBothUsernameAndEmail = false
        //RequireBothUsernameAndEmail 기본값은 true로 설정되어 있다.
        //이 옵션이 true면 사용자이름(ID)과 이메일이 모두 필요 합니다.
        //우리는 ID는 사용하지 않고 이메일을 ID처럼 사용해서 로그인 할 것이기 때문에
        //false 처리해 줘야 사용자이름(ID) 입력을 하지않고도 계정 생성을 할 수 있게 된다.
    }

    private void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        g_Message = "가입 성공";
    }

    private void RegisterFailure(PlayFabError error)
    {
        g_Message = "가입 실패 : " + error.GenerateErrorReport();
    }

    void OnGUI()
    {
        if(g_Message != "")
        {
            //White, Black, Bluem Cyan, Gray,... color = #ff0000
            GUI.Label(new Rect(20, 15, 1500, 100),
                "<color=White><size=25>" + g_Message + "</size></color>");
            //GUILayout.Label("<color=White><size=25>" + g_Message + "</size></color>");
        }
    }

    //----- 이메일형식이 맞는지 확인하는 방법 스크립트
    //https://blog.naver.com/rlawndks4204/221591566567
    // <summary>
    /// 올바른 이메일인지 체크.
    /// </summary>
    private bool CheckEmailAddress(string EmailStr)
    {
        if (string.IsNullOrEmpty(EmailStr)) isValidFormat = false;

        EmailStr = Regex.Replace(EmailStr, @"(@)(.+)$", this.DomainMapper, RegexOptions.None);
        if (invalidEmailType) isValidFormat = false;

        // true 로 반환할 시, 올바른 이메일 포맷임.
        isValidFormat = Regex.IsMatch(EmailStr,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase);
        return isValidFormat;
    }

    /// <summary>
    /// 도메인으로 변경해줌.
    /// </summary>
    /// <param name="match"></param>
    /// <returns></returns>
    private string DomainMapper(Match match)
    {
        // IdnMapping class with default property values.
        IdnMapping idn = new IdnMapping();

        string domainName = match.Groups[2].Value;
        try
        {
            domainName = idn.GetAscii(domainName);
        }
        catch (ArgumentException)
        {
            invalidEmailType = true;
        }
        return match.Groups[1].Value + domainName;
    }
    //-----

    public void LoginBtn()
    {
        string a_IdStr = IDInputField.text;
        string a_PwStr = PassInputField.text;

        if(a_IdStr.Trim() == "" || a_PwStr.Trim() == "")
        {
            g_Message = "ID, PW, 별명에 빈칸 없이 입력해 주셔야 합니다.";
            return;
        }

        if (!(3 <= a_IdStr.Length && a_IdStr.Length < 20))
        {
            g_Message = "ID는 3글자 이상 20글자 이하로 작성해 주세요.";
            return;
        }

        if (!(6 <= a_PwStr.Length && a_PwStr.Length < 20))
        {
            g_Message = "ID는 3글자 이상 20글자 이하로 작성해 주세요.";
            return;
        }

        if (!CheckEmailAddress(a_IdStr))
        {
            g_Message = "Email 형식이 맞지 않습니다.";
            return;
        }

        //----- 이 옵션을 추가해 줘야 로그인하면서 유저의 각종 정보를 가져올 수 있다.
        var option = new GetPlayerCombinedInfoRequestParams()
        { 
            //----- 이 옵션으로 DisplayName(닉네임), AvatarUrl을 가져올 수 있다.
            GetPlayerProfile = true,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true,
            },
            //-----

            GetPlayerStatistics = true, //-이 옵션으로 통계값(순위표에 관여하는)을 불러올 수 있다.
            GetUserData = true,         //- 이 옵션으로 <플레이어 데이터(타이틀)> 값을 불러올 수 있다.
        };
        //-----

        var request = new LoginWithEmailAddressRequest
        {
            Email = IDInputField.text,
            Password = PassInputField.text,
            InfoRequestParameters = option
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }//public void LoginBtn()

    private void OnLoginSuccess(LoginResult result)
    {
        g_Message = "로그인 성공";

        GlobalValue.g_Unique_ID = result.PlayFabId;

        if(result.InfoResultPayload != null)
        {
            GlobalValue.g_NickName = result.InfoResultPayload.PlayerProfile.DisplayName;
            //Debug.Log(result.InfoResultPayload.PlayerProfile.DisplayName);

            //-----유저 통계값(순위표에 관여하는) 불러오기 : 옵션설정에 의해 LoginWin
            foreach(var eachStat in result.InfoResultPayload.PlayerStatistics)
            {
                if(eachStat.StatisticName == "ToTalScore")
                {
                    GlobalValue.g_TotalScore = eachStat.Value;
                }

                else if(eachStat.StatisticName == "BestScore")
                {
                    GlobalValue.g_BestScore = eachStat.Value;
                }
            }//foreach
            //-----

            int a_GetValue = 0;
            int Idx = 0;
            foreach(var eachData in result.InfoResultPayload.UserData)
            {
                if(eachData.Key == "UserGold")
                {
                    //a_GetValue = int.Parse(eachData.Value.Value);
                    if(int.TryParse(eachData.Value.Value, out a_GetValue) == true)
                        GlobalValue.g_UserGold = a_GetValue;
                }
                else if(eachData.Key.Contains("ChrItem_") == true)
                {
                    Idx = 0;
                    string[] strArr = eachData.Key.Split('_');
                    if(2 <= strArr.Length)
                    {
                        if (int.TryParse(strArr[1], out Idx) == false)
                            g_Message = "string -> int : TryParse 실패";
                    }

                    if (GlobalValue.m_CrDataList.Count <= Idx)
                        continue;

                    if(int.TryParse(eachData.Value.Value, out a_GetValue)== false)
                        g_Message = "string -> int : TryParse 실패";

                    GlobalValue.m_CrDataList[Idx].m_Level = a_GetValue;
                }

                else if(eachData.Key.Contains("UserImgName") == true)
                {
                    GlobalValue.g_UserImgName = eachData.Value.Value;
                }
            }

        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        g_Message = "로그인 실패 : " + error.GenerateErrorReport();
    }
}
