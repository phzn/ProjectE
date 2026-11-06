using System;
using UnityEngine;

namespace Features.MasturbationSystem
{
	// Token: 0x02000FFC RID: 4092
	[DisallowMultipleComponent]
	public class OrgasmHeartbeatDriver : MonoBehaviour
	{
		// Token: 0x06007C1B RID: 31771 RVA: 0x0032B56A File Offset: 0x0032976A
		private void Start()
		{
			if (this.meter == null)
			{
				this.meter = OrgasmMeterController.Instance;
			}
			if (this.heartbeat == null)
			{
				this.heartbeat = Object.FindObjectOfType<HeartbeatController>();
			}
		}

		// Token: 0x06007C1C RID: 31772 RVA: 0x0032B59E File Offset: 0x0032979E
		private void OnDisable()
		{
			if (this._registered && this.heartbeat != null)
			{
				this.heartbeat.SetSourceIntensity(this.sourceId, 0f);
				this._registered = false;
			}
		}

		// Token: 0x06007C1D RID: 31773 RVA: 0x0032B5D4 File Offset: 0x003297D4
		private void Update()
		{
			if (this.meter == null)
			{
				this.meter = OrgasmMeterController.Instance;
				if (this.meter == null)
				{
					return;
				}
			}
			if (this.heartbeat == null)
			{
				this.heartbeat = Object.FindObjectOfType<HeartbeatController>();
				if (this.heartbeat == null)
				{
					return;
				}
			}
			float num = Mathf.Clamp01(this.meter.Value);
			this.heartbeat.SetSourceIntensity(this.sourceId, num);
			this._registered = true;
			if (this.debugMode && Time.frameCount % 60 == 0)
			{
				Debug.Log(string.Format("[OrgasmHeartbeatDriver] '{0}' → {1:F2}", this.sourceId, num));
			}
		}

		// Token: 0x04007FE0 RID: 32736
		[Header("References")]
		[Tooltip("Controller logico del meter. Se null, viene risolto dal singleton OrgasmMeterController.Instance allo Start.")]
		[SerializeField]
		private OrgasmMeterController meter;

		// Token: 0x04007FE1 RID: 32737
		[Tooltip("HeartbeatController da pilotare. Se null, viene cercato in scena.")]
		[SerializeField]
		private HeartbeatController heartbeat;

		// Token: 0x04007FE2 RID: 32738
		[Header("Source ID")]
		[Tooltip("ID univoco di questo driver. HeartbeatController usa l'ID per gestire piu' sorgenti (max-wins). Lascia default per il driver dell'orgasm meter.")]
		[SerializeField]
		private string sourceId = "orgasm";

		// Token: 0x04007FE3 RID: 32739
		[Header("Debug")]
		[SerializeField]
		private bool debugMode;

		// Token: 0x04007FE4 RID: 32740
		private bool _registered;
	}
}
