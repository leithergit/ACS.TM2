using System;
using Apache.NMS;
using ZECS.Common.ActiveMQ;
using System.Windows.Forms;


namespace UmsServer.Mq
{
    public class MqReciever
    {
        private ZQueue_Receive_Sync m_zQueueReceive = null;

        private bool m_bStart = false;

        public MqReciever()
        {
           
        }

        public void OnMessageReceive(IMessage message, out IMessage msgRet)
        {
            msgRet = null;
            try
            {
                string sendMessage = string.Empty;
                IMessage msg = message;
                if (msg is ITextMessage)
                {
                    ITextMessage txtMsg = msg as ITextMessage;
                    sendMessage = txtMsg.Text;
                }
                else if (msg is IObjectMessage)
                {
                    sendMessage = "IObjectMessage: " + msg.ToString();
                }
                else
                {
                    sendMessage = "Unexpected message type: " + msg.GetType().Name;
                }

                //返回消息
                msgRet = m_zQueueReceive.CreateTextMessage(sendMessage);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message.ToString());
            }
        }

        public void StopRecieveMq()
        {
            if (!m_bStart)
            {
                return;
            }
               
            m_zQueueReceive.Stop();
            m_bStart = false;
        }
    }

  
}
