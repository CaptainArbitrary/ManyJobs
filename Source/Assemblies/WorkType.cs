using Mono.Unix.Native;
using Verse;

namespace ManyJobs
{
    public class WorkType
    {
        public string Name;
        public bool Enabled;
        
        WorkTypeDef _def;

        public WorkType(string name) {
            Name = name;
            Enabled = true;
            _def = null;
        }

        public WorkTypeDef Def
        {
            get
            {
                if (_def == null)
                {
                    _def = DefDatabase<WorkTypeDef>.GetNamedSilentFail(Name);
                }
                return _def;
            }
        }
    }
}
