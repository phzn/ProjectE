using System;
using System.Collections;
using System.Collections.Generic;
using Features.Detection;
using Features.GoldBikini;
using UnityEngine;

namespace Features.Bush
{
	// Token: 0x020014D0 RID: 5328
	[DisallowMultipleComponent]
	public class BushConcealment : MonoBehaviour
	{
		// Token: 0x0600A3D6 RID: 41942 RVA: 0x004417ED File Offset: 0x0043F9ED
		internal Object ReadUniversityGameplayReference(UniversityGameplayField field)
		{
			if (field == UniversityGameplayField.BushConcealment_enterSound)
			{
				return this.enterSound;
			}
			throw new InvalidOperationException("Unsupported BushConcealment gameplay binding: " + field.ToString());
		}

		// Token: 0x0600A3D7 RID: 41943 RVA: 0x00441816 File Offset: 0x0043FA16
		internal void WriteUniversityGameplayReference(UniversityGameplayField field, Object asset)
		{
			if (field == UniversityGameplayField.BushConcealment_enterSound)
			{
				this.enterSound = (AudioClip)asset;
				return;
			}
			throw new InvalidOperationException("Unsupported BushConcealment gameplay binding: " + field.ToString());
		}

		// Token: 0x0600A3D8 RID: 41944 RVA: 0x00441845 File Offset: 0x0043FA45
		public void ConfigureFoliageVolume(float bodyCoverage, AudioClip rustle)
		{
			this.coverage = Mathf.Clamp(bodyCoverage, 0f, 0.95f);
			this.enterSound = rustle;
			this.fadeFoliage = false;
			this.slipChanceOnExit = 0f;
		}

		// Token: 0x0600A3D9 RID: 41945 RVA: 0x00441878 File Offset: 0x0043FA78
		private void Awake()
		{
			if (this.fadeFoliage && this.bushRenderer == null)
			{
				this.bushRenderer = base.GetComponentInChildren<Renderer>();
			}
			if (this.fadeFoliage && this.bushRenderer != null)
			{
				this._mat = this.bushRenderer.material;
			}
			this._opaqueAlpha = this.ReadAlpha();
			if (this.enterSound != null && this.enterSound.loadState == AudioDataLoadState.Unloaded)
			{
				this.enterSound.LoadAudioData();
			}
		}

		// Token: 0x0600A3DA RID: 41946 RVA: 0x00441904 File Offset: 0x0043FB04
		private void OnTriggerEnter(Collider other)
		{
			if (!this.IsPlayerCollider(other) || !this._playerColliders.Add(other))
			{
				return;
			}
			if (this._playerInside)
			{
				return;
			}
			this._playerInside = true;
			PlayerStealthBridge.SetConcealmentSource(this, this.coverage);
			if (this.enterSound != null)
			{
				if (this.audioSource == null)
				{
					this.audioSource = base.gameObject.AddComponent<AudioSource>();
					this.audioSource.playOnAwake = false;
					this.audioSource.spatialBlend = 1f;
					this.audioSource.maxDistance = 12f;
				}
				this.audioSource.PlayOneShot(this.enterSound, this.enterVolume);
			}
			if (this.bikiniSlip == null)
			{
				this.bikiniSlip = other.GetComponentInParent<BikiniSlipDiveController>() ?? other.GetComponentInChildren<BikiniSlipDiveController>();
			}
			this.StartFade(this.concealedOpacity);
		}

		// Token: 0x0600A3DB RID: 41947 RVA: 0x004419E5 File Offset: 0x0043FBE5
		private void OnTriggerExit(Collider other)
		{
			if (!this._playerColliders.Remove(other) || this._playerColliders.Count > 0)
			{
				return;
			}
			this.LeaveBush(true);
		}

		// Token: 0x0600A3DC RID: 41948 RVA: 0x00441A0B File Offset: 0x0043FC0B
		private void OnTriggerStay(Collider other)
		{
			if (!this._playerColliders.Contains(other))
			{
				this.OnTriggerEnter(other);
			}
		}

