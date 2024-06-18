using Serilog;

public static class AppSettingsHelper {
    private static IConfiguration _config;
        public static void SetConfig(IConfiguration configuration)
        {
            _config = configuration;
        }

    #region SERILOG
    public static void EnableLogger()
    {
        try
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File(_config["LogPath"] + "/log_.txt", rollOnFileSizeLimit: true, shared: true,
            fileSizeLimitBytes: 10000000, rollingInterval: RollingInterval.Day).CreateLogger();
            Log.Information("START PROCESS");
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
        } 
    }
    #endregion

    #region SQL DB CONNECTIONS
    public static string AbcDbConnection => _config["ConnectionStrings:DefaultConnection"];

    public static string EmailSenderMail => _config["EmailSenderCredentials:Mail"];
    public static string EmailSenderPw => _config["EmailSenderCredentials:Password"];
    public static int EmailSenderPort => Convert.ToInt32(_config["EmailSenderCredentials:Port"]);
    public static string EmailSenderSmtp => _config["EmailSenderCredentials:Smtp"];

    #endregion

}