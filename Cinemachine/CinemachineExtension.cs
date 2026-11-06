using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000036 RID: 54
	[DocumentationSorting(DocumentationSortingAttribute.Level.API)]
	public abstract class CinemachineExtension : MonoBehaviour
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000293 RID: 659 RVA: 0x000123A2 File Offset: 0x000105A2
		public CinemachineVirtualCameraBase VirtualCamera
		{
			get
			{
				if (this.m_vcamOwner == null)
				{
					this.m_vcamOwner = base.GetComponent<CinemachineVirtualCameraBase>();
				}
				return this.m_vcamOwner;
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x000123C4 File Offset: 0x000105C4
		protected virtual void Awake()
		{
			this.ConnectToVcam(true);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x000123CD File Offset: 0x000105CD
		protected virtual void OnEnable()
		{
		}

		// Token: 0x06000296 RID: 662 RVA: 0x000123CF File Offset: 0x000105CF
		protected virtual void OnDestroy()
		{
			this.ConnectToVcam(false);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x000123D8 File Offset: 0x000105D8
		internal void EnsureStarted()
		{
			this.ConnectToVcam(true);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x000123E4 File Offset: 0x000105E4
		protected virtual void ConnectToVcam(bool connect)
		{
			if (connect && this.VirtualCamera == null)
			{
				Debug.LogError("CinemachineExtension requires a Cinemachine Virtual Camera component");
			}
			if (this.VirtualCamera != null)
			{
				if (connect)
				{
					this.VirtualCamera.AddExtension(this);
				}
				else
				{
					this.VirtualCamera.RemoveExtension(this);
				}
			}
			this.mExtraState = null;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0001243E File Offset: 0x0001063E
		public virtual void PrePipelineMutateCameraStateCallback(CinemachineVirtualCameraBase vcam, ref CameraState curState, float deltaTime)
		{
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00012440 File Offset: 0x00010640
		public void InvokePostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{
			this.PostPipelineStageCallback(vcam, stage, ref state, deltaTime);
		}

		// Token: 0x0600029B RID: 667
		protected abstract void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime);

		// Token: 0x0600029C RID: 668 RVA: 0x0001244D File Offset: 0x0001064D
		public virtual void OnTargetObjectWarped(Transform target, Vector3 positionDelta)
		{
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0001244F File Offset: 0x0001064F
		public virtual void ForceCameraPosition(Vector3 pos, Quaternion rot)
		{
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00012451 File Offset: 0x00010651
		public virtual bool OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
		{
			return false;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00012454 File Offset: 0x00010654
		public virtual float GetMaxDampTime()
		{
			return 0f;
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x0001245B File Offset: 0x0001065B
		public virtual bool RequiresUserInput
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00012460 File Offset: 0x00010660
		protected T GetExtraState<T>(ICinemachineCamera vcam) where T : class, new()
		{
			if (this.mExtraState == null)
			{
				this.mExtraState = new Dictionary<ICinemachineCamera, object>();
			}
			object obj = null;
			if (!this.mExtraState.TryGetValue(vcam, out obj))
			{
				obj = (this.mExtraState[vcam] = new T());
			}
			return obj as T;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x000124B8 File Offset: 0x000106B8
		protected List<T> GetAllExtraStates<T>() where T : class, new()
		{
			List<T> list = new List<T>();
			if (this.mExtraState != null)
			{
				foreach (KeyValuePair<ICinemachineCamera, object> keyValuePair in this.mExtraState)
				{
					list.Add(keyValuePair.Value as T);
				}
			}
			return list;
		}

		// Token: 0x040001DE RID: 478
		protected const float Epsilon = 0.0001f;

		// Token: 0x040001DF RID: 479
		private CinemachineVirtualCameraBase m_vcamOwner;

		// Token: 0x040001E0 RID: 480
		private Dictionary<ICinemachineCamera, object> mExtraState;
	}
}
