using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DA
{
    public class DAManager
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataTable dt;
        public DAManager() {
            string cnnstr = new ConfigurationBuilder()
           .AddJsonFile("D:\\ITI1\\oop\\itp\\Material\\allado\\DA\\AppSettings.json")
            .Build()
            .GetSection("constr").Value;
            con=new SqlConnection(cnnstr);
        }
        public DataTable GetAll(string stm)
        {
            cmd=new SqlCommand(stm,con);
            adapter=new SqlDataAdapter(cmd);
            dt=new DataTable();
            adapter.Fill(dt);
            return dt;
        }
        public int Executenonq(string sql) 
        {
            cmd=new SqlCommand(sql,con);
            con.Open();
            int x=cmd.ExecuteNonQuery();
            con.Close();
            return x;

        }
        public int Executenonq(string sql, List<SqlParameter> l)
        {
            cmd = new SqlCommand(sql, con);
            // Console.WriteLine(l.Count());
            for (int i = 0; i < l.Count; i++)
            {
                cmd.Parameters.Add(l[i]);
            }
            con.Open();
            int x = cmd.ExecuteNonQuery();
            con.Close();
            return x;

        }

        public int Executenonqupdate(string sql)
        {
            cmd = new SqlCommand(sql, con);
            con.Open();
            int x = cmd.ExecuteNonQuery();
            con.Close();
            return x;

        }
        public int Executenonqupdate(string sql,List<SqlParameter> l)
        {
            cmd = new SqlCommand(sql, con);
            con.Open();
            for (int i = 0; i < l.Count; i++)
            {
                cmd.Parameters.Add(l[i]);
            }
            int x = cmd.ExecuteNonQuery();
            con.Close();
            return x;

        }

        public int Executenonqdelete(string sql)
        {
            cmd = new SqlCommand(sql, con);
            con.Open();
            int x = cmd.ExecuteNonQuery();
            con.Close();
            return x;

        }
        public int Executenonqdelete2(string sql, List<SqlParameter> l)
        {
            cmd = new SqlCommand(sql, con);
            con.Open();

            for (int i = 0; i < l.Count; i++)
            {
                cmd.Parameters.Add(l[i]);
            }
            int x = cmd.ExecuteNonQuery();
            con.Close();
            return x;

        }


    }
}
