/*
* Copyright(C) 2012 Giovanni Capuano <webmaster@giovannicapuano.net>
*
* JaqaacGUI is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*
* JaqaacGUI is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with JaqaacGUI. If not, see <http:*www.gnu.org/licenses/>.
*/

using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace JaqaacGUI
{
    public partial class Form1 : Form
    {
        List<string> audio;
        string       output, qaac, eac3to, windows;

        private string[] SelectFile(string initialDirectory, string filter, string title, bool multiselect = false)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = filter;
            dialog.InitialDirectory = initialDirectory;
            dialog.Title = title;
            dialog.Multiselect = multiselect;
            if (dialog.ShowDialog() == DialogResult.OK)
                if (multiselect)
                    return dialog.FileNames;
                else
                    return new string[] { File.Exists(dialog.FileName) ? dialog.FileName : "" };
            else
                if(multiselect)
                    return new string[] { };
                else
                    return new string[] { "" };
        }

        private string SelectFolder(Environment.SpecialFolder initialDirectory)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.RootFolder = initialDirectory;
            return dialog.ShowDialog() == DialogResult.OK ? dialog.SelectedPath : "";
        }

        private string SelectFolder()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            return dialog.ShowDialog() == DialogResult.OK ? dialog.SelectedPath : "";
        }

        private void CommandExecute(string cmd)
        {
            string file = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\JaqaacGUI.bat";
            StreamWriter writer = new StreamWriter(file);
            writer.WriteLine(cmd);
            writer.Close();
            Process p = new Process();
            p.StartInfo.FileName = file;
            p.Start();
            p.WaitForExit();
            if (File.Exists(file))
                File.Delete(file);
        }

        public Form1()
        {
            InitializeComponent();
            audio = new List<string>();
            output = "";
            qaac = "";
            eac3to = "";
            windows = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (audio.Count == 0)
            {
                MessageBox.Show("No file audio has been chosen.");
                return;
            }
           
            if (output == "" || !Directory.Exists(output))
            {
                MessageBox.Show("Output folder doesn't found.");
                return;
            }

            if (qaac == "")
                qaac = File.Exists(windows + "\\qaac.exe") ? windows + "\\qaac.exe" : SelectFile(windows, "Executable (*.exe)|*.exe|All files (*.*)|*.*", "Select an executable for qaac")[0];
           
            if (qaac == "" || !File.Exists(qaac)) {
                MessageBox.Show("qaac executable doesn't found.");
                return;
            }

            if (MessageBox.Show("Can I send the commands?", "Send the commands?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            StringBuilder buffer = new StringBuilder();
            buffer.Append("\"" + qaac + "\" ");

            /* AAC Mode and quality/bitrate */
            string encoding = "";
            switch (comboBox1.SelectedItem.ToString())
            {
                case "ABR":
                    encoding = "-a";
                    break;
                case "TVBR":
                    encoding = "-V";
                    break;
                case "CVBR":
                    encoding = "-v";
                    break;
                case "CBR":
                    encoding = "-c";
                    break;
                case "ALAC":
                    encoding = "-A";
                    break;
            }
            buffer.Append(encoding);
            if(numericUpDown1.Enabled)
                buffer.Append(" " + numericUpDown1.Value.ToString());

            /* Flags */
            if(checkBox1.Checked && checkBox1.Enabled)
                buffer.Append(" --he");
            if (checkBox2.Checked && checkBox2.Enabled)
                buffer.Append(" --ignorelength");
            if (checkBox3.Checked && checkBox3.Enabled)
                buffer.Append(" --raw");
            if(checkBox4.Checked && checkBox4.Enabled)
                buffer.Append(" --raw-channels " + numericUpDown2.Value);
            if (checkBox5.Checked && checkBox5.Enabled)
                buffer.Append(" --threading");

            /* Additional commands */
            if (textBox1.Text != "")
                buffer.Append(" " + textBox1.Text);

            /* Input and output files */
            for (int i = 0, length = audio.Count; i < length; ++i)
            {
                string cmd;
                if (audio[i].EndsWith(".m2ts"))
                {
                    if(eac3to == "")
                        eac3to = File.Exists(windows + "\\eac3to.exe") ? windows + "\\eac3to.exe" : SelectFile(windows, "Executable (*.exe)|*.exe|All files (*.*)|*.*", "Select an executable for eac3to")[0];

                    if (eac3to == "" || !File.Exists(eac3to))
                    {
                        MessageBox.Show("eac3to executable doesn't found.");
                        return;
                    }

                    string output_wav = output + "\\" + Path.GetFileNameWithoutExtension(audio[i]) + ".m4a";
                    cmd = String.Format("\"{0}\" \"{1}\" 2: stdout.wav | ", eac3to, audio[i]) +
                          buffer.ToString() + String.Format(" - -o \"{0}\"", output_wav);
                }
                else
                    cmd = buffer.ToString() + String.Format(" \"{0}\" -d \"{1}\"", audio[i], output);
                
                CommandExecute(cmd);
            }

            audio.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] files = SelectFile(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "FLAC (*.flac)|*.flac|L16 (*.l16)|*.l16|WAV (*.wav)|*.wav|AIFF (*.aiff)|(*.aiff)|AU (*.au)|*.au|PCM (*.pcm)|*.pcm|M2TS (*.m2ts)|*m2ts", "Select a file audio", true);
            
            foreach(string f in files)
                if (File.Exists(f))
                    audio.Add(f);                
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "TVBR")
            {
                label2.Text = "Quality";
                numericUpDown1.Maximum = 127;
                checkBox1.Enabled = false;

                label2.Visible = true;
                numericUpDown1.Visible = true;
                numericUpDown1.Enabled = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "ALAC")
            {
                label2.Visible = false;
                numericUpDown1.Visible = false;
                numericUpDown1.Enabled = false;
            }
            else
            {
                label2.Text = "Bitrate";
                numericUpDown1.Maximum = 320;
                checkBox1.Enabled = true;

                label2.Visible = true;
                numericUpDown1.Visible = true;
                numericUpDown1.Enabled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {  
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            checkBox4.Enabled = checkBox3.Checked;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown2.Enabled = checkBox4.Checked;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/RoxasShadow"); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            output = SelectFolder();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://omnivium.it"); 
        }
    }
}
