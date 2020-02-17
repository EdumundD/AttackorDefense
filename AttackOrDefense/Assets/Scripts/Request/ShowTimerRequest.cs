//
// @brief: 显示倒计时请求类（开始游戏的倒计时）
// @version: 1.0.0
// @author lhy
// @date: 2019/12/5
// 
// 
//

using Common;

public class ShowTimerRequest : BaseRequest {

    private GamePanel gamePanel;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.ShowTimer;
        gamePanel = GetComponent<GamePanel>();
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        int time = int.Parse(data);
        gamePanel.ShowTimeSync(time);
        if(time == 1)
        {

        }
    }

}
