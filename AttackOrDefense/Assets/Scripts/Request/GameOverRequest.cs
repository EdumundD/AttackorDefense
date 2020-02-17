//
// @brief: 结束游戏请求类
// @version: 1.0.0
// @author lhy
// @date: 2019/12/10
// 
// 
//

using Common;

public class GameOverRequest : BaseRequest {
    private GamePanel gamePanel;
    private bool isGameOver = false;
    private ReturnCode returnCode;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.GameOver;
        gamePanel = GetComponent<GamePanel>();
        base.Awake();
    }
    private void Update()
    {
        if (isGameOver)
        {
            gamePanel.OnGameOverResponse(returnCode);
            isGameOver = false;
        }
    }
    public override void OnResponse(string data)
    {
        returnCode = (ReturnCode)int.Parse(data);
        isGameOver = true;
    }
}
