using System;
using System.ComponentModel;
using System.Windows.Forms;
using static wise4051irda.clsWise4051;
using static wise4051irda.clswise4051device;
using static wise4051irda.clsNetwork;



namespace wise4051irda
{
    public partial class Form1 : Form
    {
        clsWise4051 clsWise4051 = new clsWise4051();
        clsNetwork clsNetwork = new clsNetwork();
        delegate void SetTextCallback(Control ctl, String str);
        private bool Formisclose = false;
        private String[] wise_ip = { };
        private String[] wise_id = { };
        private String[] wise_api= { };
        private BackgroundWorker[] backgroundWorker = { };
        private WISE4051Device[] WISE4051Device = new WISE4051Device[1];
        private Timer[] timers = new Timer[1];
        private String[] DIs;
        int DIValconut = 0;
        private const int pin= 8;
        //接腳電位記錄
        private int[,] ch0 = new int[1, 1];
        private int[,] ch1 = new int[1, 1];
        private int[,] ch2 = new int[1, 1];
        private int[,] ch3 = new int[1, 1];
        private int[,] ch4 = new int[1, 1];
        private int[,] ch5 = new int[1, 1];
        private int[,] ch6 = new int[1, 1];
        private int[,] ch7 = new int[1, 1];

        private int[,] l_ch0 = new int[1, 1];
        private int[,] l_ch1 = new int[1, 1];
        private int[,] l_ch2 = new int[1, 1];
        private int[,] l_ch3 = new int[1, 1];
        private int[,] l_ch4 = new int[1, 1];
        private int[,] l_ch5 = new int[1, 1];
        private int[,] l_ch6 = new int[1, 1];
        private int[,] l_ch7 = new int[1, 1];
        public Form1()
        {
            InitializeComponent();
            clsNetwork.GeadWiseIp(ref wise_id, ref wise_ip,ref wise_api);
            InitializeObjects(wise_id.Length);
            initArrayConsts(wise_id.Length);
            initBackgroundWorker();
            initwiseid(wise_id.Length);
            initTimer();
            //backgroundWorker1.RunWorkerAsync();
        }

        private void InitializeObjects(int length)
        {
            Array.Resize(ref backgroundWorker, length);
            Array.Resize(ref timers, length);
            Array.Resize(ref WISE4051Device, length);
            for (int i=0; i < length;i++)
            {
                backgroundWorker[i] = new BackgroundWorker();
                timers[i] = new Timer();
                WISE4051Device[i] = new WISE4051Device();
            }
        }
        private void myResize1(ref int[,] changeArray, int rank0, int rank1)
        {
            int s_rank0 = changeArray.GetLength(0);
            int s_rank1 = changeArray.GetLength(1);
            int[,] array2 = new int[rank0, rank1];
            Array.Copy(changeArray, array2, s_rank0 * s_rank1);
            changeArray = array2;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        private void initwiseid(int length)
        {
            for (int i = 0; i < length; i++)
            { this.Controls.Find("label_id" + i.ToString(), true)[0].Text = wise_id[i]; }
        }
        private void initBackgroundWorker()
        {
            for (int i = 0; i < timers.Length; i++)
            {
                backgroundWorker[i].WorkerReportsProgress = true;
                backgroundWorker[i].WorkerSupportsCancellation = true;
                backgroundWorker[i].DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
                backgroundWorker[i].ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
                backgroundWorker[i].RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
                backgroundWorker[i].RunWorkerAsync(i);
            }
        }
        private void initArrayConsts(int length)
        {

            Array.Resize(ref DIs, length);
            myResize1(ref ch0,length, pin);
            myResize1(ref ch1,length, pin);
            myResize1(ref ch2,length, pin);
            myResize1(ref ch3,length, pin);
            myResize1(ref ch4,length, pin);
            myResize1(ref ch5,length, pin);
            myResize1(ref ch6,length, pin);
            myResize1(ref ch7,length, pin);
            myResize1(ref l_ch0,length, pin);
            myResize1(ref l_ch1,length, pin);
            myResize1(ref l_ch2,length, pin);
            myResize1(ref l_ch3,length, pin);
            myResize1(ref l_ch4,length, pin);
            myResize1(ref l_ch5,length, pin);
            myResize1(ref l_ch6,length, pin);
            myResize1(ref l_ch7,length, pin);
        }
        private void initTimer()
        {
            for (int i = 0; i < timers.Length; i++)
            {
                timers[i] = new Timer();
                timers[i].Interval = 500;
                timers[i].Tag = i.ToString();
                timers[i].Tick += new EventHandler(Timer_Tick);
                timers[i].Enabled = true;
            }
        }
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int bk_no = (int)e.Argument;
            e.Result = bk_no;
            if (Formisclose == false)
                settext(this.Controls.Find("label_data" + bk_no.ToString(), true)[0], DIs[bk_no]);
            System.Threading.Thread.Sleep(500);
            backgroundWorker[bk_no].ReportProgress(bk_no);
        }

