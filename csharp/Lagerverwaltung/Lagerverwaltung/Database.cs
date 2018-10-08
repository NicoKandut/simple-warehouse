using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lagerverwaltung
{
    public class Database
    {
        public static string connString { get; set; }
        public static OracleConnection conn { get; set; }
        public static void connect(string _connString)
        {
            connString = _connString;
            conn = new OracleConnection(connString);
            conn.Open();
        }
        public static void login(string username, string password)
        {
            try
            {
                string select = "select * from sw_owner where name like '" + username + "' and password like '" + password + "'";
                OracleCommand cmd = new OracleCommand(select, conn);
                OracleDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    throw new Exception("Wrong username or password!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void register(string username, string password)
        {
            try
            {
                string select = "insert into sw_owner values(seq_owner.nextval, '" + username + "', '" + password + "')";
                OracleCommand cmd = new OracleCommand(select, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void closeConnection()
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
    }
}
