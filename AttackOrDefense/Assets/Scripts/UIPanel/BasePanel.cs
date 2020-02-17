//
// @brief: 面板基类
// @version: 1.0.0
// @author lhy
// @date: 2019/11/10
// 
// 
//

using UnityEngine;
using System.Collections;

public class BasePanel : MonoBehaviour {
    protected UIManager uiMng;
    private GameFacade facade;

    public UIManager UIMng { set { uiMng = value; } }

    public GameFacade Facade { get { return facade; } set { facade = value; } }

    protected void PlayClickSound()
    {
        Facade.PlayNormalSound(AudioManager.Sound_ButtonClick);
    }
    /// <summary>
    /// 界面被显示出来
    /// </summary>
    public virtual void OnEnter()
    {

    }

    /// <summary>
    /// 界面暂停
    /// </summary>
    public virtual void OnPause()
    {

    }

    /// <summary>
    /// 界面继续
    /// </summary>
    public virtual void OnResume()
    {

    }

    /// <summary>
    /// 界面不显示,退出这个界面，界面被关闭
    /// </summary>
    public virtual void OnExit()
    {

    }

}