        //處理進度
        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int i = (int)e.ProgressPercentage;
            string status = "";
            int startsec = 0;
            int pausesec = 0;
            int stopsec = 0;
            int closesec = 0;
            if (Formisclose == false)
            {
                //status欄位
                if (ch0[i, 0] == 0 && ch1[i, 1] == 0 && ch2[i, 2] == 0)
                { status = "-2"; startsec = 0; pausesec = 0; stopsec = 0; closesec = 1; }
                else
                {
                    if (ch0[i, 0] == 1) { status = "-1"; startsec = 0; pausesec = 0; stopsec = 1; closesec = 0; }
                    if (ch1[i, 1] == 1) { status = "0"; startsec = 0; pausesec = 1; stopsec = 0; closesec = 0; }
                    if (ch2[i, 2] == 1) { status = "1"; ; startsec = 1; pausesec = 0; stopsec = 0; closesec = 0; }
                }
                //8腳位任一支電位變化就存入資料庫
                if (ch0[i, 0] != l_ch0[i, 0] || ch1[i, 1] != l_ch1[i, 1] || ch2[i, 2] != l_ch2[i, 2] || ch3[i, 3] != l_ch3[i, 3] || ch4[i, 4] != l_ch4[i, 4] ||
                   ch5[i, 5] != l_ch5[i, 5] || ch6[i, 6] != l_ch6[i, 6] || ch7[i, 7] != l_ch7[i, 7])
                {
                    clsMysql.insertadamdata("INSERT INTO iot_data(device, ip, status, countChange, catchTime, startSec, pauseSec, " +
                                                        "stopSec, closeSec, ch0, ch1, ch2, ch3, ch4, ch5, ch6, ch7)values(" +
                                                        "'" + wise_id[i] + "','" + wise_ip[i] + "','" + status + "','" + ch3[i, 3] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.000") + "'," +
                                                        startsec.ToString() + "," + pausesec.ToString() + "," + stopsec.ToString() + "," + closesec.ToString() + "," +
                                                        "'" + ch0[i, 0] + "','" + ch1[i, 1] + "','" + ch2[i, 2] + "','" + ch3[i, 3] + "','" + ch4[i, 4] + "'," +
                                                        "'" + ch5[i, 5] + "','" + ch6[i, 6] + "','" + ch7[i, 7] + "')");
                }
                l_ch0[i, 0] = ch0[i, 0]; l_ch1[i, 1] = ch1[i, 1]; l_ch2[i, 2] = ch2[i, 2]; l_ch3[i, 3] = ch3[i, 3]; l_ch4[i, 4] = ch4[i, 4];
                l_ch5[i, 5] = ch5[i, 5]; l_ch6[i, 6] = ch6[i, 6]; l_ch7[i, 7] = ch7[i, 7];
            }
        }

        //執行完成
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int bk_no = (int)e.Result;
            if (Formisclose == false) { }
            backgroundWorker[bk_no].RunWorkerAsync(bk_no);
        }


        private void Timer_Tick(object Sender, EventArgs e)
        {
            int i = Convert.ToInt32(((Timer)(Sender)).Tag);
            timers[i].Enabled = false;
            string di = "";
            if (clsNetwork.isExitst(wise_ip[i]))
                {
                    WISE4051Device[i] = clsWise4051.wise4051Get(wise_ip[i], wise_api[i]);
                    if (WISE4051Device[i].DIVal != null)
                    {
                    for(DIValconut=0; DIValconut< WISE4051Device[i].DIVal.Count; DIValconut++)
                    {
                        switch (DIValconut)
                        {
                            case 0:
                                ch0[i, DIValconut] = Convert.ToInt32(WISE4051Device[i].DIVal[DIValconut].Val);
                                break;
                            case 1:
                                ch1[i, DIValconut] = Convert.ToInt32(WISE4051Device[i].DIVal[DIValconut].Val);
                                break;
                            case 2:
                                ch2[i, DIValconut] = Convert.ToInt32(WISE4051Device[i].DIVal[DIValconut].Val);
                                break;
                            case 3:
                                ch3[i, DIValconut] = Convert.ToInt32(WISE4051Device[i].DIVal[DIValconut].Val);
                                break;
                            case 4:
                                ch4[i, DIValconut] = Convert.ToInt32(WISE4051Device[i].DIVal[DIValconut].Val);
                                break;
                            case 5:
                                ch5[i, DIValconut] = Convert.ToInt32(WISE4051Device[i].DIVal[DIValconut].Val);
                                break;
                            case 6:
                                ch6[i, DIValconut] = Convert.ToInt32(WISE4051Device[i].DIVal[DIValconut].Val);
                                break;
                            case 7:
                                ch7[i, DIValconut] = Convert.ToInt32(WISE4051Device[i].DIVal[DIValconut].Val);
                                break;
                        }
                        di += Convert.ToInt32(WISE4051Device[i].DIVal[DIValconut].Val).ToString();
                    }
                        DIs[i] = di;
                    }
                    else
                    {
                        DIs[i] = "0".PadLeft(DIs.Length,Convert.ToChar("0"));
                    }
                }
                else { DIs[i] = "未連接"; }
                timers[i].Enabled = true;
        }

        private void Form1_FormClosed(Object sender, FormClosedEventArgs e)
        {
            Formisclose = true;
            for (int i = 0; i < wise_id.Length; i++)
            { backgroundWorker[i].Dispose();}
        }

        public void settext(Control ctl, String str)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    SetTextCallback s = new SetTextCallback(settext);
                    this.Invoke(s, ctl, str);
                }
                else
                {
                    ctl.Text = str;
                }
            }
            catch { }
        }
    }
}
