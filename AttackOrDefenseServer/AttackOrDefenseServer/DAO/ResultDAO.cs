using System;
using AttackOrDefenseServer.Model;
using MySql.Data.MySqlClient;

namespace AttackOrDefenseServer.DAO
{
    class ResultDAO
    {
        public Result GetResultByUserid(MySqlConnection conn, int userId)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from result where userid = @userid", conn);
                cmd.Parameters.AddWithValue("userid", userId);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    int totalCount = reader.GetInt32("totalcount");
                    int winCount = reader.GetInt32("wincount");
                    Result res = new Result(id, userId, totalCount, winCount);
                    return res;
                }
                else
                {
                    Result res = new Result(-1, userId, 0, 0);
                    return res;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在GetResultByUserid的时候出现异常:" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return null;
        }
        public void UpdateOrAddResult(MySqlConnection conn, Result result)
        {
            try
            {
                MySqlCommand cmd = null;
                if (result.Id <= -1)
                {
                    cmd = new MySqlCommand("insert into result set totalcount = @totalcount,wincount = @wincount,userid = @userid", conn);
                }
                else
                {
                    cmd = new MySqlCommand("update result set totalcount = @totalcount,wincount = @wincount where userid = @userid", conn);
                }
                cmd.Parameters.AddWithValue("userid", result.UserId);
                cmd.Parameters.AddWithValue("totalcount", result.TotalCount);
                cmd.Parameters.AddWithValue("wincount", result.WinCount);
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.WriteLine("在UpdateOrAddResult的时候出现异常:" + e);
            }
        }
    }
}

