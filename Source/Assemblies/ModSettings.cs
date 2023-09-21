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
        public bool MJobs_Rescuing = true;
        public bool MJobs_Operating = true;
        public bool MJobs_Caring = true;
        public bool MJobs_Teaching = true;
        public bool MJobs_PriorityHauling = true;
        public bool MJobs_PriorityCleaning = true;
        public bool MJobs_CleaningPollution = false;
        public bool MJobs_Undertaking = true;
        public bool MJobs_Counseling = true;
        public bool MJobs_Converting = true;
        public bool MJobs_Recruiting = true;
        public bool MJobs_AnimalTraining = true;
        public bool MJobs_AnimalTaming = true;
        public bool MJobs_Butchering = true;
        public bool MJobs_Brewing = true;
        public bool MJobs_Maintaining = true;
        public bool MJobs_Deconstructing = true;
        public bool MJobs_Smoothing = false;
        public bool MJobs_Painting = false;
        public bool MJobs_Harvesting = true;
        public bool MJobs_Drilling = true;
        public bool MJobs_Pruning = false;
        public bool MJobs_Mechanitor = true;
        public bool MJobs_Fabricating = true;
        public bool MJobs_Synthesizing = true;
        public bool MJobs_Refining = true;
        public bool MJobs_Smelting = true;
        public bool MJobs_Stonecutting = true;
        public bool MJobs_Delivering = true;
        public bool MJobs_Loading = true;
        public bool MJobs_Merging = false;
        public bool MJobs_Scanning = true;

        private readonly List<WorkType> _workTypes = new List<WorkType>();

        private readonly Dictionary<WorkType, bool> _workTypesDefaults = new Dictionary<WorkType, bool>();

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
            // This is a little convoluted. First we use reflection to get an array of all this class's public instance
            // fields. Then we iterate over the array and create a new anonymous WorkType object for each field that's
            // of type bool and that has a name that starts with "MJobs_". We add each anonymous WorkType object to a
            // list that we can iterate over later. THEN we add each field's value (true or false) to a dictionary that
            // maps WorkTypes to default values. We use this to decide whether a given bool should default to true or
            // false when we pass it to Scribe_Values.Look down in the ExposeData method.

            foreach (FieldInfo field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (field.Name.StartsWith("MJobs_") && (field.FieldType == typeof(bool)))
                {
                    bool defaultValue = (bool)field.GetValue(this);
                    WorkType workType = new WorkType(field.Name, defaultValue);
                    _workTypes.Add(workType);
                    _workTypesDefaults.Add(workType, defaultValue);
                }
            }
        }

        public void OnLateInitialize()
        {
            foreach (WorkType workType in _workTypes)
            {
                Vector2 nameSize = Text.CalcSize(workType.LabelShort);
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
            foreach (WorkType workType in _workTypes)
            {
                if (workType.IsDirty)
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
            foreach (WorkType workType in _workTypes)
            {
                Scribe_Values.Look(ref workType.IsEnabled, workType.Name, _workTypesDefaults[workType]);
                workType.IsEnabledInConfigFile = workType.IsEnabled;
                this.GetType().GetField(workType.Name).SetValue(this, workType.IsEnabled);
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
                filteredWorkTypes = _workTypes.Where(wt => wt.Def.labelShort.Contains(_filter) || wt.Def.description.Contains(_filter)).ToList();
                offButtonLabel = SelectedOffButtonLabel;
                onButtonLabel = SelectedOnButtonLabel;
            }
            else
            {
                filteredWorkTypes = _workTypes;
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

            foreach (WorkType workType in filteredWorkTypes)
            {
                Rect rect = _workTypesListing.GetRect(GenUI.GapTiny + Text.LineHeight + GenUI.GapTiny);

                Rect nameRect = new Rect(rect) { width = _maxWorkTypeNameWidth + GenUI.Gap };
                Rect descriptionRect = new Rect(rect) { x = nameRect.xMax, width = rect.width - nameRect.width - GenUI.Gap * 2 };
                Rect clickableRect = new Rect(rect) { width = rect.width - Widgets.CheckboxSize };

                Widgets.Label(nameRect, workType.LabelShort);

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
