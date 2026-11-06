using System;
using System.Collections;
using System.Collections.Generic;
using Features.UI.TimedEffects;
using UnityEngine;

namespace ProjectExhibitionist
{
	// Token: 0x020008EB RID: 2283
	public class ShowerBubbleSpawner : MonoBehaviour
	{
		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x06003DA8 RID: 15784 RVA: 0x0016EBF0 File Offset: 0x0016CDF0
		public bool BubblesActive
		{
			get
			{
				return this._spawned.Count > 0;
			}
		}

		// Token: 0x1400006D RID: 109
		// (add) Token: 0x06003DA9 RID: 15785 RVA: 0x0016EC00 File Offset: 0x0016CE00
		// (remove) Token: 0x06003DAA RID: 15786 RVA: 0x0016EC38 File Offset: 0x0016CE38
		public event Action OnBubblesGone;

		// Token: 0x06003DAB RID: 15787 RVA: 0x0016EC70 File Offset: 0x0016CE70
		public void ShowBubbles()
		{
			if (this._spawned.Count > 0)
			{
				return;
			}
			if (this.setup == null)
			{
				Debug.LogError("[ShowerBubbleSpawner] No BubbleBoneSetup assigned.", this);
				return;
			}
			Transform transform = this.ResolveSkeletonRoot();
			if (transform == null)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"[ShowerBubbleSpawner] Could not resolve skeleton root ('",
					this.setup.skeletonRootPath,
					"') under '",
					base.name,
					"'."
				}), this);
				return;
			}
			foreach (BubbleBoneSetup.BubblePlacement bubblePlacement in this.setup.placements)
			{
				if (bubblePlacement != null && !(bubblePlacement.prefab == null))
				{
					Transform transform2 = (string.IsNullOrEmpty(bubblePlacement.bonePath) ? transform : transform.Find(bubblePlacement.bonePath));
					if (transform2 == null)
					{
						Debug.LogWarning(string.Concat(new string[] { "[ShowerBubbleSpawner] Bone '", bubblePlacement.bonePath, "' not found for bubble '", bubblePlacement.sourceName, "'. Skipping." }), this);
					}
					else
					{
						GameObject gameObject = Object.Instantiate<GameObject>(bubblePlacement.prefab, transform2);
						gameObject.transform.localPosition = bubblePlacement.localPosition;
						gameObject.transform.localEulerAngles = bubblePlacement.localEulerAngles;
						gameObject.transform.localScale = bubblePlacement.localScale;
						gameObject.name = (string.IsNullOrEmpty(bubblePlacement.sourceName) ? bubblePlacement.prefab.name : bubblePlacement.sourceName);
						this._spawned.Add(gameObject);
						this._spawnedTransforms.Add(gameObject.transform);
						this._targetScales.Add(bubblePlacement.localScale);
					}
				}
			}
			Debug.Log(string.Format("[ShowerBubbles] Spawned {0} bubble(s).", this._spawned.Count), this);
			if (this._spawned.Count == 0)
			{
				return;
			}
			if (this.growInDuration > 0f)
			{
				this._growing = true;
				this._growTimer = 0f;
				this.ApplyGrowScale(0f);
			}
		}

		// Token: 0x06003DAC RID: 15788 RVA: 0x0016EEC0 File Offset: 0x0016D0C0
		public void BeginDissolve()
		{
			if (this._spawned.Count == 0)
			{
				return;
			}
			if (this._lifetimeRoutine != null)
			{
				return;
			}
			this.RegisterHUDTimer();
			this._lifetimeRoutine = base.StartCoroutine(this.LifetimeRoutine());
		}

		// Token: 0x06003DAD RID: 15789 RVA: 0x0016EEF4 File Offset: 0x0016D0F4
		public void ResetToFull()
		{
			if (this._lifetimeRoutine != null)
			{
				base.StopCoroutine(this._lifetimeRoutine);
				this._lifetimeRoutine = null;
			}
			this.UnregisterHUDTimer();
			if (this._spawned.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this._spawnedTransforms.Count; i++)
			{
				if (this._spawnedTransforms[i] != null)
				{
					this._spawnedTransforms[i].localScale = this._targetScales[i];
				}
			}
		}

		// Token: 0x06003DAE RID: 15790 RVA: 0x0016EF78 File Offset: 0x0016D178
		public void HideBubbles()
		{
			bool flag = this._spawned.Count > 0;
			this._growing = false;
			if (this._lifetimeRoutine != null)
			{
				base.StopCoroutine(this._lifetimeRoutine);
				this._lifetimeRoutine = null;
			}
			for (int i = 0; i < this._spawned.Count; i++)
			{
				if (this._spawned[i] != null)
				{
					Object.Destroy(this._spawned[i]);
				}
			}
			this._spawned.Clear();
			this._spawnedTransforms.Clear();
			this._targetScales.Clear();
			this.UnregisterHUDTimer();
			if (flag)
			{
				Action onBubblesGone = this.OnBubblesGone;
				if (onBubblesGone == null)
				{
					return;
				}
				onBubblesGone();
			}
		}

		// Token: 0x06003DAF RID: 15791 RVA: 0x0016F02C File Offset: 0x0016D22C
		private void Update()
		{
			if (!this._growing)
			{
				return;
			}
			this._growTimer += Time.deltaTime;
			float num = Mathf.Clamp01(this._growTimer / this.growInDuration);
			this.ApplyGrowScale(num);
			if (num >= 1f)
			{
				this._growing = false;
			}
		}

		// Token: 0x06003DB0 RID: 15792 RVA: 0x0016F080 File Offset: 0x0016D280
		private void ApplyGrowScale(float t)
		{
			float num = t * t * (3f - 2f * t);
			for (int i = 0; i < this._spawnedTransforms.Count; i++)
			{
				if (this._spawnedTransforms[i] != null)
				{
					this._spawnedTransforms[i].localScale = this._targetScales[i] * num;
				}
			}
		}

		// Token: 0x06003DB1 RID: 15793 RVA: 0x0016F0EC File Offset: 0x0016D2EC
		private IEnumerator LifetimeRoutine()
		{
			while (this._growing)
			{
				yield return null;
			}
			float life = Mathf.Max(0.01f, this.lifetimeSeconds);
			float elapsed = 0f;
			while (elapsed < life)
			{
				elapsed += Time.deltaTime;
				float num = Mathf.Clamp01(elapsed / life);
				float num2 = Mathf.Clamp01(this.shrinkCurve.Evaluate(num));
				float num3 = Mathf.Lerp(this.endScaleFraction, 1f, num2);
				for (int i = 0; i < this._spawnedTransforms.Count; i++)
				{
					if (this._spawnedTransforms[i] != null)
					{
						this._spawnedTransforms[i].localScale = this._targetScales[i] * num3;
					}
				}
				if (this.showHUDTimer && TimedEffectHUDService.Instance != null)
				{
					TimedEffectHUDService.Instance.SetNormalizedProgress(this.hudTimerData.EffectId, 1f - num);
				}
				yield return null;
			}
			this._lifetimeRoutine = null;
			this.HideBubbles();
			yield break;
		}

		// Token: 0x06003DB2 RID: 15794 RVA: 0x0016F0FC File Offset: 0x0016D2FC
		private void RegisterHUDTimer()
		{
			if (!this.showHUDTimer || TimedEffectHUDService.Instance == null)
			{
				return;
			}
			this.hudTimerData.TotalDuration = Mathf.Max(0.01f, this.lifetimeSeconds);
			TimedEffectHUDService.Instance.RegisterEffect(this.hudTimerData);
		}

		// Token: 0x06003DB3 RID: 15795 RVA: 0x0016F14A File Offset: 0x0016D34A
		private void UnregisterHUDTimer()
		{
			if (!this.showHUDTimer || TimedEffectHUDService.Instance == null)
			{
				return;
			}
			TimedEffectHUDService.Instance.UnregisterEffect(this.hudTimerData.EffectId);
		}

		// Token: 0x06003DB4 RID: 15796 RVA: 0x0016F178 File Offset: 0x0016D378
		private Transform ResolveSkeletonRoot()
		{
			if (this.skeletonRootOverride != null)
			{
				return this.skeletonRootOverride;
			}
			Transform transform = this.ResolveCharacterRoot();
			if (this.setup == null || string.IsNullOrEmpty(this.setup.skeletonRootPath))
			{
				return transform;
			}
			Transform transform2 = transform.Find(this.setup.skeletonRootPath);
			if (transform2 == null)
			{
				transform2 = base.transform.Find(this.setup.skeletonRootPath);
			}
			return transform2;
		}

		// Token: 0x06003DB5 RID: 15797 RVA: 0x0016F1F8 File Offset: 0x0016D3F8
		private Transform ResolveCharacterRoot()
		{
			Transform transform = base.transform;
			while (transform != null)
			{
				if (transform.CompareTag("Player"))
				{
					return transform;
				}
				transform = transform.parent;
			}
			return base.transform.root;
		}

		// Token: 0x06003DB6 RID: 15798 RVA: 0x0016F238 File Offset: 0x0016D438
		private void OnDisable()
		{
			this.HideBubbles();
		}

		// Token: 0x04003DC9 RID: 15817
		[Header("Setup")]
		[Tooltip("Recipe describing which bubble prefabs go on which bones (captured via Tools/Shower).")]
		[SerializeField]
		private BubbleBoneSetup setup;

		// Token: 0x04003DCA RID: 15818
		[Tooltip("Optional override for the skeleton root. If null, resolved from setup.skeletonRootPath.")]
		[SerializeField]
		private Transform skeletonRootOverride;

		// Token: 0x04003DCB RID: 15819
		[Header("Appearance")]
		[Tooltip("If > 0, bubbles scale in over this many seconds instead of popping instantly.")]
		[SerializeField]
		private float growInDuration = 0.6f;

		// Token: 0x04003DCC RID: 15820
		[Header("Lifetime (gradual shrink to nothing)")]
		[Tooltip("Seconds over which the bubbles gradually shrink until they disappear. Editor-tunable.")]
		[SerializeField]
		private float lifetimeSeconds = 60f;

		// Token: 0x04003DCD RID: 15821
		[Tooltip("Fraction of the original size the bubbles reach at the END of their lifetime (0 = fully gone). Usually 0.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float endScaleFraction;

		// Token: 0x04003DCE RID: 15822
		[Tooltip("Shape of the shrink over the lifetime (X: 0..1 normalized time, Y: 0..1 size factor where 1 = full size, 0 = endScaleFraction). Defaults to a smooth ease-out shrink.")]
		[SerializeField]
		private AnimationCurve shrinkCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

		// Token: 0x04003DCF RID: 15823
		[Header("HUD Timer (right side of screen)")]
		[Tooltip("Show the shared radial HUD timer (same as body-paint decals) for the bubble lifetime.")]
		[SerializeField]
		private bool showHUDTimer = true;

		// Token: 0x04003DD0 RID: 15824
		[Tooltip("HUD timer descriptor (id / label / accent color / optional icon). TotalDuration is set automatically from lifetimeSeconds.")]
		[SerializeField]
		private TimedEffectData hudTimerData = new TimedEffectData
		{
			EffectId = "ShowerBubbles",
			DisplayName = "Bubbles",
			AccentColor = new Color(0.6f, 0.85f, 1f, 1f)
		};

		// Token: 0x04003DD2 RID: 15826
		private readonly List<GameObject> _spawned = new List<GameObject>();

		// Token: 0x04003DD3 RID: 15827
		private readonly List<Transform> _spawnedTransforms = new List<Transform>();

		// Token: 0x04003DD4 RID: 15828
		private readonly List<Vector3> _targetScales = new List<Vector3>();

		// Token: 0x04003DD5 RID: 15829
		private float _growTimer;

		// Token: 0x04003DD6 RID: 15830
		private bool _growing;

		// Token: 0x04003DD7 RID: 15831
		private Coroutine _lifetimeRoutine;
	}
}
