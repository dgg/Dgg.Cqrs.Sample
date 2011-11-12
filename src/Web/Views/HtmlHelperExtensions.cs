using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.UI;

namespace Dgg.Cqrs.Sample.Web.Views
{
	public static class HtmlHelperExtensions
	{
		public static MvcHtmlString DeleteAction<TController>(this HtmlHelper helper, string title, Expression<Action<TController>> action, string imagePath) where TController : Controller
		{
			var input = new ImageInputBuilder(helper, imagePath, title);

			var form = new FormBuilder<TController>(helper, action)
			{
				InnerHtml = input.ToString(TagRenderMode.SelfClosing) + helper.HttpMethodOverride(HttpVerbs.Delete)
			};

			return MvcHtmlString.Create(form.ToString());
		}

		public static MvcHtmlString Action<TController>(this HtmlHelper helper, string title, Expression<Action<TController>> action, string imagePath) where TController : Controller
		{
			TagBuilder a = new ImageLinkBuilder<TController>(helper, action,
				new ImageBuilder(helper, imagePath, title));

			return MvcHtmlString.Create(a.ToString(TagRenderMode.Normal));
		}

		public static ConditionalActionLink<TController> MaybeAction<TController>(this HtmlHelper helper, string title, Expression<Action<TController>> action, string imagePath) where TController : Controller
		{
			return new ConditionalActionLink<TController>(helper, title, action, imagePath);
		}

		public static ConditionalPostActionLink<TController> MaybePostAction<TController>(this HtmlHelper helper, string title, Expression<Action<TController>> action, string imagePath) where TController : Controller
		{
			return new ConditionalPostActionLink<TController>(helper, title, action, imagePath);
		}
	}

	public static class Name
	{
		public static string After<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)
		{
			return ExpressionHelper.GetExpressionText(property);
		}

		public static string Lower(this HtmlTextWriterAttribute attribute)
		{
			return attribute.ToString().ToLowerInvariant();
		}

		public static string Lower(this HtmlTextWriterTag tag)
		{
			return tag.ToString().ToLowerInvariant();
		}
	}

	public class ConditionalActionLink<TController> where TController : Controller
	{
		private readonly HtmlHelper _helper;
		private readonly string _title;
		private readonly Expression<Action<TController>> _action;
		private readonly string _imagePath;

		public ConditionalActionLink(HtmlHelper helper, string title, Expression<Action<TController>> action, string imagePath)
		{
			_helper = helper;
			_title = title;
			_action = action;
			_imagePath = imagePath;
		}

		public MvcHtmlString DisabledIf(bool condition)
		{
			return condition ? disabledLink() : enabledLink();
		}

		private MvcHtmlString disabledLink()
		{
			return MvcHtmlString.Empty;
		}

		private MvcHtmlString enabledLink()
		{
			var a = new ImageLinkBuilder<TController>(_helper, _action,
				new ImageBuilder(_helper, _imagePath, _title));
			return MvcHtmlString.Create(a.ToString(TagRenderMode.Normal));
		}
	}

	public class ConditionalPostActionLink<TController> where TController : Controller
	{
		private readonly HtmlHelper _helper;
		private readonly string _title;
		private readonly Expression<Action<TController>> _action;
		private readonly string _imagePath;

		public ConditionalPostActionLink(HtmlHelper helper, string title, Expression<Action<TController>> action, string imagePath)
		{
			_helper = helper;
			_title = title;
			_action = action;
			_imagePath = imagePath;
		}

		public MvcHtmlString DisabledIf(bool condition)
		{
			return condition ? disabledLink() : enabledLink();
		}

		private MvcHtmlString disabledLink()
		{
			return MvcHtmlString.Empty;
		}

		private MvcHtmlString enabledLink()
		{
			var img = new ImageInputBuilder(_helper, _imagePath, _title);
			img.MergeAttribute(HtmlTextWriterAttribute.Class.Lower(), "post");

			var form = new FormBuilder<TController>(_helper, _action)
			{
				InnerHtml = img.ToString(TagRenderMode.SelfClosing)
			};

			return new MvcHtmlString(form.ToString(TagRenderMode.Normal));
		}
	}
}