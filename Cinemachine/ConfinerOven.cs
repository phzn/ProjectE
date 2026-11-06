using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000046 RID: 70
	internal class ConfinerOven
	{
		// Token: 0x06000317 RID: 791 RVA: 0x0001390C File Offset: 0x00011B0C
		public ConfinerOven(in List<List<Vector2>> inputPath, in float aspectRatio, float maxFrustumHeight, float skeletonPadding)
		{
			this.Initialize(in inputPath, in aspectRatio, maxFrustumHeight, Mathf.Max(0f, skeletonPadding) + 1f);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00013968 File Offset: 0x00011B68
		public ConfinerOven.BakedSolution GetBakedSolution(float frustumHeight)
		{
			frustumHeight = ((this.m_Cache.userSetMaxFrustumHeight <= 0f) ? frustumHeight : Mathf.Min(this.m_Cache.userSetMaxFrustumHeight, frustumHeight));
			if (this.State == ConfinerOven.BakingState.BAKED && frustumHeight > this.m_Cache.theoriticalMaxFrustumHeight)
			{
				return new ConfinerOven.BakedSolution(this.m_AspectStretcher.Aspect, frustumHeight, false, this.m_PolygonRect, this.m_OriginalPolygon, new List<List<ClipperLib.IntPoint>>
				{
					new List<ClipperLib.IntPoint> { this.m_MidPoint }
				});
			}
			ClipperLib.ClipperOffset clipperOffset = new ClipperLib.ClipperOffset(2.0, 0.25);
			clipperOffset.AddPaths(this.m_OriginalPolygon, ClipperLib.JoinType.jtMiter, ClipperLib.EndType.etClosedPolygon);
			List<List<ClipperLib.IntPoint>> list = new List<List<ClipperLib.IntPoint>>();
			clipperOffset.Execute(ref list, (double)(-1f * frustumHeight * 100000f));
			List<List<ClipperLib.IntPoint>> list2 = new List<List<ClipperLib.IntPoint>>();
			if (this.State == ConfinerOven.BakingState.BAKING || this.m_Skeleton.Count == 0)
			{
				list2 = list;
			}
			else
			{
				ClipperLib.Clipper clipper = new ClipperLib.Clipper(0);
				clipper.AddPaths(list, ClipperLib.PolyType.ptSubject, true);
				clipper.AddPaths(this.m_Skeleton, ClipperLib.PolyType.ptClip, true);
				clipper.Execute(ClipperLib.ClipType.ctUnion, list2, ClipperLib.PolyFillType.pftEvenOdd, ClipperLib.PolyFillType.pftEvenOdd);
			}
			return new ConfinerOven.BakedSolution(this.m_AspectStretcher.Aspect, frustumHeight, this.m_MinFrustumHeightWithBones < frustumHeight, this.m_PolygonRect, this.m_OriginalPolygon, list2);
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00013A9E File Offset: 0x00011C9E
		// (set) Token: 0x0600031A RID: 794 RVA: 0x00013AA6 File Offset: 0x00011CA6
		public ConfinerOven.BakingState State { get; private set; }

		// Token: 0x0600031B RID: 795 RVA: 0x00013AB0 File Offset: 0x00011CB0
		private void Initialize(in List<List<Vector2>> inputPath, in float aspectRatio, float maxFrustumHeight, float skeletonPadding)
		{
			this.m_Skeleton.Clear();
			this.m_Cache.userSetMaxFrustumHeight = maxFrustumHeight;
			this.m_MinFrustumHeightWithBones = float.MaxValue;
			this.m_SkeletonPadding = skeletonPadding;
			this.m_PolygonRect = ConfinerOven.<Initialize>g__GetPolygonBoundingBox|24_0(in inputPath);
			this.m_AspectStretcher = new ConfinerOven.AspectStretcher(aspectRatio, this.m_PolygonRect.center.x);
			this.m_Cache.theoriticalMaxFrustumHeight = Mathf.Max(this.m_PolygonRect.width / aspectRatio, this.m_PolygonRect.height) / 2f;
			this.m_OriginalPolygon = new List<List<ClipperLib.IntPoint>>(inputPath.Count);
			for (int i = 0; i < inputPath.Count; i++)
			{
				List<Vector2> list = inputPath[i];
				int count = list.Count;
				List<ClipperLib.IntPoint> list2 = new List<ClipperLib.IntPoint>(count);
				for (int j = 0; j < count; j++)
				{
					Vector2 vector = this.m_AspectStretcher.Stretch(list[j]);
					list2.Add(new ClipperLib.IntPoint((double)(vector.x * 100000f), (double)(vector.y * 100000f)));
				}
				this.m_OriginalPolygon.Add(list2);
			}
			this.m_MidPoint = ConfinerOven.<Initialize>g__MidPointOfIntRect|24_1(ClipperLib.ClipperBase.GetBounds(this.m_OriginalPolygon));
			if (this.m_Cache.userSetMaxFrustumHeight < 0f)
			{
				this.State = ConfinerOven.BakingState.BAKED;
				return;
			}
			this.m_Cache.maxFrustumHeight = this.m_Cache.userSetMaxFrustumHeight;
			if (this.m_Cache.maxFrustumHeight == 0f || this.m_Cache.maxFrustumHeight > this.m_Cache.theoriticalMaxFrustumHeight)
			{
				this.m_Cache.maxFrustumHeight = this.m_Cache.theoriticalMaxFrustumHeight;
			}
			this.m_Cache.stepSize = this.m_Cache.maxFrustumHeight;
			this.m_Cache.offsetter = new ClipperLib.ClipperOffset(2.0, 0.25);
			this.m_Cache.offsetter.AddPaths(this.m_OriginalPolygon, ClipperLib.JoinType.jtMiter, ClipperLib.EndType.etClosedPolygon);
			List<List<ClipperLib.IntPoint>> list3 = new List<List<ClipperLib.IntPoint>>();
			this.m_Cache.offsetter.Execute(ref list3, 0.0);
			this.m_Cache.solutions = new List<ConfinerOven.PolygonSolution>();
			this.m_Cache.solutions.Add(new ConfinerOven.PolygonSolution
			{
				polygons = list3,
				frustumHeight = 0f
			});
			this.m_Cache.rightCandidate = default(ConfinerOven.PolygonSolution);
			this.m_Cache.leftCandidate = new ConfinerOven.PolygonSolution
			{
				polygons = list3,
				frustumHeight = 0f
			};
			this.m_Cache.currentFrustumHeight = 0f;
			this.m_Cache.maxCandidate = new List<List<ClipperLib.IntPoint>>();
			this.m_Cache.offsetter.Execute(ref this.m_Cache.maxCandidate, (double)(-1f * this.m_Cache.theoriticalMaxFrustumHeight * 100000f));
			this.m_Cache.bakeTime = 0f;
			this.State = ConfinerOven.BakingState.BAKING;
			this.bakeProgress = 0f;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00013DBC File Offset: 0x00011FBC
		public void BakeConfiner(float maxComputationTimePerFrameInSeconds)
		{
			if (this.State != ConfinerOven.BakingState.BAKING)
			{
				return;
			}
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			while (this.m_Cache.solutions.Count < 1000)
			{
				List<List<ClipperLib.IntPoint>> list = new List<List<ClipperLib.IntPoint>>(this.m_Cache.leftCandidate.polygons.Count);
				this.m_Cache.stepSize = Mathf.Min(this.m_Cache.stepSize, this.m_Cache.maxFrustumHeight - this.m_Cache.leftCandidate.frustumHeight);
				this.m_Cache.currentFrustumHeight = this.m_Cache.leftCandidate.frustumHeight + this.m_Cache.stepSize;
				if (Math.Abs(this.m_Cache.currentFrustumHeight - this.m_Cache.maxFrustumHeight) < 0.0001f)
				{
					list = this.m_Cache.maxCandidate;
				}
				else
				{
					this.m_Cache.offsetter.Execute(ref list, (double)(-1f * this.m_Cache.currentFrustumHeight * 100000f));
				}
				if (this.m_Cache.leftCandidate.StateChanged(in list))
				{
					this.m_Cache.rightCandidate = new ConfinerOven.PolygonSolution
					{
						polygons = list,
						frustumHeight = this.m_Cache.currentFrustumHeight
					};
					this.m_Cache.stepSize = Mathf.Max(this.m_Cache.stepSize / 2f, 0.0005f);
				}
				else
				{
					this.m_Cache.leftCandidate = new ConfinerOven.PolygonSolution
					{
						polygons = list,
						frustumHeight = this.m_Cache.currentFrustumHeight
					};
					if (!this.m_Cache.rightCandidate.IsNull)
					{
						this.m_Cache.stepSize = Mathf.Max(this.m_Cache.stepSize / 2f, 0.0005f);
					}
				}
				if (!this.m_Cache.rightCandidate.IsNull && this.m_Cache.stepSize <= 0.0005f)
				{
					this.m_Cache.solutions.Add(this.m_Cache.leftCandidate);
					this.m_Cache.solutions.Add(this.m_Cache.rightCandidate);
					this.m_Cache.leftCandidate = this.m_Cache.rightCandidate;
					this.m_Cache.rightCandidate = default(ConfinerOven.PolygonSolution);
					this.m_Cache.stepSize = this.m_Cache.maxFrustumHeight;
				}
				else if (this.m_Cache.rightCandidate.IsNull || this.m_Cache.leftCandidate.frustumHeight >= this.m_Cache.maxFrustumHeight)
				{
					this.m_Cache.solutions.Add(this.m_Cache.leftCandidate);
					break;
				}
				float num = Time.realtimeSinceStartup - realtimeSinceStartup;
				if (num > maxComputationTimePerFrameInSeconds)
				{
					this.m_Cache.bakeTime = this.m_Cache.bakeTime + num;
					if (this.m_Cache.bakeTime > this.m_MaxComputationTimeForFullSkeletonBakeInSeconds)
					{
						this.State = ConfinerOven.BakingState.TIMEOUT;
					}
					this.bakeProgress = this.m_Cache.leftCandidate.frustumHeight / this.m_Cache.maxFrustumHeight;
					return;
				}
			}
			this.<BakeConfiner>g__ComputeSkeleton|25_0(in this.m_Cache.solutions);
			for (int i = this.m_Cache.solutions.Count - 1; i >= 0; i--)
			{
				if (this.m_Cache.solutions[i].polygons.Count == 0)
				{
					this.m_Cache.solutions.RemoveAt(i);
				}
			}
			this.bakeProgress = 1f;
			this.State = ConfinerOven.BakingState.BAKED;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00014150 File Offset: 0x00012350
		[CompilerGenerated]
		internal static Rect <Initialize>g__GetPolygonBoundingBox|24_0(in List<List<Vector2>> polygons)
		{
			float num = float.PositiveInfinity;
			float num2 = float.NegativeInfinity;
			float num3 = float.PositiveInfinity;
			float num4 = float.NegativeInfinity;
			for (int i = 0; i < polygons.Count; i++)
			{
				for (int j = 0; j < polygons[i].Count; j++)
				{
					Vector2 vector = polygons[i][j];
					num = Mathf.Min(num, vector.x);
					num2 = Mathf.Max(num2, vector.x);
					num3 = Mathf.Min(num3, vector.y);
					num4 = Mathf.Max(num4, vector.y);
				}
			}
			return new Rect(num, num3, Mathf.Max(0f, num2 - num), Mathf.Max(0f, num4 - num3));
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00014213 File Offset: 0x00012413
		[CompilerGenerated]
		internal static ClipperLib.IntPoint <Initialize>g__MidPointOfIntRect|24_1(ClipperLib.IntRect bounds)
		{
			return new ClipperLib.IntPoint((bounds.left + bounds.right) / 2L, (bounds.top + bounds.bottom) / 2L);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0001423C File Offset: 0x0001243C
		[CompilerGenerated]
		private void <BakeConfiner>g__ComputeSkeleton|25_0(in List<ConfinerOven.PolygonSolution> solutions)
		{
			ClipperLib.Clipper clipper = new ClipperLib.Clipper(0);
			ClipperLib.ClipperOffset clipperOffset = new ClipperLib.ClipperOffset(2.0, 0.25);
			for (int i = 1; i < solutions.Count - 1; i += 2)
			{
				ConfinerOven.PolygonSolution polygonSolution = solutions[i];
				ConfinerOven.PolygonSolution polygonSolution2 = solutions[i + 1];
				double num = (double)(this.m_SkeletonPadding * 100000f * (polygonSolution2.frustumHeight - polygonSolution.frustumHeight));
				List<List<ClipperLib.IntPoint>> list = new List<List<ClipperLib.IntPoint>>();
				clipperOffset.Clear();
				clipperOffset.AddPaths(polygonSolution.polygons, ClipperLib.JoinType.jtMiter, ClipperLib.EndType.etClosedPolygon);
				clipperOffset.Execute(ref list, num);
				List<List<ClipperLib.IntPoint>> list2 = new List<List<ClipperLib.IntPoint>>();
				clipperOffset.Clear();
				clipperOffset.AddPaths(polygonSolution2.polygons, ClipperLib.JoinType.jtMiter, ClipperLib.EndType.etClosedPolygon);
				clipperOffset.Execute(ref list2, num * 2.0);
				List<List<ClipperLib.IntPoint>> list3 = new List<List<ClipperLib.IntPoint>>();
				clipper.Clear();
				clipper.AddPaths(list, ClipperLib.PolyType.ptSubject, true);
				clipper.AddPaths(list2, ClipperLib.PolyType.ptClip, true);
				clipper.Execute(ClipperLib.ClipType.ctDifference, list3, ClipperLib.PolyFillType.pftEvenOdd, ClipperLib.PolyFillType.pftEvenOdd);
				if (list3.Count > 0 && list3[0].Count > 0)
				{
					this.m_Skeleton.AddRange(list3);
					if (this.m_MinFrustumHeightWithBones == 3.4028235E+38f)
					{
						this.m_MinFrustumHeightWithBones = polygonSolution2.frustumHeight;
					}
				}
			}
		}

		// Token: 0x0400020F RID: 527
		private float m_MinFrustumHeightWithBones;

		// Token: 0x04000210 RID: 528
		private float m_SkeletonPadding;

		// Token: 0x04000211 RID: 529
		private List<List<ClipperLib.IntPoint>> m_OriginalPolygon;

		// Token: 0x04000212 RID: 530
		private ClipperLib.IntPoint m_MidPoint;

		// Token: 0x04000213 RID: 531
		private List<List<ClipperLib.IntPoint>> m_Skeleton = new List<List<ClipperLib.IntPoint>>();

		// Token: 0x04000214 RID: 532
		private const long k_FloatToIntScaler = 100000L;

		// Token: 0x04000215 RID: 533
		private const float k_IntToFloatScaler = 1E-05f;

		// Token: 0x04000216 RID: 534
		private const float k_MinStepSize = 0.0005f;

		// Token: 0x04000217 RID: 535
		private Rect m_PolygonRect;

		// Token: 0x04000218 RID: 536
		private ConfinerOven.AspectStretcher m_AspectStretcher = new ConfinerOven.AspectStretcher(1f, 0f);

		// Token: 0x04000219 RID: 537
		private float m_MaxComputationTimeForFullSkeletonBakeInSeconds = 5f;

		// Token: 0x0400021B RID: 539
		public float bakeProgress;

		// Token: 0x0400021C RID: 540
		private ConfinerOven.BakingStateCache m_Cache;

		// Token: 0x020000B4 RID: 180
		public class BakedSolution
		{
			// Token: 0x06000461 RID: 1121 RVA: 0x0001943C File Offset: 0x0001763C
			public BakedSolution(float aspectRatio, float frustumHeight, bool hasBones, Rect polygonBounds, List<List<ClipperLib.IntPoint>> originalPolygon, List<List<ClipperLib.IntPoint>> solution)
			{
				this.m_AspectStretcher = new ConfinerOven.AspectStretcher(aspectRatio, polygonBounds.center.x);
				this.m_FrustumSizeIntSpace = frustumHeight * 100000f;
				this.m_HasBones = hasBones;
				this.m_OriginalPolygon = originalPolygon;
				this.m_Solution = solution;
				float num = polygonBounds.width / aspectRatio * 100000f;
				float num2 = polygonBounds.height * 100000f;
				this.m_SqrPolygonDiagonal = (double)(num * num + num2 * num2);
			}

			// Token: 0x06000462 RID: 1122 RVA: 0x000194B7 File Offset: 0x000176B7
			public bool IsValid()
			{
				return this.m_Solution != null;
			}

			// Token: 0x06000463 RID: 1123 RVA: 0x000194C4 File Offset: 0x000176C4
			public Vector2 ConfinePoint(in Vector2 pointToConfine)
			{
				if (this.m_Solution.Count <= 0)
				{
					return pointToConfine;
				}
				Vector2 vector = this.m_AspectStretcher.Stretch(pointToConfine);
				ClipperLib.IntPoint intPoint = new ClipperLib.IntPoint((double)(vector.x * 100000f), (double)(vector.y * 100000f));
				for (int i = 0; i < this.m_Solution.Count; i++)
				{
					if (ClipperLib.Clipper.PointInPolygon(intPoint, this.m_Solution[i]) != 0)
					{
						return pointToConfine;
					}
				}
				bool flag = this.m_HasBones && this.<ConfinePoint>g__IsInsideOriginal|9_1(intPoint);
				ClipperLib.IntPoint intPoint2 = intPoint;
				double num = double.MaxValue;
				for (int j = 0; j < this.m_Solution.Count; j++)
				{
					int count = this.m_Solution[j].Count;
					for (int k = 0; k < count; k++)
					{
						ClipperLib.IntPoint intPoint3 = this.m_Solution[j][k];
						ClipperLib.IntPoint intPoint4 = this.m_Solution[j][(k + 1) % count];
						ClipperLib.IntPoint intPoint5 = ConfinerOven.BakedSolution.<ConfinePoint>g__IntPointLerp|9_0(intPoint3, intPoint4, ConfinerOven.BakedSolution.<ConfinePoint>g__ClosestPointOnSegment|9_2(intPoint, intPoint3, intPoint4));
						double num2 = (double)Mathf.Abs((float)(intPoint.X - intPoint5.X));
						double num3 = (double)Mathf.Abs((float)(intPoint.Y - intPoint5.Y));
						double num4 = num2 * num2 + num3 * num3;
						if (num2 > (double)this.m_FrustumSizeIntSpace || num3 > (double)this.m_FrustumSizeIntSpace)
						{
							num4 += this.m_SqrPolygonDiagonal;
						}
						if (num4 < num && (!flag || !this.<ConfinePoint>g__DoesIntersectOriginal|9_3(intPoint, intPoint5)))
						{
							num = num4;
							intPoint2 = intPoint5;
						}
					}
				}
				Vector2 vector2 = new Vector2((float)intPoint2.X * 1E-05f, (float)intPoint2.Y * 1E-05f);
				return this.m_AspectStretcher.Unstretch(vector2);
			}

			// Token: 0x06000464 RID: 1124 RVA: 0x000196A0 File Offset: 0x000178A0
			private static int FindIntersection(in ClipperLib.IntPoint p1, in ClipperLib.IntPoint p2, in ClipperLib.IntPoint p3, in ClipperLib.IntPoint p4)
			{
				double num = (double)(p2.X - p1.X);
				double num2 = (double)(p2.Y - p1.Y);
				double num3 = (double)(p4.X - p3.X);
				double num4 = (double)(p4.Y - p3.Y);
				double num5 = num2 * num3 - num * num4;
				double num6 = ((double)(p1.X - p3.X) * num4 + (double)(p3.Y - p1.Y) * num3) / num5;
				if (double.IsInfinity(num6) || double.IsNaN(num6))
				{
					if (ConfinerOven.BakedSolution.<FindIntersection>g__IntPointDiffSqrMagnitude|10_0(p1, p3) < 1000.0 || ConfinerOven.BakedSolution.<FindIntersection>g__IntPointDiffSqrMagnitude|10_0(p1, p4) < 1000.0 || ConfinerOven.BakedSolution.<FindIntersection>g__IntPointDiffSqrMagnitude|10_0(p2, p3) < 1000.0 || ConfinerOven.BakedSolution.<FindIntersection>g__IntPointDiffSqrMagnitude|10_0(p2, p4) < 1000.0)
					{
						return 2;
					}
					return 0;
				}
				else
				{
					double num7 = ((double)(p3.X - p1.X) * num2 + (double)(p1.Y - p3.Y) * num) / -num5;
					if (num6 < 0.0 || num6 > 1.0 || num7 < 0.0 || num7 >= 1.0)
					{
						return 1;
					}
					return 2;
				}
			}

			// Token: 0x06000465 RID: 1125 RVA: 0x000197FC File Offset: 0x000179FC
			[CompilerGenerated]
			internal static ClipperLib.IntPoint <ConfinePoint>g__IntPointLerp|9_0(ClipperLib.IntPoint a, ClipperLib.IntPoint b, float lerp)
			{
				return new ClipperLib.IntPoint
				{
					X = (long)Mathf.RoundToInt((float)a.X + (float)(b.X - a.X) * lerp),
					Y = (long)Mathf.RoundToInt((float)a.Y + (float)(b.Y - a.Y) * lerp)
				};
			}

			// Token: 0x06000466 RID: 1126 RVA: 0x0001985C File Offset: 0x00017A5C
			[CompilerGenerated]
			private bool <ConfinePoint>g__IsInsideOriginal|9_1(ClipperLib.IntPoint point)
			{
				return this.m_OriginalPolygon.Any<List<ClipperLib.IntPoint>>((List<ClipperLib.IntPoint> t) => ClipperLib.Clipper.PointInPolygon(point, t) != 0);
			}

			// Token: 0x06000467 RID: 1127 RVA: 0x00019890 File Offset: 0x00017A90
			[CompilerGenerated]
			internal static float <ConfinePoint>g__ClosestPointOnSegment|9_2(ClipperLib.IntPoint point, ClipperLib.IntPoint s0, ClipperLib.IntPoint s1)
			{
				double num = (double)(s1.X - s0.X);
				double num2 = (double)(s1.Y - s0.Y);
				double num3 = num * num + num2 * num2;
				if (num3 < 1000.0)
				{
					return 0f;
				}
				float num4 = (float)((double)(point.X - s0.X));
				double num5 = (double)(point.Y - s0.Y);
				return Mathf.Clamp01((float)(((double)num4 * num + num5 * num2) / num3));
			}

			// Token: 0x06000468 RID: 1128 RVA: 0x00019900 File Offset: 0x00017B00
			[CompilerGenerated]
			private bool <ConfinePoint>g__DoesIntersectOriginal|9_3(ClipperLib.IntPoint l1, ClipperLib.IntPoint l2)
			{
				foreach (List<ClipperLib.IntPoint> list in this.m_OriginalPolygon)
				{
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						ClipperLib.IntPoint intPoint = list[i];
						ClipperLib.IntPoint intPoint2 = list[(i + 1) % count];
						if (ConfinerOven.BakedSolution.FindIntersection(in l1, in l2, in intPoint, in intPoint2) == 2)
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x06000469 RID: 1129 RVA: 0x00019990 File Offset: 0x00017B90
			[CompilerGenerated]
			internal static double <FindIntersection>g__IntPointDiffSqrMagnitude|10_0(ClipperLib.IntPoint point1, ClipperLib.IntPoint point2)
			{
				double num = (double)(point1.X - point2.X);
				double num2 = (double)(point1.Y - point2.Y);
				return num * num + num2 * num2;
			}

			// Token: 0x0400039B RID: 923
			private float m_FrustumSizeIntSpace;

			// Token: 0x0400039C RID: 924
			private readonly ConfinerOven.AspectStretcher m_AspectStretcher;

			// Token: 0x0400039D RID: 925
			private readonly bool m_HasBones;

			// Token: 0x0400039E RID: 926
			private readonly double m_SqrPolygonDiagonal;

			// Token: 0x0400039F RID: 927
			private List<List<ClipperLib.IntPoint>> m_OriginalPolygon;

			// Token: 0x040003A0 RID: 928
			private List<List<ClipperLib.IntPoint>> m_Solution;

			// Token: 0x040003A1 RID: 929
			private const double k_ClipperEpsilon = 1000.0;
		}

		// Token: 0x020000B5 RID: 181
		private readonly struct AspectStretcher
		{
			// Token: 0x170000E5 RID: 229
			// (get) Token: 0x0600046A RID: 1130 RVA: 0x000199C0 File Offset: 0x00017BC0
			public float Aspect { get; }

			// Token: 0x0600046B RID: 1131 RVA: 0x000199C8 File Offset: 0x00017BC8
			public AspectStretcher(float aspect, float centerX)
			{
				this.Aspect = aspect;
				this.m_InverseAspect = 1f / this.Aspect;
				this.m_CenterX = centerX;
			}

			// Token: 0x0600046C RID: 1132 RVA: 0x000199EA File Offset: 0x00017BEA
			public Vector2 Stretch(Vector2 p)
			{
				return new Vector2((p.x - this.m_CenterX) * this.m_InverseAspect + this.m_CenterX, p.y);
			}

			// Token: 0x0600046D RID: 1133 RVA: 0x00019A12 File Offset: 0x00017C12
			public Vector2 Unstretch(Vector2 p)
			{
				return new Vector2((p.x - this.m_CenterX) * this.Aspect + this.m_CenterX, p.y);
			}

			// Token: 0x040003A3 RID: 931
			private readonly float m_InverseAspect;

			// Token: 0x040003A4 RID: 932
			private readonly float m_CenterX;
		}

		// Token: 0x020000B6 RID: 182
		private struct PolygonSolution
		{
			// Token: 0x0600046E RID: 1134 RVA: 0x00019A3C File Offset: 0x00017C3C
			public bool StateChanged(in List<List<ClipperLib.IntPoint>> paths)
			{
				if (paths.Count != this.polygons.Count)
				{
					return true;
				}
				for (int i = 0; i < paths.Count; i++)
				{
					if (paths[i].Count != this.polygons[i].Count)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x170000E6 RID: 230
			// (get) Token: 0x0600046F RID: 1135 RVA: 0x00019A94 File Offset: 0x00017C94
			public bool IsNull
			{
				get
				{
					return this.polygons == null;
				}
			}

			// Token: 0x040003A5 RID: 933
			public List<List<ClipperLib.IntPoint>> polygons;

			// Token: 0x040003A6 RID: 934
			public float frustumHeight;
		}

		// Token: 0x020000B7 RID: 183
		public enum BakingState
		{
			// Token: 0x040003A8 RID: 936
			BAKING,
			// Token: 0x040003A9 RID: 937
			BAKED,
			// Token: 0x040003AA RID: 938
			TIMEOUT
		}

		// Token: 0x020000B8 RID: 184
		private struct BakingStateCache
		{
			// Token: 0x040003AB RID: 939
			public ClipperLib.ClipperOffset offsetter;

			// Token: 0x040003AC RID: 940
			public List<ConfinerOven.PolygonSolution> solutions;

			// Token: 0x040003AD RID: 941
			public ConfinerOven.PolygonSolution rightCandidate;

			// Token: 0x040003AE RID: 942
			public ConfinerOven.PolygonSolution leftCandidate;

			// Token: 0x040003AF RID: 943
			public List<List<ClipperLib.IntPoint>> maxCandidate;

			// Token: 0x040003B0 RID: 944
			public float stepSize;

			// Token: 0x040003B1 RID: 945
			public float maxFrustumHeight;

			// Token: 0x040003B2 RID: 946
			public float userSetMaxFrustumHeight;

			// Token: 0x040003B3 RID: 947
			public float theoriticalMaxFrustumHeight;

			// Token: 0x040003B4 RID: 948
			public float currentFrustumHeight;

			// Token: 0x040003B5 RID: 949
			public float bakeTime;
		}
	}
}
