using System;
using System.Collections.Generic;
using Features.Detection;
using MagicaCloth2;
using UnityEngine;

namespace Features.PlayerEmotes
{
	// Token: 0x02000E24 RID: 3620
	[DisallowMultipleComponent]
	public sealed class HandstandBreastGravity : MonoBehaviour
	{
		// Token: 0x06006C08 RID: 27656 RVA: 0x002B3A58 File Offset: 0x002B1C58
		private void OnEnable()
		{
			this._emotes = base.GetComponent<GameplayEmoteController>();
			if (this._emotes == null)
			{
				return;
			}
			this._emotes.OnEmoteStarted += this.OnStarted;
			this._emotes.OnEmoteEnded += this.OnEnded;
			if (this._emotes.IsPlaying)
			{
				this.OnStarted(this._emotes.PlayingEmote);
			}
		}

		// Token: 0x06006C09 RID: 27657 RVA: 0x002B3ACC File Offset: 0x002B1CCC
		private void OnDisable()
		{
			if (this._emotes != null)
			{
				this._emotes.OnEmoteStarted -= this.OnStarted;
				this._emotes.OnEmoteEnded -= this.OnEnded;
			}
			this.Restore();
		}

		// Token: 0x06006C0A RID: 27658 RVA: 0x002B3B1C File Offset: 0x002B1D1C
		private void OnStarted(EmoteDefinitionSO emote)
		{
			if (emote == null || emote.EmoteId != "handstand_walk" || this._emotes == null || !this._emotes.IsPlaying || this._emotes.PlayingEmote != emote || this._swaps.Count > 0)
			{
				return;
			}
			MagicaCloth[] componentsInChildren = base.GetComponentsInChildren<MagicaCloth>(true);
			foreach (MagicaCloth magicaCloth in componentsInChildren)
			{
				if ((!(magicaCloth.name != "BoobL") || !(magicaCloth.name != "BoobR")) && magicaCloth.isActiveAndEnabled)
				{
					MagicaCloth magicaCloth2 = null;
					foreach (MagicaCloth magicaCloth3 in componentsInChildren)
					{
						if (magicaCloth3.name == magicaCloth.name + "_HandstandGravity")
						{
							magicaCloth2 = magicaCloth3;
							break;
						}
					}
					if (!(magicaCloth2 == null) && magicaCloth2.SerializeData.clothType == ClothProcess.ClothType.BoneCloth)
					{
						HandstandBreastGravity.EnsureTips(magicaCloth2);
						this._swaps.Add(new HandstandBreastGravity.Swap
						{
							normal = magicaCloth,
							gravity = magicaCloth2,
							wasActive = magicaCloth.gameObject.activeSelf
						});
						MagicaCloth magicaCloth4 = magicaCloth2;
						magicaCloth4.OnBuildComplete = (Action<MagicaCloth, bool>)Delegate.Combine(magicaCloth4.OnBuildComplete, new Action<MagicaCloth, bool>(this.OnBuildComplete));
						magicaCloth.gameObject.SetActive(false);
						magicaCloth2.gameObject.SetActive(true);
						magicaCloth2.ResetCloth(false);
					}
				}
			}
			this._buildStartedAt = Time.unscaledTime;
		}

		// Token: 0x06006C0B RID: 27659 RVA: 0x002B3CBB File Offset: 0x002B1EBB
		private void OnBuildComplete(MagicaCloth cloth, bool success)
		{
			if (success)
			{
				return;
			}
			Debug.LogWarning("[HandstandBreastGravity] Gravity cloth failed to build; restoring normal physics.", this);
			this.Restore();
		}

		// Token: 0x06006C0C RID: 27660 RVA: 0x002B3CD2 File Offset: 0x002B1ED2
		private void OnEnded(EmoteDefinitionSO emote)
		{
			if (emote != null && emote.EmoteId == "handstand_walk")
			{
				this.Restore();
			}
		}

		// Token: 0x06006C0D RID: 27661 RVA: 0x002B3CF8 File Offset: 0x002B1EF8
		private void LateUpdate()
		{
			if (this._swaps.Count > 0 && (this._emotes == null || !this._emotes.isActiveAndEnabled || !this._emotes.IsPlaying || this._emotes.PlayingEmote == null || this._emotes.PlayingEmote.EmoteId != "handstand_walk"))
			{
				this.Restore();
			}
			if (this._swaps.Count > 0 && Time.unscaledTime - this._buildStartedAt > 5f)
			{
				foreach (HandstandBreastGravity.Swap swap in this._swaps)
				{
					if (swap.gravity == null || !swap.gravity.IsValid())
					{
						this.Restore();
						break;
					}
				}
			}
		}

		// Token: 0x06006C0E RID: 27662 RVA: 0x002B3DF4 File Offset: 0x002B1FF4
		private void Restore()
		{
			foreach (HandstandBreastGravity.Swap swap in this._swaps)
			{
				if (swap.gravity != null)
				{
					MagicaCloth gravity = swap.gravity;
					gravity.OnBuildComplete = (Action<MagicaCloth, bool>)Delegate.Remove(gravity.OnBuildComplete, new Action<MagicaCloth, bool>(this.OnBuildComplete));
					swap.gravity.gameObject.SetActive(false);
				}
				if (!(swap.normal == null))
				{
					swap.normal.gameObject.SetActive(swap.wasActive);
					if (swap.wasActive)
					{
						swap.normal.ResetCloth(false);
					}
				}
			}
			this._swaps.Clear();
		}

		// Token: 0x06006C0F RID: 27663 RVA: 0x002B3ED0 File Offset: 0x002B20D0
		private static void EnsureTips(MagicaCloth cloth)
		{
			foreach (Transform transform in cloth.SerializeData.rootBones)
			{
				if (!(transform == null) && transform.childCount <= 0)
				{
					Vector3 vector = ((transform.parent != null) ? (transform.position - transform.parent.position) : transform.forward);
					if (vector.sqrMagnitude < 1E-06f)
					{
						vector = transform.forward;
					}
					Transform transform2 = new GameObject(transform.name + "_GravityTip").transform;
					transform2.SetParent(transform, false);
					transform2.position = transform.position + vector.normalized * 0.09f;
				}
			}
		}

		// Token: 0x04006E8C RID: 28300
		public const string VariantSuffix = "_HandstandGravity";

		// Token: 0x04006E8D RID: 28301
		private GameplayEmoteController _emotes;

		// Token: 0x04006E8E RID: 28302
		private readonly List<HandstandBreastGravity.Swap> _swaps = new List<HandstandBreastGravity.Swap>();

		// Token: 0x04006E8F RID: 28303
		private float _buildStartedAt;

		// Token: 0x02000E25 RID: 3621
		private sealed class Swap
		{
			// Token: 0x04006E90 RID: 28304
			public MagicaCloth normal;

			// Token: 0x04006E91 RID: 28305
			public MagicaCloth gravity;

			// Token: 0x04006E92 RID: 28306
			public bool wasActive;
		}
	}
}
