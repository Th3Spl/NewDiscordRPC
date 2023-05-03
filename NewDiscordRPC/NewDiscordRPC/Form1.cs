using DiscordRPC;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace NewDiscordRPC
{
    public partial class Form1 : Form
    {
        //These are for the navbar
        bool mouseDown;
        private Point offset;

        //Globals
        DiscordRpcClient client;
        RichPresence presence;
        bool timestamp = false;
        bool button = false;

        //Options
        string applicationId = null;
        string details = null;
        string state = null;
        string largeImageKey = string.Empty;
        string largeImageText = string.Empty;
        string smallImageKey = string.Empty;
        string smallImageText = string.Empty;
        string buttonLabel = null;
        string buttonUrl = null;
        private object applicationIdTextBox;

        public Form1()
        {
            InitializeComponent();
        }

        //Close button
        private void XButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Minimize button
        private void minimizeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //Navbar movement

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            offset.X = e.X;
            offset.Y = e.Y;
            mouseDown = true;
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown == true)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - offset.X, currentScreenPos.Y - offset.Y);
            }
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }


        //rjToggleBox1 CheckedChanged
        private void rjToggleButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (onOff1.Text == "Off")
            {
                timestamp = true;
                onOff1.Text = "On";
            }
            else
            {
                timestamp = false;
                onOff1.Text = "Off";
            }
        }

        //rjToggleBox2 CheckedChanged
        private void rjToggleButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (onOff2.Text == "Off")
            {
                button = true;
                onOff2.Text = "On";
            }
            else
            {
                button = false;
                onOff2.Text = "Off";
            }
        }

        //startButton Clicked event
        private void startButton_Click(object sender, EventArgs e)
        {
            startPresence();
        }


        //This function will take all the textbox values and then it will assign them to the variables
        private bool assignValuesToTheVariables(bool swapCheckMethod)
        {
            //Set the vars

            if (swapCheckMethod == true)
            {
                if (applicationidTextBox.Texts.Length > 0)
                {
                    applicationId = applicationidTextBox.Texts;
                }
                else
                {
                    MessageBox.Show("Error the application id must be a real application id!");
                    return false;
                }

                if (string.IsNullOrEmpty(stateTextBox.Texts) == false)
                {
                    state = stateTextBox.Texts;
                }

                if (string.IsNullOrEmpty(detailsTextBox.Texts) == false)
                {
                    details = detailsTextBox.Texts;
                }

                if (string.IsNullOrEmpty(largeImageKeyTextBox.Texts) == false)
                {
                    largeImageKey = largeImageKeyTextBox.Texts;
                }

                if (string.IsNullOrEmpty(largeImageTextTextBox.Texts) == false)
                {
                    largeImageText = largeImageTextTextBox.Texts;
                }

                if (string.IsNullOrEmpty(smallImageKeyTextBox.Texts) == false)
                {
                    smallImageKey = smallImageKeyTextBox.Texts;
                }

                if (string.IsNullOrEmpty(largeImageTextTextBox.Texts) == false)
                {
                    smallImageText = smallImageTextTextBox.Texts;
                }

                if (string.IsNullOrEmpty(buttonLabelTextBox.Texts) == false)
                {
                    buttonLabel = buttonLabelTextBox.Texts;
                }

                if (string.IsNullOrEmpty(buttonUrl = buttonUrlTextBox.Texts) == false)
                {
                    buttonUrl = buttonUrlTextBox.Texts;
                }

            }
            else
            {
                if (applicationId.Length > 0)
                {
                    applicationidTextBox.Texts = applicationId;
                }else
                {
                    return false;
                }

                detailsTextBox.Texts = details;
                stateTextBox.Texts = state;

                largeImageKeyTextBox.Texts = largeImageKey;
                largeImageTextTextBox.Texts = largeImageText;

                smallImageKeyTextBox.Texts = smallImageKey;
                smallImageTextTextBox.Texts = smallImageText;

                if (timestamp == true)
                {
                    rjToggleButton1.Checked = true;
                }else
                {
                    rjToggleButton1.Checked = false;
                }

                if (button == true)
                {
                    rjToggleButton2.Checked = true;
                    buttonLabelTextBox.Texts = buttonLabel;
                    buttonUrlTextBox.Texts = buttonUrl;
                }else
                {
                    rjToggleButton2.Checked = false;
                }
            }

            return true;
        }

        //This function will set and start the discordRPC
        private void startPresence()
        {
            if (assignValuesToTheVariables(true) == true)
            {


                try
                {
                    client = new DiscordRpcClient(applicationId);
                    client.Initialize();

                    presence = new RichPresence();

                    if (state != null) { presence.State = state; };
                    if (details != null) { presence.Details = details; };


                    if (largeImageKey != null) { presence.Assets = new Assets() { LargeImageKey = largeImageKey }; };
                    if (largeImageText != null) { presence.Assets = new Assets() { LargeImageText = largeImageText }; };
                    if (smallImageKey != null) { presence.Assets = new Assets() { SmallImageKey = smallImageKey }; };
                    if (smallImageText != null) { presence.Assets = new Assets() { SmallImageText = smallImageText }; };

                    if (timestamp == true)
                    {
                        presence.Timestamps = new Timestamps();
                        presence.Timestamps.Start = DateTime.UtcNow.ToLocalTime();
                        presence.Timestamps.End = null;
                    };

                    if (button == true)
                    {
                        if (buttonLabel != null && buttonUrl != null)
                        {
                            presence.Buttons = new DiscordRPC.Button[]
                            {
                        new DiscordRPC.Button()
                        {
                            Label = buttonLabel,
                            Url = buttonUrl
                        }
                            };
                        }
                    }

                    client.SetPresence(presence);
                }
                catch (Exception e)
                {
                    MessageBox.Show("An error has occurred while trying to start the discordRPC!\nCheck again your settings!");
                }
            }
            else
            {
                MessageBox.Show("An error has occurred while trying to get the application id! \n Check again and try again");
            }
        }

        private void presetsButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "DAT files (*.dat)|*.dat";
            dialog.Title = "Select a preset (.dat)";
            dialog.FilterIndex = 1;
            dialog.Multiselect = false;

            string presetPath = string.Empty;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                presetPath = dialog.FileName;
            }

            if (presetPath != string.Empty)
            {
                string[] lines = File.ReadAllLines(presetPath);

                foreach (string line in lines)
                {
                    if (line.Contains("AppId=") == true)
                    {
                        applicationId = line.Substring(6);
                    }
                    else if (line.Contains("Details=") == true)
                    {
                        details = line.Substring(8);
                    }
                    else if (line.Contains("State=") == true)
                    {
                        state = line.Substring(6);
                    }
                    else if (line.Contains("LargeImageKey=") == true)
                    {
                        largeImageKey = line.Substring(14);
                    }
                    else if (line.Contains("LargeImageText=") == true)
                    {
                        largeImageText = line.Substring(15);
                    }
                    else if (line.Contains("SmallImageKey=") == true)
                    {
                        smallImageKey = line.Substring(14);
                    }
                    else if (line.Contains("SmallImageText=") == true)
                    {
                        smallImageText = line.Substring(15);
                    }
                    else if (line.Contains("Timestamp=") == true)
                    {
                        if (line.Substring(10) == "1")
                        {
                            timestamp = true;
                        }
                        else
                        {
                            timestamp = false;
                        }
                    }
                    else if (line.Contains("Button=") == true)
                    {
                        if (line.Substring(7) == "1")
                        {
                            button = true;
                        }
                        else
                        {
                            button = false;
                            break;  // <-- We can stop the cycle cuz we don't need the next 2 options
                        }
                    }
                    else if (line.Contains("ButtonLabel=") == true)
                    {
                        buttonLabel = line.Substring(12);
                    }
                    else if (line.Contains("ButtonURL=") == true)
                    {
                        buttonUrl = line.Substring(10);
                    }
                }
                assignValuesToTheVariables(false);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Title = "Select a location and a name for the file that you're gonna save";
            string content;

            if (timestamp == true && button == true)
            {
                content = "AppId=" + applicationId +
                    "\nDetails=" + details +
                    "\nState=" + state +
                    "\nLargeImageKey=" + largeImageKey +
                    "\nLargeImageText=" + largeImageText +
                    "\nSmallImageKey=" + smallImageKey +
                    "\nSmallImageText=" + smallImageText +
                    "\nTimestamp=1" +
                    "\nButton=1" +
                    "\nButtonLabel=" + buttonLabel +
                    "\nButtonURL=" + buttonUrl;
            }
            else if (timestamp == true && button == false)
            {
                content = "AppId=" + applicationId +
                    "\nDetails=" + details +
                    "\nState=" + state +
                    "\nLargeImageKey=" + largeImageKey +
                    "\nLargeImageText=" + largeImageText +
                    "\nSmallImageKey=" + smallImageKey +
                    "\nSmallImageText=" + smallImageText +
                    "\nTimestamp=1" +
                    "\nButton=0" +
                    "\nButtonLabel=" + buttonLabel +
                    "\nButtonURL=" + buttonUrl;
            }
            else if (timestamp == false && button == true)
            {
                content = "AppId=" + applicationId +
                    "\nDetails=" + details +
                    "\nState=" + state +
                    "\nLargeImageKey=" + largeImageKey +
                    "\nLargeImageText=" + largeImageText +
                    "\nSmallImageKey=" + smallImageKey +
                    "\nSmallImageText=" + smallImageText +
                    "\nTimestamp=0" +
                    "\nButton=1" +
                    "\nButtonLabel=" + buttonLabel +
                    "\nButtonURL=" + buttonUrl;
            }
            else
            {
                content = "AppId=" + applicationId +
                    "\nDetails=" + details +
                    "\nState=" + state +
                    "\nLargeImageKey=" + largeImageKey +
                    "\nLargeImageText=" + largeImageText +
                    "\nSmallImageKey=" + smallImageKey +
                    "\nSmallImageText=" + smallImageText +
                    "\nTimestamp=0" +
                    "\nButton=0" +
                    "\nButtonLabel=" + buttonLabel +
                    "\nButtonURL=" + buttonUrl;
            }


            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using(Stream s = File.Open(dialog.FileName, FileMode.CreateNew))
                using(StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write(content);
                    sw.Flush();
                    sw.Close();
                }
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void rjButton1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            notifyIcon1.Visible = true;
            Hide();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void rjButton2_Click(object sender, EventArgs e)
        {
            try
            {
                client.Dispose();
            }catch (Exception ex)
            {
                MessageBox.Show("You need to start a RPC before closing one!");
            }

        }

        private void rjButton3_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.com/developers/applications");
        }
    }
}