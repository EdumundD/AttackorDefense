using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour {

    public Text username;
    public Text totalCount;
    public Text winCount;
    public Text userCount;
    public Button joinButton;
    private int id;
    private RoomListPanel roomListPanel;

	void Awake()
    {
        username = transform.Find("Username").GetComponent<Text>();
        totalCount = transform.Find("TotalCount").GetComponent<Text>();
        winCount = transform.Find("WinCount").GetComponent<Text>();
        userCount = transform.Find("UserCount").GetComponent<Text>();
        joinButton = transform.Find("JoinButton").GetComponent<Button>();
        if (joinButton != null)
        {
            joinButton.onClick.AddListener(OnJoinClick);
        }
	}
    public void SetRoomInfo(int id, string username, int totalCount, int winCount, int userCount, RoomListPanel roomListPanel)
    {
        this.id = id;
        this.username.text = username;
        this.totalCount.text = "总场数：" + totalCount;
        this.winCount.text = "胜场：" + winCount;
        this.userCount.text = userCount + "/4";
        this.roomListPanel = roomListPanel;
    }
    private void OnJoinClick()
    {
        roomListPanel.OnJoinClick(id);
    }
    public void DestroySelf()
    {
        GameObject.Destroy(this.gameObject);
    }
}
