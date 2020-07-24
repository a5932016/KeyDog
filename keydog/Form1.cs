using System;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Configuration;

namespace keydog
{
    public partial class Form1 : Form
    {
        [DllImport("User32.dll")]
        public static extern void keybd_event(Byte bVk, Byte bScan, int dwFlags, int dwExtraInfo);

        GlobalKeyboardHook gHook;
        int kv;//將keyValue轉成整數用的變數
        bool ctrl, alt, shift;//按下功能鍵時就改為true
        string keyQ = string.Empty, keyW = string.Empty, keyE = string.Empty, keyR = string.Empty;
        string PVPNum4, PVPNum7, PVPNum8;
        bool key_action = false;
        bool PvpKind = false;

        private void Naruto_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                TextBox tb = (TextBox)sender;
                tb.Text = e.KeyCode.ToString();
            }
            catch
            {
            }
        }

        private void Naruto_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                TextBox tb = (TextBox)sender;
                tb.BackColor = Color.White;
            }
            catch
            { }
        }

        private void Naruto_Leave(object sender, EventArgs e)
        {
            try
            {
                TextBox tb = (TextBox)sender;
                tb.BackColor = Color.Gainsboro;
            }
            catch { }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //x.Text = System.Windows.Forms.Cursor.Position.X.ToString();
            //y.Text = System.Windows.Forms.Cursor.Position.Y.ToString();
        }

        public Form1()
        {
            InitializeComponent();

            setNarutoDefault();
            setNarutoPanel();
            setNarutoPVP();
        }

        public void setNarutoDefault()
        {
            if (!Program.isSetting("Num7"))
                Program.AddUpdateAppSettings("Num7", "D3");
            if (!Program.isSetting("Num7_X"))
                Program.AddUpdateAppSettings("Num7_X", "1360");
            if (!Program.isSetting("Num7_Y"))
                Program.AddUpdateAppSettings("Num7_Y", "970");
            if (!Program.isSetting("HG"))
                Program.AddUpdateAppSettings("HG", "D5");
            if (!Program.isSetting("HC"))
                Program.AddUpdateAppSettings("HC", "D4");
            if (!Program.isSetting("Strengthen"))
                Program.AddUpdateAppSettings("Strengthen", "D6");
            if (!Program.isSetting("HGRecord"))
                Program.AddUpdateAppSettings("HGRecord", "D9");
            if (!Program.isSetting("CRecord"))
                Program.AddUpdateAppSettings("CRecord", "D0");
            if (!Program.isSetting("PVPNum4"))
                Program.AddUpdateAppSettings("PVPNum4", "D2");
            if (!Program.isSetting("PVPNum7"))
                Program.AddUpdateAppSettings("PVPNum7", "D3");
            if (!Program.isSetting("PVPNum8"))
                Program.AddUpdateAppSettings("PVPNum8", "D4");
        }

        public void setNarutoPVP()
        {
            PVPNum4 = Program.ReadSetting("PVPNum4");
            PVPNum7 = Program.ReadSetting("PVPNum7");
            PVPNum8 = Program.ReadSetting("PVPNum8");
        }

        public void setNarutoPanel()
        {
            foreach (Control con in NarutoPanel.Controls)
            {
                if (typeof(TextBox) != con.GetType())
                    continue;

                con.Text = Program.ReadSetting(con.Name);
            }
        }

        private void NarutoSave_Click(object sender, EventArgs e)
        {
            foreach (Control con in NarutoPanel.Controls)
            {
                if (typeof(TextBox) != con.GetType())
                    continue;

                Program.AddUpdateAppSettings(con.Name, con.Text);
            }

            MessageBox.Show("儲存完成！");
        }

        private void NarutoDefault_Click(object sender, EventArgs e)
        {
            Num7.Text = "D3";
            Num7_X.Text = "1360";
            Num7_Y.Text = "970";
            HG.Text = "D5";
            HC.Text = "D4";
            Strengthen.Text = "D6";
            HGRecord.Text = "D9";
            CRecord.Text = "D0";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                gHook = new GlobalKeyboardHook(); //根據作者的程式碼(class)創造一個新物件

                gHook.KeyDown += new KeyEventHandler(gHook_KeyDown);// 連結KeyDown事件

                foreach (Keys key in Enum.GetValues(typeof(Keys)))
                    gHook.HookedKeys.Add(key);

                gHook.hook();//開始監控
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void gHook_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                kv = (int)e.KeyValue;//把按下的按鍵號碼轉成整數存在kv中

                if (e.KeyCode == Keys.Delete)
                {
                    if (key_action)
                        key_action = false;
                    else
                        key_action = true;
                }
                else if (e.KeyCode == Keys.Home)
                {
                    if (PvpKind)
                    {
                        PvpLabel.Visible = false;
                        PvpKind = false;
                    }
                    else
                    {
                        PvpLabel.Visible = true;
                        PvpKind = true;
                    }
                }

                if (!key_action)
                    return;

                if (PvpKind)
                {
                    if ((Keys)Enum.Parse(typeof(Keys), PVPNum4, true) == e.KeyCode) //4
                    {
                        keybd_event(100, 0, 0x0100, 0);
                        keybd_event(100, 0, 2, 0);
                        Mouse.LeftClick();
                    }
                    else if ((Keys)Enum.Parse(typeof(Keys), PVPNum7, true) == e.KeyCode) //7
                    {
                        keybd_event(103, 0, 0x0100, 0);
                        keybd_event(103, 0, 2, 0);
                        Mouse.LeftClick();
                    }
                    else if ((Keys)Enum.Parse(typeof(Keys), PVPNum8, true) == e.KeyCode) //8
                    {
                        keybd_event(104, 0, 0x0100, 0);
                        keybd_event(104, 0, 2, 0);
                        Mouse.LeftClick();
                    }
                }
                else if ((Keys)Enum.Parse(typeof(Keys), HG.Text, true) == e.KeyCode)
                {
                    keybd_event(0x0D, 0, 0x0100, 0);
                    keybd_event(0x0D, 0, 2, 0);
                    SendKeys.Send("hg");
                    keybd_event(0x0D, 0, 0x0100, 0);
                    keybd_event(0x0D, 0, 2, 0);
                }
                else if ((Keys)Enum.Parse(typeof(Keys), HC.Text, true) == e.KeyCode)
                {
                    keybd_event(0x0D, 0, 0x0100, 0);
                    keybd_event(0x0D, 0, 2, 0);
                    SendKeys.Send("hc");
                    keybd_event(0x0D, 0, 0x0100, 0);
                    keybd_event(0x0D, 0, 2, 0);
                }
                else if ((Keys)Enum.Parse(typeof(Keys), HGRecord.Text, true) == e.KeyCode)
                {
                    keybd_event(0x0D, 0, 0x0100, 0);
                    keybd_event(0x0D, 0, 2, 0);
                    SendKeys.Send("記錄回城點");
                    keybd_event(0x0D, 0, 0x0100, 0);
                    keybd_event(0x0D, 0, 2, 0);
                    keybd_event(0x0D, 0, 0x0100, 0);
                    keybd_event(0x0D, 0, 2, 0);
                }
                else if ((Keys)Enum.Parse(typeof(Keys), CRecord.Text, true) == e.KeyCode)
                {
                    keybd_event(0x0D, 0, 0x0100, 0);
                    keybd_event(0x0D, 0, 2, 0);
                    SendKeys.Send("記錄C鍵回城點");
                    keybd_event(0x0D, 0, 0x0100, 0);
                    keybd_event(0x0D, 0, 2, 0);
                    keybd_event(0x0D, 0, 0x0100, 0);
                    keybd_event(0x0D, 0, 2, 0);
                }
                else if ((Keys)Enum.Parse(typeof(Keys), Strengthen.Text, true) == e.KeyCode)
                {
                    int x = System.Windows.Forms.Cursor.Position.X;
                    int y = System.Windows.Forms.Cursor.Position.Y;

                    keybd_event(98, 0, 0x0100, 0);
                    keybd_event(98, 0, 2, 0);
                    Mouse.MoveTo(Convert.ToInt32(Num7_X.Text), Convert.ToInt32(Num7_Y.Text));
                    Thread.Sleep(10);
                    Mouse.LeftClick();
                    Mouse.MoveTo(x, y);
                }
                else if ((Keys)Enum.Parse(typeof(Keys), Num7.Text, true) == e.KeyCode)
                {
                    keybd_event(103, 0, 0x0100, 0);
                    keybd_event(103, 0, 2, 0);
                    Mouse.LeftClick();
                }

                #region 賭博買技
                //if (key_action == true)
                //{
                //    if (e.KeyCode == Keys.D2)
                //    {
                //        keybd_event(0x67, 0, 0, 0);
                //    }
                //    else if (e.KeyCode == Keys.D3)
                //    {
                //        keybd_event(0x68, 0, 0, 0);
                //    }

                //    //自訂
                //    else if (keyQ != string.Empty)
                //    {
                //        if (e.KeyCode == Keys.Q)
                //        {
                //            if (keyQ != "Q")
                //                SendKeys.Send(keyQ);

                //            if (checkBox1.Checked == true)
                //            {
                //                Mouse.LeftClick();
                //            }
                //        }
                //    }
                //    else if (keyW != string.Empty)
                //    {
                //        if (e.KeyCode == Keys.W)
                //        {
                //            if (keyW != "W")
                //                SendKeys.Send(keyW);

                //            if (checkBox2.Checked == true)
                //            {
                //                Mouse.LeftClick();
                //            }
                //        }
                //    }
                //    else if (keyE != string.Empty)
                //    {
                //        if (e.KeyCode == Keys.E)
                //        {
                //            if (keyE != "E")
                //                SendKeys.Send(keyE);

                //            if (checkBox3.Checked == true)
                //            {
                //                Mouse.LeftClick();
                //            }
                //        }
                //    }
                //    else if (keyR != string.Empty)
                //    {
                //        if (e.KeyCode == Keys.R)
                //        {
                //            if (keyR != "R")
                //                SendKeys.Send(keyR);

                //            if (checkBox4.Checked == true)
                //            {
                //                Mouse.LeftClick();
                //            }
                //        }
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void butAction_Click_1(object sender, EventArgs e)
        {
            if (txtE.Text != string.Empty)
                keyE = txtE.Text;
            if (txtQ.Text != string.Empty)
                keyQ = txtQ.Text;
            if (txtW.Text != string.Empty)
                keyW = txtW.Text;
            if (txtR.Text != string.Empty)
                keyR = txtR.Text;

            MessageBox.Show("按鍵登錄完成!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtE.Text = string.Empty;
            txtQ.Text = string.Empty;
            txtR.Text = string.Empty;
            txtW.Text = string.Empty;

            keyE = string.Empty;
            keyQ = string.Empty;
            keyW = string.Empty;
            keyR = string.Empty;

            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
        }
    }
}
