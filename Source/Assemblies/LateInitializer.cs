using Verse;

namespace ManyJobs
{
    [StaticConstructorOnStartup]
    public static class LateInitializer
    {
        static LateInitializer()
        {
            LoadedModManager.GetMod<ManyJobs>().OnLateInitialize();
        }
    }
}
