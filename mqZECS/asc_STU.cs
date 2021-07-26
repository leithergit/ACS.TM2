using System;
using System.Runtime.InteropServices;


namespace ASMeSDK_CSharp
{
    public class asc_STU
    {
        public enum asc_device_type
        {
            AS_ME_TYPE_UNKOWN = -1,
            AS_ME_TYPE_MS_12 = 0x10022,
            AS_ME_TYPE_MS_24 = 0x10044,
            AS_ME_TYPE_ME_12 = 0x10122,
            AS_ME_TYPE_ME_24 = 0x10144,
            AS_ME_TYPE_ME_20 = 0x10212,
            AS_ME_TYPE_ME_30 = 0x10312,
            AS_ME_TYPE_ME_20IC = 0x10213,
            AS_ME_TYPE_ME_30IC = 0x10313,
            AS_ME_TYPE_DE_12 = 0x10222,
            AS_ME_TYPE_DE_24 = 0x10244,
            AS_ME_TYPE_DE_20 = 0x10245,
            AS_ME_TYPE_DE_20IC = 0x10246
        }

        public const int OK = 0;
        #region 错误码
        /*******************************************************************************
        //Error Code
        *******************************************************************************/
        public const int AS_ME_ERR_GLOB_CANCEL = -1;                     //操作被用户取消
        public const int AS_ME_ERR_GLOB_PARAM = -2;                      //函数的调用参数不正确
        public const int AS_ME_ERR_GLOB_NO_MEMORY = -3;                  //内存不足

        public const int AS_ME_ERR_COMM_CREATE_SOCKET = -101;            //无法创建套接字
        public const int AS_ME_ERR_COMM_SET_PARAM = -102;                //无法更改套接字参数
        public const int AS_ME_ERR_COMM_WRITE = -103;                    //发送数据出错
        public const int AS_ME_ERR_COMM_WRITE_TIMEOUT = -104;            //发送数据超时
        public const int AS_ME_ERR_COMM_READ = -105;                     //接收数据出错
        public const int AS_ME_ERR_COMM_READ_TIMEOUT = -106;             //接收数据超时
        public const int AS_ME_ERR_COMM_CONNECT = -107;                  //网络连接失败
        public const int AS_ME_ERR_COMM_CNT_TIMEOUT = -108;              //连接超时，可能是通讯线路堵塞或设备未正常工作
        public const int AS_ME_ERR_COMM_WRC = -109;                      //读写超时或网络连接断开
        public const int AS_ME_ERR_COMM_OPEN_SERIAL = -110;              //无法打开串行口，可能该串口不存在或已被其它程序打开
        public const int AS_ME_ERR_COMM_NO_SUPPORT = -111;               //不支持此类通讯
        public const int AS_ME_ERR_COMM_CREATE_EVENT = -112;             //SDK无法创建事件对象
        public const int AS_ME_ERR_COMM_FORMAT = -113;                   //从控制器读取的数据格式错误
        public const int AS_ME_ERR_COMM_SET_TIMEOUT = -114;              //设置通讯超时失败
        public const int AS_ME_ERR_COMM_CHANGE_RATE = -115;              //改变通讯速率失败
        public const int AS_ME_ERR_COMM_FLOW = -116;                     //通讯流程错误。

        public const int AS_ME_ERR_CTRL_NEED_MORE = -201;                //应答数据包不完整，后续数据没收到
        public const int AS_ME_ERR_CTRL_NO_HEAD = -202;                  //找不到数据包包头
        public const int AS_ME_ERR_CTRL_CHECKSUM = -203;                 //数据包校验和不正确
        public const int AS_ME_ERR_CTRL_CHECK_ADDR = -204;               //应答包返回的设备地址或读写模式不正确
        public const int AS_ME_ERR_CTRL_ERROR_END = -205;                //找不到数据包包尾

