using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000447 RID: 1095
	public class vCullingFadeControl : MonoBehaviour
	{
		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001B78 RID: 7032 RVA: 0x000AD00A File Offset: 0x000AB20A
		public Transform targetObject
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001B79 RID: 7033 RVA: 0x000AD014 File Offset: 0x000AB214
		public Transform cameraTransform
		{
			get
			{
				Transform transform = base.transform;
				if (Camera.main != null)
				{
					transform = Camera.main.transform;
				}
				if (transform == base.transform)
				{
					Debug.LogWarning("Invector : Missing MainCamera");
					base.enabled = false;
				}
				return transform;
			}
		}

		// Token: 0x06001B7A RID: 7034 RVA: 0x000AD060 File Offset: 0x000AB260
		private void Start()
		{
			this.Init();
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x000AD068 File Offset: 0x000AB268
		public void Init()
		{
			foreach (FadeMaterials fadeMaterials in this.fadeMeshRenderers)
			{
				fadeMaterials.originalAlpha = new float[fadeMaterials.originalMaterials.Length];
				int i = 0;
				while (i < fadeMaterials.originalMaterials.Length)
				{
					if (fadeMaterials.fadeMaterials[i] == null)
					{
						try
						{
							fadeMaterials.originalAlpha[i] = fadeMaterials.originalMaterials[i].color.a;
							fadeMaterials.fadeMaterials[i] = fadeMaterials.originalMaterials[i];
							goto IL_008F;
						}
						catch
						{
							goto IL_008F;
						}
						goto IL_006F;
					}
					goto IL_006F;
					IL_008F:
					i++;
					continue;
					IL_006F:
					try
					{
						fadeMaterials.originalAlpha[i] = fadeMaterials.fadeMaterials[i].color.a;
					}
					catch
					{
					}
					goto IL_008F;
				}
			}
			foreach (FadeMaterials fadeMaterials2 in this.fadeSkinnedMeshRenderes)
			{
				fadeMaterials2.originalAlpha = new float[fadeMaterials2.originalMaterials.Length];
				int j = 0;
				while (j < fadeMaterials2.originalMaterials.Length)
				{
					if (fadeMaterials2.fadeMaterials[j] == null)
					{
						try
						{
							fadeMaterials2.originalAlpha[j] = fadeMaterials2.originalMaterials[j].color.a;
							fadeMaterials2.fadeMaterials[j] = fadeMaterials2.originalMaterials[j];
							goto IL_0151;
						}
						catch
						{
							goto IL_0151;
						}
						goto IL_012F;
					}
					goto IL_012F;
					IL_0151:
					j++;
					continue;
					IL_012F:
					try
					{
						fadeMaterials2.originalAlpha[j] = fadeMaterials2.fadeMaterials[j].color.a;
					}
					catch
					{
					}
					goto IL_0151;
				}
			}
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x000AD240 File Offset: 0x000AB440
		private void LateUpdate()
		{
			this.UpdateEffect();
			if (this.usingTransp)
			{
				this.ChangeAlphaFromDistance();
			}
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x000AD258 File Offset: 0x000AB458
		private void UpdateEffect()
		{
			float num = Vector3.Distance(this.cameraTransform.position, this.targetObject.position + this.offset);
			if (num < this.distanceToStartFade && !this.usingTransp)
			{
				this.usingTransp = true;
				this.ChangeMaterialsToFade();
				return;
			}
			if (this.usingTransp && num > this.distanceToStartFade)
			{
				this.usingTransp = false;
				this.ChangeMaterialsToOriginal();
			}
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x000AD2CC File Offset: 0x000AB4CC
		private void ChangeMaterialsToOriginal()
		{
			foreach (FadeMaterials fadeMaterials in this.fadeMeshRenderers)
			{
				try
				{
					fadeMaterials.renderer.sharedMaterials = fadeMaterials.originalMaterials;
				}
				catch
				{
				}
			}
			foreach (FadeMaterials fadeMaterials2 in this.fadeSkinnedMeshRenderes)
			{
				try
				{
					fadeMaterials2.renderer.sharedMaterials = fadeMaterials2.originalMaterials;
				}
				catch
				{
				}
			}
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x000AD398 File Offset: 0x000AB598
		private void ChangeMaterialsToFade()
		{
			foreach (FadeMaterials fadeMaterials in this.fadeMeshRenderers)
			{
				try
				{
					fadeMaterials.renderer.sharedMaterials = fadeMaterials.fadeMaterials;
				}
				catch
				{
				}
			}
			foreach (FadeMaterials fadeMaterials2 in this.fadeSkinnedMeshRenderes)
			{
				try
				{
					fadeMaterials2.renderer.sharedMaterials = fadeMaterials2.fadeMaterials;
				}
				catch
				{
				}
			}
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x000AD464 File Offset: 0x000AB664
		public void ChangeAlphaFromDistance()
		{
			float num = Vector3.Distance(this.cameraTransform.position, this.targetObject.position + this.offset);
			for (int i = 0; i < this.fadeMeshRenderers.Count; i++)
			{
				for (int j = 0; j < this.fadeMeshRenderers[i].fadeMaterials.Length; j++)
				{
					try
					{
						float num2 = this.fadeMeshRenderers[i].originalAlpha[j] / (this.distanceToStartFade - this.distanceToEndFade);
						Color color = this.fadeMeshRenderers[i].renderer.sharedMaterials[j].color;
						float num3 = this.distanceToStartFade - this.distanceToEndFade - (this.distanceToStartFade - num);
						color.a = num2 * num3;
						color.a = Mathf.Clamp(color.a, 0f, this.fadeMeshRenderers[i].originalAlpha[j]);
						this.fadeMeshRenderers[i].renderer.materials[j].color = color;
					}
					catch
					{
					}
				}
			}
			for (int k = 0; k < this.fadeSkinnedMeshRenderes.Count; k++)
			{
				for (int l = 0; l < this.fadeSkinnedMeshRenderes[k].fadeMaterials.Length; l++)
				{
					try
					{
						float num4 = this.fadeSkinnedMeshRenderes[k].originalAlpha[l] / (this.distanceToStartFade - this.distanceToEndFade);
						Color color2 = this.fadeSkinnedMeshRenderes[k].renderer.sharedMaterials[l].color;
						float num5 = this.distanceToStartFade - this.distanceToEndFade - (this.distanceToStartFade - num);
						color2.a = num4 * num5;
						color2.a = Mathf.Clamp(color2.a, 0f, this.fadeSkinnedMeshRenderes[k].originalAlpha[l]);
						this.fadeSkinnedMeshRenderes[k].renderer.materials[l].color = color2;
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x04001CA8 RID: 7336
		public float distanceToStartFade = 0.55f;

		// Token: 0x04001CA9 RID: 7337
		public float distanceToEndFade = 0.4f;

		// Token: 0x04001CAA RID: 7338
		public Vector3 offset = new Vector3(0f, 1.3f, 0f);

		// Token: 0x04001CAB RID: 7339
		public List<FadeMaterials> fadeMeshRenderers;

		// Token: 0x04001CAC RID: 7340
		public List<FadeMaterials> fadeSkinnedMeshRenderes;

		// Token: 0x04001CAD RID: 7341
		public bool usingTransp;
	}
}
