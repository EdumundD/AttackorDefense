using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Common;

public class LoginPanel : BasePanel {

    private Button closeButton;
    private InputField usernameIF;
    private InputField passwordIF;

    private LoginRequest loginRequest;
    private void Awake()
    {
        loginRequest = GetComponent<LoginRequest>();
        usernameIF = transform.Find("UsernameLable/UsernameInput").GetComponent<InputField>();
        passwordIF = transform.Find("PasswordLable/PasswordInput").GetComponent<InputField>();
        closeButton = transform.Find("CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(OnCloseClick);

        transform.Find("LoginButton").GetComponent<Button>().onClick.AddListener(OnLoginClick);

        transform.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterClick);
    }
    public override void OnEnter()
    {
        EnterAnimation(); 
    }
    public override void OnPause()
    {
        HideAnimation();   
    }
    public override void OnResume()
    {
        EnterAnimation();
    }
    public override void OnExit()
    {
        HideAnimation();
    }

    private void OnCloseClick()
    {
        PlayClickSound();
        uiMng.PopPanel();
    }

    private void OnLoginClick() {
        PlayClickSound();
        string msg = "";
        if (string.IsNullOrEmpty(usernameIF.text))
        {
            msg += "用户名不能为空 ";
        }
        if (string.IsNullOrEmpty(passwordIF.text))
        {
            msg += " 密码不能为空";
        }
        if(msg != "")
        {
            uiMng.ShowMessage(msg);return;
        }

        loginRequest.SendRequest(usernameIF.text, passwordIF.text);
    }

    public void OnLoginResponse(ReturnCode returnCode,string[] strs)
    {
        if(returnCode == ReturnCode.Success)
        {
            uiMng.ShowMessageSync("登陆成功");
            uiMng.PushPanelSync(UIPanelType.RoomList);
        }
        else
        {
            uiMng.ShowMessageSync("用户名或密码输入错误");
        }
    }

    private void OnRegisterClick()
    {
        PlayClickSound();
        uiMng.PushPanel(UIPanelType.Register);
    }
    
    private void EnterAnimation()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.localPosition = new Vector3(800, 0, 0);
        transform.DOScale(1, 0.5f);
        transform.DOLocalMove(Vector3.zero, 0.5f);
    }

    private void HideAnimation()
    {
        transform.DOScale(0, 0.5f);
        transform.DOLocalMove(new Vector3(800, 0, 0), 0.5f).OnComplete(() => gameObject.SetActive(false));
    }
}