        public const int AS_ME_ERR_CTRL_NO_DEVICE = -301;                //指定地址检测不到控制器
        public const int AS_ME_ERR_CTRL_UNKNOWN = -302;                  //控制器的类型是未知的
        public const int AS_ME_ERR_CTRL_ILLEGAL_DATA = -303;             //应答包数据格式错误

        public const int AS_ME_ERR_MCU_FIRST = -600;                     //首卡认证错误
        public const int AS_ME_ERR_MCU_OVERFLOW = -601;                  //组数溢出
        public const int AS_ME_ERR_MCU_CARD_FULL = -602;                 //超过卡的最大容量
        public const int AS_ME_ERR_MCU_MULTI_CARDS = -604;               //多卡组设置溢出
        public const int AS_ME_ERR_MCU_MAGNET_0 = -605;                  //门磁0设置错误
        public const int AS_ME_ERR_MCU_MAGNET_1 = -606;                  //门磁1设置错误
        public const int AS_ME_ERR_MCU_MAGNET_2 = -607;                  //门磁2设置错误
        public const int AS_ME_ERR_MCU_MAGNET_3 = -608;                  //门磁3设置错误
        public const int AS_ME_ERR_MCU_BUTTON_0 = -609;                  //出门按钮0设置错误
        public const int AS_ME_ERR_MCU_BUTTON_1 = -610;                  //出门按钮1设置错误
        public const int AS_ME_ERR_MCU_BUTTON_2 = -611;                  //出门按钮2设置错误
        public const int AS_ME_ERR_MCU_BUTTON_3 = -612;                  //出门按钮3设置错误
        public const int AS_ME_ERR_MCU_ALARM_0 = -613;                   //报警0设置错误
        public const int AS_ME_ERR_MCU_ALARM_1 = -614;                   //报警1设置错误
        public const int AS_ME_ERR_MCU_ALARM_2 = -615;                   //报警2设置错误
        public const int AS_ME_ERR_MCU_ALARM_3 = -616;                   //报警3设置错误
        public const int AS_ME_ERR_MCU_MEMEORY = -617;                   //设备内存损坏
        public const int AS_ME_ERR_MCU_OUTPUT = -618;                    //输出端子损坏
        public const int AS_ME_ERR_MCU_INPUT = -619;                     //输入端子损坏
        public const int AS_ME_ERR_MCU_TIME = -620;                      //设备时间错误
        public const int AS_ME_ERR_MCU_READ_CARD = -621;                 //读卡出错
        public const int AS_ME_ERR_MCU_COMMUNICATION = -623;             //通讯出错
        public const int AS_ME_ERR_MCU_EXTEND_BOARD = -624;              //没有扩展板连接
        public const int AS_ME_ERR_MCU_PASSWORD = -625;                  //通讯密码错误

        public const int AS_ME_ERR_SEND_UNKNOWN = -626;                  //发送时出现未知错误
        #endregion

        #region 非设备错误
        public const int ZT_ERR_NO_DATA = -2000;                         //数据不存在
        public const int ZT_ERR_GETDATA_END = -2001;                     //数据获取结束
        public const int ZT_ERR_DEVICE_NO = -2002;                       //设备不在线
        public const int ZT_ERR_DATAINSERT_ERR = -2003;                  //数据操作错误,签到无法完成.
        public const int ZT_ERR_NJTYJQGDL_ERR = -2004;                   //你今天已经签过到了.

        public const int ZT_ERR_LOGIN_FAILURE = -3000;                   //登录失败,用户名或密码错误.

        public const int ZT_ERR_CONTINUE_RECEIVE = -4000;                //继续接收数据

        public const int ZT_ERR_FUNCTION_UNREALIZED = -4001;             //功能未实现
        public const int ZT_ERR_OPERATING = -4002;                       //上一个操作正在处理等待完成
        public const int ZT_ERR_RMTP_FAILURE = -4003;                    //无法连接RMTP服务器
        public const int ZT_ERR_FAILURE = -4004;                         //操作失败
        public const int ZT_ERR_DEVICETYPE = -4005;                      //设备类型不匹配
        #endregion

