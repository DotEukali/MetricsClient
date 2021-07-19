using System;
using System.Linq;
using System.Reflection;

namespace DotEukali.MetricsClient.Core.Extensions
{
    internal static class EnumExtensions
    {
        public static string Description<T>(this T enumValue) where T : struct, IConvertible
        {
            Type genericEnumType = enumValue.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(enumValue.ToString()!);

            if (memberInfo.Length > 0)
            {
                var attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (attribs.Any())
                {
                    return ((System.ComponentModel.DescriptionAttribute)attribs.ElementAt(0)).Description;
                }
            }
            return enumValue.ToString();
        }
    }
}
