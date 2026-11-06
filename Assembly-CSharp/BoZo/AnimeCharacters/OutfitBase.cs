using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008BD RID: 2237
	public abstract class OutfitBase : MonoBehaviour
	{
		// Token: 0x06003C0F RID: 15375 RVA: 0x00165738 File Offset: 0x00163938
		private void Awake()
		{
			this.material = base.GetComponentInChildren<Renderer>().material;
		}

		// Token: 0x06003C10 RID: 15376 RVA: 0x00004F31 File Offset: 0x00003131
		public virtual void SetColor(Color color, int index, bool linkedChanged = false)
		{
		}

		// Token: 0x06003C11 RID: 15377 RVA: 0x0016574C File Offset: 0x0016394C
		public virtual void SetColor(Color color)
		{
			if (!this.material)
			{
				this.material = base.GetComponentInChildren<Renderer>().material;
			}
			MonoBehaviour.print("!");
			if (this.material.HasProperty("_Color_1"))
			{
				this.material.SetColor("_Color_1", color);
				return;
			}
			if (this.material.HasProperty("_Color"))
			{
				this.material.SetColor("_Color", color);
				return;
			}
			if (this.material.HasProperty("_MainColor"))
			{
				this.material.SetColor("_MainColor", color);
			}
		}

		// Token: 0x06003C12 RID: 15378 RVA: 0x001657EC File Offset: 0x001639EC
		public virtual List<Color> GetColors()
		{
			if (!this.material)
			{
				this.material = base.GetComponentInChildren<Renderer>().material;
			}
			List<Color> list = new List<Color>();
			if (this.customShader)
			{
				list.Add(this.material.color);
				return list;
			}
			for (int i = 1; i <= 9; i++)
			{
				list.Add(this.material.GetColor("_Color_" + i.ToString()));
			}
			return list;
		}

		// Token: 0x06003C13 RID: 15379 RVA: 0x00165868 File Offset: 0x00163A68
		public virtual Color GetColor(int channel)
		{
			if (!this.material)
			{
				this.material = base.GetComponentInChildren<Renderer>().material;
			}
			if (!this.customShader)
			{
				return this.material.GetColor("_Color_" + channel.ToString());
			}
			if (this.material.HasProperty("_Color_1"))
			{
				return this.material.GetColor("_Color_1");
			}
			if (this.material.HasProperty("_Color"))
			{
				return this.material.GetColor("_Color");
			}
			if (this.material.HasProperty("_MainColor"))
			{
				return this.material.GetColor("_MainColor");
			}
			return Color.white;
		}

		// Token: 0x06003C14 RID: 15380 RVA: 0x00004F31 File Offset: 0x00003131
		public virtual void SetSwatch(int swatchIndex, bool linkedChange = false)
		{
		}

		// Token: 0x06003C15 RID: 15381 RVA: 0x00165928 File Offset: 0x00163B28
		public virtual void SetDecal(Texture texture)
		{
			if (!this.material)
			{
				this.material = base.GetComponentInChildren<Renderer>().material;
			}
			if (texture == null)
			{
				this.material.SetTexture("_DecalMap", null);
				return;
			}
			this.material.SetTexture("_DecalMap", texture);
		}

		// Token: 0x06003C16 RID: 15382 RVA: 0x0016597F File Offset: 0x00163B7F
		public virtual void SetDecalSize(Vector4 size)
		{
			if (!this.material)
			{
				this.material = base.GetComponentInChildren<Renderer>().material;
			}
			this.material.SetVector("_DecalScale", size);
		}

		// Token: 0x06003C17 RID: 15383 RVA: 0x001659B0 File Offset: 0x00163BB0
		public virtual Vector4 GetDecalSize()
		{
			if (!this.material)
			{
				this.material = base.GetComponentInChildren<Renderer>().material;
			}
			Vector4 vector = this.material.GetVector("_DecalScale");
			return new Vector4(vector.x, vector.y, 0f, 0f);
		}

		// Token: 0x06003C18 RID: 15384 RVA: 0x00165A07 File Offset: 0x00163C07
		public virtual Texture GetDecal()
		{
			if (!this.material)
			{
				this.material = base.GetComponentInChildren<Renderer>().material;
			}
			return this.material.GetTexture("_DecalMap");
		}

		// Token: 0x06003C19 RID: 15385 RVA: 0x00165A37 File Offset: 0x00163C37
		public virtual void SetDecalColor(Color color, int index)
		{
			if (!this.material)
			{
				this.material = base.GetComponentInChildren<Renderer>().material;
			}
			this.material.SetColor("_DecalColor_" + index.ToString(), color);
		}

		// Token: 0x06003C1A RID: 15386 RVA: 0x00165A74 File Offset: 0x00163C74
		public virtual Color GetDecalColor(int index)
		{
			if (!this.material)
			{
				this.material = base.GetComponentInChildren<Renderer>().material;
			}
			return this.material.GetColor("_DecalColor_" + index.ToString());
		}

		// Token: 0x06003C1B RID: 15387 RVA: 0x00165AB0 File Offset: 0x00163CB0
		public virtual List<Color> GetDecalColors()
		{
			if (!this.material)
			{
				this.material = base.GetComponentInChildren<Renderer>().material;
			}
			List<Color> list = new List<Color>();
			for (int i = 1; i < 4; i++)
			{
				list.Add(this.material.GetColor("_DecalColor_" + i.ToString()));
			}
			return list;
		}

		// Token: 0x06003C1C RID: 15388 RVA: 0x00165B10 File Offset: 0x00163D10
		public virtual void SetPattern(Texture texture)
		{
			if (!this.material)
			{
				this.material = base.GetComponentInChildren<Renderer>().material;
			}
			this.material.SetTexture("_PatternMap", texture);
		}

		// Token: 0x06003C1D RID: 15389 RVA: 0x00165B41 File Offset: 0x00163D41
		public virtual Texture GetPattern()
		{
			if (!this.material)
			{
				this.material = base.GetComponentInChildren<Renderer>().material;
			}
			return this.material.GetTexture("_PatternMap");
		}

		// Token: 0x06003C1E RID: 15390 RVA: 0x00165B71 File Offset: 0x00163D71
		public virtual void SetPatternColor(Color color, int index)
		{
			if (!this.material)
			{
				this.material = base.GetComponentInChildren<Renderer>().material;
			}
			this.material.SetColor("_PatternColor_" + index.ToString(), color);
		}

		// Token: 0x06003C1F RID: 15391 RVA: 0x00165BAE File Offset: 0x00163DAE
		public virtual Color GetPatternColor(int index)
		{
			if (!this.material)
			{
				this.material = base.GetComponentInChildren<Renderer>().material;
			}
			return this.material.GetColor("_PatternColor_" + index.ToString());
		}

		// Token: 0x06003C20 RID: 15392 RVA: 0x00165BEC File Offset: 0x00163DEC
		public virtual List<Color> GetPatternColors()
		{
			if (!this.material)
			{
				this.material = base.GetComponentInChildren<Renderer>().material;
			}
			List<Color> list = new List<Color>();
			for (int i = 1; i < 4; i++)
			{
				list.Add(this.material.GetColor("_PatternColor_" + i.ToString()));
			}
			return list;
		}

		// Token: 0x06003C21 RID: 15393 RVA: 0x00165C4C File Offset: 0x00163E4C
		public virtual void SetPatternSize(Vector2 size)
		{
			if (!this.material)
			{
				this.material = base.GetComponentInChildren<Renderer>().material;
			}
			this.material.SetVector("_PatternScale", size);
		}

		// Token: 0x06003C22 RID: 15394 RVA: 0x00165C84 File Offset: 0x00163E84
		public virtual Vector4 GetPatternSize()
		{
			if (!this.material)
			{
				this.material = base.GetComponentInChildren<Renderer>().material;
			}
			Vector4 vector = this.material.GetVector("_PatternScale");
			return new Vector4(vector.x, vector.y, 0f, 0f);
		}

		// Token: 0x06003C23 RID: 15395 RVA: 0x00004F31 File Offset: 0x00003131
		public virtual void SetBaseTexture(Texture texture, Texture normalTexture = null)
		{
		}

		// Token: 0x04003C37 RID: 15415
		public bool customShader;

		// Token: 0x04003C38 RID: 15416
		public bool skipFromMerge;

		// Token: 0x04003C39 RID: 15417
		protected Material material;
	}
}
