using System;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
	// Token: 0x02000013 RID: 19
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[RequireComponent(typeof(Camera))]
	[DisallowMultipleComponent]
	[AddComponentMenu("Cinemachine/CinemachineExternalCamera")]
	[ExecuteAlways]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineExternalCamera.html")]
	public class CinemachineExternalCamera : CinemachineVirtualCameraBase
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00006827 File Offset: 0x00004A27
		public override CameraState State
		{
			get
			{
				return this.m_State;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000AE RID: 174 RVA: 0x0000682F File Offset: 0x00004A2F
		// (set) Token: 0x060000AF RID: 175 RVA: 0x00006837 File Offset: 0x00004A37
		public override Transform LookAt
		{
			get
			{
				return this.m_LookAt;
			}
			set
			{
				this.m_LookAt = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00006840 File Offset: 0x00004A40
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x00006848 File Offset: 0x00004A48
		public override Transform Follow { get; set; }

		// Token: 0x060000B2 RID: 178 RVA: 0x00006854 File Offset: 0x00004A54
		public override void InternalUpdateCameraState(Vector3 worldUp, float deltaTime)
		{
			if (this.m_Camera == null)
			{
				base.TryGetComponent<Camera>(out this.m_Camera);
			}
			this.m_State = CameraState.Default;
			this.m_State.RawPosition = base.transform.position;
			this.m_State.RawOrientation = base.transform.rotation;
			this.m_State.ReferenceUp = this.m_State.RawOrientation * Vector3.up;
			if (this.m_Camera != null)
			{
				this.m_State.Lens = LensSettings.FromCamera(this.m_Camera);
			}
			if (this.m_LookAt != null)
			{
				this.m_State.ReferenceLookAt = this.m_LookAt.transform.position;
				Vector3 vector = this.m_State.ReferenceLookAt - this.State.RawPosition;
				if (!vector.AlmostZero())
				{
					this.m_State.ReferenceLookAt = this.m_State.RawPosition + Vector3.Project(vector, this.State.RawOrientation * Vector3.forward);
				}
			}
			base.ApplyPositionBlendMethod(ref this.m_State, this.m_BlendHint);
			base.InvokePostPipelineStageCallback(this, CinemachineCore.Stage.Finalize, ref this.m_State, deltaTime);
		}

		// Token: 0x04000076 RID: 118
		[Tooltip("The object that the camera is looking at.  Setting this will improve the quality of the blends to and from this camera")]
		[NoSaveDuringPlay]
		[VcamTargetProperty]
		public Transform m_LookAt;

		// Token: 0x04000077 RID: 119
		private Camera m_Camera;

		// Token: 0x04000078 RID: 120
		private CameraState m_State = CameraState.Default;

		// Token: 0x0400007A RID: 122
		[Tooltip("Hint for blending positions to and from this virtual camera")]
		[FormerlySerializedAs("m_PositionBlending")]
		public CinemachineVirtualCameraBase.BlendHint m_BlendHint;
	}
}
