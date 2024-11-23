namespace farm.Models
{
    public class Chicken 
    {
        public int Id { get; set; } // id
        public int Age { get; set; } // age
        public double Weight { get; set; } // weight
        public int EggsPerMonth { get; set; } // coount of eggs per month
        public string Breed { get; set; } // breed

        // relate with cage
        public int CageId { get; set; } // key
        public Cage Cage { get; set; } // link
    }
}