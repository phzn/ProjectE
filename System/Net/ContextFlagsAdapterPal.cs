using System;

namespace System.Net
{
	// Token: 0x0200055F RID: 1375
	internal static class ContextFlagsAdapterPal
	{
		// Token: 0x06002C99 RID: 11417 RVA: 0x000980F8 File Offset: 0x000962F8
		internal static ContextFlagsPal GetContextFlagsPalFromInterop(global::Interop.SspiCli.ContextFlags win32Flags)
		{
			ContextFlagsPal contextFlagsPal = ContextFlagsPal.None;
			foreach (ContextFlagsAdapterPal.ContextFlagMapping contextFlagMapping in ContextFlagsAdapterPal.s_contextFlagMapping)
			{
				if ((win32Flags & contextFlagMapping.Win32Flag) == contextFlagMapping.Win32Flag)
				{
					contextFlagsPal |= contextFlagMapping.ContextFlag;
				}
			}
			return contextFlagsPal;
		}

		// Token: 0x06002C9A RID: 11418 RVA: 0x00098140 File Offset: 0x00096340
		internal static global::Interop.SspiCli.ContextFlags GetInteropFromContextFlagsPal(ContextFlagsPal flags)
		{
			global::Interop.SspiCli.ContextFlags contextFlags = global::Interop.SspiCli.ContextFlags.Zero;
			foreach (ContextFlagsAdapterPal.ContextFlagMapping contextFlagMapping in ContextFlagsAdapterPal.s_contextFlagMapping)
			{
				if ((flags & contextFlagMapping.ContextFlag) == contextFlagMapping.ContextFlag)
				{
					contextFlags |= contextFlagMapping.Win32Flag;
				}
			}
			return contextFlags;
		}

		// Token: 0x040017F9 RID: 6137
		private static readonly ContextFlagsAdapterPal.ContextFlagMapping[] s_contextFlagMapping = new ContextFlagsAdapterPal.ContextFlagMapping[]
		{
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.AcceptExtendedError, ContextFlagsPal.AcceptExtendedError),
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.InitManualCredValidation, ContextFlagsPal.InitManualCredValidation),
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.AcceptIntegrity, ContextFlagsPal.AcceptIntegrity),
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.AcceptStream, ContextFlagsPal.AcceptStream),
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.AllocateMemory, ContextFlagsPal.AllocateMemory),
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.AllowMissingBindings, ContextFlagsPal.AllowMissingBindings),
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.Confidentiality, ContextFlagsPal.Confidentiality),
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.Connection, ContextFlagsPal.Connection),
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.Delegate, ContextFlagsPal.Delegate),
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.InitExtendedError, ContextFlagsPal.InitExtendedError),
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.AcceptIntegrity, ContextFlagsPal.AcceptIntegrity),
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.InitManualCredValidation, ContextFlagsPal.InitManualCredValidation),
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.AcceptStream, ContextFlagsPal.AcceptStream),
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.AcceptExtendedError, ContextFlagsPal.AcceptExtendedError),
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.InitUseSuppliedCreds, ContextFlagsPal.InitUseSuppliedCreds),
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.MutualAuth, ContextFlagsPal.MutualAuth),
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.ProxyBindings, ContextFlagsPal.ProxyBindings),
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.ReplayDetect, ContextFlagsPal.ReplayDetect),
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.SequenceDetect, ContextFlagsPal.SequenceDetect),
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.UnverifiedTargetName, ContextFlagsPal.UnverifiedTargetName),
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.UseSessionKey, ContextFlagsPal.UseSessionKey),
			new ContextFlagsAdapterPal.ContextFlagMapping(global::Interop.SspiCli.ContextFlags.Zero, ContextFlagsPal.None)
		};

		// Token: 0x02000560 RID: 1376
		private readonly struct ContextFlagMapping
		{
			// Token: 0x06002C9C RID: 11420 RVA: 0x0009835E File Offset: 0x0009655E
			public ContextFlagMapping(global::Interop.SspiCli.ContextFlags win32Flag, ContextFlagsPal contextFlag)
			{
				this.Win32Flag = win32Flag;
				this.ContextFlag = contextFlag;
			}

			// Token: 0x040017FA RID: 6138
			public readonly global::Interop.SspiCli.ContextFlags Win32Flag;

			// Token: 0x040017FB RID: 6139
			public readonly ContextFlagsPal ContextFlag;
		}
	}
}
