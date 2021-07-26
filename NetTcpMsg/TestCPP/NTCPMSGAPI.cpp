#include "StdAfx.h"
#include "NTCPMSGAPI.h"
#include <afxctl.h>

#pragma warning( once : 4311 )

using namespace NTCPMSG;

CNTCPMSGAPI::CNTCPMSGAPI(void)
{
	if(g_nCMMRefCount == 0)
	{
		CoInitialize(NULL); 
		//CoInitializeEx(NULL,COINIT_MULTITHREADED);
	}
	g_nCMMRefCount ++;

	p_cmm.CreateInstance(__uuidof(NTCPMSGService));

	 
}

CNTCPMSGAPI::~CNTCPMSGAPI(void)
{
	if(p_cmm)
	{
		p_cmm.Release();
		p_cmm = NULL;
		g_nCMMRefCount --;
	}

	if(g_nCMMRefCount == 0)
	{
		CoUninitialize();
	} 
} 
/// <summary>
/// CString转成VARIANT
/// </summary> 
BOOL CNTCPMSGAPI::CV_Str_To_Variant(CString str,VARIANT& var)
{
	VariantClear(&var);
	VariantInit(&var);
	var.vt = VT_BSTR;
	var.bstrVal = _bstr_t(str).copy(); 
	return TRUE;
}
/// <summary>
/// Byte数组转成VARIANT
/// byteArray:Byte数组
/// size:Byte数组大小,最大为4096字节
/// </summary> 
BOOL CNTCPMSGAPI::CV_ByteArray_To_Variant(byte* byteArray,ULONG size,VARIANT& var)
{
	if(byteArray == NULL || size > 4096)
		return FALSE;

	SAFEARRAY* psaVariant = NULL;
	SAFEARRAYBOUND bound;
	bound.cElements = size;
	bound.lLbound = 0;
	VARTYPE vtVariant = VT_UI1;
	psaVariant = SafeArrayCreate(vtVariant, 1, &bound);

	// Lock the array get a pointer to its data.
	long * pData;
	SafeArrayAccessData(psaVariant, (LPVOID *)&pData);
	// Set or get any values in the array.
	memcpy(pData,byteArray,size);
	// Unlock the array.(pData is no longer valid.)
	SafeArrayUnaccessData(psaVariant);
 
	VariantClear(&var);
	VariantInit(&var);
	var.vt = VT_UI1 | VT_ARRAY;
	var.parray = psaVariant; 
 
	return TRUE;
}

//***********************  TCP Server  ************************************************************
/// <summary>
/// TCP Server Start
/// PortNO:端口号,默认9600
/// dwUserData:用户自定义数据,用于回调函数里,默认NULL
/// onMsgReceive:接收客户端的数据回调函数
/// </summary>
BOOL CNTCPMSGAPI::TS_Start(int PortNO , DWORD dwUserData,OnMsgReceiveFunc_TS onMsgReceive)
{
	if(p_cmm == NULL)
		return FALSE;
	return p_cmm->TS_Start(PortNO,(unsigned long)dwUserData,(unsigned long)onMsgReceive);

}
/// <summary>
/// TCP Server Stop
/// </summary>
BOOL CNTCPMSGAPI::TS_Stop()
{
	if(p_cmm == NULL)
		return FALSE;
	return p_cmm->TS_Stop();
}
/// <summary>
/// TCP Server 获取开启状态
/// </summary>
/// <returns></returns>
BOOL CNTCPMSGAPI::TS_IsRun()
{
	if(p_cmm == NULL)
		return FALSE;
	return p_cmm->TS_IsRun();
}

