using System;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace ManyJobs
{
    public class ModSettings : Verse.ModSettings
    {
        public readonly List<WorkType> WorkTypes = new List<WorkType>()
        {
            new WorkType("MJobs_Rescuing", isEnabledByDefault: true),
            new WorkType("MJobs_Operating", isEnabledByDefault: true),
            new WorkType("MJobs_Caring", isEnabledByDefault: true),
            new WorkType("MJobs_Teaching", isEnabledByDefault: true),
            new WorkType("MJobs_PriorityHauling", isEnabledByDefault: true),
            new WorkType("MJobs_PriorityCleaning", isEnabledByDefault: true),
            new WorkType("MJobs_CleaningPollution", isEnabledByDefault: false),
            new WorkType("MJobs_Undertaking", isEnabledByDefault: true),
            new WorkType("MJobs_Counseling", isEnabledByDefault: true),
            new WorkType("MJobs_Converting", isEnabledByDefault: true),
            new WorkType("MJobs_Recruiting", isEnabledByDefault: true),
            new WorkType("MJobs_AnimalTraining", isEnabledByDefault: true),
            new WorkType("MJobs_AnimalTaming", isEnabledByDefault: true),
            new WorkType("MJobs_Butchering", isEnabledByDefault: true),
            new WorkType("MJobs_Brewing", isEnabledByDefault: true),
            new WorkType("MJobs_Maintaining", isEnabledByDefault: true),
            new WorkType("MJobs_Deconstructing", isEnabledByDefault: true),
            new WorkType("MJobs_Smoothing", isEnabledByDefault: false),
            new WorkType("MJobs_Painting", isEnabledByDefault: false),
            new WorkType("MJobs_Harvesting", isEnabledByDefault: true),
            new WorkType("MJobs_Drilling", isEnabledByDefault: true),
            new WorkType("MJobs_Pruning", isEnabledByDefault: false),
            new WorkType("MJobs_Mechanitor", isEnabledByDefault: true),
            new WorkType("MJobs_Fabricating", isEnabledByDefault: true),
            new WorkType("MJobs_Synthesizing", isEnabledByDefault: true),
            new WorkType("MJobs_Refining", isEnabledByDefault: true),
            new WorkType("MJobs_Smelting", isEnabledByDefault: true),
            new WorkType("MJobs_Stonecutting", isEnabledByDefault: true),
            new WorkType("MJobs_Delivering", isEnabledByDefault: true),
            new WorkType("MJobs_Loading", isEnabledByDefault: true),
            new WorkType("MJobs_Merging", isEnabledByDefault: false),
            new WorkType("MJobs_Scanning", isEnabledByDefault: true)
        };

        private const string RestartDialogMessage = "Changes to the list of enabled work types will not take effect until you restart the game.\n\nRestart now? Unsaved progress will be lost.";

        private readonly Listing_Standard _workTypesListing = new Listing_Standard();
        private Vector2 _scrollPositionVector = Vector2.zero;

        private float _maxWorkTypeNameWidth;

        private readonly QuickSearchWidget _quickSearchWidget = new QuickSearchWidget();

        private const string AllOnButtonLabel = "Enable All";
        private const string AllOffButtonLabel = "Disable All";
        private const string SelectedOnButtonLabel = "Enable Shown";
        private const string SelectedOffButtonLabel = "Disable Shown";
        private float _maximumButtonLabelSize;

        private string _filter;
        private bool _filtered;

        public ModSettings()
        {
        }

        public void OnLateInitialize()
        {
            foreach (WorkType wt in WorkTypes)
            {
                Vector2 nameSize = Text.CalcSize(wt.LabelShort);
                _maxWorkTypeNameWidth = Mathf.Max(_maxWorkTypeNameWidth, nameSize.x);
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
            foreach (WorkType wt in WorkTypes)
            {
                if (wt.IsDirty)
                {
                    isDirty = true;
                    break;
                }
            }

            if (isDirty)
            {
                Find.WindowStack.Add(new Dialog_MessageBox(RestartDialogMessage, "Yes".Translate(), GenCommandLine.Restart, "No".Translate(), null, null, true, null, null, WindowLayer.Dialog));
            }
        }

        public override void ExposeData()
        {
            foreach (WorkType wt in WorkTypes)
            {
                Scribe_Values.Look(ref wt.IsEnabled, wt.Name, wt.IsEnabledByDefault);
                if (wt.WasEnabledAtStartup == null) wt.WasEnabledAtStartup = wt.IsEnabled;
            }

            base.ExposeData();
        }

        public void DoSettingsWindowContents(Rect inRect)
        {
            string offButtonLabel = AllOffButtonLabel;
            string onButtonLabel = AllOnButtonLabel;

            List<WorkType> filteredWorkTypes;
            if (_filtered)
            {
                filteredWorkTypes = WorkTypes.Where(wt => wt.LabelShort.Contains(_filter) || wt.Description.Contains(_filter)).ToList();
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
                height = GenUI.ListSpacing,
            };
            offButtonRect.x = inRect.width - offButtonRect.width;
            offButtonRect.y = inRect.y;
            bool offButton = Widgets.ButtonText(offButtonRect, offButtonLabel);

            Rect onButtonRect = new Rect
            {
                width = _maximumButtonLabelSize,
                height = GenUI.ListSpacing,
            };
            onButtonRect.x = inRect.width - onButtonRect.width - GenUI.GapSmall - onButtonRect.width;
            onButtonRect.y = inRect.y;
            bool onButton = Widgets.ButtonText(onButtonRect, onButtonLabel);

            Rect quickSearchRect = new Rect(inRect.x, inRect.y, inRect.width - _maximumButtonLabelSize - GenUI.GapSmall - _maximumButtonLabelSize - GenUI.GapSmall, GenUI.ListSpacing);
            _quickSearchWidget.OnGUI(quickSearchRect);
            _filter = _quickSearchWidget.filter.Text.ToLower();
            _filtered = !String.IsNullOrEmpty(_filter);

            Rect outerRect = new Rect(inRect.x, inRect.y + quickSearchRect.height + GenUI.GapSmall, inRect.width, inRect.height - quickSearchRect.height - GenUI.GapSmall);
            Rect innerRect = new Rect(inRect.x, inRect.y, inRect.width - (GenUI.ScrollBarWidth + GenUI.GapSmall), _workTypesListing.CurHeight);
            Rect listingRect = new Rect(innerRect.x, innerRect.y, innerRect.width, 99999f);

            Widgets.BeginScrollView(outerRect, ref _scrollPositionVector, innerRect, true);
            _workTypesListing.Begin(listingRect);

            TextAnchor originalAnchor = Text.Anchor;
            Text.Anchor = TextAnchor.MiddleLeft;

            foreach (WorkType wt in filteredWorkTypes)
            {
                Rect rect = _workTypesListing.GetRect(GenUI.GapTiny + Text.LineHeight + GenUI.GapTiny);

                Rect nameRect = new Rect(rect) { width = _maxWorkTypeNameWidth + GenUI.Gap };
                Rect descriptionRect = new Rect(rect) { x = nameRect.xMax, width = rect.width - nameRect.width - GenUI.Gap * 2 };
                Rect clickableRect = new Rect(rect) { width = rect.width - Widgets.CheckboxSize };

                Widgets.Label(nameRect, wt.LabelShort);

                GUI.color = Color.gray;
                Widgets.Label(descriptionRect, wt.Description);
                GUI.color = Color.white;

                if (Widgets.ButtonInvisible(clickableRect))
                {
                    wt.IsEnabled = !wt.IsEnabled;
                    if (wt.IsEnabled)
                    {
                        SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera();
                    }
                    else
                    {
                        SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera();
                    }
                }

                Widgets.Checkbox(new Vector2(rect.xMax - Widgets.CheckboxSize, rect.center.y - Widgets.CheckboxSize / 2f), ref wt.IsEnabled);

                Widgets.DrawHighlightIfMouseover(rect);

                MouseoverSounds.DoRegion(rect);
            }

            Text.Anchor = originalAnchor;

            _workTypesListing.End();
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
                foreach (WorkType wt in filteredWorkTypes)
                {
                    wt.IsEnabled = true;
                }
            }

            if (offButton)
            {
                SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera();
                foreach (WorkType wt in filteredWorkTypes)
                {
                    wt.IsEnabled = false;
                }
            }
        }
    }
}
