using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Dept_id { get; set; }

        public string DeptName { get; set; }

        public override string ToString()
        {
            return $"{Id} : {Name} : {Age} : {Dept_id}";
        }

    }
}
