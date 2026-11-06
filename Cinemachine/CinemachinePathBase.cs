using System;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000039 RID: 57
	public abstract class CinemachinePathBase : MonoBehaviour
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002A9 RID: 681
		public abstract float MinPos { get; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002AA RID: 682
		public abstract float MaxPos { get; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002AB RID: 683
		public abstract bool Looped { get; }

		// Token: 0x060002AC RID: 684 RVA: 0x000127B4 File Offset: 0x000109B4
		public virtual float StandardizePos(float pos)
		{
			if (this.Looped && this.MaxPos > 0f)
			{
				pos %= this.MaxPos;
				if (pos < 0f)
				{
					pos += this.MaxPos;
				}
				return pos;
			}
			return Mathf.Clamp(pos, 0f, this.MaxPos);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00012805 File Offset: 0x00010A05
		public virtual Vector3 EvaluatePosition(float pos)
		{
			return base.transform.TransformPoint(this.EvaluateLocalPosition(pos));
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00012819 File Offset: 0x00010A19
		public virtual Vector3 EvaluateTangent(float pos)
		{
			return base.transform.TransformDirection(this.EvaluateLocalTangent(pos));
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0001282D File Offset: 0x00010A2D
		public virtual Quaternion EvaluateOrientation(float pos)
		{
			return base.transform.rotation * this.EvaluateLocalOrientation(pos);
		}

		// Token: 0x060002B0 RID: 688
		public abstract Vector3 EvaluateLocalPosition(float pos);

		// Token: 0x060002B1 RID: 689
		public abstract Vector3 EvaluateLocalTangent(float pos);

		// Token: 0x060002B2 RID: 690
		public abstract Quaternion EvaluateLocalOrientation(float pos);

		// Token: 0x060002B3 RID: 691 RVA: 0x00012848 File Offset: 0x00010A48
		public virtual float FindClosestPoint(Vector3 p, int startSegment, int searchRadius, int stepsPerSegment)
		{
			float num = this.MinPos;
			float num2 = this.MaxPos;
			if (searchRadius >= 0)
			{
				if (this.Looped)
				{
					int num3 = Mathf.Min(searchRadius, Mathf.FloorToInt((num2 - num) / 2f));
					num = (float)(startSegment - num3);
					num2 = (float)(startSegment + num3 + 1);
				}
				else
				{
					num = Mathf.Max((float)(startSegment - searchRadius), this.MinPos);
					num2 = Mathf.Min((float)(startSegment + searchRadius + 1), this.MaxPos);
				}
			}
			stepsPerSegment = Mathf.RoundToInt(Mathf.Clamp((float)stepsPerSegment, 1f, 100f));
			float num4 = 1f / (float)stepsPerSegment;
			float num5 = (float)startSegment;
			float num6 = float.MaxValue;
			int num7 = ((stepsPerSegment == 1) ? 1 : 3);
			for (int i = 0; i < num7; i++)
			{
				Vector3 vector = this.EvaluatePosition(num);
				for (float num8 = num + num4; num8 <= num2; num8 += num4)
				{
					Vector3 vector2 = this.EvaluatePosition(num8);
					float num9 = p.ClosestPointOnSegment(vector, vector2);
					float num10 = Vector3.SqrMagnitude(p - Vector3.Lerp(vector, vector2, num9));
					if (num10 < num6)
					{
						num6 = num10;
						num5 = num8 - (1f - num9) * num4;
					}
					vector = vector2;
				}
				num = num5 - num4;
				num2 = num5 + num4;
				num4 /= (float)stepsPerSegment;
			}
			return num5;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00012974 File Offset: 0x00010B74
		public float MinUnit(CinemachinePathBase.PositionUnits units)
		{
			if (units == CinemachinePathBase.PositionUnits.Normalized)
			{
				return 0f;
			}
			if (units != CinemachinePathBase.PositionUnits.Distance)
			{
				return this.MinPos;
			}
			return 0f;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00012990 File Offset: 0x00010B90
		public float MaxUnit(CinemachinePathBase.PositionUnits units)
		{
			if (units == CinemachinePathBase.PositionUnits.Normalized)
			{
				return 1f;
			}
			if (units != CinemachinePathBase.PositionUnits.Distance)
			{
				return this.MaxPos;
			}
			return this.PathLength;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x000129B0 File Offset: 0x00010BB0
		public virtual float StandardizeUnit(float pos, CinemachinePathBase.PositionUnits units)
		{
			if (units == CinemachinePathBase.PositionUnits.PathUnits)
			{
				return this.StandardizePos(pos);
			}
			if (units == CinemachinePathBase.PositionUnits.Distance)
			{
				return this.StandardizePathDistance(pos);
			}
			float pathLength = this.PathLength;
			if (pathLength < 0.0001f)
			{
				return 0f;
			}
			return this.StandardizePathDistance(pos * pathLength) / pathLength;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x000129F4 File Offset: 0x00010BF4
		public Vector3 EvaluatePositionAtUnit(float pos, CinemachinePathBase.PositionUnits units)
		{
			return this.EvaluatePosition(this.ToNativePathUnits(pos, units));
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00012A04 File Offset: 0x00010C04
		public Vector3 EvaluateTangentAtUnit(float pos, CinemachinePathBase.PositionUnits units)
		{
			return this.EvaluateTangent(this.ToNativePathUnits(pos, units));
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00012A14 File Offset: 0x00010C14
		public Quaternion EvaluateOrientationAtUnit(float pos, CinemachinePathBase.PositionUnits units)
		{
			return this.EvaluateOrientation(this.ToNativePathUnits(pos, units));
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002BA RID: 698
		public abstract int DistanceCacheSampleStepsPerSegment { get; }

		// Token: 0x060002BB RID: 699 RVA: 0x00012A24 File Offset: 0x00010C24
		public virtual void InvalidateDistanceCache()
		{
			this.m_DistanceToPos = null;
			this.m_PosToDistance = null;
			this.m_CachedSampleSteps = 0;
			this.m_PathLength = 0f;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00012A46 File Offset: 0x00010C46
		public bool DistanceCacheIsValid()
		{
			return this.MaxPos == this.MinPos || (this.m_DistanceToPos != null && this.m_PosToDistance != null && this.m_CachedSampleSteps == this.DistanceCacheSampleStepsPerSegment && this.m_CachedSampleSteps > 0);
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002BD RID: 701 RVA: 0x00012A81 File Offset: 0x00010C81
		public float PathLength
		{
			get
			{
				if (this.DistanceCacheSampleStepsPerSegment < 1)
				{
					return 0f;
				}
				if (!this.DistanceCacheIsValid())
				{
					this.ResamplePath(this.DistanceCacheSampleStepsPerSegment);
				}
				return this.m_PathLength;
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00012AAC File Offset: 0x00010CAC
		public float StandardizePathDistance(float distance)
		{
			float pathLength = this.PathLength;
			if (pathLength < 1E-05f)
			{
				return 0f;
			}
			if (this.Looped)
			{
				distance %= pathLength;
				if (distance < 0f)
				{
					distance += pathLength;
				}
			}
			return Mathf.Clamp(distance, 0f, pathLength);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00012AF4 File Offset: 0x00010CF4
		public float ToNativePathUnits(float pos, CinemachinePathBase.PositionUnits units)
		{
			if (units == CinemachinePathBase.PositionUnits.PathUnits)
			{
				return pos;
			}
			if (this.DistanceCacheSampleStepsPerSegment < 1 || this.PathLength < 0.0001f)
			{
				return this.MinPos;
			}
			if (units == CinemachinePathBase.PositionUnits.Normalized)
			{
				pos *= this.PathLength;
			}
			pos = this.StandardizePathDistance(pos);
			float num = pos / this.m_cachedDistanceStepSize;
			int num2 = Mathf.FloorToInt(num);
			if (num2 >= this.m_DistanceToPos.Length - 1)
			{
				return this.MaxPos;
			}
			float num3 = num - (float)num2;
			return this.MinPos + Mathf.Lerp(this.m_DistanceToPos[num2], this.m_DistanceToPos[num2 + 1], num3);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00012B84 File Offset: 0x00010D84
		public float FromPathNativeUnits(float pos, CinemachinePathBase.PositionUnits units)
		{
			if (units == CinemachinePathBase.PositionUnits.PathUnits)
			{
				return pos;
			}
			float pathLength = this.PathLength;
			if (this.DistanceCacheSampleStepsPerSegment < 1 || pathLength < 0.0001f)
			{
				return 0f;
			}
			pos = this.StandardizePos(pos);
			float num = pos / this.m_cachedPosStepSize;
			int num2 = Mathf.FloorToInt(num);
			if (num2 >= this.m_PosToDistance.Length - 1)
			{
				pos = this.m_PathLength;
			}
			else
			{
				float num3 = num - (float)num2;
				pos = Mathf.Lerp(this.m_PosToDistance[num2], this.m_PosToDistance[num2 + 1], num3);
			}
			if (units == CinemachinePathBase.PositionUnits.Normalized)
			{
				pos /= pathLength;
			}
			return pos;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00012C10 File Offset: 0x00010E10
		private void ResamplePath(int stepsPerSegment)
		{
			this.InvalidateDistanceCache();
			float minPos = this.MinPos;
			float maxPos = this.MaxPos;
			float num = 1f / (float)Mathf.Max(1, stepsPerSegment);
			int num2 = Mathf.RoundToInt((maxPos - minPos) / num) + 1;
			this.m_PosToDistance = new float[num2];
			this.m_CachedSampleSteps = stepsPerSegment;
			this.m_cachedPosStepSize = num;
			Vector3 vector = this.EvaluatePosition(0f);
			this.m_PosToDistance[0] = 0f;
			float num3 = minPos;
			for (int i = 1; i < num2; i++)
			{
				num3 += num;
				Vector3 vector2 = this.EvaluatePosition(num3);
				float num4 = Vector3.Distance(vector, vector2);
				this.m_PathLength += num4;
				vector = vector2;
				this.m_PosToDistance[i] = this.m_PathLength;
			}
			this.m_DistanceToPos = new float[num2];
			this.m_DistanceToPos[0] = 0f;
			if (num2 > 1)
			{
				num = this.m_PathLength / (float)(num2 - 1);
				this.m_cachedDistanceStepSize = num;
				float num5 = 0f;
				int num6 = 1;
				for (int j = 1; j < num2; j++)
				{
					num5 += num;
					float num7 = this.m_PosToDistance[num6];
					while (num7 < num5 && num6 < num2 - 1)
					{
						num7 = this.m_PosToDistance[++num6];
					}
					float num8 = this.m_PosToDistance[num6 - 1];
					float num9 = num7 - num8;
					float num10 = (num5 - num8) / num9;
					this.m_DistanceToPos[j] = this.m_cachedPosStepSize * (num10 + (float)num6 - 1f);
				}
			}
		}

		// Token: 0x040001EC RID: 492
		[Tooltip("Path samples per waypoint.  This is used for calculating path distances.")]
		[Range(1f, 100f)]
		public int m_Resolution = 20;

		// Token: 0x040001ED RID: 493
		[Tooltip("The settings that control how the path will appear in the editor scene view.")]
		public CinemachinePathBase.Appearance m_Appearance = new CinemachinePathBase.Appearance();

		// Token: 0x040001EE RID: 494
		private float[] m_DistanceToPos;

		// Token: 0x040001EF RID: 495
		private float[] m_PosToDistance;

		// Token: 0x040001F0 RID: 496
		private int m_CachedSampleSteps;

		// Token: 0x040001F1 RID: 497
		private float m_PathLength;

		// Token: 0x040001F2 RID: 498
		private float m_cachedPosStepSize;

		// Token: 0x040001F3 RID: 499
		private float m_cachedDistanceStepSize;

		// Token: 0x020000AD RID: 173
		[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
		[Serializable]
		public class Appearance
		{
			// Token: 0x04000382 RID: 898
			[Tooltip("The color of the path itself when it is active in the editor")]
			public Color pathColor = Color.green;

			// Token: 0x04000383 RID: 899
			[Tooltip("The color of the path itself when it is inactive in the editor")]
			public Color inactivePathColor = Color.gray;

			// Token: 0x04000384 RID: 900
			[Tooltip("The width of the railroad-tracks that are drawn to represent the path")]
			[Range(0f, 10f)]
			public float width = 0.2f;
		}

		// Token: 0x020000AE RID: 174
		public enum PositionUnits
		{
			// Token: 0x04000386 RID: 902
			PathUnits,
			// Token: 0x04000387 RID: 903
			Distance,
			// Token: 0x04000388 RID: 904
			Normalized
		}
	}
}
