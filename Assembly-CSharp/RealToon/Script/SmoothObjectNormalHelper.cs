using System;
using UnityEngine;

namespace RealToon.Script
{
	// Token: 0x0200043C RID: 1084
	[ExecuteAlways]
	[AddComponentMenu("RealToon/Tools/Smooth Object Normal - Helper")]
	public class SmoothObjectNormalHelper : MonoBehaviour
	{
		// Token: 0x06001B46 RID: 6982 RVA: 0x000ABD4C File Offset: 0x000A9F4C
		private void LateUpdate()
		{
			if (!(this.Material == null) && !(this.ObjectToFollow == null) && (this.Material.shader.name == this.RT_Sha_Nam_URP || this.Material.shader.name == this.RT_Sha_Nam_HDRP || this.Material.shader.name == this.RT_Sha_Nam_BiRP_DD || this.Material.shader.name == this.RT_Sha_Nam_BiRP_DFT || this.Material.shader.name == this.RT_Sha_Nam_BiRP_DR || this.Material.shader.name == this.RT_Sha_Nam_BiRP_TDD || this.Material.shader.name == this.RT_Sha_Nam_BiRP_TDFT || this.Material.shader.name == this.RT_Sha_Nam_BiRP_TDR || this.Material.shader.name == this.RT_Sha_Nam_BiRP_LD || this.Material.shader.name == this.RT_Sha_Nam_BiRP_LFT))
			{
				Vector3 position = this.ObjectToFollow.gameObject.transform.position;
				this.Material.SetVector("_XYZPosition", position);
				this.Material.SetFloat("_SmoothObjectNormal", this.Intensity);
			}
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x000ABEF0 File Offset: 0x000AA0F0
		private void OnValidate()
		{
			if (this.Material == null)
			{
				this.checkstart = true;
				return;
			}
			if (this.Material != null && (this.Material.shader.name == this.RT_Sha_Nam_URP || this.Material.shader.name == this.RT_Sha_Nam_HDRP || this.Material.shader.name == this.RT_Sha_Nam_BiRP_DD || this.Material.shader.name == this.RT_Sha_Nam_BiRP_DFT || this.Material.shader.name == this.RT_Sha_Nam_BiRP_DR || this.Material.shader.name == this.RT_Sha_Nam_BiRP_TDD || this.Material.shader.name == this.RT_Sha_Nam_BiRP_TDFT || this.Material.shader.name == this.RT_Sha_Nam_BiRP_TDR || this.Material.shader.name == this.RT_Sha_Nam_BiRP_LD || this.Material.shader.name == this.RT_Sha_Nam_BiRP_LFT))
			{
				if (this.checkstart && !this.Material.IsKeywordEnabled("N_F_SON_ON"))
				{
					this.Material.EnableKeyword("N_F_SON_ON");
					this.Material.SetFloat("_N_F_SON", 1f);
					this.checkstart = false;
				}
				if (this.ShowNormal)
				{
					this.Material.SetFloat("_ShowNormal", 1f);
					return;
				}
				this.Material.SetFloat("_ShowNormal", 0f);
			}
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x000AC0CE File Offset: 0x000AA2CE
		private void Reset()
		{
			this.Material = null;
			this.ObjectToFollow = null;
			this.checkstart = true;
		}

		// Token: 0x04001C61 RID: 7265
		[Header("Note: Smooth Object Normal feature will be automatically enable\nWhen you put a material that uses RealToon Shader on the Material slot.")]
		[Space(25f)]
		[SerializeField]
		[Range(0f, 1f)]
		[Tooltip("The amount of Smoothed Object Normal.")]
		public float Intensity;

		// Token: 0x04001C62 RID: 7266
		[Space(10f)]
		[SerializeField]
		[Tooltip("A material that uses 'RealToon - Smooth Object Normal' feature.")]
		public Material Material;

		// Token: 0x04001C63 RID: 7267
		[SerializeField]
		[Tooltip("An object to follow the position.")]
		public Transform ObjectToFollow;

		// Token: 0x04001C64 RID: 7268
		[Space(10f)]
		[SerializeField]
		[Tooltip("Show object normal.")]
		public bool ShowNormal;

		// Token: 0x04001C65 RID: 7269
		[HideInInspector]
		[SerializeField]
		private bool checkstart = true;

		// Token: 0x04001C66 RID: 7270
		private string RT_Sha_Nam_URP = "Universal Render Pipeline/RealToon/Version 5/Default/Default";

		// Token: 0x04001C67 RID: 7271
		private string RT_Sha_Nam_HDRP = "HDRP/RealToon/Version 5/Default";

		// Token: 0x04001C68 RID: 7272
		private string RT_Sha_Nam_BiRP_DD = "RealToon/Version 5/Default/Default";

		// Token: 0x04001C69 RID: 7273
		private string RT_Sha_Nam_BiRP_DFT = "RealToon/Version 5/Default/Fade Transparency";

		// Token: 0x04001C6A RID: 7274
		private string RT_Sha_Nam_BiRP_DR = "RealToon/Version 5/Default/Refraction";

		// Token: 0x04001C6B RID: 7275
		private string RT_Sha_Nam_BiRP_TDD = "RealToon/Version 5/Tessellation/Default";

		// Token: 0x04001C6C RID: 7276
		private string RT_Sha_Nam_BiRP_TDFT = "RealToon/Version 5/Tessellation/Fade Transparency";

		// Token: 0x04001C6D RID: 7277
		private string RT_Sha_Nam_BiRP_TDR = "RealToon/Version 5/Tessellation/Refraction";

		// Token: 0x04001C6E RID: 7278
		private string RT_Sha_Nam_BiRP_LD = "RealToon/Version 5/Lite/Default";

		// Token: 0x04001C6F RID: 7279
		private string RT_Sha_Nam_BiRP_LFT = "RealToon/Version 5/Lite/Fade Transparency";
	}
}
