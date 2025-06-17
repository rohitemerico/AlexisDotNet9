using Alexis.Common;
using Dashboard.Common.Business.Component;
using MDM.iOS.Entities.MDM;

namespace MDM.iOS.Business.BusinessLogics.MDM.MDM_MessageQueue;

/// <summary>
/// MessageQueue_Fire is another intermediate class that uses message queue to push the 
/// notifications to another end device. MessageQueue is the actual class. 
/// </summary>
public class MessageQueue_Fire
{
    /// <summary>
    /// Push the available command list to the designated message queue. This is used in the 
    /// checkIn and server controller components of the MDM server. 
    /// </summary>
    /// <param name="my_MQ_IpadCommandList_List"></param>
    /// <returns></returns>
    #region Send Req to message
    public static bool ReqtoMessage(List<MQ_IpadCommandList> my_MQ_IpadCommandList_List)
    {
        bool ret = false;
        try
        {
            foreach (MQ_IpadCommandList my_MQ_IpadCommandList in my_MQ_IpadCommandList_List)
            {
                MessageQueue<MQ_IpadCommandList> my_MessageQueue = new MessageQueue<MQ_IpadCommandList>();


                // the MQueue part will not work. We need to change the app settings for it to work. 
                // which part of web config should we make the changes? 
                my_MessageQueue.MessageQueueSend(ConfigHelper.MQueue, my_MQ_IpadCommandList.commandName.ToString(), my_MQ_IpadCommandList);
            }

            ret = true;
        }
        catch (Exception ex)
        {

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                               System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                               System.Reflection.MethodBase.GetCurrentMethod().Name,
                               ex);
        }

        return ret;
    }

    /// <summary>
    /// Push the available command list to the designated message queue. This is used in the 
    /// dashboard components. 
    /// </summary>
    /// <param name="my_MQ_IpadCommandList_List"></param>
    /// <returns></returns>
    public static bool PushtoMessage(List<MQ_IpadCommandList> my_MQ_IpadCommandList_List)
    {
        bool ret = false;
        try
        {
            foreach (MQ_IpadCommandList my_MQ_IpadCommandList in my_MQ_IpadCommandList_List)
            {
                MessageQueue<MQ_IpadCommandList> my_MessageQueue = new MessageQueue<MQ_IpadCommandList>();

                // sending the commands to the message queue using the MQueue value obtained from web.config 
                my_MessageQueue.MessageQueueSend(ConfigHelper.MQueue, my_MQ_IpadCommandList.commandName.ToString(), my_MQ_IpadCommandList, my_MQ_IpadCommandList.Mprio);

                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name,
                                   "PushToMessage - Firing message is successful, queue name : " + ConfigHelper.MQueue);
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name,
                                   "PushToMessage - Firing message is successful, command : " + my_MQ_IpadCommandList.commandName);

                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                               System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                               System.Reflection.MethodBase.GetCurrentMethod().Name,
                               "============================================================================================");
            }


            ret = true;
        }
        catch (Exception ex)
        {

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                               System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                               System.Reflection.MethodBase.GetCurrentMethod().Name,
                               ex);

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                               System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                               System.Reflection.MethodBase.GetCurrentMethod().Name,
                               "PushToMessage - Firing message is failing, queue name : " + ConfigHelper.MQueue);

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           "============================================================================================");
        }

        return ret;
    }
    #endregion
}
