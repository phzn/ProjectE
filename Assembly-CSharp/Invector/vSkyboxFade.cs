using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Invector
{
	// Token: 0x020004B2 RID: 1202
	[vClassHeader("Skybox Fade", true, "icon_v2", false, "", helpBoxText = "A Skybox with Cubemap type is Required to use this Component", useHelpBox = true)]
	public class vSkyboxFade : vMonoBehaviour
	{
		// Token: 0x06001D44 RID: 7492 RVA: 0x000B3E30 File Offset: 0x000B2030
		private void Start()
		{
			this.skybox = RenderSettings.skybox;
			this.lastFadeSettings = new vSkyboxFade.SkyboxFadeSettings();
			this.currentFadeSettings = this.defaultSkyboxSettings.Copy();
			this.currentFadeSettings = new vSkyboxFade.SkyboxFadeSettings(this.skybox);
			this.skybox.SetColor("_Tint", this.defaultSkyboxSettings.tint);
			this.skybox.SetFloat("_Exposure", this.defaultSkyboxSettings.exposure);
			this.skybox.SetFloat("_Rotation", this.defaultSkyboxSettings.rotation);
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x000B3EC8 File Offset: 0x000B20C8
		private void OnApplicationQuit()
		{
			this.skybox.SetColor("_Tint", this.defaultSkyboxSettings.tint);
			this.skybox.SetFloat("_Exposure", this.defaultSkyboxSettings.exposure);
			this.skybox.SetFloat("_Rotation", this.defaultSkyboxSettings.rotation);
		}

		// Token: 0x06001D46 RID: 7494 RVA: 0x000B3F28 File Offset: 0x000B2128
		public void Fade(string _fadeName)
		{
			this.targetFadeSettings = this.fadeSettings.Find((vSkyboxFade.SkyboxFadeSettings f) => f.name.Equals(_fadeName));
			if (this.targetFadeSettings != null)
			{
				this.exitRoutine = true;
				base.StartCoroutine(this.FadeRoutine());
			}
		}

		// Token: 0x06001D47 RID: 7495 RVA: 0x000B3F7B File Offset: 0x000B217B
		public void FadeToDefault()
		{
			this.targetFadeSettings = this.defaultSkyboxSettings.Copy();
			this.exitRoutine = true;
			base.StartCoroutine(this.FadeRoutine());
		}

		// Token: 0x06001D48 RID: 7496 RVA: 0x000B3FA2 File Offset: 0x000B21A2
		private IEnumerator FadeRoutine()
		{
			yield return new WaitForEndOfFrame();
			this.exitRoutine = false;
			this.lastFadeSettings.tint = this.currentFadeSettings.tint;
			this.lastFadeSettings.exposure = this.currentFadeSettings.exposure;
			this.lastFadeSettings.rotation = this.currentFadeSettings.rotation;
			float timer = 0f;
			if (!(this.lastFadeSettings.tint == this.targetFadeSettings.tint) || this.lastFadeSettings.exposure != this.targetFadeSettings.exposure || this.targetFadeSettings.rotation != this.lastFadeSettings.rotation)
			{
				do
				{
					this.currentFadeSettings.tint = Color.Lerp(this.lastFadeSettings.tint, this.targetFadeSettings.tint, this.targetFadeSettings.curve.Evaluate(timer));
					this.currentFadeSettings.exposure = Mathf.Lerp(this.lastFadeSettings.exposure, this.targetFadeSettings.exposure, this.targetFadeSettings.curve.Evaluate(timer));
					this.currentFadeSettings.rotation = Mathf.Lerp(this.lastFadeSettings.rotation, this.targetFadeSettings.rotation, this.targetFadeSettings.curve.Evaluate(timer));
					this.skybox.SetColor("_Tint", this.currentFadeSettings.tint);
					this.skybox.SetFloat("_Exposure", this.currentFadeSettings.exposure);
					this.skybox.SetFloat("_Rotation", this.currentFadeSettings.rotation);
					yield return null;
					if (timer >= 1f)
					{
						break;
					}
					timer += Time.fixedDeltaTime / this.targetFadeSettings.fadeTime;
				}
				while (!this.exitRoutine);
			}
			yield break;
		}

		// Token: 0x04001E52 RID: 7762
		public vSkyboxFade.SkyboxFadeSettings defaultSkyboxSettings;

		// Token: 0x04001E53 RID: 7763
		public List<vSkyboxFade.SkyboxFadeSettings> fadeSettings;

		// Token: 0x04001E54 RID: 7764
		private vSkyboxFade.SkyboxFadeSettings currentFadeSettings;

		// Token: 0x04001E55 RID: 7765
		private vSkyboxFade.SkyboxFadeSettings lastFadeSettings;

		// Token: 0x04001E56 RID: 7766
		private vSkyboxFade.SkyboxFadeSettings targetFadeSettings;

		// Token: 0x04001E57 RID: 7767
		private Material skybox;

		// Token: 0x04001E58 RID: 7768
		private bool exitRoutine;

		// Token: 0x020004B3 RID: 1203
		[Serializable]
		public class SkyboxFadeSettings
		{
			// Token: 0x06001D4A RID: 7498 RVA: 0x000B3FB4 File Offset: 0x000B21B4
			public SkyboxFadeSettings()
			{
			}

			// Token: 0x06001D4B RID: 7499 RVA: 0x000B4028 File Offset: 0x000B2228
			public SkyboxFadeSettings(Material mat)
			{
				this.tint = mat.GetColor("_Tint");
				this.exposure = mat.GetFloat("_Exposure");
				this.rotation = mat.GetFloat("_Rotation");
			}

			// Token: 0x06001D4C RID: 7500 RVA: 0x000B40CD File Offset: 0x000B22CD
			public void CopyMaterial(Material mat)
			{
				this.tint = mat.GetColor("_Tint");
				this.exposure = mat.GetFloat("_Exposure");
				this.rotation = mat.GetFloat("_Rotation");
			}

			// Token: 0x06001D4D RID: 7501 RVA: 0x000B4104 File Offset: 0x000B2304
			public vSkyboxFade.SkyboxFadeSettings Copy()
			{
				return new vSkyboxFade.SkyboxFadeSettings
				{
					curve = this.curve,
					fadeTime = this.fadeTime,
					tint = this.tint,
					exposure = this.exposure,
					rotation = this.rotation
				};
			}

			// Token: 0x04001E59 RID: 7769
			public string name = "My SkySettings";

			// Token: 0x04001E5A RID: 7770
			public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

			// Token: 0x04001E5B RID: 7771
			public float fadeTime = 1f;

			// Token: 0x04001E5C RID: 7772
			public Color tint = new Color(0.5f, 0.5f, 0.5f, 0.5f);

			// Token: 0x04001E5D RID: 7773
			[Range(0f, 8f)]
			public float exposure = 1f;

			// Token: 0x04001E5E RID: 7774
			[Range(0f, 360f)]
			public float rotation;
		}
	}
}
