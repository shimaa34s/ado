using BLL;

namespace form1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var res = StudentManger.GetStudentList();
            listBox1.DataSource = res;
            listBox1.DisplayMember = "Name";
            listBox1.ValueMember = "id";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int z = StudentManger.Add(textBox1.Text,int.Parse(textBox2.Text),int.Parse(textBox3.Text));
            if (z == 1)
            {
                MessageBox.Show("added");
            }
            else
            {
                MessageBox.Show("error");

            }
        }
    }
}
