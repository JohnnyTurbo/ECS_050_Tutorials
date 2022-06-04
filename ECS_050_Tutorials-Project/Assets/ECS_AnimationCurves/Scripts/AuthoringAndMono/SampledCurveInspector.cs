using System.Text;
using Unity.Entities;
using Unity.Properties.UI;
using UnityEngine.UIElements;

namespace TMG.AnimationCurves
{
    public class SampledCurveInspector : Inspector<BlobAssetReference<SampledCurve>>
    {
        public override VisualElement Build()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"Number of Sample Points: {Target.Value.NumberOfSamples}\n");
            for (var i = 0; i < Target.Value.NumberOfSamples; i++)
            {
                stringBuilder.Append($"{i}\t{Target.Value.SampledPoints[i]}\n");
            }

            return new Label(stringBuilder.ToString());
        }
    }
}