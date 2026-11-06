using System;
using UnityEngine;

namespace RealToon.Script
{
	// Token: 0x0200043A RID: 1082
	[ExecuteAlways]
	[AddComponentMenu("RealToon/Tools/ShadowT SDF Mode - Helper")]
	public class ShadowTSDFModeHelper : MonoBehaviour
	{
		// Token: 0x06001B3A RID: 6970 RVA: 0x000AB16C File Offset: 0x000A936C
		private void LateUpdate()
		{
			if (!(this.Material == null) && !(this.ObjectToFollow == null) && (this.Material.shader.name == this.RT_Sha_Nam_URP || this.Material.shader.name == this.RT_Sha_Nam_HDRP || this.Material.shader.name == this.RT_Sha_Nam_BiRP_DD || this.Material.shader.name == this.RT_Sha_Nam_BiRP_DFT || this.Material.shader.name == this.RT_Sha_Nam_BiRP_DR || this.Material.shader.name == this.RT_Sha_Nam_BiRP_TDD || this.Material.shader.name == this.RT_Sha_Nam_BiRP_TDFT || this.Material.shader.name == this.RT_Sha_Nam_BiRP_TDR))
			{
				this.Material.SetFloat("_ShadowTLightThreshold", this.LightThreshold);
				if (!this.ForwardInverted)
				{
					this.Material.SetVector("_ObjectForward", this.ObjectToFollow.transform.forward);
				}
				else
				{
					this.Material.SetVector("_ObjectForward", -this.ObjectToFollow.transform.forward);
				}
				if (!this.RightInverted)
				{
					this.Material.SetVector("_ObjectRight", this.ObjectToFollow.transform.right);
					return;
				}
				this.Material.SetVector("_ObjectRight", -this.ObjectToFollow.transform.right);
			}
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x000AB358 File Offset: 0x000A9558
		private void OnValidate()
		{
			if (this.Material == null)
			{
				this.checkstart = true;
				return;
			}
			if (this.Material != null && (this.Material.shader.name == this.RT_Sha_Nam_URP || this.Material.shader.name == this.RT_Sha_Nam_HDRP || this.Material.shader.name == this.RT_Sha_Nam_BiRP_DD || this.Material.shader.name == this.RT_Sha_Nam_BiRP_DFT || this.Material.shader.name == this.RT_Sha_Nam_BiRP_DR || this.Material.shader.name == this.RT_Sha_Nam_BiRP_TDD || this.Material.shader.name == this.RT_Sha_Nam_BiRP_TDFT || this.Material.shader.name == this.RT_Sha_Nam_BiRP_TDR) && this.checkstart && ((!this.Material.IsKeywordEnabled("N_F_ST_ON") && !this.Material.IsKeywordEnabled("N_F_STSDFM_ON")) || (this.Material.IsKeywordEnabled("N_F_ST_ON") && !this.Material.IsKeywordEnabled("N_F_STSDFM_ON")) || (!this.Material.IsKeywordEnabled("N_F_ST_ON") && this.Material.IsKeywordEnabled("N_F_STSDFM_ON"))))
			{
				this.Material.EnableKeyword("N_F_ST_ON");
				this.Material.SetFloat("_N_F_ST", 1f);
				this.Material.EnableKeyword("N_F_STSDFM_ON");
				this.Material.SetFloat("_N_F_STSDFM", 1f);
				this.checkstart = false;
			}
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x000AB548 File Offset: 0x000A9748
		private void Reset()
		{
			this.Material = null;
			this.ObjectToFollow = null;
			this.checkstart = true;
		}

		// Token: 0x04001C44 RID: 7236
		[Header("Note: ShadowT feature and SDF Mode will be automatically enable\nWhen you put a material that uses RealToon Shader on the Material slot.")]
		[Space(25f)]
		[Space(10f)]
		[SerializeField]
		[Tooltip("A material that uses 'RealToon - ShadowT' feature and enabled 'SDF Mode'.")]
		public Material Material;

		// Token: 0x04001C45 RID: 7237
		[SerializeField]
		[Tooltip("An object to follow the position.")]
		public Transform ObjectToFollow;

		// Token: 0x04001C46 RID: 7238
		[Space(10f)]
		[SerializeField]
		[Tooltip("The amount of light.")]
		private float LightThreshold = 200f;

		// Token: 0x04001C47 RID: 7239
		[Space(10f)]
		[SerializeField]
		[Tooltip("Invert the Foward/Front position of the object.")]
		private bool ForwardInverted;

		// Token: 0x04001C48 RID: 7240
		[Tooltip("Invert the Right position of the object.")]
		[SerializeField]
		private bool RightInverted;

		// Token: 0x04001C49 RID: 7241
		[HideInInspector]
		[SerializeField]
		private bool checkstart = true;

		// Token: 0x04001C4A RID: 7242
		private string RT_Sha_Nam_URP = "Universal Render Pipeline/RealToon/Version 5/Default/Default";

		// Token: 0x04001C4B RID: 7243
		private string RT_Sha_Nam_HDRP = "HDRP/RealToon/Version 5/Default";

		// Token: 0x04001C4C RID: 7244
		private string RT_Sha_Nam_BiRP_DD = "RealToon/Version 5/Default/Default";

		// Token: 0x04001C4D RID: 7245
		private string RT_Sha_Nam_BiRP_DFT = "RealToon/Version 5/Default/Fade Transparency";

		// Token: 0x04001C4E RID: 7246
		private string RT_Sha_Nam_BiRP_DR = "RealToon/Version 5/Default/Refraction";

		// Token: 0x04001C4F RID: 7247
		private string RT_Sha_Nam_BiRP_TDD = "RealToon/Version 5/Tessellation/Default";

		// Token: 0x04001C50 RID: 7248
		private string RT_Sha_Nam_BiRP_TDFT = "RealToon/Version 5/Tessellation/Fade Transparency";

		// Token: 0x04001C51 RID: 7249
		private string RT_Sha_Nam_BiRP_TDR = "RealToon/Version 5/Tessellation/Refraction";
	}
}
