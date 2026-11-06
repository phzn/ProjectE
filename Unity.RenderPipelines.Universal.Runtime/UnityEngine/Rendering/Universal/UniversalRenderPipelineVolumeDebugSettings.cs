using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200005F RID: 95
	public class UniversalRenderPipelineVolumeDebugSettings : VolumeDebugSettings<UniversalAdditionalCameraData>
	{
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600039A RID: 922 RVA: 0x000158BB File Offset: 0x00013ABB
		public override Type targetRenderPipeline
		{
			get
			{
				return typeof(UniversalRenderPipeline);
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600039B RID: 923 RVA: 0x000158C8 File Offset: 0x00013AC8
		public override VolumeStack selectedCameraVolumeStack
		{
			get
			{
				if (base.selectedCamera == null)
				{
					return null;
				}
				UniversalAdditionalCameraData component = base.selectedCamera.GetComponent<UniversalAdditionalCameraData>();
				if (component == null)
				{
					return null;
				}
				VolumeStack volumeStack = component.volumeStack;
				if (volumeStack != null)
				{
					return volumeStack;
				}
				return VolumeManager.instance.stack;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600039C RID: 924 RVA: 0x00015912 File Offset: 0x00013B12
		public override LayerMask selectedCameraLayerMask
		{
			get
			{
				if (!(base.selectedCamera != null))
				{
					return 0;
				}
				return base.selectedCamera.GetComponent<UniversalAdditionalCameraData>().volumeLayerMask;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600039D RID: 925 RVA: 0x00015939 File Offset: 0x00013B39
		public override Vector3 selectedCameraPosition
		{
			get
			{
				if (!(base.selectedCamera != null))
				{
					return Vector3.zero;
				}
				return base.selectedCamera.transform.position;
			}
		}
	}
}
