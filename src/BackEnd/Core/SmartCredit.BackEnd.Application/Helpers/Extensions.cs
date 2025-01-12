using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SmartCredit.BackEnd.Application.Helpers
{
    public static class Extensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
          return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()
            ?.GetName();
        }
    }
}