using System;
using Features.Inventory.Wearable;
using Features.PubicHairMask;
using MagicaCloth2;
using UnityEngine;

namespace Features.SpicyHatch
{
	// Token: 0x02000C53 RID: 3155
	public class SpicyHatchWind : MonoBehaviour
	{
		// Token: 0x17000EF8 RID: 3832
		// (get) Token: 0x06005B28 RID: 23336 RVA: 0x0023B0D8 File Offset: 0x002392D8
		// (set) Token: 0x06005B29 RID: 23337 RVA: 0x0023B0DF File Offset: 0x002392DF
		public static float LastWindTime { get; private set; } = -1f;

		// Token: 0x06005B2A RID: 23338 RVA: 0x0023B0E7 File Offset: 0x002392E7
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			SpicyHatchWind.LastWindTime = -1f;
		}

		// Token: 0x06005B2B RID: 23339 RVA: 0x0023B0F4 File Offset: 0x002392F4
		private void Awake()
		{
			if (this.windZone != null)
			{
				this.windZone.gameObject.SetActive(false);
			}
			if (this.windSfx != null)
			{
				this.windSfx.playOnAwake = false;
				this.windSfx.loop = true;
				this.windSfx.volume = 0f;
			}
		}

		// Token: 0x06005B2C RID: 23340 RVA: 0x0023B158 File Offset: 0x00239358
		private void Update()
		{
			if (this._playerInside)
			{
				this.Refresh();
			}
			bool flag = this.windZone != null && this.windZone.gameObject.activeSelf;
			if (flag)
			{
				SpicyHatchWind.LastWindTime = Time.time;
			}
			if (this.embFaceDuration > 0f && flag)
			{
				SpicyHatchEmbarrassment.NotifySkirtLifted(this.embFaceDuration);
			}
			if (flag)
			{
				PubicHairStencilMask.NotifyWindLift();
			}
			this.UpdateWindSfx(flag);
		}

		// Token: 0x06005B2D RID: 23341 RVA: 0x0023B1D0 File Offset: 0x002393D0
		private void UpdateWindSfx(bool windOn)
		{
			if (this.windSfx == null)
			{
				return;
			}
			float num = (windOn ? this.sfxVolume : 0f);
			if (windOn && !this.windSfx.isPlaying)
			{
				this.windSfx.Play();
			}
			float num2 = (windOn ? this.sfxFadeIn : this.sfxFadeOut);
			float num3 = ((num2 > 0.01f) ? (this.sfxVolume / num2 * Time.deltaTime) : 1f);
			this.windSfx.volume = Mathf.MoveTowards(this.windSfx.volume, num, num3);
			if (!windOn && this.windSfx.isPlaying && this.windSfx.volume <= 0.001f)
			{
				this.windSfx.Stop();
			}
		}

		// Token: 0x06005B2E RID: 23342 RVA: 0x0023B292 File Offset: 0x00239492
		private void OnDisable()
		{
			this._playerInside = false;
			this.Unsubscribe();
			this.Refresh();
		}

		// Token: 0x06005B2F RID: 23343 RVA: 0x0023B2A8 File Offset: 0x002394A8
		private void OnTriggerEnter(Collider other)
		{
			if (this._playerInside || !other.CompareTag(this.playerTag))
			{
				return;
			}
			this._playerInside = true;
			if (this._wear == null)
			{
				this._wear = other.GetComponentInParent<WearableUseController>();
				if (this._wear == null)
				{
					Animator componentInParent = other.GetComponentInParent<Animator>();
					Transform transform = ((componentInParent != null) ? componentInParent.transform : other.transform.root);
					this._wear = transform.GetComponentInChildren<WearableUseController>(true);
				}
			}
			this.Subscribe();
			this.Refresh();
		}

