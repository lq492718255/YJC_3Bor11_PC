using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Reflection;
using System.Management;
using System.IO;
using System.IO.Ports;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;

namespace YJC_3Bor11_PC
{
    public partial class Form1 : Form
    {
        //int xSize = 10; //chart的X轴点数
        double setspeed, maxspeed, minspeed; //设定转速,上限,下限
        Button btn_mouseDown; //被按下的键
        int measpeed_max = 0; //测到的最大值
        int Fresh_seconds = 30; //刷新间隔时间

        List<byte> Convert_list = new List<byte>(); //串口接收等待被转换为字符串的byte码
        List<string> Params = new List<string>() { "Tpr", "AD1" }; //此列表中的参数接收显示
        struct XYvalue { public DateTime Xval; public string Yval;  }; //收数时间与数据
        List<XYvalue> XYval_list = new List<XYvalue>(); //收数列表,按接收时间填充,在timer2取第一个数画曲线图并写入txt后删掉
        DateTime FirstDatadt = new DateTime(); //全图第一个数据点的x轴时间值,初始值DateTime.MinValue{0001/1/1 0:00:00}
        XYvalue XYval_pre; //刚被画到曲线图上的参数点

        bool SaveStatus = false; //是否在记录
        string Savefilename; //保存文件全路径
        FileStream Savefilestream; //保存文件流
        StreamWriter Savestreamwr; //写文件流                
        DateTime Startsavetime = DateTime.MinValue;

        XYvalue reviewXYval_list_first, reviewXYval_list_last; //回放数据列表里的第一个和最后一个
        List<XYvalue> reviewXYval_list = new List<XYvalue>(); //加载回放数据列表
        bool ReviewStatus = false; //是否回放

        Thread drawChartThread; //画图线程
        static object lockObj = new object();
        int LimtDrawCnt = 999999999; //曲线图可画点数上限

        public Form1()
        {
            InitializeComponent();
            System.Drawing.Rectangle ScreenArea = System.Windows.Forms.Screen.GetWorkingArea(this);
            this.Size = new Size(1150, 678);
            //this.WindowState = FormWindowState.Maximized;  //最大化窗体

            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            button_save.Enabled = false;
            button_sndPID.Enabled = false;
            button1.BackColor = Color.Orange;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button_Xadd.Enabled = false;
            button_Xsub.Enabled = false;
            button_autosize.Enabled = false;
            comboBox_cmd.Enabled = false;
            button_sendcmd.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            comboBox1GetComNum();
            if (comboBox1.Items.Count > 0)
                comboBox1.Text = comboBox1.Items[0].ToString();
            comboBox2.Text = "9600"; comboBox3.Text = "无校验";

            //定义图表区域
            this.chart1.ChartAreas.Clear();
            ChartArea chartArea1 = new ChartArea("C1");
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Series.Clear();
            //设置图表显示样式
            chart1.ChartAreas[0].AxisY.Minimum = -600;
            chart1.ChartAreas[0].AxisY.Maximum = 500;
            //chart1.ChartAreas[0].AxisX.ScaleView.Size = xSize;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.NotSet;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.NotSet; //Y轴无格子线
            //=====================允许XY轴放大=================================
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;//
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].CursorX.Interval = 0;
            chart1.ChartAreas[0].CursorX.IntervalOffset = 0;
            chart1.ChartAreas[0].CursorX.IntervalType = DateTimeIntervalType.Minutes;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            //chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = false; //滚动条在图标区外
            chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            //====================设置图表X轴显示样式==============================
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";   //毫秒格式HH:mm:ss.fff 此时要注意轴的最大值和最小值不要差太大
            chart1.ChartAreas[0].AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 1;    //坐标值间隔1S
            chart1.ChartAreas[0].AxisX.LabelStyle.IsEndLabelVisible = false;   //防止X轴坐标跳跃
            chart1.ChartAreas[0].AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 1;    //网格间隔
            //chart1.ChartAreas[0].AxisX.Minimum = DateTime.Now.ToOADate();
            //chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.AddSeconds(Fresh_seconds).ToOADate();
            //====================设置图表Y轴显示样式==============================
            chart1.ChartAreas[0].AxisY.LabelStyle.Interval = 100;
            chart1.ChartAreas[0].AxisY.MajorGrid.Interval = 100;    //网格间隔

            timer1.Stop(); //按住按键时间计时器

            //drawChartThread = new Thread(whileDrawChart);

            # region //画转速目标线,上限线,下线线
            /*double.TryParse(textBox1.Text, out setspeed);            
            maxspeed = (setspeed + 0.12) * 81;
            minspeed = (setspeed - 0.12) * 81;
            setspeed *= 81;
            label3.Text = "*81= " + setspeed.ToString();
            chart1.ChartAreas[0].AxisY.StripLines.Clear();
            StripLine setspeedline = new StripLine();
            setspeedline.Text = string.Format("目标转速：{0:F0}", setspeed);
            setspeedline.Interval = 0;
            setspeedline.IntervalOffset = (int)setspeed;
            setspeedline.StripWidth = 1;
            setspeedline.BackColor = Color.Teal;
            setspeedline.BorderDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisY.StripLines.Add(setspeedline); //画目标线
            StripLine maxspeedline = new StripLine();
            maxspeedline.Text = string.Format("上限转速：{0:F0}", maxspeed);
            maxspeedline.Interval = 0;
            maxspeedline.IntervalOffset = (int)maxspeed;
            maxspeedline.StripWidth = 1;
            maxspeedline.BackColor = Color.DarkBlue;
            maxspeedline.BorderDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisY.StripLines.Add(maxspeedline); //画上限线
            StripLine minspeedline = new StripLine();
            minspeedline.Text = string.Format("下限转速：{0:F0}", minspeed);
            minspeedline.Interval = 0;
            minspeedline.IntervalOffset = (int)minspeed;
            minspeedline.StripWidth = 1;
            minspeedline.BackColor = Color.DarkOrange;
            minspeedline.BorderDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisY.StripLines.Add(minspeedline); //画下线线
            */
            #endregion
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                int rbn = serialPort1.BytesToRead;
                byte[] buf = new byte[rbn];
                serialPort1.Read(buf, 0, rbn);

                if (checkBox_HEXshow.Checked == true)
                {
                    string hexstr = string.Empty;
                    for (int i = 0; i < buf.Length; i++)
                        hexstr += string.Format("{0:X2}", buf[i]) + " ";
                    this.BeginInvoke((MethodInvoker)delegate
                        {
                            //textBox_HEX.AppendText(DateTime.Now.ToString("HH:mm:ss:fff <<<--\r\n") + hexstr + "\r\n");
                            richTextBox_HEX.AppendText(DateTime.Now.ToString("HH:mm:ss:fff <<<--\r\n"));
                            richTextBox_HEX.SelectionColor = Color.Green;
                            richTextBox_HEX.SelectionFont = new Font("宋体", 12F, FontStyle.Regular);
                            richTextBox_HEX.AppendText(hexstr + "\r\n");
                        });
                }
                //if (checkBox_HEXshow.Checked == false) //不管checkBox_HEXshow是否勾选都要转字符串
                {
                    for (int i = 0; i < buf.Length; i++)
                    {
                        string strbufi = Encoding.ASCII.GetString(new byte[] { buf[i] }, 0, 1);
                        //this.BeginInvoke((MethodInvoker)delegate 
                        //    {   
                        //        //textBox_HEX.AppendText(strbufi);
                        //        richTextBox_HEX.SelectionColor = Color.Green;
                        //        richTextBox_HEX.AppendText(strbufi);
                        //    });
                        if (strbufi != "\r" && strbufi != "\n")
                            Convert_list.Add(buf[i]);
                        else if ((strbufi == "\r" || strbufi == "\n") && Convert_list.Count > 1)
                        {
                            string recstr = Encoding.ASCII.GetString(Convert_list.ToArray(), 0, Convert_list.ToArray().Length);
                            Convert_list.Clear();
                            this.BeginInvoke((MethodInvoker)delegate
                                {
                                    //textBox_HEX.AppendText("\r\n" + DateTime.Now.ToString("HH:mm:ss:fff <<<--"));
                                    //textBox_HEX.AppendText(recstr + "\r\n");
                                    //richTextBox_HEX.SelectionColor = Color.Green;
                                    richTextBox_HEX.AppendText(DateTime.Now.ToString("HH:mm:ss:fff <<<--\r\n"));
                                    richTextBox_HEX.SelectionColor = Color.Green;
                                    richTextBox_HEX.SelectionFont = new Font("宋体", 12F, FontStyle.Regular);
                                    richTextBox_HEX.AppendText(recstr + "\r\n");
                                    if (toolStripMenuItem1.Text == "手动输入" && recstr.StartsWith("Tva="))
                                        textBox_Tva.Text=Convert.ToInt32(recstr.Replace("Tva=", "0"), 16).ToString("X2"); 
                                });

                            #region 字符接收 //不使用checkBox_HEX时此段直接放置在serialPort1_DataReceived内
                            //string recstr = serialPort1.ReadLine(); //不使用checkBox_HEX时打开
                            DateTime datetimeNow = DateTime.Now; //DateTime.FromOADate(double OLEdatetime)                
                            XYvalue xyval_recone; //本次接收数据点
                            xyval_recone.Xval = datetimeNow;
                            xyval_recone.Yval = recstr;
                            //if (SaveStatus ==true) //(button_save.Text.StartsWith("停止记录") ) //正在记录中,保存所有接收到的参数到文件
                            //    Savestreamwr.WriteLine(xyval_recone.Xval.ToString("yyyy-MM-dd HH:mm:ss:fff") + "@" + xyval_recone.Yval);
                            //this.BeginInvoke((MethodInvoker)delegate
                            //  { textBox_HEX.AppendText(xyval.Xval.ToString("HH:mm:ss:fff") + " " + xyval.Yval + "\r\n"); });
                            foreach (string paramdd in Params)
                                if (recstr.StartsWith(paramdd + "="))
                                {
                                    XYval_list.Add(xyval_recone);
                                    if (SaveStatus == true) //(button_save.Text.StartsWith("停止记录") ) //正在记录中,保存指定的接收参数到文件
                                        Savestreamwr.WriteLine(xyval_recone.Xval.ToString("yyyy-MM-dd HH:mm:ss:fff") + "@" + xyval_recone.Yval);
                                    break;
                                }
                            #endregion
                        }
                    }
                }
            }
            catch (Exception err)
            { }

