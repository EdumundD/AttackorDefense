//
// @brief: 开始面板类
// @version: 1.0.0
// @author lhy
// @date: 2019/11/10
// 
// 
//

using UnityEngine.UI;
using DG.Tweening;

public class StartPanel : BasePanel {
    private Button loginButton;
    public override void OnEnter()
    {
        base.OnEnter();
        loginButton = transform.Find("LoginButton").GetComponent<Button>();
        loginButton.onClick.AddListener(OnloginClick);
    }
    public override void OnPause()
    {
        base.OnPause();
        Tween tween = loginButton.transform.DOScale(0, 0.3f);
        tween.OnComplete(()=>loginButton.gameObject.SetActive(false));
    }

    public override void OnResume()
    {
        base.OnResume();
        loginButton.gameObject.SetActive(true);
        loginButton.transform.DOScale(1, 0.3f);

    }
    private void OnloginClick()
    {
        test();
        PlayClickSound();
        loginButton.transform.DOScale(1.2f, 0.2f).OnComplete(() => {
            uiMng.PushPanel(UIPanelType.Login);
            gameObject.SetActive(false);
        }); ;
    }

    private void test()
    {
       
    }
}