		// Token: 0x06005B30 RID: 23344 RVA: 0x0023B338 File Offset: 0x00239538
		private void OnTriggerExit(Collider other)
		{
			if (!other.CompareTag(this.playerTag))
			{
				return;
			}
			this._playerInside = false;
			this.Unsubscribe();
			this.Refresh();
		}

		// Token: 0x06005B31 RID: 23345 RVA: 0x0023B35C File Offset: 0x0023955C
		private void Subscribe()
		{
			if (this._wear == null)
			{
				return;
			}
			this.Unsubscribe();
			this._wear.OnItemWorn += this.HandleWornChanged;
			this._wear.OnItemUnworn += this.HandleWornChanged;
		}

		// Token: 0x06005B32 RID: 23346 RVA: 0x0023B3AC File Offset: 0x002395AC
		private void Unsubscribe()
		{
			if (this._wear == null)
			{
				return;
			}
			this._wear.OnItemWorn -= this.HandleWornChanged;
			this._wear.OnItemUnworn -= this.HandleWornChanged;
		}

		// Token: 0x06005B33 RID: 23347 RVA: 0x0023B3EB File Offset: 0x002395EB
		private void HandleWornChanged(WearableItemSO item)
		{
			this.Refresh();
		}

		// Token: 0x06005B34 RID: 23348 RVA: 0x0023B3F4 File Offset: 0x002395F4
		private void Refresh()
		{
			if (this.windZone == null)
			{
				return;
			}
			bool flag = this._playerInside && this._wear != null && this._wear.HasVisibleSkirt();
			if (this.windZone.gameObject.activeSelf != flag)
			{
				this.windZone.gameObject.SetActive(flag);
				if (this.windParticles != null)
				{
					if (flag)
					{
						this.windParticles.Play();
					}
					else
					{
						this.windParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
					}
				}
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[SpicyHatch] Wind {0} (inside={1})", flag ? "ON" : "OFF", this._playerInside));
				}
			}
		}

		// Token: 0x04005C57 RID: 23639
		[Header("Wind")]
		[Tooltip("MagicaWindZone figlia (direzione verso l'alto). Spenta di default, accesa solo col player sopra.")]
		[SerializeField]
		private MagicaWindZone windZone;

		// Token: 0x04005C58 RID: 23640
		[Header("Player")]
		[SerializeField]
		private string playerTag = "Player";

		// Token: 0x04005C59 RID: 23641
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04005C5A RID: 23642
		[Header("Reaction")]
		[Tooltip("Secondi di 'Emb Face Supreme' dopo che il vento le alza la gonna (il timer si estende finché resta sopra). 0 = niente reazione.")]
		[SerializeField]
		private float embFaceDuration = 25f;

		// Token: 0x04005C5B RID: 23643
		[Header("Audio")]
		[Tooltip("AudioSource del rumore vento su un child SEMPRE attivo (non sulla wind zone, che viene spenta/accesa). Fade gestito qui.")]
		[SerializeField]
		private AudioSource windSfx;

		// Token: 0x04005C5C RID: 23644
		[Tooltip("Volume a regime del vento.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float sfxVolume = 0.2f;

		// Token: 0x04005C5D RID: 23645
		[Tooltip("Secondi di fade-in del rumore quando il vento parte.")]
		[SerializeField]
		private float sfxFadeIn = 1.2f;

		// Token: 0x04005C5E RID: 23646
		[Tooltip("Secondi di fade-out del rumore quando il vento si ferma.")]
		[SerializeField]
		private float sfxFadeOut = 1.8f;

		// Token: 0x04005C5F RID: 23647
		[Header("VFX")]
		[Tooltip("ParticleSystem dei puff di vento (child sempre attivo, playOnAwake OFF): Play all'attivazione, StopEmitting allo stop così i puff in volo svaniscono da soli.")]
		[SerializeField]
		private ParticleSystem windParticles;

		// Token: 0x04005C60 RID: 23648
		private WearableUseController _wear;

		// Token: 0x04005C61 RID: 23649
		private bool _playerInside;
	}
}
