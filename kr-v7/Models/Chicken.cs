namespace krv7.Models
{
    public class Chicken
    {
        public int Id { get; set; }
        public double Weight { get; set; }
        public int Age { get; set; }
        public int EggsPerMonth { get; set; }
        public int CageId { get; set; }
        public Cage? Cage { get; set; } // relate to cage
    }
}
