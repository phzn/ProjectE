using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.DualShock.LowLevel
{
	// Token: 0x020000AE RID: 174
	[StructLayout(LayoutKind.Explicit, Size = 40)]
	internal struct DualShockHIDOutputReport : IInputDeviceCommandInfo
	{
		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x00043DA8 File Offset: 0x00041FA8
		public static FourCC Type
		{
			get
			{
				return new FourCC('H', 'I', 'D', 'O');
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000CE1 RID: 3297 RVA: 0x00043DB7 File Offset: 0x00041FB7
		public FourCC typeStatic
		{
			get
			{
				return DualShockHIDOutputReport.Type;
			}
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00043DC0 File Offset: 0x00041FC0
		public void SetMotorSpeeds(float lowFreq, float highFreq)
		{
			this.flags |= 1;
			this.lowFrequencyMotorSpeed = (byte)Mathf.Clamp(lowFreq * 255f, 0f, 255f);
			this.highFrequencyMotorSpeed = (byte)Mathf.Clamp(highFreq * 255f, 0f, 255f);
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x00043E18 File Offset: 0x00042018
		public void SetColor(Color color)
		{
			this.flags |= 2;
			this.redColor = (byte)Mathf.Clamp(color.r * 255f, 0f, 255f);
			this.greenColor = (byte)Mathf.Clamp(color.g * 255f, 0f, 255f);
			this.blueColor = (byte)Mathf.Clamp(color.b * 255f, 0f, 255f);
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x00043E9C File Offset: 0x0004209C
		public static DualShockHIDOutputReport Create(int outputReportSize)
		{
			return new DualShockHIDOutputReport
			{
				baseCommand = new InputDeviceCommand(DualShockHIDOutputReport.Type, 8 + outputReportSize),
				reportId = 5
			};
		}

		// Token: 0x040004E4 RID: 1252
		internal const int kSize = 40;

		// Token: 0x040004E5 RID: 1253
		internal const int kReportId = 5;

		// Token: 0x040004E6 RID: 1254
		[FieldOffset(0)]
		public InputDeviceCommand baseCommand;

		// Token: 0x040004E7 RID: 1255
		[FieldOffset(8)]
		public byte reportId;

		// Token: 0x040004E8 RID: 1256
		[FieldOffset(9)]
		public byte flags;

		// Token: 0x040004E9 RID: 1257
		[FixedBuffer(typeof(byte), 2)]
		[FieldOffset(10)]
		public DualShockHIDOutputReport.<unknown1>e__FixedBuffer unknown1;

		// Token: 0x040004EA RID: 1258
		[FieldOffset(12)]
		public byte highFrequencyMotorSpeed;

		// Token: 0x040004EB RID: 1259
		[FieldOffset(13)]
		public byte lowFrequencyMotorSpeed;

		// Token: 0x040004EC RID: 1260
		[FieldOffset(14)]
		public byte redColor;

		// Token: 0x040004ED RID: 1261
		[FieldOffset(15)]
		public byte greenColor;

		// Token: 0x040004EE RID: 1262
		[FieldOffset(16)]
		public byte blueColor;

		// Token: 0x040004EF RID: 1263
		[FixedBuffer(typeof(byte), 23)]
		[FieldOffset(17)]
		public DualShockHIDOutputReport.<unknown2>e__FixedBuffer unknown2;

		// Token: 0x02000204 RID: 516
		[Flags]
		public enum Flags
		{
			// Token: 0x04000B7A RID: 2938
			Rumble = 1,
			// Token: 0x04000B7B RID: 2939
			Color = 2
		}

		// Token: 0x02000205 RID: 517
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 2)]
		public struct <unknown1>e__FixedBuffer
		{
			// Token: 0x04000B7C RID: 2940
			public byte FixedElementField;
		}

		// Token: 0x02000206 RID: 518
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 23)]
		public struct <unknown2>e__FixedBuffer
		{
			// Token: 0x04000B7D RID: 2941
			public byte FixedElementField;
		}
	}
}
