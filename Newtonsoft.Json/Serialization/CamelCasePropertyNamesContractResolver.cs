using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000072 RID: 114
	[NullableContext(1)]
	[Nullable(0)]
	public class CamelCasePropertyNamesContractResolver : DefaultContractResolver
	{
		// Token: 0x060005FA RID: 1530 RVA: 0x00018B24 File Offset: 0x00016D24
		public CamelCasePropertyNamesContractResolver()
		{
			base.NamingStrategy = new CamelCaseNamingStrategy
			{
				ProcessDictionaryKeys = true,
				OverrideSpecifiedNames = true
			};
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00018B48 File Offset: 0x00016D48
		public override JsonContract ResolveContract(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			StructMultiKey<Type, Type> structMultiKey = new StructMultiKey<Type, Type>(base.GetType(), type);
			Dictionary<StructMultiKey<Type, Type>, JsonContract> dictionary = CamelCasePropertyNamesContractResolver._contractCache;
			JsonContract jsonContract;
			if (dictionary == null || !dictionary.TryGetValue(structMultiKey, out jsonContract))
			{
				jsonContract = this.CreateContract(type);
				object typeContractCacheLock = CamelCasePropertyNamesContractResolver.TypeContractCacheLock;
				lock (typeContractCacheLock)
				{
					dictionary = CamelCasePropertyNamesContractResolver._contractCache;
					Dictionary<StructMultiKey<Type, Type>, JsonContract> dictionary2 = ((dictionary != null) ? new Dictionary<StructMultiKey<Type, Type>, JsonContract>(dictionary) : new Dictionary<StructMultiKey<Type, Type>, JsonContract>());
					dictionary2[structMultiKey] = jsonContract;
					CamelCasePropertyNamesContractResolver._contractCache = dictionary2;
				}
			}
			return jsonContract;
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00018BE8 File Offset: 0x00016DE8
		internal override DefaultJsonNameTable GetNameTable()
		{
			return CamelCasePropertyNamesContractResolver.NameTable;
		}

		// Token: 0x0400022A RID: 554
		private static readonly object TypeContractCacheLock = new object();

		// Token: 0x0400022B RID: 555
		private static readonly DefaultJsonNameTable NameTable = new DefaultJsonNameTable();

		// Token: 0x0400022C RID: 556
		[Nullable(new byte[] { 2, 0, 1, 1, 1 })]
		private static Dictionary<StructMultiKey<Type, Type>, JsonContract> _contractCache;
	}
}
