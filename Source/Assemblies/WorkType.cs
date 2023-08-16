using Verse;

namespace ManyJobs
{
    public class WorkType
    {
        public string Name;
        public bool IsEnabled;
        public bool IsEnabledInConfigFile;

        WorkTypeDef workTypeDef;

        public WorkType(string name) {
            Name = name;
            IsEnabled = true;
            IsEnabledInConfigFile = true;
            workTypeDef = null;
        }

        public bool IsDirty { get
            {
                return IsEnabled != IsEnabledInConfigFile;
            }
        }

        public WorkTypeDef Def
        {
            get
            {
                if (workTypeDef == null)
                {
                    workTypeDef = DefDatabase<WorkTypeDef>.GetNamedSilentFail(Name);
                }
                return workTypeDef;
            }
        }
    }
}
