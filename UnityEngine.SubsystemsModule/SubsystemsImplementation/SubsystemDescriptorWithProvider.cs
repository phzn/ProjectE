using System;

namespace UnityEngine.SubsystemsImplementation
{
	// Token: 0x02000012 RID: 18
	public abstract class SubsystemDescriptorWithProvider : ISubsystemDescriptor
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002AC3 File Offset: 0x00000CC3
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00002ACB File Offset: 0x00000CCB
		public string id { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002AD4 File Offset: 0x00000CD4
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00002ADC File Offset: 0x00000CDC
		protected internal Type providerType { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00002AE5 File Offset: 0x00000CE5
		// (set) Token: 0x0600005F RID: 95 RVA: 0x00002AED File Offset: 0x00000CED
		protected internal Type subsystemTypeOverride { get; set; }

		// Token: 0x06000060 RID: 96
		internal abstract ISubsystem CreateImpl();

		// Token: 0x06000061 RID: 97 RVA: 0x00002AF6 File Offset: 0x00000CF6
		ISubsystem ISubsystemDescriptor.Create()
		{
			return this.CreateImpl();
		}

		// Token: 0x06000062 RID: 98
		internal abstract void ThrowIfInvalid();
	}
}
