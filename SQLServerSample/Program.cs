using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SQLServerSample
{
    class Program
    {
        public static SqlConnection conn;
        private const string connectionString = "Data Source=192.168.2.210;Pooling=False;Max Pool Size = 1024;Initial Catalog=Amadeus51;Persist Security Info=True;User ID=sa;Password=1";

        //打开数据库连接
        public static void OpenConn(string strConnection)
        {
            conn = new SqlConnection(strConnection);
            if (conn.State.ToString().ToLower() == "open")
            {

            }
            else
            {
                conn.Open();
            }
        }

        //关闭数据库连接
        public static void CloseConn()
        {
            if (conn.State.ToString().ToLower() == "open")
            {
                //连接打开时
                conn.Close();
                conn.Dispose();
            }
        }


        // 读取数据
        public static SqlDataReader GetDataReaderValue(string sql)
        {
            //OpenConn(connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader dr = cmd.ExecuteReader();
           // CloseConn();
            return dr;
        }


        // 返回DataSet
        public DataSet GetDataSetValue(string sql, string tableName)
        {
            OpenConn(connectionString);
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            da = new SqlDataAdapter(sql, conn);
            da.Fill(ds, tableName);
            CloseConn();
            return ds;
        }

        //  返回DataView
        public DataView GetDataViewValue(string sql)
        {
            OpenConn(connectionString);
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            da = new SqlDataAdapter(sql, conn);
            da.Fill(ds, "temp");
            CloseConn();
            return ds.Tables[0].DefaultView;
        }

        // 返回DataTable
        public DataTable GetDataTableValue(string sql)
        {
            OpenConn(connectionString);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.Fill(dt);
            CloseConn();
            return dt;
        }

        // 执行一个SQL操作:添加、删除、更新操作
        public void ExecuteNonQuery(string sql)
        {
            OpenConn(connectionString);
            SqlCommand cmd;
            cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            CloseConn();
        }

        // 执行一个SQL操作:添加、删除、更新操作，返回受影响的行
        public int ExecuteNonQueryCount(string sql)
        {
            OpenConn(connectionString);
            SqlCommand cmd;
            cmd = new SqlCommand(sql, conn);
            int value = cmd.ExecuteNonQuery();
            return value;
        }

        //执行一条返回第一条记录第一列的SqlCommand命令
        public object ExecuteScalar(string sql)
        {
            OpenConn(connectionString);
            SqlCommand cmd;
            cmd = new SqlCommand(sql, conn);
            object value = cmd.ExecuteScalar();
            return value;
        }

        // 返回记录数
        public int SqlServerRecordCount(string sql)
        {
            OpenConn(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            int RecordCount = 0;
            while (dr.Read())
            {
                RecordCount = RecordCount + 1;
            }
            CloseConn();
            return RecordCount;
        }
        static void Main(string[] args)
        {
            string strSQL="SELECT dbo.CRDHLD.ID,dbo.CRDHLD.Last_Name,dbo.CRDHLD.First_Name,dbo.CRDHLD.From_Date,dbo.CRDHLD.Valid,dbo.Card.Code FROM  dbo.CRDHLD INNER JOIN dbo.Card ON dbo.CRDHLD.ID = dbo.Card.Owner ";
            OpenConn(connectionString);
            SqlDataReader dr = GetDataReaderValue(strSQL);
             while (dr.Read())
                {
                    Console.Write(dr["id"].ToString()+",");
                    Console.Write(dr["Last_Name"].ToString() + ",");
                    Console.Write(dr["First_Name"].ToString() + ",");
                    Console.Write(dr["From_Date"].ToString() + ",");
                    Console.WriteLine(dr["Code"].ToString());
                }
                dr.Close();

        }
    }
}