//*********************** TCP Client *****************************************************************
/// <summary>
/// TCP Client Start
/// strIP:TCP Server的IP
/// PortNO:端口号,默认9600
/// dwUserData:用户自定义数据,用于回调函数里,默认NULL
/// onMsgReceive:接收Server端的数据回调函数
/// onDisposed:客户端退出的回调函数
/// </summary>
BOOL CNTCPMSGAPI::TC_Connect(CString strIP, int PortNO,DWORD dwUserData,
							 OnMsgReceiveFunc_TC onMsgReceive, OnDisConnectFunc onDisConnect)
{
	if(p_cmm == NULL)
		return FALSE;
	return p_cmm->TC_Connect(bstr_t(strIP),PortNO,(unsigned long)dwUserData,(unsigned long)onMsgReceive,(unsigned long)onDisConnect);
}
/// <summary>
/// TCP Client Stop
/// </summary>
BOOL CNTCPMSGAPI::TC_DisConnect()
{
	if(p_cmm == NULL)
		return FALSE;
	return p_cmm->TC_DisConnect();
}
/// <summary>
/// TCP Client 获取开启状态
/// </summary> 
BOOL CNTCPMSGAPI::TC_IsConnected()
{
	if(p_cmm == NULL)
		return FALSE;
	return p_cmm->TC_IsConnected();
} 
/// <summary>
/// 异步发送消息
/// uMsgID:消息标识,自定义
/// varMsg:消息内容,为常规数据,不可嵌套2层.
/// </summary> 
BOOL CNTCPMSGAPI::TC_AsyncSend(DWORD uMsgID,VARIANT& varMsg)
{
	if(p_cmm == NULL)
		return FALSE;
	return p_cmm->TC_AsyncSend(uMsgID,varMsg);
}
/// <summary>
/// 异步发送消息
/// uMsgID:消息标识,自定义
/// strMsg:string消息
/// </summary> 
BOOL CNTCPMSGAPI::TC_AsyncSend(DWORD uMsgID,CString strMsg)
{
	if(p_cmm == NULL)
		return FALSE;
	VARIANT  varMsg;
	CV_Str_To_Variant(strMsg,varMsg); 
	BOOL bRet = TC_AsyncSend(uMsgID,varMsg);
	VariantClear(&varMsg);
	return bRet;
}
/// <summary>
/// 异步发送消息
/// uMsgID:消息标识,自定义
/// byteArray:Byte数组
/// size:Byte数组大小
/// </summary> 
BOOL CNTCPMSGAPI::TC_AsyncSend(DWORD uMsgID,byte* byteArray,ULONG size)
{
	if(p_cmm == NULL)
		return FALSE;

	if(byteArray == NULL)
		return FALSE;

	VARIANT  varMsg;
	CV_ByteArray_To_Variant(byteArray,size,varMsg); 

	BOOL bRet = TC_AsyncSend(uMsgID,varMsg);
 
	VariantClear(&varMsg);

	return bRet;
}
/// <summary>
/// 同步发送消息
/// uMsgID:消息标识,自定义
/// varMsg:消息内容,为常规数据,不可嵌套2层.
/// </summary> 
BOOL CNTCPMSGAPI::TC_SyncSend(DWORD uMsgID,VARIANT& varMsg,VARIANT& varDataReturn)
{
	if(p_cmm == NULL)
		return FALSE;
	return p_cmm->TC_SyncSend(uMsgID,varMsg,&varDataReturn);
}
/// <summary>
/// 同步发送消息
/// uMsgID:消息标识,自定义
/// strMsg:string消息
/// </summary> 
BOOL CNTCPMSGAPI::TC_SyncSend(DWORD uMsgID,CString strMsg,VARIANT& varDataReturn)
{
	if(p_cmm == NULL)
		return FALSE;
	VARIANT  varMsg;
	CV_Str_To_Variant(strMsg,varMsg); 
	BOOL bRet = TC_SyncSend(uMsgID,varMsg,varDataReturn);
	VariantClear(&varMsg);
	return bRet;
}
/// <summary>
/// 同步发送消息
/// uMsgID:消息标识,自定义
/// byteArray:Byte数组
/// size:Byte数组大小
/// </summary> 
BOOL CNTCPMSGAPI::TC_SyncSend(DWORD uMsgID,byte* byteArray,ULONG size,VARIANT& varDataReturn)
{
	if(p_cmm == NULL)
		return FALSE;

	if(byteArray == NULL)
		return FALSE;

	VARIANT  varMsg;
	CV_ByteArray_To_Variant(byteArray,size,varMsg); 

	BOOL bRet = TC_SyncSend(uMsgID,varMsg,varDataReturn);

	VariantClear(&varMsg);

	return bRet;
}




