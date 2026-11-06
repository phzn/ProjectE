using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics.Geometry
{
	// Token: 0x0200004F RID: 79
	[DebuggerDisplay("{Normal}, {Distance}")]
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct Plane
	{
		// Token: 0x060024B1 RID: 9393 RVA: 0x00068F24 File Offset: 0x00067124
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Plane(float coefficientA, float coefficientB, float coefficientC, float coefficientD)
		{
			this.NormalAndDistance = Plane.Normalize(new float4(coefficientA, coefficientB, coefficientC, coefficientD));
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x00068F3B File Offset: 0x0006713B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Plane(float3 normal, float distance)
		{
			this.NormalAndDistance = Plane.Normalize(new float4(normal, distance));
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x00068F4F File Offset: 0x0006714F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Plane(float3 normal, float3 pointInPlane)
		{
			this = new Plane(normal, -math.dot(normal, pointInPlane));
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x00068F60 File Offset: 0x00067160
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Plane(float3 vector1InPlane, float3 vector2InPlane, float3 pointInPlane)
		{
			this = new Plane(math.cross(vector1InPlane, vector2InPlane), pointInPlane);
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x00068F70 File Offset: 0x00067170
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Plane CreateFromUnitNormalAndDistance(float3 unitNormal, float distance)
		{
			return new Plane
			{
				NormalAndDistance = new float4(unitNormal, distance)
			};
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x00068F94 File Offset: 0x00067194
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Plane CreateFromUnitNormalAndPointInPlane(float3 unitNormal, float3 pointInPlane)
		{
			return new Plane
			{
				NormalAndDistance = new float4(unitNormal, -math.dot(unitNormal, pointInPlane))
			};
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x060024B7 RID: 9399 RVA: 0x00068FBF File Offset: 0x000671BF
		// (set) Token: 0x060024B8 RID: 9400 RVA: 0x00068FCC File Offset: 0x000671CC
		public float3 Normal
		{
			get
			{
				return this.NormalAndDistance.xyz;
			}
			set
			{
				this.NormalAndDistance.xyz = value;
			}
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x060024B9 RID: 9401 RVA: 0x00068FDA File Offset: 0x000671DA
		// (set) Token: 0x060024BA RID: 9402 RVA: 0x00068FE7 File Offset: 0x000671E7
		public float Distance
		{
			get
			{
				return this.NormalAndDistance.w;
			}
			set
			{
				this.NormalAndDistance.w = value;
			}
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x00068FF8 File Offset: 0x000671F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Plane Normalize(Plane plane)
		{
			return new Plane
			{
				NormalAndDistance = Plane.Normalize(plane.NormalAndDistance)
			};
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x00069020 File Offset: 0x00067220
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 Normalize(float4 planeCoefficients)
		{
			float num = math.rsqrt(math.lengthsq(planeCoefficients.xyz));
			return new Plane
			{
				NormalAndDistance = planeCoefficients * num
			};
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x0006905B File Offset: 0x0006725B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float SignedDistanceToPoint(float3 point)
		{
			return math.dot(this.NormalAndDistance, new float4(point, 1f));
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x00069073 File Offset: 0x00067273
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3 Projection(float3 point)
		{
			return point - this.Normal * this.SignedDistanceToPoint(point);
		}

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x060024BF RID: 9407 RVA: 0x00069090 File Offset: 0x00067290
		public Plane Flipped
		{
			get
			{
				return new Plane
				{
					NormalAndDistance = -this.NormalAndDistance
				};
			}
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x000690B8 File Offset: 0x000672B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4(Plane plane)
		{
			return plane.NormalAndDistance;
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x000690C0 File Offset: 0x000672C0
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private void CheckPlaneIsNormalized()
		{
			float num = math.lengthsq(this.Normal.xyz);
			if (num < 0.99800104f || num > 1.002001f)
			{
				throw new ArgumentException("Plane must be normalized. Call Plane.Normalize() to normalize plane.");
			}
		}

		// Token: 0x0400012F RID: 303
		public float4 NormalAndDistance;
	}
}
