namespace SqlIntro
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
    public enum Crud
    {
        Create,
        Read,
        Update,
        Delete
    }
}