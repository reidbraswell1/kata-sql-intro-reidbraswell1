using System.ComponentModel;
namespace SqlIntro
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
    public enum Crud
    {
        [Description("INSERT")]
        Create,
        [Description("SELECT")]
        Read,
        [Description("UPDATE")]
        Update,
        [Description("DELETE")]
        Delet
    }
}