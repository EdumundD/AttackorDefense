using Common;
using System;

public class StartPlayRequest : BaseRequest {
    private bool isStartPlaying = false;
    private int playerCount;
    public override void Awake()
    {
        actionCode = ActionCode.StartPlay;
        base.Awake();
    }

    private void Update()
    {
        if (isStartPlaying)
        {
            facade.EnterPlayingSync(playerCount);
            isStartPlaying = false;
        }
    }
    public override void OnResponse(string data)
    {
        playerCount = Int32.Parse(data);
        isStartPlaying = true;
    }

}
