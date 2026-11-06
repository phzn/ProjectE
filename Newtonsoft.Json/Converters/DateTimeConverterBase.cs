using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000E1 RID: 225
	public abstract class DateTimeConverterBase : JsonConverter
	{
		// Token: 0x06000C31 RID: 3121 RVA: 0x000307BC File Offset: 0x0002E9BC
		[NullableContext(1)]
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(DateTime) || objectType == typeof(DateTime?) || (objectType == typeof(DateTimeOffset) || objectType == typeof(DateTimeOffset?));
		}
	}
}