		// Token: 0x0600A3DD RID: 41949 RVA: 0x00441A24 File Offset: 0x0043FC24
		private bool IsPlayerCollider(Collider other)
		{
			if (other == null || other.isTrigger)
			{
				return false;
			}
			Transform transform = other.transform;
			while (transform != null)
			{
				if (transform.CompareTag(this.playerTag))
				{
					return true;
				}
				transform = transform.parent;
			}
			return false;
		}

		// Token: 0x0600A3DE RID: 41950 RVA: 0x00441A6E File Offset: 0x0043FC6E
		private void FixedUpdate()
		{
			if (!this._playerInside)
			{
				return;
			}
			this._playerColliders.RemoveWhere(new Predicate<Collider>(BushConcealment.IsInactiveCollider));
			if (this._playerColliders.Count == 0)
			{
				this.LeaveBush(false);
			}
		}

		// Token: 0x0600A3DF RID: 41951 RVA: 0x00441AA5 File Offset: 0x0043FCA5
		private static bool IsInactiveCollider(Collider collider)
		{
			return collider == null || !collider.enabled || !collider.gameObject.activeInHierarchy;
		}

		// Token: 0x0600A3E0 RID: 41952 RVA: 0x00441AC8 File Offset: 0x0043FCC8
		private void LeaveBush(bool allowSlip)
		{
			if (!this._playerInside)
			{
				return;
			}
			this._playerInside = false;
			PlayerStealthBridge.SetConcealmentSource(this, 0f);
			this.StartFade(this._opaqueAlpha);
			if (allowSlip && this.slipChanceOnExit > 0f && this.bikiniSlip != null)
			{
				this.bikiniSlip.TrySlipCoveredPieces(this.slipChanceOnExit, this.maxSlipPieces);
			}
		}

		// Token: 0x0600A3E1 RID: 41953 RVA: 0x00441B32 File Offset: 0x0043FD32
		private void StartFade(float targetAlpha)
		{
			if (this._mat == null)
			{
				return;
			}
			if (this._fade != null)
			{
				base.StopCoroutine(this._fade);
			}
			this._fade = base.StartCoroutine(this.FadeAlpha(targetAlpha));
		}

		// Token: 0x0600A3E2 RID: 41954 RVA: 0x00441B6A File Offset: 0x0043FD6A
		private IEnumerator FadeAlpha(float target)
		{
			float start = this.ReadAlpha();
			if (this.fadeDuration <= 0f)
			{
				this.WriteAlpha(target);
				this._fade = null;
				yield break;
			}
			float t = 0f;
			while (t < this.fadeDuration)
			{
				t += Time.deltaTime;
				this.WriteAlpha(Mathf.Lerp(start, target, t / this.fadeDuration));
				yield return null;
			}
			this.WriteAlpha(target);
			this._fade = null;
			yield break;
		}

		// Token: 0x0600A3E3 RID: 41955 RVA: 0x00441B80 File Offset: 0x0043FD80
		private float ReadAlpha()
		{
			if (this._mat == null)
			{
				return 1f;
			}
			if (this._mat.HasProperty(BushConcealment.ColorId))
			{
				return this._mat.GetColor(BushConcealment.ColorId).a;
			}
			if (this._mat.HasProperty(BushConcealment.BaseColorId))
			{
				return this._mat.GetColor(BushConcealment.BaseColorId).a;
			}
			if (this._mat.HasProperty(BushConcealment.TransparencyId))
			{
				return this._mat.GetFloat(BushConcealment.TransparencyId);
			}
			return 1f;
		}

		// Token: 0x0600A3E4 RID: 41956 RVA: 0x00441C1C File Offset: 0x0043FE1C
		private void WriteAlpha(float a)
		{
			if (this._mat == null)
			{
				return;
			}
			if (this._mat.HasProperty(BushConcealment.ColorId))
			{
				Color color = this._mat.GetColor(BushConcealment.ColorId);
				color.a = a;
				this._mat.SetColor(BushConcealment.ColorId, color);
			}
			if (this._mat.HasProperty(BushConcealment.BaseColorId))
			{
				Color color2 = this._mat.GetColor(BushConcealment.BaseColorId);
				color2.a = a;
				this._mat.SetColor(BushConcealment.BaseColorId, color2);
			}
			if (this._mat.HasProperty(BushConcealment.TransparencyId))
			{
				this._mat.SetFloat(BushConcealment.TransparencyId, a);
			}
		}

