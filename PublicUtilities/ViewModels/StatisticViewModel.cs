namespace PublicUtilities.ViewModels
{
    public class StatisticViewModel
    {
        public string PlacesOfResidence { get; set; }
        public List<DataPoint> DataPoints { get; set; }
        public StatisticViewModel()
        {
            DataPoints = new List<DataPoint>();
        }
    }

    public class DataPoint
    {
        public string Date { get; set; }
        public string Indicator { get; set; }
        public decimal Price { get; set; }
    }
}
