using System;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000007 RID: 7
	[NativeHeader("Modules/Subsystems/SubsystemDescriptor.h")]
	[UsedByNativeCode("SubsystemDescriptor")]
	[StructLayout(LayoutKind.Sequential)]
	public class IntegratedSubsystemDescriptor<TSubsystem> : IntegratedSubsystemDescriptor where TSubsystem : IntegratedSubsystem
	{
		// Token: 0x06000015 RID: 21 RVA: 0x000020F8 File Offset: 0x000002F8
		internal override ISubsystem CreateImpl()
		{
			return this.Create();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002118 File Offset: 0x00000318
		public TSubsystem Create()
		{
			IntPtr intPtr = SubsystemDescriptorBindings.Create(this.m_Ptr);
			TSubsystem tsubsystem = (TSubsystem)((object)SubsystemManager.GetIntegratedSubsystemByPtr(intPtr));
			bool flag = tsubsystem != null;
			if (flag)
			{
				tsubsystem.m_SubsystemDescriptor = this;
			}
			return tsubsystem;
		}
	}
}
