using System;
using System.Runtime.InteropServices;


namespace ASMeSDK_CSharp
{
    public class asc_SDKAPI
    {

        const string dllname = "ASMeSDKu.dll";
        // 打开设备
        [DllImport(dllname)]
        public static extern Int32 AS_ME_OpenController(int dwType,//DWORD dwType,
                                                        ref asc_STU.LPAS_ME_COMM_ADDRESS pAddress,
                                                        UInt16 wCtrlAddress,//WORD wCtrlAddress,
                                                        UInt16 wPassword,//WORD wPassword,
                                                        ref int pbStop,//BOOL *pbStop,
                                                        ref IntPtr hController);//HANDLE *pController

        [DllImport(dllname)]
        public static extern int AS_ME_GetType(IntPtr hController);

        //关闭设备
        [DllImport(dllname)]
        public static extern void AS_ME_CloseController(IntPtr hController);//HANDLE hController
        //获取设备基本信息
        [DllImport(dllname)]
        public static extern Int32 AS_ME_CommuncationTest(IntPtr hController, //HANDLE hController
                                                        ref asc_STU.LPAS_ME_TYPE_INFO pInfo, //返回设备信息
                                                        int nTryTimes);//测试时间
        //检测设备
        [DllImport(dllname, CharSet = CharSet.Unicode)]
        public static extern Int32 AS_ME_DetectController(
                                                        ref asc_STU.LPAS_ME_COMM_ADDRESS pAddress,
                                                        UInt16 wCtrlAddress,
                                                        ref asc_STU.LPAS_ME_TYPE_INFO pInfo,
                                                        ref int pbStop
            );
        //初始化控制器
        [DllImport(dllname)]
        public static extern Int32 AS_ME_InitController(IntPtr hController);//CTRLSDK_API int AS_ME_InitController(HANDLE hController);
        ////获取控制器类型
        //[DllImport(dllname)]
        //public static extern Int32 AS_ME_GetType(IntPtr hController);//HANDLE hController
        //获取时间
        [DllImport(dllname)]
        public static extern Int32 AS_ME_GetTime(IntPtr hController, //HANDLE hController
                                                        ref asc_STU.SYSTEMTIME pSystemTime);//SYSTEMTIME* pSystemTime
        //设置时间
        [DllImport(dllname)]
        public static extern Int32 AS_ME_SetTime(IntPtr hController, //HANDLE hController, 
                                                        ref asc_STU.SYSTEMTIME pSystemTime);
        //设置防潜回组
        [DllImport(dllname)]
        public static extern Int32 AS_ME_SetPreventBack(
                                                        IntPtr hController, //HANDLE hController,
                                                        UInt32 dwRule1, // DWORD dwRule1,
                                                        UInt32 dwRule2);//DWORD dwRule2

