using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000DD RID: 221
	[StructLayout(LayoutKind.Explicit, Size = 560)]
	internal struct TouchscreenState : IInputStateTypeInfo
	{
		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000D83 RID: 3459 RVA: 0x00044D17 File Offset: 0x00042F17
		public static FourCC Format
		{
			get
			{
				return new FourCC('T', 'S', 'C', 'R');
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000D84 RID: 3460 RVA: 0x00044D28 File Offset: 0x00042F28
		public unsafe TouchState* primaryTouch
		{
			get
			{
				fixed (byte* ptr = &this.primaryTouchData.FixedElementField)
				{
					return (TouchState*)ptr;
				}
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000D85 RID: 3461 RVA: 0x00044D44 File Offset: 0x00042F44
		public unsafe TouchState* touches
		{
			get
			{
				fixed (byte* ptr = &this.touchData.FixedElementField)
				{
					return (TouchState*)ptr;
				}
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000D86 RID: 3462 RVA: 0x00044D5F File Offset: 0x00042F5F
		public FourCC format
		{
			get
			{
				return TouchscreenState.Format;
			}
		}

		// Token: 0x0400058F RID: 1423
		public const int MaxTouches = 10;

		// Token: 0x04000590 RID: 1424
		[FixedBuffer(typeof(byte), 56)]
		[InputControl(name = "primaryTouch", displayName = "Primary Touch", layout = "Touch", synthetic = true)]
		[InputControl(name = "primaryTouch/tap", usage = "PrimaryAction")]
		[InputControl(name = "position", useStateFrom = "primaryTouch/position")]
		[InputControl(name = "delta", useStateFrom = "primaryTouch/delta", layout = "Delta")]
		[InputControl(name = "pressure", useStateFrom = "primaryTouch/pressure")]
		[InputControl(name = "radius", useStateFrom = "primaryTouch/radius")]
		[InputControl(name = "press", useStateFrom = "primaryTouch/phase", layout = "TouchPress", synthetic = true, usages = new string[] { })]
		[InputControl(name = "displayIndex", useStateFrom = "primaryTouch/displayIndex", format = "BYTE")]
		[FieldOffset(0)]
		public TouchscreenState.<primaryTouchData>e__FixedBuffer primaryTouchData;

		// Token: 0x04000591 RID: 1425
		internal const int kTouchDataOffset = 56;

		// Token: 0x04000592 RID: 1426
		[FixedBuffer(typeof(byte), 560)]
		[InputControl(layout = "Touch", name = "touch", displayName = "Touch", arraySize = 10)]
		[FieldOffset(56)]
		public TouchscreenState.<touchData>e__FixedBuffer touchData;

		// Token: 0x02000210 RID: 528
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 56)]
		public struct <primaryTouchData>e__FixedBuffer
		{
			// Token: 0x04000B93 RID: 2963
			public byte FixedElementField;
		}

		// Token: 0x02000211 RID: 529
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 560)]
		public struct <touchData>e__FixedBuffer
		{
			// Token: 0x04000B94 RID: 2964
			public byte FixedElementField;
		}
	}
}
