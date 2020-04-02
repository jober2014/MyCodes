using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using vlcPlayer;

namespace vlcPlayer
{

    public partial class PlayerWindow : Form
    {

        private bool is_playinig_;
        private readonly string pluginPath = System.Environment.CurrentDirectory + "\\vlc\\plugins\\";
        private static VlcPlayer vlc_player_;
        public PlayerWindow(String path)
        {
           
            InitializeComponent();
            vlc_player_ = new VlcPlayer(pluginPath);
            IntPtr render_wnd = this.panel1.Handle;
            vlc_player_.SetRenderWindow((int)render_wnd);
            is_playinig_ = false;
            this.trackBar2.Maximum = vlc_player_.GetVolume();
            int vol = vlc_player_.GetVolume();
            this.trackBar2.Value = vol / 2;
            vlc_player_.SetVolume(vol / 2);
            if (!string.IsNullOrEmpty(path))
            {
                FormPlay(path);
            }
            this.AllowDrop = true;
            tbVideoTime.Text = "00:00:00/00:00:00";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FormPlay(ofd.FileName);

            }
        }

        public void FormPlay(string path)
        {
            if (vlc_player_ == null)
            {
                vlc_player_ = new VlcPlayer(pluginPath);
            }

            if (!string.IsNullOrEmpty(path))
            {
                vlc_player_.PlayFile(path);
                trackBar1.SetRange(0, (int)vlc_player_.Duration());
                trackBar1.Value = 0;
                timer1.Start();
                is_playinig_ = true;
                this.Activate();
                button2.Text = "暂停";

            }


        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            if (is_playinig_)
            {
                vlc_player_.Stop();
                trackBar1.Value = 0;
                timer1.Stop();
                is_playinig_ = false;
                button2.Text = "播放";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (is_playinig_)
            {
                if (trackBar1.Value == trackBar1.Maximum)
                {
                    vlc_player_.Stop();
                    timer1.Stop();
                    WIN32api.SetTaskbarState(WIN32api.AppBarStates.AlwaysOnTop);
                }
                else
                {
                    trackBar1.Value = trackBar1.Value + 1;
                    tbVideoTime.Text = string.Format("{0}/{1}",
                        GetTimeString(trackBar1.Value),
                        GetTimeString(trackBar1.Maximum));
                }
            }
        }

        private string GetTimeString(int val)
        {
            int hour = val / 3600;
            val %= 3600;
            int minute = val / 60;
            int second = val % 60;
            return string.Format("{0:00}:{1:00}:{2:00}", hour, minute, second);
        }
        //滑块托动事件
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (is_playinig_)
            {
                vlc_player_.SetPlayTime(trackBar1.Value);
                trackBar1.Value = (int)vlc_player_.GetPlayTime();
            }
        }
        //音量滑块托动事件
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            vlc_player_.SetVolume(this.trackBar2.Value);
        }
        /// <summary>
        /// 全屏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

            this.panel2.Visible = false;
            this.WindowState = FormWindowState.Maximized;
            this.panel1.Dock = DockStyle.Fill;
            this.FormBorderStyle = FormBorderStyle.None;
            WIN32api.SetTaskbarState(WIN32api.AppBarStates.AutoHide);

        }

        private void PlayerWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            WIN32api.SetTaskbarState(WIN32api.AppBarStates.AlwaysOnTop);

        }

        private void panel1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.panel2.Visible = true;
            }

        }


        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.panel2.Visible = true;
        }

        private void PlayerWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.panel2.Visible = true;
                WIN32api.SetTaskbarState(WIN32api.AppBarStates.AlwaysOnTop);
            }
        }
        /// <summary>
        /// 拖入完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerWindow_DragDrop(object sender, DragEventArgs e)
        {

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                FormPlay(file);
            }

        }
        /// <summary>
        /// 文件拖入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerWindow_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (is_playinig_)
            {
                if (button2.Text == "暂停")
                {
                    vlc_player_.Pause();
                    timer1.Stop();
                    button2.Text = "播放";
                }
                else
                {
                    vlc_player_.Play();
                    timer1.Start();
                    button2.Text = "暂停";
                }


            }
            else
            {
                button2.Text = "播放";
            }
        }
    }

}
