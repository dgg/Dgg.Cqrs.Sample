using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Dgg.Cqrs.Sample.Core.Presentation.Models.Admin
{
	public class BuildAssignation
	{
		[Obsolete("serialization")]
		public BuildAssignation() { }

		public BuildAssignation(Build build, IEnumerable<Solution> assignableSolutions)
		{
			Build = build;
			string assignedId = build.AssignedTo != null ? build.AssignedTo.Id : string.Empty;
			Solutions = assignableSolutions.ToSelectList(
				s => s.Id,
				s => s.Name,
				s => string.Equals(s.Id, assignedId, StringComparison.OrdinalIgnoreCase));
		}

		public Build Build { get; private set; }
		public IEnumerable<SelectListItem> Solutions { get; private set; }
	}
}
