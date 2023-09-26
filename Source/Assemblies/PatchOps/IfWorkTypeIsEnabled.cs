using System.Collections.Generic;
using System.Xml;
using Verse;
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value

namespace ManyJobs.PatchOps
{
    public class IfWorkTypeIsEnabled : PatchOperation
    {
        // ReSharper disable once InconsistentNaming
        private string workType;
        // ReSharper disable once InconsistentNaming
        private List<PatchOperation> operations;
        // ReSharper disable once InconsistentNaming
        private PatchOperation lastFailedOperation;

        protected override bool ApplyWorker(XmlDocument xml)
        {

            WorkType wt = LoadedModManager.GetMod<ManyJobs>().GetSettings<ModSettings>().WorkTypes.FirstOrDefault(item => item.Name == workType);
            if (wt == null)
            {
                Log.Error($"WorkType {workType} not found in ModSettings.WorkTypes.");
                return false;
            }
            if (!wt.IsEnabled) return true;

            foreach (PatchOperation operation in operations)
            {
                if (!operation.Apply(xml))
                {
                    lastFailedOperation = operation;
                    return false;
                }
            }
            return true;
        }

        public override void Complete(string modIdentifier)
        {
            base.Complete(modIdentifier);
            lastFailedOperation = null;
        }

        public override string ToString()
        {
            int num = ((operations != null) ? operations.Count : 0);
            string text = $"{base.ToString()}(count={num}";
            if (lastFailedOperation != null)
            {
                text = text + ", lastFailedOperation=" + lastFailedOperation;
            }
            return text + ")";
        }
    }
}
