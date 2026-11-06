using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.UIElements
{
	// Token: 0x0200027B RID: 635
	[NativeHeader("ModuleOverrides/com.unity.ui/Core/Native/Renderer/UIPainter2D.bindings.h")]
	internal static class UIPainter2D
	{
		// Token: 0x060011F1 RID: 4593
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr Create(bool computeBBox = false);

		// Token: 0x060011F2 RID: 4594
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Destroy(IntPtr handle);

		// Token: 0x060011F3 RID: 4595
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Reset(IntPtr handle);

		// Token: 0x060011F4 RID: 4596
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetLineWidth(IntPtr handle);

		// Token: 0x060011F5 RID: 4597
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetLineWidth(IntPtr handle, float value);

		// Token: 0x060011F6 RID: 4598 RVA: 0x0004113C File Offset: 0x0003F33C
		public static Color GetStrokeColor(IntPtr handle)
		{
			Color color;
			UIPainter2D.GetStrokeColor_Injected(handle, out color);
			return color;
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x00041152 File Offset: 0x0003F352
		public static void SetStrokeColor(IntPtr handle, Color value)
		{
			UIPainter2D.SetStrokeColor_Injected(handle, ref value);
		}

		// Token: 0x060011F8 RID: 4600
		[NativeName("GetStrokeGradientCopy")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Gradient GetStrokeGradient(IntPtr handle);

		// Token: 0x060011F9 RID: 4601
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStrokeGradient(IntPtr handle, Gradient gradient);

		// Token: 0x060011FA RID: 4602 RVA: 0x0004115C File Offset: 0x0003F35C
		public static Color GetFillColor(IntPtr handle)
		{
			Color color;
			UIPainter2D.GetFillColor_Injected(handle, out color);
			return color;
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00041172 File Offset: 0x0003F372
		public static void SetFillColor(IntPtr handle, Color value)
		{
			UIPainter2D.SetFillColor_Injected(handle, ref value);
		}

		// Token: 0x060011FC RID: 4604
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern LineJoin GetLineJoin(IntPtr handle);

		// Token: 0x060011FD RID: 4605
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetLineJoin(IntPtr handle, LineJoin value);

		// Token: 0x060011FE RID: 4606
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern LineCap GetLineCap(IntPtr handle);

		// Token: 0x060011FF RID: 4607
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetLineCap(IntPtr handle, LineCap value);

		// Token: 0x06001200 RID: 4608
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetMiterLimit(IntPtr handle);

		// Token: 0x06001201 RID: 4609
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetMiterLimit(IntPtr handle, float value);

		// Token: 0x06001202 RID: 4610
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void BeginPath(IntPtr handle);

		// Token: 0x06001203 RID: 4611 RVA: 0x0004117C File Offset: 0x0003F37C
		public static void MoveTo(IntPtr handle, Vector2 pos)
		{
			UIPainter2D.MoveTo_Injected(handle, ref pos);
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x00041186 File Offset: 0x0003F386
		public static void LineTo(IntPtr handle, Vector2 pos)
		{
			UIPainter2D.LineTo_Injected(handle, ref pos);
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x00041190 File Offset: 0x0003F390
		public static void ArcTo(IntPtr handle, Vector2 p1, Vector2 p2, float radius)
		{
			UIPainter2D.ArcTo_Injected(handle, ref p1, ref p2, radius);
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x0004119D File Offset: 0x0003F39D
		public static void Arc(IntPtr handle, Vector2 center, float radius, float startAngleRads, float endAngleRads, ArcDirection direction)
		{
			UIPainter2D.Arc_Injected(handle, ref center, radius, startAngleRads, endAngleRads, direction);
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x000411AD File Offset: 0x0003F3AD
		public static void BezierCurveTo(IntPtr handle, Vector2 p1, Vector2 p2, Vector2 p3)
		{
			UIPainter2D.BezierCurveTo_Injected(handle, ref p1, ref p2, ref p3);
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x000411BB File Offset: 0x0003F3BB
		public static void QuadraticCurveTo(IntPtr handle, Vector2 p1, Vector2 p2)
		{
			UIPainter2D.QuadraticCurveTo_Injected(handle, ref p1, ref p2);
		}

		// Token: 0x06001209 RID: 4617
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ClosePath(IntPtr handle);

		// Token: 0x0600120A RID: 4618 RVA: 0x000411C8 File Offset: 0x0003F3C8
		public static Rect GetBBox(IntPtr handle)
		{
			Rect rect;
			UIPainter2D.GetBBox_Injected(handle, out rect);
			return rect;
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x000411E0 File Offset: 0x0003F3E0
		public static MeshWriteDataInterface Stroke(IntPtr handle)
		{
			MeshWriteDataInterface meshWriteDataInterface;
			UIPainter2D.Stroke_Injected(handle, out meshWriteDataInterface);
			return meshWriteDataInterface;
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x000411F8 File Offset: 0x0003F3F8
		public static MeshWriteDataInterface Fill(IntPtr handle, FillRule fillRule)
		{
			MeshWriteDataInterface meshWriteDataInterface;
			UIPainter2D.Fill_Injected(handle, fillRule, out meshWriteDataInterface);
			return meshWriteDataInterface;
		}

		// Token: 0x0600120D RID: 4621
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetStrokeColor_Injected(IntPtr handle, out Color ret);

		// Token: 0x0600120E RID: 4622
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetStrokeColor_Injected(IntPtr handle, ref Color value);

		// Token: 0x0600120F RID: 4623
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetFillColor_Injected(IntPtr handle, out Color ret);

		// Token: 0x06001210 RID: 4624
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFillColor_Injected(IntPtr handle, ref Color value);

		// Token: 0x06001211 RID: 4625
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void MoveTo_Injected(IntPtr handle, ref Vector2 pos);

		// Token: 0x06001212 RID: 4626
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void LineTo_Injected(IntPtr handle, ref Vector2 pos);

		// Token: 0x06001213 RID: 4627
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ArcTo_Injected(IntPtr handle, ref Vector2 p1, ref Vector2 p2, float radius);

		// Token: 0x06001214 RID: 4628
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Arc_Injected(IntPtr handle, ref Vector2 center, float radius, float startAngleRads, float endAngleRads, ArcDirection direction);

		// Token: 0x06001215 RID: 4629
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void BezierCurveTo_Injected(IntPtr handle, ref Vector2 p1, ref Vector2 p2, ref Vector2 p3);

		// Token: 0x06001216 RID: 4630
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void QuadraticCurveTo_Injected(IntPtr handle, ref Vector2 p1, ref Vector2 p2);

		// Token: 0x06001217 RID: 4631
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetBBox_Injected(IntPtr handle, out Rect ret);

		// Token: 0x06001218 RID: 4632
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Stroke_Injected(IntPtr handle, out MeshWriteDataInterface ret);

		// Token: 0x06001219 RID: 4633
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Fill_Injected(IntPtr handle, FillRule fillRule, out MeshWriteDataInterface ret);
	}
}
