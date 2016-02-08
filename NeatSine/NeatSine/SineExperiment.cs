using SharpNeat.Core;
using SharpNeat.Phenomes;

namespace NeatSine
{
    class SineExperiment : SimpleNeatExperiment
    {
        public SineExperiment(bool evaluateParents, IPhenomeEvaluator<IBlackBox> phenomeEvaluator)
        {
            EvaluateParents = evaluateParents;
            PhenomeEvaluator = phenomeEvaluator;
        }

        public override IPhenomeEvaluator<IBlackBox> PhenomeEvaluator { get; }
        public override int InputCount => 1;
        public override int OutputCount => 1;
        public override bool EvaluateParents { get; }
    }
}
