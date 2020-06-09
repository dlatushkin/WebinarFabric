using System;

namespace ClusterModels.Trains
{
    public class TrainPositionMoment
    {
        public string LineId { get; set; }

        public string Number { get; set; }

        public float Point { get; set; }

        public DateTime Moment { get; set; }
    }
}
