using System;
using UnityEngine;
using UnityEngine.Events;

namespace GPUInstancerPro
{
	// Token: 0x02000051 RID: 81
	[RequireComponent(typeof(Light))]
	[DefaultExecutionOrder(-1000)]
	[ExecuteInEditMode]
	public class GPUISRPLightSetting : MonoBehaviour
	{
		// Token: 0x060002A8 RID: 680 RVA: 0x000024BC File Offset: 0x000006BC
		private void Awake()
		{
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000F241 File Offset: 0x0000D441
		private void OnEnable()
		{
			if (GPUIRuntimeSettings.Instance.RenderPipeline == GPUIRenderPipeline.URP)
			{
				base.GetComponent<Light>().intensity = this.uRPIntensity;
				this.HandleAPV();
				return;
			}
			this.HandleAPV();
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000F272 File Offset: 0x0000D472
		private void HandleAPV()
		{
			if (this.lightProbeGroup != null)
			{
				if (GPUIRuntimeSettings.IsAdaptiveProbeVolumesEnabled())
				{
					UnityEvent unityEvent = this.onAPVEnabled;
					if (unityEvent == null)
					{
						return;
					}
					unityEvent.Invoke();
					return;
				}
				else
				{
					UnityEvent unityEvent2 = this.onAPVDisabled;
					if (unityEvent2 == null)
					{
						return;
					}
					unityEvent2.Invoke();
				}
			}
		}

		// Token: 0x04000217 RID: 535
		[Header("URP")]
		public float uRPIntensity = 1f;

		// Token: 0x04000218 RID: 536
		[Header("HDRP")]
		public float hDRPIntensity = 100000f;

		// Token: 0x04000219 RID: 537
		[Range(0f, 3f)]
		public int hDRPShadowResolutionLevel = 2;

		// Token: 0x0400021A RID: 538
		public LightProbeGroup lightProbeGroup;

		// Token: 0x0400021B RID: 539
		public Vector3 probeVolumeSize = new Vector3(50f, 50f, 50f);

		// Token: 0x0400021C RID: 540
		public UnityEvent onAPVEnabled;

		// Token: 0x0400021D RID: 541
		public UnityEvent onAPVDisabled;
	}
}
