using System;
using UnityEngine;

namespace ProjectExhibitionist.Subway
{
	// Token: 0x0200093F RID: 2367
	public class SubwayTunnelScroller : MonoBehaviour
	{
		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06004070 RID: 16496 RVA: 0x0017EC78 File Offset: 0x0017CE78
		public float Speed
		{
			get
			{
				return this._speed;
			}
		}

		// Token: 0x06004071 RID: 16497 RVA: 0x0017EC80 File Offset: 0x0017CE80
		private void Awake()
		{
			int num = this.wallRenderers.Length;
			this._all = new Renderer[num + this.capRenderers.Length];
			this._allMats = new Material[this._all.Length];
			this._wallMats = new Material[num];
			this._baseRgb = new Color[this._all.Length];
			this._offsets = new float[num];
			for (int i = 0; i < this._all.Length; i++)
			{
				this._all[i] = ((i < num) ? this.wallRenderers[i] : this.capRenderers[i - num]);
				if (!(this._all[i] == null))
				{
					this._allMats[i] = this._all[i].material;
					this._baseRgb[i] = this._allMats[i].GetColor(SubwayTunnelScroller.BaseColorId);
					if (i < num)
					{
						this._wallMats[i] = this._allMats[i];
					}
				}
			}
			this._alpha = 0f;
			this._alphaTarget = 0f;
			this.ApplyAlpha();
		}

		// Token: 0x06004072 RID: 16498 RVA: 0x0017ED92 File Offset: 0x0017CF92
		public void SetSpeed(float metersPerSecond)
		{
			this._speed = metersPerSecond;
		}

		// Token: 0x06004073 RID: 16499 RVA: 0x0017ED9B File Offset: 0x0017CF9B
		public void Show(float fadeSeconds = 0.1f)
		{
			this._alphaTarget = 1f;
			this._fadeSpeed = 1f / Mathf.Max(0.02f, fadeSeconds);
		}

		// Token: 0x06004074 RID: 16500 RVA: 0x0017EDBF File Offset: 0x0017CFBF
		public void Hide(float fadeSeconds = 0.1f)
		{
			this._alphaTarget = 0f;
			this._fadeSpeed = 1f / Mathf.Max(0.02f, fadeSeconds);
		}

		// Token: 0x06004075 RID: 16501 RVA: 0x0017EDE4 File Offset: 0x0017CFE4
		private void Update()
		{
			bool flag = this._alpha > 0.001f;
			if (!Mathf.Approximately(this._alpha, this._alphaTarget))
			{
				this._alpha = Mathf.MoveTowards(this._alpha, this._alphaTarget, this._fadeSpeed * Time.deltaTime);
				this.ApplyAlpha();
			}
			if (!flag && this._alpha <= 0.001f)
			{
				return;
			}
			if (this._speed != 0f)
			{
				float num = this._speed * Time.deltaTime / Mathf.Max(0.01f, this.textureWorldLength);
				for (int i = 0; i < this._wallMats.Length; i++)
				{
					if (!(this._wallMats[i] == null))
					{
						float num2 = ((i < this.scrollSigns.Length) ? this.scrollSigns[i] : 1f);
						this._offsets[i] = Mathf.Repeat(this._offsets[i] + num * num2, 1f);
						this._wallMats[i].SetTextureOffset(SubwayTunnelScroller.BaseMapId, new Vector2(this._offsets[i], 0f));
					}
				}
			}
		}

		// Token: 0x06004076 RID: 16502 RVA: 0x0017EEF8 File Offset: 0x0017D0F8
		private void ApplyAlpha()
		{
			bool flag = this._alpha > 0.001f;
			for (int i = 0; i < this._all.Length; i++)
			{
				if (!(this._all[i] == null))
				{
					this._all[i].enabled = flag;
					if (this._allMats[i] != null)
					{
						Color color = this._baseRgb[i];
						color.a = this._alpha;
						this._allMats[i].SetColor(SubwayTunnelScroller.BaseColorId, color);
					}
				}
			}
		}

		// Token: 0x0400403F RID: 16447
		[Header("Walls (authored by Tools/Subway)")]
		[SerializeField]
		private Renderer[] wallRenderers = new Renderer[0];

		// Token: 0x04004040 RID: 16448
		[Tooltip("+1/-1 per renderer: offset sign so the texture streams opposite to travel on both sides.")]
		[SerializeField]
		private float[] scrollSigns = new float[0];

		// Token: 0x04004041 RID: 16449
		[Tooltip("Static dark caps closing the tunnel box at the train's front/back ends.")]
		[SerializeField]
		private Renderer[] capRenderers = new Renderer[0];

		// Token: 0x04004042 RID: 16450
		[Header("Scroll")]
		[Tooltip("Meters of tunnel wall covered by ONE horizontal repeat of the texture.")]
		[SerializeField]
		private float textureWorldLength = 8f;

		// Token: 0x04004043 RID: 16451
		private Renderer[] _all = new Renderer[0];

		// Token: 0x04004044 RID: 16452
		private Material[] _wallMats = new Material[0];

		// Token: 0x04004045 RID: 16453
		private Material[] _allMats = new Material[0];

		// Token: 0x04004046 RID: 16454
		private Color[] _baseRgb = new Color[0];

		// Token: 0x04004047 RID: 16455
		private float[] _offsets = new float[0];

		// Token: 0x04004048 RID: 16456
		private float _speed;

		// Token: 0x04004049 RID: 16457
		private float _alpha;

		// Token: 0x0400404A RID: 16458
		private float _alphaTarget;

		// Token: 0x0400404B RID: 16459
		private float _fadeSpeed = 2f;

		// Token: 0x0400404C RID: 16460
		private static readonly int BaseMapId = Shader.PropertyToID("_BaseMap");

		// Token: 0x0400404D RID: 16461
		private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
	}
}
