using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	// Token: 0x0200001A RID: 26
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class JsonConverter
	{
		// Token: 0x06000081 RID: 129
		public abstract void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer);

		// Token: 0x06000082 RID: 130
		[return: Nullable(2)]
		public abstract object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer);

		// Token: 0x06000083 RID: 131
		public abstract bool CanConvert(Type objectType);

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00002E6F File Offset: 0x0000106F
		public virtual bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00002E72 File Offset: 0x00001072
		public virtual bool CanWrite
		{
			get
			{
				return true;
			}
		}
	}
}
