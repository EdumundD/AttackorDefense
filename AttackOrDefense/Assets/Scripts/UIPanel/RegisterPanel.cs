using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;

public class RegisterPanel : BasePanel {
    private InputField usernameIF;
    private InputField passwordIF;
    private InputField rePasswordIF;

    private RegisterRequest registerRequest;
    private void Awake()
    {
        registerRequest = GetComponent<RegisterRequest>();

        usernameIF = transform.Find("UsernameLable/UsernameInput").GetComponent<InputField>();
        passwordIF = transform.Find("PasswordLable/PasswordInput").GetComponent<InputField>();
        rePasswordIF = transform.Find("RePassWordLable/RePasswordInput").GetComponent<InputField>();

        transform.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterClick);
        transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick);
    }
    public override void OnEnter()
    {
        gameObject.SetActive(true);

        transform.localScale = Vector3.zero;
        transform.localPosition = new Vector3(800, 0, 0);
        transform.DOScale(1, 0.5f);
        transform.DOLocalMove(Vector3.zero, 0.5f);

    }
    private void OnRegisterClick()
    {
        PlayClickSound();
        string msg = "";
        if (string.IsNullOrEmpty(usernameIF.text))
        {
            msg += "用户名不能为空\n";
        }else if (string.IsNullOrEmpty(passwordIF.text))
        {
            msg += "密码不能为空\n";
        }else if(rePasswordIF.text != passwordIF.text)
        {
            msg += "密码不一致\n";
        }
        if (msg != "")
        {
            uiMng.ShowMessage(msg);return;
        }
        //TODO 注册  发送到服务器端
        registerRequest.SendRequest(usernameIF.text, passwordIF.text);
    }

    public void OnRegisterResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            uiMng.ShowMessageSync("注册成功！");
        }
        else
        {
            uiMng.ShowMessageSync("用户名重复,注册失败！");
        }
    }

    private void OnCloseClick()
    {
        PlayClickSound();
        transform.DOScale(0, 0.5f);
        transform.DOLocalMove(new Vector3(800, 0, 0), 0.5f).OnComplete(() => uiMng.PopPanel());
    }
    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }

}
