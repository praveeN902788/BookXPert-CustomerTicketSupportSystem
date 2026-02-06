using CustomerSupport.Desktop.Forms;
using CustomerSupport.Desktop.Services;

namespace CustomerSupport.Desktop
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            
            var apiService = new ApiService("https://localhost:57230/api");
            
            Application.Run(new LoginForm(apiService));
        }
    }
}