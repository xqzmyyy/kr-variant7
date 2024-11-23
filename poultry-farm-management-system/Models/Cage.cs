namespace farm.Models
{
    public class Cage 
    {
        public int Id { get; set; } // id

        // relate with employee
        public int EmployeeId { get; set; } // key
        public Employee Employee { get; set; } // link

        // relate with chicken
        public int? ChickenId { get; set; } // key
        public Chicken? Chicken { get; set; } // link

        // relate with eggs
        public DateTime Date { get; set; }  // date
        public bool IsEggLaid { get; set; } // has egg
    }
}