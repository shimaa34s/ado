using DA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DA;
using Microsoft.Data.SqlClient;

namespace BLL
{
    public static class StudentManger
    {
        public static StudentList GetStudentList()
        {
            DAManager da = new DAManager();
            string sql = "select * from students";
           DataTable dt= da.GetAll(sql);
            var res= converfromDTtoSL(dt);
            return res;
        }

        public static int Add(string name,int age,int dept_id)
        {
            DAManager da = new DAManager();
            string sql = $"insert into students (name,age,dept_id) values('{name}',{age},{dept_id})";
            int x= da.Executenonq(sql);
            return x;

        }
        public static int Add2(string name, int age, int dept_id)
        {
            DAManager da = new DAManager();
            string sql = $"insert into students (name,age,dept_id) values(@name,@age,@dept_id)";
            List<SqlParameter> l = new List<SqlParameter>();
            
            SqlParameter param1 = new SqlParameter()
            {
                ParameterName = "name",
                Value = name,
                Direction = System.Data.ParameterDirection.Input,
                SqlDbType = SqlDbType.NChar,
            };
            l.Add(param1);
            SqlParameter param2 = new SqlParameter()
            {
                ParameterName = "age",
                Value = age,
                Direction = System.Data.ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
            };
            l.Add(param2);
            SqlParameter param3 = new SqlParameter()
            {
                ParameterName = "dept_id",
                Value = dept_id,
                Direction = System.Data.ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
            };
            l.Add(param3);
            int x = da.Executenonq(sql,l);

            return x;

        }
        public static int update(int id,string name, int age, int dept_id)
        {
            DAManager da = new DAManager();
            string sql = $"update students set name='{name}',age={age},dept_id={dept_id} where id={id}";
            int x = da.Executenonqupdate(sql);
            return x;

        }
        public static int update2(int id, string name, int age, int dept_id)
        {
            DAManager da = new DAManager();
            string sql = $"update students set name=@name,age=@age,dept_id=@dept_id where id=@id";
            List<SqlParameter> l = new List<SqlParameter>();

            SqlParameter param1 = new SqlParameter()
            {
                ParameterName = "name",
                Value = name,
                Direction = System.Data.ParameterDirection.Input,
                SqlDbType = SqlDbType.NChar,
            };
            l.Add(param1);
            SqlParameter param2 = new SqlParameter()
            {
                ParameterName = "age",
                Value = age,
                Direction = System.Data.ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
            };
            l.Add(param2);
            SqlParameter param3 = new SqlParameter()
            {
                ParameterName = "dept_id",
                Value = dept_id,
                Direction = System.Data.ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
            };
            l.Add(param3);
            SqlParameter param4 = new SqlParameter()
            {
                ParameterName = "id",
                Value = id,
                Direction = System.Data.ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
            };
            l.Add(param4);
            int x = da.Executenonqupdate(sql,l);
            return x;

        }
        public static int delete(int id)
        {
            DAManager da = new DAManager();
            string sql = $"delete from students where id={id}";
            int x = da.Executenonqdelete(sql);
            return x;

        }
        public static int delete2(int id)
        {
            DAManager da = new DAManager();
            string sql = "delete from students where id=@id";
            List<SqlParameter> l = new List<SqlParameter>();

            SqlParameter param1 = new SqlParameter()
            {
                ParameterName = "id",
                Value = id,
                Direction = System.Data.ParameterDirection.Input,
                SqlDbType = SqlDbType.NChar,
            };
            l.Add(param1);
            int x = da.Executenonqdelete2(sql,l);
            return x;

        }
        internal static StudentList converfromDTtoSL(DataTable dt)
        {
            StudentList studentList = new StudentList();
            foreach (DataRow dr in dt.Rows) 
            {
                studentList.Add(converfromDTtoD(dr));
            }
            return studentList;
        }
        internal static Student converfromDTtoD(DataRow dr)
        {
            return new Student()
            {
                Id=(int) dr["ID"],
                Name = dr[1].ToString(),
                Age = (int)dr[2],
                Dept_id = (int)dr[3]
            };
        }

    }
}
