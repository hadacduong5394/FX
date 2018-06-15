using System.Linq.Expressions;

namespace FX.Utils.DynamicLinQUtils
{
    public class DynamicOrdering
    {
        public Expression Selector;
        public bool Ascending;
    }
}