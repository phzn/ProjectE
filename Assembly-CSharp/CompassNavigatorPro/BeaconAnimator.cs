using System;
using UnityEngine;

namespace CompassNavigatorPro
{
	// Token: 0x0200027E RID: 638
	public class BeaconAnimator : MonoBehaviour
	{
		// Token: 0x060010AB RID: 4267 RVA: 0x0006F684 File Offset: 0x0006D884
		private void Awake()
		{
			this.mat = base.GetComponent<Renderer>().material;
			this.fullyTransparentColor = new Color(0f, 0f, 0f, 0f);
			this.duration = 1f;
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x0006F6C4 File Offset: 0x0006D8C4
		private void Start()
		{
			this.startingTime = Time.time;
			this.originalColor = this.mat.color * this.tintColor * this.intensity;
			this.mat.SetColor(BeaconAnimator.ShaderParams.EmissionColor, this.tintColor);
			this.UpdateColor();
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x0006F71F File Offset: 0x0006D91F
		private void OnDisable()
		{
			this.DestroyBeacon();
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x0006F728 File Offset: 0x0006D928
		private void Update()
		{
			float time = Time.time;
			this.mat.mainTextureOffset = new Vector2(time * -0.25f, time * -0.25f);
			this.mat.SetTextureOffset(BeaconAnimator.ShaderParams.EmissionMap, new Vector2(time * -0.15f, time * -0.2f));
			this.UpdateColor();
			if (this.target != null)
			{
				base.transform.position = this.target.position + new Vector3(0f, base.transform.localScale.y * 0.5f, 0f);
			}
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x0006F7D0 File Offset: 0x0006D9D0
		private void UpdateColor()
		{
			float num = ((this.duration <= 0f) ? 1f : Mathf.Clamp01((Time.time - this.startingTime) / this.duration));
			if (num >= 1f)
			{
				this.DestroyBeacon();
				return;
			}
			float num2 = this.Ease(num);
			this.mat.color = Color.Lerp(this.fullyTransparentColor, this.originalColor, num2);
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x0006F83E File Offset: 0x0006DA3E
		private float Ease(float t)
		{
			return Mathf.Sin(t * 3.1415927f);
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x0006F84C File Offset: 0x0006DA4C
		private void DestroyBeacon()
		{
			if (this.mat != null)
			{
				Object.DestroyImmediate(this.mat);
				this.mat = null;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x0400121F RID: 4639
		public Transform target;

		// Token: 0x04001220 RID: 4640
		public float intensity = 5f;

		// Token: 0x04001221 RID: 4641
		public float duration;

		// Token: 0x04001222 RID: 4642
		public Color tintColor;

		// Token: 0x04001223 RID: 4643
		private float startingTime;

		// Token: 0x04001224 RID: 4644
		private Material mat;

		// Token: 0x04001225 RID: 4645
		private Color fullyTransparentColor;

		// Token: 0x04001226 RID: 4646
		private Color originalColor;

		// Token: 0x0200027F RID: 639
		private static class ShaderParams
		{
			// Token: 0x04001227 RID: 4647
			public static int EmissionMap = Shader.PropertyToID("_EmissionMap");

			// Token: 0x04001228 RID: 4648
			public static int EmissionColor = Shader.PropertyToID("_EmissionColor");
		}
	}
}
