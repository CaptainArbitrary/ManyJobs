using System.Collections.Generic;
using Verse;

namespace ManyJobs
{
    public class ManyJobsModSettings : Verse.ModSettings
    {
        public bool MJobs_Rescuing = true;
        public bool MJobs_Operating = true;
        public bool MJobs_Caring = true;
        public bool MJobs_PriorityHauling = true;
        public bool MJobs_PriorityCleaning = true;
        public bool MJobs_Undertaking = true;
        public bool MJobs_Recruiting = true;
        public bool MJobs_AnimalTraining = true;
        public bool MJobs_AnimalTaming = true;
        public bool MJobs_Butchering = true;
        public bool MJobs_Brewing = true;
        public bool MJobs_Maintaining = true;
        public bool MJobs_Deconstructing = true;
        public bool MJobs_Harvesting = true;
        public bool MJobs_Drilling = true;
        public bool MJobs_Fabricating = true;
        public bool MJobs_Synthesizing = true;
        public bool MJobs_Refining = true;
        public bool MJobs_Smelting = true;
        public bool MJobs_Stonecutting = true;
        public bool MJobs_Delivering = true;
        public bool MJobs_Loading = true;
        public bool MJobs_Scanning = true;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref MJobs_Rescuing, "MJobs_Rescuing", true);
            Scribe_Values.Look(ref MJobs_Operating, "MJobs_Operating", true);
            Scribe_Values.Look(ref MJobs_Caring, "MJobs_Caring", true);
            Scribe_Values.Look(ref MJobs_PriorityHauling, "MJobs_PriorityHauling", true);
            Scribe_Values.Look(ref MJobs_PriorityCleaning, "MJobs_PriorityCleaning", true);
            Scribe_Values.Look(ref MJobs_Undertaking, "MJobs_Undertaking", true);
            Scribe_Values.Look(ref MJobs_Recruiting, "MJobs_Recruiting", true);
            Scribe_Values.Look(ref MJobs_AnimalTraining, "MJobs_AnimalTraining", true);
            Scribe_Values.Look(ref MJobs_AnimalTaming, "MJobs_AnimalTaming", true);
            Scribe_Values.Look(ref MJobs_Butchering, "MJobs_Butchering", true);
            Scribe_Values.Look(ref MJobs_Brewing, "MJobs_Brewing", true);
            Scribe_Values.Look(ref MJobs_Maintaining, "MJobs_Maintaining", true);
            Scribe_Values.Look(ref MJobs_Deconstructing, "MJobs_Deconstructing", true);
            Scribe_Values.Look(ref MJobs_Harvesting, "MJobs_Harvesting", true);
            Scribe_Values.Look(ref MJobs_Drilling, "MJobs_Drilling", true);
            Scribe_Values.Look(ref MJobs_Fabricating, "MJobs_Fabricating", true);
            Scribe_Values.Look(ref MJobs_Synthesizing, "MJobs_Synthesizing", true);
            Scribe_Values.Look(ref MJobs_Refining, "MJobs_Refining", true);
            Scribe_Values.Look(ref MJobs_Smelting, "MJobs_Smelting", true);
            Scribe_Values.Look(ref MJobs_Stonecutting, "MJobs_Stonecutting", true);
            Scribe_Values.Look(ref MJobs_Delivering, "MJobs_Delivering", true);
            Scribe_Values.Look(ref MJobs_Loading, "MJobs_Loading", true);
            Scribe_Values.Look(ref MJobs_Scanning, "MJobs_Scanning", true);

            base.ExposeData();
        }
    }
}
