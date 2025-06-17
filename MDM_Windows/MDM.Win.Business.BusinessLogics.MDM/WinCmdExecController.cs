using System.Text;
using Alexis.Common;
using Newtonsoft.Json;

namespace MDM.Win.Business.BusinessLogics.MDM;

//DM protocol commands, Exec: Invokes an executable on the client device
public class WinCmdExecController
{

    public static async Task<bool> RebootNow(string deviceId)
    {
        const string Endpoint = "insertcmd";
        const string CmdNodePath = "./Vendor/MSFT/Reboot/RebootNow";
        string BaseURL = ConfigHelper.WindowsMDMBaseURL;
        try
        {
            using HttpClient client = new();
            client.BaseAddress = new Uri(BaseURL);
            client.DefaultRequestHeaders.Clear();

            var dataObj = new PostDMSessionCmd()
            {
                DeviceId = deviceId,
                CommandNodePath = CmdNodePath
            };

            var json = JsonConvert.SerializeObject(dataObj);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(Endpoint, data);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                // Optionally log or process 'result' here
                return true;
            }
        }
        catch (Exception ex)
        {
            // Optionally log the exception here
            Console.Error.WriteLine($"Error during reboot request: {ex.Message}");
        }
        return false;
    }
}