namespace WebApi.Core.Application.Search.OrderBy
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class QueryStringOrderByTypeConverter : TypeConverter
    {
        private readonly Type _type;

        public QueryStringOrderByTypeConverter(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(QueryStringOrderBy<>) && type.GetGenericArguments().Length == 1)
                _type = type;
            else
                throw new ArgumentException("Incompatible type", "type");
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    return Activator.CreateInstance(_type, new[] { value });
                }
                catch
                {
                    return base.ConvertFrom(context, culture, value);
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}