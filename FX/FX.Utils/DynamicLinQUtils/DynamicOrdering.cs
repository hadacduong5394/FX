using System.Linq.Expressions;

namespace FX.Utils.DynamicLinQUtils
{
    internal class DynamicOrdering
    {
        public Expression Selector;
        public bool Ascending;
    }
}