using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Dgg.Cqrs.Sample.Core.Presentation.Models.Admin
{
	public class AppVersionAssignation
	{
		[Obsolete("serialization")]
		public AppVersionAssignation() { }

		public AppVersionAssignation(AppVersion version, IEnumerable<Solution> assignableSolutions)
		{
			Version = version;
			string assignedId = version.AssignedTo != null ? version.AssignedTo.Id : string.Empty;
			Solutions = assignableSolutions.ToSelectList(
				s => s.Id,
				s => s.Name,
				s => string.Equals(s.Id, assignedId, StringComparison.OrdinalIgnoreCase));
		}

		public AppVersion Version { get; private set; }
		public IEnumerable<SelectListItem> Solutions { get; private set; }
	}
}
