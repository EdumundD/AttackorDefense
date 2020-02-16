using System;
using AttackOrDefenseServer.Model;
using MySql.Data.MySqlClient;

namespace AttackOrDefenseServer.DAO
{
    class UserDAO
    {
        public User VerifyUser(MySqlConnection conn, string username, string password)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where username = @username and password = @password", conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Console.WriteLine(username + " 登陆成功");
                    int id = reader.GetInt32("id");
                    User user = new User(id, username, password);
                    return user;
                }
                else
                {
                    Console.WriteLine(username + " 登陆失败");
                    return null;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("在VerifyUser的时候出现异常:" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return null;
        }

        public bool GetUserByUsername(MySqlConnection conn, string username)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where username = @username", conn);
                cmd.Parameters.AddWithValue("username", username);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Console.WriteLine("用户名重复，无法注册");
                    return true;
                }
                else
                {
                    Console.WriteLine("没有重复的用户名,可以注册");
                    return false;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("在GetUserByUsername的时候出现异常:" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return false;
        }

        public void AddUser(MySqlConnection conn, string username, string password)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("insert into user set username = @username , password = @password", conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                int i = cmd.ExecuteNonQuery();
                if (i == 1) Console.WriteLine("注册成功");
            }
            catch (Exception e)
            {
                Console.WriteLine("在AddUser的时候出现异常:" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }
    }
}
