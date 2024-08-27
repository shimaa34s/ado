using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    internal class Depertment
    {
        public int Dept_Id { get; set; }
        public string Dept_Name { get; set; }
        public int capacity { get; set; }
        public override string ToString()
        {
            return $"{Dept_Id} : {Dept_Name} : {capacity}";
        }
    }
}
