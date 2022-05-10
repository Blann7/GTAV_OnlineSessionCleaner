using System.Diagnostics;

namespace CleanSession
{
    public partial class Main : Form
    {
        public static Process? GTAV_PROCESS;
        public static int delay = new Random().Next(8000, 10000);
        public static bool IsReady = false;
        public static ProgressBar pBar = null!;
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            pBar = progressBar;
        }

        private static void FindGtaVProcess()
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName == "GTA5") GTAV_PROCESS = process;
            }
            if (GTAV_PROCESS is null) ErrorMessageBox("GTAV process not found!\nStart the game.");
            else IsReady = true;
        }
        public static void ErrorMessageBox(string message)
        {
            MessageBox.Show(
                message,
                "Error", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Warning, 
                MessageBoxDefaultButton.Button1);
        }
        public static void SuccessfulMessageBox(string message)
        {
            MessageBox.Show(
                message,
                "Successfully",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }
        private async void button_Click(object sender, EventArgs e)
        {
            if(IsReady)
            {
                await ClearSession();
            }
            else
            {
                FindGtaVProcess();
                if(IsReady) await ClearSession();
            }
        }

        public static async Task ClearSession()
        {
            GTAV_PROCESS?.Suspend();

            await FillProgressBar();
            
            GTAV_PROCESS?.Resume();

            SuccessfulMessageBox("Session was cleared!");
            pBar.Value = 0;
        }
        public static async Task FillProgressBar()
        {
            int randomTimes = new Random().Next(5, 15);
            int delayPerTime = delay / randomTimes;
            for (int i = 0; i < randomTimes; i++)
            {
                if(pBar.Value + (100 / randomTimes) > 100) pBar.Value = 100;
                else if(i == randomTimes - 1) pBar.Value = 100;
                else pBar.Value += 100 / randomTimes;
                await Task.Delay(delayPerTime);
            }
        }
    }
}