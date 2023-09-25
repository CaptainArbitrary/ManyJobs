using System.Globalization;
using Verse;

namespace ManyJobs
{
    public class WorkType
    {
        public string Name { get; }
        public bool IsEnabledByDefault { get; set; }
        public bool IsEnabled;
        public bool? WasEnabledAtStartup { get; set; }
        public WorkTypeDef Def => _workTypeDef ?? (_workTypeDef = DefDatabase<WorkTypeDef>.GetNamedSilentFail(Name));

        private WorkTypeDef _workTypeDef;

        public WorkType(string name, bool isEnabledByDefault) {
            Name = name;
            IsEnabledByDefault = isEnabledByDefault;
            IsEnabled = false;
            WasEnabledAtStartup = null;
            _workTypeDef = null;
        }

        public bool IsDirty => IsEnabled != WasEnabledAtStartup;

        public string LabelShort => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Def?.labelShort ?? Name);

        public string Description => Def?.description ?? string.Empty;

        public override string ToString() => LabelShort;
    }
}
