namespace ibDiary_app
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            try
            {
                return new Window(new MainPage()) { Title = "ibDiary-app" };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Window creation error: {ex}");
                throw;
            }
        }

        protected override async void OnStart()
        {
            base.OnStart();
            await MauiProgram.RequestNotificationPermission();
            MauiProgram.SetupMedicineReminders();
            MauiProgram.SetupStatsService();
        }
    }
}
