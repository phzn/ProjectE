using System;
using System.Collections.Generic;
using Invector.vEventSystems;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vShooter
{
	// Token: 0x0200066C RID: 1644
	[vClassHeader("Projectile Control", "The damage value is changed from minDamage, maxDamage, DropOffStart, DropOffEnd of the ShooterWeapon", openClose = false)]
	public class vProjectileControl : vMonoBehaviour
	{
		// Token: 0x060025EC RID: 9708 RVA: 0x000D32C0 File Offset: 0x000D14C0
		protected virtual void Start()
		{
			base.transform.SetParent(vObjectContainer.root, true);
			this.debugLife = this.bulletLife;
			this._rigidBody = base.GetComponent<Rigidbody>();
			this.startPosition = base.transform.position;
			this.previousPosition = base.transform.position - base.transform.forward * 0.1f;
			if (this.trail)
			{
				this.AddTrailPosition();
			}
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x000D3348 File Offset: 0x000D1548
		protected virtual void Update()
		{
			if (this._rigidBody.velocity.magnitude > 1f)
			{
				base.transform.rotation = Quaternion.LookRotation(this._rigidBody.velocity.normalized, base.transform.up);
			}
			RaycastHit raycastHit;
			if (Physics.Linecast(this.previousPosition, base.transform.position + base.transform.forward * 0.5f, out raycastHit, this.hitLayer))
			{
				if (!raycastHit.collider)
				{
					return;
				}
				float num = Vector3.Distance(this.startPosition, base.transform.position) + this.castDist;
				if (!this.ignoreTags.Contains(raycastHit.collider.gameObject.tag) && (!(this.shooterTransform != null) || !raycastHit.collider.transform.IsChildOf(this.shooterTransform)))
				{
					if (this.debugHittedObject)
					{
						Debug.Log(raycastHit.collider.gameObject.name, raycastHit.collider);
					}
					this.onCastCollider.Invoke(raycastHit);
					this.damage.damageValue = (float)this.maxDamage;
					if (this.damageByDistance)
					{
						int num2 = this.maxDamage - this.minDamage;
						if (num - this.minDamageDistance >= 0f)
						{
							float num3 = Mathf.Clamp((float)((int)Math.Round((double)(100f * (num - this.minDamageDistance)) / (double)(this.maxDamageDistance - this.minDamageDistance))) * 0.01f, 0f, 1f);
							this.damage.damageValue = (float)(this.maxDamage - (int)((float)num2 * num3));
						}
						else
						{
							this.damage.damageValue = (float)this.maxDamage;
						}
					}
					this.damage.hitPosition = raycastHit.point;
					this.damage.receiver = raycastHit.collider.transform;
					this.damage.force = base.transform.forward * this.damage.damageValue * this.forceMultiplier;
					if (this.damage.damageValue > 0f)
					{
						this.onPassDamage.Invoke(this.damage);
						raycastHit.collider.gameObject.ApplyDamage(this.damage, this.damage.sender ? this.damage.sender.GetComponent<vIMeleeFighter>() : null);
					}
					Rigidbody component = raycastHit.collider.gameObject.GetComponent<Rigidbody>();
					if (component)
					{
						component.AddForce(base.transform.forward * this.damage.damageValue * this.forceMultiplier, ForceMode.Impulse);
					}
					this.startPosition = base.transform.position;
					this.castDist = num;
					if (this.destroyOnCast)
					{
						if (this.bulletLifeSettings)
						{
							vBulletLifeSettings.vBulletLifeInfo reduceLife = this.bulletLifeSettings.GetReduceLife(raycastHit.collider.gameObject.tag, raycastHit.collider.gameObject.layer);
							this.bulletLife -= reduceLife.lostLife;
							if (this.debugTrajetory)
							{
								this.DrawHitPoint(raycastHit.point);
							}
							bool flag = false;
							if (this.bulletLife > 0 && !reduceLife.ricochet)
							{
								Vector3 vector = (base.transform.position = raycastHit.point + base.transform.forward * 0.001f);
								if (this.trail)
								{
									this.trail.AddPosition(base.transform.position);
								}
								if (this.debugTrajetory)
								{
									Debug.DrawLine(base.transform.position, this.previousPosition, this.debugColor, 10f);
								}
								for (float num4 = 0f; num4 <= reduceLife.maxThicknessToCross; num4 += 0.01f)
								{
									Vector3 vector2 = vector + base.transform.forward * num4;
									if (Physics.Linecast(vector2, vector))
									{
										raycastHit.point = vector2;
										raycastHit.normal = base.transform.forward;
										this.onCastCollider.Invoke(raycastHit);
										flag = true;
										break;
									}
								}
								if (flag && this.trail)
								{
									this.AddTrailPosition();
								}
							}
							if (!flag && !reduceLife.ricochet)
							{
								this.bulletLife = 0;
								base.transform.position = raycastHit.point;
								if (this.debugTrajetory)
								{
									Debug.DrawLine(base.transform.position, this.previousPosition, this.debugColor, 10f);
								}
								this.onDestroyProjectile.Invoke(raycastHit);
								if (this.trail && this.trail.gameObject != base.gameObject)
								{
									if (this.trail)
									{
										this.AddTrailPosition();
									}
									this.trail.transform.SetParent(vObjectContainer.root);
								}
								Object.Destroy(base.gameObject);
								return;
							}
							this.maxDamage -= this.maxDamage - this.maxDamage * reduceLife.lostDamage / 100;
							this.minDamage -= this.minDamage - this.minDamage * reduceLife.lostDamage / 100;
							if (this.maxDamage < 0)
							{
								this.maxDamage = 0;
							}
							if (this.minDamage < 0)
							{
								this.minDamage = 0;
							}
							float num5 = Random.Range(reduceLife.minChangeTrajectory, reduceLife.maxChangeTrajectory) * (float)((Random.Range(-1, 1) >= 0) ? 1 : (-1));
							float num6 = Random.Range(reduceLife.minChangeTrajectory, reduceLife.maxChangeTrajectory) * (float)((Random.Range(-1, 1) >= 0) ? 1 : (-1));
							if (num6 > 60f || num6 < -60f)
							{
								num5 = Mathf.Clamp(num5, -15f, 15f);
							}
							if (num5 != 0f || num6 != 0f)
							{
								Vector3 vector3 = Quaternion.Euler(num5, num6, 0f) * this._rigidBody.velocity;
								if (vector3 != Vector3.zero)
								{
									this._rigidBody.velocity = vector3 * (float)(reduceLife.ricochet ? (-1) : 1);
									base.transform.forward = vector3 * (float)(reduceLife.ricochet ? (-1) : 1);
								}
							}
							if (this.debugTrajetory)
							{
								float num7 = (float)this.bulletLife / (float)this.debugLife * 100f;
								this.debugColor = ((num7 > 76f) ? Color.green : ((num7 > 51f) ? Color.yellow : ((num7 > 26f) ? new Color(1f, 0.5f, 0f) : Color.red)));
								this.debugColor.a = 0.5f;
							}
						}
						else
						{
							this.bulletLife = 0;
						}
						if (this.bulletLife <= 0 || this.bulletLifeSettings == null)
						{
							base.transform.position = raycastHit.point;
							if (this.debugTrajetory)
							{
								Debug.DrawLine(base.transform.position, this.previousPosition, this.debugColor, 10f);
							}
							this.onDestroyProjectile.Invoke(raycastHit);
							if (this.trail && this.trail.gameObject != base.gameObject)
							{
								if (this.trail)
								{
									this.AddTrailPosition();
								}
								this.trail.transform.SetParent(vObjectContainer.root);
							}
							Object.Destroy(base.gameObject);
							return;
						}
					}
				}
				else
				{
					base.transform.position = raycastHit.point + base.transform.forward * 0.001f;
					if (this.trail && this.trail.gameObject != base.gameObject && this.trail)
					{
						this.AddTrailPosition();
					}
					if (this.debugTrajetory)
					{
						Debug.DrawLine(base.transform.position, this.previousPosition, this.debugColor, 10f);
					}
				}
			}
			else if (this.debugTrajetory)
			{
				Debug.DrawLine(base.transform.position, this.previousPosition, this.debugColor, 10f);
			}
			this.previousPosition = base.transform.position;
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x000D3BF8 File Offset: 0x000D1DF8
		private void AddTrailPosition()
		{
			if (this.trajectoryPositions.Count > 0)
			{
				Vector3 vector = this.trajectoryPositions[this.trajectoryPositions.Count - 1];
				float num = Vector3.Distance(vector, base.transform.position);
				Vector3 vector2 = base.transform.position - vector;
				int num2 = (int)(num / 0.5f);
				for (int i = 0; i < num2; i++)
				{
					this.trajectoryPositions.Add(vector + vector2.normalized * 0.5f);
					if (this.debugTrajetory)
					{
						Debug.DrawRay(vector, Vector3.up * 0.1f, Color.red, 10f);
					}
					vector += vector2.normalized * 0.5f;
				}
			}
			else
			{
				this.trajectoryPositions.Add(base.transform.position);
			}
			this.trail.Clear();
			Vector3[] array = this.trajectoryPositions.ToArray();
			this.trail.AddPositions(array);
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x000D3D08 File Offset: 0x000D1F08
		private void DrawHitPoint(Vector3 point)
		{
			Debug.DrawRay(point, -base.transform.forward * 0.1f, Color.red, 10f);
			Debug.DrawRay(point, base.transform.right * 0.1f, Color.red, 10f);
			Debug.DrawRay(point, -base.transform.right * 0.1f, Color.red, 10f);
			Debug.DrawRay(point, base.transform.up * 0.1f, Color.red, 10f);
			Debug.DrawRay(point, -base.transform.up * 0.1f, Color.red, 10f);
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x000D3DDD File Offset: 0x000D1FDD
		public void RemoveParentOfOther(Transform other)
		{
			other.SetParent(vObjectContainer.root, true);
		}

		// Token: 0x040025AA RID: 9642
		public vBulletLifeSettings bulletLifeSettings;

		// Token: 0x040025AB RID: 9643
		public int bulletLife = 100;

		// Token: 0x040025AC RID: 9644
		public bool debugTrajetory;

		// Token: 0x040025AD RID: 9645
		public bool debugHittedObject;

		// Token: 0x040025AE RID: 9646
		public vDamage damage;

		// Token: 0x040025AF RID: 9647
		public float forceMultiplier = 1f;

		// Token: 0x040025B0 RID: 9648
		public bool destroyOnCast = true;

		// Token: 0x040025B1 RID: 9649
		[Tooltip("Control Trail renderer")]
		public TrailRenderer trail;

		// Token: 0x040025B2 RID: 9650
		public vProjectileControl.ProjectilePassDamage onPassDamage;

		// Token: 0x040025B3 RID: 9651
		public vProjectileControl.ProjectileCastColliderEvent onCastCollider;

		// Token: 0x040025B4 RID: 9652
		public vProjectileControl.ProjectileCastColliderEvent onDestroyProjectile;

		// Token: 0x040025B5 RID: 9653
		public vProjectileInstantiateData instantiateData;

		// Token: 0x040025B6 RID: 9654
		internal bool damageByDistance;

		// Token: 0x040025B7 RID: 9655
		internal float velocity = 580f;

		// Token: 0x040025B8 RID: 9656
		internal int minDamage;

		// Token: 0x040025B9 RID: 9657
		internal int maxDamage;

		// Token: 0x040025BA RID: 9658
		internal float minDamageDistance = 8f;

		// Token: 0x040025BB RID: 9659
		internal float maxDamageDistance = 50f;

		// Token: 0x040025BC RID: 9660
		internal Vector3 startPosition;

		// Token: 0x040025BD RID: 9661
		internal LayerMask hitLayer = -1;

		// Token: 0x040025BE RID: 9662
		internal List<string> ignoreTags = new List<string>();

		// Token: 0x040025BF RID: 9663
		internal Transform shooterTransform;

		// Token: 0x040025C0 RID: 9664
		protected Vector3 previousPosition;

		// Token: 0x040025C1 RID: 9665
		protected Rigidbody _rigidBody;

		// Token: 0x040025C2 RID: 9666
		protected Color debugColor = Color.green;

		// Token: 0x040025C3 RID: 9667
		protected int debugLife;

		// Token: 0x040025C4 RID: 9668
		protected float castDist;

		// Token: 0x040025C5 RID: 9669
		protected List<Vector3> trajectoryPositions = new List<Vector3>();

		// Token: 0x0200066D RID: 1645
		[Serializable]
		public class ProjectileCastColliderEvent : UnityEvent<RaycastHit>
		{
		}

		// Token: 0x0200066E RID: 1646
		[Serializable]
		public class ProjectilePassDamage : UnityEvent<vDamage>
		{
		}
	}
}
