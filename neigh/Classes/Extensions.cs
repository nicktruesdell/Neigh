using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace neigh.Classes
{
    public static class Extensions
    {
        public static string GetDisplayName(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);
            return attr != null ? attr.Name : enu.ToString();
        }

        public static string GetDescription(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);
            return attr != null ? attr.Description : enu.ToString();
        }

        public static MvcHtmlString EnumDisplayFor<TModel, TValue>(this System.Web.Mvc.HtmlHelper<TModel> helper, System.Linq.Expressions.Expression<Func<TModel, TValue>> expression)
        {
            String strReturn = "";
            var data = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            if (data.Model is Enum)
            {
                Type type = data.Model.GetType();
                var field = type.GetField(data.Model.ToString());
                if (field != null)
                {
                    DisplayAttribute attr = field.GetCustomAttribute<DisplayAttribute>();
                    if (attr != null)
                    {
                        strReturn = attr.Name;
                    }
                    else
                    {
                        strReturn = data.Model.ToString();
                    }
                }
            }
            else
                strReturn = "";

            return new MvcHtmlString(strReturn);
        }

        public static MvcHtmlString VariableNumericDropdownList(this HtmlHelper helper, string name, int MinValue, int MaxValue, object htmlAttributes )
        {
            List<SelectListItem> lstItems = new List<SelectListItem>();
            for (int i = MinValue; i <= MaxValue; i++)
            {
                lstItems.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            return helper.DropDownList(name, lstItems, htmlAttributes);
        }

        private static DisplayAttribute GetDisplayAttribute(object value)
        {
            Type type = value.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException(string.Format("Type {0} is not an enum", type));
            }

            // Get the enum field.
            var field = type.GetField(value.ToString());
            return field == null ? null : field.GetCustomAttribute<DisplayAttribute>();
        }
    }
}