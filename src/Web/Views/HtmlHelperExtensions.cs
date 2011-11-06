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

		public static MvcHtmlString Url<TController>(this HtmlHelper helper, Expression<Action<TController>> action) where TController : Controller
		{
			return MvcHtmlString.Create(helper.BuildUrlFromExpression(action));
		}

		public static ConditionalActionLink MaybeAction(this HtmlHelper helper, string linkText, string actionName, object routeValues)
		{
			return MaybeAction(helper, linkText, actionName, routeValues, null);
		}

		public static ConditionalActionLink MaybeAction(this HtmlHelper helper, string linkText, string actionName, object routeValues, object htmlAttributes)
		{
			return new ConditionalActionLink(helper, linkText, actionName, routeValues, htmlAttributes);
		}
	}

	public static class Name
	{
		public static string After<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)
		{
			return ExpressionHelper.GetExpressionText(property);
		}
	}

	public class ConditionalActionLink
	{
		private readonly HtmlHelper _helper;
		private readonly string _linkText, _actionName;
		private readonly object _routeValues, _htmlAttributes;

		public ConditionalActionLink(HtmlHelper helper, string linkText, string actionName, object routeValues, object htmlAttributes)
		{
			_helper = helper;
			_linkText = linkText;
			_actionName = actionName;
			_routeValues = routeValues;
			_htmlAttributes = htmlAttributes;
		}

		public MvcHtmlString DisabledIf(bool condition)
		{
			return condition ? disabledLink() : enabledLink();
		}

		private MvcHtmlString disabledLink()
		{
			var span = new TagBuilder("span");
			span.MergeAttribute("class", "disabled");

			span.InnerHtml = _linkText;

			return MvcHtmlString.Create(span.ToString(TagRenderMode.Normal));
		}

		private MvcHtmlString enabledLink()
		{
			return System.Web.Mvc.Html.LinkExtensions.ActionLink(_helper, _linkText, _actionName, _routeValues, _htmlAttributes);
		}
	}
}