using System;
using UnityEngine;

namespace RealToon.Script
{
	// Token: 0x02000439 RID: 1081
	[ExecuteAlways]
	[AddComponentMenu("RealToon/Tools/Perspective Adjustment - Controller")]
	public class PerspectiveAdjustmentController : MonoBehaviour
	{
		// Token: 0x06001B32 RID: 6962 RVA: 0x000AA06E File Offset: 0x000A826E
		private void Start()
		{
			if (this.checkstart)
			{
				this.InitStart();
				this.checkstart = false;
			}
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x000AA088 File Offset: 0x000A8288
		private void LateUpdate()
		{
			if (this.Mat != null && this.CanAnimateIt)
			{
				foreach (Material material in this.Mat)
				{
					if (material != null && (material.shader.name == this.RT_Sha_Nam_URP || material.shader.name == this.RT_Sha_Nam_HDRP || material.shader.name == this.RT_Sha_Nam_BiRP_DD || material.shader.name == this.RT_Sha_Nam_BiRP_DFT || material.shader.name == this.RT_Sha_Nam_BiRP_DR || material.shader.name == this.RT_Sha_Nam_BiRP_TDD || material.shader.name == this.RT_Sha_Nam_BiRP_TDFT || material.shader.name == this.RT_Sha_Nam_BiRP_TDR || material.shader.name == this.RT_Sha_Nam_BiRP_DDL || material.shader.name == this.RT_Sha_Nam_BiRP_DFTL))
					{
						this.Set_Shad_Prop(material);
					}
				}
			}
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x000AA1DC File Offset: 0x000A83DC
		private void OnValidate()
		{
			if (this.Mat != null)
			{
				foreach (Material material in this.Mat)
				{
					if (material != null && (material.shader.name == this.RT_Sha_Nam_URP || material.shader.name == this.RT_Sha_Nam_HDRP || material.shader.name == this.RT_Sha_Nam_BiRP_DD || material.shader.name == this.RT_Sha_Nam_BiRP_DFT || material.shader.name == this.RT_Sha_Nam_BiRP_DR || material.shader.name == this.RT_Sha_Nam_BiRP_TDD || material.shader.name == this.RT_Sha_Nam_BiRP_TDFT || material.shader.name == this.RT_Sha_Nam_BiRP_TDR || material.shader.name == this.RT_Sha_Nam_BiRP_DDL || material.shader.name == this.RT_Sha_Nam_BiRP_DFTL))
					{
						this.Set_Shad_Prop(material);
					}
				}
			}
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x000AA324 File Offset: 0x000A8524
		private void Reset()
		{
			if (this.Mat != null)
			{
				this.checkstart = true;
				this.Res_Shad_Prop();
				foreach (Material material in this.Mat)
				{
					if (material != null && (material.shader.name == this.RT_Sha_Nam_URP || material.shader.name == this.RT_Sha_Nam_HDRP || material.shader.name == this.RT_Sha_Nam_BiRP_DD || material.shader.name == this.RT_Sha_Nam_BiRP_DFT || material.shader.name == this.RT_Sha_Nam_BiRP_DR || material.shader.name == this.RT_Sha_Nam_BiRP_TDD || material.shader.name == this.RT_Sha_Nam_BiRP_TDFT || material.shader.name == this.RT_Sha_Nam_BiRP_TDR || material.shader.name == this.RT_Sha_Nam_BiRP_DDL || material.shader.name == this.RT_Sha_Nam_BiRP_DFTL))
					{
						this.Set_Shad_Prop(material);
					}
				}
				this.coun_obj_wi_ralsha = 0;
				this.coun_obj_mat = 0;
				this.coun_obj_mat_arr = 0;
				this.InitStart();
			}
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x000AA494 File Offset: 0x000A8694
		private void InitStart()
		{
			if (this.attac == null || this.attac.Length == 0)
			{
				this.attac = base.gameObject.GetComponentsInChildren<Transform>();
			}
			int num = 0;
			foreach (Transform transform in this.attac)
			{
				Debug.Log("yes");
				if (transform.GetComponent<SkinnedMeshRenderer>() && transform.GetComponent<SkinnedMeshRenderer>().sharedMaterial != null && (transform.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_URP || transform.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_HDRP || transform.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DD || transform.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DFT || transform.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DR || transform.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_TDD || transform.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_TDFT || transform.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_TDR || transform.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DDL || transform.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DFTL))
				{
					this.coun_obj_wi_ralsha++;
					this.coun_obj_mat += transform.GetComponent<SkinnedMeshRenderer>().sharedMaterials.Length;
				}
				if (transform.GetComponent<MeshRenderer>() && transform.GetComponent<MeshRenderer>().sharedMaterial != null && (transform.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_URP || transform.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_HDRP || transform.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DD || transform.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DFT || transform.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DR || transform.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_TDD || transform.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_TDFT || transform.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_TDR || transform.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DDL || transform.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DFTL))
				{
					this.coun_obj_wi_ralsha++;
					this.coun_obj_mat += transform.GetComponent<MeshRenderer>().sharedMaterials.Length;
				}
			}
			this.SubTran = new Transform[this.coun_obj_wi_ralsha];
			foreach (Transform transform2 in this.attac)
			{
				if (transform2.GetComponent<SkinnedMeshRenderer>() && transform2.GetComponent<SkinnedMeshRenderer>().sharedMaterial != null && (transform2.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_URP || transform2.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_HDRP || transform2.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DD || transform2.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DFT || transform2.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DR || transform2.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_TDD || transform2.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_TDFT || transform2.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_TDR || transform2.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DDL || transform2.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DFTL))
				{
					this.SubTran[num] = transform2;
					num++;
				}
				if (transform2.GetComponent<MeshRenderer>() && transform2.GetComponent<MeshRenderer>().sharedMaterial != null && (transform2.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_URP || transform2.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_HDRP || transform2.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DD || transform2.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DFT || transform2.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DR || transform2.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_TDD || transform2.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_TDFT || transform2.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_TDR || transform2.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DDL || transform2.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DFTL))
				{
					this.SubTran[num] = transform2;
					num++;
				}
			}
			this.Mat = new Material[this.coun_obj_mat];
			foreach (Transform transform3 in this.SubTran)
			{
				if (transform3.GetComponent<SkinnedMeshRenderer>() && transform3.GetComponent<SkinnedMeshRenderer>().sharedMaterial != null && (transform3.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_URP || transform3.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_HDRP || transform3.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DD || transform3.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DFT || transform3.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DR || transform3.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_TDD || transform3.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_TDFT || transform3.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_TDR || transform3.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DDL || transform3.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DFTL))
				{
					for (int j = 0; j < transform3.GetComponent<SkinnedMeshRenderer>().sharedMaterials.Length; j++)
					{
						this.Mat[this.coun_obj_mat_arr] = transform3.GetComponent<SkinnedMeshRenderer>().sharedMaterials[j];
						this.coun_obj_mat_arr++;
					}
				}
				if (transform3.GetComponent<MeshRenderer>() && transform3.GetComponent<MeshRenderer>().sharedMaterial != null && (transform3.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_URP || transform3.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_HDRP || transform3.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DD || transform3.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DFT || transform3.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DR || transform3.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_TDD || transform3.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_TDFT || transform3.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_TDR || transform3.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DDL || transform3.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_BiRP_DFTL))
				{
					for (int k = 0; k < transform3.GetComponent<MeshRenderer>().sharedMaterials.Length; k++)
					{
						this.Mat[this.coun_obj_mat_arr] = transform3.GetComponent<MeshRenderer>().sharedMaterials[k];
						this.coun_obj_mat_arr++;
					}
				}
			}
			foreach (Material material in this.Mat)
			{
				if (material != null)
				{
					this.Set_Shad_Prop(material);
				}
			}
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x000AAFF4 File Offset: 0x000A91F4
		private void Set_Shad_Prop(Material Mat)
		{
			if (Mat.IsKeywordEnabled("N_F_PA_ON"))
			{
				Mat.SetFloat("_PresAdju", this.Perspective);
				Mat.SetFloat("_ClipAdju", this.Clip);
				Mat.SetFloat("_PASize", this.CloseUpSize);
				Mat.SetFloat("_PASmooTrans", this.CloseUpSizeSmoothTransition);
				Mat.SetFloat("_PADist", this.CloseUpSizeDistance);
				return;
			}
			if (!Mat.IsKeywordEnabled("N_F_PA_ON"))
			{
				Mat.EnableKeyword("N_F_PA_ON");
				Mat.SetInt("_N_F_PA", 1);
			}
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x000AB088 File Offset: 0x000A9288
		private void Res_Shad_Prop()
		{
			this.Perspective = 1f;
			this.Clip = 0f;
			this.CloseUpSize = 0f;
			this.CloseUpSizeSmoothTransition = 1f;
			this.CloseUpSizeDistance = 0f;
		}

		// Token: 0x04001C2D RID: 7213
		[HideInInspector]
		[SerializeField]
		private Transform[] SubTran;

		// Token: 0x04001C2E RID: 7214
		[HideInInspector]
		[SerializeField]
		private Material[] Mat;

		// Token: 0x04001C2F RID: 7215
		[HideInInspector]
		[SerializeField]
		private Transform[] attac;

		// Token: 0x04001C30 RID: 7216
		[Header("Note: Perspective Adjustment feature will be automatically enable\nOn the materials of the object/model that uses RealToon Shader.")]
		[Header("It is recommend to set the Clipping Planes - Near of the camera\nto 0.01 to prevent slicing when near.")]
		[Space(25f)]
		[SerializeField]
		[Tooltip("This will change the perspective of an object to 2D or 3D or FOV stretch look.\nFor 2d toon/anime look, set it to 0.5 or 0.")]
		public float Perspective = 1f;

		// Token: 0x04001C31 RID: 7217
		[SerializeField]
		[Tooltip("This will change the clipping on the object.\nChange this if the object is overlapping front or back.")]
		public float Clip;

		// Token: 0x04001C32 RID: 7218
		[Space(15f)]
		[SerializeField]
		[Tooltip("This will adjust the size of the object when the camera is closer.")]
		public float CloseUpSize;

		// Token: 0x04001C33 RID: 7219
		[SerializeField]
		[Tooltip("How smooth the transition of the sizing.")]
		public float CloseUpSizeSmoothTransition = 1f;

		// Token: 0x04001C34 RID: 7220
		[SerializeField]
		[Tooltip("Distance transition from the camera to the object.")]
		public float CloseUpSizeDistance;

		// Token: 0x04001C35 RID: 7221
		[Space(15f)]
		[SerializeField]
		[Tooltip("Can animate the values/properties or create keyframe.")]
		public bool CanAnimateIt = true;

		// Token: 0x04001C36 RID: 7222
		private int coun_obj_wi_ralsha;

		// Token: 0x04001C37 RID: 7223
		private int coun_obj_mat;

		// Token: 0x04001C38 RID: 7224
		private int coun_obj_mat_arr;

		// Token: 0x04001C39 RID: 7225
		private string RT_Sha_Nam_URP = "Universal Render Pipeline/RealToon/Version 5/Default/Default";

		// Token: 0x04001C3A RID: 7226
		private string RT_Sha_Nam_HDRP = "HDRP/RealToon/Version 5/Default";

		// Token: 0x04001C3B RID: 7227
		private string RT_Sha_Nam_BiRP_DD = "RealToon/Version 5/Default/Default";

		// Token: 0x04001C3C RID: 7228
		private string RT_Sha_Nam_BiRP_DFT = "RealToon/Version 5/Default/Fade Transparency";

		// Token: 0x04001C3D RID: 7229
		private string RT_Sha_Nam_BiRP_DR = "RealToon/Version 5/Default/Refraction";

		// Token: 0x04001C3E RID: 7230
		private string RT_Sha_Nam_BiRP_TDD = "RealToon/Version 5/Tessellation/Default";

		// Token: 0x04001C3F RID: 7231
		private string RT_Sha_Nam_BiRP_TDFT = "RealToon/Version 5/Tessellation/Fade Transparency";

		// Token: 0x04001C40 RID: 7232
		private string RT_Sha_Nam_BiRP_TDR = "RealToon/Version 5/Tessellation/Refraction";

		// Token: 0x04001C41 RID: 7233
		private string RT_Sha_Nam_BiRP_DDL = "RealToon/Version 5/Lite/Default";

		// Token: 0x04001C42 RID: 7234
		private string RT_Sha_Nam_BiRP_DFTL = "RealToon/Version 5/Lite/Fade Transparency";

		// Token: 0x04001C43 RID: 7235
		[HideInInspector]
		[SerializeField]
		private bool checkstart = true;
	}
}
