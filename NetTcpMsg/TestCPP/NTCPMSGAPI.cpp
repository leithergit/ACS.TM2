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
/// CStringת��VARIANT
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
/// Byte����ת��VARIANT
/// byteArray:Byte����
/// size:Byte�����С,���Ϊ4096�ֽ�
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
/// PortNO:�˿ں�,Ĭ��9600
/// dwUserData:�û��Զ�������,���ڻص�������,Ĭ��NULL
/// onMsgReceive:���տͻ��˵����ݻص�����
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
/// TCP Server ��ȡ����״̬
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
/// strIP:TCP Server��IP
/// PortNO:�˿ں�,Ĭ��9600
/// dwUserData:�û��Զ�������,���ڻص�������,Ĭ��NULL
/// onMsgReceive:����Server�˵����ݻص�����
/// onDisposed:�ͻ����˳��Ļص�����
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
/// TCP Client ��ȡ����״̬
/// </summary> 
BOOL CNTCPMSGAPI::TC_IsConnected()
{
	if(p_cmm == NULL)
		return FALSE;
	return p_cmm->TC_IsConnected();
} 
/// <summary>
/// �첽������Ϣ
/// uMsgID:��Ϣ��ʶ,�Զ���
/// varMsg:��Ϣ����,Ϊ��������,����Ƕ��2��.
/// </summary> 
BOOL CNTCPMSGAPI::TC_AsyncSend(DWORD uMsgID,VARIANT& varMsg)
{
	if(p_cmm == NULL)
		return FALSE;
	return p_cmm->TC_AsyncSend(uMsgID,varMsg);
}
/// <summary>
/// �첽������Ϣ
/// uMsgID:��Ϣ��ʶ,�Զ���
/// strMsg:string��Ϣ
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
/// �첽������Ϣ
/// uMsgID:��Ϣ��ʶ,�Զ���
/// byteArray:Byte����
/// size:Byte�����С
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
/// ͬ��������Ϣ
/// uMsgID:��Ϣ��ʶ,�Զ���
/// varMsg:��Ϣ����,Ϊ��������,����Ƕ��2��.
/// </summary> 
BOOL CNTCPMSGAPI::TC_SyncSend(DWORD uMsgID,VARIANT& varMsg,VARIANT& varDataReturn)
{
	if(p_cmm == NULL)
		return FALSE;
	return p_cmm->TC_SyncSend(uMsgID,varMsg,&varDataReturn);
}
/// <summary>
/// ͬ��������Ϣ
/// uMsgID:��Ϣ��ʶ,�Զ���
/// strMsg:string��Ϣ
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
/// ͬ��������Ϣ
/// uMsgID:��Ϣ��ʶ,�Զ���
/// byteArray:Byte����
/// size:Byte�����С
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
/// PortNO:�˿ں�,Ĭ��9800
/// dwUserData:�û��Զ�������,���ڻص�������,Ĭ��NULL
/// onMsgReceive:���տͻ��˵����ݻص�����
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
/// �����¼�,֪ͨ���¼����ж�����
/// nEventID:Ψһ���¼�ID
/// varData:�¼�����
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
/// strIP:Event Server��IP
/// PortNO:�˿ں�,Ĭ��9800
/// dwUserData:�û��Զ�������,���ڻص�������,Ĭ��NULL
/// onMsgReceive:����Server�˵����ݻص�����
/// onDisposed:�ͻ����˳��Ļص�����
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
/// �����¼�
/// eventName:�¼���
/// varParameter:�¼�����,����Ƕ��2��.
/// ���ض��ĵ��¼�ID,С�ڵ���0Ϊ����,����0��Ϊ�ɹ�.
/// </summary>
int  CNTCPMSGAPI::EC_Subscribe(CString eventName, VARIANT& varParameter)
{
	if(p_cmm == NULL)
		return -1;
	return p_cmm->EC_Subscribe(bstr_t(eventName),varParameter);
}
/// <summary>
/// �˶��¼�
/// nEventID:Ψһ���¼�ID
/// </summary>
BOOL CNTCPMSGAPI::EC_UnSubscribe(int nEventID)
{
	if(p_cmm == NULL || nEventID <= 0)
		return FALSE;
	return p_cmm->EC_UnSubscribe(nEventID);
}
/// <summary>
/// �����¼�,֪ͨ���¼��������ж�����
/// nEventID:Ψһ���¼�ID
/// varData:�¼�����
/// </summary>
BOOL CNTCPMSGAPI::EC_RaiseEvent(int nEventID, VARIANT& varData)
{
	if(p_cmm == NULL || nEventID <= 0)
		return FALSE;
	return p_cmm->EC_RaiseEvent(nEventID,varData);
} 