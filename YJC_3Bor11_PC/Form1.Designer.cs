namespace YJC_3Bor11_PC
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button_Xsub = new System.Windows.Forms.Button();
            this.button_Xadd = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.button_autosize = new System.Windows.Forms.Button();
            this.button_sndPID = new System.Windows.Forms.Button();
            this.textBox_kp = new System.Windows.Forms.TextBox();
            this.textBox_ki = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_trackFresh = new System.Windows.Forms.RadioButton();
            this.radioButton_oscFresh = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton_allHis = new System.Windows.Forms.RadioButton();
            this.radioButton_oneCyc = new System.Windows.Forms.RadioButton();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.checkBox_HEXshow = new System.Windows.Forms.CheckBox();
            this.button_save = new System.Windows.Forms.Button();
            this.textBox_HEX = new System.Windows.Forms.TextBox();
            this.button_review = new System.Windows.Forms.Button();
            this.comboBox_cmd = new System.Windows.Forms.ComboBox();
            this.button_sendcmd = new System.Windows.Forms.Button();
            this.checkBox_HEXsend = new System.Windows.Forms.CheckBox();
            this.richTextBox_HEX = new System.Windows.Forms.RichTextBox();
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label_oxy = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.textBox_oxy1 = new System.Windows.Forms.TextBox();
            this.textBox_t1 = new System.Windows.Forms.TextBox();
            this.textBox_oxy2 = new System.Windows.Forms.TextBox();
            this.textBox_t2 = new System.Windows.Forms.TextBox();
            this.textBox_kz = new System.Windows.Forms.TextBox();
            this.textBox_bz = new System.Windows.Forms.TextBox();
            this.textBox_Tva = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // serialPort1
            // 
            this.serialPort1.PortName = "COM2";
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(94, 141);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Color = System.Drawing.Color.Red;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(1164, 638);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            this.chart1.GetToolTipText += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs>(this.chart1_GetToolTipText);
            this.chart1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseClick);
            this.chart1.MouseLeave += new System.EventHandler(this.chart1_MouseLeave);
            this.chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseMove);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(295, 67);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 51);
            this.button1.TabIndex = 2;
            this.button1.Text = "打开串口";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 250;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(602, 818);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "设定转速=";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(769, 818);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "*81=";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(24, 147);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(48, 52);
            this.button2.TabIndex = 6;
            this.button2.Text = "+";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            this.button2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAddSubXY_MouseDown);
            this.button2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAddSubXY_MouseUp);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(24, 216);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(48, 52);
            this.button3.TabIndex = 7;
            this.button3.Text = "-";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            this.button3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAddSubXY_MouseDown);
            this.button3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAddSubXY_MouseUp);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(24, 639);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(48, 52);
            this.button4.TabIndex = 8;
            this.button4.Text = "+";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            this.button4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAddSubXY_MouseDown);
            this.button4.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAddSubXY_MouseUp);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(24, 711);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(48, 52);
            this.button5.TabIndex = 9;
            this.button5.Text = "-";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            this.button5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAddSubXY_MouseDown);
            this.button5.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAddSubXY_MouseUp);
            // 
            // button_Xsub
            // 
            this.button_Xsub.Location = new System.Drawing.Point(390, 805);
            this.button_Xsub.Name = "button_Xsub";
            this.button_Xsub.Size = new System.Drawing.Size(70, 45);
            this.button_Xsub.TabIndex = 10;
            this.button_Xsub.Text = "-";
            this.button_Xsub.UseVisualStyleBackColor = true;
            this.button_Xsub.Click += new System.EventHandler(this.button_Xsub_Click);
            this.button_Xsub.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAddSubXY_MouseDown);
            this.button_Xsub.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAddSubXY_MouseUp);
            // 
            // button_Xadd
            // 
            this.button_Xadd.Location = new System.Drawing.Point(489, 805);
            this.button_Xadd.Name = "button_Xadd";
            this.button_Xadd.Size = new System.Drawing.Size(70, 45);
            this.button_Xadd.TabIndex = 11;
            this.button_Xadd.Text = "+";
            this.button_Xadd.UseVisualStyleBackColor = true;
            this.button_Xadd.Click += new System.EventHandler(this.button_Xadd_Click);
            this.button_Xadd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAddSubXY_MouseDown);
            this.button_Xadd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAddSubXY_MouseUp);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 18);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(383, 29);
            this.comboBox1.TabIndex = 12;
            this.comboBox1.DropDown += new System.EventHandler(this.comboBox1_DropDown);
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "9600",
            "115200"});
            this.comboBox2.Location = new System.Drawing.Point(12, 73);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(97, 29);
            this.comboBox2.TabIndex = 13;
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "无校验",
            "奇校验",
            "偶校验"});
            this.comboBox3.Location = new System.Drawing.Point(147, 73);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(100, 29);
            this.comboBox3.TabIndex = 14;
            // 
            // button_autosize
            // 
            this.button_autosize.Location = new System.Drawing.Point(24, 330);
            this.button_autosize.Name = "button_autosize";
            this.button_autosize.Size = new System.Drawing.Size(48, 52);
            this.button_autosize.TabIndex = 15;
            this.button_autosize.Text = "自动";
            this.button_autosize.UseVisualStyleBackColor = true;
            this.button_autosize.Visible = false;
            this.button_autosize.Click += new System.EventHandler(this.button_autosize_Click);
            // 
            // button_sndPID
            // 
            this.button_sndPID.Location = new System.Drawing.Point(784, 873);
            this.button_sndPID.Name = "button_sndPID";
            this.button_sndPID.Size = new System.Drawing.Size(75, 34);
            this.button_sndPID.TabIndex = 16;
            this.button_sndPID.Text = "发pid";
            this.button_sndPID.UseVisualStyleBackColor = true;
            this.button_sndPID.Click += new System.EventHandler(this.button_sndPID_Click);
            // 
            // textBox_kp
            // 
            this.textBox_kp.Location = new System.Drawing.Point(701, 856);
            this.textBox_kp.Name = "textBox_kp";
            this.textBox_kp.Size = new System.Drawing.Size(65, 28);
            this.textBox_kp.TabIndex = 17;
            this.textBox_kp.Text = "0.03";
            // 
            // textBox_ki
            // 
            this.textBox_ki.Location = new System.Drawing.Point(701, 893);
            this.textBox_ki.Name = "textBox_ki";
            this.textBox_ki.Size = new System.Drawing.Size(65, 28);
            this.textBox_ki.TabIndex = 18;
            this.textBox_ki.Text = "0.006";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(663, 861);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 18);
            this.label4.TabIndex = 19;
            this.label4.Text = "kp=";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(663, 898);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 18);
            this.label5.TabIndex = 20;
            this.label5.Text = "ki=";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_trackFresh);
            this.groupBox1.Controls.Add(this.radioButton_oscFresh);
            this.groupBox1.Location = new System.Drawing.Point(418, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(143, 91);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            // 
            // radioButton_trackFresh
            // 
            this.radioButton_trackFresh.AutoSize = true;
            this.radioButton_trackFresh.Location = new System.Drawing.Point(11, 57);
            this.radioButton_trackFresh.Name = "radioButton_trackFresh";
            this.radioButton_trackFresh.Size = new System.Drawing.Size(105, 22);
            this.radioButton_trackFresh.TabIndex = 1;
            this.radioButton_trackFresh.Text = "追踪刷新";
            this.radioButton_trackFresh.UseVisualStyleBackColor = true;
            // 
            // radioButton_oscFresh
            // 
            this.radioButton_oscFresh.AutoSize = true;
            this.radioButton_oscFresh.Checked = true;
            this.radioButton_oscFresh.Location = new System.Drawing.Point(11, 21);
            this.radioButton_oscFresh.Name = "radioButton_oscFresh";
            this.radioButton_oscFresh.Size = new System.Drawing.Size(123, 22);
            this.radioButton_oscFresh.TabIndex = 0;
            this.radioButton_oscFresh.TabStop = true;
            this.radioButton_oscFresh.Text = "示波器刷新";
            this.radioButton_oscFresh.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton_allHis);
            this.groupBox2.Controls.Add(this.radioButton_oneCyc);
            this.groupBox2.Location = new System.Drawing.Point(583, 9);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(128, 91);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            // 
            // radioButton_allHis
            // 
            this.radioButton_allHis.AutoSize = true;
            this.radioButton_allHis.Location = new System.Drawing.Point(13, 58);
            this.radioButton_allHis.Name = "radioButton_allHis";
            this.radioButton_allHis.Size = new System.Drawing.Size(105, 22);
            this.radioButton_allHis.TabIndex = 1;
            this.radioButton_allHis.Text = "全部历史";
            this.radioButton_allHis.UseVisualStyleBackColor = true;
            this.radioButton_allHis.Click += new System.EventHandler(this.radioButton_allHis_Click);
            // 
            // radioButton_oneCyc
            // 
            this.radioButton_oneCyc.AutoSize = true;
            this.radioButton_oneCyc.Checked = true;
            this.radioButton_oneCyc.Location = new System.Drawing.Point(12, 22);
            this.radioButton_oneCyc.Name = "radioButton_oneCyc";
            this.radioButton_oneCyc.Size = new System.Drawing.Size(105, 22);
            this.radioButton_oneCyc.TabIndex = 0;
            this.radioButton_oneCyc.TabStop = true;
            this.radioButton_oneCyc.Text = "当前周期";
            this.radioButton_oneCyc.UseVisualStyleBackColor = true;
            this.radioButton_oneCyc.Click += new System.EventHandler(this.radioButton_oneCyc_Click);
            // 
            // timer2
            // 
            this.timer2.Interval = 1;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(1267, 131);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(144, 648);
            this.groupBox3.TabIndex = 24;
            this.groupBox3.TabStop = false;
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Items.AddRange(new object[] {
            "关闭",
            "5",
            "7.5",
            "9"});
            this.comboBox4.Location = new System.Drawing.Point(694, 814);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(72, 26);
            this.comboBox4.TabIndex = 25;
            this.comboBox4.TextChanged += new System.EventHandler(this.comboBox4_TextChanged);
            // 
            // checkBox_HEXshow
            // 
            this.checkBox_HEXshow.AutoSize = true;
            this.checkBox_HEXshow.Location = new System.Drawing.Point(1421, 113);
            this.checkBox_HEXshow.Name = "checkBox_HEXshow";
            this.checkBox_HEXshow.Size = new System.Drawing.Size(97, 22);
            this.checkBox_HEXshow.TabIndex = 26;
            this.checkBox_HEXshow.Text = "HEX显示";
            this.checkBox_HEXshow.UseVisualStyleBackColor = true;
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(734, 25);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(123, 65);
            this.button_save.TabIndex = 27;
            this.button_save.Text = "开始记录";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // textBox_HEX
            // 
            this.textBox_HEX.Location = new System.Drawing.Point(1267, 107);
            this.textBox_HEX.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_HEX.Multiline = true;
            this.textBox_HEX.Name = "textBox_HEX";
            this.textBox_HEX.ReadOnly = true;
            this.textBox_HEX.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_HEX.Size = new System.Drawing.Size(56, 26);
            this.textBox_HEX.TabIndex = 316;
            this.textBox_HEX.Visible = false;
            // 
            // button_review
            // 
            this.button_review.Location = new System.Drawing.Point(880, 33);
            this.button_review.Name = "button_review";
            this.button_review.Size = new System.Drawing.Size(93, 48);
            this.button_review.TabIndex = 317;
            this.button_review.Text = "查看记录";
            this.button_review.UseVisualStyleBackColor = true;
            this.button_review.Click += new System.EventHandler(this.button_review_Click);
            // 
            // comboBox_cmd
            // 
            this.comboBox_cmd.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_cmd.FormattingEnabled = true;
            this.comboBox_cmd.Items.AddRange(new object[] {
            "BB 00 B0",
            "BB 02 B2",
            "3B通气 55 AA 01 00 00 00 00 00 00 00 00 00",
            "3B关气 55 AA 00 00 00 00 00 00 00 00 00 01",
            "11通气 AA 55 00 00 62 02 00 00 00 00 00 9D",
            "11关气 AA 55 00 00 EA 01 00 00 00 00 00 16",
            "修改KB CC 11 22 33 44 55 66 77 89 C9",
            "修改KB CC 00 00 80 23 00 00 00 F1 C1"});
            this.comboBox_cmd.Location = new System.Drawing.Point(994, 37);
            this.comboBox_cmd.Name = "comboBox_cmd";
            this.comboBox_cmd.Size = new System.Drawing.Size(550, 30);
            this.comboBox_cmd.TabIndex = 318;
            // 
            // button_sendcmd
            // 
            this.button_sendcmd.Location = new System.Drawing.Point(1565, 33);
            this.button_sendcmd.Name = "button_sendcmd";
            this.button_sendcmd.Size = new System.Drawing.Size(90, 41);
            this.button_sendcmd.TabIndex = 319;
            this.button_sendcmd.Text = "发送";
            this.button_sendcmd.UseVisualStyleBackColor = true;
            this.button_sendcmd.Click += new System.EventHandler(this.button_sendcmd_Click);
            // 
            // checkBox_HEXsend
            // 
            this.checkBox_HEXsend.AutoSize = true;
            this.checkBox_HEXsend.Checked = true;
            this.checkBox_HEXsend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_HEXsend.Location = new System.Drawing.Point(1563, 113);
            this.checkBox_HEXsend.Name = "checkBox_HEXsend";
            this.checkBox_HEXsend.Size = new System.Drawing.Size(97, 22);
            this.checkBox_HEXsend.TabIndex = 320;
            this.checkBox_HEXsend.Text = "HEX发送";
            this.checkBox_HEXsend.UseVisualStyleBackColor = true;
            // 
            // richTextBox_HEX
            // 
            this.richTextBox_HEX.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBox_HEX.HideSelection = false;
            this.richTextBox_HEX.Location = new System.Drawing.Point(1422, 142);
            this.richTextBox_HEX.Name = "richTextBox_HEX";
            this.richTextBox_HEX.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox_HEX.Size = new System.Drawing.Size(238, 637);
            this.richTextBox_HEX.TabIndex = 321;
            this.richTextBox_HEX.Text = "";
            // 
            // timer3
            // 
            this.timer3.Interval = 1000;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(440, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 18);
            this.label1.TabIndex = 322;
            this.label1.Text = "label1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(902, 804);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 18);
            this.label6.TabIndex = 323;
            this.label6.Text = "大气压(kpa):";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(910, 848);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 18);
            this.label7.TabIndex = 324;
            this.label7.Text = "氧浓度1(%):";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1094, 849);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 18);
            this.label8.TabIndex = 325;
            this.label8.Text = "T值1:0x";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(910, 895);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(107, 18);
            this.label9.TabIndex = 326;
            this.label9.Text = "氧浓度2(%):";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1094, 894);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 18);
            this.label10.TabIndex = 327;
            this.label10.Text = "T值2:0x";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(1303, 849);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 18);
            this.label11.TabIndex = 328;
            this.label11.Text = "K值:0x";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1303, 894);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 18);
            this.label12.TabIndex = 329;
            this.label12.Text = "B值:0x";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(1285, 806);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 18);
            this.label13.TabIndex = 330;
            this.label13.Text = "Tva值:0x";
            // 
            // label_oxy
            // 
            this.label_oxy.AutoSize = true;
            this.label_oxy.Location = new System.Drawing.Point(1490, 849);
            this.label_oxy.Name = "label_oxy";
            this.label_oxy.Size = new System.Drawing.Size(107, 18);
            this.label_oxy.TabIndex = 331;
            this.label_oxy.Text = "氧分压(kpa)";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 1;
            this.numericUpDown1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown1.Location = new System.Drawing.Point(1020, 800);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(81, 28);
            this.numericUpDown1.TabIndex = 332;
            this.numericUpDown1.Value = new decimal(new int[] {
            1015,
            0,
            0,
            65536});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // textBox_oxy1
            // 
            this.textBox_oxy1.Location = new System.Drawing.Point(1020, 844);
            this.textBox_oxy1.Name = "textBox_oxy1";
            this.textBox_oxy1.Size = new System.Drawing.Size(57, 28);
            this.textBox_oxy1.TabIndex = 333;
            this.textBox_oxy1.Text = "20.9";
            this.textBox_oxy1.TextChanged += new System.EventHandler(this.textBox_oxy1_TextChanged);
            this.textBox_oxy1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_oxy1_KeyPress);
            // 
            // textBox_t1
            // 
            this.textBox_t1.Location = new System.Drawing.Point(1168, 845);
            this.textBox_t1.Name = "textBox_t1";
            this.textBox_t1.Size = new System.Drawing.Size(99, 28);
            this.textBox_t1.TabIndex = 334;
            this.textBox_t1.TextChanged += new System.EventHandler(this.textBox_t1_TextChanged);
            this.textBox_t1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_t1_KeyPress);
            // 
            // textBox_oxy2
            // 
            this.textBox_oxy2.Location = new System.Drawing.Point(1019, 891);
            this.textBox_oxy2.Name = "textBox_oxy2";
            this.textBox_oxy2.Size = new System.Drawing.Size(57, 28);
            this.textBox_oxy2.TabIndex = 335;
            this.textBox_oxy2.Text = "75";
            this.textBox_oxy2.TextChanged += new System.EventHandler(this.textBox_oxy2_TextChanged);
            this.textBox_oxy2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_oxy2_KeyPress);
            // 
            // textBox_t2
            // 
            this.textBox_t2.Location = new System.Drawing.Point(1168, 889);
            this.textBox_t2.Name = "textBox_t2";
            this.textBox_t2.Size = new System.Drawing.Size(99, 28);
            this.textBox_t2.TabIndex = 336;
            this.textBox_t2.TextChanged += new System.EventHandler(this.textBox_t2_TextChanged);
            this.textBox_t2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_t2_KeyPress);
            // 
            // textBox_kz
            // 
            this.textBox_kz.Location = new System.Drawing.Point(1368, 844);
            this.textBox_kz.Name = "textBox_kz";
            this.textBox_kz.Size = new System.Drawing.Size(99, 28);
            this.textBox_kz.TabIndex = 337;
            this.textBox_kz.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_kz_KeyPress);
            // 
            // textBox_bz
            // 
            this.textBox_bz.Location = new System.Drawing.Point(1368, 889);
            this.textBox_bz.Name = "textBox_bz";
            this.textBox_bz.Size = new System.Drawing.Size(99, 28);
            this.textBox_bz.TabIndex = 338;
            this.textBox_bz.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_bz_KeyPress);
            // 
            // textBox_Tva
            // 
            this.textBox_Tva.Location = new System.Drawing.Point(1368, 800);
            this.textBox_Tva.Name = "textBox_Tva";
            this.textBox_Tva.Size = new System.Drawing.Size(99, 28);
            this.textBox_Tva.TabIndex = 339;
            this.textBox_Tva.TextChanged += new System.EventHandler(this.textBox_Tva_TextChanged);
            this.textBox_Tva.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_Tva_KeyPress);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1763, 962);
            this.Controls.Add(this.textBox_Tva);
            this.Controls.Add(this.textBox_bz);
            this.Controls.Add(this.textBox_kz);
            this.Controls.Add(this.textBox_t2);
            this.Controls.Add(this.textBox_oxy2);
            this.Controls.Add(this.textBox_t1);
            this.Controls.Add(this.textBox_oxy1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label_oxy);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox_HEX);
            this.Controls.Add(this.checkBox_HEXsend);
            this.Controls.Add(this.button_sendcmd);
            this.Controls.Add(this.comboBox_cmd);
            this.Controls.Add(this.button_review);
            this.Controls.Add(this.textBox_HEX);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.checkBox_HEXshow);
            this.Controls.Add(this.comboBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_ki);
            this.Controls.Add(this.textBox_kp);
            this.Controls.Add(this.button_sndPID);
            this.Controls.Add(this.button_autosize);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button_Xadd);
            this.Controls.Add(this.button_Xsub);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chart1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "YJC_3Bor11_PC";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Click += new System.EventHandler(this.Form1_Click);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button_Xsub;
        private System.Windows.Forms.Button button_Xadd;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Button button_autosize;
        private System.Windows.Forms.Button button_sndPID;
        private System.Windows.Forms.TextBox textBox_kp;
        private System.Windows.Forms.TextBox textBox_ki;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton_trackFresh;
        private System.Windows.Forms.RadioButton radioButton_oscFresh;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton_allHis;
        private System.Windows.Forms.RadioButton radioButton_oneCyc;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.CheckBox checkBox_HEXshow;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.TextBox textBox_HEX;
        private System.Windows.Forms.Button button_review;
        private System.Windows.Forms.ComboBox comboBox_cmd;
        private System.Windows.Forms.Button button_sendcmd;
        private System.Windows.Forms.CheckBox checkBox_HEXsend;
        private System.Windows.Forms.RichTextBox richTextBox_HEX;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label_oxy;
        internal System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.TextBox textBox_oxy1;
        private System.Windows.Forms.TextBox textBox_t1;
        private System.Windows.Forms.TextBox textBox_oxy2;
        private System.Windows.Forms.TextBox textBox_t2;
        private System.Windows.Forms.TextBox textBox_kz;
        private System.Windows.Forms.TextBox textBox_bz;
        private System.Windows.Forms.TextBox textBox_Tva;
        
    }
}

