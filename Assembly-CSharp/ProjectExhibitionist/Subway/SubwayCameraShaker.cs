using System;
using Features.MainMenu;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProjectExhibitionist.Subway
{
	// Token: 0x02000920 RID: 2336
	public class SubwayCameraShaker : MonoBehaviour
	{
		// Token: 0x06003F5E RID: 16222 RVA: 0x001781DC File Offset: 0x001763DC
		public static void SetIntensity(float amplitude)
		{
			if (SubwayCameraShaker._instance == null)
			{
				if (amplitude <= 0f)
				{
					return;
				}
				SubwayCameraShaker._instance = new GameObject("SubwayCameraShaker").AddComponent<SubwayCameraShaker>();
			}
			SubwayCameraShaker._instance._target = Mathf.Max(0f, amplitude);
		}

		// Token: 0x06003F5F RID: 16223 RVA: 0x00178228 File Offset: 0x00176428
		private void Awake()
		{
			this._seed = Random.value * 100f;
			RenderPipelineManager.beginCameraRendering += this.OnBeginCameraRendering;
			RenderPipelineManager.endCameraRendering += this.OnEndCameraRendering;
		}

		// Token: 0x06003F60 RID: 16224 RVA: 0x0017825D File Offset: 0x0017645D
		private void OnDestroy()
		{
			RenderPipelineManager.beginCameraRendering -= this.OnBeginCameraRendering;
			RenderPipelineManager.endCameraRendering -= this.OnEndCameraRendering;
			if (SubwayCameraShaker._instance == this)
			{
				SubwayCameraShaker._instance = null;
			}
		}

		// Token: 0x06003F61 RID: 16225 RVA: 0x00178294 File Offset: 0x00176494
		private void Update()
		{
			this._amp = Mathf.MoveTowards(this._amp, this._target, Time.deltaTime * 0.15f);
		}

		// Token: 0x06003F62 RID: 16226 RVA: 0x001782B8 File Offset: 0x001764B8
		private void OnBeginCameraRendering(ScriptableRenderContext ctx, Camera cam)
		{
			if (!GameSettingsService.CameraShake)
			{
				return;
			}
			if (this._amp <= 0.0001f)
			{
				return;
			}
			if (cam.cameraType != CameraType.Game || cam != Camera.main)
			{
				return;
			}
			float time = Time.time;
			Vector3 vector = new Vector3((Mathf.PerlinNoise(this._seed, time * 2.1f) - 0.5f) * 2f, (Mathf.PerlinNoise(this._seed + 17f, time * 6.3f) - 0.5f) * 2.6f, (Mathf.PerlinNoise(this._seed + 43f, time * 1.7f) - 0.5f) * 1.2f) * this._amp;
			this._shakenCam = cam;
			this._savedPos = cam.transform.position;
			cam.transform.position = this._savedPos + vector;
		}

		// Token: 0x06003F63 RID: 16227 RVA: 0x0017839E File Offset: 0x0017659E
		private void OnEndCameraRendering(ScriptableRenderContext ctx, Camera cam)
		{
			if (cam != this._shakenCam)
			{
				return;
			}
			cam.transform.position = this._savedPos;
			this._shakenCam = null;
		}

		// Token: 0x04003F31 RID: 16177
		private static SubwayCameraShaker _instance;

		// Token: 0x04003F32 RID: 16178
		private float _target;

		// Token: 0x04003F33 RID: 16179
		private float _amp;

		// Token: 0x04003F34 RID: 16180
		private float _seed;

		// Token: 0x04003F35 RID: 16181
		private Camera _shakenCam;

		// Token: 0x04003F36 RID: 16182
		private Vector3 _savedPos;
	}
}
