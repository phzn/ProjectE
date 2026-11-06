using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000765 RID: 1893
	[SelectionBase]
	public class TireMovementController : MonoBehaviour, ISetColorMap
	{
		// Token: 0x060030F4 RID: 12532 RVA: 0x00113B08 File Offset: 0x00111D08
		private void Start()
		{
			this.sphereRigidBody = this.SpherePhysicsObject.GetComponent<Rigidbody>();
			this.SpherePhysicsObject.transform.parent = null;
			this.TireMaterial.SetFloat("_Mud_Amount", 0f);
			if (!this.StampOverDistance)
			{
				base.TryGetComponent<StampOverDistance>(out this.StampOverDistance);
			}
			this.mudEffectComponents = base.GetComponents<LivingMudEffectsPlayer>().ToList<LivingMudEffectsPlayer>();
		}

		// Token: 0x060030F5 RID: 12533 RVA: 0x00113B78 File Offset: 0x00111D78
		private void Update()
		{
			this.CheckIfInMud();
			base.transform.position = this.SpherePhysicsObject.transform.position;
			this.forwardVelocity = Input.GetAxis("Vertical");
			this.angularForce = Input.GetAxis("Horizontal") * this.TurnForce;
			base.transform.Rotate(Vector3.up, this.angularForce);
			this.RotateGameObject.transform.Rotate(Vector3.right * Input.GetAxis("Vertical") * 3f, Space.Self);
			if (this.inMud && this.traction < this.SpinOutThreshold && !this.spinningOut)
			{
				this.SpinOut();
			}
			if (this.traction > this.SpinOutThreshold && this.spinningOut)
			{
				this.SpinOutStop();
			}
			if (!this.inMud && this.spinningOut)
			{
				this.SpinOutStop();
			}
			if (this.framesTraveled < 100f && this.inMud && Input.GetAxis("Horizontal") != 0f)
			{
				this.framesTraveled += 1f;
				this.TireMaterial.SetFloat("_Mud_Amount", this.framesTraveled * 0.01f);
			}
		}

		// Token: 0x060030F6 RID: 12534 RVA: 0x00113CBC File Offset: 0x00111EBC
		private void CheckIfInMud()
		{
			bool flag = false;
			if (!this.inMud && this.StampOverDistance.InMud)
			{
				flag = true;
			}
			this.inMud = this.StampOverDistance.InMud;
			if (flag && this.surface != this.StampOverDistance.Surface)
			{
				StampOverDistance stampOverDistance = this.StampOverDistance;
				this.surface = ((stampOverDistance != null) ? stampOverDistance.Surface : null);
				if (this.surface == null)
				{
					Debug.LogWarning("Mud Surface is null");
					return;
				}
				this.SetColor(this.surface.GetColor());
				this.SetColorMap(this.surface.GetColorMap());
			}
		}

		// Token: 0x060030F7 RID: 12535 RVA: 0x00113D64 File Offset: 0x00111F64
		private void SpinOut()
		{
			if (this.spinningOut)
			{
				return;
			}
			this.spinningOut = true;
			foreach (LivingMudEffectsPlayer livingMudEffectsPlayer in this.mudEffectComponents)
			{
				if (livingMudEffectsPlayer is ISpinOut)
				{
					livingMudEffectsPlayer.StartEffects(this.surface, this.surface.Thickness, false);
				}
			}
		}

		// Token: 0x060030F8 RID: 12536 RVA: 0x00113DE0 File Offset: 0x00111FE0
		private void SpinOutStop()
		{
			if (!this.spinningOut)
			{
				return;
			}
			this.spinningOut = false;
			foreach (LivingMudEffectsPlayer livingMudEffectsPlayer in this.mudEffectComponents)
			{
				if (livingMudEffectsPlayer is ISpinOut)
				{
					livingMudEffectsPlayer.StopEffects();
				}
			}
		}

		// Token: 0x060030F9 RID: 12537 RVA: 0x00113E4C File Offset: 0x0011204C
		private void FixedUpdate()
		{
			this.sphereRigidBody.velocity = Vector3.Lerp(this.sphereRigidBody.velocity, this.TopSpeed * this.forwardVelocity * base.transform.forward, Time.fixedDeltaTime * this.Responsiveness);
			this.sphereRigidBody.AddForce(-this.sphereRigidBody.velocity.normalized * this.Friction, ForceMode.Acceleration);
			this.traction = this.sphereRigidBody.velocity.magnitude - Mathf.Abs(this.TopSpeed * this.forwardVelocity);
		}

		// Token: 0x060030FA RID: 12538 RVA: 0x00113EF8 File Offset: 0x001120F8
		public void SetColorMap(Texture2D tex)
		{
			if (tex == null)
			{
				return;
			}
			this.SpinOutVFX.SetTexture("Color Map", tex);
		}

		// Token: 0x060030FB RID: 12539 RVA: 0x00113F18 File Offset: 0x00112118
		public void SetColor(Color color)
		{
			if (color.a < 0.1f)
			{
				Debug.LogWarning(string.Format("Setting Effects to a color that has alpha = {0}. Is this intended?", color.a));
			}
			this.SpinOutVFX.SetVector4("Tint", new Vector4(color.r, color.g, color.b, color.a));
			this.MudSprayParticles.main.startColor = color;
		}

		// Token: 0x0400315E RID: 12638
		public float Responsiveness = 3f;

		// Token: 0x0400315F RID: 12639
		public float TopSpeed = 10f;

		// Token: 0x04003160 RID: 12640
		public float TurnForce = 1f;

		// Token: 0x04003161 RID: 12641
		public float SpinForce = 1f;

		// Token: 0x04003162 RID: 12642
		public float SpinOutThreshold = -5f;

		// Token: 0x04003163 RID: 12643
		public float Friction = 1f;

		// Token: 0x04003164 RID: 12644
		public GameObject SpherePhysicsObject;

		// Token: 0x04003165 RID: 12645
		public GameObject RotateGameObject;

		// Token: 0x04003166 RID: 12646
		public ParticleSystem MudSprayParticles;

		// Token: 0x04003167 RID: 12647
		public VisualEffect SpinOutVFX;

		// Token: 0x04003168 RID: 12648
		public Material TireMaterial;

		// Token: 0x04003169 RID: 12649
		public StampOverDistance StampOverDistance;

		// Token: 0x0400316A RID: 12650
		private bool inMud;

		// Token: 0x0400316B RID: 12651
		private DynamicMudSurface surface;

		// Token: 0x0400316C RID: 12652
		private Rigidbody sphereRigidBody;

		// Token: 0x0400316D RID: 12653
		private float forwardVelocity;

		// Token: 0x0400316E RID: 12654
		private float angularForce;

		// Token: 0x0400316F RID: 12655
		private float traction;

		// Token: 0x04003170 RID: 12656
		private bool spinningOut;

		// Token: 0x04003171 RID: 12657
		private float framesTraveled;

		// Token: 0x04003172 RID: 12658
		private List<LivingMudEffectsPlayer> mudEffectComponents;
	}
}
