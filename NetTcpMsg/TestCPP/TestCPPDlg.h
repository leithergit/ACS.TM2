// TestCPPDlg.h : ͷ�ļ�
//

#pragma once


// CTestCPPDlg �Ի���
class CTestCPPDlg : public CDialog
{
// ����
public:
	CTestCPPDlg(CWnd* pParent = NULL);	// ��׼���캯��

// �Ի�������
	enum { IDD = IDD_TESTCPP_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV ֧��


// ʵ��
protected:
	HICON m_hIcon;

	// ���ɵ���Ϣӳ�亯��
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedOk();

	static void TS_OnMessageReceiveFunc(DWORD dwUserData,DWORD  uEvent,VARIANT& varData,VARIANT& varDataToClient);

	static void TC_OnMessageReceiveFunc(DWORD dwUserData,DWORD  uEvent,VARIANT& varData);
};
