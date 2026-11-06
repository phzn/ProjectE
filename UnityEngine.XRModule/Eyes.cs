using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR
{
	// Token: 0x02000015 RID: 21
	[NativeConditional("ENABLE_VR")]
	[NativeHeader("Modules/XR/XRPrefix.h")]
	[NativeHeader("XRScriptingClasses.h")]
	[NativeHeader("Modules/XR/Subsystems/Input/Public/XRInputDevices.h")]
	[StaticAccessor("XRInputDevices::Get()", StaticAccessorType.Dot)]
	[RequiredByNativeCode]
	public struct Eyes : IEquatable<Eyes>
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003940 File Offset: 0x00001B40
		internal ulong deviceId
		{
			get
			{
				return this.m_DeviceId;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00003958 File Offset: 0x00001B58
		internal uint featureIndex
		{
			get
			{
				return this.m_FeatureIndex;
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003970 File Offset: 0x00001B70
		public bool TryGetLeftEyePosition(out Vector3 position)
		{
			return Eyes.Eyes_TryGetEyePosition(this, EyeSide.Left, out position);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003990 File Offset: 0x00001B90
		public bool TryGetRightEyePosition(out Vector3 position)
		{
			return Eyes.Eyes_TryGetEyePosition(this, EyeSide.Right, out position);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000039B0 File Offset: 0x00001BB0
		public bool TryGetLeftEyeRotation(out Quaternion rotation)
		{
			return Eyes.Eyes_TryGetEyeRotation(this, EyeSide.Left, out rotation);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000039D0 File Offset: 0x00001BD0
		public bool TryGetRightEyeRotation(out Quaternion rotation)
		{
			return Eyes.Eyes_TryGetEyeRotation(this, EyeSide.Right, out rotation);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000039EF File Offset: 0x00001BEF
		private static bool Eyes_TryGetEyePosition(Eyes eyes, EyeSide chirality, out Vector3 position)
		{
			return Eyes.Eyes_TryGetEyePosition_Injected(ref eyes, chirality, out position);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000039FA File Offset: 0x00001BFA
		private static bool Eyes_TryGetEyeRotation(Eyes eyes, EyeSide chirality, out Quaternion rotation)
		{
			return Eyes.Eyes_TryGetEyeRotation_Injected(ref eyes, chirality, out rotation);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003A08 File Offset: 0x00001C08
		public bool TryGetFixationPoint(out Vector3 fixationPoint)
		{
			return Eyes.Eyes_TryGetFixationPoint(this, out fixationPoint);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003A26 File Offset: 0x00001C26
		private static bool Eyes_TryGetFixationPoint(Eyes eyes, out Vector3 fixationPoint)
		{
			return Eyes.Eyes_TryGetFixationPoint_Injected(ref eyes, out fixationPoint);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003A30 File Offset: 0x00001C30
		public bool TryGetLeftEyeOpenAmount(out float openAmount)
		{
			return Eyes.Eyes_TryGetEyeOpenAmount(this, EyeSide.Left, out openAmount);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003A50 File Offset: 0x00001C50
		public bool TryGetRightEyeOpenAmount(out float openAmount)
		{
			return Eyes.Eyes_TryGetEyeOpenAmount(this, EyeSide.Right, out openAmount);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003A6F File Offset: 0x00001C6F
		private static bool Eyes_TryGetEyeOpenAmount(Eyes eyes, EyeSide chirality, out float openAmount)
		{
			return Eyes.Eyes_TryGetEyeOpenAmount_Injected(ref eyes, chirality, out openAmount);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003A7C File Offset: 0x00001C7C
		public override bool Equals(object obj)
		{
			bool flag = !(obj is Eyes);
			return !flag && this.Equals((Eyes)obj);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003AB0 File Offset: 0x00001CB0
		public bool Equals(Eyes other)
		{
			return this.deviceId == other.deviceId && this.featureIndex == other.featureIndex;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003AE4 File Offset: 0x00001CE4
		public override int GetHashCode()
		{
			return this.deviceId.GetHashCode() ^ (this.featureIndex.GetHashCode() << 1);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003B18 File Offset: 0x00001D18
		public static bool operator ==(Eyes a, Eyes b)
		{
			return a.Equals(b);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003B34 File Offset: 0x00001D34
		public static bool operator !=(Eyes a, Eyes b)
		{
			return !(a == b);
		}

		// Token: 0x0600009C RID: 156
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Eyes_TryGetEyePosition_Injected(ref Eyes eyes, EyeSide chirality, out Vector3 position);

		// Token: 0x0600009D RID: 157
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Eyes_TryGetEyeRotation_Injected(ref Eyes eyes, EyeSide chirality, out Quaternion rotation);

		// Token: 0x0600009E RID: 158
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Eyes_TryGetFixationPoint_Injected(ref Eyes eyes, out Vector3 fixationPoint);

		// Token: 0x0600009F RID: 159
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Eyes_TryGetEyeOpenAmount_Injected(ref Eyes eyes, EyeSide chirality, out float openAmount);

		// Token: 0x040000A8 RID: 168
		private ulong m_DeviceId;

		// Token: 0x040000A9 RID: 169
		private uint m_FeatureIndex;
	}
}
