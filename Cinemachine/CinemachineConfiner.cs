using System;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000010 RID: 16
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("")]
	[SaveDuringPlay]
	[ExecuteAlways]
	[DisallowMultipleComponent]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineConfiner.html")]
	public class CinemachineConfiner : CinemachineExtension
	{
		// Token: 0x06000095 RID: 149 RVA: 0x00005DBA File Offset: 0x00003FBA
		public bool CameraWasDisplaced(CinemachineVirtualCameraBase vcam)
		{
			return this.GetCameraDisplacementDistance(vcam) > 0f;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00005DCA File Offset: 0x00003FCA
		public float GetCameraDisplacementDistance(CinemachineVirtualCameraBase vcam)
		{
			return base.GetExtraState<CinemachineConfiner.VcamExtraState>(vcam).confinerDisplacement;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00005DD8 File Offset: 0x00003FD8
		private void OnValidate()
		{
			this.m_Damping = Mathf.Max(0f, this.m_Damping);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00005DF0 File Offset: 0x00003FF0
		protected override void ConnectToVcam(bool connect)
		{
			base.ConnectToVcam(connect);
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00005DFC File Offset: 0x00003FFC
		public bool IsValid
		{
			get
			{
				return (this.m_ConfineMode == CinemachineConfiner.Mode.Confine3D && this.m_BoundingVolume != null && this.m_BoundingVolume.enabled && this.m_BoundingVolume.gameObject.activeInHierarchy) || (this.m_ConfineMode == CinemachineConfiner.Mode.Confine2D && this.m_BoundingShape2D != null && this.m_BoundingShape2D.enabled && this.m_BoundingShape2D.gameObject.activeInHierarchy);
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00005E76 File Offset: 0x00004076
		public override float GetMaxDampTime()
		{
			return this.m_Damping;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00005E80 File Offset: 0x00004080
		protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{
			if (this.IsValid && stage == CinemachineCore.Stage.Body)
			{
				CinemachineConfiner.VcamExtraState extraState = base.GetExtraState<CinemachineConfiner.VcamExtraState>(vcam);
				Vector3 vector;
				if (this.m_ConfineScreenEdges && state.Lens.Orthographic)
				{
					vector = this.ConfineScreenEdges(ref state);
				}
				else
				{
					vector = this.ConfinePoint(state.CorrectedPosition);
				}
				if (this.m_Damping > 0f && deltaTime >= 0f && base.VirtualCamera.PreviousStateIsValid)
				{
					Vector3 vector2 = vector - extraState.m_previousDisplacement;
					vector2 = Damper.Damp(vector2, this.m_Damping, deltaTime);
					vector = extraState.m_previousDisplacement + vector2;
				}
				extraState.m_previousDisplacement = vector;
				state.PositionCorrection += vector;
				extraState.confinerDisplacement = vector.magnitude;
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00005F49 File Offset: 0x00004149
		public void InvalidatePathCache()
		{
			this.m_pathCache = null;
			this.m_BoundingShape2DCache = null;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00005F5C File Offset: 0x0000415C
		private bool ValidatePathCache()
		{
			if (this.m_BoundingShape2DCache != this.m_BoundingShape2D)
			{
				this.InvalidatePathCache();
				this.m_BoundingShape2DCache = this.m_BoundingShape2D;
			}
			Type type = ((this.m_BoundingShape2D == null) ? null : this.m_BoundingShape2D.GetType());
			if (type == typeof(PolygonCollider2D))
			{
				PolygonCollider2D polygonCollider2D = this.m_BoundingShape2D as PolygonCollider2D;
				if (this.m_pathCache == null || this.m_pathCache.Count != polygonCollider2D.pathCount || this.m_pathTotalPointCount != polygonCollider2D.GetTotalPointCount())
				{
					this.m_pathCache = new List<List<Vector2>>();
					for (int i = 0; i < polygonCollider2D.pathCount; i++)
					{
						Vector2[] path = polygonCollider2D.GetPath(i);
						List<Vector2> list = new List<Vector2>();
						for (int j = 0; j < path.Length; j++)
						{
							list.Add(path[j]);
						}
						this.m_pathCache.Add(list);
					}
					this.m_pathTotalPointCount = polygonCollider2D.GetTotalPointCount();
				}
				return true;
			}
			if (type == typeof(CompositeCollider2D))
			{
				CompositeCollider2D compositeCollider2D = this.m_BoundingShape2D as CompositeCollider2D;
				if (this.m_pathCache == null || this.m_pathCache.Count != compositeCollider2D.pathCount || this.m_pathTotalPointCount != compositeCollider2D.pointCount)
				{
					this.m_pathCache = new List<List<Vector2>>();
					Vector2[] array = new Vector2[compositeCollider2D.pointCount];
					Vector3 lossyScale = this.m_BoundingShape2D.transform.lossyScale;
					Vector2 vector = new Vector2(1f / lossyScale.x, 1f / lossyScale.y);
					for (int k = 0; k < compositeCollider2D.pathCount; k++)
					{
						int path2 = compositeCollider2D.GetPath(k, array);
						List<Vector2> list2 = new List<Vector2>();
						for (int l = 0; l < path2; l++)
						{
							list2.Add(array[l] * vector);
						}
						this.m_pathCache.Add(list2);
					}
					this.m_pathTotalPointCount = compositeCollider2D.pointCount;
				}
				return true;
			}
			this.InvalidatePathCache();
			return false;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00006170 File Offset: 0x00004370
		private Vector3 ConfinePoint(Vector3 camPos)
		{
			if (this.m_ConfineMode == CinemachineConfiner.Mode.Confine3D)
			{
				return this.m_BoundingVolume.ClosestPoint(camPos) - camPos;
			}
			Vector2 vector = camPos;
			Vector2 vector2 = vector;
			if (this.m_BoundingShape2D.OverlapPoint(camPos))
			{
				return Vector3.zero;
			}
			if (!this.ValidatePathCache())
			{
				return Vector3.zero;
			}
			float num = float.MaxValue;
			for (int i = 0; i < this.m_pathCache.Count; i++)
			{
				int count = this.m_pathCache[i].Count;
				if (count > 0)
				{
					Vector2 vector3 = this.m_BoundingShape2D.transform.TransformPoint(this.m_pathCache[i][count - 1] + this.m_BoundingShape2D.offset);
					for (int j = 0; j < count; j++)
					{
						Vector2 vector4 = this.m_BoundingShape2D.transform.TransformPoint(this.m_pathCache[i][j] + this.m_BoundingShape2D.offset);
						Vector2 vector5 = Vector2.Lerp(vector3, vector4, vector.ClosestPointOnSegment(vector3, vector4));
						float num2 = Vector2.SqrMagnitude(vector - vector5);
						if (num2 < num)
						{
							num = num2;
							vector2 = vector5;
						}
						vector3 = vector4;
					}
				}
			}
			return vector2 - vector;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000062D8 File Offset: 0x000044D8
		private Vector3 ConfineScreenEdges(ref CameraState state)
		{
			Quaternion correctedOrientation = state.CorrectedOrientation;
			float orthographicSize = state.Lens.OrthographicSize;
			float num = orthographicSize * state.Lens.Aspect;
			Vector3 vector = correctedOrientation * Vector3.right * num;
			Vector3 vector2 = correctedOrientation * Vector3.up * orthographicSize;
			Vector3 vector3 = Vector3.zero;
			Vector3 vector4 = state.CorrectedPosition;
			Vector3 vector5 = Vector3.zero;
			for (int i = 0; i < 12; i++)
			{
				Vector3 vector6 = this.ConfinePoint(vector4 - vector2 - vector);
				if (vector6.AlmostZero())
				{
					vector6 = this.ConfinePoint(vector4 + vector2 + vector);
				}
				if (vector6.AlmostZero())
				{
					vector6 = this.ConfinePoint(vector4 - vector2 + vector);
				}
				if (vector6.AlmostZero())
				{
					vector6 = this.ConfinePoint(vector4 + vector2 - vector);
				}
				if (vector6.AlmostZero())
				{
					break;
				}
				if ((vector6 + vector5).AlmostZero())
				{
					vector3 += vector6 * 0.5f;
					break;
				}
				vector3 += vector6;
				vector4 += vector6;
				vector5 = vector6;
			}
			return vector3;
		}

		// Token: 0x04000062 RID: 98
		[Tooltip("The confiner can operate using a 2D bounding shape or a 3D bounding volume")]
		public CinemachineConfiner.Mode m_ConfineMode;

		// Token: 0x04000063 RID: 99
		[Tooltip("The volume within which the camera is to be contained")]
		public Collider m_BoundingVolume;

		// Token: 0x04000064 RID: 100
		[Tooltip("The 2D shape within which the camera is to be contained")]
		public Collider2D m_BoundingShape2D;

		// Token: 0x04000065 RID: 101
		private Collider2D m_BoundingShape2DCache;

		// Token: 0x04000066 RID: 102
		[Tooltip("If camera is orthographic, screen edges will be confined to the volume.  If not checked, then only the camera center will be confined")]
		public bool m_ConfineScreenEdges = true;

		// Token: 0x04000067 RID: 103
		[Tooltip("How gradually to return the camera to the bounding volume if it goes beyond the borders.  Higher numbers are more gradual.")]
		[Range(0f, 10f)]
		public float m_Damping;

		// Token: 0x04000068 RID: 104
		private List<List<Vector2>> m_pathCache;

		// Token: 0x04000069 RID: 105
		private int m_pathTotalPointCount;

		// Token: 0x02000079 RID: 121
		public enum Mode
		{
			// Token: 0x040002D2 RID: 722
			Confine2D,
			// Token: 0x040002D3 RID: 723
			Confine3D
		}

		// Token: 0x0200007A RID: 122
		private class VcamExtraState
		{
			// Token: 0x040002D4 RID: 724
			public Vector3 m_previousDisplacement;

			// Token: 0x040002D5 RID: 725
			public float confinerDisplacement;
		}
	}
}
