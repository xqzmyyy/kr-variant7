namespace krv7.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // init value
        public double Salary { get; set; }
        public List<Cage> Cages { get; set; } = new List<Cage>(); // relate with cage
    }
}
