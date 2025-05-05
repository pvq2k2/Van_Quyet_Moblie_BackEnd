using VQM.Debugging;

namespace VQM;

public class VQMConsts
{
    public const string LocalizationSourceName = "VQM";

    public const string ConnectionStringName = "Default";

    public const bool MultiTenancyEnabled = true;


    /// <summary>
    /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
    /// </summary>
    public static readonly string DefaultPassPhrase =
        DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "1bebc13a0ecb4ba09b0a84aee1de3e07";
}
