/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 14/January/2015
 * Date Moddified :- 18/January/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using System.Windows.Forms;

namespace VaultTech
{
    public static class ExceptionHandler
    {
        static SystemInformation SystemInfo = new SystemInformation();
        static string ErrorMsg, StackTraceMsg, HardwareData, CrashLog;

        static string GetPost(string Url, params string[] postdata)
        {
            string result = string.Empty;
            string data = string.Empty;

            System.Text.ASCIIEncoding ascii = new ASCIIEncoding();

            if (postdata.Length % 2 != 0)
            {
                MessageBox.Show("Parameters must be even , \"user\" , \"value\" , ... etc", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }

            for (int i = 0; i < postdata.Length; i += 2)
                data += string.Format("&{0}={1}", postdata[i], postdata[i + 1]);

            data = data.Remove(0, 1);

            byte[] bytesarr = ascii.GetBytes(data);
            try
            {
                WebRequest request = WebRequest.Create(Url);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytesarr.Length;

                Stream streamwriter = request.GetRequestStream();
                streamwriter.Write(bytesarr, 0, bytesarr.Length);
                streamwriter.Close();

                WebResponse response = request.GetResponse();
                streamwriter = response.GetResponseStream();

                StreamReader streamread = new StreamReader(streamwriter);
                result = streamread.ReadToEnd();
                streamread.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }

        static void PrepareReport(Exception e)
        {
            ErrorMsg = "----------------------------------------------------------------------------------------------------------------------" + "\n" +
                        "Unhandled Exception " + "\n" +
                        "----------------------------------------------------------------------------------------------------------------------" + "\n" +
                        e.Message + "\n" +
                        "----------------------------------------------------------------------------------------------------------------------" + "\n\n";

            StackTraceMsg = "----------------------------------------------------------------------------------------------------------------------" + "\n" +
                            "Call Stack " + "\n" +
                            "----------------------------------------------------------------------------------------------------------------------" + "\n" +
                            e.StackTrace + "\n" +
                            "----------------------------------------------------------------------------------------------------------------------" + "\n\n";


            HardwareData += "----------------------------------------------------------------------------------------------------------------------" + "\n";
            HardwareData += "CPU" + "\n";
            HardwareData += "----------------------------------------------------------------------------------------------------------------------";
            HardwareData += SystemInfo.GetCPU;

            HardwareData += "\n\n" + "----------------------------------------------------------------------------------------------------------------------" + "\n";
            HardwareData += "RAM" + "\n";
            HardwareData += "----------------------------------------------------------------------------------------------------------------------";
            HardwareData += SystemInfo.GetRAM;

            HardwareData += "\n\n" + "----------------------------------------------------------------------------------------------------------------------" + "\n";
            HardwareData += "GPU" + "\n";
            HardwareData += "----------------------------------------------------------------------------------------------------------------------";
            HardwareData += SystemInfo.GetGPU;

            HardwareData += "\n\n" + "----------------------------------------------------------------------------------------------------------------------" + "\n";
            HardwareData += "Sound Device" + "\n";
            HardwareData += "----------------------------------------------------------------------------------------------------------------------";
            HardwareData += SystemInfo.GetSoundDevice;

            HardwareData += "\n\n" + "----------------------------------------------------------------------------------------------------------------------" + "\n";
            HardwareData += "Get Network Adapter" + "\n";
            HardwareData += "----------------------------------------------------------------------------------------------------------------------";
            HardwareData += SystemInfo.GetNetworkAdapter;

            HardwareData += "\n\n" + "----------------------------------------------------------------------------------------------------------------------" + "\n";
            HardwareData += "The end of the report" + "\n";
            HardwareData += "----------------------------------------------------------------------------------------------------------------------";

            CrashLog = ErrorMsg + StackTraceMsg + HardwareData;

            CrashLog = CrashLog.Replace("&", "and");
        }

        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;

            PrepareReport(e);
            string ResultData = GetPost("http://vault16software.com/GameFiles/CrashLogsSubmiter.php", "Username", "Me", "CrashLog", CrashLog);

            MessageBox.Show("A problem caused the program to stop working correctly.\n\n A report will be sent to the developer. Please make sure your internet connection is on. Sorry for the inconvenience.", "Error Reporting", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void HandledAllExceptions()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
        }
    }
}