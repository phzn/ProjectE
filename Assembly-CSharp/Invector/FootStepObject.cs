using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x0200045E RID: 1118
	public class FootStepObject
	{
		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001BD6 RID: 7126 RVA: 0x000AF0CB File Offset: 0x000AD2CB
		public bool isTerrain
		{
			get
			{
				return this.terrain != null;
			}
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x000AF0DC File Offset: 0x000AD2DC
		public FootStepObject(Transform sender, Collider ground)
		{
			this.name = "";
			this.sender = sender;
			this.ground = ground;
			this.terrain = ground.GetComponent<Terrain>();
			this.stepHandle = ground.GetComponent<vFootStepHandler>();
			this.renderer = ground.GetComponent<Renderer>();
			this.spawnSoundEffect = true;
			this.spawnStepMarkEffect = true;
			this.spawnParticleEffect = true;
			this.volume = 1f;
			if (this.renderer != null && this.renderer.material != null)
			{
				int num = 0;
				this.name = string.Empty;
				if (this.stepHandle != null && this.stepHandle.material_ID > 0)
				{
					num = this.stepHandle.material_ID;
				}
				if (this.stepHandle)
				{
					vFootStepHandler.StepHandleType stepHandleType = this.stepHandle.stepHandleType;
					if (stepHandleType == vFootStepHandler.StepHandleType.materialName)
					{
						this.name = this.renderer.materials[num].name;
						return;
					}
					if (stepHandleType != vFootStepHandler.StepHandleType.textureName)
					{
						return;
					}
					this.name = this.renderer.materials[num].mainTexture.name;
					return;
				}
				else
				{
					this.name = this.renderer.materials[num].name;
				}
			}
		}

		// Token: 0x04001D16 RID: 7446
		public string name;

		// Token: 0x04001D17 RID: 7447
		public Transform sender;

		// Token: 0x04001D18 RID: 7448
		public Collider ground;

		// Token: 0x04001D19 RID: 7449
		public Terrain terrain;

		// Token: 0x04001D1A RID: 7450
		public vFootStepHandler stepHandle;

		// Token: 0x04001D1B RID: 7451
		public Renderer renderer;

		// Token: 0x04001D1C RID: 7452
		public bool spawnSoundEffect;

		// Token: 0x04001D1D RID: 7453
		public bool spawnStepMarkEffect;

		// Token: 0x04001D1E RID: 7454
		public bool spawnParticleEffect;

		// Token: 0x04001D1F RID: 7455
		public float volume;
	}
}
