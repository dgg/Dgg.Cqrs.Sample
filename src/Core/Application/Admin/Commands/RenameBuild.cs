﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Dgg.Cqrs.Sample.Core.Infrastructure.Commanding;
using Dgg.Cqrs.Sample.Core.Infrastructure.Validation.Validators;

namespace Dgg.Cqrs.Sample.Core.Application.Admin.Commands
{
	public class RenameBuild : ICommand
	{
		[Obsolete("serialization")]
		public RenameBuild() { }

		public RenameBuild(Guid id, string newName)
		{
			Id = id;
			NewName = newName;
		}

		[NotEmptyId]
		public Guid Id { get; set; }
		[DisplayName("New Name")]
		[Required, StringLength(255, MinimumLength = 1)]
		public string NewName { get; set; }
	}
}