        //设置互锁组
        [DllImport(dllname)]
        public static extern Int32 AS_ME_SetInterlock(
                                                        IntPtr hController, //HANDLE hController,
                                                        UInt32 dwRule1, //DWORD dwRule1,
                                                        UInt32 dwRule2);//DWORD dwRule2
        //设置读头协议
        [DllImport(dllname)]
        public static extern Int32 AS_ME_SetReaderProtocol(IntPtr hController, int nType);
        //自定义读头协议
        [DllImport(dllname)]
        public static extern Int32 AS_ME_CustomWiegandFormat(
                                                        IntPtr hController,
                                                        int nBits,
                                                        int nValidBits,
                                                        int nCheck1,
                                                        int nCheck2,
                                                        int nCheck3);
        //设置报警输入
        [DllImport(dllname)]
        public static extern Int32 AS_ME_SetAlarmInput(
                                                        IntPtr hController,
                                                        int nAlarm,
                                                        bool bNormalOpen);//BOOL
        //关联输入输出,连动输出设置
        [DllImport(dllname)]
        public static extern Int32 AS_ME_AssociateInOut(
                                                        IntPtr hController,
                                                        int nRelay,//BYTE3 01 表示继电器1，以此类推08表示继电器8
                                                        ref asc_STU.LPAS_ME_RELAY_PARAM pParam,
                                                        ref asc_STU.LPAS_ME_ALARM_SOURCE pSource);
        //扩展的输入输出
        [DllImport(dllname)]
        public static extern Int32 AS_ME_AssociateInOutEx(
                                                        IntPtr hController,
                                                        int nRelay,
                                                        ref asc_STU.LPAS_ME_RELAY_PARAM pParam,
                                                        ref asc_STU.LPAS_ME_ALARM_SOURCE pSource);
        //门级方法********************************************
        //设置电锁关联继电器
        [DllImport(dllname)]
        public static extern Int32 AS_ME_SetLockRelay(IntPtr hController, int nDoor, int nRelay);
        //设置关门延时
        [DllImport(dllname)]
        public static extern Int32 AS_ME_SetLockDelay(IntPtr hController, int nDoor, int nDelay);
        //设置门状态报警检测（门磁报警）
        [DllImport(dllname)]
        public static extern Int32 AS_ME_SetDoorStateInput(
                                                        IntPtr hController,
                                                        int nDoor,
                                                        int nDelay,
                                                        bool bNormalOpen);//BOOL
        //设置出门按钮
        [DllImport(dllname)]
        public static extern Int32 AS_ME_SetDoorSwitch(IntPtr hController, int nDoor, bool bNormalOpen, bool bHasSchedule);
        //设置门是否使用常开时间计划
        [DllImport(dllname)]
        public static extern Int32 AS_ME_SetDoorOpenMode(IntPtr hController, int nDoor, bool bHasSchedule, bool bLatchMode);
        //手动开门
        [DllImport(dllname)]
        public static extern Int32 AS_ME_OpenDoor(IntPtr hController, UInt32 dwDoorMask);
        //强制门控
        [DllImport(dllname)]
        public static extern Int32 AS_ME_ToggleDoorState(
                                                        IntPtr hController,
                                                        int bOpen,//BOOL
                                                        UInt32 dwDoorMask);
        //取消强制门控
        [DllImport(dllname)]
        public static extern Int32 AS_ME_CancelToggleState(IntPtr hController, UInt32 dwDoorMask);
        //读卡器级方法********************************************
        //设置读卡器
        [DllImport(dllname)]
        public static extern Int32 AS_ME_SetReader(
                                                        IntPtr hController,
                                                        int nReader,
                                                        ref asc_STU.LPAS_ME_READER_PARAM pParam);
        //格式化存储区0表示权限组内存，1表示周编程内存，2表示假日编程，3表示卡
        [DllImport(dllname)]
        public static extern Int32 AS_ME_Clear_Memory(IntPtr hController, int nType);
        //权限管理方法********************************************
        //设置周编程
        [DllImport(dllname)]
        public static extern Int32 AS_ME_SetWeekSchedule(
                                                        IntPtr hController,
                                                        int nWeekID,//组号
                                                        ref asc_STU.LPAS_ME_WEEK_SCHEDULE pSchedule);
        //设置假日编程
        [DllImport(dllname)]
        public static extern Int32 AS_ME_SetHolidaySchedule(
                                                        IntPtr hController,
                                                        int nHoliday,
                                                        ref asc_STU.LPAS_ME_HOLIDAY_SCHEDULE pSchedule);
        //设置权限组, 一次要将所有读卡器与时间计划的关联全部设置好
        [DllImport(dllname)]
        public static extern Int32 AS_ME_SetGroup(
                                                        IntPtr hController,
                                                        int nGroup,
                                                        UInt32 dwReaderMask,//读头Mask
                                                                            //[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 10)]
                                                        ref int[] aWeekSchedule,//BYTE6-BYTE7：    读卡器1周编程
                                                                                //BYTE8-BYTE9：    读卡器2周编程
                                                                                //BYTE10-BYTE11：  读卡器3周编程
                                                                                //BYTE12-BYTE13：  读卡器4周编程
                                                                                //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
                                                        ref int[] aHolidaySchedule);
        //BYTE14-BYTE15:   读卡器1假日编程
        //BYTE16-BYTE17:   读卡器2假日编程
        //BYTE18-BYTE19:   读卡器3假日编程
        //BYTE20-BYTE21:   读卡器4假日编程
        //设置卡
        [DllImport(dllname)]
        public static extern Int32 AS_ME_SetCard(
                                                        IntPtr hController,
                                                        int nCardID,
                                                        bool bIgnoreCardNum,//BOOL用于无卡密码开门
                                                        UInt64 dw64CardNum,
                                                        bool bSearch,//BOOL
                                                        ref asc_STU.LPAS_ME_CARD_PARAM pParam);
        //其它方法********************************************
        //获取事件记录,返回0表示结束,大于0表示获得了多少条记录
        [DllImport(dllname)]
        public static extern Int32 AS_ME_GetEventRecord(IntPtr hController, IntPtr aRecord);
        [DllImport(dllname)]
        public static extern Int32 AS_ME_ClearAllEventRecord(IntPtr hController);
        [DllImport(dllname)]
        public static extern Int32 AS_ME_GetAlarmRecord(IntPtr hController, IntPtr aRecord);
        [DllImport(dllname)]
        public static extern Int32 AS_ME_ClearAllAlarmRecord(IntPtr hController);
        //开始登记卡
        [DllImport(dllname)]
        public static extern Int32 AS_ME_StartReadCard(IntPtr hController);
        //登记卡
        [DllImport(dllname)]
        public static extern Int32 AS_ME_ReadCard(IntPtr hController, ref UInt64 dw64Card);
        //停止登记卡
        [DllImport(dllname)]
        public static extern Int32 AS_ME_StopReadCard(IntPtr hController);
        //将控制器设为单向进出或双向进出。 
        [DllImport(dllname)]
        public static extern int AS_ME_SetControlDirection(IntPtr hController, bool bBidirection);
        //启用或禁用编程卡
        [DllImport(dllname)]
        public static extern int AS_ME_SetProgrammeCard(IntPtr hController, bool bEnable);
        //设置门开模式。 
        [DllImport(dllname)]
        public static extern int AS_ME_SetDoorOpenMode2(IntPtr hController, int nDoor, int nDoorMode);
        //设置门常开周计划。
        [DllImport(dllname)]
        public static extern int AS_ME_SetOpenSchedule(IntPtr hController,  int nDoor, ref asc_STU.LPAS_ME_WEEK_SCHEDULE pSchedule);
        //设置门常开假日计划。
        [DllImport(dllname)]
        public static extern int AS_ME_SetOpenHSchdulde(IntPtr hController, int nDoor, ref asc_STU.LPAS_ME_HOLIDAY_SCHEDULE pSchedule);

