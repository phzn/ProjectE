using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x0200003F RID: 63
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct RigidTransform
	{
		// Token: 0x06001EB9 RID: 7865 RVA: 0x00059BC3 File Offset: 0x00057DC3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public RigidTransform(quaternion rotation, float3 translation)
		{
			this.rot = rotation;
			this.pos = translation;
		}

		// Token: 0x06001EBA RID: 7866 RVA: 0x00059BD3 File Offset: 0x00057DD3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public RigidTransform(float3x3 rotation, float3 translation)
		{
			this.rot = new quaternion(rotation);
			this.pos = translation;
		}

		// Token: 0x06001EBB RID: 7867 RVA: 0x00059BE8 File Offset: 0x00057DE8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public RigidTransform(float4x4 transform)
		{
			this.rot = new quaternion(transform);
			this.pos = transform.c3.xyz;
		}

		// Token: 0x06001EBC RID: 7868 RVA: 0x00059C08 File Offset: 0x00057E08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform AxisAngle(float3 axis, float angle)
		{
			return new RigidTransform(quaternion.AxisAngle(axis, angle), float3.zero);
		}

		// Token: 0x06001EBD RID: 7869 RVA: 0x00059C1B File Offset: 0x00057E1B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform EulerXYZ(float3 xyz)
		{
			return new RigidTransform(quaternion.EulerXYZ(xyz), float3.zero);
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x00059C2D File Offset: 0x00057E2D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform EulerXZY(float3 xyz)
		{
			return new RigidTransform(quaternion.EulerXZY(xyz), float3.zero);
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x00059C3F File Offset: 0x00057E3F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform EulerYXZ(float3 xyz)
		{
			return new RigidTransform(quaternion.EulerYXZ(xyz), float3.zero);
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x00059C51 File Offset: 0x00057E51
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform EulerYZX(float3 xyz)
		{
			return new RigidTransform(quaternion.EulerYZX(xyz), float3.zero);
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x00059C63 File Offset: 0x00057E63
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform EulerZXY(float3 xyz)
		{
			return new RigidTransform(quaternion.EulerZXY(xyz), float3.zero);
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x00059C75 File Offset: 0x00057E75
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform EulerZYX(float3 xyz)
		{
			return new RigidTransform(quaternion.EulerZYX(xyz), float3.zero);
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x00059C87 File Offset: 0x00057E87
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform EulerXYZ(float x, float y, float z)
		{
			return RigidTransform.EulerXYZ(math.float3(x, y, z));
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x00059C96 File Offset: 0x00057E96
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform EulerXZY(float x, float y, float z)
		{
			return RigidTransform.EulerXZY(math.float3(x, y, z));
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x00059CA5 File Offset: 0x00057EA5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform EulerYXZ(float x, float y, float z)
		{
			return RigidTransform.EulerYXZ(math.float3(x, y, z));
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x00059CB4 File Offset: 0x00057EB4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform EulerYZX(float x, float y, float z)
		{
			return RigidTransform.EulerYZX(math.float3(x, y, z));
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x00059CC3 File Offset: 0x00057EC3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform EulerZXY(float x, float y, float z)
		{
			return RigidTransform.EulerZXY(math.float3(x, y, z));
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x00059CD2 File Offset: 0x00057ED2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform EulerZYX(float x, float y, float z)
		{
			return RigidTransform.EulerZYX(math.float3(x, y, z));
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x00059CE4 File Offset: 0x00057EE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform Euler(float3 xyz, math.RotationOrder order = math.RotationOrder.ZXY)
		{
			switch (order)
			{
			case math.RotationOrder.XYZ:
				return RigidTransform.EulerXYZ(xyz);
			case math.RotationOrder.XZY:
				return RigidTransform.EulerXZY(xyz);
			case math.RotationOrder.YXZ:
				return RigidTransform.EulerYXZ(xyz);
			case math.RotationOrder.YZX:
				return RigidTransform.EulerYZX(xyz);
			case math.RotationOrder.ZXY:
				return RigidTransform.EulerZXY(xyz);
			case math.RotationOrder.ZYX:
				return RigidTransform.EulerZYX(xyz);
			default:
				return RigidTransform.identity;
			}
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x00059D40 File Offset: 0x00057F40
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform Euler(float x, float y, float z, math.RotationOrder order = math.RotationOrder.ZXY)
		{
			return RigidTransform.Euler(math.float3(x, y, z), order);
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x00059D50 File Offset: 0x00057F50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform RotateX(float angle)
		{
			return new RigidTransform(quaternion.RotateX(angle), float3.zero);
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x00059D62 File Offset: 0x00057F62
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform RotateY(float angle)
		{
			return new RigidTransform(quaternion.RotateY(angle), float3.zero);
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x00059D74 File Offset: 0x00057F74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform RotateZ(float angle)
		{
			return new RigidTransform(quaternion.RotateZ(angle), float3.zero);
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x00059D86 File Offset: 0x00057F86
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform Translate(float3 vector)
		{
			return new RigidTransform(quaternion.identity, vector);
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x00059D93 File Offset: 0x00057F93
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(RigidTransform x)
		{
			return this.rot.Equals(x.rot) && this.pos.Equals(x.pos);
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x00059DBC File Offset: 0x00057FBC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object x)
		{
			if (x is RigidTransform)
			{
				RigidTransform rigidTransform = (RigidTransform)x;
				return this.Equals(rigidTransform);
			}
			return false;
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x00059DE1 File Offset: 0x00057FE1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x00059DF0 File Offset: 0x00057FF0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("RigidTransform(({0}f, {1}f, {2}f, {3}f),  ({4}f, {5}f, {6}f))", new object[]
			{
				this.rot.value.x,
				this.rot.value.y,
				this.rot.value.z,
				this.rot.value.w,
				this.pos.x,
				this.pos.y,
				this.pos.z
			});
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x00059EA8 File Offset: 0x000580A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("float4x4(({0}f, {1}f, {2}f, {3}f),  ({4}f, {5}f, {6}f))", new object[]
			{
				this.rot.value.x.ToString(format, formatProvider),
				this.rot.value.y.ToString(format, formatProvider),
				this.rot.value.z.ToString(format, formatProvider),
				this.rot.value.w.ToString(format, formatProvider),
				this.pos.x.ToString(format, formatProvider),
				this.pos.y.ToString(format, formatProvider),
				this.pos.z.ToString(format, formatProvider)
			});
		}

		// Token: 0x040000F3 RID: 243
		public quaternion rot;

		// Token: 0x040000F4 RID: 244
		public float3 pos;

		// Token: 0x040000F5 RID: 245
		public static readonly RigidTransform identity = new RigidTransform(new quaternion(0f, 0f, 0f, 1f), new float3(0f, 0f, 0f));
	}
}
