using Apache.NMS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ZECS.Common.ActiveMQ;

namespace UmsServer.Mq
{
    public class MqSender
    {
        public static ZQueue_Send_Sync m_zQueueSend = null;

        private bool m_bStart = false;

        public string retMessage = string.Empty;

        public MqSender()
        {
            //ConnectSendMq();
        }

        public static string MqstrIP = "";
        public static int MqPort = 0;
        public bool ConnectSendMq()
        {
            //string strIP = "10.28.254.35";
            //int port = 61616;
            String strQueueOrTopicName = "TOUMSSERVER";
            m_zQueueSend = new ZQueue_Send_Sync(MqstrIP, MqPort, strQueueOrTopicName);
            m_bStart = m_zQueueSend.Start();
            if (!m_bStart)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Stop_SendMq()
        {
            if (!m_bStart)
            {
                return;
            }

            if (m_zQueueSend != null)
            {
                m_zQueueSend.Stop();
            }

            m_bStart = false;
        }

        public string SendMsg(string msg)
        {
            if (!m_zQueueSend.IsStart)
            {
                ConnectSendMq();
            }
            String DATA = msg;
            bool bSendRet = false;
            ITextMessage sendmsg = m_zQueueSend.CreateTextMessage(DATA);
            IMessage msgRet;
            bSendRet = m_zQueueSend.Send_Sync(sendmsg, out msgRet);
            if (bSendRet)
            {
                if (msgRet != null)
                {
                    ITextMessage msgText = msgRet as ITextMessage;
                    retMessage = msgText.Text;
                    return retMessage;
                }
                else
                {
                    return "false";
                }
            }
            else
            {
                return "false";
            }
        }
    }
}
