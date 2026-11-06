using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR
{
	// Token: 0x02000013 RID: 19
	[NativeHeader("Modules/XR/XRPrefix.h")]
	[RequiredByNativeCode]
	[StaticAccessor("XRInputDevices::Get()", StaticAccessorType.Dot)]
	[NativeHeader("Modules/XR/Subsystems/Input/Public/XRInputDevices.h")]
	[NativeHeader("XRScriptingClasses.h")]
	[NativeConditional("ENABLE_VR")]
	public struct Hand : IEquatable<Hand>
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600007D RID: 125 RVA: 0x000037D4 File Offset: 0x000019D4
		internal ulong deviceId
		{
			get
			{
				return this.m_DeviceId;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600007E RID: 126 RVA: 0x000037EC File Offset: 0x000019EC
		internal uint featureIndex
		{
			get
			{
				return this.m_FeatureIndex;
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003804 File Offset: 0x00001A04
		public bool TryGetRootBone(out Bone boneOut)
		{
			return Hand.Hand_TryGetRootBone(this, out boneOut);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003822 File Offset: 0x00001A22
		private static bool Hand_TryGetRootBone(Hand hand, out Bone boneOut)
		{
			return Hand.Hand_TryGetRootBone_Injected(ref hand, out boneOut);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000382C File Offset: 0x00001A2C
		public bool TryGetFingerBones(HandFinger finger, List<Bone> bonesOut)
		{
			bool flag = bonesOut == null;
			if (flag)
			{
				throw new ArgumentNullException("bonesOut");
			}
			return Hand.Hand_TryGetFingerBonesAsList(this, finger, bonesOut);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000385E File Offset: 0x00001A5E
		private static bool Hand_TryGetFingerBonesAsList(Hand hand, HandFinger finger, [NotNull("ArgumentNullException")] List<Bone> bonesOut)
		{
			return Hand.Hand_TryGetFingerBonesAsList_Injected(ref hand, finger, bonesOut);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000386C File Offset: 0x00001A6C
		public override bool Equals(object obj)
		{
			bool flag = !(obj is Hand);
			return !flag && this.Equals((Hand)obj);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000038A0 File Offset: 0x00001AA0
		public bool Equals(Hand other)
		{
			return this.deviceId == other.deviceId && this.featureIndex == other.featureIndex;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000038D4 File Offset: 0x00001AD4
		public override int GetHashCode()
		{
			return this.deviceId.GetHashCode() ^ (this.featureIndex.GetHashCode() << 1);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003908 File Offset: 0x00001B08
		public static bool operator ==(Hand a, Hand b)
		{
			return a.Equals(b);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003924 File Offset: 0x00001B24
		public static bool operator !=(Hand a, Hand b)
		{
			return !(a == b);
		}

		// Token: 0x06000088 RID: 136
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Hand_TryGetRootBone_Injected(ref Hand hand, out Bone boneOut);

		// Token: 0x06000089 RID: 137
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Hand_TryGetFingerBonesAsList_Injected(ref Hand hand, HandFinger finger, List<Bone> bonesOut);

		// Token: 0x040000A3 RID: 163
		private ulong m_DeviceId;

		// Token: 0x040000A4 RID: 164
		private uint m_FeatureIndex;
	}
}
