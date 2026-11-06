using System;
using UnityEngine;

namespace Features.MasturbationSystem
{
	// Token: 0x02000FFD RID: 4093
	[DisallowMultipleComponent]
	public class OrgasmMeterController : MonoBehaviour
	{
		// Token: 0x17001410 RID: 5136
		// (get) Token: 0x06007C1F RID: 31775 RVA: 0x0032B69B File Offset: 0x0032989B
		// (set) Token: 0x06007C20 RID: 31776 RVA: 0x0032B6A2 File Offset: 0x003298A2
		public static OrgasmMeterController Instance { get; private set; }

		// Token: 0x17001411 RID: 5137
		// (get) Token: 0x06007C21 RID: 31777 RVA: 0x0032B6AA File Offset: 0x003298AA
		// (set) Token: 0x06007C22 RID: 31778 RVA: 0x0032B6B2 File Offset: 0x003298B2
		public float Value { get; private set; }

		// Token: 0x17001412 RID: 5138
		// (get) Token: 0x06007C23 RID: 31779 RVA: 0x0032B6BB File Offset: 0x003298BB
		// (set) Token: 0x06007C24 RID: 31780 RVA: 0x0032B6C3 File Offset: 0x003298C3
		public OrgasmMeterController.MeterState State { get; private set; }

		// Token: 0x17001413 RID: 5139
		// (get) Token: 0x06007C25 RID: 31781 RVA: 0x0032B6CC File Offset: 0x003298CC
		// (set) Token: 0x06007C26 RID: 31782 RVA: 0x0032B6D4 File Offset: 0x003298D4
		public bool IsLocked { get; private set; }

		// Token: 0x140000DC RID: 220
		// (add) Token: 0x06007C27 RID: 31783 RVA: 0x0032B6E0 File Offset: 0x003298E0
		// (remove) Token: 0x06007C28 RID: 31784 RVA: 0x0032B718 File Offset: 0x00329918
		public event Action<float> OnValueChanged;

		// Token: 0x140000DD RID: 221
		// (add) Token: 0x06007C29 RID: 31785 RVA: 0x0032B750 File Offset: 0x00329950
		// (remove) Token: 0x06007C2A RID: 31786 RVA: 0x0032B788 File Offset: 0x00329988
		public event Action OnFillingStart;

		// Token: 0x140000DE RID: 222
		// (add) Token: 0x06007C2B RID: 31787 RVA: 0x0032B7C0 File Offset: 0x003299C0
		// (remove) Token: 0x06007C2C RID: 31788 RVA: 0x0032B7F8 File Offset: 0x003299F8
		public event Action OnEmptied;

		// Token: 0x140000DF RID: 223
		// (add) Token: 0x06007C2D RID: 31789 RVA: 0x0032B830 File Offset: 0x00329A30
		// (remove) Token: 0x06007C2E RID: 31790 RVA: 0x0032B868 File Offset: 0x00329A68
		public event Action OnMaxReached;

		// Token: 0x140000E0 RID: 224
		// (add) Token: 0x06007C2F RID: 31791 RVA: 0x0032B8A0 File Offset: 0x00329AA0
		// (remove) Token: 0x06007C30 RID: 31792 RVA: 0x0032B8D8 File Offset: 0x00329AD8
		public event Action OnCooldownStart;

