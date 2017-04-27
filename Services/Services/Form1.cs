using System;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace Services
{
    public partial class Form1 : Form
    {
        private static string BaseDir = System.AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/");

        private SystemINI ini;
        private SystemXml iniXml;

        public Form1()
        {
            InitializeComponent();

            //this.ini = new SystemINI(BaseDir + "service.ini");
            this.iniXml = new SystemXml(BaseDir + "service.xml");

            reloadList();
        }

        public void reloadList() {
            XmlNode r = this.iniXml.rootNode();

            listProjectBox.Items.Clear();
            int i = 0;
            foreach (XmlNode r_one in r)
            {
                listProjectBox.Items.Add(r_one.Attributes["name"].Value);
                i++;
            }

            if (i > 0)
            {
                textBox_PN.Text = r.FirstChild.Attributes["name"].Value;
                listProjectBox.SelectedIndex = 0;
            }
        }

        public void log(string log)
        {
            Console.WriteLine(DateTime.Now.ToLocalTime().ToString() + "---------------------" + log);
        } 


        private void button_start_Click(object sender, EventArgs e)
        {
            log("start");
            this.iniXml.rootNode();
        }


        private void project_add_Click(object sender, EventArgs e)
        {
            string PN = "PN" + (listProjectBox.Items.Count + 1).ToString();
            listProjectBox.Items.Add(PN);
            listProjectBox.SelectedIndex = listProjectBox.Items.Count - 1;

            this.iniXml.addNode(PN, "dir", "test");
            textBox_PN.Text = PN;
        }

        private void project_del_Click(object sender, EventArgs e)
        {
            if (listProjectBox.Items.Count > 0 )
            {
                this.iniXml.removeNode(listProjectBox.SelectedIndex);
                listProjectBox.Items.RemoveAt(listProjectBox.SelectedIndex);

                if (listProjectBox.Items.Count > 0) {
                    listProjectBox.SelectedIndex = 0;
                }

            }
            else {
                if (listProjectBox.Items.Count > 0)
                {
                    listProjectBox.SelectedIndex = 0;
                    MessageBox.Show("没有选择!!!");
                }
                else {
                    MessageBox.Show("没有数据!!!");
                }           
            }
        }

        private void textBox_PN_TextChanged(object sender, EventArgs e)
        {
            string pn = textBox_PN.Text;
            if ( listProjectBox.SelectedIndex > -1 ){
                this.iniXml.updateNode(listProjectBox.SelectedIndex, "name", pn);
                listProjectBox.Items[listProjectBox.SelectedIndex] = pn;
            }
        }

        private void listProjectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = listProjectBox.SelectedIndex;
            XmlNode ch = this.iniXml.selectedNode(i);
            if (ch != null) {
                textBox_PN.Text = ch.Attributes["name"].Value;
            }
        }

        private void button_choose_Click(object sender, EventArgs e)
        {
            if ( listProjectBox.SelectedIndex > -1 ){
                FolderBrowserDialog dir = new FolderBrowserDialog();
                dir.RootFolder = Environment.SpecialFolder.Desktop;
                if (dir.ShowDialog() == DialogResult.OK)
                {
                    log("ffff: " + dir.SelectedPath);
                    if (listProjectBox.SelectedIndex > -1)
                    {
                        this.iniXml.updateNode(listProjectBox.SelectedIndex, "dir", dir.SelectedPath);
                    }
                }
            }
        }

      
    }
}
