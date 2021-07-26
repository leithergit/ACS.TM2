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
	/// CStringת��VARIANT
	/// </summary> 
	BOOL CV_Str_To_Variant(CString str,VARIANT& var);
	/// <summary>
	/// Byte����ת��VARIANT
	/// byteArray:Byte����
	/// size:Byte�����С,���Ϊ4096�ֽ�
	/// </summary> 
	BOOL CV_ByteArray_To_Variant(byte* byteArray,ULONG size,VARIANT& var);

//***********************  TCP Server  ******************************************************************
	/// <summary>
	/// TCP Server Start
	/// PortNO:�˿ں�,Ĭ��9600
	/// dwUserData:�û��Զ�������,���ڻص�������,Ĭ��NULL
	/// onMsgReceive:���տͻ��˵����ݻص�����
	/// </summary>
	BOOL TS_Start(int PortNO = 9600,DWORD dwUserData=NULL,OnMsgReceiveFunc_TS onMsgReceive=NULL);
	/// <summary>
	/// TCP Server Stop
	/// </summary>
	BOOL TS_Stop();
	/// <summary>
	/// TCP Server��ȡ����״̬
	/// </summary>
	/// <returns></returns>
	BOOL TS_IsRun(); 

//*********************** TCP Client *****************************************************************
	/// <summary>
	/// TCP Client Start
	/// strIP:TCP Server��IP
	/// PortNO:�˿ں�,Ĭ��9600
	/// dwUserData:�û��Զ�������,���ڻص�������,Ĭ��NULL
	/// onMsgReceive:����Server�˵����ݻص�����
	/// onDisConnect:�ͻ����˳��Ļص�����
	/// </summary>
	BOOL TC_Connect(CString strIP, int PortNO = 9600,DWORD dwUserData=NULL,
					OnMsgReceiveFunc_TC onMsgReceive=NULL,OnDisConnectFunc onDisConnect=NULL );
	/// <summary>
	/// TCP Client Stop
	/// </summary>
	BOOL TC_DisConnect();
	/// <summary>
	/// TCP Client ��ȡ����״̬
	/// </summary> 
	BOOL TC_IsConnected(); 
	/// <summary>
	/// �첽������Ϣ
	/// uMsgID:��Ϣ��ʶ,�Զ���
	/// varMsg:��Ϣ����,Ϊ��������,����Ƕ��2��.
	/// </summary> 
	BOOL TC_AsyncSend(DWORD uMsgID,VARIANT& varMsg);
	/// <summary>
	/// �첽������Ϣ
	/// uMsgID:��Ϣ��ʶ,�Զ���
	/// strMsg:string��Ϣ
	/// </summary> 
	BOOL TC_AsyncSend(DWORD uMsgID,CString strMsg);
	/// <summary>
	/// �첽������Ϣ
	/// uMsgID:��Ϣ��ʶ,�Զ���
	/// byteArray:Byte����
	/// size:Byte�����С
	/// </summary> 
	BOOL TC_AsyncSend(DWORD uMsgID,byte* byteArray,ULONG size);

	/// <summary>
	/// ͬ��������Ϣ
	/// uMsgID:��Ϣ��ʶ,�Զ���
	/// varMsg:��Ϣ����,Ϊ��������,����Ƕ��2��.
	/// </summary> 
	BOOL TC_SyncSend(DWORD uMsgID,VARIANT& varMsg,VARIANT& varDataReturn);
	/// <summary>
	/// ͬ��������Ϣ
	/// uMsgID:��Ϣ��ʶ,�Զ���
	/// strMsg:string��Ϣ
	/// </summary> 
	BOOL TC_SyncSend(DWORD uMsgID,CString strMsg,VARIANT& varDataReturn);
	/// <summary>
	/// ͬ��������Ϣ
	/// uMsgID:��Ϣ��ʶ,�Զ���
	/// byteArray:Byte����
	/// size:Byte�����С
	/// </summary> 
	BOOL TC_SyncSend(DWORD uMsgID,byte* byteArray,ULONG size,VARIANT& varDataReturn);


//************************************** Event Server ****************************************
	/// <summary>
	/// Event Server Start
	/// PortNO:�˿ں�,Ĭ��9800
	/// dwUserData:�û��Զ�������,���ڻص�������,Ĭ��NULL
	/// onMsgReceive:���տͻ��˵����ݻص�����
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
	/// �����¼�,֪ͨ���¼����ж�����
	/// nEventID:Ψһ���¼�ID
	/// varData:�¼�����
	/// </summary>
    BOOL ES_RaiseEvent(int nEventID, VARIANT& varData);

//************************************** Event Client ****************************************
	/// <summary>
	/// Event Client Connect 
	/// strIP:Event Server��IP
	/// PortNO:�˿ں�,Ĭ��9800
	/// dwUserData:�û��Զ�������,���ڻص�������,Ĭ��NULL
	/// onMsgReceive:����Server�˵����ݻص�����
	/// onDisConnect:�ͻ����˳��Ļص�����
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
	/// �����¼�
	/// eventName:�¼���
	/// varParameter:�¼�����,����Ƕ��2��.
	/// ���ض��ĵ��¼�ID,С�ڵ���0Ϊ����,����0��Ϊ�ɹ�.
	/// </summary>
    int  EC_Subscribe(CString eventName, VARIANT& varParameter);
	/// <summary>
	/// �˶��¼�
	/// nEventID:Ψһ���¼�ID
	/// </summary>
    BOOL EC_UnSubscribe(int nEventID);
	/// <summary>
	/// �����¼�,֪ͨ���¼��������ж�����
	/// nEventID:Ψһ���¼�ID
	/// varData:�¼�����
	/// </summary>
    BOOL EC_RaiseEvent(int nEventID, VARIANT& varData); 
};