            #region //直接操作内存字节方法
            /*try
            {                
                if (serialPort1.BytesToRead >= 6 && serialPort1.ReadByte() == 0xAB && serialPort1.ReadByte() == 0xCD)
                {
                    #region 内存字节转数值-指针操作内存写法
                    //int measpeed = 0;
                    //unsafe
                    //{
                    //    byte* p = (byte*)(&measpeed);
                    //    *p = (byte)serialPort1.ReadByte();
                    //    *(p + 1) = (byte)serialPort1.ReadByte();
                    //    *(p + 2) = (byte)serialPort1.ReadByte();
                    //    *(p + 3) = (byte)serialPort1.ReadByte();
                    //}
                    #endregion

                    #region 内存字节转数值-c#写法
                    byte[] measpeed_bytes = { 0, 0, 0, 0 };
                    measpeed_bytes[3] = (byte)serialPort1.ReadByte();
                    measpeed_bytes[2] = (byte)serialPort1.ReadByte();
                    measpeed_bytes[1] = (byte)serialPort1.ReadByte();
                    measpeed_bytes[0] = (byte)serialPort1.ReadByte();
                    //判断计算机结构的endian设置,IsLittleEndian为指示数据在此计算机结构中存储时的字节顺序（“Endian”性质）
                    //如果为 Little-endian，则该值为 true；如果结构为 Big-endian，则该值为 false
                    //“Little-endian”表示最大的有效字节位于单词的右端“Big-endian”表示最大的有效字节位于单词的左端
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(measpeed_bytes); //转换排序
                    int measpeed = BitConverter.ToInt32(measpeed_bytes, 0);
                    #endregion
                }
            }
            catch(Exception err) 
            { }*/
            #endregion
        }

        private void button1_Click(object sender, EventArgs e)//打开关闭串口
        {
            if (button1.Text == "打开串口") //开启串口
            {
                try
                {
                    serialPort1.PortName = comboBox1.Text.Split(':')[0];
                    int baudRate;
                    int.TryParse(comboBox2.Text, out baudRate);
                    serialPort1.BaudRate = baudRate;
                    switch (comboBox3.Text)
                    {
                        case "无校验":
                            serialPort1.Parity = Parity.None;
                            break;
                        case "奇校验":
                            serialPort1.Parity = Parity.Odd;
                            break;
                        case "偶校验":
                            serialPort1.Parity = Parity.Even;
                            break;
                        default:
                            break;
                    }
                    serialPort1.Open();
                    chart1.ChartAreas[0].AxisX.Minimum = DateTime.Now.ToOADate();
                    chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.AddSeconds(Fresh_seconds).ToOADate();
                    timer2.Start();  //开启定时器画chart图
                    //drawChartThread.Start(); //开线程画chart图 ,//禁用,数据太多之后界面卡
                    button1.Text = "关闭串口";
                    button1.BackColor = Color.LightGreen;
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    button_review.Enabled = false;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button5.Enabled = true;
                    button_Xadd.Enabled = true;
                    button_Xsub.Enabled = true;
                    button_autosize.Enabled = true;
                    groupBox1.Enabled = true;
                    groupBox2.Enabled = true;
                    button_save.Enabled = true;
                    button_save.BackColor = Color.Orange;
                    button_sndPID.Enabled = true;
                    comboBox_cmd.Enabled = true;
                    button_sendcmd.Enabled = true;
                    chart1.Series.Clear(); //清空,恢复默认
                    chart1.Titles.Clear();
                    Fresh_seconds = 30;
                    XYval_list.Clear();
                    FirstDatadt = DateTime.MinValue;
                    XYval_pre.Xval = DateTime.MinValue;
                    XYval_pre.Yval = "";
                }
                catch (Exception err)
                {
                    MessageBox.Show("串口打开错误");
                    //return;
                }
            }
            else if (button1.Text == "关闭串口") //关闭串口
            {
                //Measpd.Clear();
                //chart1.Series[0].Points.Clear();
                //chart1.Series[1].Points.Clear();
                //serialPort1.Dispose();
                serialPort1.Close();
                //drawChartThread.Abort(); //关闭画图线程 //实时直播时禁用线程画图
                timer2.Stop(); //关闭画图定时器
                timer3.Stop(); //关闭记录计时定时器
                button1.Text = "打开串口";
                button1.BackColor = Color.Orange;
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                button_review.Enabled = true;
                groupBox1.Enabled = false;
                groupBox2.Enabled = false;
                button_sndPID.Enabled = false;
                button_save.Enabled = false;
                button_save.UseVisualStyleBackColor = true;
                comboBox_cmd.Enabled = false;
                button_sendcmd.Enabled = false;
                if (SaveStatus == true)//(button_save.Text.StartsWith("停止记录")) //正在记录中
                {
                    Savestreamwr.Close();
                    #region 比较起止时间后 改文件名
                    FileInfo nfi = new FileInfo(Savefilestream.Name);
                    string begyyyy = Savefilestream.Name.Substring(Savefilestream.Name.LastIndexOf("\\") + 1).Split('年')[0];
                    string begMM = Savefilestream.Name.Split('年')[1].Split('月')[0];
                    string begdd = Savefilestream.Name.Split('月')[1].Split('日')[0];
                    string begHH = Savefilestream.Name.Split('日')[1].Split('时')[0];
                    string begmm = Savefilestream.Name.Split('时')[1].Split('分')[0];
                    string begss = Savefilestream.Name.Split('分')[1].Split('秒')[0];
                    string endtime = DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒");
                    string endyyyy = endtime.Split('年')[0];
                    string endMM = endtime.Split('年')[1].Split('月')[0];
                    string enddd = endtime.Split('月')[1].Split('日')[0];
                    string endHH = endtime.Split('日')[1].Split('时')[0];
                    string endmm = endtime.Split('时')[1].Split('分')[0];
                    string endss = endtime.Split('分')[1].Split('秒')[0];
                    //文件名=开始记录时间-停止记录时间(不同时刻)
                    //string showyyyy = begyyyy == endyyyy ? "" : endyyyy + "年"; 
                    //string showMM = begMM == endMM ? "" : endMM + "月";
                    //string showdd = begdd == enddd ? "" : enddd + "日";
                    //string showHH = begHH == endHH ? "" : endHH + "时";
                    //string showmm = begmm == endmm ? "" : endmm + "分";
                    //string showss = begss == endss ? "" : endss + "秒";                
                    //nfi.MoveTo(Savefilestream.Name.Replace(".txt", "") + "-" + showyyyy + showMM + showdd + showHH + showmm + showss + ".txt");
                    //文件名=开始记录时间(记录时长)
                    DateTime begt = Convert.ToDateTime(begyyyy + "-" + begMM + "-" + begdd + " " + begHH + ":" + begmm + ":" + begss);
                    DateTime endt = Convert.ToDateTime(endyyyy + "-" + endMM + "-" + enddd + " " + endHH + ":" + endmm + ":" + endss);
                    TimeSpan tsbe = endt - begt;
                    string showdd = tsbe.Days == 0 ? "" : tsbe.Days + "日";
                    string showHH = tsbe.Hours == 0 ? "" : tsbe.Hours + "时";
                    string showmm = tsbe.Minutes == 0 ? "" : tsbe.Minutes + "分";
                    string showss = tsbe.Seconds == 0 ? "" : tsbe.Seconds + "秒";
                    nfi.MoveTo(Savefilestream.Name.Replace(".txt", "") + "(时长" + showdd + showHH + showmm + showss + ").txt");
                    #endregion
                    Savefilestream.Close();
                    button_save.Text = "开始记录";
                    SaveStatus = false;
                }
            }
        }

        #region XY轴缩放按键点击与长按
        private void button2_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisY.Maximum += 10;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (chart1.ChartAreas[0].AxisY.Maximum > chart1.ChartAreas[0].AxisY.Minimum + 30)
                chart1.ChartAreas[0].AxisY.Maximum -= 10;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (chart1.ChartAreas[0].AxisY.Maximum > chart1.ChartAreas[0].AxisY.Minimum + 30)
                chart1.ChartAreas[0].AxisY.Minimum += 10;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisY.Minimum -= 10;
        }
        private void button_Xadd_Click(object sender, EventArgs e)
        {
            //xSize += 10;
            //xSize = xSize >= 0 ? xSize : 20;
            //chart1.ChartAreas[0].AxisX.ScaleView.Size = xSize;

            chart1.ChartAreas[0].AxisX.Maximum = DateTime.FromOADate(chart1.ChartAreas[0].AxisX.Maximum).AddSeconds(1).ToOADate();
            Fresh_seconds = ++Fresh_seconds < 60000 ? Fresh_seconds : 60000;
        }
        private void button_Xsub_Click(object sender, EventArgs e)
        {
            //xSize -= 10;
            //xSize = xSize >= 0 ? xSize : 20;
            //chart1.ChartAreas[0].AxisX.ScaleView.Size = xSize;

            double dtd = DateTime.FromOADate(chart1.ChartAreas[0].AxisX.Maximum).AddSeconds(-1).ToOADate();
            Debug.WriteLine("cha====" + (DateTime.FromOADate(dtd) - DateTime.FromOADate(chart1.ChartAreas[0].AxisX.Minimum)).Seconds);
            if ((DateTime.FromOADate(dtd) - DateTime.FromOADate(chart1.ChartAreas[0].AxisX.Minimum)).Seconds > 3)
                chart1.ChartAreas[0].AxisX.Maximum = dtd;
            Fresh_seconds = --Fresh_seconds > 3 ? Fresh_seconds : 3;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (btn_mouseDown.Name)
            {
                case "button2":
                    button2_Click(sender, e);
                    break;
                case "button3":
                    button3_Click(sender, e);
                    break;
                case "button4":
                    button4_Click(sender, e);
                    break;
                case "button5":
                    button5_Click(sender, e);
                    break;
                case "button_Xsub":
                    button_Xsub_Click(sender, e);
                    break;
                case "button_Xadd":
                    button_Xadd_Click(sender, e);
                    break;
                default:
                    break;
            }
        }
        private void btnAddSubXY_MouseDown(object sender, MouseEventArgs e)
        {
            btn_mouseDown = (Button)sender;
            timer1.Start();
        }
        private void btnAddSubXY_MouseUp(object sender, MouseEventArgs e)
        {
            btn_mouseDown = null;
            timer1.Stop();
        }
        #endregion

        #region 获取本机串口信息
        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1GetComNum();
        }
        private void comboBox5_DropDown(object sender, EventArgs e)
        {
            comboBox5GetComNum();
        }
        public void comboBox1GetComNum()
        {
            comboBox1.Items.Clear();
            string[] vPortNames = SerialPort.GetPortNames();
            string[] strArr = GetHarewareInfo(HardwareEnum.Win32_PnPEntity, "Name");

            foreach (string vpn in vPortNames)
                foreach (string s in strArr)
                    if (s.Contains(vpn))
                    {
                        comboBox1.Items.Add(vpn + ": " + s);
                        break;
                    }
        }
        public void comboBox5GetComNum()
        {
            comboBox5.Items.Clear();
            string[] vPortNames = SerialPort.GetPortNames();
            string[] strArr = GetHarewareInfo(HardwareEnum.Win32_PnPEntity, "Name");

            foreach (string vpn in vPortNames)
                foreach (string s in strArr)
                    if (s.Contains(vpn))
                    {
                        comboBox5.Items.Add(vpn + ": " + s);
                        break;
                    }
        }
        /// <summary>
        /// Get the system devices information with windows api.
        /// </summary>
        /// <param name="hardType">Device type.</param>
        /// <param name="propKey">the property of the device.</param>
        /// <returns></returns>
        private static string[] GetHarewareInfo(HardwareEnum hardType, string propKey)
        {

            List<string> strs = new List<string>();
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + hardType))
                {
                    var hardInfos = searcher.Get();
                    foreach (var hardInfo in hardInfos)
                    {
                        if (hardInfo.Properties[propKey].Value != null)
                        {
                            String str = hardInfo.Properties[propKey].Value.ToString();
                            strs.Add(str);
                        }
                    }
                }
                return strs.ToArray();
            }
            catch
            {
                return null;
            }
            finally
            {
                strs = null;
            }
        }//end of func GetHarewareInfo().
        /// <summary>
        /// 枚举win32 api
        /// </summary>
        public enum HardwareEnum
        {
            // 硬件
            Win32_Processor, // CPU 处理器
            Win32_PhysicalMemory, // 物理内存条
            Win32_Keyboard, // 键盘
            Win32_PointingDevice, // 点输入设备，包括鼠标。
            Win32_FloppyDrive, // 软盘驱动器
            Win32_DiskDrive, // 硬盘驱动器
            Win32_CDROMDrive, // 光盘驱动器
            Win32_BaseBoard, // 主板
            Win32_BIOS, // BIOS 芯片
            Win32_ParallelPort, // 并口
            Win32_SerialPort, // 串口
            Win32_SerialPortConfiguration, // 串口配置
            Win32_SoundDevice, // 多媒体设置，一般指声卡。
            Win32_SystemSlot, // 主板插槽 (ISA & PCI & AGP)
            Win32_USBController, // USB 控制器
            Win32_NetworkAdapter, // 网络适配器
            Win32_NetworkAdapterConfiguration, // 网络适配器设置
            Win32_Printer, // 打印机
            Win32_PrinterConfiguration, // 打印机设置
            Win32_PrintJob, // 打印机任务
            Win32_TCPIPPrinterPort, // 打印机端口
            Win32_POTSModem, // MODEM
            Win32_POTSModemToSerialPort, // MODEM 端口
            Win32_DesktopMonitor, // 显示器
            Win32_DisplayConfiguration, // 显卡
            Win32_DisplayControllerConfiguration, // 显卡设置
            Win32_VideoController, // 显卡细节。
            Win32_VideoSettings, // 显卡支持的显示模式。

            // 操作系统
            Win32_TimeZone, // 时区
            Win32_SystemDriver, // 驱动程序
            Win32_DiskPartition, // 磁盘分区
            Win32_LogicalDisk, // 逻辑磁盘
            Win32_LogicalDiskToPartition, // 逻辑磁盘所在分区及始末位置。
            Win32_LogicalMemoryConfiguration, // 逻辑内存配置
            Win32_PageFile, // 系统页文件信息
            Win32_PageFileSetting, // 页文件设置
            Win32_BootConfiguration, // 系统启动配置
            Win32_ComputerSystem, // 计算机信息简要
            Win32_OperatingSystem, // 操作系统信息
            Win32_StartupCommand, // 系统自动启动程序
            Win32_Service, // 系统安装的服务
            Win32_Group, // 系统管理组
            Win32_GroupUser, // 系统组帐号
            Win32_UserAccount, // 用户帐号
            Win32_Process, // 系统进程
            Win32_Thread, // 系统线程
            Win32_Share, // 共享
            Win32_NetworkClient, // 已安装的网络客户端
            Win32_NetworkProtocol, // 已安装的网络协议
            Win32_PnPEntity,//all device
        }
        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                    serialPort1.Close();
                if (button_save.Text.StartsWith("停止记录")) //正在记录中
                {
                    Savestreamwr.Close();
                    #region 比较起止时间后 改文件名
                    FileInfo nfi = new FileInfo(Savefilestream.Name);
                    string begyyyy = Savefilestream.Name.Substring(Savefilestream.Name.LastIndexOf("\\") + 1).Split('年')[0];
                    string begMM = Savefilestream.Name.Split('年')[1].Split('月')[0];
                    string begdd = Savefilestream.Name.Split('月')[1].Split('日')[0];
                    string begHH = Savefilestream.Name.Split('日')[1].Split('时')[0];
                    string begmm = Savefilestream.Name.Split('时')[1].Split('分')[0];
                    string begss = Savefilestream.Name.Split('分')[1].Split('秒')[0];
                    string endtime = DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒");
                    string endyyyy = endtime.Split('年')[0];
                    string endMM = endtime.Split('年')[1].Split('月')[0];
                    string enddd = endtime.Split('月')[1].Split('日')[0];
                    string endHH = endtime.Split('日')[1].Split('时')[0];
                    string endmm = endtime.Split('时')[1].Split('分')[0];
                    string endss = endtime.Split('分')[1].Split('秒')[0];
                    //文件名=开始记录时间-停止记录时间(不同时刻)
                    //string showyyyy = begyyyy == endyyyy ? "" : endyyyy + "年"; 
                    //string showMM = begMM == endMM ? "" : endMM + "月";
                    //string showdd = begdd == enddd ? "" : enddd + "日";
                    //string showHH = begHH == endHH ? "" : endHH + "时";
                    //string showmm = begmm == endmm ? "" : endmm + "分";
                    //string showss = begss == endss ? "" : endss + "秒";                
                    //nfi.MoveTo(Savefilestream.Name.Replace(".txt", "") + "-" + showyyyy + showMM + showdd + showHH + showmm + showss + ".txt");
                    //文件名=开始记录时间(记录时长)
                    DateTime begt = Convert.ToDateTime(begyyyy + "-" + begMM + "-" + begdd + " " + begHH + ":" + begmm + ":" + begss);
                    DateTime endt = Convert.ToDateTime(endyyyy + "-" + endMM + "-" + enddd + " " + endHH + ":" + endmm + ":" + endss);
                    TimeSpan tsbe = endt - begt;
                    string showdd = tsbe.Days == 0 ? "" : tsbe.Days + "日";
                    string showHH = tsbe.Hours == 0 ? "" : tsbe.Hours + "时";
                    string showmm = tsbe.Minutes == 0 ? "" : tsbe.Minutes + "分";
                    string showss = tsbe.Seconds == 0 ? "" : tsbe.Seconds + "秒";
                    nfi.MoveTo(Savefilestream.Name.Replace(".txt", "") + "(时长" + showdd + showHH + showmm + showss + ").txt");
                    #endregion
                    Savefilestream.Close();
                }
            }
            catch (Exception err) { }
            finally
            {
                //drawChartThread.Abort();
                System.Environment.Exit(0);
            }
        }

        private void button_autosize_Click(object sender, EventArgs e) //自动调整chart图表,禁用,直接点击滚动条上按键
        {
            //定义图表区域
            this.chart1.ChartAreas.Clear();
            ChartArea chartArea1 = new ChartArea("C1");
            this.chart1.ChartAreas.Add(chartArea1);
            if (button1.Text == "关闭串口") //实时显示接收数据时按下自动清除,回放时按下自动则不清除
                this.chart1.Series.Clear();
            //设置图表显示样式
            //chart1.ChartAreas[0].AxisY.Minimum = -500;
            //chart1.ChartAreas[0].AxisY.Maximum = 500;
            ////chart1.ChartAreas[0].AxisX.ScaleView.Size = xSize;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.NotSet;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.NotSet; //Y轴无格子线
            //chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = false; //滚动条在图标区外
            //=====================允许XY轴放大=================================
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].CursorX.Interval = 0;
            chart1.ChartAreas[0].CursorX.IntervalOffset = 0;
            chart1.ChartAreas[0].CursorX.IntervalType = DateTimeIntervalType.Minutes;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            //chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = false;
            chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            //====================设置图表X轴显示样式==============================
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";   //毫秒格式HH:mm:ss.fff 此时要注意轴的最大值和最小值不要差太大
            chart1.ChartAreas[0].AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 1;    //坐标值间隔1S
            chart1.ChartAreas[0].AxisX.LabelStyle.IsEndLabelVisible = false;   //防止X轴坐标跳跃
            chart1.ChartAreas[0].AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 1;    //网格间隔

            if (button1.Text == "打开串口") //回放时按下自动则重排XY
            {
                Delay(500);
                double ymax = 0, ymin = 0, tp;
                foreach (var va in chart1.Series)
                    if (va.Enabled == true)
                    {
                        ymax = ymax > va.Points.FindMaxByValue().YValues.Max() ? ymax : va.Points.FindMaxByValue().YValues.Max();
                        ymin = ymin < va.Points.FindMinByValue().YValues.Min() ? ymin : va.Points.FindMinByValue().YValues.Min();
                    }
                if (ymax < ymin)
                {
                    tp = ymax; ymax = ymin; ymin = tp;
                }
                chart1.ChartAreas[0].AxisY.Maximum = ymax + 50;
                chart1.ChartAreas[0].AxisY.Minimum = ymin - 50;
                chart1.ChartAreas[0].AxisX.Maximum = reviewXYval_list_last.Xval.ToOADate();
                chart1.ChartAreas[0].AxisX.Minimum = reviewXYval_list_first.Xval.ToOADate();
            }

        }

        public void Delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)
                System.Windows.Forms.Application.DoEvents();
        }

        private void button_sndPID_Click(object sender, EventArgs e) //发送kp,ki调试电机PID
        {
            return;
            byte[] snd = new byte[10];
            float kp = 0, ki = 0;
            float.TryParse(textBox_kp.Text, out kp);
            float.TryParse(textBox_ki.Text, out ki);
            unsafe
            {
                snd = new byte[] { 0xDD, *(byte*)(&kp), *((byte*)(&kp) + 1), *((byte*)(&kp) + 2), *((byte*)(&kp) + 3),
                            *(byte*)(&ki),*((byte*)(&ki)+1),*((byte*)(&ki)+2),*((byte*)(&ki)+3),(byte)(0xD0|((byte)(*((byte*)(&ki)+3))&0x0F)) };
            }
            if (serialPort1.IsOpen)
                serialPort1.Write(snd, 0, snd.Length);
        }

        private void timer2_Tick(object sender, EventArgs e) //定时器更新图表
        {
            drawChart();
        }

        private void drawChart() //在曲线图上画XYval_list最前头数据点
        {
            if (XYval_list.Count > 0)
                foreach (string param in Params)
                    if (XYval_list.First().Yval.StartsWith(param) == true)
                    {
                        if (FirstDatadt == DateTime.MinValue) //收到了第一个数据点的时间记在FirstDatadt
                            FirstDatadt = XYval_list.First().Xval;
                        int ydata;
                        int.TryParse(XYval_list.First().Yval.Replace(param + "=", ""), out ydata); //ss.Substring(ss.IndexOf("AD1="), ss.IndexOf("AD2=") - ss.IndexOf("AD1=") - 1);

                        if (chart1.Series.Count == 0) //无图直接添加
                        {
                            Series series1 = new Series(param);
                            series1.ChartArea = "C1";
                            series1.ChartType = SeriesChartType.Spline;
                            chart1.Series.Add(series1);
                        }
                        for (int i = 0; i < chart1.Series.Count; i++) //已经有图则检查是否已经存在该参数的曲线图
                        {
                            if (chart1.Series[i].Name.ToString() == param)
                                break;
                            if (i == chart1.Series.Count - 1)
                            {
                                Series series1 = new Series(param);
                                series1.ChartArea = "C1";
                                series1.ChartType = SeriesChartType.Spline;
                                chart1.Series.Add(series1);
                            }
                        }
                        if (groupBox3.Controls.Find("checkBox_" + param, true).Length == 0) //groupBox3里没有该checkBox,添加上
                        {
                            CheckBox ckb = new CheckBox();
                            ckb.AutoSize = true;
                            ckb.Location = new System.Drawing.Point(10, 15 + 25 * groupBox3.Controls.Count);
                            ckb.Name = "checkBox_" + param;
                            ckb.Size = new System.Drawing.Size(115, 22);
                            ckb.TabIndex = 0;
                            ckb.Text = param;
                            ckb.UseVisualStyleBackColor = true;
                            groupBox3.Controls.Add(ckb);
                            ckb.Checked = true;
                            ckb.CheckedChanged += new System.EventHandler(ckb_CheckedChanged);
                        }
                        //==================================X轴刷新方式1:示波器======================== 
                        if (radioButton_oscFresh.Checked == true)
                            if (XYval_list.First().Xval.ToOADate() > chart1.ChartAreas[0].AxisX.Maximum) //到达刷新周期了,当前值超过X轴最大值刷新X轴
                            {
                                chart1.ChartAreas[0].AxisX.Maximum = XYval_list.First().Xval.AddSeconds(Fresh_seconds).ToOADate();//x轴加上一个刷新周期
                                if (radioButton_oneCyc.Checked == true) //显示最近30s数据,还需在radiooneCyc按下一刻更新X轴min值
                                    chart1.ChartAreas[0].AxisX.Minimum = XYval_list.First().Xval.ToOADate();
                                else if (radioButton_allHis.Checked == true) //显示全部历史数据,还需在radioallHis按下一刻更新X轴min值
                                {
                                    if (chart1.Series[0].Points.Count > LimtDrawCnt) //全图点数过多,强制禁用显示全部历史数据
                                    {
                                        radioButton_oneCyc.Checked = true;
                                        radioButton_allHis.Checked = false;
                                    }
                                    else
                                    {
                                        chart1.ChartAreas[0].AxisX.Minimum = FirstDatadt.ToOADate(); //X轴起点设为第一个数据点时间
                                        //====更改X轴显示间隔
                                        double mms = (XYval_list.First().Xval.AddSeconds(Fresh_seconds) - FirstDatadt).TotalMinutes / 25;
                                        chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
                                        chart1.ChartAreas[0].AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Minutes;
                                        chart1.ChartAreas[0].AxisX.LabelStyle.Interval = mms;    //坐标值间隔分钟
                                        chart1.ChartAreas[0].AxisX.LabelStyle.IsEndLabelVisible = false;   //防止X轴坐标跳跃
                                        chart1.ChartAreas[0].AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Minutes;
                                        chart1.ChartAreas[0].AxisX.MajorGrid.Interval = mms;    //网格间隔
                                        //====Y轴显示样式==
                                        chart1.ChartAreas[0].AxisY.LabelStyle.Interval = 100;
                                        chart1.ChartAreas[0].AxisY.MajorGrid.Interval = 100;    //网格间隔
                                    }
                                }
                            }
                        //=================================X轴刷新方式2:追踪==================================
                        if (radioButton_trackFresh.Checked == true)
                        {
                            chart1.ChartAreas[0].AxisX.Maximum = XYval_list.First().Xval.AddSeconds(1).ToOADate();   //X轴坐标后移1秒
                            if (radioButton_oneCyc.Checked == true) //显示最近30s数据
                                chart1.ChartAreas[0].AxisX.Minimum = XYval_list.First().Xval.AddSeconds(-Fresh_seconds).ToOADate();//此刻30s前作为X轴起始,只显示最近30s数据
                            else if (radioButton_allHis.Checked == true) //显示全部历史数据
                            {
                                if (chart1.Series[0].Points.Count > LimtDrawCnt) //全图点数过多,强制禁用显示全部历史数据
                                {
                                    radioButton_oneCyc.Checked = true;
                                    radioButton_allHis.Checked = false;
                                }
                                else
                                {
                                    chart1.ChartAreas[0].AxisX.Minimum = FirstDatadt.ToOADate(); //X轴起点设为第一个数据点时间
                                    //====更改X轴显示间隔
                                    double mms = (XYval_list.First().Xval.AddSeconds(Fresh_seconds) - FirstDatadt).TotalMinutes / 25;
                                    chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
                                    chart1.ChartAreas[0].AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Minutes;
                                    chart1.ChartAreas[0].AxisX.LabelStyle.Interval = mms;    //坐标值间隔分钟
                                    chart1.ChartAreas[0].AxisX.LabelStyle.IsEndLabelVisible = false;   //防止X轴坐标跳跃
                                    chart1.ChartAreas[0].AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Minutes;
                                    chart1.ChartAreas[0].AxisX.MajorGrid.Interval = mms;    //网格间隔
                                    //====Y轴显示样式==
                                    chart1.ChartAreas[0].AxisY.LabelStyle.Interval = 100;
                                    chart1.ChartAreas[0].AxisY.MajorGrid.Interval = 100;    //网格间隔
                                }
                            }
                        }

                        chart1.Series[param].Points.AddXY(XYval_list.First().Xval.ToOADate(), ydata); //XY轴打点
                        chart1.Series[param].LegendText = param + "= " + ydata.ToString(); //实时值显示在图例上
                        //CheckBox ckbx = groupBox3.Controls.Find("checkBox_" + param, true)[0] as CheckBox;
                        //ckbx.Text = param + "= " + ydata;
                        //chart1.Series[param].Enabled = ckbx.Checked;
                        XYval_pre = XYval_list.First(); //记下本次画图的点
                        if (XYval_list.Count > 0)
                            XYval_list.RemoveAt(0);  //lock (lockObj) { XYval_list.RemoveAt(0); }//画完一个点后删除掉                            
                        break;
                    }
        }

        void whileDrawChart() //回放时用线程画图,实时直播时不可用,数据太多之后界面卡
        {
            while (drawChartThread.ThreadState == System.Threading.ThreadState.Running)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    drawChart();
                });
                Delay(1); //需要加延时否则while太快卡死
                if (ReviewStatus) //说明是回放状态,线程是被查看回放打开的,那么回放加载完成后退出线程
                {
                    this.BeginInvoke((MethodInvoker)delegate
                        { chart1.Titles[0].Text = "已加载: " + ((reviewXYval_list.Count - XYval_list.Count) * 100.0 / reviewXYval_list.Count).ToString("F02") + "%  " + (reviewXYval_list.Count - XYval_list.Count) + "/" + reviewXYval_list.Count; });
                    if (XYval_list.Count == 0) //回放数据全部加载完毕,关闭线程
                    {
                        this.BeginInvoke((MethodInvoker)delegate
                            { chart1.Titles[0].Text = "记录时间: " + reviewXYval_list_first.Xval.ToString() + " -- " + reviewXYval_list_last.Xval.ToString(); });
                        ReviewStatus = false;
                        //drawChartThread.Abort(); //回放数据全部加载完毕,关闭线程
                        break;
                    }
                }

            }
        }

        private void comboBox4_TextChanged(object sender, EventArgs e) //选电机目标转速
        {
            if (comboBox4.Text == "关闭")
            {
                while (chart1.ChartAreas[0].AxisY.StripLines.Count > 0)
                    chart1.ChartAreas[0].AxisY.StripLines.Remove(chart1.ChartAreas[0].AxisY.StripLines[0]);
                label2.Focus();
                return;
            }
            double.TryParse(comboBox4.Text, out setspeed);
            maxspeed = (setspeed + 0.12) * 81;
            minspeed = (setspeed - 0.12) * 81;
            setspeed *= 81;
            label3.Text = "*81=" + setspeed.ToString();

            # region 画目标线,上限线,下线线
            chart1.ChartAreas[0].AxisY.StripLines.Clear();
            StripLine setspeedline = new StripLine();
            setspeedline.Text = string.Format("目标转速：{0:F0}", setspeed);
            setspeedline.Interval = 0;
            setspeedline.IntervalOffset = (int)setspeed;
            setspeedline.StripWidth = 1;
            setspeedline.BackColor = Color.Teal;
            setspeedline.BorderDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisY.StripLines.Add(setspeedline); //画目标线
            StripLine maxspeedline = new StripLine();
            maxspeedline.Text = string.Format("上限转速：{0:F0}", maxspeed);
            maxspeedline.Interval = 0;
            maxspeedline.IntervalOffset = (int)maxspeed;
            maxspeedline.StripWidth = 1;
            maxspeedline.BackColor = Color.DarkBlue;
            maxspeedline.BorderDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisY.StripLines.Add(maxspeedline); //画上限线
            StripLine minspeedline = new StripLine();
            minspeedline.Text = string.Format("下限转速：{0:F0}", minspeed);
            minspeedline.Interval = 0;
            minspeedline.IntervalOffset = (int)minspeed;
            minspeedline.StripWidth = 1;
            minspeedline.BackColor = Color.DarkOrange;
            minspeedline.BorderDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisY.StripLines.Add(minspeedline); //画下限线
            # endregion

            label2.Focus();
        }

        private void button_save_Click(object sender, EventArgs e) //保存记录数据
        {
            if (button_save.Text == "开始记录")
            {
                Savefilename = System.Windows.Forms.Application.StartupPath + "\\" + DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒");// +".txt";
                Savefilestream = new FileStream(Savefilename, FileMode.Append);
                Savestreamwr = new StreamWriter(Savefilestream);
                button_save.Text = "停止记录 00:00:00";
                button_save.BackColor = Color.LightGreen;
                Startsavetime = DateTime.Now;
                timer3.Start(); //记录计时定时器
                SaveStatus = true;

                button_autosize_Click(sender, e); //记录前清除
                FirstDatadt = DateTime.MinValue; //记录前清除
                XYval_list.Clear(); //记录前清除
                chart1.Series.Clear(); //记录前清除
                groupBox3.Controls.Clear(); //记录前清除
                //chart1.ChartAreas[0].AxisX.Minimum = .Xval.ToOADate();
                //chart1.ChartAreas[0].AxisX.Maximum = .Xval.ToOADate();
                chart1.ChartAreas[0].AxisY.Minimum = -600;
                chart1.ChartAreas[0].AxisY.Maximum = 500;

            }
            else if (button_save.Text.StartsWith("停止记录")) //正在记录中
            {
                SaveStatus = false;
                button_save.Text = "开始记录";
                button_save.BackColor = Color.Orange;
                timer3.Stop();
                Savestreamwr.Close();
                #region 比较起止时间后 改文件名
                FileInfo nfi = new FileInfo(Savefilestream.Name);
                string begyyyy = Savefilestream.Name.Substring(Savefilestream.Name.LastIndexOf("\\") + 1).Split('年')[0];
                string begMM = Savefilestream.Name.Split('年')[1].Split('月')[0];
                string begdd = Savefilestream.Name.Split('月')[1].Split('日')[0];
                string begHH = Savefilestream.Name.Split('日')[1].Split('时')[0];
                string begmm = Savefilestream.Name.Split('时')[1].Split('分')[0];
                string begss = Savefilestream.Name.Split('分')[1].Split('秒')[0];
                string endtime = DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒");
                string endyyyy = endtime.Split('年')[0];
                string endMM = endtime.Split('年')[1].Split('月')[0];
                string enddd = endtime.Split('月')[1].Split('日')[0];
                string endHH = endtime.Split('日')[1].Split('时')[0];
                string endmm = endtime.Split('时')[1].Split('分')[0];
                string endss = endtime.Split('分')[1].Split('秒')[0];
                //文件名=开始记录时间-停止记录时间(不同时刻)
                //string showyyyy = begyyyy == endyyyy ? "" : endyyyy + "年"; 
                //string showMM = begMM == endMM ? "" : endMM + "月";
                //string showdd = begdd == enddd ? "" : enddd + "日";
                //string showHH = begHH == endHH ? "" : endHH + "时";
                //string showmm = begmm == endmm ? "" : endmm + "分";
                //string showss = begss == endss ? "" : endss + "秒";                
                //nfi.MoveTo(Savefilestream.Name.Replace(".txt", "") + "-" + showyyyy + showMM + showdd + showHH + showmm + showss + ".txt");
                //文件名=开始记录时间(记录时长)
                DateTime begt = Convert.ToDateTime(begyyyy + "-" + begMM + "-" + begdd + " " + begHH + ":" + begmm + ":" + begss);
                DateTime endt = Convert.ToDateTime(endyyyy + "-" + endMM + "-" + enddd + " " + endHH + ":" + endmm + ":" + endss);
                TimeSpan tsbe = endt - begt;
                string showdd = tsbe.Days == 0 ? "" : tsbe.Days + "日";
                string showHH = tsbe.Hours == 0 ? "" : tsbe.Hours + "时";
                string showmm = tsbe.Minutes == 0 ? "" : tsbe.Minutes + "分";
                string showss = tsbe.Seconds == 0 ? "" : tsbe.Seconds + "秒";
                nfi.MoveTo(Savefilestream.Name.Replace(".txt", "") + "(时长" + showdd + showHH + showmm + showss + ").txt");
                #endregion
                Savefilestream.Close();

            }
        }

        private void radioButton_oneCyc_Click(object sender, EventArgs e) //chart曲线图只显示当前一段时间的数据
        {
            chart1.ChartAreas[0].AxisX.Maximum = XYval_pre.Xval.AddSeconds(Fresh_seconds).ToOADate();
            chart1.ChartAreas[0].AxisX.Minimum = XYval_pre.Xval.ToOADate();
        }

        private void radioButton_allHis_Click(object sender, EventArgs e)//chart曲线图显示所有历史数据
        {
            if (chart1.Series.Count == 0)
                return;
            //if (((TimeSpan)(DateTime.Now - FirstDatadt)).TotalMinutes < 30) //全图时间限制
            if (chart1.Series[0].Points.Count < LimtDrawCnt)  //全图总点限制   
                chart1.ChartAreas[0].AxisX.Minimum = FirstDatadt.ToOADate(); //X轴起点设为第一个数据点时间
            else
            {
                radioButton_allHis.Checked = false;
                radioButton_oneCyc.Checked = true;
                MessageBox.Show("全部历史数据较大,无法全部显示");
            }
        }

        private void ckb_CheckedChanged(object sender, EventArgs e) //选择显示哪些参数曲线
        {
            CheckBox ck = sender as CheckBox;
            Debug.WriteLine(ck.Name + " " + ck.Checked);
            chart1.Series[ck.Name.Replace("checkBox_", "")].Enabled = ck.Checked;
        }

        private void button_review_Click(object sender, EventArgs e) //回放已保存记录的数据
        {
            string path = "";
            OpenFileDialog fileDialog = new OpenFileDialog();
            //fileDialog.InitialDirectory="C:\\";    //打开对话框后的初始目录
            fileDialog.Filter = "文本文件|*.txt|所有文件|*.*";
            fileDialog.RestoreDirectory = false;    //若为false，则打开对话框后为上次的目录。若为true，则为初始目录
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {

                reviewXYval_list.Clear();
                path = System.IO.Path.GetFullPath(fileDialog.FileName);//将选中的文件的路径
                StreamReader sr = new StreamReader(path, Encoding.Default);
                String line;
                DateTime dt = DateTime.MinValue;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("@")) //自己的记录数据文件
                    {
                        string dtstr = line.Split('@')[0];
                        string valstr = line.Split('@')[1];
                        string[] strArr = dtstr.Split(new char[] { '-', ' ', ':' });
                        if (strArr.Length == 7)
                        {
                            dt = new DateTime(int.Parse(strArr[0]), int.Parse(strArr[1]), int.Parse(strArr[2]), int.Parse(strArr[3]),
                                               int.Parse(strArr[4]), int.Parse(strArr[5]), int.Parse(strArr[6]));
                            foreach (string param in Params)
                                if (valstr.StartsWith(param) == true)
                                {
                                    XYvalue xyv;
                                    xyv.Xval = dt; xyv.Yval = valstr;
                                    reviewXYval_list.Add(xyv);
                                    break;
                                }
                        }
                    }
                    else if (line.Contains("[") && line.Contains("]")) //读sscom数据文件的时间
                    {
                        string dtstr = line.Substring(line.IndexOf("[") + 1, line.IndexOf("]") - line.IndexOf("[") - 1);
                        string[] strArr = dtstr.Split(new char[] { '.', ' ', ':' });
                        dt = new DateTime(1999, 09, 09, int.Parse(strArr[0]), int.Parse(strArr[1]), int.Parse(strArr[2]), int.Parse(strArr[3]));
                    }
                    else//读sscom数据的参数值
                    {
                        foreach (string param in Params)
                            if (line.StartsWith(param) == true && Regex.IsMatch(line.Last().ToString(), @"^\d+$"))
                            {
                                XYvalue xyv;
                                xyv.Xval = dt; xyv.Yval = line.Trim(); ;
                                reviewXYval_list.Add(xyv);
                                break;
                            }
                    }
                }
                if (reviewXYval_list.Count > 0)
                {
                    ReviewStatus = false;
                    //drawChartThread.Abort();//先停止前一个回放
                    reviewXYval_list_first = reviewXYval_list.First();
                    reviewXYval_list_last = reviewXYval_list.Last();
                    button_autosize_Click(sender, e); //加载前清除
                    FirstDatadt = DateTime.MinValue; //加载前清除
                    XYval_list.Clear(); //加载前清除
                    XYval_list = new List<XYvalue>(reviewXYval_list.ToArray());
                    chart1.Series.Clear(); //加载前清除
                    groupBox3.Controls.Clear(); //加载前清除
                    chart1.ChartAreas[0].AxisX.Minimum = XYval_list.First().Xval.ToOADate();
                    chart1.ChartAreas[0].AxisX.Maximum = XYval_list.Last().Xval.ToOADate();
                    chart1.ChartAreas[0].AxisY.Minimum = -600;
                    chart1.ChartAreas[0].AxisY.Maximum = 500;
                    //设置标题
                    chart1.Titles.Clear();
                    chart1.Titles.Add("S01");
                    chart1.Titles[0].Text = "记录时间: " + reviewXYval_list_first.Xval.ToString() + " -- " + reviewXYval_list_last.Xval.ToString();
                    chart1.Titles[0].ForeColor = Color.DarkRed;
                    chart1.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
                    while (XYval_list.Count > 0) //回放时在主线程画图表
                    {
                        drawChart();
                        //Delay(1);
                        chart1.Titles[0].Text = "已加载: " + ((reviewXYval_list.Count - XYval_list.Count) * 100.0 / reviewXYval_list.Count).ToString("F02") + "%  " + (reviewXYval_list.Count - XYval_list.Count) + "/" + reviewXYval_list.Count;
                        if (XYval_list.Count == 0) //回放数据全部加载完毕
                            chart1.Titles[0].Text = "记录时间: " + reviewXYval_list_first.Xval.ToString() + " -- " + reviewXYval_list_last.Xval.ToString();
                    }
                    //ReviewStatus = true;
                    //drawChartThread.Start();//回放时开线程画图
                    button_autosize.Enabled = true;
                }
            }
        }

        private void button_sendcmd_Click(object sender, EventArgs e) //发送命令
        {
            string cmdstr;
            if (checkBox_HEXsend.Checked == true)
            {
                cmdstr = comboBox_cmd.Text.Replace("3B通气", "").Replace("3B关气", "").Replace("11通气", "").Replace("11关气", "").Replace("修改KB", "").Trim();
                try
                {
                    byte[] cmdbyteArray = cmdstr.Split(' ').Select(x => Convert.ToByte(x, 16)).ToArray();
                    serialPort1.Write(cmdbyteArray, 0, cmdbyteArray.Length);
                    //textBox_HEX.AppendText("\r\n" + DateTime.Now.ToString("HH:mm:ss:fff -->>>\r\n") + cmdstr + "\r\n");
                    richTextBox_HEX.AppendText(DateTime.Now.ToString("HH:mm:ss:fff -->>>\r\n"));
                    richTextBox_HEX.SelectionColor = Color.Red;
                    richTextBox_HEX.SelectionFont = new Font("宋体", 12F, FontStyle.Regular);
                    richTextBox_HEX.AppendText(cmdstr + "\r\n");
                }
                catch (Exception err) { MessageBox.Show("只能输入十六进制数值,空格间隔"); }
            }
            else
            {
                cmdstr = comboBox_cmd.Text;
                serialPort1.WriteLine(cmdstr);
                //textBox_HEX.AppendText("\r\n" + DateTime.Now.ToString("HH:mm:ss:fff <<<--\r\n") + cmdstr + "\r\n");
                richTextBox_HEX.SelectionColor = Color.Red;
                richTextBox_HEX.AppendText("\r\n" + DateTime.Now.ToString("HH:mm:ss:fff <<<--\r\n") + cmdstr + "\r\n");
            }
        }

        private void timer3_Tick(object sender, EventArgs e) //记录数据时间计时器
        {
            if (button_save.Text.StartsWith("停止记录")) //正在记录中
            {
                TimeSpan ff = DateTime.Now - Startsavetime; //Math.Round((double)(ff.Seconds + ff.Milliseconds),0)
                button_save.Text = "停止记录 " + ff.Hours.ToString("00") + ":" + ff.Minutes.ToString("00") + ":" + (ff.Seconds + ff.Milliseconds * 0.001).ToString("00");

            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            label2.Focus();
        }

        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            if (button1.Text == "打开串口")
                return;
            HitTestResult Result = new HitTestResult();
            Result = chart1.HitTest(e.X, e.Y);
            if (Result.Series != null) //点击处有曲线则获取曲线点坐标值
            {
                double xValue = Result.Series.Points[Result.PointIndex].XValue;
                double yValue = Result.Series.Points[Result.PointIndex].YValues[0];
                label1.Text = "曲线点 " + DateTime.FromOADate(xValue).ToString("yyyy-MM-dd HH:mm:ss:fff") + "  " + yValue.ToString();
            }
            else //点击处无曲线则获取空白处的坐标值
            {
                double xValue = chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
                double yValue = chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);
                label1.Text = "空白处 " + DateTime.FromOADate(xValue).ToString("yyyy-MM-dd HH:mm:ss:fff") + "  " + yValue.ToString(".00");
            }
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (button1.Text == "打开串口")
                return;
            Point mousePoint = new Point(e.X, e.Y);
            chart1.ChartAreas[0].CursorX.SetCursorPixelPosition(mousePoint, true);  //位置处显示红竖线
            chart1.ChartAreas[0].CursorY.SetCursorPixelPosition(mousePoint, true);  //位置处显示红横线            
        }

        private void chart1_GetToolTipText(object sender, ToolTipEventArgs e) //移动鼠标在chart上显示曲线坐标点
        {
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                int i = e.HitTestResult.PointIndex;
                DataPoint dp = e.HitTestResult.Series.Points[i];
                e.Text = DateTime.FromOADate(dp.XValue).ToString("yyyy-MM-dd HH:mm:ss:fff") + "  " + dp.YValues[0].ToString();
            }
        }

        private void chart1_MouseLeave(object sender, EventArgs e)
        {
            Point mousePoint = new Point(0, 999);
            chart1.ChartAreas[0].CursorX.SetCursorPixelPosition(mousePoint, true);
            chart1.ChartAreas[0].CursorY.SetCursorPixelPosition(mousePoint, true);
        }

        #region 计算KB值等
        long kz = 0, bz = 0;
        void kzbzoxy_cal(object sender, EventArgs e) //计算KB值,氧分压值
        {            
            TextBox tb = sender as TextBox;
            if (tb.Name != "textBox_Tva" && tb.Name != "textBox_kz" && tb.Name != "textBox_bz")
            {
                //double oxy1 = 0.209, oxy2 = 0.75, dqy = 101.5;
                //int tva1 = 0x287FF8, tva2 = 0x9634BC;
                double oxy1 = double.Parse(textBox_oxy1.Text) * 0.01, oxy2 = double.Parse(textBox_oxy2.Text) * 0.01, dqy = (double)numericUpDown1.Value;
                if (textBox_t1.Text != "" && textBox_t2.Text != "" && textBox_t1.Text != textBox_t2.Text && oxy2 != oxy1)
                {
                    int tva1 = Convert.ToInt32(textBox_t1.Text, 16);
                    int tva2 = Convert.ToInt32(textBox_t2.Text, 16);
                    kz = (long)((oxy2 - oxy1) * dqy / (tva2 - tva1) * 4294967296);
                    //bz = (int)((oxy1 * dqy - tva1 / (tva2 - tva1) * (oxy2 - oxy1) * dqy)*256 );
                    bz = (int)(dqy*oxy1-kz*tva1);
                }
                textBox_kz.Text = kz.ToString("X2");
                textBox_bz.Text = bz.ToString("X2");
            }

            
            //int Tva = 0x103FFF7;
            int Tva = Convert.ToInt32(textBox_Tva.Text == "" ? "0" : textBox_Tva.Text, 16);
            kz = Convert.ToInt64(textBox_kz.Text == "" ? "0" : textBox_kz.Text, 16);
            bz = Convert.ToInt64(textBox_bz.Text == "" ? "0" : textBox_bz.Text, 16);
            long oxytemp = kz * Tva + bz;
            int oxy = (int)(oxytemp / 4294967296.0 * 10);
            label_oxy.Text ="氧分压= "+(oxy * 0.1).ToString("F01") +"kpa";
            
        }

        private void tb1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Regex.IsMatch(text.Replace("-", ""), @"^\d+$") //匹配是否是数字
            TextBox tb = sender as TextBox;
            if (e.KeyChar == 13)//回车
            {
                label1.Focus();
                tb.BackColor = Color.White;
                return;
            }
            e.Handled = true;
            if (e.KeyChar == 8)
                e.Handled = false;
            if ("0123456789".IndexOf(e.KeyChar) >= 0 && !tb.Text.Substring(tb.SelectionStart + tb.SelectionLength).Contains("-"))
                e.Handled = false;//数字不能在负号之前                     
            if (e.KeyChar == '-' && tb.SelectionStart == 0 && !tb.Text.Substring(tb.SelectionStart + tb.SelectionLength).Contains("-"))
                e.Handled = false; //负号不能不是第一位,且未被选中部分不能已经存在负号了
            if (e.KeyChar == 46 && Regex.IsMatch(tb.Text.Substring(0, tb.SelectionStart).Replace("-", ""), @"^\d+$") && !tb.Text.Substring(0, tb.SelectionStart).Contains(".") && !tb.Text.Substring(tb.SelectionStart + tb.SelectionLength).Contains("."))
                e.Handled = false;  //小数点之前必须已经有数字,且未被选中部分不能已经存在点了
            if (e.KeyChar == '-')
                e.Handled = true;//不能是负号
        }

        private void tb2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                label1.Focus();
                return;
            }
            e.Handled = "0123456789ABCDEF".IndexOf(char.ToUpper(e.KeyChar)) < 0;
            e.KeyChar = char.ToUpper(e.KeyChar);
            if (e.KeyChar == 8)
                e.Handled = false;
        }

        private void textBox_oxy1_KeyPress(object sender, KeyPressEventArgs e)
        {
            tb1_KeyPress(sender, e);
        }

        private void textBox_t1_KeyPress(object sender, KeyPressEventArgs e)
        {
            tb2_KeyPress(sender, e);
        }

        private void textBox_oxy2_KeyPress(object sender, KeyPressEventArgs e)
        {
            tb1_KeyPress(sender, e);
        }

        private void textBox_t2_KeyPress(object sender, KeyPressEventArgs e)
        {
            tb2_KeyPress(sender, e);
        }

        private void textBox_kz_KeyPress(object sender, KeyPressEventArgs e)
        {
            tb2_KeyPress(sender, e);
        }

        private void textBox_bz_KeyPress(object sender, KeyPressEventArgs e)
        {
            tb2_KeyPress(sender, e);
        }

        private void textBox_Tva_KeyPress(object sender, KeyPressEventArgs e)
        {
            tb2_KeyPress(sender, e);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            kzbzoxy_cal(sender, e);
        }

        private void textBox_oxy1_TextChanged(object sender, EventArgs e)
        {
            kzbzoxy_cal(sender, e);
        }

        private void textBox_t1_TextChanged(object sender, EventArgs e)
        {
            kzbzoxy_cal(sender, e);
        }

        private void textBox_oxy2_TextChanged(object sender, EventArgs e)
        {
            kzbzoxy_cal(sender, e);
        }

        private void textBox_t2_TextChanged(object sender, EventArgs e)
        {
            kzbzoxy_cal(sender, e);
        }

        private void textBox_Tva_TextChanged(object sender, EventArgs e)
        {
            kzbzoxy_cal(sender, e);
        }

        private void textBox_kz_TextChanged(object sender, EventArgs e)
        {
            //kz = Convert.ToInt64(textBox_kz.Text == "" ? "0" : textBox_kz.Text, 16);
            kzbzoxy_cal(sender, e);
        }

        private void textBox_bz_TextChanged(object sender, EventArgs e)
        {
            //bz = Convert.ToInt64(textBox_bz.Text == "" ? "0" : textBox_bz.Text, 16);
            kzbzoxy_cal(sender, e);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) //textBox_Tva右键切换Tval来源
        {
            if (toolStripMenuItem1.Text == "使用接收值")
            {
                toolStripMenuItem1.Text = "手动输入";
                textBox_Tva.BackColor = Color.LightGreen;
            }
            else
            {
                toolStripMenuItem1.Text = "使用接收值";
                textBox_Tva.BackColor = Color.White;
            }
        }
        #endregion

        private void checkBox_sndOXY_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_sndOXY.Checked == true)
            {
                try
                {
                    if (kz == 0)
                    {
                        MessageBox.Show("K值不能为0");
                        checkBox_sndOXY.Checked = false;
                        return;
                    }
                    serialPort2.PortName = comboBox5.Text.Split(':')[0];
                    serialPort2.Open();
                    timer4.Start();
                }
                catch(Exception err)
                {
                    MessageBox.Show("串口打开错误");
                    checkBox_sndOXY.Checked = false;
                }
            }
            else if (checkBox_sndOXY.Checked == false)
            {
                try
                {
                    timer4.Stop();
                    serialPort2.Close();
                    
                }
                catch (Exception err)
                {
                    MessageBox.Show("关闭串口错误");
                    checkBox_sndOXY.Checked = true;
                }
            }
        }

        private void textBox_sndOXY_KeyPress(object sender, KeyPressEventArgs e)
        {
            tb1_KeyPress(sender, e);
        }
        
        private void timer4_Tick(object sender, EventArgs e) //定时发送OXY
        {
            double sndOXY = double.Parse(textBox_sndOXY.Text == "" ? "0" : textBox_sndOXY.Text);
            long sndOXYtemp = (long)(4294967296.0 * sndOXY);
            int sndTva = (int)((sndOXYtemp - bz) / kz);
            int temper = 8888;
            byte jyh = (byte)(0x100 - (byte)(0xAA + 0x55 + (byte)((sndTva >> 24) & 0xff) + (byte)((sndTva >> 16) & 0xff) + (byte)((sndTva >> 8) & 0xff)
                + (byte)(sndTva & 0xff)+ (byte)((temper >> 8) & 0xff)+ (byte)(temper & 0xff))) ;
            byte[] sndarr = new byte[] { 0xAA, 0x55, (byte)((sndTva >> 24) & 0xff), (byte)((sndTva >> 16) & 0xff), 
                (byte)((sndTva >> 8) & 0xff), (byte)(sndTva & 0xff), (byte)((temper >> 8) & 0xff), (byte)(temper & 0xff), jyh};
            if (serialPort2.IsOpen && checkBox_sndOXY.Checked == true)
                serialPort2.Write(sndarr, 0, sndarr.Length);
            else
            {
                MessageBox.Show("发送串口未打开");
                timer4.Stop();
            }
        }

        



        #region //chart1点击拖动鼠标画方框
        /*chart1.MouseDown += new MouseEventHandler(chart1_MouseDown);
        chart1.MouseMove += new MouseEventHandler(chart1_MouseMove);
        chart1.MouseUp += new MouseEventHandler(chart1_MouseUp);
        chart1.Paint += new PaintEventHandler(chart1_Paint);
        bool bDrawStart = false;
        Point pointStart = Point.Empty;
        Point pointContinue = Point.Empty;
        int chart1AxisXmax = 0;
        int chart1AxisXmin = 0;
        int chart1AxisYmax = 0;
        int chart1AxisYmin = 0;
        private void chart1_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black, 1f);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;            
            if (bDrawStart)
            {
                //实时的画矩形
                int w = pointContinue.X - pointStart.X;
                int h = pointContinue.Y - pointStart.Y;
                Rectangle rect = new Rectangle(pointStart, new Size(w, h));
                e.Graphics.DrawRectangle(pen, rect);
                
            }
            pen.Dispose();
        }

        void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            chart1AxisXmax = (int)chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
            chart1AxisYmax = (int)chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);

            if (bDrawStart)            
                bDrawStart = false;            
            else
            {
                bDrawStart = true;
                pointStart = e.Location;
            }
        }
        void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (bDrawStart)
            {
                pointContinue = e.Location;
                //Refresh();
                Invalidate();
            }
        }
        void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            if (bDrawStart)
            {
                chart1AxisXmin = (int)chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
                chart1AxisYmin = (int)chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);
                if (chart1AxisXmax < chart1AxisXmin)
                {
                    chart1AxisXmax ^= chart1AxisXmin;
                    chart1AxisXmin ^= chart1AxisXmax;
                    chart1AxisXmax ^= chart1AxisXmin;
                }
                if (chart1AxisYmax < chart1AxisYmin)
                {
                    chart1AxisYmax ^= chart1AxisYmin;
                    chart1AxisYmin ^= chart1AxisYmax;
                    chart1AxisYmax ^= chart1AxisYmin;
                }
                if ((chart1AxisYmax - chart1AxisYmin) > 10)
                {
                    //chart1.ChartAreas[0].AxisX.Maximum = chart1AxisXmax;
                    //chart1.ChartAreas[0].AxisX.Minimum = chart1AxisXmin;
                    chart1.ChartAreas[0].AxisY.Maximum = chart1AxisYmax;
                    chart1.ChartAreas[0].AxisY.Minimum = chart1AxisYmin;
                }
                chart1AxisXmax = 0;
                chart1AxisXmin = 0;
                chart1AxisYmax = 0;
                chart1AxisYmin = 0;
            }
            bDrawStart = false;
        }*/
        #endregion chart1点击画方框

    }
}
