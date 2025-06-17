/*Copyright 2011 Arash Norouzi

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/



using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Dashboard.Common.Business.Component;

namespace MDM.iOS.Business.BusinessLogics.MDM_APNS
{
    /// <summary>
    /// Main class used for sending a push notification from MDM server to APNS server 
    /// </summary>
    public class PushNotification
    {
        private TcpClient _apnsClient;
        private SslStream _apnsStream;
        private X509Certificate _certificate;
        private X509CertificateCollection _certificates;

        private string _p12File { get; set; }
        private string _p12FilePassword { get; set; }

        // Default configurations for APNS
        private const string ProductionHost = "gateway.push.apple.com";
        private const string SandboxHost = "gateway.sandbox.push.apple.com";
        private const int NotificationPort = 2195;

        // Default configurations for Feedback Service
        private const string ProductionFeedbackHost = "feedback.push.apple.com";
        private const string SandboxFeedbackHost = "feedback.sandbox.push.apple.com";
        private const int FeedbackPort = 2196;

        private string _appDeviceToken;
        private bool _conected = false;

        private readonly string _host;
        private readonly string _feedbackHost;

        private List<NotificationPayload> _notifications = new List<NotificationPayload>();
        private List<string> _rejected = new List<string>();

        private Dictionary<int, string> _errorList = new Dictionary<int, string>();


        /// <summary>
        /// Constructor for PushNotification
        /// </summary>
        /// <param name="useSandbox"></param>
        /// <param name="p12File"></param>
        /// <param name="p12FilePassword"></param>
        public PushNotification(bool useSandbox, string p12File, string p12FilePassword)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            if (useSandbox)
            {
                _host = SandboxHost;
                _feedbackHost = SandboxFeedbackHost;
            }
            else
            {
                _host = ProductionHost;
                _feedbackHost = ProductionFeedbackHost;
            }

            // set the P12 File and passwords 
            _p12File = p12File;
            _p12FilePassword = p12FilePassword;


            //Load Certificates in to collection.
            _certificate = string.IsNullOrEmpty(p12FilePassword) ? new X509Certificate2(File.ReadAllBytes(p12File)) : new X509Certificate2(File.ReadAllBytes(p12File), p12FilePassword);
            _certificates = new X509CertificateCollection { _certificate };

            // Loading Apple error response list.
            _errorList.Add(0, "No errors encountered");
            _errorList.Add(1, "Processing error");
            _errorList.Add(2, "Missing device token");
            _errorList.Add(3, "Missing topic");
            _errorList.Add(4, "Missing payload");
            _errorList.Add(5, "Invalid token size");
            _errorList.Add(6, "Invalid topic size");
            _errorList.Add(7, "Invalid payload size");
            _errorList.Add(8, "Invalid token");
            _errorList.Add(255, "None (unknown)");
        }

        public PushNotification()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }


        // This is called from the ipad_mobileconfig file. 
        /// <summary>
        /// The main method and entry point of the Push Notification class. Sends the list of 
        /// notification paylad to the APNS server. 
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        public List<string> SendToApple(List<NotificationPayload> queue)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name, ',',
                                 "Payload queue received.");

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name, ',',
                                 "My queue count :" + queue.Count.ToString());

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name, ',',
                                 "My queue items :" + queue);



            _notifications = queue;
            if (queue.Count < 8999)
            {
                SendQueueToapple(_notifications);
            }
            else
            {
                const int pageSize = 8999;
                int numberOfPages = (queue.Count / pageSize) + (queue.Count % pageSize == 0 ? 0 : 1);
                int currentPage = 0;

                while (currentPage < numberOfPages)
                {
                    _notifications = (queue.Skip(currentPage * pageSize).Take(pageSize)).ToList();
                    SendQueueToapple(_notifications);
                    currentPage++;
                }
            }
            //Close the connection
            Disconnect();
            return _rejected;
        }

        public void SendNotification()
        {

            int port = 2195;
            //String hostname = "gateway.sandbox.push.apple.com";
            String hostname = "gateway.push.apple.com";
            String certificatePassword = "123456";
            //string certificatePath = "E:\\IOS_MDM\\Publishers\\Console.Alexis.MDM\\Applecertificate\\testiosmdmpushnotification.p12";
            string certificatePath = "C:\\Users\\bryan\\Downloads\\bank_islam_official\\Apps\\Apps\\Console.Alexis.MDM\\Applecertificate\\testiosmdmpushnotification.p12";

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    "checkAppleServerConnection.log",
                    ',', "Certificate Path: " + certificatePath);

            TcpClient client = new TcpClient(hostname, port);
            X509Certificate2 clientCertificate = new X509Certificate2(System.IO.File.ReadAllBytes(certificatePath), certificatePassword);
            X509Certificate2Collection certificatesCollection = new X509Certificate2Collection(clientCertificate);
            SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(validateServerCertificate), null);


            string DeviceToken = "32aee722052aed37ebdff1880117469ffb99cac080b78f6d7c1f1ab64e1756ec";
            String LoginName = "Name";
            int Counter = 1; //Badge Count;  
            String Message = "You succeeeded triggering Push Notifications! 🤩🥳😎🤩🥳😎";
            String UID = "your choice UID";
            String payload = "{\"aps\":{\"alert\":\"" + Message + "\",\"badge\":" + Counter + ",\"sound\":\"default\"},\"UID\":\"" + UID + "\",\"LoginName\":\"" + LoginName + "\"}";

            try
            {

                sslStream.AuthenticateAsClient(hostname, certificatesCollection, SslProtocols.Tls, false);
                MemoryStream memoryStream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(memoryStream);

                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((byte)32);

                writer.Write(HexStringToByteArray(DeviceToken.ToUpper()));
                writer.Write((byte)0);
                writer.Write((byte)payload.Length);
                byte[] b1 = System.Text.Encoding.UTF8.GetBytes(payload);
                writer.Write(b1);
                writer.Flush();
                byte[] array = memoryStream.ToArray();
                sslStream.Write(array);
                sslStream.Flush();
                client.Close();
            }

            catch (System.Security.Authentication.AuthenticationException e)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        "checkAppleServerConnection.log",
                        ',', "Exception: " + e);
                client.Close();
            }

            catch (Exception e)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        "checkAppleServerConnection.log",
                        ',', "Exception: " + e);
                client.Close();
            }
        }


        public void SendiOSAppPushNotification(string sandbox, string deviceToken, string certName, string certPwd, string isSilent)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            // place the arguments to be used for the APN controller 
            string url = String.Format("http://47.250.45.40/testapple2/apn/{0}/{1}/{2}/{3}/{4}", sandbox, deviceToken, certName, certPwd, isSilent);

            // Create an HttpClient instance 
            HttpClient client = new HttpClient();
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name,
                 ',', "url -> " + url);


            // Send a GET request asynchronously continue when complete 
            client.GetAsync(url).ContinueWith(
                (requestTask) =>
                {
                    // Get HTTP response from completed task. 
                    HttpResponseMessage response = requestTask.Result;

                    // Check that response was successful or throw exception 
                    response.EnsureSuccessStatusCode();

                    // Read response asynchronously as JsonValue
                    Task<string> content = response.Content.ReadAsStringAsync();

                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                      System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                      System.Reflection.MethodBase.GetCurrentMethod().Name,
                                      ',', "response.Content -> " + content.Result);

                });
        }


        /// <summary>
        /// Convert a hexstring to an array of bytes. 
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        /// <summary>
        /// Helper function to send the payload notifications in the form of queue to the APNS server for MDM-related commands. 
        /// </summary>
        /// <param name="queue"></param>
        private void SendQueueToapple(IEnumerable<NotificationPayload> queue)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            int i = 1000;
            foreach (var item in queue)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    "checkAppleServerConnection.log",
                                    ',', "SendQueueToapple _conected = " + _conected);

                string mdmkey = "";
                foreach (string key in item.CustomItems.Keys)
                {
                    mdmkey = (string)item.CustomItems[key][0];

                }
                //string url = "http://localhost:30042/apn/AC39A7411B408509BB174B48F3FF18BE5D8B0B688FABD2951BF046D574E8C015/6E25EF7B-C32C-4916-BE6B-02EB6956463C"
                string url = "http://47.250.45.40/testapple/" + "apn/" + item.DeviceToken + "/" + mdmkey;

                // host the url in the alibaba cloud. 

                // Create an HttpClient instance 
                HttpClient client = new HttpClient();
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                     System.Reflection.MethodBase.GetCurrentMethod().Name,
                     ',', "url -> " + url);

                // Send a request asynchronously continue when complete 
                client.GetAsync(url).ContinueWith(
                    (requestTask) =>
                    {
                        // Get HTTP response from completed task. 
                        HttpResponseMessage response = requestTask.Result;

                        // Check that response was successful or throw exception 
                        response.EnsureSuccessStatusCode();

                        // Read response asynchronously as JsonValue
                        Task<string> content = response.Content.ReadAsStringAsync();

                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                          System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                          System.Reflection.MethodBase.GetCurrentMethod().Name,
                                          ',', "response.Content -> " + content.Result);

                    });
                Thread.Sleep(1000); //Wait to get the response from apple.

            }
        }


        /// <summary>
        /// Read the response from the APNS server. 
        /// </summary>
        /// <param name="ar"></param>
        private void ReadResponse(IAsyncResult ar)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    "checkAppleServerConnection.log",
                                    ',', "ReadResponse _conected = " + _conected);
            if (!_conected)
                return;

            string payLoadId = "";
            int payLoadIndex = 0;
            try
            {

                var info = ar.AsyncState as MyAsyncInfo;
                info.MyStream.ReadTimeout = 100;

                if (_apnsStream.CanRead)
                {
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           ','
                           , "ReadResponse infoByteArray = "
                           , " [0] = " + info.ByteArray[0].ToString()
                           , " [1] = " + info.ByteArray[1].ToString()
                           , " [2] = " + info.ByteArray[2].ToString()
                           , " [3] = " + info.ByteArray[3].ToString()
                           , " [4] = " + info.ByteArray[4].ToString()
                           , " [5] = " + info.ByteArray[5].ToString());

                    var command = Convert.ToInt16(info.ByteArray[0]);
                    var status = Convert.ToInt16(info.ByteArray[1]);
                    var ID = new byte[4];
                    Array.Copy(info.ByteArray, 2, ID, 0, 4);

                    payLoadId = Encoding.Default.GetString(ID);
                    payLoadIndex = ((int.Parse(payLoadId)) - 1000);

                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           ','
                           , "Apple rejected payload for device token : " + _notifications[payLoadIndex].DeviceToken
                           , "Apple Error code : " + _errorList[status]
                           , "Connection terminated by Apple.");

                    _rejected.Add(_notifications[payLoadIndex].DeviceToken);
                    _conected = false;

                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    "checkAppleServerConnection.log",
                                    ',', "SendQueueToapple2 _conected = " + _conected);
                }
            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name,
                                 ',', "An error occurred while reading Apple response for token {0} - {1}", _notifications[payLoadIndex].DeviceToken, ex.Message);

            }
        }

        /// <summary>
        /// Establishes a secure SSL connection with the APNS (Apple Push Notification Service) server. 
        /// </summary>
        private void Connect(string host, int port, X509CertificateCollection certificates)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name,
                                 ',', "Connecting to apple server.");
            try
            {
                _apnsClient = new TcpClient();
                _apnsClient.Connect(host, port);
            }
            catch (SocketException ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name,
                                 ',', "An error occurred while connecting to APNS servers - " + ex.Message);
            }

            var sslOpened = OpenSslStream(host, certificates);

            if (sslOpened)
            {
                _conected = true;
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                System.Reflection.MethodBase.GetCurrentMethod().Name,
                                ',', "Connected.");

                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    "checkAppleServerConnection.log",
                                    ',', "Connect sslOpened _conected = " + _conected);
            }
            else
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    "checkAppleServerConnection.log",
                                    ',', "Connect sslOpened Failed");
            }

        }

        /// <summary>
        /// Disconnect the connection with APNS server.
        /// </summary>
        private void Disconnect()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            try
            {
                Thread.Sleep(500);
                _apnsClient.Close();
                _apnsStream.Close();
                _apnsStream.Dispose();
                _apnsStream = null;
                _conected = false;


                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    "checkAppleServerConnection.log",
                                    ',', "Disconnect _conected = " + _conected);

                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                System.Reflection.MethodBase.GetCurrentMethod().Name,
                                ',', "Disconnected.");
            }
            catch (Exception ex)
            {

                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name,
                                 ',', "An error occurred while disconnecting. - " + ex.Message);
            }
        }

        /// <summary>
        /// Create a SSL connection and authenticate the user's identity based on hostname and certificates available. 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="certificates"></param>
        /// <returns></returns>
        private bool OpenSslStream(string host, X509CertificateCollection certificates)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                              System.Reflection.MethodBase.GetCurrentMethod().Name,
                              ',', "Creating SSL connection.");
            _apnsStream = new SslStream(_apnsClient.GetStream(), false, validateServerCertificate, SelectLocalCertificate);

            try
            {
                //System.Security.Cryptography.X509Certificates.X509Certificate2Collection xc = new System.Security.Cryptography.X509Certificates.X509Certificate2Collection();
                _apnsStream.AuthenticateAsClient(host, certificates, System.Security.Authentication.SslProtocols.Tls12, false);
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                             System.Reflection.MethodBase.GetCurrentMethod().Name,
                             ex.Message);
                return false;
            }

            if (!_apnsStream.IsMutuallyAuthenticated)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                             System.Reflection.MethodBase.GetCurrentMethod().Name,
                             ',', "SSL Stream Failed to Authenticate");
                return false;
            }

            if (!_apnsStream.CanWrite)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                             System.Reflection.MethodBase.GetCurrentMethod().Name,
                             ',', "SSL Stream is not Writable");
                return false;
            }
            return true;
        }

        /// <summary>
        /// This method validates the server certificate. However the current implementation doesn't explicitly validate it. 
        /// It will always returns true. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        private bool validateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true; // Dont care about server's cert
        }

        /// <summary>
        /// This method retrieves the local certificate as an X509Certificate object. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="targetHost"></param>
        /// <param name="localCertificates"></param>
        /// <param name="remoteCertificate"></param>
        /// <param name="acceptableIssuers"></param>
        /// <returns></returns>
        private X509Certificate SelectLocalCertificate(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            return _certificate;
        }



        private static byte[] GeneratePayload(NotificationPayload payload)
        {
            try
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

                //convert Devide token to HEX value.
                byte[] deviceToken = new byte[payload.DeviceToken.Length / 2];
                for (int i = 0; i < deviceToken.Length; i++)
                    deviceToken[i] = byte.Parse(payload.DeviceToken.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);

                var memoryStream = new MemoryStream();

                // Command
                memoryStream.WriteByte(1); // Changed command Type 

                //Adding ID to Payload
                memoryStream.Write(Encoding.ASCII.GetBytes(payload.PayloadId.ToString()), 0, payload.PayloadId.ToString().Length);

                //Adding ExpiryDate to Payload
                int epoch = (int)(DateTime.UtcNow.AddMinutes(300) - new DateTime(1970, 1, 1)).TotalSeconds;
                byte[] timeStamp = BitConverter.GetBytes(epoch);
                memoryStream.Write(timeStamp, 0, timeStamp.Length);

                byte[] tokenLength = BitConverter.GetBytes((Int16)32);
                Array.Reverse(tokenLength);
                // device token length
                memoryStream.Write(tokenLength, 0, 2);

                // Token
                memoryStream.Write(deviceToken, 0, 32);

                // String length
                string apnMessage = payload.ToJson();
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           ',', "Payload generated for " + payload.DeviceToken + " : " + apnMessage);

                byte[] apnMessageLength = BitConverter.GetBytes((Int16)apnMessage.Length);
                Array.Reverse(apnMessageLength);

                // message length
                memoryStream.Write(apnMessageLength, 0, 2);

                // Write the message
                memoryStream.Write(Encoding.ASCII.GetBytes(apnMessage), 0, apnMessage.Length);
                return memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           ',', "An error occurred while disconnecting. - " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Retrieve the feedback from APNS server after sending a push notification. 
        /// </summary>
        /// <returns></returns>
        public List<Feedback> GetFeedBack()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            try
            {
                var feedbacks = new List<Feedback>();
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           ',', "Connecting to feedback service.");


                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    "checkAppleServerConnection.log",
                                    ',', "GetFeedBack _conected = " + _conected);

                if (!_conected)
                    Connect(_feedbackHost, FeedbackPort, _certificates);

                if (_conected)
                {
                    //Set up
                    byte[] buffer = new byte[38];
                    int recd = 0;
                    DateTime minTimestamp = DateTime.Now.AddYears(-1);

                    //Get the first feedback
                    recd = _apnsStream.Read(buffer, 0, buffer.Length);
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           ',', "Feedback response received.");

                    if (recd == 0)
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           ',', "Feedback response is empty.");


                    //Continue while we have results and are not disposing
                    while (recd > 0)
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           ',', "processing feedback response");
                        var fb = new Feedback();

                        //Get our seconds since 1970
                        byte[] bSeconds = new byte[4];
                        byte[] bDeviceToken = new byte[32];

                        Array.Copy(buffer, 0, bSeconds, 0, 4);

                        //Check endianness
                        if (BitConverter.IsLittleEndian)
                            Array.Reverse(bSeconds);

                        int tSeconds = BitConverter.ToInt32(bSeconds, 0);

                        //Add seconds since 1970 to that date, in UTC and then get it locally
                        fb.Timestamp = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(tSeconds).ToLocalTime();


                        //Now copy out the device token
                        Array.Copy(buffer, 6, bDeviceToken, 0, 32);

                        fb.DeviceToken = BitConverter.ToString(bDeviceToken).Replace("-", "").ToLower().Trim();

                        //Make sure we have a good feedback tuple
                        if (fb.DeviceToken.Length == 64 && fb.Timestamp > minTimestamp)
                        {
                            //Raise event
                            //this.Feedback(this, fb);
                            feedbacks.Add(fb);
                        }

                        //Clear our array to reuse it
                        Array.Clear(buffer, 0, buffer.Length);

                        //Read the next feedback
                        recd = _apnsStream.Read(buffer, 0, buffer.Length);
                    }
                    //clode the connection here !
                    Disconnect();
                    if (feedbacks.Count > 0)
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           ',', "Total {0} feedbacks received.", feedbacks.Count);
                    return feedbacks;
                }
            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           ',', "An error occurred while disconnecting. - " + ex.Message);
                return null;
            }
            return null;
        }
    }

    public class MyAsyncInfo
    {
        public Byte[] ByteArray { get; set; }
        public SslStream MyStream { get; set; }

        public MyAsyncInfo(Byte[] array, SslStream stream)
        {
            ByteArray = array;
            MyStream = stream;
        }
    }
}
