﻿//
// @brief: 用户信息类
// @version: 1.0.0
// @author lhy
// @date: 2019/11/15
// 
// 
//



public class UserData
{
    public UserData(string userData)
    {
        string[] strs = userData.Split(',');
        this.Id = int.Parse(strs[0]);
        this.Username = strs[1];
        this.TotalCount = int.Parse(strs[2]);
        this.WinCount = int.Parse(strs[3]);
    }
    public UserData(string username,int totalcount,int wincount)
    {
        this.Username = username;
        this.TotalCount = totalcount;
        this.WinCount = wincount;
    }
    public UserData(int id,string username, int totalcount, int wincount)
    {
        this.Id = id;
        this.Username = username;
        this.TotalCount = totalcount;
        this.WinCount = wincount;
    }
    public int Id { get; set; }
    public string Username { get; private set; }
    public int TotalCount { get;set; }
    public int WinCount { get;set; }
}

