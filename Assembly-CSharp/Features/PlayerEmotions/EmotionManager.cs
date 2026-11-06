using System;
using System.Collections.Generic;
using Features.RunModes;
using Features.TimeFreeze;
using Invector.vCharacterController;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Features.PlayerEmotions
{
	// Token: 0x02000E04 RID: 3588
	public class EmotionManager : MonoBehaviour
	{
		// Token: 0x17001188 RID: 4488
		// (get) Token: 0x06006AD8 RID: 27352 RVA: 0x002ADDCA File Offset: 0x002ABFCA
		// (set) Token: 0x06006AD9 RID: 27353 RVA: 0x002ADDD1 File Offset: 0x002ABFD1
		public static EmotionManager Instance { get; private set; }

		// Token: 0x17001189 RID: 4489
		// (get) Token: 0x06006ADA RID: 27354 RVA: 0x002ADDD9 File Offset: 0x002ABFD9
		// (set) Token: 0x06006ADB RID: 27355 RVA: 0x002ADDE4 File Offset: 0x002ABFE4
		public float EmotionBalance
		{
			get
			{
				return this._emotionBalance;
			}
			private set
			{
				float num = Mathf.Clamp(value, -100f, 100f);
				EmotionStance? emotionStance = this.StanceLock;
				EmotionStance emotionStance2 = EmotionStance.Embarassed;
				if ((emotionStance.GetValueOrDefault() == emotionStance2) & (emotionStance != null))
				{
					num = Mathf.Min(num, -this.StanceRestingLevel);
				}
				else
				{
					emotionStance = this.StanceLock;
					emotionStance2 = EmotionStance.Confident;
					if ((emotionStance.GetValueOrDefault() == emotionStance2) & (emotionStance != null))
					{
						num = Mathf.Max(num, this.StanceRestingLevel);
					}
				}
				this._emotionBalance = num;
			}
		}

		// Token: 0x1700118A RID: 4490
		// (get) Token: 0x06006ADC RID: 27356 RVA: 0x002ADE60 File Offset: 0x002AC060
		public float ShameLevel
		{
			get
			{
				if (this._emotionBalance >= 0f)
				{
					return 0f;
				}
				return -this._emotionBalance;
			}
		}

		// Token: 0x1700118B RID: 4491
		// (get) Token: 0x06006ADD RID: 27357 RVA: 0x002ADE7C File Offset: 0x002AC07C
		public float ExhibitionLevel
		{
			get
			{
				if (this._emotionBalance <= 0f)
				{
					return 0f;
				}
				return this._emotionBalance;
			}
		}

		// Token: 0x1700118C RID: 4492
		// (get) Token: 0x06006ADE RID: 27358 RVA: 0x002ADE97 File Offset: 0x002AC097
		public bool IsShame
		{
			get
			{
				return this._emotionBalance < 0f;
			}
		}

		// Token: 0x1700118D RID: 4493
		// (get) Token: 0x06006ADF RID: 27359 RVA: 0x002ADEA6 File Offset: 0x002AC0A6
		public bool IsExhibition
		{
			get
			{
				return this._emotionBalance > 0f;
			}
		}

		// Token: 0x1700118E RID: 4494
		// (get) Token: 0x06006AE0 RID: 27360 RVA: 0x002ADEB5 File Offset: 0x002AC0B5
		public EmotionStance CurrentStance
		{
			get
			{
				if (this._emotionBalance >= 0f)
				{
					return EmotionStance.Confident;
				}
				return EmotionStance.Embarassed;
			}
		}

		// Token: 0x140000B4 RID: 180
		// (add) Token: 0x06006AE1 RID: 27361 RVA: 0x002ADEC8 File Offset: 0x002AC0C8
		// (remove) Token: 0x06006AE2 RID: 27362 RVA: 0x002ADF00 File Offset: 0x002AC100
		public event Action<EmotionChangedEventArgs> OnEmotionChanged;

		// Token: 0x140000B5 RID: 181
		// (add) Token: 0x06006AE3 RID: 27363 RVA: 0x002ADF38 File Offset: 0x002AC138
		// (remove) Token: 0x06006AE4 RID: 27364 RVA: 0x002ADF70 File Offset: 0x002AC170
		public event Action<EmotionModifier> OnModifierAdded;

		// Token: 0x140000B6 RID: 182
		// (add) Token: 0x06006AE5 RID: 27365 RVA: 0x002ADFA8 File Offset: 0x002AC1A8
		// (remove) Token: 0x06006AE6 RID: 27366 RVA: 0x002ADFE0 File Offset: 0x002AC1E0
		public event Action<EmotionModifier> OnModifierRemoved;

		// Token: 0x140000B7 RID: 183
		// (add) Token: 0x06006AE7 RID: 27367 RVA: 0x002AE018 File Offset: 0x002AC218
		// (remove) Token: 0x06006AE8 RID: 27368 RVA: 0x002AE050 File Offset: 0x002AC250
		public event Action<EmotionStance> OnStanceChanged;

		// Token: 0x1700118F RID: 4495
		// (get) Token: 0x06006AE9 RID: 27369 RVA: 0x002AE085 File Offset: 0x002AC285
		public bool IsPlayerNaked
		{
			get
			{
				return this.ClothingController != null && !this.ClothingController.IsFullyClothed;
			}
		}

		// Token: 0x17001190 RID: 4496
		// (get) Token: 0x06006AEA RID: 27370 RVA: 0x002AE0A5 File Offset: 0x002AC2A5
		public IReadOnlyList<EmotionModifier> ActiveModifiers
		{
			get
			{
				return this._activeModifiers;
			}
		}

		// Token: 0x17001191 RID: 4497
		// (get) Token: 0x06006AEB RID: 27371 RVA: 0x002AE0AD File Offset: 0x002AC2AD
		public float NormalizedBalance
		{
			get
			{
				return this._emotionBalance / 100f;
			}
		}

		// Token: 0x06006AEC RID: 27372 RVA: 0x002AE0BB File Offset: 0x002AC2BB
		private void Awake()
		{
			if (EmotionManager.Instance != null && EmotionManager.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			EmotionManager.Instance = this;
		}

		// Token: 0x06006AED RID: 27373 RVA: 0x002AE0E9 File Offset: 0x002AC2E9
		private void Start()
		{
			if (this.ClothingController == null)
			{
				this.ClothingController = Object.FindObjectOfType<ClothingToggleController>();
			}
			this.EmotionBalance = this.BaseBalance;
		}

		// Token: 0x06006AEE RID: 27374 RVA: 0x002AE110 File Offset: 0x002AC310
		private void Update()
		{
			if (this._isFrozen)
			{
				return;
			}
			this.HandleStanceSwitchInput();
			if (!this._isInActiveEvent)
			{
				this.UpdateContinuousSources();
			}
			this.UpdateModifiers();
			if (this.EnableDecay && !this._isInActiveEvent && !this._decaySuspended && Time.time - this._lastImpactTime > this.DecayStartDelay)
			{
				this.ApplyDecay();
			}
		}

		// Token: 0x06006AEF RID: 27375 RVA: 0x002AE172 File Offset: 0x002AC372
		private void OnDestroy()
		{
			if (EmotionManager.Instance == this)
			{
				EmotionManager.Instance = null;
			}
		}

		// Token: 0x06006AF0 RID: 27376 RVA: 0x002AE188 File Offset: 0x002AC388
		public void ApplyImpact(EmotionImpactSO impactSO, float distance = 0f, float curveInput = 1f)
		{
			if (impactSO == null)
			{
				return;
			}
			EmotionImpactData emotionImpactData = impactSO.CalculateImpact(this.IsPlayerNaked, distance, curveInput);
			this.ApplyImpactInternal(emotionImpactData, impactSO.ImpactId, impactSO.Category);
		}

		// Token: 0x06006AF1 RID: 27377 RVA: 0x002AE1C4 File Offset: 0x002AC3C4
		public void ApplyImpact(EmotionImpactData impact, string sourceId, EmotionSourceCategory category)
		{
			EmotionImpactData emotionImpactData = impact.ApplyModifiers(this.IsPlayerNaked, 0f);
			this.ApplyImpactInternal(emotionImpactData, sourceId, category);
		}

		// Token: 0x06006AF2 RID: 27378 RVA: 0x002AE1EC File Offset: 0x002AC3EC
		public void ApplyDelta(float delta, string sourceId, EmotionSourceCategory category = EmotionSourceCategory.System)
		{
			EmotionImpactData emotionImpactData = new EmotionImpactData(delta, EmotionImpactType.Delta);
			this.ApplyImpactInternal(emotionImpactData, sourceId, category);
		}

		// Token: 0x06006AF3 RID: 27379 RVA: 0x002AE20C File Offset: 0x002AC40C
		public void SetBalance(float value, string sourceId = "Direct")
		{
			EmotionImpactData emotionImpactData = new EmotionImpactData(value, EmotionImpactType.SetTo);
			this.ApplyImpactInternal(emotionImpactData, sourceId, EmotionSourceCategory.System);
		}

		// Token: 0x06006AF4 RID: 27380 RVA: 0x002AE22C File Offset: 0x002AC42C
		public void ModifyBalance(float delta, string sourceId = "Direct")
		{
			if (this._isFrozen)
			{
				return;
			}
			if (Mathf.Approximately(delta, 0f))
			{
				return;
			}
			float emotionBalance = this.EmotionBalance;
			this.EmotionBalance += delta;
			if (!Mathf.Approximately(emotionBalance, this.EmotionBalance))
			{
				this._lastImpactTime = Time.time;
				this.NotifyChange(emotionBalance, sourceId, EmotionSourceCategory.System);
			}
		}

		// Token: 0x06006AF5 RID: 27381 RVA: 0x002AE287 File Offset: 0x002AC487
		public void ModifyShameLevel(float delta, string sourceId = "Direct")
		{
			this.ModifyBalance(-delta, sourceId);
		}

		// Token: 0x06006AF6 RID: 27382 RVA: 0x002AE292 File Offset: 0x002AC492
		public void ModifyExhibitionLevel(float delta, string sourceId = "Direct")
		{
			this.ModifyBalance(delta, sourceId);
		}

		// Token: 0x06006AF7 RID: 27383 RVA: 0x002AE29C File Offset: 0x002AC49C
		public void SetEmotionLevels(float shame, float exhibition, string sourceId = "Direct")
		{
			float num = exhibition - shame;
			this.SetBalance(num, sourceId);
		}

		// Token: 0x06006AF8 RID: 27384 RVA: 0x002AE2B5 File Offset: 0x002AC4B5
		public void ResetToBaseValues()
		{
			this.SetBalance(this.BaseBalance, "Reset");
		}

		// Token: 0x06006AF9 RID: 27385 RVA: 0x002AE2C8 File Offset: 0x002AC4C8
		private void HandleStanceSwitchInput()
		{
			if (this._isFrozen || !this.EnableStanceSwitchInput || this.StanceSwitchInput == null || !this.StanceSwitchInput.useInput || (this._isInActiveEvent && !TimeFreezeController.IsAnyActive) || !PlayerActivityGate.Allows(PlayerActivity.StanceSwitch))
			{
				return;
			}
			if (ControllerInput.IsActive ? ControllerGameplayBridge.Read(KeyCode.Tab, 1) : this.StanceSwitchInput.GetButtonDown())
			{
				this.ToggleStance("StanceSwitch");
			}
		}

		// Token: 0x06006AFA RID: 27386 RVA: 0x002AE33C File Offset: 0x002AC53C
		public void ToggleStance(string sourceId = "StanceSwitch")
		{
			EmotionStance emotionStance = ((this.CurrentStance == EmotionStance.Confident) ? EmotionStance.Embarassed : EmotionStance.Confident);
			this.SetStance(emotionStance, sourceId);
		}

		// Token: 0x06006AFB RID: 27387 RVA: 0x002AE360 File Offset: 0x002AC560
		public void SetStance(EmotionStance stance, string sourceId = "StanceSwitch")
		{
			if (this._isFrozen)
			{
				return;
			}
			float num = ((stance == EmotionStance.Confident) ? this.StanceSwitchLevel : (-this.StanceSwitchLevel));
			this.SetBalance(num, sourceId);
			if (this.EnableDebugLog)
			{
				Debug.Log(string.Format("[EmotionManager] Stance set to {0} (balance -> {1:F0})", stance, num));
			}
		}

		// Token: 0x06006AFC RID: 27388 RVA: 0x002AE3B4 File Offset: 0x002AC5B4
		public void OnActiveEventStarted()
		{
			this._isInActiveEvent = true;
			if (this.EnableDebugLog)
			{
				Debug.Log("[EmotionManager] Active event started - decay paused");
			}
		}

		// Token: 0x06006AFD RID: 27389 RVA: 0x002AE3CF File Offset: 0x002AC5CF
		public void OnActiveEventEnded()
		{
			this._isInActiveEvent = false;
			this._lastImpactTime = Time.time;
			if (this.EnableDebugLog)
			{
				Debug.Log("[EmotionManager] Active event ended - decay will resume after delay");
			}
		}

		// Token: 0x06006AFE RID: 27390 RVA: 0x002AE3F5 File Offset: 0x002AC5F5
		public void SetFrozen(bool frozen)
		{
			this._isFrozen = frozen;
			if (this.EnableDebugLog)
			{
				Debug.Log(string.Format("[EmotionManager] Frozen = {0}", frozen));
			}
		}

		// Token: 0x17001192 RID: 4498
		// (get) Token: 0x06006AFF RID: 27391 RVA: 0x002AE41B File Offset: 0x002AC61B
		public bool IsFrozen
		{
			get
			{
				return this._isFrozen;
			}
		}

		// Token: 0x06006B00 RID: 27392 RVA: 0x002AE424 File Offset: 0x002AC624
		public void SetStanceLock(EmotionStance? lockedStance)
		{
			this._stanceLock = lockedStance;
			if (this.StanceLock != null)
			{
				this.EmotionBalance = this._emotionBalance;
			}
			if (this.EnableDebugLog)
			{
				Debug.Log("[EmotionManager] StanceLock = " + ((lockedStance != null) ? lockedStance.Value.ToString() : "none"));
			}
		}

		// Token: 0x17001193 RID: 4499
		// (get) Token: 0x06006B01 RID: 27393 RVA: 0x002AE490 File Offset: 0x002AC690
		public EmotionStance? StanceLock
		{
			get
			{
				if (!RunModeService.IsLongWayHome)
				{
					return this._stanceLock;
				}
				return new EmotionStance?(EmotionStance.Embarassed);
			}
		}

		// Token: 0x06006B02 RID: 27394 RVA: 0x002AE4A6 File Offset: 0x002AC6A6
		public void SetDecaySuspended(bool suspended)
		{
			if (this._decaySuspended == suspended)
			{
				return;
			}
			this._decaySuspended = suspended;
			if (suspended)
			{
				this._lastImpactTime = Time.time;
			}
			if (this.EnableDebugLog)
			{
				Debug.Log(string.Format("[EmotionManager] DecaySuspended = {0}", suspended));
			}
		}

		// Token: 0x17001194 RID: 4500
		// (get) Token: 0x06006B03 RID: 27395 RVA: 0x002AE4E4 File Offset: 0x002AC6E4
		public bool IsDecaySuspended
		{
			get
			{
				return this._decaySuspended;
			}
		}

		// Token: 0x17001195 RID: 4501
		// (get) Token: 0x06006B04 RID: 27396 RVA: 0x002AE4EC File Offset: 0x002AC6EC
		public bool IsInActiveEvent
		{
			get
			{
				return this._isInActiveEvent;
			}
		}

		// Token: 0x06006B05 RID: 27397 RVA: 0x002AE4F4 File Offset: 0x002AC6F4
		public EmotionModifier AddModifier(EmotionModifierSO modifierSO, string sourceId = null)
		{
			if (modifierSO == null)
			{
				return null;
			}
			EmotionModifier emotionModifier = modifierSO.CreateInstance(sourceId);
			return this.AddModifier(emotionModifier);
		}

		// Token: 0x06006B06 RID: 27398 RVA: 0x002AE51C File Offset: 0x002AC71C
		public EmotionModifier AddModifier(EmotionModifier modifier)
		{
			if (modifier == null)
			{
				return null;
			}
			if (!modifier.CanStack)
			{
				this._activeModifiers.RemoveAll((EmotionModifier m) => m.ModifierId == modifier.ModifierId);
			}
			else if (modifier.MaxStacks > 0 && this._activeModifiers.FindAll((EmotionModifier m) => m.ModifierId == modifier.ModifierId).Count >= modifier.MaxStacks)
			{
				if (this.EnableDebugLog)
				{
					Debug.Log(string.Format("[EmotionManager] Modifier '{0}' at max stacks ({1})", modifier.DisplayName, modifier.MaxStacks));
				}
				return null;
			}
			modifier.StartTime = Time.time;
			this._activeModifiers.Add(modifier);
			Action<EmotionModifier> onModifierAdded = this.OnModifierAdded;
			if (onModifierAdded != null)
			{
				onModifierAdded(modifier);
			}
			if (this.EnableDebugLog)
			{
				Debug.Log(string.Format("[EmotionManager] Added modifier: {0} (Duration: {1}s)", modifier.DisplayName, modifier.Duration));
			}
			return modifier;
		}

		// Token: 0x06006B07 RID: 27399 RVA: 0x002AE644 File Offset: 0x002AC844
		public bool RemoveModifier(EmotionModifier modifier)
		{
			if (modifier == null)
			{
				return false;
			}
			bool flag = this._activeModifiers.Remove(modifier);
			if (flag)
			{
				Action<EmotionModifier> onModifierRemoved = this.OnModifierRemoved;
				if (onModifierRemoved != null)
				{
					onModifierRemoved(modifier);
				}
				if (this.EnableDebugLog)
				{
					Debug.Log("[EmotionManager] Removed modifier: " + modifier.DisplayName);
				}
			}
			return flag;
		}

		// Token: 0x06006B08 RID: 27400 RVA: 0x002AE694 File Offset: 0x002AC894
		public int RemoveModifiersById(string modifierId)
		{
			List<EmotionModifier> list = this._activeModifiers.FindAll((EmotionModifier m) => m.ModifierId == modifierId);
			foreach (EmotionModifier emotionModifier in list)
			{
				this._activeModifiers.Remove(emotionModifier);
				Action<EmotionModifier> onModifierRemoved = this.OnModifierRemoved;
				if (onModifierRemoved != null)
				{
					onModifierRemoved(emotionModifier);
				}
			}
			if (this.EnableDebugLog && list.Count > 0)
			{
				Debug.Log(string.Format("[EmotionManager] Removed {0} modifiers with ID: {1}", list.Count, modifierId));
			}
			return list.Count;
		}

		// Token: 0x06006B09 RID: 27401 RVA: 0x002AE758 File Offset: 0x002AC958
		public int RemoveModifiersBySource(string sourceId)
		{
			List<EmotionModifier> list = this._activeModifiers.FindAll((EmotionModifier m) => m.SourceId == sourceId);
			foreach (EmotionModifier emotionModifier in list)
			{
				this._activeModifiers.Remove(emotionModifier);
				Action<EmotionModifier> onModifierRemoved = this.OnModifierRemoved;
				if (onModifierRemoved != null)
				{
					onModifierRemoved(emotionModifier);
				}
			}
			return list.Count;
		}

		// Token: 0x06006B0A RID: 27402 RVA: 0x002AE7EC File Offset: 0x002AC9EC
		public void ClearAllModifiers()
		{
			foreach (EmotionModifier emotionModifier in this._activeModifiers)
			{
				Action<EmotionModifier> onModifierRemoved = this.OnModifierRemoved;
				if (onModifierRemoved != null)
				{
					onModifierRemoved(emotionModifier);
				}
			}
			this._activeModifiers.Clear();
			if (this.EnableDebugLog)
			{
				Debug.Log("[EmotionManager] Cleared all modifiers");
			}
		}

		// Token: 0x06006B0B RID: 27403 RVA: 0x002AE868 File Offset: 0x002ACA68
		public void RegisterContinuousSource(IContinuousEmotionSource source)
		{
			if (source == null || string.IsNullOrEmpty(source.SourceId))
			{
				return;
			}
			this._continuousSources[source.SourceId] = source;
			if (this.EnableDebugLog)
			{
				Debug.Log("[EmotionManager] Registered continuous source: " + source.SourceId);
			}
		}

		// Token: 0x06006B0C RID: 27404 RVA: 0x002AE8B5 File Offset: 0x002ACAB5
		public void UnregisterContinuousSource(string sourceId)
		{
			if (this._continuousSources.Remove(sourceId) && this.EnableDebugLog)
			{
				Debug.Log("[EmotionManager] Unregistered continuous source: " + sourceId);
			}
		}

		// Token: 0x06006B0D RID: 27405 RVA: 0x002AE8E0 File Offset: 0x002ACAE0
		private void ApplyImpactInternal(EmotionImpactData impact, string sourceId, EmotionSourceCategory category)
		{
			if (this._isFrozen)
			{
				return;
			}
			EmotionImpactData emotionImpactData = this.ApplyModifiersToImpact(impact);
			float emotionBalance = this.EmotionBalance;
			float value = emotionImpactData.Value;
			if (emotionImpactData.ImpactType == EmotionImpactType.SetTo)
			{
				this.EmotionBalance = value;
			}
			else
			{
				switch (emotionImpactData.ApplicationMode)
				{
				case EmotionApplicationMode.Instant:
					this.EmotionBalance += value;
					break;
				case EmotionApplicationMode.PerSecond:
					this.EmotionBalance += value * Time.deltaTime;
					break;
				case EmotionApplicationMode.OverDuration:
					this.EmotionBalance += value;
					break;
				}
			}
			this._lastImpactTime = Time.time;
			if (!Mathf.Approximately(emotionBalance, this.EmotionBalance))
			{
				this.NotifyChange(emotionBalance, sourceId, category);
				if (this.EnableDebugLog)
				{
					string text = ((emotionImpactData.ImpactType == EmotionImpactType.SetTo) ? "SetTo" : "Delta");
					Debug.Log(string.Format("[EmotionManager] {0} from '{1}': Balance {2:F1} -> {3:F1} (Shame={4:F1}, Exhib={5:F1})", new object[] { text, sourceId, emotionBalance, this.EmotionBalance, this.ShameLevel, this.ExhibitionLevel }));
				}
			}
		}

		// Token: 0x06006B0E RID: 27406 RVA: 0x002AEA00 File Offset: 0x002ACC00
		private EmotionImpactData ApplyModifiersToImpact(EmotionImpactData impact)
		{
			EmotionImpactData emotionImpactData = impact;
			foreach (EmotionModifier emotionModifier in this._activeModifiers)
			{
				emotionImpactData = emotionModifier.ApplyToImpact(emotionImpactData);
			}
			return emotionImpactData;
		}

		// Token: 0x06006B0F RID: 27407 RVA: 0x002AEA58 File Offset: 0x002ACC58
		private void UpdateContinuousSources()
		{
			foreach (KeyValuePair<string, IContinuousEmotionSource> keyValuePair in this._continuousSources)
			{
				IContinuousEmotionSource value = keyValuePair.Value;
				if (value.IsActive)
				{
					EmotionImpactData continuousImpact = value.GetContinuousImpact();
					if (continuousImpact != null && !Mathf.Approximately(continuousImpact.Value, 0f))
					{
						this.ApplyImpactInternal(continuousImpact, value.SourceId, value.Category);
					}
				}
			}
		}

		// Token: 0x06006B10 RID: 27408 RVA: 0x002AEAE4 File Offset: 0x002ACCE4
		private void UpdateModifiers()
		{
			for (int i = this._activeModifiers.Count - 1; i >= 0; i--)
			{
				if (this._activeModifiers[i].IsExpired)
				{
					EmotionModifier emotionModifier = this._activeModifiers[i];
					this._activeModifiers.RemoveAt(i);
					Action<EmotionModifier> onModifierRemoved = this.OnModifierRemoved;
					if (onModifierRemoved != null)
					{
						onModifierRemoved(emotionModifier);
					}
					if (this.EnableDebugLog)
					{
						Debug.Log("[EmotionManager] Modifier expired: " + emotionModifier.DisplayName);
					}
				}
			}
		}

		// Token: 0x06006B11 RID: 27409 RVA: 0x002AEB64 File Offset: 0x002ACD64
		private void ApplyDecay()
		{
			float emotionBalance = this.EmotionBalance;
			float num = 1f;
			foreach (EmotionModifier emotionModifier in this._activeModifiers)
			{
				num *= emotionModifier.DecayMultiplier;
			}
			float num2 = ((this._emotionBalance > 0f) ? this.StanceRestingLevel : ((this._emotionBalance < 0f) ? (-this.StanceRestingLevel) : 0f));
			if (!Mathf.Approximately(this.EmotionBalance, num2))
			{
				float num3 = this.DecayRate * num * Time.deltaTime;
				if (this.EmotionBalance > num2)
				{
					this.EmotionBalance = Mathf.Max(num2, this.EmotionBalance - num3);
				}
				else
				{
					this.EmotionBalance = Mathf.Min(num2, this.EmotionBalance + num3);
				}
				if (!Mathf.Approximately(emotionBalance, this.EmotionBalance))
				{
					this.NotifyChange(emotionBalance, "Decay", EmotionSourceCategory.System);
				}
			}
		}

		// Token: 0x06006B12 RID: 27410 RVA: 0x002AEC68 File Offset: 0x002ACE68
		private void NotifyChange(float oldBalance, string sourceId, EmotionSourceCategory category)
		{
			UnityEvent<float, float, float> onEmotionBalanceChanged = this.OnEmotionBalanceChanged;
			if (onEmotionBalanceChanged != null)
			{
				onEmotionBalanceChanged.Invoke(this.EmotionBalance, this.ShameLevel, this.ExhibitionLevel);
			}
			EmotionChangedEventArgs e = new EmotionChangedEventArgs(oldBalance, this.EmotionBalance, sourceId, category);
			Action<EmotionChangedEventArgs> onEmotionChanged = this.OnEmotionChanged;
			if (onEmotionChanged != null)
			{
				onEmotionChanged(e);
			}
			EmotionStance emotionStance = ((oldBalance < 0f) ? EmotionStance.Embarassed : EmotionStance.Confident);
			EmotionStance currentStance = this.CurrentStance;
			if (emotionStance != currentStance)
			{
				Action<EmotionStance> onStanceChanged = this.OnStanceChanged;
				if (onStanceChanged == null)
				{
					return;
				}
				onStanceChanged(currentStance);
			}
		}

		// Token: 0x04006D99 RID: 28057
		[Header("=== EMOTION BALANCE ===")]
		[Tooltip("Current emotion balance: -100 (Shame) to +100 (Exhibition)")]
		[Range(-100f, 100f)]
		[SerializeField]
		private float _emotionBalance;

		// Token: 0x04006D9A RID: 28058
		private EmotionStance? _stanceLock;

		// Token: 0x04006D9B RID: 28059
		[Header("=== BASE VALUE ===")]
		[Tooltip("Initial emotion balance at Start and value used by ResetToBaseValues (game-over). NOT the decay target anymore: decay rests at StanceRestingLevel on the current side.")]
		[Range(-100f, 100f)]
		public float BaseBalance;

		// Token: 0x04006D9C RID: 28060
		[Header("=== DECAY SETTINGS ===")]
		[Tooltip("Enable automatic emotion decay towards the current stance resting level")]
		public bool EnableDecay = true;

		// Token: 0x04006D9D RID: 28061
		[Tooltip("Decay rate per second (how fast balance returns to resting level)")]
		[Range(0f, 20f)]
		public float DecayRate = 2f;

		// Token: 0x04006D9E RID: 28062
		[Tooltip("Delay after last impact before decay starts (seconds)")]
		[Range(0f, 30f)]
		public float DecayStartDelay = 5f;

		// Token: 0x04006D9F RID: 28063
		[Header("=== STANCE SWITCH ===")]
		[Tooltip("Allow the player to toggle stance (Confident <-> Embarassed) via input")]
		public bool EnableStanceSwitchInput = true;

		// Token: 0x04006DA0 RID: 28064
		[Tooltip("Input that toggles the stance")]
		public GenericInput StanceSwitchInput = new GenericInput("Tab", "RightStickClick", "RightStickClick");

		// Token: 0x04006DA1 RID: 28065
		[Tooltip("Balance the switch jumps to: +value for Confident, -value for Embarassed (base level, not Supreme)")]
		[Range(0f, 100f)]
		public float StanceSwitchLevel = 80f;

		// Token: 0x04006DA2 RID: 28066
		[Tooltip("Intensity where decay rests WITHOUT ever crossing to the other stance (e.g. 1 = balance decays to +1 or -1)")]
		[Range(0f, 20f)]
		public float StanceRestingLevel = 1f;

		// Token: 0x04006DA3 RID: 28067
		[Header("=== REFERENCES ===")]
		[Tooltip("Reference to ClothingToggleController")]
		public ClothingToggleController ClothingController;

		// Token: 0x04006DA4 RID: 28068
		[Header("=== DEBUG ===")]
		public bool EnableDebugLog;

		// Token: 0x04006DA5 RID: 28069
		[Header("=== EVENTS ===")]
		[Tooltip("Fired when emotion balance changes. Parameters: (balance, shameLevel, exhibitionLevel)")]
		public UnityEvent<float, float, float> OnEmotionBalanceChanged;

		// Token: 0x04006DAA RID: 28074
		private List<EmotionModifier> _activeModifiers = new List<EmotionModifier>();

		// Token: 0x04006DAB RID: 28075
		private float _lastImpactTime = -999f;

		// Token: 0x04006DAC RID: 28076
		private bool _isInActiveEvent;

		// Token: 0x04006DAD RID: 28077
		private bool _isFrozen;

		// Token: 0x04006DAE RID: 28078
		private bool _decaySuspended;

		// Token: 0x04006DAF RID: 28079
		private Dictionary<string, IContinuousEmotionSource> _continuousSources = new Dictionary<string, IContinuousEmotionSource>();
	}
}
