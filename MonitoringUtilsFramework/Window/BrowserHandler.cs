using System.Diagnostics;
using UIAutomationClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace MonitoringUtils.Window
{
    public static class BrowserHandler
    {
        public static async Task<string> GetActiveTabURL(WindowInfo windowInfo, bool getOnlyURLDomain = true)
        {
            string tabName = null;

            CUIAutomation cUIAutomation = new CUIAutomation();

            using (Process process = Process.GetProcessById(windowInfo.ProcessId))
            {
                try
                {
                    IUIAutomationElement elm = cUIAutomation.ElementFromHandle(process.MainWindowHandle);

                    IUIAutomationCondition Cond;
                    Cond = cUIAutomation.CreatePropertyCondition(30003, 50004); // Without the condition, all hyperlink-related components are returned.
                    //Cond = cUIAutomation.CreateTrueCondition();

                    IUIAutomationElementArray elm2 = null;
                    elm2 = await Task.Run(() => elm.FindAll(TreeScope.TreeScope_Descendants, Cond));

                    if (elm2 == null) return null;

                    for (int i = 0; i < elm2.Length; i++)
                    {
                        IUIAutomationElement elm3 = elm2.GetElement(i);
                        IUIAutomationValuePattern val = (IUIAutomationValuePattern)elm3.GetCurrentPattern(10002);

                        // Skip invalid values
                        if (val == null || val.CurrentValue == "") continue;

                        tabName = val.CurrentValue;

                        // Check if it's an URL
                        if (!IsAnURL(tabName)) continue;

                        // Format if necessary
                        if (getOnlyURLDomain) tabName = GetURLDomainRegex(tabName);

                        return tabName;
                    }
                }
                catch { }
            }

            return null;
        }

        public static string GetURLDomainRegex(string baseURL)
        {
            string finalURL;

            // Remove https:// and www.
            finalURL = Regex.Replace(baseURL, "(?:https?:\\/\\/|www\\.)", String.Empty);

            // Get url up to the first /
            Match match = Regex.Match(finalURL, "[^\\/]+");
            if (match.Success) finalURL = match.Value;

            return finalURL;
        }

        public static string GetURLDomain(string baseURL)
        {
            string finalURL;

            finalURL = baseURL.Replace("https://", String.Empty);
            finalURL = finalURL.Replace("http://", String.Empty);
            finalURL = finalURL.Replace("www.", String.Empty);

            if (finalURL.Contains('/')) finalURL = finalURL.Substring(0, finalURL.IndexOf('/'));

            return finalURL;
        }

        public static bool IsAnURL(string url)
        {
            // Check whitespaces
            Match match1 = Regex.Match(url, "[\\s]");
            if (match1.Success) return false;


            Match match2 = Regex.Match(url, "[a-zA-Z0-9\\-]+\\.[a-zA-Z0-9\\-]+");

            return match2.Success;
        }
    }
}
