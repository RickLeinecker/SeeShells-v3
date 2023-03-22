
namespace SeeShellsV3.Data
{
    public class Fields
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public Fields(string field, object fieldVal) 
        {
            Name = field;
            Value = fieldVal;
        }
    }
}