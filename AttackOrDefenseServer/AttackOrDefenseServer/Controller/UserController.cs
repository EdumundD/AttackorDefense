using System;
using AttackOrDefenseServer.DAO;
using AttackOrDefenseServer.Model;
using AttackOrDefenseServer.Servers;
using Common;

namespace AttackOrDefenseServer.Controller
{
    class UserController:BaseController
    {
        private UserDAO userDAO = new UserDAO();
        private ResultDAO resultDAO = new ResultDAO();
        public UserController()
        {
            requestCode = RequestCode.User;
        }
        public string Login(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            User user = userDAO.VerifyUser(client.MySQLConn, strs[0], strs[1]);
            if (user == null)
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                Result res = resultDAO.GetResultByUserid(client.MySQLConn, user.Id);
                client.SetUserData(user, res);
                return string.Format("{0},{1},{2},{3}", ((int)ReturnCode.Success).ToString(), user.Username, res.TotalCount, res.WinCount);
            }
        }
        public string Register(string data, Client client, Server server)
        {
            Console.WriteLine("一个客户端正在注册");
            string[] strs = data.Split(',');
            bool res = userDAO.GetUserByUsername(client.MySQLConn, strs[0]);
            if (res)
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            userDAO.AddUser(client.MySQLConn, strs[0], strs[1]);
            return ((int)ReturnCode.Success).ToString();
        }
    }
}
