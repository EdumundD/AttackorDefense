//
// @brief: 记录输入类
// @version: 1.0.0
// @author lhy
// @date: 2020/2/10
// 
// 
//

using UnityEngine;

public class InputMono : MonoBehaviour
{
    private static bool IsReplay = GameData.g_bRplayMode;

    public bool isStartBattle = false;
    public bool isRefresh = false;
    public FrameInput frameInput = new FrameInput();
    public void FixedUpdate()
    {
        if (isStartBattle)
        {
            if (!IsReplay)
            {
                if (isRefresh)
                {
                    isRefresh = false;
                    GameFacade.Instance.SendFrameInput(frameInput);
                    frameInput = new FrameInput();
                }
            }
        }
    }
}

