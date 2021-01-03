using System.Diagnostics;

namespace ProjectsTM.Service
{
    public static class LaunchHelpService
    {
        public static void Show()
        {
            Process.Start(@".\Help\help.html");
        }
    }
}
