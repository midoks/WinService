using System;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Threading;
using IWshRuntimeLibrary;
using System.Collections;
using System.ComponentModel;

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
            CheckForIllegalCrossThreadCalls = false; 

            reloadList();

            //拦截标题栏的关闭事件
            this.Closing += new CancelEventHandler(Form_Closing);
        }

        private void Form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 取消关闭窗体
            e.Cancel = true;

            // 将窗体变为最小化
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false; //不显示在系统任务栏 
            notifyIcon_main.Visible = true;
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

                if (System.IO.File.Exists(pid_path)){
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

        public void start_bat() {
            int i = listProjectBox.SelectedIndex;
            XmlNode ch = this.iniXml.selectedNode(i);
            if (ch != null)
            {
                string v = ch.Attributes["dir"].Value;
                string cmd_start_bat = getDirPath(v);
                Wcmd(cmd_start_bat + "/start.bat");
            }
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
                    if (System.IO.File.Exists(cmd_start_bat + "/start.bat"))
                    {
                        Thread t1 = new Thread(new ThreadStart(start_bat));
                        t1.IsBackground = true;
                       
                        t1.Start();
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
                    if (System.IO.File.Exists(cmd_stop_bat + "/stop.bat"))
                    {
                        Wcmd(cmd_stop_bat + "/stop.bat");
                    }
                    else
                    {
                        MessageBox.Show("不存在启动脚本!!!");
                        return;
                    }
                    System.IO.File.Delete(pid_path);
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

                if (System.IO.File.Exists(pid_path))
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
            dir = getDirPath(dir);
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
                return BaseDir + "scripts/" + v;
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
            //log("sssss-----sss");
            //log(cmdtext + "sssss-----sss");
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
            string str = Tcmd.StandardOutput.ReadToEnd();
            log(str);
            Tcmd.Close();
        }

        //获取超级权限
        private void Wcmd64(string cmdtext)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.UseShellExecute = false;
            info.CreateNoWindow = true;
            info.RedirectStandardError = true;
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.WorkingDirectory = Environment.CurrentDirectory;
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
                if (System.IO.File.Exists(pid_path))
                {

                    string cmd_start_bat = getDirPath(v);
                    log(cmd_start_bat + "/restart.bat");
                    if (System.IO.File.Exists(cmd_start_bat + "/restart.bat"))
                    {
                        Wcmd(cmd_start_bat + "/restart.bat");
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

        private void button_m_icon_Click(object sender, EventArgs e)
        {
            string deskTop = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            if (System.IO.File.Exists(deskTop + "\\Services.lnk"))  //
            {
                System.IO.File.Delete(deskTop + "\\Services.lnk");//删除原来的桌面快捷键方式
                //return;
            }

            WshShell shell = new WshShell();

            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + "Services.lnk");
            shortcut.TargetPath = @Application.StartupPath + "\\Services.exe"; //目标文件
            shortcut.WorkingDirectory = System.Environment.CurrentDirectory;//该属性指定应用程序的工作目录，当用户没有指定一个具体的目录时，快捷方式的目标应用程序将使用该属性所指定的目录来装载或保存文件。
            shortcut.WindowStyle = 1; //目标应用程序的窗口状态分为普通、最大化、最小化【1,3,7】
            shortcut.Description = "Services"; //描述
            //shortcut.IconLocation = Application.StartupPath + "\\app.ico";  //快捷方式图标
            shortcut.Arguments = "";
            shortcut.Hotkey = "CTRL+ALT+F10"; // 快捷键
            shortcut.Save(); //必须调用保存快捷才成创建成功
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (checkedIsHasSelfRun())
            {
                MessageBox.Show("已经在运行了!!!");
                Application.Exit();
            }

        }

        //检查是自己已经运行了
        private bool checkedIsHasSelfRun()
        {
            string cmdtext = "tasklist | findstr Services.exe";
            Process Tcmd = new Process();
            Tcmd.StartInfo.FileName = "cmd.exe";//设定程序名
            Tcmd.StartInfo.UseShellExecute = false;//关闭Shell的使用 
            Tcmd.StartInfo.RedirectStandardInput = true;//重定向标准输入 
            Tcmd.StartInfo.RedirectStandardOutput = true;//重定向标准输出
            Tcmd.StartInfo.RedirectStandardError = true;//重定向错误输出
            Tcmd.StartInfo.CreateNoWindow = true;
            Tcmd.StartInfo.Arguments = "/C " + cmdtext;//设置不显示窗口
            Tcmd.Start();//执行VER命令 

            string s = Tcmd.StandardOutput.ReadToEnd();
            Tcmd.Close();

            string[] ss;
            ArrayList str = new ArrayList();
            if (s != null)
            {
                ss = s.Split('\n');
                foreach (string d in ss)
                {
                    if (d != "")
                    {
                        str.Add(d);
                    }
                }

                if (str.Count > 1)
                {
                    return true;
                }
            }
            return false;
        }

        private void notifyIcon_main_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
        }

        private void close_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

      
    }
}
