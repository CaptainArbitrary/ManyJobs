using System.Globalization;
using Verse;

namespace ManyJobs
{
    public class WorkType
    {
        public readonly string Name;
        public bool IsEnabled;
        public bool IsEnabledInConfigFile;

        private WorkTypeDef _workTypeDef;

        public WorkType(string name, bool enabled) {
            Name = name;
            IsEnabled = enabled;
            IsEnabledInConfigFile = enabled;
            _workTypeDef = null;
        }

        public bool IsDirty => IsEnabled != IsEnabledInConfigFile;

        public WorkTypeDef Def => _workTypeDef ?? (_workTypeDef = DefDatabase<WorkTypeDef>.GetNamedSilentFail(Name));

        public string LabelShort => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Def?.labelShort ?? Name);

        public string Description => Def?.description ?? string.Empty;

        public override string ToString() => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Def?.labelShort ?? Name);
    }
}
