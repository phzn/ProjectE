using System;
using UnityEngine;
using VehicleSpawnPlugin;

namespace Features.Vehicles.Traffic
{
	// Token: 0x02000BAD RID: 2989
	[DisallowMultipleComponent]
	public class VehicleLightsController : MonoBehaviour
	{
		// Token: 0x17000E46 RID: 3654
		// (get) Token: 0x060055B1 RID: 21937 RVA: 0x002142DD File Offset: 0x002124DD
		public bool LightsOn
		{
			get
			{
				return this._lightsOn;
			}
		}

		// Token: 0x060055B2 RID: 21938 RVA: 0x002142E8 File Offset: 0x002124E8
		private void Awake()
		{
			this._vehicle = base.GetComponent<VehicleAI>();
			if (this._vehicle == null)
			{
				this._vehicle = base.GetComponentInParent<VehicleAI>();
			}
			this._mpb = new MaterialPropertyBlock();
			this._emissionOn = this.spotColor * Mathf.LinearToGammaSpace(1f);
		}

		// Token: 0x060055B3 RID: 21939 RVA: 0x00214341 File Offset: 0x00212541
		private void OnEnable()
		{
			if (!this._isRegistered)
			{
				VehicleLightsManager.Register(this);
				this._isRegistered = true;
			}
		}

		// Token: 0x060055B4 RID: 21940 RVA: 0x00214358 File Offset: 0x00212558
		private void OnDisable()
		{
			if (this._isRegistered)
			{
				VehicleLightsManager.Unregister(this);
				this._isRegistered = false;
			}
			this._flashUntil = -1f;
			this.ApplySteady(false);
		}

		// Token: 0x17000E47 RID: 3655
		// (get) Token: 0x060055B5 RID: 21941 RVA: 0x00008891 File Offset: 0x00006A91
		public Vector3 Position
		{
			get
			{
				return base.transform.position;
			}
		}

		// Token: 0x060055B6 RID: 21942 RVA: 0x00214381 File Offset: 0x00212581
		public void SetLightsEnabled(bool on)
		{
			if (this._lightsOn == on)
			{
				this.AimHeadlights();
				return;
			}
			this._lightsOn = on;
			this.ApplySteady(on);
		}

		// Token: 0x060055B7 RID: 21943 RVA: 0x002143A4 File Offset: 0x002125A4
		public void AimHeadlights()
		{
			if (!this._rigBuilt || !this._lightsOn)
			{
				return;
			}
			Vector3 vector = ((this._vehicle != null) ? this._vehicle.TravelDirection : base.transform.forward);
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = base.transform.forward;
			}
			vector.Normalize();
			Vector3 normalized = Vector3.Cross(Vector3.up, vector).normalized;
			Vector3 vector2 = base.transform.position + vector * this.headlightForward + Vector3.up * this.headlightHeight;
			this._leftSpotTf.position = vector2 - normalized * this.headlightSideOffset;
			this._rightSpotTf.position = vector2 + normalized * this.headlightSideOffset;
			Quaternion quaternion = Quaternion.LookRotation(vector, Vector3.up);
			this._leftSpotTf.rotation = quaternion;
			this._rightSpotTf.rotation = quaternion;
		}

		// Token: 0x060055B8 RID: 21944 RVA: 0x002144AF File Offset: 0x002126AF
		public void FlashHeadlights(float duration)
		{
			if (!this._lightsOn)
			{
				return;
			}
			this._flashUntil = Time.time + Mathf.Max(0.05f, duration);
			this.SetIntensity(this.spotIntensity * this.flashIntensityMultiplier);
		}

		// Token: 0x060055B9 RID: 21945 RVA: 0x002144E4 File Offset: 0x002126E4
		public void TickFlash()
		{
			if (this._flashUntil < 0f)
			{
				return;
			}
			if (Time.time >= this._flashUntil)
			{
				this._flashUntil = -1f;
				this.SetIntensity(this.spotIntensity);
			}
		}

		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x060055BA RID: 21946 RVA: 0x00214518 File Offset: 0x00212718
		public bool HasPendingFlash
		{
			get
			{
				return this._flashUntil >= 0f;
			}
		}

		// Token: 0x060055BB RID: 21947 RVA: 0x0021452C File Offset: 0x0021272C
		private void ApplySteady(bool on)
		{
			if (on && !this._rigBuilt)
			{
				this.BuildRig();
			}
			if (this._leftSpot != null)
			{
				this._leftSpot.enabled = on;
			}
			if (this._rightSpot != null)
			{
				this._rightSpot.enabled = on;
			}
			if (on)
			{
				this.SetIntensity(this.spotIntensity);
				this.AimHeadlights();
			}
			this.UpdateEmissive(on);
		}

