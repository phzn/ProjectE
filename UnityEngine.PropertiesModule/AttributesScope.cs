using System;
using System.Collections.Generic;
using Unity.Properties.Internal;

namespace Unity.Properties
{
	// Token: 0x0200001A RID: 26
	public readonly struct AttributesScope : IDisposable
	{
		// Token: 0x0600005F RID: 95 RVA: 0x00002F6C File Offset: 0x0000116C
		public AttributesScope(IProperty target, IProperty source)
		{
			this.m_Target = target as IAttributes;
			IAttributes attributes = target as IAttributes;
			this.m_Previous = ((attributes != null) ? attributes.Attributes : null);
			bool flag = this.m_Target != null;
			if (flag)
			{
				IAttributes target2 = this.m_Target;
				IAttributes attributes2 = source as IAttributes;
				target2.Attributes = ((attributes2 != null) ? attributes2.Attributes : null);
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002FC9 File Offset: 0x000011C9
		internal AttributesScope(IAttributes target, List<Attribute> attributes)
		{
			this.m_Target = target;
			this.m_Previous = target.Attributes;
			target.Attributes = attributes;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002FE8 File Offset: 0x000011E8
		public void Dispose()
		{
			bool flag = this.m_Target != null;
			if (flag)
			{
				this.m_Target.Attributes = this.m_Previous;
			}
		}

		// Token: 0x04000028 RID: 40
		private readonly IAttributes m_Target;

		// Token: 0x04000029 RID: 41
		private readonly List<Attribute> m_Previous;
	}
}
