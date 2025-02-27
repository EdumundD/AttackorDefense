﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public enum ActionCode
    {
        None,
        Login,
        Register,
        ListRoom,
        CreateRoom,
        JoinRoom,
        UpdateRoom,
        QuitRoom,
        ShowTimer,
        StartGame,
        StartPlay,
        ListBuilding,
        UpdateResult,
        GameOver,
        NotFound,
        BuildTower,
        CreateSoldier,
        EndBattle,
        LockStepLogic
    }
}
