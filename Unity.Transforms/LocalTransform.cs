using System;
using System.Globalization;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Properties;

namespace Unity.Transforms
{
	// Token: 0x02000016 RID: 22
	[BurstCompile]
	public struct LocalTransform : IComponentData, IQueryTypeParameter
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00003558 File Offset: 0x00001758
		public static LocalTransform FromMatrix(float4x4 matrix)
		{
			float3 xyz = matrix.c3.xyz;
			float num = math.length(matrix.c0.xyz);
			float num2 = math.length(matrix.c1.xyz);
			float num3 = math.length(matrix.c2.xyz);
			float num4 = math.max(num, math.max(num2, num3));
			float3x3 float3x = math.orthonormalize(new float3x3(matrix));
			quaternion quaternion = new quaternion(float3x);
			return new LocalTransform
			{
				Position = xyz,
				Scale = num4,
				Rotation = quaternion
			};
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000035F0 File Offset: 0x000017F0
		public static LocalTransform FromMatrixSafe(float4x4 matrix)
		{
			float num = 0.001f;
			float num2 = num * num;
			float num3 = math.lengthsq(matrix.c0.xyz);
			float num4 = math.lengthsq(matrix.c1.xyz);
			float num5 = math.lengthsq(matrix.c2.xyz);
			if (math.abs(num3 - num4) > num2 || math.abs(num3 - num5) > num2)
			{
				throw new ArgumentException("Trying to convert a float4x4 to a LocalTransform, but the scale is not uniform");
			}
			float num6 = math.sqrt(num3);
			float num7 = math.sqrt(num4);
			float num8 = math.sqrt(num5);
			float num9 = math.max(num6, math.max(num7, num8));
			float3x3 float3x = new float3x3(matrix);
			float num10 = math.dot(float3x.c0, float3x.c1);
			float num11 = math.dot(float3x.c0, float3x.c2);
			float num12 = math.dot(float3x.c1, float3x.c2);
			if (math.abs(num10) > num2 || math.abs(num11) > num2 || math.abs(num12) > num2)
			{
				throw new ArgumentException("Trying to convert a float4x4 to a LocalTransform, but the rotation 3x3 is not orthogonal");
			}
			float3x3 float3x2 = math.orthonormalize(float3x);
			quaternion quaternion = new quaternion(float3x2);
			float3 xyz = matrix.c3.xyz;
			return new LocalTransform
			{
				Position = xyz,
				Scale = num9,
				Rotation = quaternion
			};
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003734 File Offset: 0x00001934
		public static LocalTransform FromPositionRotation(float3 position, quaternion rotation)
		{
			return new LocalTransform
			{
				Position = position,
				Scale = 1f,
				Rotation = rotation
			};
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003768 File Offset: 0x00001968
		public static LocalTransform FromPositionRotationScale(float3 position, quaternion rotation, float scale)
		{
			return new LocalTransform
			{
				Position = position,
				Scale = scale,
				Rotation = rotation
			};
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003798 File Offset: 0x00001998
		public static LocalTransform FromPosition(float3 position)
		{
			return new LocalTransform
			{
				Position = position,
				Scale = 1f,
				Rotation = quaternion.identity
			};
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000037D0 File Offset: 0x000019D0
		public static LocalTransform FromPosition(float x, float y, float z)
		{
			return new LocalTransform
			{
				Position = new float3(x, y, z),
				Scale = 1f,
				Rotation = quaternion.identity
			};
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003810 File Offset: 0x00001A10
		public static LocalTransform FromRotation(quaternion rotation)
		{
			return new LocalTransform
			{
				Position = float3.zero,
				Scale = 1f,
				Rotation = rotation
			};
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003848 File Offset: 0x00001A48
		public static LocalTransform FromScale(float scale)
		{
			return new LocalTransform
			{
				Position = float3.zero,
				Scale = scale,
				Rotation = quaternion.identity
			};
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003880 File Offset: 0x00001A80
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Position=",
				this.Position.ToString(),
				" Rotation=",
				this.Rotation.ToString(),
				" Scale=",
				this.Scale.ToString(CultureInfo.InvariantCulture)
			});
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000038EB File Offset: 0x00001AEB
		public float3 Right()
		{
			return this.TransformDirection(math.right());
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000038F8 File Offset: 0x00001AF8
		public float3 Up()
		{
			return this.TransformDirection(math.up());
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003905 File Offset: 0x00001B05
		public float3 Forward()
		{
			return this.TransformDirection(math.forward());
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003912 File Offset: 0x00001B12
		public float3 TransformPoint(float3 point)
		{
			return this.Position + math.rotate(this.Rotation, point) * this.Scale;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003936 File Offset: 0x00001B36
		public float3 InverseTransformPoint(float3 point)
		{
			return math.rotate(math.conjugate(this.Rotation), point - this.Position) / this.Scale;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000395F File Offset: 0x00001B5F
		public float3 TransformDirection(float3 direction)
		{
			return math.rotate(this.Rotation, direction);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000396D File Offset: 0x00001B6D
		public float3 InverseTransformDirection(float3 direction)
		{
			return math.rotate(math.conjugate(this.Rotation), direction);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003980 File Offset: 0x00001B80
		public quaternion TransformRotation(quaternion rotation)
		{
			return math.mul(this.Rotation, rotation);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000398E File Offset: 0x00001B8E
		public quaternion InverseTransformRotation(quaternion rotation)
		{
			return math.mul(math.conjugate(this.Rotation), rotation);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000039A1 File Offset: 0x00001BA1
		public float TransformScale(float scale)
		{
			return scale * this.Scale;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000039AB File Offset: 0x00001BAB
		public float InverseTransformScale(float scale)
		{
			return scale / this.Scale;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000039B8 File Offset: 0x00001BB8
		public LocalTransform TransformTransform(in LocalTransform transformData)
		{
			return new LocalTransform
			{
				Position = this.TransformPoint(transformData.Position),
				Scale = this.TransformScale(transformData.Scale),
				Rotation = this.TransformRotation(transformData.Rotation)
			};
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003A08 File Offset: 0x00001C08
		public LocalTransform InverseTransformTransform(in LocalTransform transformData)
		{
			return new LocalTransform
			{
				Position = this.InverseTransformPoint(transformData.Position),
				Scale = this.InverseTransformScale(transformData.Scale),
				Rotation = this.InverseTransformRotation(transformData.Rotation)
			};
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003A58 File Offset: 0x00001C58
		public LocalTransform Inverse()
		{
			quaternion quaternion = math.conjugate(this.Rotation);
			float num = 1f / this.Scale;
			return new LocalTransform
			{
				Position = -math.rotate(quaternion, this.Position) * num,
				Scale = num,
				Rotation = quaternion
			};
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003AB5 File Offset: 0x00001CB5
		public float4x4 ToMatrix()
		{
			return float4x4.TRS(this.Position, this.Rotation, this.Scale);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003AD4 File Offset: 0x00001CD4
		public float4x4 ToInverseMatrix()
		{
			return this.Inverse().ToMatrix();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003AF0 File Offset: 0x00001CF0
		public LocalTransform WithPosition(float3 position)
		{
			return new LocalTransform
			{
				Position = position,
				Scale = this.Scale,
				Rotation = this.Rotation
			};
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003B28 File Offset: 0x00001D28
		public LocalTransform WithPosition(float x, float y, float z)
		{
			return new LocalTransform
			{
				Position = new float3(x, y, z),
				Scale = this.Scale,
				Rotation = this.Rotation
			};
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003B68 File Offset: 0x00001D68
		public LocalTransform WithRotation(quaternion rotation)
		{
			return new LocalTransform
			{
				Position = this.Position,
				Scale = this.Scale,
				Rotation = rotation
			};
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003BA0 File Offset: 0x00001DA0
		public LocalTransform WithScale(float scale)
		{
			return new LocalTransform
			{
				Position = this.Position,
				Scale = scale,
				Rotation = this.Rotation
			};
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003BD8 File Offset: 0x00001DD8
		public LocalTransform Translate(float3 translation)
		{
			return new LocalTransform
			{
				Position = this.Position + translation,
				Scale = this.Scale,
				Rotation = this.Rotation
			};
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003C1C File Offset: 0x00001E1C
		public LocalTransform ApplyScale(float scale)
		{
			return new LocalTransform
			{
				Position = this.Position,
				Scale = this.Scale * scale,
				Rotation = this.Rotation
			};
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003C5C File Offset: 0x00001E5C
		public LocalTransform Rotate(quaternion rotation)
		{
			return new LocalTransform
			{
				Position = this.Position,
				Scale = this.Scale,
				Rotation = math.mul(this.Rotation, rotation)
			};
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003C9F File Offset: 0x00001E9F
		public LocalTransform RotateX(float angle)
		{
			return this.Rotate(quaternion.RotateX(angle));
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003CAD File Offset: 0x00001EAD
		public LocalTransform RotateY(float angle)
		{
			return this.Rotate(quaternion.RotateY(angle));
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003CBB File Offset: 0x00001EBB
		public LocalTransform RotateZ(float angle)
		{
			return this.Rotate(quaternion.RotateZ(angle));
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003CC9 File Offset: 0x00001EC9
		public bool Equals(in LocalTransform other)
		{
			return this.Position.Equals(other.Position) && this.Rotation.Equals(other.Rotation) && this.Scale.Equals(other.Scale);
		}

		// Token: 0x04000047 RID: 71
		[CreateProperty]
		public float3 Position;

		// Token: 0x04000048 RID: 72
		[CreateProperty]
		public float Scale;

		// Token: 0x04000049 RID: 73
		[CreateProperty]
		public quaternion Rotation;

		// Token: 0x0400004A RID: 74
		public static readonly LocalTransform Identity = new LocalTransform
		{
			Scale = 1f,
			Rotation = quaternion.identity
		};
	}
}