//************************************** Event Server ****************************************
/// <summary>
/// Event Server Start
/// PortNO:端口号,默认9800
/// dwUserData:用户自定义数据,用于回调函数里,默认NULL
/// onMsgReceive:接收客户端的数据回调函数
/// </summary>
BOOL CNTCPMSGAPI::ES_Start(int PortNO, DWORD dwUserData, OnMsgReceiveFunc_ES onMsgReceive)
{
	if(p_cmm == NULL)
		return FALSE;
	return p_cmm->ES_Start(PortNO,(unsigned long)dwUserData,(unsigned long)onMsgReceive);
}
/// <summary>
/// Event Server Stop
/// </summary>
BOOL CNTCPMSGAPI::ES_Stop()
{
	if(p_cmm == NULL)
		return FALSE;
	return p_cmm->ES_Stop();
}
/// <summary>
/// ES_IsRun
/// </summary>
BOOL CNTCPMSGAPI::ES_IsRun()
{
	if(p_cmm == NULL)
		return FALSE;
	return p_cmm->ES_IsRun();
}
/// <summary>
/// 触发事件,通知此事件所有订阅者
/// nEventID:唯一的事件ID
/// varData:事件数据
/// </summary>
BOOL CNTCPMSGAPI::ES_RaiseEvent(int nEventID, VARIANT& varData)
{
	if(p_cmm == NULL || nEventID <= 0)
		return FALSE;
	return p_cmm->ES_RaiseEvent(nEventID,varData);
}

//************************************** Event Client ****************************************
/// <summary>
/// Event Client Connect 
/// strIP:Event Server的IP
/// PortNO:端口号,默认9800
/// dwUserData:用户自定义数据,用于回调函数里,默认NULL
/// onMsgReceive:接收Server端的数据回调函数
/// onDisposed:客户端退出的回调函数
/// </summary>
BOOL CNTCPMSGAPI::EC_Connect(CString strIP, int PortNO,DWORD dwUserData,
							 OnMsgReceiveFunc_EC onMsgReceive, OnDisConnectFunc onDisConnect)
{
	if(p_cmm == NULL)
		return FALSE;
	return p_cmm->EC_Connect(bstr_t(strIP),PortNO,(unsigned long)dwUserData,(unsigned long)onMsgReceive,(unsigned long)onDisConnect);

}
/// <summary>
/// Event Client DisConnect
/// </summary>
BOOL CNTCPMSGAPI::EC_DisConnect()
{
	if(p_cmm == NULL)
		return FALSE;
	return p_cmm->EC_DisConnect();
}
/// <summary>
/// EC_IsConnected
/// </summary>
BOOL CNTCPMSGAPI::EC_IsConnected()
{
	if(p_cmm == NULL)
		return FALSE;
	return p_cmm->EC_IsConnected();
}
/// <summary>
/// 订阅事件
/// eventName:事件名
/// varParameter:事件参数,不可嵌套2层.
/// 返回订阅的事件ID,小于等于0为错误,大于0的为成功.
/// </summary>
int  CNTCPMSGAPI::EC_Subscribe(CString eventName, VARIANT& varParameter)
{
	if(p_cmm == NULL)
		return -1;
	return p_cmm->EC_Subscribe(bstr_t(eventName),varParameter);
}
/// <summary>
/// 退订事件
/// nEventID:唯一的事件ID
/// </summary>
BOOL CNTCPMSGAPI::EC_UnSubscribe(int nEventID)
{
	if(p_cmm == NULL || nEventID <= 0)
		return FALSE;
	return p_cmm->EC_UnSubscribe(nEventID);
}
/// <summary>
/// 触发事件,通知此事件其他所有订阅者
/// nEventID:唯一的事件ID
/// varData:事件数据
/// </summary>
BOOL CNTCPMSGAPI::EC_RaiseEvent(int nEventID, VARIANT& varData)
{
	if(p_cmm == NULL || nEventID <= 0)
		return FALSE;
	return p_cmm->EC_RaiseEvent(nEventID,varData);
} 