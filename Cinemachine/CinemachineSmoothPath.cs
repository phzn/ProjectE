using System;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x0200001A RID: 26
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("Cinemachine/CinemachineSmoothPath")]
	[SaveDuringPlay]
	[DisallowMultipleComponent]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineSmoothPath.html")]
	public class CinemachineSmoothPath : CinemachinePathBase
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00008B19 File Offset: 0x00006D19
		public override float MinPos
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00008B20 File Offset: 0x00006D20
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

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00008B51 File Offset: 0x00006D51
		public override bool Looped
		{
			get
			{
				return this.m_Looped;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00008B59 File Offset: 0x00006D59
		public override int DistanceCacheSampleStepsPerSegment
		{
			get
			{
				return this.m_Resolution;
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00008B61 File Offset: 0x00006D61
		private void OnValidate()
		{
			this.InvalidateDistanceCache();
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00008B6C File Offset: 0x00006D6C
		private void Reset()
		{
			this.m_Looped = false;
			this.m_Waypoints = new CinemachineSmoothPath.Waypoint[]
			{
				new CinemachineSmoothPath.Waypoint
				{
					position = new Vector3(0f, 0f, -5f)
				},
				new CinemachineSmoothPath.Waypoint
				{
					position = new Vector3(0f, 0f, 5f)
				}
			};
			this.m_Appearance = new CinemachinePathBase.Appearance();
			this.InvalidateDistanceCache();
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00008BF3 File Offset: 0x00006DF3
		public override void InvalidateDistanceCache()
		{
			base.InvalidateDistanceCache();
			this.m_ControlPoints1 = null;
			this.m_ControlPoints2 = null;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00008C0C File Offset: 0x00006E0C
		internal void UpdateControlPoints()
		{
			int num = ((this.m_Waypoints == null) ? 0 : this.m_Waypoints.Length);
			if (num > 1 && (this.Looped != this.m_IsLoopedCache || this.m_ControlPoints1 == null || this.m_ControlPoints1.Length != num || this.m_ControlPoints2 == null || this.m_ControlPoints2.Length != num))
			{
				Vector4[] array = new Vector4[num];
				Vector4[] array2 = new Vector4[num];
				Vector4[] array3 = new Vector4[num];
				for (int i = 0; i < num; i++)
				{
					array3[i] = this.m_Waypoints[i].AsVector4;
				}
				if (this.Looped)
				{
					SplineHelpers.ComputeSmoothControlPointsLooped(ref array3, ref array, ref array2);
				}
				else
				{
					SplineHelpers.ComputeSmoothControlPoints(ref array3, ref array, ref array2);
				}
				this.m_ControlPoints1 = new CinemachineSmoothPath.Waypoint[num];
				this.m_ControlPoints2 = new CinemachineSmoothPath.Waypoint[num];
				for (int j = 0; j < num; j++)
				{
					this.m_ControlPoints1[j] = CinemachineSmoothPath.Waypoint.FromVector4(array[j]);
					this.m_ControlPoints2[j] = CinemachineSmoothPath.Waypoint.FromVector4(array2[j]);
				}
				this.m_IsLoopedCache = this.Looped;
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00008D34 File Offset: 0x00006F34
		private float GetBoundingIndices(float pos, out int indexA, out int indexB)
		{
			pos = this.StandardizePos(pos);
			int num = this.m_Waypoints.Length;
			if (num < 2)
			{
				indexA = (indexB = 0);
			}
			else
			{
				indexA = Mathf.FloorToInt(pos);
				if (indexA >= num)
				{
					pos -= this.MaxPos;
					indexA = 0;
				}
				indexB = indexA + 1;
				if (indexB == num)
				{
					if (this.Looped)
					{
						indexB = 0;
					}
					else
					{
						indexB--;
						indexA--;
					}
				}
			}
			return pos;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00008DA0 File Offset: 0x00006FA0
		public override Vector3 EvaluateLocalPosition(float pos)
		{
			Vector3 vector = Vector3.zero;
			if (this.m_Waypoints.Length != 0)
			{
				this.UpdateControlPoints();
				int num;
				int num2;
				pos = this.GetBoundingIndices(pos, out num, out num2);
				if (num == num2)
				{
					vector = this.m_Waypoints[num].position;
				}
				else
				{
					vector = SplineHelpers.Bezier3(pos - (float)num, this.m_Waypoints[num].position, this.m_ControlPoints1[num].position, this.m_ControlPoints2[num].position, this.m_Waypoints[num2].position);
				}
			}
			return vector;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00008E38 File Offset: 0x00007038
		public override Vector3 EvaluateLocalTangent(float pos)
		{
			Vector3 vector = Vector3.forward;
			if (this.m_Waypoints.Length > 1)
			{
				this.UpdateControlPoints();
				int num;
				int num2;
				pos = this.GetBoundingIndices(pos, out num, out num2);
				if (!this.Looped && num == this.m_Waypoints.Length - 1)
				{
					num--;
				}
				vector = SplineHelpers.BezierTangent3(pos - (float)num, this.m_Waypoints[num].position, this.m_ControlPoints1[num].position, this.m_ControlPoints2[num].position, this.m_Waypoints[num2].position);
			}
			return vector;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00008ED4 File Offset: 0x000070D4
		public override Quaternion EvaluateLocalOrientation(float pos)
		{
			Quaternion quaternion = Quaternion.identity;
			if (this.m_Waypoints.Length != 0)
			{
				int num;
				int num2;
				pos = this.GetBoundingIndices(pos, out num, out num2);
				float num3;
				if (num == num2)
				{
					num3 = this.m_Waypoints[num].roll;
				}
				else
				{
					this.UpdateControlPoints();
					num3 = SplineHelpers.Bezier1(pos - (float)num, this.m_Waypoints[num].roll, this.m_ControlPoints1[num].roll, this.m_ControlPoints2[num].roll, this.m_Waypoints[num2].roll);
				}
				Vector3 vector = this.EvaluateLocalTangent(pos);
				if (!vector.AlmostZero())
				{
					quaternion = Quaternion.LookRotation(vector) * CinemachineSmoothPath.RollAroundForward(num3);
				}
			}
			return quaternion;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00008F94 File Offset: 0x00007194
		private static Quaternion RollAroundForward(float angle)
		{
			float num = angle * 0.5f * 0.017453292f;
			return new Quaternion(0f, 0f, Mathf.Sin(num), Mathf.Cos(num));
		}

		// Token: 0x040000AF RID: 175
		[Tooltip("If checked, then the path ends are joined to form a continuous loop.")]
		public bool m_Looped;

		// Token: 0x040000B0 RID: 176
		[Tooltip("The waypoints that define the path.  They will be interpolated using a bezier curve.")]
		public CinemachineSmoothPath.Waypoint[] m_Waypoints = Array.Empty<CinemachineSmoothPath.Waypoint>();

		// Token: 0x040000B1 RID: 177
		internal CinemachineSmoothPath.Waypoint[] m_ControlPoints1;

		// Token: 0x040000B2 RID: 178
		internal CinemachineSmoothPath.Waypoint[] m_ControlPoints2;

		// Token: 0x040000B3 RID: 179
		private bool m_IsLoopedCache;

		// Token: 0x02000085 RID: 133
		[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
		[Serializable]
		public struct Waypoint
		{
			// Token: 0x170000E4 RID: 228
			// (get) Token: 0x0600042D RID: 1069 RVA: 0x00018CEC File Offset: 0x00016EEC
			internal Vector4 AsVector4
			{
				get
				{
					return new Vector4(this.position.x, this.position.y, this.position.z, this.roll);
				}
			}

			// Token: 0x0600042E RID: 1070 RVA: 0x00018D1C File Offset: 0x00016F1C
			internal static CinemachineSmoothPath.Waypoint FromVector4(Vector4 v)
			{
				return new CinemachineSmoothPath.Waypoint
				{
					position = new Vector3(v[0], v[1], v[2]),
					roll = v[3]
				};
			}

			// Token: 0x040002F2 RID: 754
			[Tooltip("Position in path-local space")]
			public Vector3 position;

			// Token: 0x040002F3 RID: 755
			[Tooltip("Defines the roll of the path at this waypoint.  The other orientation axes are inferred from the tangent and world up.")]
			public float roll;
		}
	}
}
