using System;
using System.Collections.Generic;
using Features.BodyPhysics;
using MagicaCloth2;
using UnityEngine;

namespace Features.GoldBikini
{
	// Token: 0x020011C8 RID: 4552
	public class BreastJiggleImpulse : MonoBehaviour
	{
		// Token: 0x06008BD9 RID: 35801 RVA: 0x0039285C File Offset: 0x00390A5C
		private void Awake()
		{
			Animator componentInParent = base.GetComponentInParent<Animator>();
			this._player = ((componentInParent != null) ? componentInParent.transform : base.transform.root);
			if (this.breastCloths == null || this.breastCloths.Length == 0)
			{
				this.breastCloths = this.ResolveBreastCloths();
			}
		}

		// Token: 0x06008BDA RID: 35802 RVA: 0x003928B0 File Offset: 0x00390AB0
		private MagicaCloth[] ResolveBreastCloths()
		{
			Component component = ((this._player != null) ? this._player : base.transform.root);
			List<MagicaCloth> list = new List<MagicaCloth>();
			foreach (MagicaCloth magicaCloth in component.GetComponentsInChildren<MagicaCloth>(true))
			{
				if (!(magicaCloth == null))
				{
					foreach (string text in this.breastObjectNames)
					{
						if (!string.IsNullOrEmpty(text) && (magicaCloth.gameObject.name == text || magicaCloth.gameObject.name.StartsWith(text)))
						{
							list.Add(magicaCloth);
							break;
						}
					}
				}
			}
			if (list.Count == 0)
			{
				Debug.LogWarning("[BreastJiggleImpulse] No breast MagicaCloth found (looked for " + string.Join("/", this.breastObjectNames) + "). Shake will do nothing.", this);
			}
			return list.ToArray();
		}

		// Token: 0x06008BDB RID: 35803 RVA: 0x00392995 File Offset: 0x00390B95
		public void Shake()
		{
			this.Shake(this.localDirection, this.velocity);
		}

		// Token: 0x06008BDC RID: 35804 RVA: 0x003929AC File Offset: 0x00390BAC
		public void Shake(Vector3 localDir, float strength)
		{
			Debug.Log(string.Format("[BreastJiggleDIAG] Shake() called: cloths={0} dir={1} strength={2}", (this.breastCloths != null) ? this.breastCloths.Length : 0, localDir, strength), this);
			if (localDir.sqrMagnitude < 1E-06f || strength <= 0f)
			{
				return;
			}
			if (AnimeBodyPhysics.TryShake(this._player, localDir, this.opposingVertical, 1f))
			{
				return;
			}
			if (this.breastCloths == null || this.breastCloths.Length == 0)
			{
				Debug.LogWarning("[BreastJiggleDIAG] no cloths resolved!", this);
				return;
			}
			foreach (MagicaCloth magicaCloth in this.breastCloths)
			{
				if (!(magicaCloth == null))
				{
					Vector3 vector = localDir;
					if (this.opposingVertical)
					{
						bool flag = !string.IsNullOrEmpty(this.leftBreastNameContains) && magicaCloth.gameObject.name.Contains(this.leftBreastNameContains);
						float num = Mathf.Abs(localDir.y) * (flag ? 1f : (-1f));
						vector = new Vector3(localDir.x, num, localDir.z);
					}
					Vector3 normalized = ((this._player != null) ? this._player.TransformDirection(vector) : vector).normalized;
					Debug.Log(string.Format("[BreastJiggleDIAG] AddForce on '{0}' type={1} valid={2} worldDir={3}", new object[]
					{
						magicaCloth.gameObject.name,
						magicaCloth.SerializeData.clothType,
						magicaCloth.IsValid(),
						normalized
					}), this);
					magicaCloth.AddForce(normalized, strength, this.forceMode);
				}
			}
		}

		// Token: 0x04008ECD RID: 36557
		[Header("Breast cloth (auto-found by bone name if empty)")]
		[Tooltip("MagicaCloth components driving the breast jiggle (BoobL / BoobR). Auto-resolved at Awake from objects whose name matches one of 'breastObjectNames' if left empty.")]
		[SerializeField]
		private MagicaCloth[] breastCloths;

		// Token: 0x04008ECE RID: 36558
		[Tooltip("GameObject names that own the breast MagicaCloth components.")]
		[SerializeField]
		private string[] breastObjectNames = new string[] { "BoobL", "BoobR" };

		// Token: 0x04008ECF RID: 36559
		[Header("Impulse")]
		[Tooltip("Local direction of the shake relative to the player. A mostly-downward kick with a little forward bias reads best on the stiff BoneSpring breast rig (a pure tiny downward add gets damped instantly and is invisible).")]
		[SerializeField]
		private Vector3 localDirection = new Vector3(0f, -1f, 0.35f);

		// Token: 0x04008ED0 RID: 36560
		[Tooltip("Impulse strength (m/s). BoneSpring is stiff/anchored, so it needs a big kick to be visible — 2.5 was far too small. Tune to taste.")]
		[SerializeField]
		private float velocity = 45f;

		// Token: 0x04008ED1 RID: 36561
		[Tooltip("VelocityAddWithoutDepth ADDS to current velocity (bigger strength = bigger kick) and ignores depth weighting so even the anchored root particles get the full punch.")]
		[SerializeField]
		private ClothForceMode forceMode = ClothForceMode.VelocityAddWithoutDepth;

		// Token: 0x04008ED2 RID: 36562
		[Header("See-saw")]
		[Tooltip("If true, the two breasts get OPPOSITE vertical kicks — the left goes UP while the right goes DOWN (a see-saw wobble) instead of both moving the same way.")]
		[SerializeField]
		private bool opposingVertical = true;

		// Token: 0x04008ED3 RID: 36563
		[Tooltip("Name fragment that identifies the LEFT breast cloth (it gets the UPWARD kick; the other breast gets the downward one). Matched against the cloth's GameObject name.")]
		[SerializeField]
		private string leftBreastNameContains = "BoobL";

		// Token: 0x04008ED4 RID: 36564
		private Transform _player;
	}
}
