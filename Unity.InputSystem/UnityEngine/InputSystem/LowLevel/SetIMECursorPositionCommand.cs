using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000C5 RID: 197
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	public struct SetIMECursorPositionCommand : IInputDeviceCommandInfo
	{
		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000D3B RID: 3387 RVA: 0x000446E4 File Offset: 0x000428E4
		public static FourCC Type
		{
			get
			{
				return new FourCC('I', 'M', 'E', 'P');
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000D3C RID: 3388 RVA: 0x000446F3 File Offset: 0x000428F3
		public Vector2 position
		{
			get
			{
				return this.m_Position;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000D3D RID: 3389 RVA: 0x000446FB File Offset: 0x000428FB
		public FourCC typeStatic
		{
			get
			{
				return SetIMECursorPositionCommand.Type;
			}
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00044704 File Offset: 0x00042904
		public static SetIMECursorPositionCommand Create(Vector2 cursorPosition)
		{
			return new SetIMECursorPositionCommand
			{
				baseCommand = new InputDeviceCommand(SetIMECursorPositionCommand.Type, 16),
				m_Position = cursorPosition
			};
		}

		// Token: 0x04000528 RID: 1320
		internal const int kSize = 16;

		// Token: 0x04000529 RID: 1321
		[FieldOffset(0)]
		public InputDeviceCommand baseCommand;

		// Token: 0x0400052A RID: 1322
		[FieldOffset(8)]
		private Vector2 m_Position;
	}
}