		// Token: 0x0600A3E5 RID: 41957 RVA: 0x00441CD3 File Offset: 0x0043FED3
		private void OnDestroy()
		{
			PlayerStealthBridge.SetConcealmentSource(this, 0f);
			if (this._mat != null)
			{
				Object.Destroy(this._mat);
			}
		}

		// Token: 0x0600A3E6 RID: 41958 RVA: 0x00441CFC File Offset: 0x0043FEFC
		private void OnDisable()
		{
			this._playerInside = false;
			this._playerColliders.Clear();
			PlayerStealthBridge.SetConcealmentSource(this, 0f);
			if (this._fade != null)
			{
				base.StopCoroutine(this._fade);
			}
			this._fade = null;
			this.WriteAlpha(this._opaqueAlpha);
		}

		// Token: 0x0400A7B2 RID: 42930
		[Header("Player")]
		[Tooltip("Tag that identifies the player entering the bush.")]
		[SerializeField]
		private string playerTag = "Player";

		// Token: 0x0400A7B3 RID: 42931
		[Header("Concealment")]
		[Tooltip("Visual detection reduction while inside. Foliage provides cover, not complete invisibility.")]
		[SerializeField]
		[Range(0f, 0.95f)]
		private float coverage = 0.8f;

		// Token: 0x0400A7B4 RID: 42932
		[Header("Renderer (auto-found)")]
		[Tooltip("The bush mesh renderer whose single material fades. Auto-found in children if empty.")]
		[SerializeField]
		private Renderer bushRenderer;

		// Token: 0x0400A7B5 RID: 42933
		[Header("Transparency")]
		[Tooltip("Disable for map foliage using shared/combined materials; no material instance is then created.")]
		[SerializeField]
		private bool fadeFoliage = true;

		// Token: 0x0400A7B6 RID: 42934
		[Tooltip("Opacity (0..1) the bush fades TO while the player is inside (lower = more see-through).")]
		[SerializeField]
		[Range(0f, 1f)]
		private float concealedOpacity = 0.35f;

		// Token: 0x0400A7B7 RID: 42935
		[Tooltip("Seconds to fade the opacity in/out.")]
		[SerializeField]
		private float fadeDuration = 0.3f;

		// Token: 0x0400A7B8 RID: 42936
		[Header("Audio")]
		[Tooltip("Rustle one-shot played when the player enters the bush.")]
		[SerializeField]
		private AudioClip enterSound;

		// Token: 0x0400A7B9 RID: 42937
		[SerializeField]
		[Range(0f, 1f)]
		private float enterVolume = 1f;

		// Token: 0x0400A7BA RID: 42938
		[Tooltip("Persistent AudioSource (auto-created on this GameObject if empty).")]
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x0400A7BB RID: 42939
		[Header("Bikini slip on exit")]
		[Tooltip("Probability (0..1) that leaving the bush slips one or more still-covered bikini pieces.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float slipChanceOnExit = 0.4f;

		// Token: 0x0400A7BC RID: 42940
		[Tooltip("Maximum number of pieces that can slip at once on a successful roll.")]
		[SerializeField]
		private int maxSlipPieces = 2;

		// Token: 0x0400A7BD RID: 42941
		[Tooltip("Bikini slip controller on the player. Auto-found from the entering player if empty.")]
		[SerializeField]
		private BikiniSlipDiveController bikiniSlip;

		// Token: 0x0400A7BE RID: 42942
		private static readonly int ColorId = Shader.PropertyToID("_Color");

		// Token: 0x0400A7BF RID: 42943
		private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

		// Token: 0x0400A7C0 RID: 42944
		private static readonly int TransparencyId = Shader.PropertyToID("_Transparency");

		// Token: 0x0400A7C1 RID: 42945
		private Material _mat;

		// Token: 0x0400A7C2 RID: 42946
		private float _opaqueAlpha = 1f;

		// Token: 0x0400A7C3 RID: 42947
		private Coroutine _fade;

		// Token: 0x0400A7C4 RID: 42948
		private bool _playerInside;

		// Token: 0x0400A7C5 RID: 42949
		private readonly HashSet<Collider> _playerColliders = new HashSet<Collider>();
	}
}
