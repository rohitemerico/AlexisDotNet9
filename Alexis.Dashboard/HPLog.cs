namespace Alexis.Dashboard;

public static class HPLog
{
    public static string KIOSKID = "Dashboard";

    private static string CreateFilePath()
    {
        string PagePath = System.Configuration.ConfigurationManager.AppSettings["LogDirectory"];
        string LogPath = PagePath;

        if (!Directory.Exists(PagePath))
        {
            Directory.CreateDirectory(PagePath);
        }

        if (!Directory.Exists(LogPath))
        {
            Directory.CreateDirectory(LogPath);
        }

        return LogPath;
    }

    public static void WriteLog(string exMsg, string MsgType)
    {
        int i = 0;

        while (true)
        {
            if (i == 5)//set retry to 5 to prevent unlimited loop from happening if directory cannot be found
            {
                break;
            }

            try
            {
                MoveFile();
                string Newpath = CreateFilePath() + "\\ErrorLog.txt";

                if (!File.Exists(Newpath))
                {
                    StreamWriter oFile = File.CreateText(Newpath);
                    oFile.Close();
                    TextWriter tw = new StreamWriter(Newpath);
                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + ", " + MsgType + ", " + KIOSKID + ", " + exMsg.Trim());
                    tw.Close();
                    break;
                }
                else if (File.Exists(Newpath))
                {
                    TextWriter tw = new StreamWriter(Newpath, true);
                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + ", " + MsgType + ", " + KIOSKID + ", " + exMsg.Trim());
                    tw.Close();
                    break;
                }
            }
            catch
            {
                //Do nothing let it loop and 
                i++;
                continue;
            }
        }
    }

    public static void MoveFile()
    {
        //StreamReader file = new System.IO.StreamReader(CreateFilePath() + "\\ErrorLog.txt");


        string Newpath = CreateFilePath() + "\\ErrorLog_.txt";
        if (File.Exists(Newpath))
        {
            try
            {
                string line = File.ReadAllLines(CreateFilePath() + "\\ErrorLog.txt").First();
                if (line != "")
                {
                    string[] words = line.Split(',');

                    DateTime dtZ = CYParse(words[0]).Date;
                    DateTime dtN = DateTime.Now.Date;

                    if (dtZ != dtN)
                    {
                        if (File.Exists(CreateFilePath() + "\\ErrorLog_" + dtZ.ToString("yyyyMMdd") + ".txt"))
                            File.Delete(CreateFilePath() + "\\ErrorLog_" + dtZ.ToString("yyyyMMdd") + ".txt");

                        File.Move(CreateFilePath() + "\\ErrorLog.txt", CreateFilePath() + "\\ErrorLog_" + dtZ.ToString("yyyyMMdd") + ".txt");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

    public static DateTime CYParse(string datetime)
    {
        string[] spDateTime = datetime.Split(' ');
        string[] spDMY = spDateTime[0].Split('/');

        return new DateTime(int.Parse(spDMY[2]), int.Parse(spDMY[1]), int.Parse(spDMY[0]));
    }
}
