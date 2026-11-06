using System;

namespace Unity.Properties
{
	// Token: 0x02000017 RID: 23
	[Serializable]
	public class MissingPropertyBagException : Exception
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002E73 File Offset: 0x00001073
		public Type Type { get; }

		// Token: 0x06000056 RID: 86 RVA: 0x00002E7B File Offset: 0x0000107B
		public MissingPropertyBagException(Type type)
			: base(MissingPropertyBagException.GetMessageForType(type))
		{
			this.Type = type;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002E92 File Offset: 0x00001092
		public MissingPropertyBagException(Type type, Exception inner)
			: base(MissingPropertyBagException.GetMessageForType(type), inner)
		{
			this.Type = type;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002EAC File Offset: 0x000010AC
		private static string GetMessageForType(Type type)
		{
			return "No PropertyBag was found for Type=[" + type.FullName + "]. Please make sure all types are declared ahead of time using [GeneratePropertyBagAttribute], [GeneratePropertyBagsForTypeAttribute] or [GeneratePropertyBagsForTypesQualifiedWithAttribute]";
		}
	}
}
