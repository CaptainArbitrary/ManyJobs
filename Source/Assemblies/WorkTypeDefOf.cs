using RimWorld;
using Verse;

namespace ManyJobs
{
    [DefOf]
    public static class WorkTypeDefOf
    {
        public static WorkTypeDef MJobs_Rescuing;
        public static WorkTypeDef MJobs_Operating;
        public static WorkTypeDef MJobs_Caring;
        public static WorkTypeDef MJobs_PriorityHauling;
        public static WorkTypeDef MJobs_PriorityCleaning;
        public static WorkTypeDef MJobs_Undertaking;
        public static WorkTypeDef MJobs_Recruiting;
        public static WorkTypeDef MJobs_AnimalTraining;
        public static WorkTypeDef MJobs_AnimalTaming;
        public static WorkTypeDef MJobs_Butchering;
        public static WorkTypeDef MJobs_Brewing;
        public static WorkTypeDef MJobs_Maintaining;
        public static WorkTypeDef MJobs_Deconstructing;
        public static WorkTypeDef MJobs_Harvesting;
        public static WorkTypeDef MJobs_Drilling;
        public static WorkTypeDef MJobs_Fabricating;
        public static WorkTypeDef MJobs_Synthesizing;
        public static WorkTypeDef MJobs_Refining;
        public static WorkTypeDef MJobs_Smelting;
        public static WorkTypeDef MJobs_Stonecutting;
        public static WorkTypeDef MJobs_Delivering;
        public static WorkTypeDef MJobs_Loading;
        public static WorkTypeDef MJobs_Scanning;

        static WorkTypeDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(WorkTypeDefOf));
        }
    }
}
