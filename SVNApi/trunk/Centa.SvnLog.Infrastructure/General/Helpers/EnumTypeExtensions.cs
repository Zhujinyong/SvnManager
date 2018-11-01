using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Centa.SvnLog.Infrastructure.General.Helpers
{
    public static class EnumTypeExtensions
    {
        /// <summary>
        /// 从枚举中输出字符串
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetEnumDescription(this Type enumType)
        {
            FieldInfo[] fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
            Dictionary<int, string> dic = new Dictionary<int, string>();
            foreach (var field in fields)
            {
                var dna = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
                string description = string.Empty;
                if (dna == null || string.IsNullOrEmpty(dna.Description))
                {
                    description = "未知分类";
                }
                else
                {
                    description = dna.Description;
                }
                int value = (int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null);
                if (dic.ContainsKey(value))
                {
                    dic[value] += description;
                }
                else
                {
                    dic.Add(value, description);
                }
            }
            return dic;
        }
    }
}