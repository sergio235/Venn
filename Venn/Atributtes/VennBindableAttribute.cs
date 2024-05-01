using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venn.Atributtes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class VennBindableAttribute : Attribute 
    {
        public string PropertyName { get; }
        public string FieldType { get; }
        public VennBindableAttribute() { }

        public VennBindableAttribute(string propertyName, string fieldType) 
        {
            PropertyName = propertyName;
            FieldType = fieldType;
        }
    }
}
