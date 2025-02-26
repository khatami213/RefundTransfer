using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefundTransferService.Helper_Code.Enums
{
    public class EnumTemplate
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }

    public static class ExtentionMethodes
    {
        public static List<EnumTemplate> GetItems(this Type enumType)
        {
            //Type enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T is not System.Enum");

            var enumValList = new List<EnumTemplate>();

            //foreach (var e in Enum.GetValues(typeof(T)))
            foreach (var e in Enum.GetValues(enumType))
            {
                var fi = e.GetType().GetField(e.ToString());

                enumValList.Add(new EnumTemplate
                {
                    Title = e.ToString(),
                    Id = (int)e
                });
            }

            return enumValList;
        }
    }
}
