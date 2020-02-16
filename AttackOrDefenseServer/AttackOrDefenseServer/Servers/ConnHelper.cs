using System;
using MySql.Data.MySqlClient;

namespace AttackOrDefenseServer.Servers
{
    class ConnHelper
    {
        public const string CONNECTIONSTRING = "datasource = 127.0.0.1;port = 3307;database = game;user = root;pwd = root;SslMode = none";

        public static MySqlConnection Connect()
        {
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTRING);
            try
            {
                conn.Open();
                return conn;
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public static void CloseConnection(MySqlConnection conn)
        {
            if (conn != null) conn.Close();
            else Console.WriteLine("MysqlConnection 不能为空！");
        }
    }
}
