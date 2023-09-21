using UnityEngine;
using Verse;

namespace ManyJobs
{
    public class ManyJobs : Mod
    {
        private readonly ModSettings _settings;

        public ManyJobs(ModContentPack content) : base(content)
        {
            _settings = GetSettings<ModSettings>();
        }

        public void OnLateInitialize()
        {
            _settings.OnLateInitialize();
        }

        public override string SettingsCategory()
        {
            return Content.Name;
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            _settings.DoSettingsWindowContents(inRect);
        }

        public override void WriteSettings()
        {
            _settings.WriteSettings();
            base.WriteSettings();
        }
    }
}
