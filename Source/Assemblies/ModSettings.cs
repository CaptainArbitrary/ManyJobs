﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        Listing_Standard workTypesListing = new Listing_Standard();
        Vector2 scrollPositionVector = Vector2.zero;

        readonly float buttonWidth = 100f;
        readonly float buttonHeight = GenUI.ListSpacing;

        public void DoSettingsWindowContents(Rect inRect)
        {
            Rect restartGameMessageRect = new Rect(inRect.x, inRect.y, 644f, buttonHeight);
            Color savedColor = GUI.color;
            GUI.color = ColorLibrary.RedReadable;
            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.Label(restartGameMessageRect, "You will need to restart the game for any changes to take effect.");
            Text.Anchor = TextAnchor.UpperLeft;
            GUI.color = savedColor;

            Rect allOnButtonRect = new Rect(inRect.width - buttonWidth - GenUI.GapSmall - buttonWidth, inRect.y, buttonWidth, buttonHeight);
            bool allOnButton = Widgets.ButtonText(allOnButtonRect, "All On");

            Rect allOffButtonRect = new Rect(inRect.width - buttonWidth, inRect.y, buttonWidth, buttonHeight);
            bool allOffButton = Widgets.ButtonText(allOffButtonRect, "All Off");

            Rect outerRect = new Rect(inRect.x, inRect.y + buttonHeight + GenUI.GapSmall, inRect.width, inRect.height - buttonHeight - GenUI.GapSmall);
            Rect innerRect = new Rect(inRect.x, inRect.y, inRect.width - (GenUI.ScrollBarWidth + GenUI.GapSmall), workTypesListing.CurHeight);
            Rect listingRect = new Rect(innerRect.x, innerRect.y, innerRect.width, 99999f);

            Widgets.BeginScrollView(outerRect, ref scrollPositionVector, innerRect, true);
            workTypesListing.Begin(listingRect);

            foreach (WorkType workType in WorkTypes)
            {
                workTypesListing.CheckboxLabeled(textInfo.ToTitleCase(workType.Def.labelShort), ref workType.Enabled);
                Text.Font = GameFont.Tiny;
                GUI.color = Color.gray;
                workTypesListing.Label(workType.Def.description);
                GUI.color = Color.white;
                Text.Font = GameFont.Small;
                if (workType != WorkTypes.Last())
                {
                    workTypesListing.Gap();
                }
            }

            workTypesListing.End();
            Widgets.EndScrollView();

            if (allOnButton)
            {
                foreach (WorkType workType in WorkTypes)
                {
                    workType.Enabled = true;
                }
            }

            if (allOffButton)
            {
                foreach (WorkType workType in WorkTypes)
                {
                    workType.Enabled = false;
                }
            }
        }
    }
}