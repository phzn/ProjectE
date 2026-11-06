using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000458 RID: 1112
	[Serializable]
	public abstract class vFootStepBase : MonoBehaviour
	{
		// Token: 0x06001BB3 RID: 7091 RVA: 0x000AE794 File Offset: 0x000AC994
		public virtual void SpawnSurfaceEffect(FootStepObject footStepObject)
		{
			if (footStepObject != null)
			{
				for (int i = 0; i < this.customSurfaces.Count; i++)
				{
					if (this.customSurfaces[i] != null && this.ContainsTexture(footStepObject.name, this.customSurfaces[i]))
					{
						this.customSurfaces[i].SpawnSurfaceEffect(footStepObject);
						return;
					}
				}
			}
			if (this.defaultSurface != null)
			{
				this.defaultSurface.SpawnSurfaceEffect(footStepObject);
			}
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x000AE818 File Offset: 0x000ACA18
		protected virtual bool ContainsTexture(string name, vAudioSurface surface)
		{
			for (int i = 0; i < surface.TextureOrMaterialNames.Count; i++)
			{
				if (name.Contains(surface.TextureOrMaterialNames[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001BB5 RID: 7093
		public abstract void StepOnTerrain(FootStepObject footStepObject);

		// Token: 0x06001BB6 RID: 7094
		public abstract void StepOnMesh(FootStepObject footStepObject);

		// Token: 0x06001BB7 RID: 7095
		public abstract void PlayFootStepEffect();

		// Token: 0x06001BB8 RID: 7096 RVA: 0x00004F31 File Offset: 0x00003131
		public virtual void PlayFootStep(AnimationEvent evt)
		{
		}

		// Token: 0x06001BB9 RID: 7097 RVA: 0x00004F31 File Offset: 0x00003131
		public virtual void PlayFootStepLeft(AnimationEvent evt)
		{
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x00004F31 File Offset: 0x00003131
		public virtual void PlayFootStepRight(AnimationEvent evt)
		{
		}

		// Token: 0x04001CF7 RID: 7415
		public vAudioSurface defaultSurface;

		// Token: 0x04001CF8 RID: 7416
		public List<vAudioSurface> customSurfaces;
	}
}
