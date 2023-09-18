using System;
using RimWorld;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace ManyJobs
{
    public class ModSettings : Verse.ModSettings
    {
        public bool MJobs_Rescuing;
        public bool MJobs_Operating;
        public bool MJobs_Caring;
        public bool MJobs_Teaching;
        public bool MJobs_PriorityHauling;
        public bool MJobs_PriorityCleaning;
        public bool MJobs_Undertaking;
        public bool MJobs_Counseling;
        public bool MJobs_Converting;
        public bool MJobs_Recruiting;
        public bool MJobs_AnimalTraining;
        public bool MJobs_AnimalTaming;
        public bool MJobs_Butchering;
        public bool MJobs_Brewing;
        public bool MJobs_Maintaining;
        public bool MJobs_Deconstructing;
        public bool MJobs_Harvesting;
        public bool MJobs_Drilling;
        public bool MJobs_Mechanitor;
        public bool MJobs_Fabricating;
        public bool MJobs_Synthesizing;
        public bool MJobs_Refining;
        public bool MJobs_Smelting;
        public bool MJobs_Stonecutting;
        public bool MJobs_Delivering;
        public bool MJobs_Loading;
        public bool MJobs_Scanning;

        private readonly List<WorkType> WorkTypes = new List<WorkType>();
        private readonly TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        private const string restartDialogMessage = "Changing the list of enabled work types requires a restart.\n\nRestart now? Unsaved progress will be lost.";

        private Listing_Standard workTypesListing = new Listing_Standard();
        private Vector2 scrollPositionVector = Vector2.zero;

        private const float buttonWidth = 100f;
        private const float buttonHeight = GenUI.ListSpacing;

        private float maxWorkTypeNameWidth;

        private QuickSearchWidget _quickSearchWidget = new QuickSearchWidget();

        private const string AllOnButtonLabel = "Enable All";
        private const string AllOffButtonLabel = "Disable All";
        private const string SelectedOnButtonLabel = "Enable Shown";
        private const string SelectedOffButtonLabel = "Disable Shown";
        private float _maximumButtonLabelSize;

        public ModSettings()
        {
            foreach (FieldInfo field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                WorkTypes.Add(new WorkType(field.Name));
                field.SetValue(this, true);
            }
        }

        public void OnLateInitialize()
        {
            foreach (WorkType workType in WorkTypes)
            {
                string n = textInfo.ToTitleCase(workType.Def?.labelShort ?? workType.Name);
                Vector2 nSize = Text.CalcSize(n);
                maxWorkTypeNameWidth = Mathf.Max(maxWorkTypeNameWidth, nSize.x);
            }

            float allOnButtonLabelWidth = GenUI.Gap + Text.CalcSize(AllOnButtonLabel).x + GenUI.Gap;
            float allOffButtonLabelWidth = GenUI.Gap + Text.CalcSize(AllOffButtonLabel).x + GenUI.Gap;
            float selectedOnButtonLabelWidth = GenUI.Gap + Text.CalcSize(SelectedOnButtonLabel).x + GenUI.Gap;
            float selectedOffButtonLabelWidth = GenUI.Gap + Text.CalcSize(SelectedOffButtonLabel).x + GenUI.Gap;

            float[] sizes =
            {
                allOnButtonLabelWidth, allOffButtonLabelWidth, selectedOnButtonLabelWidth, selectedOffButtonLabelWidth
            };
            _maximumButtonLabelSize = sizes.Max();
        }

        internal void WriteSettings()
        {
            bool isDirty = false;
            foreach (WorkType workType in WorkTypes)
            {
                if (workType.IsDirty)
                {
                    isDirty = true;
                    break;
                }
            }

            if (isDirty)
            {
                Find.WindowStack.Add(new Dialog_MessageBox(restartDialogMessage, "Yes".Translate(), delegate
                {
                    GenCommandLine.Restart();
                }, "No".Translate(), null, null, true, null, null, WindowLayer.Dialog));
            }
        }

        public override void ExposeData()
        {
            foreach (WorkType workType in WorkTypes)
            {
                Scribe_Values.Look(ref workType.IsEnabled, workType.Name, true);
                workType.IsEnabledInConfigFile = workType.IsEnabled;
                this.GetType().GetField(workType.Name).SetValue(this, workType.IsEnabled);
            }

            base.ExposeData();
        }

        private string filter;
        private bool filtered;

        public void DoSettingsWindowContents(Rect inRect)
        {
            string offButtonLabel = AllOffButtonLabel;
            string onButtonLabel = AllOnButtonLabel;

            List<WorkType> filteredWorkTypes;
            if (filtered)
            {
                filteredWorkTypes = WorkTypes.Where(wt => wt.Def.labelShort.Contains(filter) || wt.Def.description.Contains(filter)).ToList();
                offButtonLabel = SelectedOffButtonLabel;
                onButtonLabel = SelectedOnButtonLabel;
            }
            else
            {
                filteredWorkTypes = WorkTypes;
            }

            Rect offButtonRect = new Rect
            {
                width = _maximumButtonLabelSize,
                height = buttonHeight,
            };
            offButtonRect.x = inRect.width - offButtonRect.width;
            offButtonRect.y = inRect.y;
            bool offButton = Widgets.ButtonText(offButtonRect, offButtonLabel);

            Rect onButtonRect = new Rect
            {
                width = _maximumButtonLabelSize,
                height = buttonHeight,
            };
            onButtonRect.x = inRect.width - onButtonRect.width - GenUI.GapSmall - onButtonRect.width;
            onButtonRect.y = inRect.y;
            bool onButton = Widgets.ButtonText(onButtonRect, onButtonLabel);

            Rect quickSearchRect = new Rect(inRect.x, inRect.y, inRect.width - _maximumButtonLabelSize - GenUI.GapSmall - _maximumButtonLabelSize - GenUI.GapSmall, buttonHeight);
            _quickSearchWidget.OnGUI(quickSearchRect);
            filter = _quickSearchWidget.filter.Text.ToLower();
            filtered = !String.IsNullOrEmpty(filter);

            Rect outerRect = new Rect(inRect.x, inRect.y + quickSearchRect.height + GenUI.GapSmall, inRect.width, inRect.height - quickSearchRect.height - GenUI.GapSmall);
            Rect innerRect = new Rect(inRect.x, inRect.y, inRect.width - (GenUI.ScrollBarWidth + GenUI.GapSmall), workTypesListing.CurHeight);
            Rect listingRect = new Rect(innerRect.x, innerRect.y, innerRect.width, 99999f);

            Widgets.BeginScrollView(outerRect, ref scrollPositionVector, innerRect, true);
            workTypesListing.Begin(listingRect);

            TextAnchor originalAnchor = Text.Anchor;
            Text.Anchor = TextAnchor.MiddleLeft;

            foreach (WorkType workType in filteredWorkTypes)
            {
                Rect rect = workTypesListing.GetRect(GenUI.GapTiny + Text.LineHeight + GenUI.GapTiny);

                Rect nameRect = new Rect(rect) { width = maxWorkTypeNameWidth + GenUI.Gap };
                Rect descriptionRect = new Rect(rect) { x = nameRect.xMax, width = rect.width - nameRect.width - GenUI.Gap * 2 };
                Rect clickableRect = new Rect(rect) { width = rect.width - Widgets.CheckboxSize };

                Widgets.Label(nameRect, textInfo.ToTitleCase(workType.Def?.labelShort ?? workType.Name));

                GUI.color = Color.gray;
                Widgets.Label(descriptionRect, workType.Def?.description ?? string.Empty);
                GUI.color = Color.white;

                if (Widgets.ButtonInvisible(clickableRect))
                {
                    workType.IsEnabled = !workType.IsEnabled;
                    if (workType.IsEnabled)
                    {
                        SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera();
                    }
                    else
                    {
                        SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera();
                    }
                }

                Widgets.Checkbox(new Vector2(rect.xMax - Widgets.CheckboxSize, rect.center.y - Widgets.CheckboxSize / 2f), ref workType.IsEnabled);

                Widgets.DrawHighlightIfMouseover(rect);

                MouseoverSounds.DoRegion(rect);
            }

            Text.Anchor = originalAnchor;

            workTypesListing.End();
            Widgets.EndScrollView();

            Rect versionInfoRect = new Rect(inRect.x, 0f, inRect.width, 40f);
            Text.Anchor = TextAnchor.MiddleRight;
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;
            Widgets.Label(versionInfoRect, "Mod Version " + Mod.Content.ModMetaData.ModVersion);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;

            if (onButton)
            {
                SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera();
                foreach (WorkType workType in filteredWorkTypes)
                {
                    workType.IsEnabled = true;
                }
            }

            if (offButton)
            {
                SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera();
                foreach (WorkType workType in filteredWorkTypes)
                {
                    workType.IsEnabled = false;
                }
            }
        }
    }
}
