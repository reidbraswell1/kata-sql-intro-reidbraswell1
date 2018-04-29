using System.ComponentModel;
using System.Reflection;
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
        Delete
    }
    static class CrudMethods
    {
        internal static string getEnumDescription(Crud crud)
        {
            FieldInfo fi = crud.GetType().GetField(crud.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : crud.ToString();
        }
    }
}