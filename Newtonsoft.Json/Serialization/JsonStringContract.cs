using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000095 RID: 149
	public class JsonStringContract : JsonPrimitiveContract
	{
		// Token: 0x060007E6 RID: 2022 RVA: 0x00022655 File Offset: 0x00020855
		[NullableContext(1)]
		public JsonStringContract(Type underlyingType)
			: base(underlyingType)
		{
			this.ContractType = JsonContractType.String;
		}
	}
}