        //使控制器进入监控状态
        [DllImport(dllname)]
        public static extern Int32 AS_ME_StartMonitor(IntPtr hController);
        [DllImport(dllname)]
        public static extern int AS_ME_StartMonitor2(IntPtr hController,ref asc_STU.LPAS_ME_OUTPUT_STATE pOutputState);
        //请求发送监控数据
        [DllImport(dllname)]
        public static extern Int32 AS_ME_RequestSend(IntPtr hController);
        //接收监控数据
        [DllImport(dllname)]
        public static extern Int32 AS_ME_MonitorRecv(IntPtr hController, ref asc_STU.LPAS_ME_REALTIME_RECORD pRecord);
        [DllImport(dllname)]
        public static extern Int32 AS_ME_StopMonitor(IntPtr hController);
        //主动上传
        [DllImport(dllname)]
        public static extern Int32 AS_ME_StartMonitorEx(IntPtr hController, RECORD_PROC pProc, IntPtr pUserParam);
        
        //
        [DllImport(dllname, CharSet = CharSet.Unicode)]
        public static extern int AS_ME_FindLocalNetController(DEVICE_PROC pProc, IntPtr pUserParam, ref bool pbStop);
        //
        [DllImport(dllname)]
        public static extern int AS_ME_FindLocalNetConvertor(DEVICE_PROC pProc, IntPtr pUserParam, ref bool pbStop);
        //
        [DllImport(dllname, CharSet = CharSet.Unicode)]
        public static extern int AS_ME_ChangeLocalNetConvertorIP(string szMac, UInt32 dwNewIP, UInt32 dwNewMask, UInt32 dwNewGate, ref bool pbStop);
        //
        [DllImport(dllname, CharSet = CharSet.Unicode)]
        public static extern int AS_ME_ChangeLocalNetControllerIP(string szMac, UInt32 dwNewIP, UInt32 dwNewMask, UInt32 dwNewGate, ushort wPassword, ref bool pbStop);

        #region delegate
        public delegate int RECORD_PROC(ref asc_STU.LPAS_ME_REALTIME_RECORD pRecord, IntPtr pUserParam);
        public delegate int DEVICE_PROC(ref asc_STU.LPAS_ME_DEVICE_ITEM pItem, IntPtr pUserParam);
        #endregion
    }
}
