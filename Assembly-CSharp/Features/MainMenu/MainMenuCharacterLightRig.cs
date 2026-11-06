using System;
using UnityEngine;

namespace Features.MainMenu
{
	// Token: 0x02001048 RID: 4168
	[DisallowMultipleComponent]
	public sealed class MainMenuCharacterLightRig : MonoBehaviour
	{
		// Token: 0x06007E9B RID: 32411 RVA: 0x0033C50C File Offset: 0x0033A70C
		public void Initialize(Animator characterAnimator)
		{
			this._characterAnimator = characterAnimator;
			if (this._rig != null)
			{
				return;
			}
			this.ResolveCharacter();
			if (this._characterAnimator == null)
			{
				Debug.LogWarning("[MainMenu] Character portrait lights not created: character missing.");
				return;
			}
			Transform transform = (this._characterAnimator.isHuman ? this._characterAnimator.GetBoneTransform(HumanBodyBones.Chest) : null);
			Vector3 vector = ((transform != null) ? transform.position : (this._characterAnimator.transform.position + Vector3.up * 1.35f));
			GameObject gameObject = new GameObject("Menu Character Portrait Lights");
			gameObject.transform.SetParent(base.transform, false);
			this._rig = gameObject.transform;
			this.CreateDirectional("Portrait Key", vector + new Vector3(-1.35f, 1.1f, 2.8f), vector, this.keyColor, this.keyIntensity);
			this.CreateDirectional("Portrait Fill", vector + new Vector3(1.65f, 0.45f, 2.2f), vector, this.fillColor, this.fillIntensity);
		}

		// Token: 0x06007E9C RID: 32412 RVA: 0x0033C630 File Offset: 0x0033A830
		private void ResolveCharacter()
		{
			if (this._characterAnimator != null)
			{
				return;
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if (gameObject != null)
			{
				this._characterAnimator = gameObject.GetComponentInChildren<Animator>(true);
			}
		}

		// Token: 0x06007E9D RID: 32413 RVA: 0x0033C670 File Offset: 0x0033A870
		private void CreateDirectional(string lightName, Vector3 position, Vector3 target, Color color, float intensity)
		{
			GameObject gameObject = new GameObject(lightName, new Type[] { typeof(Light) });
			gameObject.transform.SetParent(this._rig, true);
			gameObject.transform.position = position;
			gameObject.transform.rotation = Quaternion.LookRotation(target - position, Vector3.up);
			Light component = gameObject.GetComponent<Light>();
			component.type = LightType.Directional;
			component.color = color;
			component.intensity = intensity;
			component.shadows = LightShadows.None;
			component.renderMode = LightRenderMode.ForcePixel;
		}

		// Token: 0x04008230 RID: 33328
		private const string RigName = "Menu Character Portrait Lights";

		// Token: 0x04008231 RID: 33329
		[SerializeField]
		[Min(0f)]
		private float keyIntensity = 1.45f;

		// Token: 0x04008232 RID: 33330
		[SerializeField]
		[Min(0f)]
		private float fillIntensity = 0.55f;

		// Token: 0x04008233 RID: 33331
		[SerializeField]
		private Color keyColor = new Color(1f, 0.86f, 0.91f, 1f);

		// Token: 0x04008234 RID: 33332
		[SerializeField]
		private Color fillColor = new Color(0.72f, 0.79f, 1f, 1f);

		// Token: 0x04008235 RID: 33333
		private Animator _characterAnimator;

		// Token: 0x04008236 RID: 33334
		private Transform _rig;
	}
}
