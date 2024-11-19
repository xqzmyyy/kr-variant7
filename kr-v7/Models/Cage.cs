namespace krv7.Models
{
    public class Cage
    {
        public int Id { get; set; }
        public int? ChickenId { get; set; }
        public Chicken? Chicken { get; set; } // relate to chicken
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; } // relate to employee
    }
}
