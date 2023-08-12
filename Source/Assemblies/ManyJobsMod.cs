using System.Globalization;
using System.Reflection;
using UnityEngine;
using Verse;

namespace ManyJobs
{
    public class ManyJobsMod : Verse.Mod
    {
        ManyJobsModSettings settings;
        FieldInfo[] settingsFields;
        TextInfo textInfo;

        float listingRectCurrentHeight;
        Vector2 scrollPositionVector;

        public ManyJobsMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<ManyJobsModSettings>();
            settingsFields = settings.GetType().GetFields();
            textInfo = new CultureInfo("en-US", false).TextInfo;
        }

        public override string SettingsCategory()
        {
            return "Many Jobs";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Rect scrollViewRect = new Rect(inRect.x, inRect.y, inRect.width - 24f, listingRectCurrentHeight);
            Widgets.BeginScrollView(inRect, ref scrollPositionVector, scrollViewRect);
            Listing_Standard listing = new Listing_Standard();
            Rect listingRect = new Rect(scrollViewRect.x, scrollViewRect.y, scrollViewRect.width, 99999f);
            listing.Begin(listingRect);

            Color savedColor = GUI.color;
            GUI.color = ColorLibrary.RedReadable;
            Text.Anchor = TextAnchor.MiddleCenter;
            listing.Label("You will need to restart the game for any changes to take effect.");
            GenUI.ResetLabelAlign();
            GUI.color = savedColor;

            listing.Gap();

            Rect buttonsRect = listing.GetRect(30f);
            Listing_Standard buttonsListing = new Listing_Standard();

            buttonsListing.Begin(buttonsRect.LeftHalf());
            if (buttonsListing.ButtonText("All On"))
            {
                foreach (FieldInfo field in settingsFields)
                {
                    field.SetValue(settings, true);
                }
            }
            buttonsListing.End();

            buttonsListing.Begin(buttonsRect.RightHalf());
            if (buttonsListing.ButtonText("All Off"))
            {
                foreach (FieldInfo field in settingsFields)
                {
                    field.SetValue(settings, false);
                }
            }
            buttonsListing.End();

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_Rescuing.labelShort), ref settings.MJobs_Rescuing);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_Rescuing.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_Operating.labelShort), ref settings.MJobs_Operating);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_Operating.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_Caring.labelShort), ref settings.MJobs_Caring);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_Caring.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_PriorityHauling.labelShort), ref settings.MJobs_PriorityHauling);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_PriorityHauling.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_Undertaking.labelShort), ref settings.MJobs_Undertaking);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_Undertaking.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_Recruiting.labelShort), ref settings.MJobs_Recruiting);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_Recruiting.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_AnimalTraining.labelShort), ref settings.MJobs_AnimalTraining);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_AnimalTraining.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_AnimalTaming.labelShort), ref settings.MJobs_AnimalTaming);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_AnimalTaming.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_Butchering.labelShort), ref settings.MJobs_Butchering);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_Butchering.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_Brewing.labelShort), ref settings.MJobs_Brewing);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_Brewing.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_Maintaining.labelShort), ref settings.MJobs_Maintaining);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_Maintaining.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_Deconstructing.labelShort), ref settings.MJobs_Deconstructing);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_Deconstructing.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_Harvesting.labelShort), ref settings.MJobs_Harvesting);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_Harvesting.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_Drilling.labelShort), ref settings.MJobs_Drilling);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_Drilling.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_Fabricating.labelShort), ref settings.MJobs_Fabricating);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_Fabricating.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_Synthesizing.labelShort), ref settings.MJobs_Synthesizing);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_Synthesizing.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_Refining.labelShort), ref settings.MJobs_Refining);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_Refining.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_Smelting.labelShort), ref settings.MJobs_Smelting);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_Smelting.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_Stonecutting.labelShort), ref settings.MJobs_Stonecutting);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_Stonecutting.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_Delivering.labelShort), ref settings.MJobs_Delivering);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_Delivering.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_Loading.labelShort), ref settings.MJobs_Loading);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_Loading.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.CheckboxLabeled(textInfo.ToTitleCase(WorkTypeDefOf.MJobs_Scanning.labelShort), ref settings.MJobs_Scanning);
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            listing.Label(WorkTypeDefOf.MJobs_Scanning.description);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            listingRectCurrentHeight = listing.CurHeight;
            listing.End();
            Widgets.EndScrollView();
            base.DoSettingsWindowContents(inRect);
        }
    }
}
