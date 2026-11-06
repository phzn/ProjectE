using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000ED RID: 237
	[StructLayout(LayoutKind.Explicit, Size = 24)]
	public struct TextEvent : IInputEventTypeInfo
	{
		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06000E3B RID: 3643 RVA: 0x00047396 File Offset: 0x00045596
		public FourCC typeStatic
		{
			get
			{
				return 1413830740;
			}
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x000473A4 File Offset: 0x000455A4
		public unsafe static TextEvent* From(InputEventPtr eventPtr)
		{
			if (!eventPtr.valid)
			{
				throw new ArgumentNullException("eventPtr");
			}
			if (!eventPtr.IsA<TextEvent>())
			{
				throw new InvalidCastException(string.Format("Cannot cast event with type '{0}' into TextEvent", eventPtr.type));
			}
			return (TextEvent*)eventPtr.data;
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x000473F4 File Offset: 0x000455F4
		public static TextEvent Create(int deviceId, char character, double time = -1.0)
		{
			return new TextEvent
			{
				baseEvent = new InputEvent(1413830740, 24, deviceId, time),
				character = (int)character
			};
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x0004742C File Offset: 0x0004562C
		public static TextEvent Create(int deviceId, int character, double time = -1.0)
		{
			return new TextEvent
			{
				baseEvent = new InputEvent(1413830740, 24, deviceId, time),
				character = character
			};
		}

		// Token: 0x040005DD RID: 1501
		public const int Type = 1413830740;

		// Token: 0x040005DE RID: 1502
		[FieldOffset(0)]
		public InputEvent baseEvent;

		// Token: 0x040005DF RID: 1503
		[FieldOffset(20)]
		public int character;
	}
}
