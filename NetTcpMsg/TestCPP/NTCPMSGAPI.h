#pragma once 
#include "OAIDL.H"

#import "NTCPMSG.tlb"

/// <summary>
/// EventType
/// </summary>
enum EventType
{
	ET_UnKnown,
	ET_Subscribe,
	ET_Unsubscribe,
	ET_RaiseEvent
};

typedef void (*OnMsgReceiveFunc_TS)(DWORD dwUserData,DWORD  uEvent,
									VARIANT& varData,VARIANT& varDataToClient);
typedef void (*OnMsgReceiveFunc_TC)(DWORD dwUserData,DWORD  uEvent,VARIANT& varData);
typedef void (*OnDisConnectFunc)(DWORD dwUserData);


typedef void (*OnMsgReceiveFunc_ES)(DWORD dwUserData,EventType nEventType,
									int  nEventID,CString strEventName,VARIANT&  objEventData);
typedef void (*OnMsgReceiveFunc_EC)(DWORD dwUserData,int  nEventID,VARIANT& objEventData);



using namespace NTCPMSG;
 
static int g_nCMMRefCount = 0;

class CNTCPMSGAPI
{
 
private:
	INTCPMSGInterfacePtr p_cmm ;

public:
	CNTCPMSGAPI();
	~CNTCPMSGAPI();
	/// <summary>
	/// CString转成VARIANT
	/// </summary> 
	BOOL CV_Str_To_Variant(CString str,VARIANT& var);
	/// <summary>
	/// Byte数组转成VARIANT
	/// byteArray:Byte数组
	/// size:Byte数组大小,最大为4096字节
	/// </summary> 
	BOOL CV_ByteArray_To_Variant(byte* byteArray,ULONG size,VARIANT& var);

//***********************  TCP Server  ******************************************************************
	/// <summary>
	/// TCP Server Start
	/// PortNO:端口号,默认9600
	/// dwUserData:用户自定义数据,用于回调函数里,默认NULL
	/// onMsgReceive:接收客户端的数据回调函数
	/// </summary>
	BOOL TS_Start(int PortNO = 9600,DWORD dwUserData=NULL,OnMsgReceiveFunc_TS onMsgReceive=NULL);
	/// <summary>
	/// TCP Server Stop
	/// </summary>
	BOOL TS_Stop();
	/// <summary>
	/// TCP Server获取开启状态
	/// </summary>
	/// <returns></returns>
	BOOL TS_IsRun(); 

//*********************** TCP Client *****************************************************************
	/// <summary>
	/// TCP Client Start
	/// strIP:TCP Server的IP
	/// PortNO:端口号,默认9600
	/// dwUserData:用户自定义数据,用于回调函数里,默认NULL
	/// onMsgReceive:接收Server端的数据回调函数
	/// onDisConnect:客户端退出的回调函数
	/// </summary>
	BOOL TC_Connect(CString strIP, int PortNO = 9600,DWORD dwUserData=NULL,
					OnMsgReceiveFunc_TC onMsgReceive=NULL,OnDisConnectFunc onDisConnect=NULL );
	/// <summary>
	/// TCP Client Stop
	/// </summary>
	BOOL TC_DisConnect();
	/// <summary>
	/// TCP Client 获取开启状态
	/// </summary> 
	BOOL TC_IsConnected(); 
	/// <summary>
	/// 异步发送消息
	/// uMsgID:消息标识,自定义
	/// varMsg:消息内容,为常规数据,不可嵌套2层.
	/// </summary> 
	BOOL TC_AsyncSend(DWORD uMsgID,VARIANT& varMsg);
	/// <summary>
	/// 异步发送消息
	/// uMsgID:消息标识,自定义
	/// strMsg:string消息
	/// </summary> 
	BOOL TC_AsyncSend(DWORD uMsgID,CString strMsg);
	/// <summary>
	/// 异步发送消息
	/// uMsgID:消息标识,自定义
	/// byteArray:Byte数组
	/// size:Byte数组大小
	/// </summary> 
	BOOL TC_AsyncSend(DWORD uMsgID,byte* byteArray,ULONG size);

	/// <summary>
	/// 同步发送消息
	/// uMsgID:消息标识,自定义
	/// varMsg:消息内容,为常规数据,不可嵌套2层.
	/// </summary> 
	BOOL TC_SyncSend(DWORD uMsgID,VARIANT& varMsg,VARIANT& varDataReturn);
	/// <summary>
	/// 同步发送消息
	/// uMsgID:消息标识,自定义
	/// strMsg:string消息
	/// </summary> 
	BOOL TC_SyncSend(DWORD uMsgID,CString strMsg,VARIANT& varDataReturn);
	/// <summary>
	/// 同步发送消息
	/// uMsgID:消息标识,自定义
	/// byteArray:Byte数组
	/// size:Byte数组大小
	/// </summary> 
	BOOL TC_SyncSend(DWORD uMsgID,byte* byteArray,ULONG size,VARIANT& varDataReturn);


//************************************** Event Server ****************************************
	/// <summary>
	/// Event Server Start
	/// PortNO:端口号,默认9800
	/// dwUserData:用户自定义数据,用于回调函数里,默认NULL
	/// onMsgReceive:接收客户端的数据回调函数
	/// </summary>
    BOOL ES_Start(int PortNO=9800, DWORD dwUserData=NULL, OnMsgReceiveFunc_ES onMsgReceive = NULL);
	/// <summary>
	/// Event Server Stop
	/// </summary>
    BOOL ES_Stop();
	/// <summary>
	/// IsRun
	/// </summary>
    BOOL ES_IsRun();
	/// <summary>
	/// 触发事件,通知此事件所有订阅者
	/// nEventID:唯一的事件ID
	/// varData:事件数据
	/// </summary>
    BOOL ES_RaiseEvent(int nEventID, VARIANT& varData);

//************************************** Event Client ****************************************
	/// <summary>
	/// Event Client Connect 
	/// strIP:Event Server的IP
	/// PortNO:端口号,默认9800
	/// dwUserData:用户自定义数据,用于回调函数里,默认NULL
	/// onMsgReceive:接收Server端的数据回调函数
	/// onDisConnect:客户端退出的回调函数
	/// </summary>
    BOOL EC_Connect(CString strIP, int PortNO=9800,DWORD dwUserData=NULL, 
			OnMsgReceiveFunc_EC onMsgReceive = NULL, OnDisConnectFunc onDisConnect=NULL);
	/// <summary>
	/// Event Client DisConnect
	/// </summary>
    BOOL EC_DisConnect();
	/// <summary>
	/// EC_IsConnected
	/// </summary>
    BOOL EC_IsConnected();
	/// <summary>
	/// 订阅事件
	/// eventName:事件名
	/// varParameter:事件参数,不可嵌套2层.
	/// 返回订阅的事件ID,小于等于0为错误,大于0的为成功.
	/// </summary>
    int  EC_Subscribe(CString eventName, VARIANT& varParameter);
	/// <summary>
	/// 退订事件
	/// nEventID:唯一的事件ID
	/// </summary>
    BOOL EC_UnSubscribe(int nEventID);
	/// <summary>
	/// 触发事件,通知此事件其他所有订阅者
	/// nEventID:唯一的事件ID
	/// varData:事件数据
	/// </summary>
    BOOL EC_RaiseEvent(int nEventID, VARIANT& varData); 
};