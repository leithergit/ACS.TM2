
// SimulateSwipeCard.h : PROJECT_NAME Ӧ�ó������ͷ�ļ�
//

#pragma once

#ifndef __AFXWIN_H__
	#error "�ڰ������ļ�֮ǰ������stdafx.h�������� PCH �ļ�"
#endif

#include "resource.h"		// ������


// CSimulateSwipeCardApp: 
// �йش����ʵ�֣������ SimulateSwipeCard.cpp
//

class CSimulateSwipeCardApp : public CWinApp
{
public:
	CSimulateSwipeCardApp();

// ��д
public:
	virtual BOOL InitInstance();

// ʵ��

	DECLARE_MESSAGE_MAP()
};

extern CSimulateSwipeCardApp theApp;