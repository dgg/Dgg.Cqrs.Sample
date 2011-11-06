using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Dgg.Cqrs.Sample.Core.Presentation.Models
{
	public static class Extensions
	{
		public static IEnumerable<SelectListItem> ToSelectList<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valueSelector, Func<TItem, string> nameSelector, Func<TItem, bool> selectedValueSelector)
		{
			return items
				.Select(item => new {item, value = valueSelector(item)})
				.Select(a => new SelectListItem
				{
					Text = nameSelector(a.item),
					Value = a.value.ToString(),
					Selected = selectedValueSelector(a.item)
				});
		}
	}
}
