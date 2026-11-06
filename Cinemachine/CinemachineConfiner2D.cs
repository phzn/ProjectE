using System;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000011 RID: 17
	[AddComponentMenu("")]
	[SaveDuringPlay]
	[ExecuteAlways]
	[DisallowMultipleComponent]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineConfiner2D.html")]
	public class CinemachineConfiner2D : CinemachineExtension
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x00006423 File Offset: 0x00004623
		public void InvalidateCache()
		{
			this.m_shapeCache.Invalidate();
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00006430 File Offset: 0x00004630
		public bool ValidateCache(float cameraAspectRatio)
		{
			bool flag;
			return this.m_shapeCache.ValidateCache(this.m_BoundingShape2D, this.m_MaxWindowSize, cameraAspectRatio, this.m_Padding, out flag);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00006460 File Offset: 0x00004660
		protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{
			if (stage == CinemachineCore.Stage.Body)
			{
				float aspect = state.Lens.Aspect;
				bool flag;
				if (!this.m_shapeCache.ValidateCache(this.m_BoundingShape2D, this.m_MaxWindowSize, aspect, this.m_Padding, out flag))
				{
					return;
				}
				Vector3 correctedPosition = state.CorrectedPosition;
				Vector3 vector = this.m_shapeCache.m_DeltaWorldToBaked.MultiplyPoint3x4(correctedPosition);
				float num = this.CalculateHalfFrustumHeight(in state, in vector.z) * this.m_shapeCache.m_DeltaWorldToBaked.lossyScale.x;
				CinemachineConfiner2D.VcamExtraState extraState = base.GetExtraState<CinemachineConfiner2D.VcamExtraState>(vcam);
				extraState.m_vcam = vcam;
				if (flag || extraState.m_BakedSolution == null || !extraState.m_BakedSolution.IsValid())
				{
					extraState.m_BakedSolution = this.m_shapeCache.m_confinerOven.GetBakedSolution(num);
				}
				ConfinerOven.BakedSolution bakedSolution = extraState.m_BakedSolution;
				Vector2 vector2 = vector;
				vector = bakedSolution.ConfinePoint(in vector2);
				Vector3 vector3 = this.m_shapeCache.m_DeltaBakedToWorld.MultiplyPoint3x4(vector);
				Vector3 vector4 = state.CorrectedOrientation * Vector3.forward;
				vector3 -= vector4 * Vector3.Dot(vector4, vector3 - correctedPosition);
				Vector3 previousDisplacement = extraState.m_PreviousDisplacement;
				Vector3 vector5 = vector3 - correctedPosition;
				extraState.m_PreviousDisplacement = vector5;
				if (!base.VirtualCamera.PreviousStateIsValid || deltaTime < 0f || this.m_Damping <= 0f)
				{
					extraState.m_DampedDisplacement = Vector3.zero;
				}
				else
				{
					if (previousDisplacement.sqrMagnitude > 0.01f && Vector2.Angle(previousDisplacement, vector5) > 10f)
					{
						extraState.m_DampedDisplacement += vector5 - previousDisplacement;
					}
					extraState.m_DampedDisplacement -= Damper.Damp(extraState.m_DampedDisplacement, this.m_Damping, deltaTime);
					vector5 -= extraState.m_DampedDisplacement;
				}
				state.PositionCorrection += vector5;
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00006664 File Offset: 0x00004864
		private float CalculateHalfFrustumHeight(in CameraState state, in float cameraPosLocalZ)
		{
			LensSettings lens = state.Lens;
			float num;
			if (lens.Orthographic)
			{
				num = state.Lens.OrthographicSize;
			}
			else
			{
				num = cameraPosLocalZ * Mathf.Tan(state.Lens.FieldOfView * 0.5f * 0.017453292f);
			}
			return Mathf.Abs(num);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000066B5 File Offset: 0x000048B5
		private void OnValidate()
		{
			this.m_Damping = Mathf.Max(0f, this.m_Damping);
			this.m_shapeCache.m_maxComputationTimePerFrameInSeconds = this.m_MaxComputationTimePerFrameInSeconds;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000066DE File Offset: 0x000048DE
		private void Reset()
		{
			this.m_Damping = 0.5f;
			this.m_MaxWindowSize = -1f;
		}

		// Token: 0x0400006A RID: 106
		[Tooltip("The 2D shape within which the camera is to be contained.  Can be a 2D polygon or 2D composite collider.")]
		public Collider2D m_BoundingShape2D;

		// Token: 0x0400006B RID: 107
		[Tooltip("Damping applied around corners to avoid jumps.  Higher numbers are more gradual.")]
		[Range(0f, 5f)]
		public float m_Damping;

		// Token: 0x0400006C RID: 108
		[Tooltip("To optimize computation and memory costs, set this to the largest view size that the camera is expected to have.  The confiner will not compute a polygon cache for frustum sizes larger than this.  This refers to the size in world units of the frustum at the confiner plane (for orthographic cameras, this is just the orthographic size).  If set to 0, then this parameter is ignored and a polygon cache will be calculated for all potential window sizes.")]
		public float m_MaxWindowSize;

		// Token: 0x0400006D RID: 109
		[Tooltip("For large window sizes, the confiner will potentially generate polygons with zero area.  The padding may be used to add a small amount of area to these polygons, to prevent them from being a series of disconnected dots.")]
		[Range(0f, 100f)]
		public float m_Padding;

		// Token: 0x0400006E RID: 110
		private float m_MaxComputationTimePerFrameInSeconds = 0.008333334f;

		// Token: 0x0400006F RID: 111
		private const float k_cornerAngleTreshold = 10f;

		// Token: 0x04000070 RID: 112
		private CinemachineConfiner2D.ShapeCache m_shapeCache;

		// Token: 0x0200007B RID: 123
		private class VcamExtraState
		{
			// Token: 0x040002D6 RID: 726
			public Vector3 m_PreviousDisplacement;

			// Token: 0x040002D7 RID: 727
			public Vector3 m_DampedDisplacement;

			// Token: 0x040002D8 RID: 728
			public ConfinerOven.BakedSolution m_BakedSolution;

			// Token: 0x040002D9 RID: 729
			public CinemachineVirtualCameraBase m_vcam;
		}

		// Token: 0x0200007C RID: 124
		private struct ShapeCache
		{
			// Token: 0x0600041F RID: 1055 RVA: 0x00018980 File Offset: 0x00016B80
			public void Invalidate()
			{
				this.m_aspectRatio = 0f;
				this.m_maxWindowSize = -1f;
				this.m_DeltaBakedToWorld = (this.m_DeltaWorldToBaked = Matrix4x4.identity);
				this.m_boundingShape2D = null;
				this.m_OriginalPath = null;
				this.m_confinerOven = null;
			}

			// Token: 0x06000420 RID: 1056 RVA: 0x000189CC File Offset: 0x00016BCC
			public bool ValidateCache(Collider2D boundingShape2D, float maxWindowSize, float aspectRatio, float skeletonPadding, out bool confinerStateChanged)
			{
				confinerStateChanged = false;
				if (this.IsValid(in boundingShape2D, in aspectRatio, in maxWindowSize, in skeletonPadding))
				{
					if (this.m_confinerOven.State == ConfinerOven.BakingState.BAKING)
					{
						this.m_confinerOven.BakeConfiner(this.m_maxComputationTimePerFrameInSeconds);
						confinerStateChanged = this.m_confinerOven.State > ConfinerOven.BakingState.BAKING;
					}
					this.CalculateDeltaTransformationMatrix();
					if (this.m_DeltaWorldToBaked.lossyScale.IsUniform())
					{
						return true;
					}
				}
				this.Invalidate();
				confinerStateChanged = true;
				Type type = ((boundingShape2D == null) ? null : boundingShape2D.GetType());
				if (type == typeof(PolygonCollider2D))
				{
					PolygonCollider2D polygonCollider2D = boundingShape2D as PolygonCollider2D;
					this.m_OriginalPath = new List<List<Vector2>>();
					this.m_bakedToWorld = boundingShape2D.transform.localToWorldMatrix;
					for (int i = 0; i < polygonCollider2D.pathCount; i++)
					{
						Vector2[] path = polygonCollider2D.GetPath(i);
						List<Vector2> list = new List<Vector2>();
						for (int j = 0; j < path.Length; j++)
						{
							list.Add(this.m_bakedToWorld.MultiplyPoint3x4(path[j]));
						}
						this.m_OriginalPath.Add(list);
					}
				}
				else
				{
					if (!(type == typeof(CompositeCollider2D)))
					{
						return false;
					}
					CompositeCollider2D compositeCollider2D = boundingShape2D as CompositeCollider2D;
					this.m_OriginalPath = new List<List<Vector2>>();
					this.m_bakedToWorld = boundingShape2D.transform.localToWorldMatrix;
					Vector2[] array = new Vector2[compositeCollider2D.pointCount];
					for (int k = 0; k < compositeCollider2D.pathCount; k++)
					{
						int path2 = compositeCollider2D.GetPath(k, array);
						List<Vector2> list2 = new List<Vector2>();
						for (int l = 0; l < path2; l++)
						{
							list2.Add(this.m_bakedToWorld.MultiplyPoint3x4(array[l]));
						}
						this.m_OriginalPath.Add(list2);
					}
				}
				this.m_confinerOven = new ConfinerOven(in this.m_OriginalPath, in aspectRatio, maxWindowSize, skeletonPadding);
				this.m_aspectRatio = aspectRatio;
				this.m_boundingShape2D = boundingShape2D;
				this.m_maxWindowSize = maxWindowSize;
				this.m_skeletonPadding = skeletonPadding;
				this.CalculateDeltaTransformationMatrix();
				return true;
			}

			// Token: 0x06000421 RID: 1057 RVA: 0x00018BEC File Offset: 0x00016DEC
			private bool IsValid(in Collider2D boundingShape2D, in float aspectRatio, in float maxOrthoSize, in float padding)
			{
				return boundingShape2D != null && this.m_boundingShape2D != null && this.m_boundingShape2D == boundingShape2D && this.m_OriginalPath != null && this.m_confinerOven != null && Mathf.Abs(this.m_aspectRatio - aspectRatio) < 0.0001f && Mathf.Abs(this.m_maxWindowSize - maxOrthoSize) < 0.0001f && Mathf.Abs(this.m_skeletonPadding - padding) < 0.0001f;
			}

			// Token: 0x06000422 RID: 1058 RVA: 0x00018C74 File Offset: 0x00016E74
			private void CalculateDeltaTransformationMatrix()
			{
				Matrix4x4 matrix4x = Matrix4x4.Translate(-this.m_boundingShape2D.offset) * this.m_boundingShape2D.transform.worldToLocalMatrix;
				this.m_DeltaWorldToBaked = this.m_bakedToWorld * matrix4x;
				this.m_DeltaBakedToWorld = this.m_DeltaWorldToBaked.inverse;
			}

			// Token: 0x040002DA RID: 730
			public ConfinerOven m_confinerOven;

			// Token: 0x040002DB RID: 731
			public List<List<Vector2>> m_OriginalPath;

			// Token: 0x040002DC RID: 732
			public Matrix4x4 m_DeltaWorldToBaked;

			// Token: 0x040002DD RID: 733
			public Matrix4x4 m_DeltaBakedToWorld;

			// Token: 0x040002DE RID: 734
			private float m_aspectRatio;

			// Token: 0x040002DF RID: 735
			private float m_maxWindowSize;

			// Token: 0x040002E0 RID: 736
			private float m_skeletonPadding;

			// Token: 0x040002E1 RID: 737
			internal float m_maxComputationTimePerFrameInSeconds;

			// Token: 0x040002E2 RID: 738
			private Matrix4x4 m_bakedToWorld;

			// Token: 0x040002E3 RID: 739
			private Collider2D m_boundingShape2D;
		}
	}
}
