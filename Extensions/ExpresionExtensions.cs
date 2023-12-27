using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Venn.Extensions
{
    public static class ExpresionExtensions
    {
        public static string GetPropertyName<T, TProperty>(this Expression<Func<T, TProperty>> propertySelector)
        {
            if (propertySelector.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }
            throw new ArgumentException("Invalid property expression");
        }
    }
}
