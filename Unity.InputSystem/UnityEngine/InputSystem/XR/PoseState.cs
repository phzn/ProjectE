using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.XR;

namespace UnityEngine.InputSystem.XR
{
	// Token: 0x02000063 RID: 99
	[StructLayout(LayoutKind.Explicit, Size = 60)]
	public struct PoseState : IInputStateTypeInfo
	{
		// Token: 0x17000279 RID: 633
		// (get) Token: 0x060009CB RID: 2507 RVA: 0x00036BED File Offset: 0x00034DED
		public FourCC format
		{
			get
			{
				return PoseState.s_Format;
			}
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x00036BF4 File Offset: 0x00034DF4
		public PoseState(bool isTracked, InputTrackingState trackingState, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
		{
			this.isTracked = isTracked;
			this.trackingState = trackingState;
			this.position = position;
			this.rotation = rotation;
			this.velocity = velocity;
			this.angularVelocity = angularVelocity;
		}

		// Token: 0x04000339 RID: 825
		internal const int kSizeInBytes = 60;

		// Token: 0x0400033A RID: 826
		internal static readonly FourCC s_Format = new FourCC('P', 'o', 's', 'e');

		// Token: 0x0400033B RID: 827
		[InputControl(displayName = "Is Tracked", layout = "Button", sizeInBits = 8U)]
		[FieldOffset(0)]
		public bool isTracked;

		// Token: 0x0400033C RID: 828
		[InputControl(displayName = "Tracking State", layout = "Integer")]
		[FieldOffset(4)]
		public InputTrackingState trackingState;

		// Token: 0x0400033D RID: 829
		[InputControl(displayName = "Position", noisy = true)]
		[FieldOffset(8)]
		public Vector3 position;

		// Token: 0x0400033E RID: 830
		[InputControl(displayName = "Rotation", noisy = true)]
		[FieldOffset(20)]
		public Quaternion rotation;

		// Token: 0x0400033F RID: 831
		[InputControl(displayName = "Velocity", noisy = true)]
		[FieldOffset(36)]
		public Vector3 velocity;

		// Token: 0x04000340 RID: 832
		[InputControl(displayName = "Angular Velocity", noisy = true)]
		[FieldOffset(48)]
		public Vector3 angularVelocity;
	}
}
