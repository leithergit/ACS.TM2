using Apache.NMS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Data.SqlClient;
using System.IO;
using UmsServer.Mq;
using System.Diagnostics;

//using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Xml;
using NTCPMSG.Client;
using NTCPMSG.Event;
using NTCPMSG.Server;
using NTCPMSG.Serialize;
using NTCPMSG.TCP;

namespace mqZECS
{
    enum msgType
    {
        Log = 101,
        InfoInsert = 103,
        InfoUpdate = 104,
        InfoDelete = 105,
        InfoSync = 106,
        AccessStatus = 107
    }
    public partial class Form1 : Form
    {
        delegate void AppendMessageCallback( string text);
        
        public void AppendMessage(string strText)
        {
            if (this.InvokeRequired)
            {
                AppendMessageCallback d = new AppendMessageCallback(AppendMessage);
                this.Invoke(d, new object[] { strText });
            }
            else
            {
                richTextBox_message.AppendText(strText);
                richTextBox_message.ScrollToCaret();
            }
        }
        private NotifyIcon notifyIcon = null;
        Thread Thread_ServerListen;
        Thread Thread_UpdateUser;
        private Socket ServerSocket = null;//服务端  
        public Dictionary<string, MySession> dic_ClientSocket = new Dictionary<string, MySession>();//tcp客户端字典
        private Dictionary<string, Thread> dic_ClientThread = new Dictionary<string, Thread>();//线程字典,每新增一个连接就添加一条线程
        private bool bThreadListenRun = false;//监听客户端连接的标志
        private bool bThreadUpdateUserRun = false;
        bool bAutoStart = false;
        int nConnectionType = 0;
        public int nSyncInterval;
        public string strMQHost;
        public string strMQPort;
        public string strSQLHost;
        public string strSQLAccount;
        public string strSQLPWD;
        public string strSQLDB;
        
