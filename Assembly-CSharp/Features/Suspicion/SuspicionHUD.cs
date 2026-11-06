using System;
using System.Collections;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Suspicion
{
	// Token: 0x02000C32 RID: 3122
	[DisallowMultipleComponent]
	public class SuspicionHUD : MonoBehaviour
	{
		// Token: 0x060059F2 RID: 23026 RVA: 0x00233C0D File Offset: 0x00231E0D
		private void Awake()
		{
			if (this.rootCanvasGroup != null)
			{
				this.rootCanvasGroup.alpha = 0f;
			}
			if (this.label != null)
			{
				this.label.text = string.Empty;
			}
		}

		// Token: 0x060059F3 RID: 23027 RVA: 0x00233C4B File Offset: 0x00231E4B
		private IEnumerator Start()
		{
			if (this.manager == null)
			{
				this.manager = SuspicionManager.Instance;
			}
			this.HookManager();
			if (this.bootDelay > 0f)
			{
				yield return new WaitForSeconds(this.bootDelay);
			}
			this._bootReady = true;
			if (this.manager != null)
			{
				this.ApplyLevelImmediate(this.manager.CurrentLevel);
			}
			yield break;
		}

		// Token: 0x060059F4 RID: 23028 RVA: 0x00233C5A File Offset: 0x00231E5A
		private void OnEnable()
		{
			this.HookManager();
		}

		// Token: 0x060059F5 RID: 23029 RVA: 0x00233C62 File Offset: 0x00231E62
		private void OnDestroy()
		{
			this.UnhookManager();
		}

		// Token: 0x060059F6 RID: 23030 RVA: 0x00233C6C File Offset: 0x00231E6C
		private void HookManager()
		{
			if (this.manager == null)
			{
				this.manager = SuspicionManager.Instance;
			}
			if (this.manager == null || this._hookedToManager)
			{
				return;
			}
			this.manager.OnLevelChanged.AddListener(new UnityAction<int>(this.HandleLevelChanged));
			this._hookedToManager = true;
			if (this.debugLogs)
			{
				Debug.Log("[SuspicionHUD] Hooked to SuspicionManager");
			}
		}

		// Token: 0x060059F7 RID: 23031 RVA: 0x00233CDE File Offset: 0x00231EDE
		private void UnhookManager()
		{
			if (this.manager == null || !this._hookedToManager)
			{
				return;
			}
			this.manager.OnLevelChanged.RemoveListener(new UnityAction<int>(this.HandleLevelChanged));
			this._hookedToManager = false;
		}

		// Token: 0x060059F8 RID: 23032 RVA: 0x00233D1C File Offset: 0x00231F1C
		private void HandleLevelChanged(int newLevel)
		{
			if (!this._bootReady)
			{
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[SuspicionHUD] Level event {0} deferred until bootReady", newLevel));
				}
				return;
			}
			int num = ((this._displayedLevel < 0) ? 0 : this._displayedLevel);
			int num2 = Mathf.Max(0, newLevel);
			if (num2 == num)
			{
				return;
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[SuspicionHUD] Level transition {0} → {1}", num, num2));
			}
			this.UpdateLabel(num2);
			if (num == 0 && num2 > 0)
			{
				if (this.showFeedback != null)
				{
					this.showFeedback.PlayFeedbacks();
				}
			}
			else if (num > 0 && num2 == 0)
			{
				if (this.hideFeedback != null)
				{
					this.hideFeedback.PlayFeedbacks();
				}
			}
			else if (this.changeFeedback != null)
			{
				this.changeFeedback.PlayFeedbacks();
			}
			this._displayedLevel = num2;
		}

		// Token: 0x060059F9 RID: 23033 RVA: 0x00233E00 File Offset: 0x00232000
		private void ApplyLevelImmediate(int level)
		{
			int num = Mathf.Max(0, level);
			if (num == this._displayedLevel)
			{
				return;
			}
			this.UpdateLabel(num);
			if (num == 0)
			{
				if (this.rootCanvasGroup != null)
				{
					this.rootCanvasGroup.alpha = 0f;
				}
			}
			else if (this.rootCanvasGroup != null)
			{
				this.rootCanvasGroup.alpha = 1f;
			}
			this._displayedLevel = num;
		}

		// Token: 0x060059FA RID: 23034 RVA: 0x00233E70 File Offset: 0x00232070
		private void UpdateLabel(int level)
		{
			if (this.label == null)
			{
				return;
			}
			if (level != 0)
			{
				if (level == 1)
				{
					this.label.text = this.lv1Label;
					if (this.tintLabelPerLevel)
					{
						this.label.color = this.lv1Color;
						return;
					}
				}
				else
				{
					this.label.text = this.lv2Label;
					if (this.tintLabelPerLevel)
					{
						this.label.color = this.lv2Color;
					}
				}
			}
		}

		// Token: 0x060059FB RID: 23035 RVA: 0x00233EE8 File Offset: 0x002320E8
		public void OnHideFeedbackComplete()
		{
			if (this.label != null)
			{
				this.label.text = string.Empty;
			}
		}

		// Token: 0x04005AEB RID: 23275
		[Header("References")]
		[Tooltip("SuspicionManager to observe. If null, taken from SuspicionManager.Instance in Start.")]
		[SerializeField]
		private SuspicionManager manager;

		// Token: 0x04005AEC RID: 23276
		[Tooltip("Label text displayed under the MiniMap.")]
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04005AED RID: 23277
		[Tooltip("Optional CanvasGroup of the root. Driven to 0 in Awake so nothing flashes on the first frame; the showFeedback should fade it back in.")]
		[SerializeField]
		private CanvasGroup rootCanvasGroup;

		// Token: 0x04005AEE RID: 23278
		[Header("Level Labels (in English)")]
		[Tooltip("Text shown at Suspicion Level 1. Use a short, game-y word.")]
		[SerializeField]
		private string lv1Label = "EXPOSED";

		// Token: 0x04005AEF RID: 23279
		[Tooltip("Text shown at Suspicion Level 2.")]
		[SerializeField]
		private string lv2Label = "WANTED";

		// Token: 0x04005AF0 RID: 23280
		[Header("Level Colors (optional)")]
		[Tooltip("If true, label color is changed per level (lv1Color / lv2Color).")]
		[SerializeField]
		private bool tintLabelPerLevel = true;

		// Token: 0x04005AF1 RID: 23281
		[SerializeField]
		private Color lv1Color = new Color(1f, 0.78f, 0.2f, 1f);

		// Token: 0x04005AF2 RID: 23282
		[SerializeField]
		private Color lv2Color = new Color(1f, 0.25f, 0.25f, 1f);

		// Token: 0x04005AF3 RID: 23283
		[Header("Feedbacks (MMF)")]
		[Tooltip("Played on transition 0 → >0 (label appears).")]
		[SerializeField]
		private MMF_Player showFeedback;

		// Token: 0x04005AF4 RID: 23284
		[Tooltip("Played on transition >0 → 0 (label disappears).")]
		[SerializeField]
		private MMF_Player hideFeedback;

		// Token: 0x04005AF5 RID: 23285
		[Tooltip("Played when the level changes while the label is already visible (1↔2). Typically a punch / shake / color flash.")]
		[SerializeField]
		private MMF_Player changeFeedback;

		// Token: 0x04005AF6 RID: 23286
		[Header("Boot")]
		[Tooltip("Seconds to wait before the HUD becomes interactive after scene load. Avoids first-frame flashes if SuspicionManager fires events during scene init.")]
		[Min(0f)]
		[SerializeField]
		private float bootDelay = 0.2f;

		// Token: 0x04005AF7 RID: 23287
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04005AF8 RID: 23288
		private int _displayedLevel = -1;

		// Token: 0x04005AF9 RID: 23289
		private bool _hookedToManager;

		// Token: 0x04005AFA RID: 23290
		private bool _bootReady;
	}
}
