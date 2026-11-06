using System;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012D9 RID: 4825
	[RequireComponent(typeof(StealthActor))]
	public class PlayerStealthBridge : MonoBehaviour
	{
		// Token: 0x0600957F RID: 38271 RVA: 0x003D5D02 File Offset: 0x003D3F02
		public static void SetConcealmentSource(object source, float coverage)
		{
			if (source == null)
			{
				return;
			}
			if (coverage > 0f)
			{
				PlayerStealthBridge._concealmentSources[source] = Mathf.Clamp(coverage, 0f, 0.95f);
				return;
			}
			PlayerStealthBridge._concealmentSources.Remove(source);
		}

		// Token: 0x170017DF RID: 6111
		// (get) Token: 0x06009580 RID: 38272 RVA: 0x003D5D38 File Offset: 0x003D3F38
		public static float ConcealmentVisibilityMultiplier
		{
			get
			{
				float num = 0f;
				foreach (KeyValuePair<object, float> keyValuePair in PlayerStealthBridge._concealmentSources)
				{
					Object @object = keyValuePair.Key as Object;
					if (@object == null || !(@object == null))
					{
						num = Mathf.Max(num, keyValuePair.Value);
					}
				}
				return 1f - num;
			}
		}

		// Token: 0x06009581 RID: 38273 RVA: 0x003D5DB8 File Offset: 0x003D3FB8
		public static void SetHiddenSource(object source, bool hidden)
		{
			if (source == null)
			{
				return;
			}
			if (hidden)
			{
				PlayerStealthBridge._hiddenSources.Add(source);
				return;
			}
			PlayerStealthBridge._hiddenSources.Remove(source);
		}

		// Token: 0x170017E0 RID: 6112
		// (get) Token: 0x06009582 RID: 38274 RVA: 0x003D5DDA File Offset: 0x003D3FDA
		public static bool IsFullyHidden
		{
			get
			{
				return PlayerStealthBridge._hiddenSources.Count > 0;
			}
		}

		// Token: 0x06009583 RID: 38275 RVA: 0x003D5DE9 File Offset: 0x003D3FE9
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetHiddenSources()
		{
			PlayerStealthBridge._hiddenSources.Clear();
			PlayerStealthBridge._concealmentSources.Clear();
		}

		// Token: 0x06009584 RID: 38276 RVA: 0x003D5DFF File Offset: 0x003D3FFF
		private void Awake()
		{
			this._stealthActor = base.GetComponent<StealthActor>();
			this._stealthActor.UsePlayerConcealment = true;
			if (this.InvectorController == null)
			{
				this.InvectorController = base.GetComponentInParent<vThirdPersonController>();
			}
		}

		// Token: 0x06009585 RID: 38277 RVA: 0x003D5E33 File Offset: 0x003D4033
		private void Start()
		{
			if (SensoryManager.Instance != null)
			{
				SensoryManager.Instance.SetPlayer(this._stealthActor);
			}
		}

		// Token: 0x06009586 RID: 38278 RVA: 0x003D5E52 File Offset: 0x003D4052
		private void Update()
		{
			this._detectionNippleResponse.Update(this._stealthActor, SensoryManager.Instance);
			this.UpdateStealthInput();
		}

		// Token: 0x06009587 RID: 38279 RVA: 0x003D5E70 File Offset: 0x003D4070
		private void UpdateStealthInput()
		{
			if (this._stealthActor == null)
			{
				return;
			}
			if (this.InvectorController == null)
			{
				this._stealthActor.SetLocomotionNoiseEnabled(false);
				return;
			}
			if (this._inputController != this.InvectorController)
			{
				this._inputController = this.InvectorController;
				this._playerInput = this.InvectorController.GetComponent<vThirdPersonInput>();
			}
			float inputMagnitude = this.InvectorController.inputMagnitude;
			bool isCrouching = this.InvectorController.isCrouching;
			bool flag = PlayerStealthBridge._hiddenSources.Count > 0;
			Vector3 input = this.InvectorController.input;
			bool flag2 = this.InvectorController.isActiveAndEnabled && !this.InvectorController.lockMovement && !this.InvectorController.lockAnimMovement && !this.InvectorController.customAction && !this.InvectorController.ragdolled && (this._playerInput == null || (this._playerInput.isActiveAndEnabled && !this._playerInput.lockInput && !this._playerInput.lockMoveInput));
			bool flag3 = input.x * input.x + input.z * input.z >= 0.01f;
			float num;
			bool flag4 = this._vehicleMotion.TryGetPlanarSpeed(this.InvectorController, out num);
			float num2 = ((flag4 && num > this.VehicleNoiseSpeedThreshold) ? Mathf.Clamp01(num / Mathf.Max(0.01f, this.VehicleFullNoiseSpeed)) : 0f);
			this._stealthActor.SetInputState(inputMagnitude, isCrouching, flag, !flag4 && flag2 && flag3, num2);
		}

		// Token: 0x06009588 RID: 38280 RVA: 0x003D6011 File Offset: 0x003D4211
		private void OnDisable()
		{
			this._detectionNippleResponse.Clear();
			if (this._stealthActor != null)
			{
				this._stealthActor.SetLocomotionNoiseEnabled(false);
			}
		}

		// Token: 0x06009589 RID: 38281 RVA: 0x003D6038 File Offset: 0x003D4238
		public void ToggleSocks(bool equipped)
		{
			this._isWearingSocks = equipped;
			if (equipped)
			{
				this._stealthActor.AddModifier(this.SocksModifier);
				return;
			}
			this._stealthActor.RemoveModifier(this.SocksModifier);
		}

		// Token: 0x04009AA5 RID: 39589
		[Header("References")]
		public vThirdPersonController InvectorController;

		// Token: 0x04009AA6 RID: 39590
		private StealthActor _stealthActor;

		// Token: 0x04009AA7 RID: 39591
		private vThirdPersonController _inputController;

		// Token: 0x04009AA8 RID: 39592
		private vThirdPersonInput _playerInput;

		// Token: 0x04009AA9 RID: 39593
		private readonly RiddenVehicleMotion _vehicleMotion = new RiddenVehicleMotion();

		// Token: 0x04009AAA RID: 39594
		private readonly DetectionNippleResponse _detectionNippleResponse = new DetectionNippleResponse();

		// Token: 0x04009AAB RID: 39595
		[Header("Vehicle Noise")]
		[Tooltip("Horizontal speed in m/s below which vehicle settling is silent.")]
		[Min(0f)]
		public float VehicleNoiseSpeedThreshold = 0.15f;

		// Token: 0x04009AAC RID: 39596
		[Tooltip("Horizontal speed in m/s that reaches the configured run noise radius.")]
		[Min(0.01f)]
		public float VehicleFullNoiseSpeed = 6f;

		// Token: 0x04009AAD RID: 39597
		[Header("Items Configuration")]
		[Tooltip("Example modifier: Socks reduce noise")]
		public StealthModifier SocksModifier = new StealthModifier
		{
			SourceName = "Socks",
			NoiseMultiplier = 0.5f,
			VisibilityMultiplier = 1f
		};

		// Token: 0x04009AAE RID: 39598
		private bool _isWearingSocks;

		// Token: 0x04009AAF RID: 39599
		private static readonly HashSet<object> _hiddenSources = new HashSet<object>();

		// Token: 0x04009AB0 RID: 39600
		private static readonly Dictionary<object, float> _concealmentSources = new Dictionary<object, float>();
	}
}
