namespace Bank.Domain.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; }
        public string LastName { get; }

        public Person(int id, string firstName, string lastName) : this(firstName, lastName)
        {
            Id = id;
        }
        
        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}