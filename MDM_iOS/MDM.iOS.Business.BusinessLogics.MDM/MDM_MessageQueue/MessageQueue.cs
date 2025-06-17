using System.Configuration;
using System.Text.RegularExpressions;
using Dashboard.Common.Business.Component;
using Experimental.System.Messaging;

public class MessageQueue<T>
{
    protected T body;
    public string label;

    #region Receive message from queue
    public T MessageQueuesReceive(string queuePath)
    {
        MessageQueue mq = new MessageQueue(queuePath);
        Message msg = new Message();
        try
        {
            msg = mq.Receive(new TimeSpan(0, 0, 0, 15));
            msg.Formatter = new XmlMessageFormatter(new Type[] { typeof(T) });

            label = (string)msg.Label;
            body = (T)msg.Body;

            mq.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return body;
    }
    #endregion

    #region Receive message from queue
    /// <summary>
    /// Receive message with specified timeout
    /// </summary>
    /// <param name="queuePath">Name of the queue</param>
    /// <param name="timeOut">Timeout value, 0 for no timeout</param>
    /// <returns></returns>
    public T MessageQueuesReceive(string queuePath, int timeOut)
    {
        MessageQueue mq = new MessageQueue(queuePath);
        Message msg = new Message();
        try
        {
            if (timeOut == 0)
                msg = mq.Receive();
            else
                msg = mq.Receive(new TimeSpan(0, 0, 0, timeOut));
            msg.Formatter = new XmlMessageFormatter(new Type[] { typeof(T) });

            label = (string)msg.Label;
            body = (T)msg.Body;

            mq.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return body;
    }
    #endregion

    #region Send message to queue
    public void MessageQueueSend(string queuePath, string label, T MyBody)
    {
        try
        {
            MessageQueue mq = new MessageQueue(queuePath);
            Message msg = new Message();
            msg.Formatter = new XmlMessageFormatter();
            msg.Recoverable = true;
            msg.Body = MyBody;
            msg.Label = label;
            mq.Send(msg);
            mq.Close();
        }
        catch (Exception ex)
        {
            Logger.LogToFile("MQ.log", ',', ex.Message);

            string newQueue = string.Empty;
            string failoverIp = ConfigurationManager.AppSettings.Get("FailOverIp");
            string MachineIp = ConfigurationManager.AppSettings.Get("MachineIp");
            if (queuePath.Contains("TCP"))
                newQueue = Regex.Replace(queuePath, MachineIp, failoverIp);
            else
                newQueue = Regex.Replace(queuePath, "OS:.", "TCP:" + failoverIp);
            MessageQueue mq = new MessageQueue(newQueue);
            Message msg = new Message();
            msg.Formatter = new XmlMessageFormatter();
            msg.Recoverable = true;
            msg.Body = MyBody;
            msg.Label = label;
            mq.Send(msg);
            mq.Close();
            Console.WriteLine("Send to Alternative queue");
        }
    }
    #endregion

    #region Send message to queue
    public void MessageQueueSend(string queuePath, string label, T MyBody, int priority)
    {
        MessageQueue mq = null;

        //if (!MessageQueue.Exists(queuePath))
        //{
        //    mq = MessageQueue.Create(queuePath);
        //}
        //else
        mq = new MessageQueue(queuePath);

        try
        {
            Message msg = new Message();
            msg.Formatter = new XmlMessageFormatter();
            msg.Recoverable = true;
            msg.Body = MyBody;
            msg.Label = label;

            switch (priority)
            {
                case 2:
                    msg.Priority = MessagePriority.Lowest;
                    break;
                case 3:
                    msg.Priority = MessagePriority.VeryLow;
                    break;
                case 4:
                    msg.Priority = MessagePriority.Low;
                    break;
                case 5:
                    msg.Priority = MessagePriority.Normal;
                    break;
                case 6:
                    msg.Priority = MessagePriority.AboveNormal;
                    break;
                case 7:
                    msg.Priority = MessagePriority.VeryHigh;
                    break;
                case 8:
                    msg.Priority = MessagePriority.Highest;
                    break;
                default:
                    msg.Priority = MessagePriority.Normal;
                    break;
            }

            mq.Send(msg);
            mq.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    #endregion
}

