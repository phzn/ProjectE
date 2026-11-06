using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR
{
	// Token: 0x02000016 RID: 22
	[RequiredByNativeCode]
	[NativeConditional("ENABLE_VR")]
	[NativeHeader("Modules/XR/XRPrefix.h")]
	[NativeHeader("XRScriptingClasses.h")]
	[NativeHeader("Modules/XR/Subsystems/Input/Public/XRInputDevices.h")]
	[StaticAccessor("XRInputDevices::Get()", StaticAccessorType.Dot)]
	public struct Bone : IEquatable<Bone>
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00003B50 File Offset: 0x00001D50
		internal ulong deviceId
		{
			get
			{
				return this.m_DeviceId;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00003B68 File Offset: 0x00001D68
		internal uint featureIndex
		{
			get
			{
				return this.m_FeatureIndex;
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003B80 File Offset: 0x00001D80
		public bool TryGetPosition(out Vector3 position)
		{
			return Bone.Bone_TryGetPosition(this, out position);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003B9E File Offset: 0x00001D9E
		private static bool Bone_TryGetPosition(Bone bone, out Vector3 position)
		{
			return Bone.Bone_TryGetPosition_Injected(ref bone, out position);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003BA8 File Offset: 0x00001DA8
		public bool TryGetRotation(out Quaternion rotation)
		{
			return Bone.Bone_TryGetRotation(this, out rotation);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003BC6 File Offset: 0x00001DC6
		private static bool Bone_TryGetRotation(Bone bone, out Quaternion rotation)
		{
			return Bone.Bone_TryGetRotation_Injected(ref bone, out rotation);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00003BD0 File Offset: 0x00001DD0
		public bool TryGetParentBone(out Bone parentBone)
		{
			return Bone.Bone_TryGetParentBone(this, out parentBone);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00003BEE File Offset: 0x00001DEE
		private static bool Bone_TryGetParentBone(Bone bone, out Bone parentBone)
		{
			return Bone.Bone_TryGetParentBone_Injected(ref bone, out parentBone);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003BF8 File Offset: 0x00001DF8
		public bool TryGetChildBones(List<Bone> childBones)
		{
			return Bone.Bone_TryGetChildBones(this, childBones);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00003C16 File Offset: 0x00001E16
		private static bool Bone_TryGetChildBones(Bone bone, [NotNull("ArgumentNullException")] List<Bone> childBones)
		{
			return Bone.Bone_TryGetChildBones_Injected(ref bone, childBones);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003C20 File Offset: 0x00001E20
		public override bool Equals(object obj)
		{
			bool flag = !(obj is Bone);
			return !flag && this.Equals((Bone)obj);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003C54 File Offset: 0x00001E54
		public bool Equals(Bone other)
		{
			return this.deviceId == other.deviceId && this.featureIndex == other.featureIndex;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003C88 File Offset: 0x00001E88
		public override int GetHashCode()
		{
			return this.deviceId.GetHashCode() ^ (this.featureIndex.GetHashCode() << 1);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003CBC File Offset: 0x00001EBC
		public static bool operator ==(Bone a, Bone b)
		{
			return a.Equals(b);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00003CD8 File Offset: 0x00001ED8
		public static bool operator !=(Bone a, Bone b)
		{
			return !(a == b);
		}

		// Token: 0x060000AF RID: 175
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Bone_TryGetPosition_Injected(ref Bone bone, out Vector3 position);

		// Token: 0x060000B0 RID: 176
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Bone_TryGetRotation_Injected(ref Bone bone, out Quaternion rotation);

		// Token: 0x060000B1 RID: 177
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Bone_TryGetParentBone_Injected(ref Bone bone, out Bone parentBone);

		// Token: 0x060000B2 RID: 178
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Bone_TryGetChildBones_Injected(ref Bone bone, List<Bone> childBones);

		// Token: 0x040000AA RID: 170
		private ulong m_DeviceId;

		// Token: 0x040000AB RID: 171
		private uint m_FeatureIndex;
	}
}
