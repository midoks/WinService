using System;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace Services
{
    public partial class Form1 : Form
    {
        private static string BaseDir = System.AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/");

        //private SystemINI ini;
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
                string pn = r.FirstChild.Attributes["name"].Value;
                string pid_path = BaseDir + "pids/" + pn + ".pid";

                if (File.Exists(pid_path)){
                    button_start.Text = "stop";
                    textBox_PN.ReadOnly = true;
                }


                textBox_PN.Text = pn;
                listProjectBox.SelectedIndex = 0;
                listBox_show.Items.Add(r.FirstChild.Attributes["dir"].Value);
            }
        }

        public void log(string log)
        {
            Console.WriteLine(DateTime.Now.ToLocalTime().ToString() + "---------------------" + log);
        } 


        private void button_start_Click(object sender, EventArgs e)
        {
            int i = listProjectBox.SelectedIndex;
            XmlNode ch = this.iniXml.selectedNode(i);
            if (ch != null)
            {
                string pn = ch.Attributes["name"].Value;
                textBox_PN.Text = pn;
                string v = ch.Attributes["dir"].Value;

                string action_name = button_start.Text;
                string pid_path = BaseDir + "pids/" + pn + ".pid";
                if (action_name.Equals("start"))
                {
                    string cmd_start_bat = getDirPath(v);
                    log(cmd_start_bat + "/start.bat");
                    if (File.Exists(cmd_start_bat + "/start.bat"))
                    {
                        Wcmd(cmd_start_bat);
                    }
                    else 
                    {
                        MessageBox.Show("不存在启动脚本!!!");
                        return;
                    }

                    _WriteContent(pid_path, pn);
                    button_start.Text = "stop";
                    textBox_PN.ReadOnly = true;
                }
                else if ( action_name.Equals("stop") )
                {
                    string cmd_stop_bat = getDirPath(v);
                    log(cmd_stop_bat + "/stop.bat");
                    if (File.Exists(cmd_stop_bat + "/stop.bat"))
                    {
                        Wcmd(cmd_stop_bat);
                    }
                    else
                    {
                        MessageBox.Show("不存在启动脚本!!!");
                        return;
                    }
                    File.Delete(pid_path);
                    button_start.Text = "start";
                    textBox_PN.ReadOnly = false;
                }
            }
            else {
                MessageBox.Show("请选择项目!");
            }
        }


        private void project_add_Click(object sender, EventArgs e)
        {
            string PN = "PN" + (listProjectBox.Items.Count + 1).ToString();
            listProjectBox.Items.Add(PN);
            listProjectBox.SelectedIndex = listProjectBox.Items.Count - 1;

            this.iniXml.addNode(PN, "dir", PN);
            textBox_PN.Text = PN;
        }

        private void project_del_Click(object sender, EventArgs e)
        {
            if (listProjectBox.Items.Count > 0 )
            {
                this.iniXml.removeNode(listProjectBox.SelectedIndex);
                listProjectBox.Items.RemoveAt(listProjectBox.SelectedIndex);

                if (listProjectBox.Items.Count > 0)
                {
                    listProjectBox.SelectedIndex = listProjectBox.Items.Count - 1;
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
                this.iniXml.updateNode(listProjectBox.SelectedIndex, "dir", pn);
                listProjectBox.Items[listProjectBox.SelectedIndex] = pn;
                
            }
        }

        private void listProjectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = listProjectBox.SelectedIndex;
            XmlNode ch = this.iniXml.selectedNode(i);
            if (ch != null) {
                string pn = ch.Attributes["name"].Value;
                textBox_PN.Text = pn;

                string pid_path = BaseDir + "pids/" + pn + ".pid";

                if (File.Exists(pid_path))
                {
                    button_start.Text = "stop";
                    textBox_PN.ReadOnly = true;
                } else {
                    button_start.Text = "start";
                    textBox_PN.ReadOnly = false;
                }

                string v = ch.Attributes["dir"].Value;
                setFileDirShow(v);
            }
        }

        private void button_choose_Click(object sender, EventArgs e)
        {
            if ( listProjectBox.SelectedIndex > -1 ){
                FolderBrowserDialog dir = new FolderBrowserDialog();
                dir.RootFolder = Environment.SpecialFolder.Desktop;
                if (dir.ShowDialog() == DialogResult.OK)
                {
                    if (listProjectBox.SelectedIndex > -1)
                    {
                        this.iniXml.updateNode(listProjectBox.SelectedIndex, "dir", dir.SelectedPath);
                        listBox_show.Items[0] = dir.SelectedPath;
                    }
                }
            }
        }

        private void listBox_show_SelectedIndexChanged(object sender, EventArgs e)
        {}

        private void button_open_dir_Click(object sender, EventArgs e)
        {
            string dir = listBox_show.Items[0].ToString();
            if (Directory.Exists(dir))
            {
                System.Diagnostics.Process.Start(dir);
            }
            else {
                MessageBox.Show(dir + "目录不存在!");
            }
        }

        private string getDirPath(string v) {

            if (Directory.Exists(v))
            {
                return v;
            }
            else
            {
                return BaseDir + "/scripts/" + v;
            }
        }

        public void setFileDirShow(string v) {
            if (listBox_show.Items.Count > 0)
            {
               listBox_show.Items[0] = v;
            }
        }

        //写入内容
        private bool _WriteContent(string path, string content)
        {
            bool ok = false;
            System.IO.StreamWriter sw = new System.IO.StreamWriter(path, false, System.Text.Encoding.Default);
            try
            {
                sw.Write(content);
                ok = true;
                sw.Close();
            }
            catch { }
            return ok;
        }

        //读取内容
        private string _ReadContent(string path)
        {
            string str = System.IO.File.ReadAllText(path);
            return str;
        }

        //执行cmd命令
        private void Wcmd(string cmdtext)
        {
            if (isSystemType64())
            {
                Wcmd64(cmdtext);
            }
            else
            {
                Wcmd32(cmdtext);
            }
        }

        private void Wcmd32(string cmdtext)
        {
            Process Tcmd = new Process();
            Tcmd.StartInfo.FileName = "cmd.exe";//设定程序名 
            Tcmd.StartInfo.UseShellExecute = false;//关闭Shell的使用 
            Tcmd.StartInfo.RedirectStandardInput = true; //重定向标准输入 
            Tcmd.StartInfo.RedirectStandardOutput = true;//重定向标准输出 
            Tcmd.StartInfo.RedirectStandardError = true;//重定向错误输出 
            Tcmd.StartInfo.CreateNoWindow = true;//设置不显示窗口 
            Tcmd.StartInfo.Arguments = "/c " + cmdtext;
            Tcmd.StartInfo.Verb = "runas";
            //Tcmd.StandardInput.WriteLine("exit");
            //Tcmd.WaitForExit();
            Tcmd.Start();//执行VER命令 
            //string str = Tcmd.StandardOutput.ReadToEnd();
            //MessageBox.Show(str);
            Tcmd.Close();
        }

        //获取超级权限
        private void Wcmd64(string cmdtext)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.UseShellExecute = false;
            info.CreateNoWindow = true;
            //info.RedirectStandardError = true;
            //info.RedirectStandardInput = true;
            //info.RedirectStandardOutput = true;
            //info.WorkingDirectory = Environment.CurrentDirectory;
            info.FileName = "cmd.exe";
            info.Arguments = "/c " + cmdtext;
            info.Verb = "runas";
            Process.Start(info);
        }

        //获取当前运行环境
        //64位  true
        //32位  false
        public bool isSystemType64()
        {
            if (IntPtr.Size == 8)
            {
                //64 bit
                //MessageBox.Show("64"); 
                return true;
            }
            else if (IntPtr.Size == 4)
            {
                //32 bit
                //MessageBox.Show("32");
                return false;
            }
            else
            {
                //...NotSupport
            }
            return false;
        }

        private void restart_Click(object sender, EventArgs e)
        {
            int i = listProjectBox.SelectedIndex;
            XmlNode ch = this.iniXml.selectedNode(i);
            if (ch != null)
            {
                string pn = ch.Attributes["name"].Value;
                textBox_PN.Text = pn;
                string v = ch.Attributes["dir"].Value;

                string action_name = button_start.Text;
                string pid_path = BaseDir + "pids/" + pn + ".pid";
                if (File.Exists(pid_path))
                {

                    string cmd_start_bat = getDirPath(v);
                    log(cmd_start_bat + "/restart.bat");
                    if (File.Exists(cmd_start_bat + "/restart.bat"))
                    {
                        Wcmd(cmd_start_bat);
                    }
                    else
                    {
                        MessageBox.Show("不存在启动脚本!!!");
                        return;
                    }

                    _WriteContent(pid_path, pn);
                    button_start.Text = "stop";
                    textBox_PN.ReadOnly = true;
                }
                else {
                    MessageBox.Show("项目未启动!");
                }
            }
            else
            {
                MessageBox.Show("请选择项目!");
            }


        }

      
    }
}
