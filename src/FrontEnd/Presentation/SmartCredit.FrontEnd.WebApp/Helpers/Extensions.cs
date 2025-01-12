﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SmartCredit.FrontEnd.WebApp.Helpers
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
