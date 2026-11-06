using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Drawing
{
	// Token: 0x02000028 RID: 40
	public static class Draw
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000159 RID: 345 RVA: 0x000083FA File Offset: 0x000065FA
		public static ref CommandBuilder ingame
		{
			get
			{
				DrawingManager.Init();
				return ref Draw.ingame_builder;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00008406 File Offset: 0x00006606
		public static ref CommandBuilder editor
		{
			get
			{
				DrawingManager.Init();
				return ref Draw.builder;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00008412 File Offset: 0x00006612
		public static CommandBuilder2D xy
		{
			get
			{
				DrawingManager.Init();
				return new CommandBuilder2D(Draw.builder, true);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00008424 File Offset: 0x00006624
		public static CommandBuilder2D xz
		{
			get
			{
				DrawingManager.Init();
				return new CommandBuilder2D(Draw.builder, false);
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00008438 File Offset: 0x00006638
		[BurstDiscard]
		public static CommandBuilder.ScopeEmpty WithMatrix(Matrix4x4 matrix)
		{
			return default(CommandBuilder.ScopeEmpty);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00008450 File Offset: 0x00006650
		[BurstDiscard]
		public static CommandBuilder.ScopeEmpty WithMatrix(float3x3 matrix)
		{
			return default(CommandBuilder.ScopeEmpty);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00008468 File Offset: 0x00006668
		[BurstDiscard]
		public static CommandBuilder.ScopeEmpty WithColor(Color color)
		{
			return default(CommandBuilder.ScopeEmpty);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00008480 File Offset: 0x00006680
		[BurstDiscard]
		public static CommandBuilder.ScopeEmpty WithLineWidth(float pixels, bool automaticJoins = true)
		{
			return default(CommandBuilder.ScopeEmpty);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void PushMatrix(Matrix4x4 matrix)
		{
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void PushMatrix(float4x4 matrix)
		{
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void PopMatrix()
		{
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Line(float3 a, float3 b)
		{
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Line(Vector3 a, Vector3 b)
		{
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Line(Vector3 a, Vector3 b, Color color)
		{
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Ray(float3 origin, float3 direction)
		{
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Ray(Ray ray, float length)
		{
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Arc(float3 center, float3 start, float3 end)
		{
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xz.Circle instead")]
		public static void CircleXZ(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Circle(float3 center, float3 normal, float radius)
		{
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireCylinder(float3 bottom, float3 top, float radius)
		{
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireCylinder(float3 position, float3 up, float height, float radius)
		{
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireCapsule(float3 start, float3 end, float radius)
		{
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireCapsule(float3 position, float3 direction, float length, float radius)
		{
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireSphere(float3 position, float radius)
		{
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(List<Vector3> points, bool cycle = false)
		{
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(Vector3[] points, bool cycle = false)
		{
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(float3[] points, bool cycle = false)
		{
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(NativeArray<float3> points, bool cycle = false)
		{
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireBox(float3 center, float3 size)
		{
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireBox(float3 center, quaternion rotation, float3 size)
		{
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireBox(Bounds bounds)
		{
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireMesh(Mesh mesh)
		{
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireMesh(NativeArray<float3> vertices, NativeArray<int> triangles)
		{
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Cross(float3 position, float size = 1f)
		{
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xz.Cross instead")]
		public static void CrossXZ(float3 position, float size = 1f)
		{
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xy.Cross instead")]
		public static void CrossXY(float3 position, float size = 1f)
		{
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Bezier(float3 p0, float3 p1, float3 p2, float3 p3)
		{
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Arrow(float3 from, float3 to)
		{
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Arrow(float3 from, float3 to, float3 up, float headSize)
		{
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void ArrowRelativeSizeHead(float3 from, float3 to, float3 up, float headFraction)
		{
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void ArrowheadArc(float3 origin, float3 direction, float offset, float width = 60f)
		{
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireGrid(float3 center, quaternion rotation, int2 cells, float2 totalSize)
		{
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireRectangle(float3 center, quaternion rotation, float2 size)
		{
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xy.WireRectangle instead")]
		public static void WireRectangle(Rect rect)
		{
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WirePlane(float3 center, float3 normal, float2 size)
		{
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WirePlane(float3 center, quaternion rotation, float2 size)
		{
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidBox(float3 center, float3 size)
		{
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidBox(Bounds bounds)
		{
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidBox(float3 center, quaternion rotation, float3 size)
		{
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Line(float3 a, float3 b, Color color)
		{
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Ray(float3 origin, float3 direction, Color color)
		{
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Ray(Ray ray, float length, Color color)
		{
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Arc(float3 center, float3 start, float3 end, Color color)
		{
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xz.Circle instead")]
		public static void CircleXZ(float3 center, float radius, float startAngle, float endAngle, Color color)
		{
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xz.Circle instead")]
		public static void CircleXZ(float3 center, float radius, Color color)
		{
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Circle(float3 center, float3 normal, float radius, Color color)
		{
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireCylinder(float3 bottom, float3 top, float radius, Color color)
		{
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireCylinder(float3 position, float3 up, float height, float radius, Color color)
		{
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireCapsule(float3 start, float3 end, float radius, Color color)
		{
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireCapsule(float3 position, float3 direction, float length, float radius, Color color)
		{
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireSphere(float3 position, float radius, Color color)
		{
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(List<Vector3> points, bool cycle, Color color)
		{
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(List<Vector3> points, Color color)
		{
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(Vector3[] points, bool cycle, Color color)
		{
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(Vector3[] points, Color color)
		{
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(float3[] points, bool cycle, Color color)
		{
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(float3[] points, Color color)
		{
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(NativeArray<float3> points, bool cycle, Color color)
		{
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(NativeArray<float3> points, Color color)
		{
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireBox(float3 center, float3 size, Color color)
		{
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireBox(float3 center, quaternion rotation, float3 size, Color color)
		{
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireBox(Bounds bounds, Color color)
		{
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireMesh(Mesh mesh, Color color)
		{
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireMesh(NativeArray<float3> vertices, NativeArray<int> triangles, Color color)
		{
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Cross(float3 position, float size, Color color)
		{
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Cross(float3 position, Color color)
		{
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xz.Cross instead")]
		public static void CrossXZ(float3 position, float size, Color color)
		{
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xz.Cross instead")]
		public static void CrossXZ(float3 position, Color color)
		{
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xy.Cross instead")]
		public static void CrossXY(float3 position, float size, Color color)
		{
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xy.Cross instead")]
		public static void CrossXY(float3 position, Color color)
		{
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Bezier(float3 p0, float3 p1, float3 p2, float3 p3, Color color)
		{
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Arrow(float3 from, float3 to, Color color)
		{
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Arrow(float3 from, float3 to, float3 up, float headSize, Color color)
		{
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void ArrowRelativeSizeHead(float3 from, float3 to, float3 up, float headFraction, Color color)
		{
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void ArrowheadArc(float3 origin, float3 direction, float offset, float width, Color color)
		{
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void ArrowheadArc(float3 origin, float3 direction, float offset, Color color)
		{
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireGrid(float3 center, quaternion rotation, int2 cells, float2 totalSize, Color color)
		{
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireRectangle(float3 center, quaternion rotation, float2 size, Color color)
		{
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xy.WireRectangle instead")]
		public static void WireRectangle(Rect rect, Color color)
		{
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WirePlane(float3 center, float3 normal, float2 size, Color color)
		{
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WirePlane(float3 center, quaternion rotation, float2 size, Color color)
		{
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidBox(float3 center, float3 size, Color color)
		{
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidBox(Bounds bounds, Color color)
		{
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidBox(float3 center, quaternion rotation, float3 size, Color color)
		{
		}

		// Token: 0x04000081 RID: 129
		internal static CommandBuilder builder;

		// Token: 0x04000082 RID: 130
		internal static CommandBuilder ingame_builder;
	}
}
