using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000162 RID: 354
	public struct VirtualMeshTransform
	{
		// Token: 0x0600069B RID: 1691 RVA: 0x00039C30 File Offset: 0x00037E30
		public VirtualMeshTransform(Transform t)
		{
			this.name = t.name.Substring(0, math.min(t.name.Length, 29));
			this.index = -1;
			this.localToWorldMatrix = t.localToWorldMatrix;
			this.worldToLocalMatrix = t.worldToLocalMatrix;
			this.parentIndex = -1;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00039C96 File Offset: 0x00037E96
		public VirtualMeshTransform(Transform t, int index)
		{
			this = new VirtualMeshTransform(t);
			this.index = index;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00039CA8 File Offset: 0x00037EA8
		public VirtualMeshTransform Clone()
		{
			return new VirtualMeshTransform
			{
				name = this.name,
				index = this.index,
				localToWorldMatrix = this.localToWorldMatrix,
				worldToLocalMatrix = this.worldToLocalMatrix,
				parentIndex = this.parentIndex
			};
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x00039D00 File Offset: 0x00037F00
		public static VirtualMeshTransform Origin
		{
			get
			{
				return new VirtualMeshTransform
				{
					name = "VirtualMesh Origin",
					localToWorldMatrix = float4x4.identity,
					worldToLocalMatrix = float4x4.identity,
					parentIndex = -1
				};
			}
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00039D47 File Offset: 0x00037F47
		public override int GetHashCode()
		{
			return this.name.GetHashCode();
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00039D5A File Offset: 0x00037F5A
		public void Update(Transform t)
		{
			this.localToWorldMatrix = t.localToWorldMatrix;
			this.worldToLocalMatrix = t.worldToLocalMatrix;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00039D7E File Offset: 0x00037F7E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3 TransformPoint(float3 pos)
		{
			return math.transform(this.localToWorldMatrix, pos);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00039D8C File Offset: 0x00037F8C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3 TransformVector(float3 vec)
		{
			return math.mul(this.localToWorldMatrix, new float4(vec, 0f)).xyz;
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00039DB8 File Offset: 0x00037FB8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3 TransformDirection(float3 dir)
		{
			float num = math.length(dir);
			if (num > 0f)
			{
				return math.normalize(this.TransformVector(dir)) * num;
			}
			return dir;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00039DE8 File Offset: 0x00037FE8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3 InverseTransformPoint(float3 pos)
		{
			return math.transform(this.worldToLocalMatrix, pos);
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00039DF8 File Offset: 0x00037FF8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3 InverseTransformVector(float3 vec)
		{
			return math.mul(this.worldToLocalMatrix, new float4(vec, 0f)).xyz;
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00039E24 File Offset: 0x00038024
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3 InverseTransformDirection(float3 dir)
		{
			float num = math.length(dir);
			if (num > 0f)
			{
				return math.normalize(this.InverseTransformVector(dir)) * num;
			}
			return dir;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00039E54 File Offset: 0x00038054
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public quaternion InverseTransformRotation(quaternion rot)
		{
			return math.mul(new quaternion(this.worldToLocalMatrix), rot);
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00039E68 File Offset: 0x00038068
		public VirtualMeshTransform Transform(in VirtualMeshTransform to)
		{
			return new VirtualMeshTransform
			{
				name = "__(temporary)__",
				index = -1,
				localToWorldMatrix = math.mul(to.worldToLocalMatrix, this.localToWorldMatrix),
				worldToLocalMatrix = math.mul(this.worldToLocalMatrix, to.localToWorldMatrix),
				parentIndex = -1
			};
		}

		// Token: 0x04000A9C RID: 2716
		public FixedString32Bytes name;

		// Token: 0x04000A9D RID: 2717
		public int index;

		// Token: 0x04000A9E RID: 2718
		public float4x4 localToWorldMatrix;

		// Token: 0x04000A9F RID: 2719
		public float4x4 worldToLocalMatrix;

		// Token: 0x04000AA0 RID: 2720
		public int parentIndex;
	}
}