		// Token: 0x060055BC RID: 21948 RVA: 0x0021459A File Offset: 0x0021279A
		private void SetIntensity(float value)
		{
			if (this._leftSpot != null)
			{
				this._leftSpot.intensity = value;
			}
			if (this._rightSpot != null)
			{
				this._rightSpot.intensity = value;
			}
		}

		// Token: 0x060055BD RID: 21949 RVA: 0x002145D0 File Offset: 0x002127D0
		private void BuildRig()
		{
			this._leftSpot = this.CreateSpot("Headlight_L");
			this._rightSpot = this.CreateSpot("Headlight_R");
			this._leftSpotTf = this._leftSpot.transform;
			this._rightSpotTf = this._rightSpot.transform;
			this._rigBuilt = true;
		}

		// Token: 0x060055BE RID: 21950 RVA: 0x00214628 File Offset: 0x00212828
		private Light CreateSpot(string goName)
		{
			GameObject gameObject = new GameObject(goName);
			gameObject.transform.SetParent(base.transform, false);
			Light light = gameObject.AddComponent<Light>();
			light.type = LightType.Spot;
			light.range = this.spotRange;
			light.spotAngle = this.spotAngle;
			light.intensity = this.spotIntensity;
			light.color = this.spotColor;
			light.shadows = LightShadows.None;
			light.renderMode = LightRenderMode.ForcePixel;
			light.enabled = false;
			return light;
		}

		// Token: 0x060055BF RID: 21951 RVA: 0x002146A0 File Offset: 0x002128A0
		private void UpdateEmissive(bool on)
		{
			if (this.emissiveRenderer == null)
			{
				return;
			}
			this.emissiveRenderer.GetPropertyBlock(this._mpb, this.emissiveMaterialIndex);
			this._mpb.SetColor(VehicleLightsController.EmissionColor, on ? this._emissionOn : Color.black);
			this.emissiveRenderer.SetPropertyBlock(this._mpb, this.emissiveMaterialIndex);
		}

		// Token: 0x0400560F RID: 22031
		[Header("Headlight rig")]
		[Tooltip("Local offset of the two headlights from the car root (forward = TravelDirection). X = half track (left/right pair), Y = height, Z = how far forward along the bumper.")]
		[SerializeField]
		private float headlightSideOffset = 0.7f;

		// Token: 0x04005610 RID: 22032
		[SerializeField]
		private float headlightHeight = 0.6f;

		// Token: 0x04005611 RID: 22033
		[SerializeField]
		private float headlightForward = 2.2f;

		// Token: 0x04005612 RID: 22034
		[Header("Spot look")]
		[SerializeField]
		private float spotRange = 18f;

		// Token: 0x04005613 RID: 22035
		[SerializeField]
		private float spotAngle = 55f;

		// Token: 0x04005614 RID: 22036
		[SerializeField]
		private float spotIntensity = 6f;

		// Token: 0x04005615 RID: 22037
		[SerializeField]
		private Color spotColor = new Color(1f, 0.96f, 0.85f);

		// Token: 0x04005616 RID: 22038
		[Header("Emissive (optional)")]
		[Tooltip("Renderer whose emission is driven with the lights (the headlight glass). Optional.")]
		[SerializeField]
		private Renderer emissiveRenderer;

		// Token: 0x04005617 RID: 22039
		[SerializeField]
		private int emissiveMaterialIndex;

		// Token: 0x04005618 RID: 22040
		[Header("Flash (driver reaction)")]
		[Tooltip("Intensity multiplier during a horn/flash blink.")]
		[SerializeField]
		private float flashIntensityMultiplier = 2.2f;

		// Token: 0x04005619 RID: 22041
		private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

		// Token: 0x0400561A RID: 22042
		private VehicleAI _vehicle;

		// Token: 0x0400561B RID: 22043
		private Transform _leftSpotTf;

		// Token: 0x0400561C RID: 22044
		private Transform _rightSpotTf;

		// Token: 0x0400561D RID: 22045
		private Light _leftSpot;

		// Token: 0x0400561E RID: 22046
		private Light _rightSpot;

		// Token: 0x0400561F RID: 22047
		private MaterialPropertyBlock _mpb;

		// Token: 0x04005620 RID: 22048
		private Color _emissionOn;

		// Token: 0x04005621 RID: 22049
		private bool _rigBuilt;

		// Token: 0x04005622 RID: 22050
		private bool _lightsOn;

		// Token: 0x04005623 RID: 22051
		private bool _isRegistered;

		// Token: 0x04005624 RID: 22052
		private float _flashUntil = -1f;
	}
}
