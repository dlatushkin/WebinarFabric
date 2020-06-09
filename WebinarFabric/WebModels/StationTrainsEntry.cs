namespace WebModels
{
    public class StationTrainsEntry
    {
        public StationEntry Station { get; set; }

        public TrainEntry[] Trains { get; set; }
    }
}
