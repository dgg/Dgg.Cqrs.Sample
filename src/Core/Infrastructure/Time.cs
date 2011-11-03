using System;

namespace Dgg.Cqrs.Sample.Core.Infrastructure
{
	public static class Time
	{
		public static Func<DateTimeOffset> UtcProvider;

		public static DateTimeOffset Now
		{
			get
			{
				return UtcProvider == null ?
					DateTimeOffset.Now :
					UtcProvider().ToLocalTime();
			}
		}

		public static DateTimeOffset UtcNow
		{
			get
			{
				return UtcProvider == null ?
					DateTimeOffset.UtcNow :
					UtcProvider();
			}
		}
	}
}
