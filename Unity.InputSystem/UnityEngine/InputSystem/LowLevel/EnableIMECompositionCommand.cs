using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000B5 RID: 181
	[StructLayout(LayoutKind.Explicit, Size = 9)]
	public struct EnableIMECompositionCommand : IInputDeviceCommandInfo
	{
		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x00044144 File Offset: 0x00042344
		public static FourCC Type
		{
			get
			{
				return new FourCC('I', 'M', 'E', 'M');
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000D01 RID: 3329 RVA: 0x00044153 File Offset: 0x00042353
		public bool imeEnabled
		{
			get
			{
				return this.m_ImeEnabled > 0;
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x0004415E File Offset: 0x0004235E
		public FourCC typeStatic
		{
			get
			{
				return EnableIMECompositionCommand.Type;
			}
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x00044168 File Offset: 0x00042368
		public static EnableIMECompositionCommand Create(bool enabled)
		{
			return new EnableIMECompositionCommand
			{
				baseCommand = new InputDeviceCommand(EnableIMECompositionCommand.Type, 9),
				m_ImeEnabled = (enabled ? byte.MaxValue : 0)
			};
		}

		// Token: 0x040004FA RID: 1274
		internal const int kSize = 12;

		// Token: 0x040004FB RID: 1275
		[FieldOffset(0)]
		public InputDeviceCommand baseCommand;

		// Token: 0x040004FC RID: 1276
		[FieldOffset(8)]
		private byte m_ImeEnabled;
	}
}
