using System;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000759 RID: 1881
	[SelectionBase]
	public class MudStamp : MonoBehaviour
	{
		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x060030A2 RID: 12450 RVA: 0x001128C6 File Offset: 0x00110AC6
		public DynamicMudSurface DynamicMudSurface
		{
			get
			{
				return this.dynamicMudSurface;
			}
		}

		// Token: 0x060030A3 RID: 12451 RVA: 0x001128CE File Offset: 0x00110ACE
		public void SetFlipX(bool flipX)
		{
			this.flipX = flipX;
		}

		// Token: 0x060030A4 RID: 12452 RVA: 0x001128D7 File Offset: 0x00110AD7
		public void SetFlipY(bool flipY)
		{
			this.flipY = flipY;
		}

		// Token: 0x060030A5 RID: 12453 RVA: 0x001128E0 File Offset: 0x00110AE0
		public void SetSurface(DynamicMudSurface newSurface)
		{
			this.dynamicMudSurface = newSurface;
		}

		// Token: 0x060030A6 RID: 12454 RVA: 0x001128E9 File Offset: 0x00110AE9
		public void SetBendAmount(float rotationAmount)
		{
			this.bendWithRotation = true;
			rotationAmount = Mathf.Clamp(rotationAmount, -45f, 45f);
			this.bendAmount = rotationAmount * 0.5f / 45f;
		}

		// Token: 0x060030A7 RID: 12455 RVA: 0x00112918 File Offset: 0x00110B18
		public void StampSurface()
		{
			if (this.dynamicMudSurface == null)
			{
				return;
			}
			if (this.StampMaterial == null || this.StampTexture == null)
			{
				return;
			}
			this.stampCustomRenderTex = this.dynamicMudSurface.StampRenderTexture;
			if (this.stampCustomRenderTex == null)
			{
				return;
			}
			Vector4 transformInTextureSpace = this.GetTransformInTextureSpace(this.StampTexture);
			Vector2 vector = new Vector2(transformInTextureSpace.x, transformInTextureSpace.y);
			float num = transformInTextureSpace.z;
			float w = transformInTextureSpace.w;
			num *= 10f / this.dynamicMudSurface.BoundingBoxSizeInUnits;
			this.SendStampTexToCustomRenderTex(this.StampMaterial, this.StampTexture, vector, num, w, 0f);
			this.stampCustomRenderTex.Update();
			this.stampCustomRenderTex.Update();
		}

		// Token: 0x060030A8 RID: 12456 RVA: 0x001129E4 File Offset: 0x00110BE4
		public Vector4 GetTransformInTextureSpace(Texture2D stampTexture)
		{
			Bounds bounds = this.dynamicMudSurface.GetComponent<Renderer>().bounds;
			Vector3 vector = this.dynamicMudSurface.transform.InverseTransformPoint(bounds.center);
			Vector3 vector2 = this.dynamicMudSurface.transform.InverseTransformPoint(base.transform.position) + vector;
			float num = Mathf.Max(bounds.size.x, bounds.size.z) * 0.5f;
			vector2 += Vector3.one * num;
			Vector3 vector3 = new Vector3(vector2.x / Mathf.Max(bounds.size.x, bounds.size.z), vector2.y / bounds.size.y, vector2.z / Mathf.Max(bounds.size.x, bounds.size.z));
			Vector2 vector4 = new Vector2(vector3.x, vector3.z);
			float num2 = -1f * (this.dynamicMudSurface.transform.rotation.eulerAngles.y - base.transform.rotation.eulerAngles.y);
			float num3 = base.transform.localScale.x * 0.1f;
			return new Vector4(vector4.x, vector4.y, num3, num2);
		}

		// Token: 0x060030A9 RID: 12457 RVA: 0x00112B58 File Offset: 0x00110D58
		public void SendStampTexToCustomRenderTex(Material stampMat, Texture2D stampTexture, Vector2 texRelativePosition, float scale, float localRot, float rotDifference = 0f)
		{
			if (stampMat == null || stampTexture == null || this.stampCustomRenderTex == null)
			{
				return;
			}
			stampMat.SetTexture("_Tex", stampTexture);
			stampMat.SetTexture("_Tex2", this.stampCustomRenderTex);
			stampMat.SetFloat("_PosX", texRelativePosition.x);
			stampMat.SetFloat("_PosY", texRelativePosition.y);
			stampMat.SetFloat("_Scale", scale);
			stampMat.SetFloat("_Degrees", localRot);
			if (this.flipX)
			{
				stampMat.SetFloat("_FlipX", 1f);
			}
			else
			{
				stampMat.SetFloat("_FlipX", 0f);
			}
			if (this.flipY)
			{
				stampMat.SetFloat("_FlipY", 1f);
			}
			else
			{
				stampMat.SetFloat("_FlipY", 0f);
			}
			if (this.bendWithRotation)
			{
				stampMat.SetFloat("_Bend", this.bendAmount);
			}
			this.stampCustomRenderTex.material = stampMat;
		}

		// Token: 0x04003118 RID: 12568
		[Header("References")]
		[Tooltip("Normal texture with alpha that stamp will draw on the Custom Render Texture.")]
		public Texture2D StampTexture;

		// Token: 0x04003119 RID: 12569
		[Tooltip("Material that uses the Stamp shader to draw to the Custom Render Texture")]
		public Material StampMaterial;

		// Token: 0x0400311A RID: 12570
		[Tooltip("Reference to the mud surface that stamp is on")]
		[SerializeField]
		private DynamicMudSurface dynamicMudSurface;

		// Token: 0x0400311B RID: 12571
		[Header("Settings")]
		[Tooltip("Instruct the shader drawing the stamp to warp the stamp toward the current foward direction of the stamp.  Useful for long tracks in the mud.")]
		[SerializeField]
		private bool bendWithRotation;

		// Token: 0x0400311C RID: 12572
		private bool flipX;

		// Token: 0x0400311D RID: 12573
		private bool flipY;

		// Token: 0x0400311E RID: 12574
		private CustomRenderTexture stampCustomRenderTex;

		// Token: 0x0400311F RID: 12575
		private float bendAmount;
	}
}
