using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