		// Token: 0x06007C31 RID: 31793 RVA: 0x0032B90D File Offset: 0x00329B0D
		private void Awake()
		{
			if (OrgasmMeterController.Instance != null && OrgasmMeterController.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			OrgasmMeterController.Instance = this;
		}

		// Token: 0x06007C32 RID: 31794 RVA: 0x0032B93B File Offset: 0x00329B3B
		private void OnDestroy()
		{
			if (OrgasmMeterController.Instance == this)
			{
				OrgasmMeterController.Instance = null;
			}
		}

		// Token: 0x06007C33 RID: 31795 RVA: 0x0032B950 File Offset: 0x00329B50
		private void Update()
		{
			if (this.State == OrgasmMeterController.MeterState.Resting || this.IsLocked)
			{
				return;
			}
			float num = Time.time - this._lastProgressTime;
			OrgasmMeterController.MeterState state = this.State;
			if (state != OrgasmMeterController.MeterState.Active)
			{
				if (state != OrgasmMeterController.MeterState.Cooling)
				{
					return;
				}
				float value = this.Value;
				this.Value = Mathf.Max(0f, this.Value - this.decayRate * Time.deltaTime);
				if (!Mathf.Approximately(value, this.Value))
				{
					Action<float> onValueChanged = this.OnValueChanged;
					if (onValueChanged != null)
					{
						onValueChanged(this.Value);
					}
				}
				if (this.Value <= 0f)
				{
					this.Value = 0f;
					this.State = OrgasmMeterController.MeterState.Resting;
					if (this.debugMode)
					{
						Debug.Log("[OrgasmMeter] Cooling → Resting (emptied)");
					}
					Action onEmptied = this.OnEmptied;
					if (onEmptied == null)
					{
						return;
					}
					onEmptied();
				}
			}
			else if (num >= this.cooldownDelay)
			{
				this.State = OrgasmMeterController.MeterState.Cooling;
				if (this.debugMode)
				{
					Debug.Log(string.Format("[OrgasmMeter] Active → Cooling (no progress for {0:F2}s)", num));
				}
				Action onCooldownStart = this.OnCooldownStart;
				if (onCooldownStart == null)
				{
					return;
				}
				onCooldownStart();
				return;
			}
		}

		// Token: 0x06007C34 RID: 31796 RVA: 0x0032BA5C File Offset: 0x00329C5C
		public void AddProgress(float amount)
		{
			if (amount <= 0f)
			{
				return;
			}
			if (this.IsLocked)
			{
				return;
			}
			this._lastProgressTime = Time.time;
			bool flag = this.State == OrgasmMeterController.MeterState.Resting;
			bool flag2 = this.State == OrgasmMeterController.MeterState.Cooling;
			if (flag || flag2)
			{
				this.State = OrgasmMeterController.MeterState.Active;
				if (flag)
				{
					if (this.debugMode)
					{
						Debug.Log("[OrgasmMeter] Resting → Active (first progress)");
					}
					Action onFillingStart = this.OnFillingStart;
					if (onFillingStart != null)
					{
						onFillingStart();
					}
				}
				else if (this.debugMode)
				{
					Debug.Log("[OrgasmMeter] Cooling → Active (progress resumed)");
				}
			}
			float value = this.Value;
			this.Value = Mathf.Clamp01(this.Value + amount);
			if (!Mathf.Approximately(value, this.Value))
			{
				Action<float> onValueChanged = this.OnValueChanged;
				if (onValueChanged != null)
				{
					onValueChanged(this.Value);
				}
			}
			if (this.Value >= 1f && value < 1f)
			{
				if (this.lockOnMaxReached)
				{
					this.IsLocked = true;
				}
				if (this.debugMode)
				{
					Debug.Log("[OrgasmMeter] Max reached");
				}
				Action onMaxReached = this.OnMaxReached;
				if (onMaxReached == null)
				{
					return;
				}
				onMaxReached();
			}
		}

		// Token: 0x06007C35 RID: 31797 RVA: 0x0032BB68 File Offset: 0x00329D68
		public void Reset()
		{
			this.IsLocked = false;
			if (this.State == OrgasmMeterController.MeterState.Resting && Mathf.Approximately(this.Value, 0f))
			{
				return;
			}
			this.Value = 0f;
			this.State = OrgasmMeterController.MeterState.Resting;
			Action<float> onValueChanged = this.OnValueChanged;
			if (onValueChanged != null)
			{
				onValueChanged(0f);
			}
			if (this.debugMode)
			{
				Debug.Log("[OrgasmMeter] Reset");
			}
			Action onEmptied = this.OnEmptied;
			if (onEmptied == null)
			{
				return;
			}
			onEmptied();
		}

		// Token: 0x06007C36 RID: 31798 RVA: 0x0032BBE4 File Offset: 0x00329DE4
		public void ForceCooldown()
		{
			if (this.IsLocked)
			{
				return;
			}
			if (this.State != OrgasmMeterController.MeterState.Active)
			{
				return;
			}
			this.State = OrgasmMeterController.MeterState.Cooling;
			this._lastProgressTime = Time.time - this.cooldownDelay;
			if (this.debugMode)
			{
				Debug.Log("[OrgasmMeter] ForceCooldown");
			}
			Action onCooldownStart = this.OnCooldownStart;
			if (onCooldownStart == null)
			{
				return;
			}
			onCooldownStart();
		}

		// Token: 0x06007C37 RID: 31799 RVA: 0x0032BC40 File Offset: 0x00329E40
		public void UnlockAndCooldown()
		{
			this.IsLocked = false;
			if (this.State != OrgasmMeterController.MeterState.Active)
			{
				if (this.debugMode)
				{
					Debug.Log(string.Format("[OrgasmMeter] UnlockAndCooldown (already in state {0})", this.State));
				}
				return;
			}
			this.State = OrgasmMeterController.MeterState.Cooling;
			this._lastProgressTime = Time.time - this.cooldownDelay;
			if (this.debugMode)
			{
				Debug.Log("[OrgasmMeter] UnlockAndCooldown → Cooling");
			}
			Action onCooldownStart = this.OnCooldownStart;
			if (onCooldownStart == null)
			{
				return;
			}
			onCooldownStart();
		}

		// Token: 0x04007FE6 RID: 32742
		[Header("Cooldown")]
		[Tooltip("Secondi senza AddProgress prima che la barra inizi a scendere.")]
		[Min(0f)]
		[SerializeField]
		private float cooldownDelay = 2f;

		// Token: 0x04007FE7 RID: 32743
		[Tooltip("Quanta barra (0..1) si svuota al secondo durante il cooldown.")]
		[Min(0f)]
		[SerializeField]
		private float decayRate = 0.15f;

		// Token: 0x04007FE8 RID: 32744
		[Header("Behavior")]
		[Tooltip("Se true, una volta raggiunto il max il meter si blocca finche' non viene chiamato Reset(). Cosi' l'ending animation puo' partire senza che il valore scenda durante la transizione.")]
		[SerializeField]
		private bool lockOnMaxReached = true;

		// Token: 0x04007FE9 RID: 32745
		[Header("Debug")]
		[SerializeField]
		private bool debugMode;

		// Token: 0x04007FF2 RID: 32754
		private float _lastProgressTime;

		// Token: 0x02000FFE RID: 4094
		public enum MeterState
		{
			// Token: 0x04007FF4 RID: 32756
			Resting,
			// Token: 0x04007FF5 RID: 32757
			Active,
			// Token: 0x04007FF6 RID: 32758
			Cooling
		}
	}
}
