namespace Project_Final.Models
{
    public class ShoeMetadata : Metadata
    {
        public ShoeMetadata()
        {
            Type = nameof(ShoeMetadata);
        }

        public double SoleHeight { get; set; }
        public double SoleLength { get; set; }
        public double SoleMeterial { get; set; }
    }
}
