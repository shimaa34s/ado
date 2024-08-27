using BLL;

namespace allado
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var res = StudentManger.GetStudentList();
            foreach (var student in res) {
                Console.WriteLine(student);
            }
            Console.WriteLine("enter student id ");
            int id = int.Parse(Console.ReadLine());
            //Console.WriteLine("enter student name");
            //string name = Console.ReadLine();
            //Console.WriteLine("enter student age ");
            //int age = int.Parse(Console.ReadLine());
            //Console.WriteLine("enter student dept_id ");
            //int dept_id = int.Parse(Console.ReadLine());
            // var res1 = StudentManger.delete(id);
            var res1 = StudentManger.delete2(id);

            if (res1==1)
            {
                //   Console.WriteLine("added");
                // Console.WriteLine("updated");
                Console.WriteLine("deleted");
            }
            else
            {
                Console.WriteLine("error");
            }

        }
    }
}