        public const int AS_ME_COMM_TYPE_COM = 1;
        public const int AS_ME_COMM_TYPE_IPV4 = 2;

        public static ushort AS_ME_NO_PASSWORD = 0xffff;

        public const int AS_ME_RECORD_TYPE_EVENT = 1;
        public const int AS_ME_RECORD_TYPE_ALARM = 2;
        public const int AS_ME_RECORD_TYPE_STATE = 3;
        public const int AS_ME_RECORD_TYPE_ERROR = 4;
        #region << 结构体 >>
        //查找返回项
        public struct AS_ME_DEVICE_ITEM
        {
            public int dwType;//DWORD dwType;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string szMac;//LPCTSTR szMac;
            public UInt32 dwIPAddress;//DWORD dwIPAddress;
            public UInt32 dwSubnetMask;//DWORD dwSubnetMask;
            public UInt32 dwIPGate;//DWORD dwIPGate;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string szVersion; //LPCTSTR szVersion;
        }
        //设备返回信息
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct LPAS_ME_TYPE_INFO
        {
            public UInt32 dwType;//类型

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
            public byte[] szTypeName;//版本

            public UInt32 dwVersion;//仅通讯时有效
            public UInt32 dwFirmVer;//仅通讯时有效
            public UInt32 dwCardNum;//仅通讯时有效
            public UInt32 dwRecordNum;//记录数

            public int nDoor;//总门数
            public int bAdjustDirection;
            public int nCurDoor;//当前门数，当bAdjustDirection为真时，仅通讯时有效
            public int nReader;

            public int nInput;//输入
            public int nOutput;//输出
            //public int nInputEx;//扩展板可接输入数
            public int nOutputEx;//扩展板可接输出数

        }
        //连接信息
        public struct Data
        {
            public UInt32 dwData1;
            public UInt32 dwData2;
        }
        public struct COM
        {
            public UInt32 dwCom;
            public UInt32 dwReserve;
        }
        public struct IPV4
        {
            public UInt32 dwIPAddress;
            public UInt16 wServicePort;
            public UInt16 wReserve;
        }
        [StructLayout(LayoutKind.Explicit, Size = 6)]
        public struct LPAS_ME_COMM_ADDRESS
        {
            [FieldOffset(0)]
            public int nType;
            [FieldOffset(0)]
            public Data Data;
            [FieldOffset(0)]
            public COM COM;
            [FieldOffset(4)]
            public IPV4 IPV4;
        }
        //时间
        public struct SYSTEMTIME
        {
            public ushort wYear;// WORD wYear;
            public ushort wMonth;//WORD wMonth;
            public ushort wDayOfWeek;//WORD wDayOfWeek;
            public ushort wDay;//WORD wDay;
            public ushort wHour;//WORD wHour;
            public ushort wMinute;//WORD wMinute;
            public ushort wSecond;//WORD wSecond;
            public ushort wMilliseconds;// WORD wMilliseconds;
        }
        public struct LPAS_ME_RELAY_PARAM
        {
            public int nMode;
            public int nTime;
        }
        public struct LPAS_ME_ALARM_SOURCE
        {
            public UInt32 dwLegalReaderAuth;
            public UInt32 dwIllegalReaderAuth;
            public UInt32 dwSwitch;
            public UInt32 dwDoorStateInput;
            public UInt32 dwAlarmInput;
            public UInt32 dwUrgencyCode;
        }

