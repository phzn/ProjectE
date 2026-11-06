using System;

namespace Unity.Properties
{
	// Token: 0x02000014 RID: 20
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public class GeneratePropertyBagsForTypesQualifiedWithAttribute : Attribute
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002DC5 File Offset: 0x00000FC5
		public Type Type { get; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00002DCD File Offset: 0x00000FCD
		public TypeGenerationOptions Options { get; }

		// Token: 0x06000051 RID: 81 RVA: 0x00002DD8 File Offset: 0x00000FD8
		public GeneratePropertyBagsForTypesQualifiedWithAttribute(Type type, TypeGenerationOptions options = TypeGenerationOptions.Default)
		{
			bool flag = type == null;
			if (flag)
			{
				throw new ArgumentException("type is null.");
			}
			bool flag2 = !type.IsInterface;
			if (flag2)
			{
				throw new ArgumentException("GeneratePropertyBagsForTypesQualifiedWithAttribute Type must be an interface type.");
			}
			this.Type = type;
			this.Options = options;
		}
	}
}
