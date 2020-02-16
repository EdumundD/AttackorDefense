using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;

public class RoomPanel : BasePanel {

    private Text BluePlayerUsername;
    private Text BluePlayerTotalCount;
    private Text BluePlayerWinCount;

    private Text GreenPlayerUsername;
    private Text GreenPlayerTotalCount;
    private Text GreenPlayerWinCount;

    private Text RedPlayerUsername;
    private Text RedPlayerTotalCount;
    private Text RedPlayerWinCount;

    private Text BrownPlayerUsername;
    private Text BrownPlayerTotalCount;
    private Text BrownPlayerWinCount;

    private Transform bluePanel;
    private Transform greenPanel;
    private Transform redPanel;
    private Transform brownPanel;
    private Transform startButton;
    private Transform exitButton;

    private UserData blueud = null;
    private UserData greenud = null;
    private UserData redud = null;
    private UserData brownud = null;

    private QuitRomRequest quitRoomRequest;
    private StartGameRequest startGameRequest;

    private void Awake()
    {
        BluePlayerUsername = transform.Find("BluePanel/Text").GetComponent<Text>();
        BluePlayerTotalCount = transform.Find("BluePanel/Text (1)").GetComponent<Text>();
        BluePlayerWinCount = transform.Find("BluePanel/Text (2)").GetComponent<Text>();   //蓝色

        GreenPlayerUsername = transform.Find("GreenPanel/Text").GetComponent<Text>();
        GreenPlayerTotalCount = transform.Find("GreenPanel/Text (1)").GetComponent<Text>();
        GreenPlayerWinCount = transform.Find("GreenPanel/Text (2)").GetComponent<Text>();  //绿色

        RedPlayerUsername = transform.Find("RedPanel/Text").GetComponent<Text>();
        RedPlayerTotalCount = transform.Find("RedPanel/Text (1)").GetComponent<Text>();
        RedPlayerWinCount = transform.Find("RedPanel/Text (2)").GetComponent<Text>();  //红色

        BrownPlayerUsername = transform.Find("BrownPanel/Text").GetComponent<Text>();
        BrownPlayerTotalCount = transform.Find("BrownPanel/Text (1)").GetComponent<Text>();
        BrownPlayerWinCount = transform.Find("BrownPanel/Text (2)").GetComponent<Text>();  //棕色
        

        transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(OnStartClick);
        transform.Find("ExitButton").GetComponent<Button>().onClick.AddListener(OnExitClick);
        
        
        bluePanel = transform.Find("BluePanel");
        greenPanel = transform.Find("GreenPanel");
        redPanel = transform.Find("RedPanel");
        brownPanel = transform.Find("BrownPanel");

        startButton = transform.Find("StartButton");
        exitButton = transform.Find("ExitButton");

        quitRoomRequest = GetComponent<QuitRomRequest>();
        startGameRequest = GetComponent<StartGameRequest>();

        EnterAnim();
        ClearEnemyPlayerRes(1);
        ClearEnemyPlayerRes(2);
        ClearEnemyPlayerRes(3);
        ClearEnemyPlayerRes(4);
    }
    private void Update()
    {
        if (blueud != null)
        {
            SetBluePlayerRes(blueud.Username, blueud.TotalCount.ToString(), blueud.WinCount.ToString());
            blueud = null;
        }
        if (greenud != null)
        {
            SetGreenPlayerRes(greenud.Username, greenud.TotalCount.ToString(), greenud.WinCount.ToString());
            greenud = null;
        }
        if (redud != null)
        {
            SeRedPlayerRes(redud.Username, redud.TotalCount.ToString(), redud.WinCount.ToString());
            redud = null;
        }
        if (brownud != null)
        {
            SetBrownPlayerRes(brownud.Username, brownud.TotalCount.ToString(), brownud.WinCount.ToString());
            brownud = null;
        }
    }
    public void SetBluePlayerResSync()
    {
        blueud = Facade.GetUserData();
    }
    public void SetAllPlayerResSync(UserData ud1,UserData ud2,UserData ud3, UserData ud4)
    {
        this.blueud = ud1;
        this.greenud = ud2;
        this.redud = ud3;
        this.brownud = ud4;
    }
    public void SetBluePlayerRes(string username, string totalCount, string winCount)
    {
        BluePlayerUsername.text = username;
        BluePlayerTotalCount.text = "总常数：" + totalCount;
        BluePlayerWinCount.text = "胜场：" + winCount;
    }
    public void SetGreenPlayerRes(string username, string totalCount, string winCount)
    {
        GreenPlayerUsername.text = username;
        GreenPlayerTotalCount.text = "总常数：" + totalCount;
        GreenPlayerWinCount.text = "胜场：" + winCount;
    }
    public void SeRedPlayerRes(string username, string totalCount, string winCount)
    {
        RedPlayerUsername.text = username;
        RedPlayerTotalCount.text = "总常数：" + totalCount;
        RedPlayerWinCount.text = "胜场：" + winCount;
    }
    public void SetBrownPlayerRes(string username, string totalCount, string winCount)
    {
        BrownPlayerUsername.text = username;
        BrownPlayerTotalCount.text = "总常数：" + totalCount;
        BrownPlayerWinCount.text = "胜场：" + winCount;
    }
    public void ClearEnemyPlayerRes(int i)
    {
        switch (i)
        {
            case 1:
                BluePlayerUsername.text = "";
                BluePlayerTotalCount.text = "等待玩家加入..";
                BluePlayerWinCount.text = "";
                break;
            case 2:
                GreenPlayerUsername.text = "";
                GreenPlayerTotalCount.text = "等待玩家加入..";
                GreenPlayerWinCount.text = "";
                break;
            case 3:
                RedPlayerUsername.text = "";
                RedPlayerTotalCount.text = "等待玩家加入..";
                RedPlayerWinCount.text = "";
                break;
            case 4:
                BrownPlayerUsername.text = "";
                BrownPlayerTotalCount.text = "等待玩家加入..";
                BrownPlayerWinCount.text = "";
                break;
        }
    }
    private void OnStartClick()
    {
        startGameRequest.SendRequest();
    }
    public void OnStartResponse(ReturnCode returnCode)
    {
        if(returnCode == ReturnCode.Fail)
        {
            uiMng.ShowMessageSync("您不是房主，无法开始游戏！");
        }
        else if( returnCode == ReturnCode.Success)
        {
            uiMng.PushPanelSync(UIPanelType.Game);
            Facade.FollowRoleSync();
        }
    }
    private void OnExitClick()
    {
        quitRoomRequest.SendRequest();
    }
    public void OnExitResponse()
    {
        uiMng.PopPanelSync();
    }
    public override void OnEnter()
    {
        if (bluePanel != null)
            EnterAnim();
    }
    public override void OnExit()
    {
        ExitAnim();
    }
    public override void OnPause()
    {
        ExitAnim();
    }
    public override void OnResume()
    {
        EnterAnim();
    }
    
    private void EnterAnim()
    {   gameObject.SetActive(true);
        bluePanel.localPosition = new Vector3(-1000, 0, 0);
        bluePanel.DOLocalMoveX(-500, 0.4f);
        greenPanel.localPosition = new Vector3(-800, 0, 0);
        greenPanel.DOLocalMoveX(-250, 0.4f);
        redPanel.localPosition = new Vector3(800, 0, 0);
        redPanel.DOLocalMoveX(250, 0.4f);
        brownPanel.localPosition = new Vector3(1000, 0, 0);
        brownPanel.DOLocalMoveX(500, 0.4f);
        startButton.localScale = Vector3.zero;
        startButton.DOScale(1, 0.4f);
        exitButton.localScale = Vector3.zero;
        exitButton.DOScale(1, 0.4f);
    }
    private void ExitAnim()
    {
        bluePanel.DOLocalMoveX(-1000, 0.4f);
        greenPanel.DOLocalMoveX(-800, 0.4f);
        redPanel.DOLocalMoveX(800, 0.4f);
        brownPanel.DOLocalMoveX(1000, 0.4f);
        startButton.DOScale(0, 0.4f);
        exitButton.DOScale(0, 0.4f).OnComplete(() => gameObject.SetActive(false));
    }
}