        //周计划
        public struct AS_ME_TIME_SEG
        {
            public Byte byStartHour;//0-23
            public Byte byStartMinute;//0-59
            public Byte byEndHour;//0-23
            public Byte byEndMinute;//0-59
        }
        public struct LPAS_ME_WEEK_SCHEDULE
        {
            //门状态周编程组：  组别：00010000（门1），F8 FF组
            //00010001（门2），F9 FF组
            //00010010（门3），FA FF组
            //00010011（门4），FB FF组
            //出门按钮失效周编程组：  00010100（出门按钮1），FC FF组
            //00010101（出门按钮2），FD FF组
            //00010110（出门按钮3），FE FF组
            //00010111（出门按钮4），FF FF组
            //备注：如果是要删除某组周编成，除了组别外，其他应全为FF

            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 21)]
            public AS_ME_TIME_SEG[,] aDaySeg;//AS_ME_TIME_SEG aDaySeg[7][3];
        }
        //假日计划
        public struct AS_ME_HOLIDAY{
            public Byte byMonth;//1-12
            public Byte byDay;//1-31
        }
        public const int Holiday_Num = 32;
        public struct LPAS_ME_HOLIDAY_SCHEDULE
        {
            //YTE3组号低位BYTE4组号高位（暂时保留为0），取值0-79,一共支持80组
            //每组64个假日，每个假日6个字节，用BCD码表示，分别表示如下
            //BYTE1: 月，BYTE2: 日，BYTE3-BYTE4: 起始时间时分， BYTE5-BYTE6：结束时间时分
            //所一个组的假日的字节为384个字节

            //门常开禁止假日计划组：  组别：
            //00010000（门1），F8 FF组
            //00010001（门2），F9 FF组
            //00010010（门3），FA FF组
            //00010011（门4），FB FF组

            //备注：如果是要删除某组假日编成，除了组别外，其他应全为FF
            //考虑的兼容性，软件先不实现分时，还是按月日设置给控制器

            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = Holiday_Num)]
            public AS_ME_HOLIDAY[] aHoliday;
        }
        public struct LPAS_ME_READER_PARAM
        {
            public int nGreenMode;
            public int nRedMode;
            public int nFirstAuthMode;
            public bool bRemoteAuth;
            public int nExtraAuth;

            public string szUrgencyCode;
        }
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct LPAS_ME_CARD_PARAM
        {
            public int nGroup;//通行权限组
            public bool bFirstCard;//BOOL是否首卡
            public bool bSetGuard;//BOOL是否有通行权限
            public bool bHasPassCount;//BOOL是否限制通行次数
            public bool bCancelOpen;//是否取消门常开权限
            public bool bHasDeadline;//BOOL是否有限制终止时间(有时间限制)
            public SYSTEMTIME stDeadline;//终止时间
            public int nPassCount;//可通行的次数
            //[MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 6)]
            public IntPtr szPassword;//
            //[MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 20)]
            public IntPtr szName;//
        }
        //事件
        public const int AS_ME_MAX_GET_RECORD_NUM = 10;
        public struct LPAS_ME_EVENT_RECORD
        {
            public int nType;
            public int nPos;
            public ulong dw64ID;
            public SYSTEMTIME stStamp;
            public int bRemote;
            public int nState;

        }
        //报警
        public struct LPAS_ME_ALARM_RECORD
        {
            public   int        nType;
            public   int        nPos;
            public   SYSTEMTIME stStamp;
            public   int nState;
        }
        //监控
        public const int AS_ME_DOOR_MAX_NUM = 8;
        public const int AS_ME_INPUT_PIN_MAX_NUM = 16;
        public const int AS_ME_RELAY_MAX_NUM = 16;
        public const int AS_ME_INPUT_PIN_EX_MAX_NUM = 8;
        public const int AS_ME_RELAY_EX_MAX_NUM = 8;

        public const int AS_ME_STATE_DOOR_CLOSE = 0;
        public const int AS_ME_STATE_DOOR_TIMEOUT = 1;
        public const int AS_ME_STATE_DOOR_BREAK_INTO = 2;
        public const int AS_ME_STATE_DOOR_OPEN = 3;

        public const int AS_ME_STATE_SWITCH_PRESS = 1;
        public const int AS_ME_STATE_SWITCH_RELEASE = 0;

        public const int AS_ME_STATE_INPUT_PIN_TRIGGER = 1;
        public const int AS_ME_STATE_INPUT_PIN_UNTRIGGER = 0;

        public const int AS_ME_STATE_RELAY_OPEN = 1;
        public const int AS_ME_STATE_RELAY_CLOSE = 0;

        public const int AS_ME_ALARM_TYPE_DOOR_MAGNET = 0;
        public const int AS_ME_ALARM_TYPE_ALARM_INPUT = 1;
        public const int AS_ME_ALARM_TYPE_BREAK_INTO = 2;

        public const int AS_ME_EVENT_TYPE_ILLEGAL_CARD = 0;   //非法卡
        public const int AS_ME_EVENT_TYPE_LEGAL_CARD = 1;  //合法卡
        public const int AS_ME_EVENT_TYPE_LEGAL_PASSWORD = 2;  //密码开门
        public const int AS_ME_EVENT_TYPE_ILLEGAL_PASSWORD = 3; //非法密码
        public const int AS_ME_EVENT_TYPE_EXIT_SWITCH = 4;  //出门按钮
        public const int AS_ME_EVENT_TYPE_URGENCY_CODE = 5;   //胁迫码
        public const int AS_ME_EVENT_TYPE_PREVENT_BACK = 6;  //卡被防潜回
        public const int AS_ME_EVENT_TYPE_DOOR = 7;   //门开关
        public const int AS_ME_EVENT_TYPE_RELAY = 8;  //继电器
        public const int AS_ME_EVENT_TYPE_DISARMING = 9;  //撤防
        public const int AS_ME_EVENT_TYPE_ARMING = 10;  //布防
        public struct LPAS_ME_OUTPUT_STATE
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = AS_ME_DOOR_MAX_NUM)]
            public int[] aDoor;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = AS_ME_DOOR_MAX_NUM)]
            public int[] aSwitch;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = AS_ME_INPUT_PIN_MAX_NUM)]
            public int[] aInputPin;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = AS_ME_INPUT_PIN_EX_MAX_NUM)]
            public int[] aInputPinEx;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = AS_ME_RELAY_MAX_NUM)]
            public int[] aRelay;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = AS_ME_RELAY_EX_MAX_NUM)]
            public int[] aRelayEx;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = AS_ME_DOOR_MAX_NUM)]
            public int[] aDoorRelay;
            public int bIsGurad;
            UInt32 dwFirmVer;
        }

        public struct LPAS_ME_REALTIME_RECORD
        {
            public bool bEmpty;
            public int nType;
            public int nCount;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = AS_ME_MAX_GET_RECORD_NUM)]
            public LPAS_ME_EVENT_RECORD[] erRecord;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = AS_ME_MAX_GET_RECORD_NUM)]
            public LPAS_ME_ALARM_RECORD[] arRecord;
            public LPAS_ME_OUTPUT_STATE opState;
        }

        public struct LPAS_ME_DEVICE_ITEM
        {
            public UInt32 dwType;
            //[MarshalAs(UnmanagedType.LPTStr)]
            public IntPtr szMac;
            public UInt32 dwIPAddress;
            public UInt32 dwSubnetMask;
            public UInt32 dwIPGate;
            //[MarshalAs(UnmanagedType.LPTStr)]
            public IntPtr szVersion;
        }
        #endregion

        #region << 委托 >>
        public delegate void DEVICE_PROC(
            ref AS_ME_DEVICE_ITEM lLoginID, IntPtr dwUser);
        #endregion

        #region 函数方法
        public static asc_STU.SYSTEMTIME DateNowToSystemTime()
        {
            asc_STU.SYSTEMTIME pSystemTime = new asc_STU.SYSTEMTIME();
            pSystemTime.wYear = (ushort)DateTime.Now.Year;
            pSystemTime.wMonth = (ushort)DateTime.Now.Month;
            pSystemTime.wDay = (ushort)DateTime.Now.Day;

            pSystemTime.wHour = (ushort)DateTime.Now.Hour;
            pSystemTime.wMinute = (ushort)DateTime.Now.Minute;
            pSystemTime.wSecond = (ushort)DateTime.Now.Second;
            pSystemTime.wDayOfWeek = (ushort)DateTime.Now.DayOfWeek;
            return pSystemTime;
        }


        #endregion
    }
}
