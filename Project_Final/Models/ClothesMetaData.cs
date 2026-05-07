namespace Project_Final.Models
{
    public class ClothesMetadata : Metadata
    {
        public ClothesMetadata()
        {
            Type = nameof(ClothesMetadata);
        }

        public string BodyType { get; set; }
        public string Accessory { get; set; }
    }
}
