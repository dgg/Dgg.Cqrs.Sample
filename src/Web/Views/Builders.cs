using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.Web.Mvc;

namespace Dgg.Cqrs.Sample.Web.Views
{
	internal class ImageBuilder : TagBuilder
	{
		public ImageBuilder(HtmlHelper helper, string src, string title)
			: base(HtmlTextWriterTag.Img.Lower())
		{
			MergeAttribute(HtmlTextWriterAttribute.Src.Lower(), new UrlHelper(helper.ViewContext.RequestContext).Content(src));
			MergeAttribute(HtmlTextWriterAttribute.Alt.Lower(), title);
			MergeAttribute(HtmlTextWriterAttribute.Title.Lower(), title);
		}
	}

	internal class ImageInputBuilder : TagBuilder
	{
		public ImageInputBuilder(HtmlHelper helper, string src, string title)
			: base(HtmlTextWriterTag.Input.Lower())
		{
			MergeAttribute("type", "image");
			MergeAttribute(HtmlTextWriterAttribute.Src.Lower(), new UrlHelper(helper.ViewContext.RequestContext).Content(src));
			MergeAttribute(HtmlTextWriterAttribute.Alt.Lower(), title);
			MergeAttribute(HtmlTextWriterAttribute.Title.Lower(), title);
		}
	}

	internal class ImageLinkBuilder<TController> : TagBuilder where TController : Controller
	{
		public ImageLinkBuilder(HtmlHelper helper, Expression<Action<TController>> action, ImageBuilder img)
			: base(HtmlTextWriterTag.A.Lower())
		{
			var url = LinkBuilder.BuildUrlFromExpression(helper.ViewContext.RequestContext, helper.RouteCollection, action);
			MergeAttribute(HtmlTextWriterAttribute.Href.Lower(), url);

			InnerHtml = img.ToString(TagRenderMode.SelfClosing);
		}
	}

	internal class FormBuilder<TController> : TagBuilder where TController : Controller
	{
		public FormBuilder(HtmlHelper helper, Expression<Action<TController>> action)
			: base(HtmlTextWriterTag.Form.Lower())
		{
			string url = helper.BuildUrlFromExpression(action);
			MergeAttribute("action", url);
			MergeAttribute("method", HtmlHelper.GetFormMethodString(FormMethod.Post));
		}
	}
}