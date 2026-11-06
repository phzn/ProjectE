using System;
using TMPro;
using UnityEngine;

namespace Features.MasturbationSystem
{
	// Token: 0x02000FF6 RID: 4086
	[DisallowMultipleComponent]
	public class MasturbationSpeedWidgetLink : MonoBehaviour
	{
		// Token: 0x06007BE0 RID: 31712 RVA: 0x0032961E File Offset: 0x0032781E
		private void Awake()
		{
			if (this.label == null)
			{
				base.TryGetComponent<TMP_Text>(out this.label);
			}
			if (this.widgetTransform == null)
			{
				this.widgetTransform = base.transform as RectTransform;
			}
		}

		// Token: 0x06007BE1 RID: 31713 RVA: 0x0032965A File Offset: 0x0032785A
		private void Start()
		{
			this.TryBind();
		}

		// Token: 0x06007BE2 RID: 31714 RVA: 0x00329662 File Offset: 0x00327862
		private void Update()
		{
			if (this._bound)
			{
				return;
			}
			if (Time.unscaledTime < this._nextRetryTime)
			{
				return;
			}
			this._nextRetryTime = Time.unscaledTime + 0.5f;
			this.TryBind();
		}

		// Token: 0x06007BE3 RID: 31715 RVA: 0x00329694 File Offset: 0x00327894
		private void TryBind()
		{
			if (this._bound)
			{
				return;
			}
			MasturbationSpeedController masturbationSpeedController = Object.FindObjectOfType<MasturbationSpeedController>();
			if (masturbationSpeedController == null)
			{
				if (this.debugMode)
				{
					Debug.LogWarning("[SpeedWidgetLink] No MasturbationSpeedController found in scene — will retry.");
				}
				return;
			}
			masturbationSpeedController.BindSpeedWidget(this.widgetTransform, this.label);
			this._bound = true;
			if (this.debugMode)
			{
				Debug.Log("[SpeedWidgetLink] Bound widget to controller on '" + masturbationSpeedController.gameObject.name + "'.");
			}
		}

		// Token: 0x04007F80 RID: 32640
		[Tooltip("TMP_Text che mostra la velocita. Se null, viene cercato sul GameObject.")]
		[SerializeField]
		private TMP_Text label;

		// Token: 0x04007F81 RID: 32641
		[Tooltip("RectTransform del widget. Se null, viene preso dal transform di questo GameObject.")]
		[SerializeField]
		private RectTransform widgetTransform;

		// Token: 0x04007F82 RID: 32642
		[Header("Debug")]
		[SerializeField]
		private bool debugMode;

		// Token: 0x04007F83 RID: 32643
		private bool _bound;

		// Token: 0x04007F84 RID: 32644
		private float _nextRetryTime;

		// Token: 0x04007F85 RID: 32645
		private const float RetryIntervalSeconds = 0.5f;
	}
}
