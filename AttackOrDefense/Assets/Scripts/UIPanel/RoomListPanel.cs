//
// @brief: 房间列表面板类
// @version: 1.0.0
// @author lhy
// @date: 2019/11/17
// 
// 
//

using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Common;

public class RoomListPanel : BasePanel {

    private RectTransform battleRes;
    private RectTransform roomList;
    private VerticalLayoutGroup roomLayout;
    private GameObject roomItemPrefab;
    private ListRoomRequest listRoomRequest;
    private List<UserData> udList = null;
    private List<int> ucList = null;
    private CreatRoomRequest creatRoomRequest;
    private JoinRoomRequest joinRoomRequest;

    private UserData blueud = null;
    private UserData greenud = null;
    private UserData redud = null;
    private UserData brownud = null;

    private bool isCreatRoomSuccess = false;
    private bool isUpdate = false;
    private bool isListRoom = false;

    private void Awake()
    {
        battleRes = transform.Find("BattleRes").GetComponent<RectTransform>();
        roomList = transform.Find("RoomList").GetComponent<RectTransform>();
        roomLayout = transform.Find("RoomList/ScrollRect/Layout").GetComponent<VerticalLayoutGroup>();
        roomItemPrefab = Resources.Load("UIPanel/RoomItem") as GameObject;
        listRoomRequest = GetComponent<ListRoomRequest>();
        creatRoomRequest = GetComponent<CreatRoomRequest>();
        joinRoomRequest = GetComponent<JoinRoomRequest>();

        transform.Find("RoomList/CreatRoomButton").GetComponent<Button>().onClick.AddListener(OnCreatRoomClick);
        transform.Find("RoomList/CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick);
        transform.Find("RoomList/RefreshButton").GetComponent<Button>().onClick.AddListener(OnRefreshClick);
    }
    public override void OnEnter()
    {
        SetBattleRes();
        if (battleRes != null)
            EnterAnim();
        if (listRoomRequest == null)
            listRoomRequest = GetComponent<ListRoomRequest>();
        listRoomRequest.SendRequest();
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
        SetBattleRes();
        listRoomRequest.SendRequest();
    }
    private void OnCloseClick()
    {
        PlayClickSound();
        uiMng.PopPanel();
    }
    private void OnCreatRoomClick()
    {
        PlayClickSound();
        creatRoomRequest.SendRequest();
    }
    private void OnRefreshClick()
    {
        listRoomRequest.SendRequest();
    }
    public void OnJoinClick(int id)
    {
        joinRoomRequest.SendRequest(id);
    }
    public void OnCreatRoomResponse()
    {
        isCreatRoomSuccess = true;
    }
    public void OnJoinResponse(ReturnCode returnCode, UserData ud1, UserData ud2, UserData ud3, UserData ud4)
    {
        switch (returnCode)
        {
            case ReturnCode.Success:
                this.blueud = ud1;
                this.greenud = ud2;
                this.redud = ud3;
                this.brownud = ud4;
                isUpdate = true;
                break;
            case ReturnCode.NotFound:
                uiMng.ShowMessageSync("房间被销毁，无法加入！");
                break;
            case ReturnCode.Fail:
                uiMng.ShowMessageSync("房间已满，无法加入！");
                break;
        }
    }
    public void OnUpdateResultResponse(int totalCount, int winCount)
    {
        //isCreatRoomSuccess = true;
    }
    private void SetBattleRes()
    {
        UserData ud = Facade.GetUserData();
        transform.Find("BattleRes/Username").GetComponent<Text>().text = ud.Username;
        transform.Find("BattleRes/TotalCount").GetComponent<Text>().text = "总场数：" + ud.TotalCount.ToString();
        transform.Find("BattleRes/WinCount").GetComponent<Text>().text = "胜利：" + ud.WinCount.ToString();
    }

    private void Update()
    {
        if (isListRoom)
        {
            isListRoom = false;
            if (udList != null)
            {
                LoadRoomItem();
                udList = null;
            }
        }
        if (isUpdate)
        {
            BasePanel roomPanel = uiMng.PushPanel(UIPanelType.Room);
            (roomPanel as RoomPanel).SetAllPlayerResSync(blueud, greenud, redud, brownud);
            isUpdate = false;
            blueud = null;
            greenud = null;
            redud = null;
            brownud = null;
        }
        if (isCreatRoomSuccess)
        {
            BasePanel panel = uiMng.PushPanel(UIPanelType.Room);
            (panel as RoomPanel).SetBluePlayerResSync();
            isCreatRoomSuccess = false;
        }
    }
    private void EnterAnim()
    {
        gameObject.SetActive(true);
        battleRes.localPosition = new Vector3(-1000, 0);
        battleRes.DOLocalMoveX(-389, 0.5f);
        roomList.localPosition = new Vector3(1000, 0);
        roomList.DOLocalMoveX(160, 0.5f);
    }
    private void HideAnim()
    {
        battleRes.DOLocalMoveX(-1000, 0.5f);
        roomList.DOLocalMoveX(1000, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }
    private void LoadRoomItem()
    {
        RoomItem[] riArray = roomLayout.GetComponentsInChildren<RoomItem>();
        foreach (RoomItem ri in riArray)
        {
            ri.DestroySelf();
        }
        int count = udList.Count;
        for (int i = 0; i < count; i++)
        {
            Debug.Log("Instantiate roomItemPrefab");
            GameObject roomItem = GameObject.Instantiate(roomItemPrefab);
            roomItem.transform.SetParent(roomLayout.transform);
            UserData ud = udList[i];
            roomItem.GetComponent<RoomItem>().SetRoomInfo(ud.Id, ud.Username, ud.TotalCount, ud.WinCount,ucList[i],this);
        }
        int roomCount = GetComponentsInChildren<RoomItem>().Length;
        Vector2 size = roomLayout.GetComponent<RectTransform>().sizeDelta;
        roomLayout.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x,
            roomCount * (roomItemPrefab.GetComponent<RectTransform>().sizeDelta.y + roomLayout.spacing));
    }
    public void LoadRoomItemSync(List<UserData> udList,List<int> unList)
    {
        this.udList = udList;
        this.ucList = unList;
        isListRoom = true;
    }
    
}
