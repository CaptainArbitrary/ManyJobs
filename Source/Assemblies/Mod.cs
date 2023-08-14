using UnityEngine;
using Verse;

namespace ManyJobs
{
    public class Mod : Verse.Mod
    {
        readonly ModSettings settings;
        readonly string settingsCategory;

        public Mod(ModContentPack content) : base(content)
        {
            settings = GetSettings<ModSettings>();
            settingsCategory = content.Name;
        }

        public override string SettingsCategory()
        {
            return settingsCategory;
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            settings.DoSettingsWindowContents(inRect);
            base.DoSettingsWindowContents(inRect);
        }

        public override void WriteSettings()
        {
            settings.WriteSettings();
            base.WriteSettings();
        }
    }
}
