using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200008B RID: 139
	public class JsonISerializableContract : JsonContainerContract
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x0001C211 File Offset: 0x0001A411
		// (set) Token: 0x060006DE RID: 1758 RVA: 0x0001C219 File Offset: 0x0001A419
		[Nullable(new byte[] { 2, 1 })]
		public ObjectConstructor<object> ISerializableCreator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0001C222 File Offset: 0x0001A422
		[NullableContext(1)]
		public JsonISerializableContract(Type underlyingType)
			: base(underlyingType)
		{
			this.ContractType = JsonContractType.Serializable;
		}
	}
}
