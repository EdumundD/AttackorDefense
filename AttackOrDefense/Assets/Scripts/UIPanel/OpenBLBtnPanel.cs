using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OpenBLBtnPanel : BasePanel
{
    private RectTransform openBLBtn;
    void Awake()
    {
        openBLBtn = gameObject.GetComponent<RectTransform>();
        transform.Find("OpenButton").GetComponent<Button>().onClick.AddListener(OnOpenClick);
    }

    void Update()
    {
        
    }
    public override void OnEnter()
    {
        EnterAnim();
    }
    public override void OnExit()
    {
        HideAnim();
    }
    public override void OnPause()
    {
        HideAnim();
    }
    public override void OnResume()
    {
        EnterAnim();
    }
    private void EnterAnim()
    {
        gameObject.SetActive(true);
        //openBLBtn.transform.localPosition = new Vector3(100, 0);
        //openBLBtn.DOMoveX(0, 0.5f);

    }
    private void HideAnim()
    {
        openBLBtn.DOLocalMoveX(0, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }
    private void OnOpenClick()
    {
        PlayClickSound();
        uiMng.PushPanel(UIPanelType.BuildingList);
        HideAnim();
    }
}
