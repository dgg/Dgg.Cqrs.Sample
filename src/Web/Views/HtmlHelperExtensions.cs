using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Microsoft.Web.Mvc;

namespace Dgg.Cqrs.Sample.Web.Views
{
	public static class HtmlHelperExtensions
	{
		public static MvcHtmlString DeleteLink<TController>(this HtmlHelper helper, Expression<Action<TController>> action, string imageUrlPath, string title) where TController : Controller
		{
			var url = LinkBuilder.BuildUrlFromExpression(helper.ViewContext.RequestContext, helper.RouteCollection, action);

			var formTag = new TagBuilder("form");

			formTag.MergeAttribute("action", url);
			formTag.MergeAttribute("method", "POST");

			var inputTag = new TagBuilder("input");
			inputTag.MergeAttribute("type", "image");
			inputTag.MergeAttribute("src", imageUrlPath);
			inputTag.MergeAttribute("alt", "Delete");

			formTag.InnerHtml = inputTag.ToString(TagRenderMode.SelfClosing) + helper.HttpMethodOverride(HttpVerbs.Delete);

			return MvcHtmlString.Create(formTag.ToString());
		}

		public static string NamedAfter<TModel, TProperty>(this HtmlHelper helper, Expression<Func<TModel, TProperty>> property)
		{
			return ExpressionHelper.GetExpressionText(property);
		}
	}
}