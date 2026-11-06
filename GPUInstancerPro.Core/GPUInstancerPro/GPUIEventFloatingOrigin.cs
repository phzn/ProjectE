using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x0200005A RID: 90
	[HelpURL("https://wiki.gurbu.com/index.php?title=GPU_Instancer_Pro:GettingStarted#Floating_Origin_Event")]
	public class GPUIEventFloatingOrigin : MonoBehaviour
	{
		// Token: 0x06000354 RID: 852 RVA: 0x0000F2E9 File Offset: 0x0000D4E9
		private void Awake()
		{
			if (!GPUIRuntimeSettings.IsSupportedPlatform())
			{
				base.enabled = false;
				return;
			}
		}

		// Token: 0x06000355 RID: 853 RVA: 0x000124CC File Offset: 0x000106CC
		private void OnEnable()
		{
			if (this.manager == null || this.floatingOrigin == null)
			{
				return;
			}
			this._previousMatrix = this.floatingOrigin.localToWorldMatrix;
			GPUIRenderingSystem.InitializeRenderingSystem();
			GPUIRenderingSystem instance = GPUIRenderingSystem.Instance;
			instance.OnPreCull = (Action<GPUICameraData>)Delegate.Remove(instance.OnPreCull, new Action<GPUICameraData>(this.HandleFloatingOrigin));
			GPUIRenderingSystem instance2 = GPUIRenderingSystem.Instance;
			instance2.OnPreCull = (Action<GPUICameraData>)Delegate.Combine(instance2.OnPreCull, new Action<GPUICameraData>(this.HandleFloatingOrigin));
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00012558 File Offset: 0x00010758
		private void OnDisable()
		{
			if (GPUIRenderingSystem.IsActive)
			{
				GPUIRenderingSystem instance = GPUIRenderingSystem.Instance;
				instance.OnPreCull = (Action<GPUICameraData>)Delegate.Remove(instance.OnPreCull, new Action<GPUICameraData>(this.HandleFloatingOrigin));
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00012588 File Offset: 0x00010788
		public void HandleFloatingOrigin(GPUICameraData cameraData)
		{
			Matrix4x4 localToWorldMatrix = this.floatingOrigin.localToWorldMatrix;
			if (!localToWorldMatrix.EqualsMatrix4x4(this._previousMatrix))
			{
				Matrix4x4 matrix4x = localToWorldMatrix * this._previousMatrix.inverse;
				GPUITransformBufferUtility.ApplyMatrixOffsetToTransforms(this.manager, matrix4x);
				this._previousMatrix = localToWorldMatrix;
			}
		}

		// Token: 0x0400026A RID: 618
		[SerializeField]
		public Transform floatingOrigin;

		// Token: 0x0400026B RID: 619
		[SerializeField]
		public GPUIManager manager;

		// Token: 0x0400026C RID: 620
		[NonSerialized]
		private Matrix4x4 _previousMatrix;
	}
}
