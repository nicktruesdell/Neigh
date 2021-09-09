using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace neigh.Helpers
{
    public static class DisplayHelpers
    { 
        public static MvcHtmlString DisplayColumnNameFor<TModel, TClass, TProperty>(this HtmlHelper<TModel> helper, IEnumerable<TClass> model, Expression<Func<TClass, TProperty>> expression)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            var metadata = ModelMetadataProviders.Current.GetMetadataForProperty(() => Activator.CreateInstance<TClass>(), typeof(TClass), name);

            return new MvcHtmlString(metadata.DisplayName);

            /*
            System.Reflection.PropertyInfo pi = typeof(TClass).GetProperty(name);
            object[] attrArr = pi.GetCustomAttributes(typeof(DisplayAttribute), false);
            DisplayAttribute a = (DisplayAttribute)attrArr[0];
            if (a != null)
            {
                return new MvcHtmlString(a.GetName());
            }
            else
            {
                return new MvcHtmlString("");
            }
            */
        }
    }
}