namespace ClusterModels.Trains
{
    public class Train
    {
        public string Code { get; set; }

        public string PrevStationCode { get; set; }

        public string CurStationCode { get; set; }

        public string NextStationCode { get; set; }
    }
}
