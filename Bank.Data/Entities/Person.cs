namespace Bank.Data.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Name => $"{FirstName ?? ""} {LastName ?? ""}";
    }
}