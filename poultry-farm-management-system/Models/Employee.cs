namespace farm.Models

public class Employee 
{
    public int Id { get; set; } // id
    public string Name { get; set; } // name
    public decimal Salary { get; set; } // salary

    // relate with cage
    public ICollection<Cage> Cages { get; set; }
}
