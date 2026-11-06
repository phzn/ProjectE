using System;
using System.Collections.Generic;
using UnityEngine;

namespace RealToon.Script
{
	// Token: 0x0200043B RID: 1083
	[ExecuteAlways]
	[AddComponentMenu("RealToon/Tools/Smear Effect - Helper")]
	public class SmearEffectHelper : MonoBehaviour
	{
		// Token: 0x06001B3E RID: 6974 RVA: 0x000AB5DD File Offset: 0x000A97DD
		private void Start()
		{
			if (this.checkstart)
			{
				this.InitStart();
				this.checkstart = false;
			}
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x000AB5F4 File Offset: 0x000A97F4
		private void LateUpdate()
		{
			if (this.SmearController != null && !this.PauseSmear && this.Mat != null)
			{
				foreach (Material material in this.Mat)
				{
					if (material != null)
					{
						material.SetVector("_ObjPosi", this.SmearController.position);
						this.recentPositions.Enqueue(this.SmearController.position);
						if (this.recentPositions.Count > this.Delay)
						{
							material.SetVector("_PrevPosition", this.recentPositions.Dequeue());
						}
						this.Set_Shad_Prop(material);
					}
				}
			}
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x000AB6B0 File Offset: 0x000A98B0
		private void Reset()
		{
			if (this.Mat != null)
			{
				foreach (Material material in this.Mat)
				{
					if (material != null)
					{
						material.SetVector("_ObjPosi", new Vector4(0f, 0f, 0f, 0f));
						material.SetVector("_PrevPosition", new Vector4(0f, 0f, 0f, 0f));
					}
				}
				this.recentPositions.Dequeue();
				this.recentPositions.Clear();
				this.checkstart = true;
				this.coun_obj_wi_ralsha = 0;
				this.coun_obj_mat = 0;
				this.coun_obj_mat_arr = 0;
				this.Res_Shad_Prop();
				this.InitStart();
				this.checkstart = false;
			}
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x000AB778 File Offset: 0x000A9978
		private void OnDisable()
		{
			this.recentPositions.Dequeue();
			foreach (Material material in this.Mat)
			{
				if (material != null)
				{
					material.SetVector("_ObjPosi", new Vector4(0f, 0f, 0f, 0f));
					material.SetVector("_PrevPosition", new Vector4(0f, 0f, 0f, 0f));
				}
			}
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x000AB7FC File Offset: 0x000A99FC
		private void InitStart()
		{
			if (this.attac == null || this.attac.Length == 0)
			{
				this.attac = base.gameObject.GetComponentsInChildren<Transform>();
			}
			if (this.SmearController == null)
			{
				this.SmearController = base.gameObject.transform;
			}
			int num = 0;
			foreach (Transform transform in this.attac)
			{
				if (transform.GetComponent<SkinnedMeshRenderer>() && transform.GetComponent<SkinnedMeshRenderer>().sharedMaterial != null && (transform.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_URP || transform.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_HDRP))
				{
					this.coun_obj_wi_ralsha++;
					this.coun_obj_mat += transform.GetComponent<SkinnedMeshRenderer>().sharedMaterials.Length;
				}
				if (transform.GetComponent<MeshRenderer>() && transform.GetComponent<MeshRenderer>().sharedMaterial != null && (transform.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_URP || transform.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_HDRP))
				{
					this.coun_obj_wi_ralsha++;
					this.coun_obj_mat += transform.GetComponent<MeshRenderer>().sharedMaterials.Length;
				}
			}
			this.SubTran = new Transform[this.coun_obj_wi_ralsha];
			foreach (Transform transform2 in this.attac)
			{
				if (transform2.GetComponent<SkinnedMeshRenderer>() && transform2.GetComponent<SkinnedMeshRenderer>().sharedMaterial != null && (transform2.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_URP || transform2.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_HDRP))
				{
					this.SubTran[num] = transform2;
					num++;
				}
				if (transform2.GetComponent<MeshRenderer>() && transform2.GetComponent<MeshRenderer>().sharedMaterial != null && (transform2.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_URP || transform2.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_HDRP))
				{
					this.SubTran[num] = transform2;
					num++;
				}
			}
			this.Mat = new Material[this.coun_obj_mat];
			foreach (Transform transform3 in this.SubTran)
			{
				if (transform3.GetComponent<SkinnedMeshRenderer>() && transform3.GetComponent<SkinnedMeshRenderer>().sharedMaterial != null && (transform3.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_URP || transform3.GetComponent<SkinnedMeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_HDRP))
				{
					for (int j = 0; j < transform3.GetComponent<SkinnedMeshRenderer>().sharedMaterials.Length; j++)
					{
						this.Mat[this.coun_obj_mat_arr] = transform3.GetComponent<SkinnedMeshRenderer>().sharedMaterials[j];
						this.coun_obj_mat_arr++;
					}
				}
				if (transform3.GetComponent<MeshRenderer>() && transform3.GetComponent<MeshRenderer>().sharedMaterial != null && (transform3.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_URP || transform3.GetComponent<MeshRenderer>().sharedMaterial.shader.name == this.RT_Sha_Nam_HDRP))
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

		// Token: 0x06001B43 RID: 6979 RVA: 0x000ABC6C File Offset: 0x000A9E6C
		private void Set_Shad_Prop(Material Mat)
		{
			if (Mat.IsKeywordEnabled("N_F_SE_ON"))
			{
				Mat.SetFloat("_NoiseSize", this.NoiseSize);
				Mat.SetFloat("_TrailSize", this.TrailSize);
				return;
			}
			if (!Mat.IsKeywordEnabled("N_F_SE_ON"))
			{
				Mat.EnableKeyword("N_F_SE_ON");
				Mat.SetInt("_N_F_SE", 1);
			}
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x000ABCCD File Offset: 0x000A9ECD
		private void Res_Shad_Prop()
		{
			this.NoiseSize = 100f;
			this.TrailSize = 1.5f;
			this.Delay = 15;
		}

		// Token: 0x04001C52 RID: 7250
		private Queue<Vector3> recentPositions = new Queue<Vector3>();

		// Token: 0x04001C53 RID: 7251
		[HideInInspector]
		[SerializeField]
		private Transform[] SubTran;

		// Token: 0x04001C54 RID: 7252
		[HideInInspector]
		[SerializeField]
		private Material[] Mat;

		// Token: 0x04001C55 RID: 7253
		[HideInInspector]
		[SerializeField]
		private Transform[] attac;

		// Token: 0x04001C56 RID: 7254
		[Header("Note: Smear Effect feature will be automatically enable\nOn the materials of the object/model that uses RealToon Shader.")]
		[Space(25f)]
		[SerializeField]
		[Tooltip("An object to control the smear effect.")]
		public Transform SmearController;

		// Token: 0x04001C57 RID: 7255
		[Space(10f)]
		[SerializeField]
		[Tooltip("How long the distorted line trails stays on the previous position.")]
		private int Delay = 15;

		// Token: 0x04001C58 RID: 7256
		[SerializeField]
		[Tooltip("How large/small the trailing noise.")]
		private float NoiseSize = 100f;

		// Token: 0x04001C59 RID: 7257
		[SerializeField]
		[Tooltip("How tall/short the trailing noise.")]
		private float TrailSize = 1.5f;

		// Token: 0x04001C5A RID: 7258
		[Space(10f)]
		[SerializeField]
		[Tooltip("Pause the current smear effect.")]
		private bool PauseSmear;

		// Token: 0x04001C5B RID: 7259
		private int coun_obj_wi_ralsha;

		// Token: 0x04001C5C RID: 7260
		private int coun_obj_mat;

		// Token: 0x04001C5D RID: 7261
		private int coun_obj_mat_arr;

		// Token: 0x04001C5E RID: 7262
		[HideInInspector]
		[SerializeField]
		private bool checkstart = true;

		// Token: 0x04001C5F RID: 7263
		private string RT_Sha_Nam_URP = "Universal Render Pipeline/RealToon/Version 5/Default/Default";

		// Token: 0x04001C60 RID: 7264
		private string RT_Sha_Nam_HDRP = "HDRP/RealToon/Version 5/Default";
	}
}