        /*
         * <?xml version="1.0" encoding="UTF-8"?>
        <Configure AutoStart="1" SyncInterval="180">
	        <MQServer Host="127.0.0.1" Port="60606"/>
	        <SQLServer Host="127.0.0.1" Account="sa" PWD="sasa" DB="Amadeus5"/>
        </Configure>
         */
        private bool LoadConfigure()
        {
            string strConfigFilePath = System.Environment.CurrentDirectory ;
            strConfigFilePath += "\\Configure.xml";
            //1
            if (!File.Exists(strConfigFilePath))
            {
                strError = "Configure.xml not exist!";
                return false;
            }
            //2
            XmlDocument configxml = new XmlDocument();
            configxml.Load(strConfigFilePath);
            XmlNode ConfigNode = configxml.SelectSingleNode("//Configure");
            //3
            if (ConfigNode != null)
            {
                string strAutoStart = ConfigNode.Attributes["AutoStart"].Value;
                string strInterval = ConfigNode.Attributes["SyncInterval"].Value;
                string strConnectionType = ConfigNode.Attributes["ConnectionType"].Value;
                nSyncInterval = Int32.Parse(strInterval);
                strAutoStart.ToUpper();
                bAutoStart = (strAutoStart == "TRUE");
                nConnectionType = Int32.Parse(strConnectionType);
                comboBox_ConnectionType.SelectedIndex = nConnectionType;
            }
            else
            {
                strError = "Can't find node \"Configure\"!";
                return false;
            }
            //4
            XmlNode MQNode = configxml.SelectSingleNode("//Configure/MQServer");//设置节点位置
            if (MQNode != null)
            {
                strMQHost = MQNode.Attributes["Host"].Value;
                strMQPort = MQNode.Attributes["Port"].Value;
                textMQPort.Text = strMQPort;
                textMQServer.Text = strMQHost;
            }
            else
            {
                strError = "Can't find node \"MQServer\"!";
                return false;
            }
            //5
            XmlNode SQLNode = configxml.SelectSingleNode("//Configure/SQLServer");
            if (SQLNode != null)
            {
                strSQLHost = SQLNode.Attributes["Host"].Value;
                strSQLAccount = SQLNode.Attributes["Account"].Value;
                strSQLPWD = SQLNode.Attributes["PWD"].Value;
                strSQLDB = SQLNode.Attributes["DB"].Value;

                textBox_SQLServer.Text = strSQLHost;
                textBox_Account.Text = strSQLAccount;
                textBox_Password.Text = strSQLPWD;
                textDB.Text = strSQLDB;
            }
            else
            {
                strError = "Can't find node \"SQLServer\"!";
                return false;
            }
                
            return true;
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="port">端口号</param>
        public bool OpenServer(int port)
        {
            try
            {
                bThreadListenRun = true;
                // 创建负责监听的套接字，注意其中的参数；
                ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // 创建包含ip和端口号的网络节点对象；
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
                try
                {
                    // 将负责监听的套接字绑定到唯一的ip和端口上；
                    ServerSocket.Bind(endPoint);
                }
                catch
                {
                    return false;
                }
                // 设置监听队列的长度；
                ServerSocket.Listen(100);
                // 创建负责监听的线程；
                Thread_ServerListen = new Thread(ListenConnecting);
                Thread_ServerListen.IsBackground = true;
                Thread_ServerListen.Start();

                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 关闭服务
        /// </summary>
        public void CloseServer()
        {
            lock (dic_ClientSocket)
            {
                foreach (var item in dic_ClientSocket)
                {
                    item.Value.Close();//关闭每一个连接
                }
                dic_ClientSocket.Clear();//清除字典
            }
            lock (dic_ClientThread)
            {
                foreach (var item in dic_ClientThread)
                {
                    item.Value.Abort();//停止线程
                }
                dic_ClientThread.Clear();
            }
            bThreadListenRun = false;
            //ServerSocket.Shutdown(SocketShutdown.Both);//服务端不能主动关闭连接,需要把监听到的连接逐个关闭
            if (ServerSocket != null)
                ServerSocket.Close();

        }

        public bool SendTcpMsg(uint nType, string strMsgSend, ref string strMsgRecv)
        {
            IPAddress ServerIP = IPAddress.Parse(strMQHost);
            IPEndPoint endPoint = new IPEndPoint(ServerIP, Int32.Parse("5086"));
            TCPClient client = new TCPClient();
            client.PortNO = Int32.Parse(strMQPort);

            try
            {
                client.Timeout = 500;
                client.Connect(strMQHost);
                if (!client.IsConnect)
                {
                    AppendMessage("\n连接服务器失败!\n");
                    return false;
                }
               
               
                //byte [] buffer = Encoding.UTF8.GetBytes(strMsgSend);
                object strSendObj = strMsgSend;
                //string strEnc = Encoding.UTF8.GetString(buffer);

                object objRecv = new byte[4096];
                if (!client.SyncSend(nType, strSendObj, ref objRecv))
                {
                    AppendMessage("\n接收数据失败!\n");
                    return false;
                }
                strMsgRecv = objRecv.ToString();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                AppendMessage("\r\n" + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 监听客户端请求的方法；
        /// </summary>
        private void ListenConnecting()
        {
            while (bThreadListenRun)  // 持续不断的监听客户端的连接请求；
            {
                try
                {
                    Socket sokConnection = ServerSocket.Accept(); // 一旦监听到一个客户端的请求，就返回一个与该客户端通信的 套接字；
                    // 将与客户端连接的 套接字 对象添加到集合中；
                    string str_EndPoint = sokConnection.RemoteEndPoint.ToString();
                    MySession myTcpClient = new MySession() 
                    { 
                        TcpSocket = sokConnection 
                    };
                    //创建线程接收数据
                    Thread th_ReceiveData = new Thread(ReceiveData);
                    th_ReceiveData.IsBackground = true;
                    th_ReceiveData.Start(myTcpClient);
                    //把线程及客户连接加入字典
                    dic_ClientThread.Add(str_EndPoint, th_ReceiveData);
                    dic_ClientSocket.Add(str_EndPoint, myTcpClient);
                }
                catch
                {

                }
            }
        }

        public delegate string TextBoxDelegate(System.Windows.Forms.TextBox tb);
        public string GetTextbox(System.Windows.Forms.TextBox tb)
        {
            if (tb.InvokeRequired)
            {
                TextBoxDelegate TBD = new TextBoxDelegate(GetTextbox);
                IAsyncResult ia = tb.BeginInvoke(TBD, new object[] { tb });
                return (string)TBD.EndInvoke(ia);
            }
            else
            {
                return tb.Text;
            }
        }
        void UpdateUser()
        {
            try
            {
                //string strSQLConnection = "Data Source=192.168.2.210;Pooling=False;Max Pool Size = 1024;Initial Catalog=Amadeus51;Persist Security Info=True;User ID=sa;Password=1";
                string strSQLConnection = string.Format( "Data Source={0};Pooling=False;Max Pool Size = 1024;Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}",strSQLHost,strSQLDB,strSQLAccount,strSQLPWD);
                SqlConnection  SQLConnection = new SqlConnection(strSQLConnection);
                if (SQLConnection.State.ToString().ToLower() != "open")
                {
                    SQLConnection.Open();
                }
                // for DDS Amedus5
                //string strSQL = "SELECT " +
                //                "dbo.CRDHLD.ID," +
                //                "dbo.CRDHLD.Last_Name," +
                //                "dbo.CRDHLD.First_Name," +
                //                "dbo.CRDHLD.From_Date," +
                //                "dbo.Card.Code," +
                //                "dbo.CRDHLD.Num AS EmployeeID," +
                //                "dbo.CRDHLD.Department," +
                //                "dbo.ACS_CardHolder.PWD " +
                //                "FROM " +
                //                "dbo.CRDHLD " +
                //                "INNER JOIN dbo.Card ON dbo.CRDHLD.ID = dbo.Card.Owner " +
                //                "INNER JOIN dbo.ACS_CardHolder ON dbo.ACS_CardHolder.CardHolderID = dbo.CRDHLD.ID ";
                // for Biostar fingerprinter
                string strSQL = "SELECT " +
                " dbo.TB_USER_DEPT.sName as Department, " +
                " dbo.TB_USER.sUserName as First_Name, " +
                " dbo.TB_USER.sUserID as EmployeeID, " +
                " dbo.TB_USER.sPassword as PWD, " +
                " dbo.TB_USER.nDepartmentIdn as RoleID" +
                " FROM  dbo.TB_USER  " +
                " INNER JOIN dbo.TB_USER_DEPT ON dbo.TB_USER.nDepartmentIdn = dbo.TB_USER_DEPT.nDepartmentIdn ";

                SqlCommand cmd = new SqlCommand(strSQL, SQLConnection);
                SqlDataReader dr = cmd.ExecuteReader();

                /*
                 *  string formatMessage = string.Empty;
                    string strSendMsg;
                    string strShowText;
                    SendInfo info = new mqZECS.SendInfo();

                    info.Sender = "Access";
                    info.MsgType = "User";
                    string strFirstName = dr["First_Name"].ToString();
                    string strLastName = dr["Last_Name"].ToString() ;
                    if (strFirstName.Length > 0 && strLastName.Length > 0)
                        info.Name = strFirstName + " " + strLastName;
                    else if (strFirstName.Length > 0)
                        info.Name = strFirstName;
                    else if (strLastName.Length > 0)
                        info.Name = strLastName;

                    info.Pwd     = dr["PWD"].ToString();
                    info.UserID  = dr["EmployeeID"].ToString();        // 工号
                    info.Role    = dr["Department"].ToString();        // 部门 
                    info.Operation = "Sync";                           // 同步数据库

                    using (var ms = new MemoryStream())
                    {
                        new DataContractJsonSerializer(info.GetType()).WriteObject(ms, info);
                        formatMessage = Encoding.UTF8.GetString(ms.ToArray());
                    }
                    //桥吊号Device为到时我们预先定义好的对照int类型的编号；
                    //打卡人员直接发送你们定义的人员姓名
                    //打卡位置可以暂且先默认成保留字符统一发送 1

                    strShowText = "User\tOperation：" + info.Operation + ",EmployeeID: " + info.UserID + ",Name:" + info.Name + ",Department: " + info.Role;

                    strSendMsg = formatMessage;
                    AppendMessage(strShowText);

                    string strResult = messageSender.SendMsg(strSendMsg) + "\n";
                    AppendMessage("\nResult:" + strResult + "\n");
                 */

                while (dr.Read())
                {
                    string formatMessage = string.Empty;
                    string strSendMsg;
                    string strShowText;
                    SendInfo info = new mqZECS.SendInfo();

                    info.Sender = "Access";
                    info.MsgType = "User";
                    string strFirstName = dr["First_Name"].ToString();
                    //string strLastName = dr["Last_Name"].ToString() ;
//                     if (strFirstName.Length > 0 && strLastName.Length > 0)
//                         info.Name = strFirstName + " " + strLastName;
//                     else if (strFirstName.Length > 0)
                    info.Name = strFirstName;
//                    else if (strLastName.Length > 0)
//                        info.Name = strLastName;

                    info.Pwd     = dr["PWD"].ToString();
                    info.UserID  = dr["EmployeeID"].ToString();        // 工号
                    info.Role    = dr["RoleID"].ToString();        // 部门 
                    info.Operation = "Sync";                           // 同步数据库

                    using (var ms = new MemoryStream())
                    {
                        new DataContractJsonSerializer(info.GetType()).WriteObject(ms, info);
                        formatMessage = Encoding.UTF8.GetString(ms.ToArray());
                    }
                    //桥吊号Device为到时我们预先定义好的对照int类型的编号；
                    //打卡人员直接发送你们定义的人员姓名
                    //打卡位置可以暂且先默认成保留字符统一发送 1

                    strShowText = "User\tOperation：" + info.Operation  +",PWD:" + info.Pwd + ",EmployeeID: " + info.UserID + ",Name:" + info.Name + ",Department: " + info.Role;

                    strSendMsg = formatMessage;
                    AppendMessage(strShowText);
                    switch(nConnectionType)
                    {
                        default:
                        case 0:
                            {
                                string strResult = messageSender.SendMsg(strSendMsg) + "\n";
                                AppendMessage("\nResult:" + strResult + "\n");
                            }
                            break;
                        case 1:
                            {
                                string strResult = "";
                                if (SendTcpMsg((uint)msgType.InfoSync, strSendMsg,ref strResult))
                                {
                                    AppendMessage("\nResult:" + strResult + "\n");
                                }
                                else
                                {
                                    AppendMessage("\nResult:Recv data Error!\n");
                                }
                            }
                            break;
                        case 2:
                            break;
                    }
                  
                    //Thread.Sleep(10);
                }
                dr.Close();
                SQLConnection.Close();
            }
            catch (Exception except)
            {
                AppendMessage(except.Message + '\n');
            }

        }
        private void ThreadUpdateUser()
        {
            Stopwatch Clock = new Stopwatch();
            Clock.Start();
            long nInterval = nSyncInterval*60 * 1000;
            long tFirst = Clock.ElapsedMilliseconds -  nInterval;
            while (bThreadUpdateUserRun)
            {
                if ((Clock.ElapsedMilliseconds - tFirst) >= nInterval)
                {
                    tFirst = Clock.ElapsedMilliseconds;
                    // 执行扫描数据库的操作
                    UpdateUser();
                }
                else
                    Thread.Sleep(50);
            }
            Clock.Stop();
        }
        
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="sokConnectionparn"></param>
        private void ReceiveData(object sokConnectionparn)
        {
            string sendmsg = "";
            MySession tcpClient = sokConnectionparn as MySession;
            Socket socketClient = tcpClient.TcpSocket;
            bool Flag_Receive = true;
            // 定义一个1K的缓存区；
            byte[] RecvBuffer = new byte[1024];
            int nRecvLength = 0;
            while (Flag_Receive)
            {
                try
                {
                    int nLength = -1;
                    byte[] TempBuff = new byte[1024];
                    nLength = socketClient.Receive(TempBuff);
                    if (nLength > 0)
                    {
                        TempBuff.CopyTo(RecvBuffer, nRecvLength);
                        nRecvLength += nLength;
                    }
                    else if (nRecvLength > 0)
                    {
                        string strRecv = System.Text.Encoding.Default.GetString(RecvBuffer, 0, nRecvLength);
                        //AppendMessage(showmsg);
                        // DeviceID,EmployeeID,UserName,Time
                        // Sample:6,20190410,LeeTest,2016-11-23T15:55:01

                        string[] strInfo = strRecv.Split(new char[] {',' });
                        string formatMessage = string.Empty;

                        /*
                         * 0为刷卡事件，1为用户事件(Insert,Update,Delete)，2为控制器事件(Online,Offline)
                         */
                        SendInfo info = new mqZECS.SendInfo();
                        msgType nType = msgType.Log;

                        info.Sender = "Access";
                        switch(strInfo[0])
                        {
                            case "0":       // 刷卡信息
                            {
                                /*
                                 *  格式：0,RosName,EmployeeID,UserName,SwipeTime
	                                Sample：0,ROS1,20190411,Test User,2019-04-11 11:20:11
                                 */
                                nType = msgType.Log;
                                info.MsgType     = "Log";
                                info.Device      = strInfo[1];
                                info.UserID      = strInfo[2];
                                info.Name        = strInfo[3];
                                //info.LogTime     = strInfo[4];
                                showmsg = "Log\tROS:" + info.Device + 
                                          ",EmployeeID:" + info.UserID +
                                          ",Name:" + info.Name +
                                          ", SwipeTime: " + strInfo[4] + "\n";
                            }
                            break;
                        case "1":       // 用户信息
                            {
                                /*
                                 * 格式:1,Insert(Update,Delete),EmployeeID,UserName,Pin,Depaert
		                           Sample:1,Insert,123456,Test Lee,1234,1
                                 */
                                nType = msgType.InfoInsert;
                                switch(strInfo[1])
                                {
                                    case "Insert":
                                        nType = msgType.InfoInsert;
                                        break;
                                    case "Update":
                                        nType = msgType.InfoUpdate;
                                        break;
                                    case "Delete":
                                        nType = msgType.InfoDelete;
                                        break;
                                }
                       
                                info.MsgType     = "User";
                                info.Operation   = strInfo[1];
                                info.UserID      = strInfo[2];
                                info.Name        = strInfo[3];
                                info.Pwd         = strInfo[4];
                                info.Role        = strInfo[5];
                                showmsg = "User\tOperation：" + info.Operation + ",PWD:" + info.Pwd + ",EmployeeID: " + info.UserID + ",Name:" + info.Name + ",Department: " + info.Role + "\n";
                            }
                            break;
                        case "2":
                            {
                                /*
                                 * 格式:2,RosName,Status
			                       Sample:2,ROS2,0		释义：2,号台，离线
                                 */
                                nType = msgType.AccessStatus;
                                info.MsgType     = "Status";
                                info.Device      = strInfo[1];
                                if (strInfo[2] == "0")
                                {
                                    info.StatusCode = "312" ;       // 离线
                                    showmsg = "Status\tRos:" + info.Device + ",Status:Offline\n";
                                }
                                    
                                if (strInfo[2] == "1")
                                {
                                    info.StatusCode = "313";        // 恢复在线
                                    showmsg = "Status\tRos:" + info.Device + ",Status:Online\n";
                                }                               
                            }
                            break;
                        }
                        using (var ms = new MemoryStream())
                        {
                            new DataContractJsonSerializer(info.GetType()).WriteObject(ms, info);
                            formatMessage = Encoding.UTF8.GetString(ms.ToArray());
                        }
                        sendmsg = formatMessage;
                        AppendMessage(showmsg);

                        switch (nConnectionType)
                        {
                            default:
                            case 0:
                                {
                                    if (MqSender.m_zQueueSend != null && MqSender.m_zQueueSend.IsStart)
                                    {
                                        if (sendmsg == "")
                                        {
                                            AppendMessage("\n未新增发送消息\n");
                                            break;
                                        }
                                        string strResult = messageSender.SendMsg(sendmsg);
                                        AppendMessage("\nResult:" + strResult + "\n");
                                        sendmsg = "";
                                        showmsg = "";
                                        break;
                                    }
                                    else
                                    {
                                        AppendMessage("\nMq未连接!\n");
                                        break;
                                    }
                                }
                                break;
                            case 1:
                                {
                                    string strResult = "";
                                    if (SendTcpMsg((uint)nType,sendmsg, ref strResult))
                                    {
                                        AppendMessage("\nResult:" + strResult + "\n");
                                    }
                                    else
                                    {
                                        AppendMessage("\nResult:Recv data Error!\n");
                                    }
                                }
                                break;
                            case 2:
                                break;
                        }
                        return;
                    }
                }
                catch
                {
                    Flag_Receive = false;
                    // 从通信线程集合中删除被中断连接的通信线程对象；
                    String keystr = socketClient.RemoteEndPoint.ToString();
                    dic_ClientSocket.Remove(keystr);//删除客户端字典中该socket
                    dic_ClientThread[keystr].Abort();//关闭线程
                    dic_ClientThread.Remove(keystr);//删除字典中该线程

                    tcpClient = null;
                    socketClient = null;
                    break;
                }
               
                Thread.Sleep(10);
            }
        }
        /// <summary>
        /// 发送数据给指定的客户端
        /// </summary>
        /// <param name="_endPoint">客户端套接字</param>
        /// <param name="_buf">发送的数组</param>
        /// <returns></returns>
        public bool SendData(string _endPoint, byte[] _buf)
        {
            MySession myT = new MySession();
            if (dic_ClientSocket.TryGetValue(_endPoint, out myT))
            {
                myT.Send(_buf);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void UpdateText2(string strText)
        {
            richTextBox_message.AppendText(strText);
        }

        private string strError = "";
        public Form1()
        {
            InitializeComponent();
            if (!LoadConfigure())
            {
                this.richTextBox_message.AppendText(strError + "\n");
            }

            textMQServer.Text = strMQHost;
            //textMQPort.Text = strMQPort;
            textBox_SQLServer.Text = strSQLHost;
            textBox_Account.Text = strSQLAccount;
            textBox_DBSyncInterval.Text = nSyncInterval.ToString();
            textDB.Text = strSQLDB;
           // AppendText = new UpdateText(UpdateText2);

             try
            {
                
                if (nConnectionType == 0)
                {
                    MqSender.MqPort = Convert.ToInt32(textMQPort.Text);
                    MqSender.MqstrIP = textMQServer.Text;
                    if (messageSender.ConnectSendMq())
                    {
                        buttonConnect.Enabled = false;
                    }
                }

                // 启动数据库用户遍历线程
                StartService();

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }           
        }

       
        private string showmsg = "";
        private string SendMsg = "";
        private void button_message_Click(object sender, EventArgs e)
        {
            string formatMessage = string.Empty;

            SendInfo info = new mqZECS.SendInfo();
            info.Sender = "Access";
            info.MsgType = "Patrol";

            info.Device ="2";
            info.Name = "4";
            info.UserID = "6";

            using (var ms = new MemoryStream())
            {
                new DataContractJsonSerializer(info.GetType()).WriteObject(ms, info);
                formatMessage = Encoding.UTF8.GetString(ms.ToArray());
            }
            //桥吊号Device为到时我们预先定义好的对照int类型的编号；
            //打卡人员直接发送你们定义的人员姓名
            //打卡位置可以暂且先默认成保留字符统一发送 1
            showmsg = "Patrol: " + "桥吊号：" + info.Device + ",打卡人:" + info.UserID + ",打卡位置:" + info.Name;

            SendMsg = formatMessage;
            this.richTextBox_message.AppendText(showmsg + "\n");
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            if (MqSender.m_zQueueSend != null && MqSender.m_zQueueSend.IsStart)
            {
                if (SendMsg == "")
                {
                    MessageBox.Show("未新增发送消息！");
                    return;
                }
                string strText = messageSender.SendMsg(SendMsg) + "\n";
                this.richTextBox_message.AppendText(strText);
                SendMsg = "";
                showmsg = "";
            }
            else
            {
                MessageBox.Show("Mq未连接！");
            }
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            bThreadListenRun = false;
            bThreadUpdateUserRun = false;
            ServerSocket.Close();
            Thread_ServerListen.Join();
            Thread_UpdateUser.Join();
            this.timer1.Stop();
            messageSender.Stop_SendMq();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //this.richTextBox_receiver.Text = messageSender.retMessage;
        }
        MqSender messageSender = new MqSender();
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (textMQServer.Text != "" && textMQPort.Text != "")
                {
                    MqSender.MqPort = Convert.ToInt32(textMQPort.Text);
                    MqSender.MqstrIP = textMQServer.Text;
                    if (messageSender.ConnectSendMq())
                    {
                        buttonConnect.Enabled = false;
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            messageSender.Stop_SendMq();
            buttonConnect.Enabled = true;
        }

        private void button_SendTCPMsg_Click(object sender, EventArgs e)
        {
            IPAddress ServerIP = IPAddress.Parse(strMQHost);
            IPEndPoint endPoint = new IPEndPoint(ServerIP, Int32.Parse("5086"));
            TCPClient client = new TCPClient();
            client.PortNO = 5086;
           
            try
            {
                client.Timeout = 500;
                client.Connect(strMQHost);
                if (!client.IsConnect)
                {
                    AppendMessage("\n连接服务器失败!\n");
                    return;
                }
                string strSendMessage = "Test string!!!";
                object strSendObj = strSendMessage;
                
                object objRecv = new byte[4096];
                if (!client.SyncSend(0, strSendObj, ref objRecv))
                {
                    AppendMessage("\n接收数据失败!\n");
                    return;
                }
                string strRecv = objRecv.ToString();

            }
            catch (Exception ex)
            {
               
                Console.WriteLine(ex.Message);
                AppendMessage("\r\n" + ex.Message);
            }

        }
        bool bServiceStart = false;
        void StartService()
        {
            if (bServiceStart) 
            {
                AppendMessage("\nThe Service has start!\n");
                return;
            }
            bThreadUpdateUserRun = true;
            Thread_UpdateUser = new Thread(ThreadUpdateUser);
            Thread_UpdateUser.IsBackground = true;
            Thread_UpdateUser.Start();
            OpenServer(5085);
            bServiceStart = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartService();
        }
    }

    /// <summary>
    /// 会话端
    /// </summary>
    public class MySession
    {
        public Socket TcpSocket;//socket对象
        public List<byte> m_Buffer = new List<byte>();//数据缓存区

        public MySession()
        {

        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="buf"></param>
        public void Send(byte[] buf)
        {
            if (buf != null)
            {
                TcpSocket.Send(buf);
            }
        }
        /// <summary>
        /// 获取连接的ip
        /// </summary>
        /// <returns></returns>
        public string GetIp()
        {
            IPEndPoint clientipe = (IPEndPoint)TcpSocket.RemoteEndPoint;
            string _ip = clientipe.Address.ToString();
            return _ip;
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            TcpSocket.Shutdown(SocketShutdown.Both);
        }
        /// <summary>
        /// 提取正确数据包
        /// </summary>
        public byte[] GetBuffer(int startIndex, int size)
        {
            byte[] buf = new byte[size];
            m_Buffer.CopyTo(startIndex, buf, 0, size);
            m_Buffer.RemoveRange(0, startIndex + size);
            return buf;
        }

        /// <summary>
        /// 添加队列数据
        /// </summary>
        /// <param name="buffer"></param>
        public void AddQueue(byte[] buffer)
        {
            m_Buffer.AddRange(buffer);
        }
        /// <summary>
        /// 清除缓存
        /// </summary>
        public void ClearQueue()
        {
            m_Buffer.Clear();
        }
    }
    public class SendInfo
    {
        private string sender;
        public string Sender
        {
            get;
            set;
        }

        private string msgType;
        public string MsgType
        {
            get;
            set;
        }

        //private string logime;
        //public string LogTime
        //{
        //    get;
        //    set;
        //}

        private string userID;
        public string UserID
        {
            get;
            set;
        }

        private string name;
        public string Name
        {
            get;
            set;
        }

        private string role;
        public string Role
        {
            get;
            set;
        }

        private string pwd;
        public string Pwd
        {
            get;
            set;
        }

        private string operation;
        public string Operation
        {
            get;
            set;
        }

        private string device;
        public string Device
        {
            get;
            set;
        }

        private string statusCode;
        public string StatusCode
        {
            get;
            set;
        }
    }

}
