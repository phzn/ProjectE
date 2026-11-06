using System;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000017 RID: 23
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("Cinemachine/CinemachinePath")]
	[SaveDuringPlay]
	[DisallowMultipleComponent]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachinePath.html")]
	public class CinemachinePath : CinemachinePathBase
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x000086AE File Offset: 0x000068AE
		public override float MinPos
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x000086B8 File Offset: 0x000068B8
		public override float MaxPos
		{
			get
			{
				int num = this.m_Waypoints.Length - 1;
				if (num < 1)
				{
					return 0f;
				}
				return (float)(this.m_Looped ? (num + 1) : num);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000FA RID: 250 RVA: 0x000086E9 File Offset: 0x000068E9
		public override bool Looped
		{
			get
			{
				return this.m_Looped;
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000086F4 File Offset: 0x000068F4
		private void Reset()
		{
			this.m_Looped = false;
			this.m_Waypoints = new CinemachinePath.Waypoint[]
			{
				new CinemachinePath.Waypoint
				{
					position = new Vector3(0f, 0f, -5f),
					tangent = new Vector3(1f, 0f, 0f)
				},
				new CinemachinePath.Waypoint
				{
					position = new Vector3(0f, 0f, 5f),
					tangent = new Vector3(1f, 0f, 0f)
				}
			};
			this.m_Appearance = new CinemachinePathBase.Appearance();
			this.InvalidateDistanceCache();
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000087B1 File Offset: 0x000069B1
		private void OnValidate()
		{
			this.InvalidateDistanceCache();
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000FD RID: 253 RVA: 0x000087B9 File Offset: 0x000069B9
		public override int DistanceCacheSampleStepsPerSegment
		{
			get
			{
				return this.m_Resolution;
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000087C4 File Offset: 0x000069C4
		private float GetBoundingIndices(float pos, out int indexA, out int indexB)
		{
			pos = this.StandardizePos(pos);
			int num = Mathf.RoundToInt(pos);
			if (Mathf.Abs(pos - (float)num) < 0.0001f)
			{
				indexA = (indexB = ((num == this.m_Waypoints.Length) ? 0 : num));
			}
			else
			{
				indexA = Mathf.FloorToInt(pos);
				if (indexA >= this.m_Waypoints.Length)
				{
					pos -= this.MaxPos;
					indexA = 0;
				}
				indexB = Mathf.CeilToInt(pos);
				if (indexB >= this.m_Waypoints.Length)
				{
					indexB = 0;
				}
			}
			return pos;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00008844 File Offset: 0x00006A44
		public override Vector3 EvaluateLocalPosition(float pos)
		{
			Vector3 vector = Vector3.zero;
			if (this.m_Waypoints.Length != 0)
			{
				int num;
				int num2;
				pos = this.GetBoundingIndices(pos, out num, out num2);
				if (num == num2)
				{
					vector = this.m_Waypoints[num].position;
				}
				else
				{
					CinemachinePath.Waypoint waypoint = this.m_Waypoints[num];
					CinemachinePath.Waypoint waypoint2 = this.m_Waypoints[num2];
					vector = SplineHelpers.Bezier3(pos - (float)num, this.m_Waypoints[num].position, waypoint.position + waypoint.tangent, waypoint2.position - waypoint2.tangent, waypoint2.position);
				}
			}
			return vector;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000088EC File Offset: 0x00006AEC
		public override Vector3 EvaluateLocalTangent(float pos)
		{
			Vector3 vector = Vector3.forward;
			if (this.m_Waypoints.Length != 0)
			{
				int num;
				int num2;
				pos = this.GetBoundingIndices(pos, out num, out num2);
				if (num == num2)
				{
					vector = this.m_Waypoints[num].tangent;
				}
				else
				{
					CinemachinePath.Waypoint waypoint = this.m_Waypoints[num];
					CinemachinePath.Waypoint waypoint2 = this.m_Waypoints[num2];
					vector = SplineHelpers.BezierTangent3(pos - (float)num, this.m_Waypoints[num].position, waypoint.position + waypoint.tangent, waypoint2.position - waypoint2.tangent, waypoint2.position);
				}
			}
			return vector;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00008994 File Offset: 0x00006B94
		public override Quaternion EvaluateLocalOrientation(float pos)
		{
			Quaternion quaternion = Quaternion.identity;
			if (this.m_Waypoints.Length != 0)
			{
				int num;
				int num2;
				pos = this.GetBoundingIndices(pos, out num, out num2);
				Vector3 vector = this.EvaluateLocalTangent(pos);
				if (!vector.AlmostZero())
				{
					quaternion = Quaternion.LookRotation(vector) * CinemachinePath.RollAroundForward(this.GetRoll(num, num2, pos));
				}
			}
			return quaternion;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000089E8 File Offset: 0x00006BE8
		internal float GetRoll(int indexA, int indexB, float standardizedPos)
		{
			if (indexA == indexB)
			{
				return this.m_Waypoints[indexA].roll;
			}
			float num = this.m_Waypoints[indexA].roll;
			float num2 = this.m_Waypoints[indexB].roll;
			if (indexB == 0)
			{
				num %= 360f;
				num2 %= 360f;
			}
			return Mathf.Lerp(num, num2, standardizedPos - (float)indexA);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00008A50 File Offset: 0x00006C50
		private static Quaternion RollAroundForward(float angle)
		{
			float num = angle * 0.5f * 0.017453292f;
			return new Quaternion(0f, 0f, Mathf.Sin(num), Mathf.Cos(num));
		}

		// Token: 0x040000AD RID: 173
		[Tooltip("If checked, then the path ends are joined to form a continuous loop.")]
		public bool m_Looped;

		// Token: 0x040000AE RID: 174
		[Tooltip("The waypoints that define the path.  They will be interpolated using a bezier curve.")]
		public CinemachinePath.Waypoint[] m_Waypoints = Array.Empty<CinemachinePath.Waypoint>();

		// Token: 0x02000084 RID: 132
		[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
		[Serializable]
		public struct Waypoint
		{
			// Token: 0x040002EF RID: 751
			[Tooltip("Position in path-local space")]
			public Vector3 position;

			// Token: 0x040002F0 RID: 752
			[Tooltip("Offset from the position, which defines the tangent of the curve at the waypoint.  The length of the tangent encodes the strength of the bezier handle.  The same handle is used symmetrically on both sides of the waypoint, to ensure smoothness.")]
			public Vector3 tangent;

			// Token: 0x040002F1 RID: 753
			[Tooltip("Defines the roll of the path at this waypoint.  The other orientation axes are inferred from the tangent and world up.")]
			public float roll;
		}
	}
}
