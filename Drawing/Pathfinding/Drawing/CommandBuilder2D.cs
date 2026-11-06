using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Drawing
{
	// Token: 0x02000027 RID: 39
	public struct CommandBuilder2D
	{
		// Token: 0x060000F1 RID: 241 RVA: 0x00006587 File Offset: 0x00004787
		public CommandBuilder2D(CommandBuilder draw, bool xy)
		{
			this.draw = draw;
			this.xy = xy;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00006598 File Offset: 0x00004798
		public unsafe void Line(float2 a, float2 b)
		{
			this.draw.Reserve<CommandBuilder.LineData>();
			UnsafeAppendBuffer* buffer = this.draw.buffer;
			int length = buffer->Length;
			int num = length + 4 + 24;
			byte* ptr = buffer->Ptr + length;
			*(int*)ptr = 5;
			CommandBuilder.LineData* ptr2 = (CommandBuilder.LineData*)(ptr + 4);
			if (this.xy)
			{
				ptr2->a = new float3(a, 0f);
				ptr2->b = new float3(b, 0f);
			}
			else
			{
				ptr2->a = new float3(a.x, 0f, a.y);
				ptr2->b = new float3(b.x, 0f, b.y);
			}
			buffer->Length = num;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00006644 File Offset: 0x00004844
		public unsafe void Line(float2 a, float2 b, Color color)
		{
			this.draw.Reserve<Color32, CommandBuilder.LineData>();
			UnsafeAppendBuffer* buffer = this.draw.buffer;
			int length = buffer->Length;
			int num = length + 4 + 24 + 4;
			byte* ptr = buffer->Ptr + length;
			*(int*)ptr = 261;
			*(int*)(ptr + 4) = (int)CommandBuilder.ConvertColor(color);
			CommandBuilder.LineData* ptr2 = (CommandBuilder.LineData*)(ptr + 8);
			if (this.xy)
			{
				ptr2->a = new float3(a, 0f);
				ptr2->b = new float3(b, 0f);
			}
			else
			{
				ptr2->a = new float3(a.x, 0f, a.y);
				ptr2->b = new float3(b.x, 0f, b.y);
			}
			buffer->Length = num;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000066FD File Offset: 0x000048FD
		public void Line(float3 a, float3 b)
		{
			this.draw.Line(a, b);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000670C File Offset: 0x0000490C
		public void Circle(float2 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			this.Circle(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), radius, startAngle, endAngle);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00006744 File Offset: 0x00004944
		public void Circle(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			if (this.xy)
			{
				this.draw.PushMatrix(CommandBuilder2D.XZ_TO_XY_MATRIX);
				this.draw.CircleXZInternal(new float3(center.x, center.z, center.y), radius, startAngle, endAngle);
				this.draw.PopMatrix();
				return;
			}
			this.draw.CircleXZInternal(center, radius, startAngle, endAngle);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000067AB File Offset: 0x000049AB
		public void SolidCircle(float2 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			this.SolidCircle(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), radius, startAngle, endAngle);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000067E4 File Offset: 0x000049E4
		public void SolidCircle(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			if (this.xy)
			{
				this.draw.PushMatrix(CommandBuilder2D.XZ_TO_XY_MATRIX);
			}
			this.draw.SolidCircleXZInternal(this.xy ? new float3(center.x, center.z, center.y) : center, radius, startAngle, endAngle);
			if (this.xy)
			{
				this.draw.PopMatrix();
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000684D File Offset: 0x00004A4D
		public void WirePill(float2 a, float2 b, float radius)
		{
			this.WirePill(a, b - a, math.length(b - a), radius);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000686C File Offset: 0x00004A6C
		public void WirePill(float2 position, float2 direction, float length, float radius)
		{
			direction = math.normalizesafe(direction, default(float2));
			if (radius <= 0f)
			{
				this.Line(position, position + direction * length);
				return;
			}
			if (length <= 0f || math.all(direction == 0f))
			{
				this.Circle(position, radius, 0f, 6.2831855f);
				return;
			}
			float4x4 float4x;
			if (this.xy)
			{
				float4x = new float4x4(new float4(direction, 0f, 0f), new float4(math.cross(new float3(direction, 0f), CommandBuilder2D.XY_UP), 0f), new float4(0f, 0f, 1f, 0f), new float4(position, 0f, 1f));
			}
			else
			{
				float4x = new float4x4(new float4(direction.x, 0f, direction.y, 0f), new float4(0f, 1f, 0f, 0f), new float4(math.cross(new float3(direction.x, 0f, direction.y), CommandBuilder2D.XZ_UP), 0f), new float4(position.x, 0f, position.y, 1f));
			}
			this.draw.PushMatrix(float4x);
			this.Circle(new float2(0f, 0f), radius, 1.5707964f, 4.712389f);
			this.Line(new float2(0f, -radius), new float2(length, -radius));
			this.Circle(new float2(length, 0f), radius, -1.5707964f, 1.5707964f);
			this.Line(new float2(0f, radius), new float2(length, radius));
			this.draw.PopMatrix();
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00006A50 File Offset: 0x00004C50
		[BurstDiscard]
		public void Polyline(List<Vector2> points, bool cycle = false)
		{
			for (int i = 0; i < points.Count - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Count > 1)
			{
				this.Line(points[points.Count - 1], points[0]);
			}
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00006AB0 File Offset: 0x00004CB0
		[BurstDiscard]
		public void Polyline(Vector2[] points, bool cycle = false)
		{
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00006B04 File Offset: 0x00004D04
		[BurstDiscard]
		public void Polyline(float2[] points, bool cycle = false)
		{
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00006B58 File Offset: 0x00004D58
		public void Polyline(NativeArray<float2> points, bool cycle = false)
		{
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00006BBC File Offset: 0x00004DBC
		public void Cross(float2 position, float size = 1f)
		{
			size *= 0.5f;
			this.Line(position - new float2(size, 0f), position + new float2(size, 0f));
			this.Line(position - new float2(0f, size), position + new float2(0f, size));
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00006C22 File Offset: 0x00004E22
		public void WireRectangle(float3 center, float2 size)
		{
			this.draw.WirePlane(center, this.xy ? CommandBuilder2D.XY_TO_XZ_ROTATION : CommandBuilder2D.XZ_TO_XZ_ROTATION, size);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00006C48 File Offset: 0x00004E48
		public void WireRectangle(Rect rect)
		{
			float2 @float = rect.min;
			float2 float2 = rect.max;
			this.Line(new float2(@float.x, @float.y), new float2(float2.x, @float.y));
			this.Line(new float2(float2.x, @float.y), new float2(float2.x, float2.y));
			this.Line(new float2(float2.x, float2.y), new float2(@float.x, float2.y));
			this.Line(new float2(@float.x, float2.y), new float2(@float.x, @float.y));
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00006D10 File Offset: 0x00004F10
		public void SolidRectangle(Rect rect)
		{
			this.draw.SolidPlane(this.xy ? new float3(rect.center.x, rect.center.y, 0f) : new float3(rect.center.x, 0f, rect.center.y), this.xy ? CommandBuilder2D.XY_TO_XZ_ROTATION : CommandBuilder2D.XZ_TO_XZ_ROTATION, new float2(rect.width, rect.height));
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00006DA0 File Offset: 0x00004FA0
		public void WireGrid(float2 center, int2 cells, float2 totalSize)
		{
			this.draw.WireGrid(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), this.xy ? CommandBuilder2D.XY_TO_XZ_ROTATION : CommandBuilder2D.XZ_TO_XZ_ROTATION, cells, totalSize);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00006DF9 File Offset: 0x00004FF9
		public void WireGrid(float3 center, int2 cells, float2 totalSize)
		{
			this.draw.WireGrid(center, this.xy ? CommandBuilder2D.XY_TO_XZ_ROTATION : CommandBuilder2D.XZ_TO_XZ_ROTATION, cells, totalSize);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00006E1D File Offset: 0x0000501D
		[BurstDiscard]
		public CommandBuilder.ScopeMatrix WithMatrix(Matrix4x4 matrix)
		{
			return this.draw.WithMatrix(matrix);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00006E2B File Offset: 0x0000502B
		[BurstDiscard]
		public CommandBuilder.ScopeMatrix WithMatrix(float3x3 matrix)
		{
			return this.draw.WithMatrix(matrix);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00006E39 File Offset: 0x00005039
		[BurstDiscard]
		public CommandBuilder.ScopeColor WithColor(Color color)
		{
			return this.draw.WithColor(color);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00006E47 File Offset: 0x00005047
		[BurstDiscard]
		public CommandBuilder.ScopeLineWidth WithLineWidth(float pixels, bool automaticJoins = true)
		{
			return this.draw.WithLineWidth(pixels, automaticJoins);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00006E56 File Offset: 0x00005056
		public void PushMatrix(Matrix4x4 matrix)
		{
			this.draw.PushMatrix(matrix);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00006E64 File Offset: 0x00005064
		public void PushMatrix(float4x4 matrix)
		{
			this.draw.PushMatrix(matrix);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00006E72 File Offset: 0x00005072
		public void PopMatrix()
		{
			this.draw.PopMatrix();
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00006E7F File Offset: 0x0000507F
		public void Line(Vector3 a, Vector3 b)
		{
			this.draw.Line(a, b);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00006E90 File Offset: 0x00005090
		public void Line(Vector2 a, Vector2 b)
		{
			this.Line(this.xy ? new Vector3(a.x, a.y, 0f) : new Vector3(a.x, 0f, a.y), this.xy ? new Vector3(b.x, b.y, 0f) : new Vector3(b.x, 0f, b.y));
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00006F0F File Offset: 0x0000510F
		public void Line(Vector3 a, Vector3 b, Color color)
		{
			this.draw.Line(a, b, color);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00006F20 File Offset: 0x00005120
		public void Line(Vector2 a, Vector2 b, Color color)
		{
			this.Line(this.xy ? new Vector3(a.x, a.y, 0f) : new Vector3(a.x, 0f, a.y), this.xy ? new Vector3(b.x, b.y, 0f) : new Vector3(b.x, 0f, b.y), color);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00006FA0 File Offset: 0x000051A0
		public void Ray(float3 origin, float3 direction)
		{
			this.draw.Ray(origin, direction);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00006FB0 File Offset: 0x000051B0
		public void Ray(float2 origin, float2 direction)
		{
			this.Ray(this.xy ? new float3(origin, 0f) : new float3(origin.x, 0f, origin.y), this.xy ? new float3(direction, 0f) : new float3(direction.x, 0f, direction.y));
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00007019 File Offset: 0x00005219
		public void Ray(Ray ray, float length)
		{
			this.draw.Ray(ray, length);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00007028 File Offset: 0x00005228
		public void Arc(float3 center, float3 start, float3 end)
		{
			this.draw.Arc(center, start, end);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00007038 File Offset: 0x00005238
		public void Arc(float2 center, float2 start, float2 end)
		{
			this.Arc(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), this.xy ? new float3(start, 0f) : new float3(start.x, 0f, start.y), this.xy ? new float3(end, 0f) : new float3(end.x, 0f, end.y));
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000070CC File Offset: 0x000052CC
		[BurstDiscard]
		public void Polyline(List<Vector3> points, bool cycle = false)
		{
			this.draw.Polyline(points, cycle);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000070DB File Offset: 0x000052DB
		[BurstDiscard]
		public void Polyline(Vector3[] points, bool cycle = false)
		{
			this.draw.Polyline(points, cycle);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000070EA File Offset: 0x000052EA
		[BurstDiscard]
		public void Polyline(float3[] points, bool cycle = false)
		{
			this.draw.Polyline(points, cycle);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000070F9 File Offset: 0x000052F9
		public void Polyline(NativeArray<float3> points, bool cycle = false)
		{
			this.draw.Polyline(points, cycle);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00007108 File Offset: 0x00005308
		public void Cross(float3 position, float size = 1f)
		{
			this.draw.Cross(position, size);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00007117 File Offset: 0x00005317
		public void Bezier(float3 p0, float3 p1, float3 p2, float3 p3)
		{
			this.draw.Bezier(p0, p1, p2, p3);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000712C File Offset: 0x0000532C
		public void Bezier(float2 p0, float2 p1, float2 p2, float2 p3)
		{
			this.Bezier(this.xy ? new float3(p0, 0f) : new float3(p0.x, 0f, p0.y), this.xy ? new float3(p1, 0f) : new float3(p1.x, 0f, p1.y), this.xy ? new float3(p2, 0f) : new float3(p2.x, 0f, p2.y), this.xy ? new float3(p3, 0f) : new float3(p3.x, 0f, p3.y));
		}

		// Token: 0x0600011C RID: 284 RVA: 0x000071EE File Offset: 0x000053EE
		public void Arrow(float3 from, float3 to)
		{
			this.ArrowRelativeSizeHead(from, to, this.xy ? CommandBuilder2D.XY_UP : CommandBuilder2D.XZ_UP, 0.2f);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00007214 File Offset: 0x00005414
		public void Arrow(float2 from, float2 to)
		{
			this.Arrow(this.xy ? new float3(from, 0f) : new float3(from.x, 0f, from.y), this.xy ? new float3(to, 0f) : new float3(to.x, 0f, to.y));
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000727D File Offset: 0x0000547D
		public void Arrow(float3 from, float3 to, float3 up, float headSize)
		{
			this.draw.Arrow(from, to, up, headSize);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00007290 File Offset: 0x00005490
		public void Arrow(float2 from, float2 to, float2 up, float headSize)
		{
			this.Arrow(this.xy ? new float3(from, 0f) : new float3(from.x, 0f, from.y), this.xy ? new float3(to, 0f) : new float3(to.x, 0f, to.y), this.xy ? new float3(up, 0f) : new float3(up.x, 0f, up.y), headSize);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00007326 File Offset: 0x00005526
		public void ArrowRelativeSizeHead(float3 from, float3 to, float3 up, float headFraction)
		{
			this.draw.ArrowRelativeSizeHead(from, to, up, headFraction);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00007338 File Offset: 0x00005538
		public void ArrowRelativeSizeHead(float2 from, float2 to, float2 up, float headFraction)
		{
			this.ArrowRelativeSizeHead(this.xy ? new float3(from, 0f) : new float3(from.x, 0f, from.y), this.xy ? new float3(to, 0f) : new float3(to.x, 0f, to.y), this.xy ? new float3(up, 0f) : new float3(up.x, 0f, up.y), headFraction);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000073D0 File Offset: 0x000055D0
		public void ArrowheadArc(float3 origin, float3 direction, float offset, float width = 60f)
		{
			if (!math.any(direction))
			{
				return;
			}
			if (offset < 0f)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset == 0f)
			{
				return;
			}
			Quaternion quaternion = Quaternion.LookRotation(direction, this.xy ? CommandBuilder2D.XY_UP : CommandBuilder2D.XZ_UP);
			this.PushMatrix(Matrix4x4.TRS(origin, quaternion, Vector3.one));
			float num = 1.5707964f - width * 0.008726646f;
			float num2 = 1.5707964f + width * 0.008726646f;
			this.draw.CircleXZInternal(float3.zero, offset, num, num2);
			float3 @float = new float3(math.cos(num), 0f, math.sin(num)) * offset;
			float3 float2 = new float3(math.cos(num2), 0f, math.sin(num2)) * offset;
			float3 float3 = new float3(0f, 0f, 1.4142f * offset);
			this.Line(@float, float3);
			this.Line(float3, float2);
			this.PopMatrix();
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000074DC File Offset: 0x000056DC
		public void ArrowheadArc(float2 origin, float2 direction, float offset, float width = 60f)
		{
			this.ArrowheadArc(this.xy ? new float3(origin, 0f) : new float3(origin.x, 0f, origin.y), this.xy ? new float3(direction, 0f) : new float3(direction.x, 0f, direction.y), offset, width);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00007548 File Offset: 0x00005748
		public void WireRectangle(float3 center, quaternion rotation, float2 size)
		{
			this.draw.WireRectangle(center, rotation, size);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00007558 File Offset: 0x00005758
		public void WireRectangle(float2 center, quaternion rotation, float2 size)
		{
			this.WireRectangle(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), rotation, size);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000758D File Offset: 0x0000578D
		public void Ray(float3 origin, float3 direction, Color color)
		{
			this.draw.Ray(origin, direction, color);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000075A0 File Offset: 0x000057A0
		public void Ray(float2 origin, float2 direction, Color color)
		{
			this.Ray(this.xy ? new float3(origin, 0f) : new float3(origin.x, 0f, origin.y), this.xy ? new float3(direction, 0f) : new float3(direction.x, 0f, direction.y), color);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000760A File Offset: 0x0000580A
		public void Ray(Ray ray, float length, Color color)
		{
			this.draw.Ray(ray, length, color);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000761A File Offset: 0x0000581A
		public void Arc(float3 center, float3 start, float3 end, Color color)
		{
			this.draw.Arc(center, start, end, color);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000762C File Offset: 0x0000582C
		public void Arc(float2 center, float2 start, float2 end, Color color)
		{
			this.Arc(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), this.xy ? new float3(start, 0f) : new float3(start.x, 0f, start.y), this.xy ? new float3(end, 0f) : new float3(end.x, 0f, end.y), color);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000076C2 File Offset: 0x000058C2
		[BurstDiscard]
		public void Polyline(List<Vector3> points, bool cycle, Color color)
		{
			this.draw.Polyline(points, cycle, color);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000076D2 File Offset: 0x000058D2
		[BurstDiscard]
		public void Polyline(List<Vector3> points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000076DD File Offset: 0x000058DD
		[BurstDiscard]
		public void Polyline(Vector3[] points, bool cycle, Color color)
		{
			this.draw.Polyline(points, cycle, color);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000076ED File Offset: 0x000058ED
		[BurstDiscard]
		public void Polyline(Vector3[] points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000076F8 File Offset: 0x000058F8
		[BurstDiscard]
		public void Polyline(float3[] points, bool cycle, Color color)
		{
			this.draw.Polyline(points, cycle, color);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00007708 File Offset: 0x00005908
		[BurstDiscard]
		public void Polyline(float3[] points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00007713 File Offset: 0x00005913
		public void Polyline(NativeArray<float3> points, bool cycle, Color color)
		{
			this.draw.Polyline(points, cycle, color);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00007723 File Offset: 0x00005923
		public void Polyline(NativeArray<float3> points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000772E File Offset: 0x0000592E
		public void Cross(float3 position, float size, Color color)
		{
			this.draw.Cross(position, size, color);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000773E File Offset: 0x0000593E
		public void Cross(float3 position, Color color)
		{
			this.Cross(position, 1f, color);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000774D File Offset: 0x0000594D
		public void Bezier(float3 p0, float3 p1, float3 p2, float3 p3, Color color)
		{
			this.draw.Bezier(p0, p1, p2, p3, color);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00007764 File Offset: 0x00005964
		public void Bezier(float2 p0, float2 p1, float2 p2, float2 p3, Color color)
		{
			this.Bezier(this.xy ? new float3(p0, 0f) : new float3(p0.x, 0f, p0.y), this.xy ? new float3(p1, 0f) : new float3(p1.x, 0f, p1.y), this.xy ? new float3(p2, 0f) : new float3(p2.x, 0f, p2.y), this.xy ? new float3(p3, 0f) : new float3(p3.x, 0f, p3.y), color);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00007828 File Offset: 0x00005A28
		public void Arrow(float3 from, float3 to, Color color)
		{
			this.ArrowRelativeSizeHead(from, to, this.xy ? CommandBuilder2D.XY_UP : CommandBuilder2D.XZ_UP, 0.2f, color);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000784C File Offset: 0x00005A4C
		public void Arrow(float2 from, float2 to, Color color)
		{
			this.Arrow(this.xy ? new float3(from, 0f) : new float3(from.x, 0f, from.y), this.xy ? new float3(to, 0f) : new float3(to.x, 0f, to.y), color);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000078B6 File Offset: 0x00005AB6
		public void Arrow(float3 from, float3 to, float3 up, float headSize, Color color)
		{
			this.draw.Arrow(from, to, up, headSize, color);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000078CC File Offset: 0x00005ACC
		public void Arrow(float2 from, float2 to, float2 up, float headSize, Color color)
		{
			this.Arrow(this.xy ? new float3(from, 0f) : new float3(from.x, 0f, from.y), this.xy ? new float3(to, 0f) : new float3(to.x, 0f, to.y), this.xy ? new float3(up, 0f) : new float3(up.x, 0f, up.y), headSize, color);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00007964 File Offset: 0x00005B64
		public void ArrowRelativeSizeHead(float3 from, float3 to, float3 up, float headFraction, Color color)
		{
			this.draw.ArrowRelativeSizeHead(from, to, up, headFraction, color);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00007978 File Offset: 0x00005B78
		public void ArrowRelativeSizeHead(float2 from, float2 to, float2 up, float headFraction, Color color)
		{
			this.ArrowRelativeSizeHead(this.xy ? new float3(from, 0f) : new float3(from.x, 0f, from.y), this.xy ? new float3(to, 0f) : new float3(to.x, 0f, to.y), this.xy ? new float3(up, 0f) : new float3(up.x, 0f, up.y), headFraction, color);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00007A10 File Offset: 0x00005C10
		public void ArrowheadArc(float3 origin, float3 direction, float offset, float width, Color color)
		{
			if (!math.any(direction))
			{
				return;
			}
			if (offset < 0f)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset == 0f)
			{
				return;
			}
			this.draw.PushColor(color);
			Quaternion quaternion = Quaternion.LookRotation(direction, this.xy ? CommandBuilder2D.XY_UP : CommandBuilder2D.XZ_UP);
			this.PushMatrix(Matrix4x4.TRS(origin, quaternion, Vector3.one));
			float num = 1.5707964f - width * 0.008726646f;
			float num2 = 1.5707964f + width * 0.008726646f;
			this.draw.CircleXZInternal(float3.zero, offset, num, num2);
			float3 @float = new float3(math.cos(num), 0f, math.sin(num)) * offset;
			float3 float2 = new float3(math.cos(num2), 0f, math.sin(num2)) * offset;
			float3 float3 = new float3(0f, 0f, 1.4142f * offset);
			this.Line(@float, float3);
			this.Line(float3, float2);
			this.PopMatrix();
			this.draw.PopColor();
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00007B33 File Offset: 0x00005D33
		public void ArrowheadArc(float3 origin, float3 direction, float offset, Color color)
		{
			this.ArrowheadArc(origin, direction, offset, 60f, color);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00007B48 File Offset: 0x00005D48
		public void ArrowheadArc(float2 origin, float2 direction, float offset, float width, Color color)
		{
			this.ArrowheadArc(this.xy ? new float3(origin, 0f) : new float3(origin.x, 0f, origin.y), this.xy ? new float3(direction, 0f) : new float3(direction.x, 0f, direction.y), offset, width, color);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00007BB6 File Offset: 0x00005DB6
		public void ArrowheadArc(float2 origin, float2 direction, float offset, Color color)
		{
			this.ArrowheadArc(origin, direction, offset, 60f, color);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00007BC8 File Offset: 0x00005DC8
		public void WireRectangle(float3 center, quaternion rotation, float2 size, Color color)
		{
			this.draw.WireRectangle(center, rotation, size, color);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00007BDA File Offset: 0x00005DDA
		public void WireRectangle(float2 center, quaternion rotation, float2 size, Color color)
		{
			this.WireRectangle(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), rotation, size, color);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00007C11 File Offset: 0x00005E11
		public void Line(float3 a, float3 b, Color color)
		{
			this.draw.Line(a, b, color);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00007C21 File Offset: 0x00005E21
		public void Circle(float2 center, float radius, float startAngle, float endAngle, Color color)
		{
			this.Circle(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), radius, startAngle, endAngle, color);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00007C5A File Offset: 0x00005E5A
		public void Circle(float2 center, float radius, Color color)
		{
			this.Circle(center, radius, 0f, 6.2831855f, color);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00007C70 File Offset: 0x00005E70
		public void Circle(float3 center, float radius, float startAngle, float endAngle, Color color)
		{
			this.draw.PushColor(color);
			if (this.xy)
			{
				this.draw.PushMatrix(CommandBuilder2D.XZ_TO_XY_MATRIX);
				this.draw.CircleXZInternal(new float3(center.x, center.z, center.y), radius, startAngle, endAngle);
				this.draw.PopMatrix();
			}
			else
			{
				this.draw.CircleXZInternal(center, radius, startAngle, endAngle);
			}
			this.draw.PopColor();
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00007CF0 File Offset: 0x00005EF0
		public void Circle(float3 center, float radius, Color color)
		{
			this.Circle(center, radius, 0f, 6.2831855f, color);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00007D05 File Offset: 0x00005F05
		public void WirePill(float2 a, float2 b, float radius, Color color)
		{
			this.WirePill(a, b - a, math.length(b - a), radius, color);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00007D24 File Offset: 0x00005F24
		public void WirePill(float2 position, float2 direction, float length, float radius, Color color)
		{
			this.draw.PushColor(color);
			direction = math.normalizesafe(direction, default(float2));
			if (radius <= 0f)
			{
				this.Line(position, position + direction * length);
			}
			else if (length <= 0f || math.all(direction == 0f))
			{
				this.Circle(position, radius, 0f, 6.2831855f);
			}
			else
			{
				float4x4 float4x;
				if (this.xy)
				{
					float4x = new float4x4(new float4(direction, 0f, 0f), new float4(math.cross(new float3(direction, 0f), CommandBuilder2D.XY_UP), 0f), new float4(0f, 0f, 1f, 0f), new float4(position, 0f, 1f));
				}
				else
				{
					float4x = new float4x4(new float4(direction.x, 0f, direction.y, 0f), new float4(0f, 1f, 0f, 0f), new float4(math.cross(new float3(direction.x, 0f, direction.y), CommandBuilder2D.XZ_UP), 0f), new float4(position.x, 0f, position.y, 1f));
				}
				this.draw.PushMatrix(float4x);
				this.Circle(new float2(0f, 0f), radius, 1.5707964f, 4.712389f);
				this.Line(new float2(0f, -radius), new float2(length, -radius));
				this.Circle(new float2(length, 0f), radius, -1.5707964f, 1.5707964f);
				this.Line(new float2(0f, radius), new float2(length, radius));
				this.draw.PopMatrix();
			}
			this.draw.PopColor();
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00007F28 File Offset: 0x00006128
		[BurstDiscard]
		public void Polyline(List<Vector2> points, bool cycle, Color color)
		{
			this.draw.PushColor(color);
			for (int i = 0; i < points.Count - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Count > 1)
			{
				this.Line(points[points.Count - 1], points[0]);
			}
			this.draw.PopColor();
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00007F9C File Offset: 0x0000619C
		[BurstDiscard]
		public void Polyline(List<Vector2> points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00007FA8 File Offset: 0x000061A8
		[BurstDiscard]
		public void Polyline(Vector2[] points, bool cycle, Color color)
		{
			this.draw.PushColor(color);
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
			this.draw.PopColor();
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00008013 File Offset: 0x00006213
		[BurstDiscard]
		public void Polyline(Vector2[] points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00008020 File Offset: 0x00006220
		[BurstDiscard]
		public void Polyline(float2[] points, bool cycle, Color color)
		{
			this.draw.PushColor(color);
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
			this.draw.PopColor();
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000808B File Offset: 0x0000628B
		[BurstDiscard]
		public void Polyline(float2[] points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00008098 File Offset: 0x00006298
		public void Polyline(NativeArray<float2> points, bool cycle, Color color)
		{
			this.draw.PushColor(color);
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
			this.draw.PopColor();
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00008113 File Offset: 0x00006313
		public void Polyline(NativeArray<float2> points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00008120 File Offset: 0x00006320
		public void Cross(float2 position, float size, Color color)
		{
			this.draw.PushColor(color);
			size *= 0.5f;
			this.Line(position - new float2(size, 0f), position + new float2(size, 0f));
			this.Line(position - new float2(0f, size), position + new float2(0f, size));
			this.draw.PopColor();
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000819D File Offset: 0x0000639D
		public void Cross(float2 position, Color color)
		{
			this.Cross(position, 1f, color);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000081AC File Offset: 0x000063AC
		public void WireRectangle(float3 center, float2 size, Color color)
		{
			this.draw.WirePlane(center, this.xy ? CommandBuilder2D.XY_TO_XZ_ROTATION : CommandBuilder2D.XZ_TO_XZ_ROTATION, size, color);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000081D0 File Offset: 0x000063D0
		public void WireRectangle(Rect rect, Color color)
		{
			this.draw.PushColor(color);
			float2 @float = rect.min;
			float2 float2 = rect.max;
			this.Line(new float2(@float.x, @float.y), new float2(float2.x, @float.y));
			this.Line(new float2(float2.x, @float.y), new float2(float2.x, float2.y));
			this.Line(new float2(float2.x, float2.y), new float2(@float.x, float2.y));
			this.Line(new float2(@float.x, float2.y), new float2(@float.x, @float.y));
			this.draw.PopColor();
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000082B0 File Offset: 0x000064B0
		public void WireGrid(float2 center, int2 cells, float2 totalSize, Color color)
		{
			this.draw.WireGrid(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), this.xy ? CommandBuilder2D.XY_TO_XZ_ROTATION : CommandBuilder2D.XZ_TO_XZ_ROTATION, cells, totalSize, color);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000830B File Offset: 0x0000650B
		public void WireGrid(float3 center, int2 cells, float2 totalSize, Color color)
		{
			this.draw.WireGrid(center, this.xy ? CommandBuilder2D.XY_TO_XZ_ROTATION : CommandBuilder2D.XZ_TO_XZ_ROTATION, cells, totalSize, color);
		}

		// Token: 0x0400007A RID: 122
		private CommandBuilder draw;

		// Token: 0x0400007B RID: 123
		private bool xy;

		// Token: 0x0400007C RID: 124
		private static readonly float3 XY_UP = new float3(0f, 0f, 1f);

		// Token: 0x0400007D RID: 125
		private static readonly float3 XZ_UP = new float3(0f, 1f, 0f);

		// Token: 0x0400007E RID: 126
		private static readonly quaternion XY_TO_XZ_ROTATION = quaternion.RotateX(-1.5707964f);

		// Token: 0x0400007F RID: 127
		private static readonly quaternion XZ_TO_XZ_ROTATION = quaternion.identity;

		// Token: 0x04000080 RID: 128
		private static readonly float4x4 XZ_TO_XY_MATRIX = new float4x4(new float4(1f, 0f, 0f, 0f), new float4(0f, 0f, 1f, 0f), new float4(0f, 1f, 0f, 0f), new float4(0f, 0f, 0f, 1f));
	}
}
