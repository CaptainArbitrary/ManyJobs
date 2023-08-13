using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using Verse;

namespace ManyJobs
{
    public class ModSettings : Verse.ModSettings
    {
        public bool MJobs_Rescuing;
        public bool MJobs_Operating;
        public bool MJobs_Caring;
        public bool MJobs_PriorityHauling;
        public bool MJobs_PriorityCleaning;
        public bool MJobs_Undertaking;
        public bool MJobs_Recruiting;
        public bool MJobs_AnimalTraining;
        public bool MJobs_AnimalTaming;
        public bool MJobs_Butchering;
        public bool MJobs_Brewing;
        public bool MJobs_Maintaining;
        public bool MJobs_Deconstructing;
        public bool MJobs_Harvesting;
        public bool MJobs_Drilling;
        public bool MJobs_Fabricating;
        public bool MJobs_Synthesizing;
        public bool MJobs_Refining;
        public bool MJobs_Smelting;
        public bool MJobs_Stonecutting;
        public bool MJobs_Delivering;
        public bool MJobs_Loading;
        public bool MJobs_Scanning;

        List<WorkType> WorkTypes = new List<WorkType>();

        readonly TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        public ModSettings()
        {
            DefDatabase<WorkTypeDef>.ResolveAllReferences();
            foreach (FieldInfo field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                WorkTypes.Add(new WorkType(field.Name));
                field.SetValue(this, true);
            }
        }

        public override void ExposeData()
        {
            foreach (WorkType workType in WorkTypes)
            {
                Scribe_Values.Look(ref workType.Enabled, workType.Name, true);
                this.GetType().GetField(workType.Name).SetValue(this, workType.Enabled);
            }

            base.ExposeData();
        }

        float listingRectCurrentHeight = 0f;
        Vector2 scrollPositionVector = Vector2.zero;

        public void DoSettingsWindowContents(Rect inRect)
        {
            Rect scrollViewRect = new Rect(inRect.x, inRect.y, inRect.width - (GenUI.ScrollBarWidth + GenUI.GapSmall), listingRectCurrentHeight);
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
                foreach (WorkType workType in WorkTypes)
                {
                    workType.Enabled = true;
                }
            }
            buttonsListing.End();

            buttonsListing.Begin(buttonsRect.RightHalf());
            if (buttonsListing.ButtonText("All Off"))
            {
                foreach (WorkType workType in WorkTypes)
                {
                    workType.Enabled = false;
                }
            }
            buttonsListing.End();

            listing.Gap();

            foreach (WorkType workType in WorkTypes)
            {
                listing.CheckboxLabeled(textInfo.ToTitleCase(workType.Def.labelShort), ref workType.Enabled);
                Text.Font = GameFont.Tiny;
                GUI.color = Color.gray;
                listing.Label(workType.Def.description);
                GUI.color = Color.white;
                Text.Font = GameFont.Small;
                listing.Gap();
            }

            listingRectCurrentHeight = listing.CurHeight;
            listing.End();
            Widgets.EndScrollView();
        }
    }
}