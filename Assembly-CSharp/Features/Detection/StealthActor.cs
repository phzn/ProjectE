using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012EB RID: 4843
	public class StealthActor : MonoBehaviour
	{
		// Token: 0x170017FA RID: 6138
		// (get) Token: 0x06009637 RID: 38455 RVA: 0x003DA8C6 File Offset: 0x003D8AC6
		public float FinalVisibility
		{
			get
			{
				return this._finalVisibility;
			}
		}

		// Token: 0x170017FB RID: 6139
		// (get) Token: 0x06009638 RID: 38456 RVA: 0x003DA8CE File Offset: 0x003D8ACE
		public float FinalNoiseRadius
		{
			get
			{
				return this._finalNoiseRadius;
			}
		}

		// Token: 0x170017FC RID: 6140
		// (get) Token: 0x06009639 RID: 38457 RVA: 0x003DA8D8 File Offset: 0x003D8AD8
		public float ExternalNoiseRadius
		{
			get
			{
				float num = 0f;
				this._staleExternalNoiseOwners.Clear();
				foreach (KeyValuePair<Object, float> keyValuePair in this._externalNoise)
				{
					if (keyValuePair.Key == null)
					{
						this._staleExternalNoiseOwners.Add(keyValuePair.Key);
					}
					else if (keyValuePair.Value > num)
					{
						num = keyValuePair.Value;
					}
				}
				for (int i = 0; i < this._staleExternalNoiseOwners.Count; i++)
				{
					this._externalNoise.Remove(this._staleExternalNoiseOwners[i]);
				}
				return num;
			}
		}

		// Token: 0x0600963A RID: 38458 RVA: 0x003DA99C File Offset: 0x003D8B9C
		public void SetExternalNoise(Object owner, float radius)
		{
			if (owner == null)
			{
				return;
			}
			if (radius > 0f)
			{
				this._externalNoise[owner] = radius;
				return;
			}
			this._externalNoise.Remove(owner);
		}

		// Token: 0x170017FD RID: 6141
		// (get) Token: 0x0600963B RID: 38459 RVA: 0x003DA9C5 File Offset: 0x003D8BC5
		// (set) Token: 0x0600963C RID: 38460 RVA: 0x003DA9CD File Offset: 0x003D8BCD
		public bool UsePlayerConcealment { get; set; }

		// Token: 0x170017FE RID: 6142
		// (get) Token: 0x0600963D RID: 38461 RVA: 0x003DA9D6 File Offset: 0x003D8BD6
		public Transform VisibilityInteractionOccluder
		{
			get
			{
				if (!(this._visibilityOverrideOwner != null))
				{
					return null;
				}
				return this._visibilityInteractionOccluder;
			}
		}

		// Token: 0x170017FF RID: 6143
		// (get) Token: 0x0600963E RID: 38462 RVA: 0x003DA9F0 File Offset: 0x003D8BF0
		public Vector3 Position
		{
			get
			{
				Vector3 vector = (this.VisibilityPoint ? this.VisibilityPoint.position : base.transform.position);
				if (this._visibilityOverrideOwner == null)
				{
					return vector;
				}
				if (this._visibilityOverridePoint != null && this._visibilityOverridePoint.position.y > vector.y)
				{
					vector = this._visibilityOverridePoint.position;
				}
				if (this._visibilityOverrideAlternate != null && this._visibilityOverrideAlternate.position.y > vector.y)
				{
					vector = this._visibilityOverrideAlternate.position;
				}
				return vector;
			}
		}

		// Token: 0x0600963F RID: 38463 RVA: 0x003DAA98 File Offset: 0x003D8C98
		public void SetVisibilityPointOverride(Object owner, Transform point, Transform alternate = null, Transform interactionOccluder = null)
		{
			if (owner == null)
			{
				return;
			}
			this._visibilityOverrideOwner = owner;
			this._visibilityOverridePoint = point;
			this._visibilityOverrideAlternate = alternate;
			this._visibilityInteractionOccluder = interactionOccluder;
		}

		// Token: 0x06009640 RID: 38464 RVA: 0x003DAAC1 File Offset: 0x003D8CC1
		public void ClearVisibilityPointOverride(Object owner)
		{
			if (this._visibilityOverrideOwner != owner)
			{
				return;
			}
			this._visibilityOverrideOwner = null;
			this._visibilityOverridePoint = null;
			this._visibilityOverrideAlternate = null;
			this._visibilityInteractionOccluder = null;
		}

		// Token: 0x06009641 RID: 38465 RVA: 0x003DAAEE File Offset: 0x003D8CEE
		public void AddModifier(StealthModifier mod)
		{
			if (!this._modifiers.Contains(mod))
			{
				this._modifiers.Add(mod);
			}
		}

		// Token: 0x06009642 RID: 38466 RVA: 0x003DAB0A File Offset: 0x003D8D0A
		public void RemoveModifier(StealthModifier mod)
		{
			if (this._modifiers.Contains(mod))
			{
				this._modifiers.Remove(mod);
			}
		}

		// Token: 0x06009643 RID: 38467 RVA: 0x003DAB27 File Offset: 0x003D8D27
		public void SetInputState(float inputMag, bool isCrouching, bool isHidden)
		{
			this.SetInputState(inputMag, isCrouching, isHidden, true);
		}

		// Token: 0x06009644 RID: 38468 RVA: 0x003DAB33 File Offset: 0x003D8D33
		public void SetInputState(float inputMag, bool isCrouching, bool isHidden, bool canEmitLocomotionNoise)
		{
			this.SetInputState(inputMag, isCrouching, isHidden, canEmitLocomotionNoise, 0f);
		}

		// Token: 0x06009645 RID: 38469 RVA: 0x003DAB48 File Offset: 0x003D8D48
		public void SetInputState(float inputMag, bool isCrouching, bool isHidden, bool canEmitLocomotionNoise, float vehicleNoiseMagnitude)
		{
			this._currentInputMagnitude = inputMag;
			this._isCrouching = isCrouching;
			this._isHidden = isHidden;
			this._canEmitLocomotionNoise = canEmitLocomotionNoise;
			this._vehicleNoiseMagnitude = Mathf.Clamp01(vehicleNoiseMagnitude);
			if ((!canEmitLocomotionNoise || inputMag < 0.1f) && this._vehicleNoiseMagnitude <= 0f)
			{
				this._finalNoiseRadius = (this._isHidden ? 0f : this.ExternalNoiseRadius);
			}
		}

		// Token: 0x06009646 RID: 38470 RVA: 0x003DABB3 File Offset: 0x003D8DB3
		public void SetLocomotionNoiseEnabled(bool enabled)
		{
			this._canEmitLocomotionNoise = enabled;
			if (!enabled)
			{
				this._vehicleNoiseMagnitude = 0f;
				this._finalNoiseRadius = (this._isHidden ? 0f : this.ExternalNoiseRadius);
			}
		}

		// Token: 0x06009647 RID: 38471 RVA: 0x003DABE5 File Offset: 0x003D8DE5
		private void Update()
		{
			this.CalculateMetrics();
		}

		// Token: 0x06009648 RID: 38472 RVA: 0x003DABF0 File Offset: 0x003D8DF0
		private void CalculateMetrics()
		{
			if (this.GlobalConfig == null)
			{
				return;
			}
			if (this._isHidden)
			{
				this._finalVisibility = 0f;
				this._finalNoiseRadius = 0f;
				return;
			}
			StealthLayerConfig stealthLayerConfig = this.GlobalConfig.WalkLayer;
			if (this._isCrouching)
			{
				stealthLayerConfig = this.GlobalConfig.CrouchLayer;
			}
			else if (this._currentInputMagnitude > 0.8f)
			{
				stealthLayerConfig = this.GlobalConfig.RunLayer;
			}
			else
			{
				stealthLayerConfig = this.GlobalConfig.WalkLayer;
			}
			float num = stealthLayerConfig.BaseVisibility;
			float num2 = ((this._canEmitLocomotionNoise && this._currentInputMagnitude >= 0.1f) ? (stealthLayerConfig.BaseNoiseRadius * Mathf.Clamp01(this._currentInputMagnitude + 0.2f)) : 0f);
			if (this._vehicleNoiseMagnitude > 0f)
			{
				num2 = Mathf.Max(num2, this.GlobalConfig.RunLayer.BaseNoiseRadius * this._vehicleNoiseMagnitude);
			}
			foreach (StealthModifier stealthModifier in this._modifiers)
			{
				num *= stealthModifier.VisibilityMultiplier;
				num2 *= stealthModifier.NoiseMultiplier;
			}
			float externalNoiseRadius = this.ExternalNoiseRadius;
			if (externalNoiseRadius > num2)
			{
				num2 = externalNoiseRadius;
			}
			if (this.UsePlayerConcealment)
			{
				num *= PlayerStealthBridge.ConcealmentVisibilityMultiplier;
			}
			this._finalVisibility = Mathf.Clamp01(num);
			this._finalNoiseRadius = num2;
			if (this.DebugMode && Time.frameCount % 60 == 0)
			{
				Debug.Log(string.Format("[StealthActor] Input: {0:F2} | State: {1} | Vis: {2:F2} | Noise: {3:F2}", new object[]
				{
					this._currentInputMagnitude,
					this._isCrouching ? "Crouch" : "Stand",
					this._finalVisibility,
					this._finalNoiseRadius
				}));
			}
		}

		// Token: 0x06009649 RID: 38473 RVA: 0x003DADC8 File Offset: 0x003D8FC8
		private void OnDrawGizmos()
		{
			if (this._finalNoiseRadius > 0f)
			{
				Gizmos.color = new Color(1f, 0f, 0f, 0.1f);
				Gizmos.DrawSphere(base.transform.position, this._finalNoiseRadius);
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(base.transform.position, this._finalNoiseRadius);
			}
		}

		// Token: 0x04009B51 RID: 39761
		[Header("Configuration")]
		public DetectionConfigSO GlobalConfig;

		// Token: 0x04009B52 RID: 39762
		public Transform VisibilityPoint;

		// Token: 0x04009B53 RID: 39763
		public bool DebugMode;

		// Token: 0x04009B54 RID: 39764
		[Header("Runtime")]
		[SerializeField]
		private float _currentInputMagnitude;

		// Token: 0x04009B55 RID: 39765
		[SerializeField]
		private bool _isCrouching;

		// Token: 0x04009B56 RID: 39766
		[SerializeField]
		private bool _isHidden;

		// Token: 0x04009B57 RID: 39767
		private bool _canEmitLocomotionNoise = true;

		// Token: 0x04009B58 RID: 39768
		private float _vehicleNoiseMagnitude;

		// Token: 0x04009B59 RID: 39769
		[Header("Debug Output")]
		[SerializeField]
		private float _finalVisibility;

		// Token: 0x04009B5A RID: 39770
		[SerializeField]
		private float _finalNoiseRadius;

		// Token: 0x04009B5B RID: 39771
		private List<StealthModifier> _modifiers = new List<StealthModifier>();

		// Token: 0x04009B5C RID: 39772
		private readonly Dictionary<Object, float> _externalNoise = new Dictionary<Object, float>(2);

		// Token: 0x04009B5D RID: 39773
		private readonly List<Object> _staleExternalNoiseOwners = new List<Object>(2);

		// Token: 0x04009B5F RID: 39775
		private Object _visibilityOverrideOwner;

		// Token: 0x04009B60 RID: 39776
		private Transform _visibilityOverridePoint;

		// Token: 0x04009B61 RID: 39777
		private Transform _visibilityOverrideAlternate;

		// Token: 0x04009B62 RID: 39778
		private Transform _visibilityInteractionOccluder;
	}
}
