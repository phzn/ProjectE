using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000EF8 RID: 3832
	[DisallowMultipleComponent]
	public class NPCSpawnFade : MonoBehaviour
	{
		// Token: 0x060072B9 RID: 29369 RVA: 0x002EAECA File Offset: 0x002E90CA
		private void Awake()
		{
			this.CacheRenderers();
		}

		// Token: 0x060072BA RID: 29370 RVA: 0x002EAED2 File Offset: 0x002E90D2
		private void OnEnable()
		{
			this.CacheRenderers();
			this._phase = NPCSpawnFade.Phase.FadingIn;
			this._timer = 0f;
			this.ApplyDither(0f);
		}

		// Token: 0x060072BB RID: 29371 RVA: 0x002EAEF7 File Offset: 0x002E90F7
		private void OnDisable()
		{
			this._phase = NPCSpawnFade.Phase.Idle;
			this._timer = 0f;
			this._onFadeOutComplete = null;
		}

		// Token: 0x060072BC RID: 29372 RVA: 0x002EAF14 File Offset: 0x002E9114
		private void Update()
		{
			if (this._phase == NPCSpawnFade.Phase.Idle)
			{
				return;
			}
			if (NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				return;
			}
			this._timer += NPCSimulationPause.SimulationDeltaTime;
			if (this._phase == NPCSpawnFade.Phase.FadingIn)
			{
				float num = ((this.fadeInDuration > 0f) ? Mathf.Clamp01(this._timer / this.fadeInDuration) : 1f);
				float num2 = 1f - Mathf.Pow(1f - num, 3f);
				this.ApplyDither(num2);
				if (num >= 1f)
				{
					this._phase = NPCSpawnFade.Phase.Idle;
					this.ApplyDither(1f);
					return;
				}
			}
			else if (this._phase == NPCSpawnFade.Phase.FadingOut)
			{
				float num3 = ((this.fadeOutDuration > 0f) ? Mathf.Clamp01(this._timer / this.fadeOutDuration) : 1f);
				float num4 = num3 * num3;
				this.ApplyDither(1f - num4);
				if (num3 >= 1f)
				{
					this._phase = NPCSpawnFade.Phase.Idle;
					this.ApplyDither(0f);
					Action onFadeOutComplete = this._onFadeOutComplete;
					this._onFadeOutComplete = null;
					if (onFadeOutComplete == null)
					{
						return;
					}
					onFadeOutComplete();
				}
			}
		}

		// Token: 0x060072BD RID: 29373 RVA: 0x002EB02D File Offset: 0x002E922D
		public void StartFadeOut(Action onComplete)
		{
			if (this._phase == NPCSpawnFade.Phase.FadingOut)
			{
				this._onFadeOutComplete = onComplete;
				return;
			}
			this._phase = NPCSpawnFade.Phase.FadingOut;
			this._timer = 0f;
			this._onFadeOutComplete = onComplete;
		}

		// Token: 0x060072BE RID: 29374 RVA: 0x002EAEF7 File Offset: 0x002E90F7
		public void CancelImmediate()
		{
			this._phase = NPCSpawnFade.Phase.Idle;
			this._timer = 0f;
			this._onFadeOutComplete = null;
		}

		// Token: 0x060072BF RID: 29375 RVA: 0x002EB059 File Offset: 0x002E9259
		public void CompleteFadeInImmediately()
		{
			if (this._phase != NPCSpawnFade.Phase.FadingIn)
			{
				return;
			}
			this._phase = NPCSpawnFade.Phase.Idle;
			this._timer = 0f;
			this.ApplyDither(1f);
		}

		// Token: 0x170012B0 RID: 4784
		// (get) Token: 0x060072C0 RID: 29376 RVA: 0x002EB082 File Offset: 0x002E9282
		public bool IsFading
		{
			get
			{
				return this._phase > NPCSpawnFade.Phase.Idle;
			}
		}

		// Token: 0x060072C1 RID: 29377 RVA: 0x002EB08D File Offset: 0x002E928D
		public void RefreshRenderers()
		{
			this._renderersCached = false;
			this.CacheRenderers();
		}

		// Token: 0x060072C2 RID: 29378 RVA: 0x002EB09C File Offset: 0x002E929C
		private void CacheRenderers()
		{
			if (this._mpb == null)
			{
				this._mpb = new MaterialPropertyBlock();
			}
			if (this._renderersCached && this._renderers != null)
			{
				return;
			}
			this._renderers = base.GetComponentsInChildren<Renderer>(true);
			this._renderersCached = true;
		}

		// Token: 0x060072C3 RID: 29379 RVA: 0x002EB0D8 File Offset: 0x002E92D8
		private void ApplyDither(float visibility)
		{
			if (this._renderers == null || this._mpb == null)
			{
				this.CacheRenderers();
			}
			if (this._renderers == null || this._mpb == null)
			{
				return;
			}
			for (int i = 0; i < this._renderers.Length; i++)
			{
				Renderer renderer = this._renderers[i];
				if (!(renderer == null) && (this._ownedDitherRenderers.Contains(renderer) || this.UsesDitherFade(renderer)))
				{
					renderer.GetPropertyBlock(this._mpb);
					this._mpb.SetFloat(NPCSpawnFade.DitherFadeID, visibility);
					renderer.SetPropertyBlock(this._mpb);
					this._ownedDitherRenderers.Add(renderer);
				}
			}
		}

		// Token: 0x060072C4 RID: 29380 RVA: 0x002EB180 File Offset: 0x002E9380
		private bool UsesDitherFade(Renderer renderer)
		{
			renderer.GetSharedMaterials(this._sharedMaterials);
			bool flag = false;
			for (int i = 0; i < this._sharedMaterials.Count; i++)
			{
				Material material = this._sharedMaterials[i];
				if (material != null && material.HasProperty(NPCSpawnFade.DitherFadeID))
				{
					flag = true;
					break;
				}
			}
			this._sharedMaterials.Clear();
			return flag;
		}

		// Token: 0x040075EC RID: 30188
		[Tooltip("Seconds for fade-in (invisible -> visible).")]
		public float fadeInDuration = 0.6f;

		// Token: 0x040075ED RID: 30189
		[Tooltip("Seconds for fade-out (visible -> invisible) before despawn.")]
		public float fadeOutDuration = 0.5f;

		// Token: 0x040075EE RID: 30190
		private static readonly int DitherFadeID = Shader.PropertyToID("_DitherFade");

		// Token: 0x040075EF RID: 30191
		private NPCSpawnFade.Phase _phase;

		// Token: 0x040075F0 RID: 30192
		private float _timer;

		// Token: 0x040075F1 RID: 30193
		private Action _onFadeOutComplete;

		// Token: 0x040075F2 RID: 30194
		private Renderer[] _renderers;

		// Token: 0x040075F3 RID: 30195
		private MaterialPropertyBlock _mpb;

		// Token: 0x040075F4 RID: 30196
		private bool _renderersCached;

		// Token: 0x040075F5 RID: 30197
		private readonly List<Material> _sharedMaterials = new List<Material>(8);

		// Token: 0x040075F6 RID: 30198
		private readonly HashSet<Renderer> _ownedDitherRenderers = new HashSet<Renderer>();

		// Token: 0x02000EF9 RID: 3833
		private enum Phase
		{
			// Token: 0x040075F8 RID: 30200
			Idle,
			// Token: 0x040075F9 RID: 30201
			FadingIn,
			// Token: 0x040075FA RID: 30202
			FadingOut
		}
	}
}
