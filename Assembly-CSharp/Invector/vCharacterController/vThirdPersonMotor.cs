using System;
using System.Collections;
using System.Text;
using Invector.vEventSystems;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Invector.vCharacterController
{
	// Token: 0x020006A1 RID: 1697
	public class vThirdPersonMotor : vCharacter, vIAnimatorStateInfoController
	{
		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06002849 RID: 10313 RVA: 0x000E578E File Offset: 0x000E398E
		// (set) Token: 0x0600284A RID: 10314 RVA: 0x000E5796 File Offset: 0x000E3996
		public virtual float maxStamina
		{
			get
			{
				return this._maxStamina;
			}
			set
			{
				this._maxStamina = value;
			}
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x0600284B RID: 10315 RVA: 0x000E579F File Offset: 0x000E399F
		// (set) Token: 0x0600284C RID: 10316 RVA: 0x000E57A7 File Offset: 0x000E39A7
		public virtual float staminaRecovery
		{
			get
			{
				return this._staminaRecovery;
			}
			set
			{
				this._staminaRecovery = value;
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x0600284D RID: 10317 RVA: 0x000E57B0 File Offset: 0x000E39B0
		// (set) Token: 0x0600284E RID: 10318 RVA: 0x000E57B8 File Offset: 0x000E39B8
		public virtual float sprintStamina
		{
			get
			{
				return this._sprintStamina;
			}
			set
			{
				this._sprintStamina = value;
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x0600284F RID: 10319 RVA: 0x000E57C1 File Offset: 0x000E39C1
		// (set) Token: 0x06002850 RID: 10320 RVA: 0x000E57C9 File Offset: 0x000E39C9
		public virtual float jumpStamina
		{
			get
			{
				return this._jumpStamina;
			}
			set
			{
				this._jumpStamina = value;
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06002851 RID: 10321 RVA: 0x000E57D2 File Offset: 0x000E39D2
		// (set) Token: 0x06002852 RID: 10322 RVA: 0x000E57DA File Offset: 0x000E39DA
		public virtual float rollStamina
		{
			get
			{
				return this._rollStamina;
			}
			set
			{
				this._rollStamina = value;
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06002853 RID: 10323 RVA: 0x000E57E3 File Offset: 0x000E39E3
		// (set) Token: 0x06002854 RID: 10324 RVA: 0x000E57EB File Offset: 0x000E39EB
		public virtual float speedMultiplier
		{
			get
			{
				return this._speedMultiplier;
			}
			set
			{
				this._speedMultiplier = value;
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06002855 RID: 10325 RVA: 0x000E57F4 File Offset: 0x000E39F4
		// (set) Token: 0x06002856 RID: 10326 RVA: 0x000E57FC File Offset: 0x000E39FC
		public virtual vThirdPersonMotor.LocomotionType locomotionType
		{
			get
			{
				return this._locomotionType;
			}
			set
			{
				this._locomotionType = value;
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06002857 RID: 10327 RVA: 0x000E5805 File Offset: 0x000E3A05
		// (set) Token: 0x06002858 RID: 10328 RVA: 0x000E580D File Offset: 0x000E3A0D
		public virtual float jumpHeight
		{
			get
			{
				return this._jumpHeight;
			}
			set
			{
				this._jumpHeight = value;
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06002859 RID: 10329 RVA: 0x000E5816 File Offset: 0x000E3A16
		// (set) Token: 0x0600285A RID: 10330 RVA: 0x000E581E File Offset: 0x000E3A1E
		public virtual float airSpeed
		{
			get
			{
				return this._airSpeed;
			}
			set
			{
				this._airSpeed = value;
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x0600285B RID: 10331 RVA: 0x000E5827 File Offset: 0x000E3A27
		// (set) Token: 0x0600285C RID: 10332 RVA: 0x000E582F File Offset: 0x000E3A2F
		public virtual float airSmooth
		{
			get
			{
				return this._airSmooth;
			}
			set
			{
				this._airSmooth = value;
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x0600285D RID: 10333 RVA: 0x000E5838 File Offset: 0x000E3A38
		// (set) Token: 0x0600285E RID: 10334 RVA: 0x000E5840 File Offset: 0x000E3A40
		public virtual float extraGravity
		{
			get
			{
				return this._extraGravity;
			}
			set
			{
				this._extraGravity = value;
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x0600285F RID: 10335 RVA: 0x000E5849 File Offset: 0x000E3A49
		// (set) Token: 0x06002860 RID: 10336 RVA: 0x000E5851 File Offset: 0x000E3A51
		public virtual float limitFallVelocity
		{
			get
			{
				return this._limitFallVelocity;
			}
			set
			{
				this._limitFallVelocity = value;
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06002861 RID: 10337 RVA: 0x000E585A File Offset: 0x000E3A5A
		// (set) Token: 0x06002862 RID: 10338 RVA: 0x000E5862 File Offset: 0x000E3A62
		public virtual float ragdollVelocity
		{
			get
			{
				return this._ragdollVelocity;
			}
			set
			{
				this._ragdollVelocity = value;
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06002863 RID: 10339 RVA: 0x000E586B File Offset: 0x000E3A6B
		// (set) Token: 0x06002864 RID: 10340 RVA: 0x000E5873 File Offset: 0x000E3A73
		public virtual float fallMinHeight
		{
			get
			{
				return this._fallMinHeight;
			}
			set
			{
				this._fallMinHeight = value;
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06002865 RID: 10341 RVA: 0x000E587C File Offset: 0x000E3A7C
		// (set) Token: 0x06002866 RID: 10342 RVA: 0x000E5884 File Offset: 0x000E3A84
		public virtual float fallMinVerticalVelocity
		{
			get
			{
				return this._fallMinVerticalVelocity;
			}
			set
			{
				this._fallMinVerticalVelocity = value;
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06002867 RID: 10343 RVA: 0x000E588D File Offset: 0x000E3A8D
		// (set) Token: 0x06002868 RID: 10344 RVA: 0x000E5895 File Offset: 0x000E3A95
		public virtual float fallDamage
		{
			get
			{
				return this._fallDamage;
			}
			set
			{
				this._fallDamage = value;
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06002869 RID: 10345 RVA: 0x000E589E File Offset: 0x000E3A9E
		// (set) Token: 0x0600286A RID: 10346 RVA: 0x000E58A6 File Offset: 0x000E3AA6
		public virtual float rollSpeed
		{
			get
			{
				return this._rollSpeed;
			}
			set
			{
				this._rollSpeed = value;
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x0600286B RID: 10347 RVA: 0x000E58AF File Offset: 0x000E3AAF
		// (set) Token: 0x0600286C RID: 10348 RVA: 0x000E58B7 File Offset: 0x000E3AB7
		public virtual float rollRotationSpeed
		{
			get
			{
				return this._rollRotationSpeed;
			}
			set
			{
				this._rollRotationSpeed = value;
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x0600286D RID: 10349 RVA: 0x000E58C0 File Offset: 0x000E3AC0
		// (set) Token: 0x0600286E RID: 10350 RVA: 0x000E58C8 File Offset: 0x000E3AC8
		public virtual float rollExtraGravity
		{
			get
			{
				return this._rollUseGravityTime;
			}
			set
			{
				this._rollUseGravityTime = value;
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x0600286F RID: 10351 RVA: 0x000E58D1 File Offset: 0x000E3AD1
		// (set) Token: 0x06002870 RID: 10352 RVA: 0x000E58D9 File Offset: 0x000E3AD9
		public virtual float timeToRollAgain
		{
			get
			{
				return this._timeToRollAgain;
			}
			set
			{
				this._timeToRollAgain = value;
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06002871 RID: 10353 RVA: 0x000E58E2 File Offset: 0x000E3AE2
		// (set) Token: 0x06002872 RID: 10354 RVA: 0x000E58EA File Offset: 0x000E3AEA
		public virtual bool noDamageWhileRolling
		{
			get
			{
				return this._noDamageWhileRolling;
			}
			set
			{
				this._noDamageWhileRolling = value;
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06002873 RID: 10355 RVA: 0x000E58F3 File Offset: 0x000E3AF3
		// (set) Token: 0x06002874 RID: 10356 RVA: 0x000E58FB File Offset: 0x000E3AFB
		public virtual bool noActiveRagdollWhileRolling
		{
			get
			{
				return this._noActiveRagdollWhileRolling;
			}
			set
			{
				this._noActiveRagdollWhileRolling = value;
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06002875 RID: 10357 RVA: 0x000E5904 File Offset: 0x000E3B04
		// (set) Token: 0x06002876 RID: 10358 RVA: 0x000E590C File Offset: 0x000E3B0C
		public bool isOnSlope
		{
			get
			{
				return this._isOnSlope;
			}
			private set
			{
				float num = (this.isInAirborne ? 0f : this.stepHeight);
				if (value && this.CheckStepAhead())
				{
					num = 0f;
				}
				if ((value != this._isOnSlope || num != this.stepHeightRatio) && (value || this.lastTrueSlope < Time.time))
				{
					this._isOnSlope = value;
					if (value)
					{
						this.lastTrueSlope = Time.time + 0.5f;
					}
				}
				this.targetHeightRatio = num;
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06002877 RID: 10359 RVA: 0x000E5986 File Offset: 0x000E3B86
		// (set) Token: 0x06002878 RID: 10360 RVA: 0x000E598E File Offset: 0x000E3B8E
		public virtual float slopeLimit
		{
			get
			{
				return this._slopeLimit;
			}
			set
			{
				this._slopeLimit = value;
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06002879 RID: 10361 RVA: 0x000E5997 File Offset: 0x000E3B97
		// (set) Token: 0x0600287A RID: 10362 RVA: 0x000E599F File Offset: 0x000E3B9F
		public virtual bool useSlide
		{
			get
			{
				return this._useSlide;
			}
			set
			{
				this._useSlide = value;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x0600287B RID: 10363 RVA: 0x000E59A8 File Offset: 0x000E3BA8
		// (set) Token: 0x0600287C RID: 10364 RVA: 0x000E59B0 File Offset: 0x000E3BB0
		public virtual float slideDownVelocity
		{
			get
			{
				return this._slideDownVelocity;
			}
			set
			{
				this._slideDownVelocity = value;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x0600287D RID: 10365 RVA: 0x000E59B9 File Offset: 0x000E3BB9
		// (set) Token: 0x0600287E RID: 10366 RVA: 0x000E59C1 File Offset: 0x000E3BC1
		public virtual float slideDownSmooth
		{
			get
			{
				return this._slideDownSmooth;
			}
			set
			{
				this._slideDownSmooth = value;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x0600287F RID: 10367 RVA: 0x000E59CA File Offset: 0x000E3BCA
		// (set) Token: 0x06002880 RID: 10368 RVA: 0x000E59D2 File Offset: 0x000E3BD2
		public virtual float slideSidewaysVelocity
		{
			get
			{
				return this._slideSidewaysVelocity;
			}
			set
			{
				this._slideSidewaysVelocity = value;
			}
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06002881 RID: 10369 RVA: 0x000E59DB File Offset: 0x000E3BDB
		// (set) Token: 0x06002882 RID: 10370 RVA: 0x000E59E3 File Offset: 0x000E3BE3
		public virtual float slidingEnterTime
		{
			get
			{
				return this._SlidingEnterTime;
			}
			set
			{
				this._SlidingEnterTime = value;
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06002883 RID: 10371 RVA: 0x000E59EC File Offset: 0x000E3BEC
		// (set) Token: 0x06002884 RID: 10372 RVA: 0x000E59F4 File Offset: 0x000E3BF4
		public virtual float rotateSlopeEnterTime
		{
			get
			{
				return this._RotateSlopeEnterTime;
			}
			set
			{
				this._RotateSlopeEnterTime = value;
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06002885 RID: 10373 RVA: 0x000E59FD File Offset: 0x000E3BFD
		// (set) Token: 0x06002886 RID: 10374 RVA: 0x000E5A05 File Offset: 0x000E3C05
		public virtual float rotateDownSlopeSmooth
		{
			get
			{
				return this._rotateDownSlopeSmooth;
			}
			set
			{
				this._rotateDownSlopeSmooth = value;
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06002887 RID: 10375 RVA: 0x000E5A0E File Offset: 0x000E3C0E
		// (set) Token: 0x06002888 RID: 10376 RVA: 0x000E5A16 File Offset: 0x000E3C16
		internal virtual float stopMoveWeight
		{
			get
			{
				return this._stopMoveWeight;
			}
			set
			{
				this._stopMoveWeight = value;
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06002889 RID: 10377 RVA: 0x000E5A1F File Offset: 0x000E3C1F
		// (set) Token: 0x0600288A RID: 10378 RVA: 0x000E5A27 File Offset: 0x000E3C27
		internal virtual float sprintWeight
		{
			get
			{
				return this._sprintWeight;
			}
			set
			{
				this._sprintWeight = value;
			}
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x0600288B RID: 10379 RVA: 0x000E5A30 File Offset: 0x000E3C30
		// (set) Token: 0x0600288C RID: 10380 RVA: 0x000E5A38 File Offset: 0x000E3C38
		public vAnimatorStateInfos animatorStateInfos
		{
			get
			{
				return this._animatorStateInfos;
			}
			protected set
			{
				this._animatorStateInfos = value;
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x0600288D RID: 10381 RVA: 0x000E5A41 File Offset: 0x000E3C41
		// (set) Token: 0x0600288E RID: 10382 RVA: 0x000E5A5B File Offset: 0x000E3C5B
		public virtual bool isStrafing
		{
			get
			{
				return (!this.sprintOnlyFree || !this.isSprinting) && this._isStrafing;
			}
			set
			{
				this._isStrafing = value;
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x0600288F RID: 10383 RVA: 0x000E5A64 File Offset: 0x000E3C64
		// (set) Token: 0x06002890 RID: 10384 RVA: 0x000E5A6C File Offset: 0x000E3C6C
		public bool isGrounded
		{
			get
			{
				return this._isGrounded;
			}
			set
			{
				if (this._isGrounded != value)
				{
					this._isGrounded = value;
					if (this._isGrounded)
					{
						this.OnGrounded.Invoke();
						return;
					}
					this.OnExitGround.Invoke();
				}
			}
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06002891 RID: 10385 RVA: 0x000E5A9D File Offset: 0x000E3C9D
		// (set) Token: 0x06002892 RID: 10386 RVA: 0x000E5AA5 File Offset: 0x000E3CA5
		public bool disableCheckGround { get; set; }

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06002893 RID: 10387 RVA: 0x000E5AAE File Offset: 0x000E3CAE
		// (set) Token: 0x06002894 RID: 10388 RVA: 0x000E5AB6 File Offset: 0x000E3CB6
		public bool inCrouchArea { get; protected set; }

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06002895 RID: 10389 RVA: 0x000E5ABF File Offset: 0x000E3CBF
		// (set) Token: 0x06002896 RID: 10390 RVA: 0x000E5AC7 File Offset: 0x000E3CC7
		public virtual bool isRunning
		{
			get
			{
				return this._isRunning;
			}
			set
			{
				this._isRunning = value;
			}
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06002897 RID: 10391 RVA: 0x000E5AD0 File Offset: 0x000E3CD0
		// (set) Token: 0x06002898 RID: 10392 RVA: 0x000E5AD8 File Offset: 0x000E3CD8
		public virtual bool isSprinting
		{
			get
			{
				return this._isSprinting;
			}
			set
			{
				this._isSprinting = value;
			}
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06002899 RID: 10393 RVA: 0x000E5AE1 File Offset: 0x000E3CE1
		// (set) Token: 0x0600289A RID: 10394 RVA: 0x000E5AE9 File Offset: 0x000E3CE9
		public bool isSliding { get; protected set; }

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x0600289B RID: 10395 RVA: 0x000E5AF2 File Offset: 0x000E3CF2
		// (set) Token: 0x0600289C RID: 10396 RVA: 0x000E5AFA File Offset: 0x000E3CFA
		public bool autoCrouch { get; protected set; }

		// Token: 0x0600289D RID: 10397 RVA: 0x000E5B04 File Offset: 0x000E3D04
		protected void RemoveComponents()
		{
			if (!this.removeComponentsAfterDie)
			{
				return;
			}
			if (this._capsuleCollider != null)
			{
				Object.Destroy(this._capsuleCollider);
			}
			if (this._rigidbody != null)
			{
				Object.Destroy(this._rigidbody);
			}
			if (base.animator != null)
			{
				Object.Destroy(base.animator);
			}
			MonoBehaviour[] components = base.GetComponents<MonoBehaviour>();
			for (int i = 0; i < components.Length; i++)
			{
				Object.Destroy(components[i]);
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x0600289E RID: 10398 RVA: 0x000E5B82 File Offset: 0x000E3D82
		internal virtual PhysicMaterial MovingPhysicsMaterial
		{
			get
			{
				return this.movingMaterial;
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x0600289F RID: 10399 RVA: 0x000E5B8A File Offset: 0x000E3D8A
		internal virtual PhysicMaterial IdlePhysicsMaterial
		{
			get
			{
				return this.idleMaterial;
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x060028A0 RID: 10400 RVA: 0x000E5B92 File Offset: 0x000E3D92
		internal virtual PhysicMaterial AirbornePhysicsMaterial
		{
			get
			{
				return this.airborneMaterial;
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x060028A1 RID: 10401 RVA: 0x000E5B9A File Offset: 0x000E3D9A
		// (set) Token: 0x060028A2 RID: 10402 RVA: 0x000E5BA2 File Offset: 0x000E3DA2
		public PhysicMaterial currentMaterialPhysics { get; protected set; }

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x060028A3 RID: 10403 RVA: 0x000E5BAB File Offset: 0x000E3DAB
		// (set) Token: 0x060028A4 RID: 10404 RVA: 0x000E5BB3 File Offset: 0x000E3DB3
		public virtual float defaultSpeedMultiplier { get; set; }

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x060028A5 RID: 10405 RVA: 0x000E5BBC File Offset: 0x000E3DBC
		// (set) Token: 0x060028A6 RID: 10406 RVA: 0x000E5BC4 File Offset: 0x000E3DC4
		public virtual float inputMagnitude { get; set; }

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x060028A7 RID: 10407 RVA: 0x000E5BCD File Offset: 0x000E3DCD
		// (set) Token: 0x060028A8 RID: 10408 RVA: 0x000E5BD5 File Offset: 0x000E3DD5
		public virtual float rotationMagnitude { get; set; }

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x060028A9 RID: 10409 RVA: 0x000E5BDE File Offset: 0x000E3DDE
		// (set) Token: 0x060028AA RID: 10410 RVA: 0x000E5BE6 File Offset: 0x000E3DE6
		public virtual float verticalSpeed { get; set; }

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x060028AB RID: 10411 RVA: 0x000E5BEF File Offset: 0x000E3DEF
		// (set) Token: 0x060028AC RID: 10412 RVA: 0x000E5BF7 File Offset: 0x000E3DF7
		public virtual float horizontalSpeed { get; set; }

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x060028AD RID: 10413 RVA: 0x000E5C00 File Offset: 0x000E3E00
		// (set) Token: 0x060028AE RID: 10414 RVA: 0x000E5C08 File Offset: 0x000E3E08
		public virtual bool invertVerticalSpeed { get; set; }

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x060028AF RID: 10415 RVA: 0x000E5C11 File Offset: 0x000E3E11
		// (set) Token: 0x060028B0 RID: 10416 RVA: 0x000E5C19 File Offset: 0x000E3E19
		public virtual bool invertHorizontalSpeed { get; set; }

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x060028B1 RID: 10417 RVA: 0x000E5C22 File Offset: 0x000E3E22
		// (set) Token: 0x060028B2 RID: 10418 RVA: 0x000E5C2A File Offset: 0x000E3E2A
		public virtual float moveSpeed { get; set; }

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x060028B3 RID: 10419 RVA: 0x000E5C33 File Offset: 0x000E3E33
		// (set) Token: 0x060028B4 RID: 10420 RVA: 0x000E5C3B File Offset: 0x000E3E3B
		public virtual float verticalVelocity { get; set; }

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x060028B5 RID: 10421 RVA: 0x000E5C44 File Offset: 0x000E3E44
		// (set) Token: 0x060028B6 RID: 10422 RVA: 0x000E5C4C File Offset: 0x000E3E4C
		public virtual float colliderRadius { get; set; }

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x060028B7 RID: 10423 RVA: 0x000E5C55 File Offset: 0x000E3E55
		// (set) Token: 0x060028B8 RID: 10424 RVA: 0x000E5C5D File Offset: 0x000E3E5D
		public virtual float colliderHeight { get; set; }

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x060028B9 RID: 10425 RVA: 0x000E5C66 File Offset: 0x000E3E66
		// (set) Token: 0x060028BA RID: 10426 RVA: 0x000E5C6E File Offset: 0x000E3E6E
		public virtual float jumpMultiplier { get; set; }

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x060028BB RID: 10427 RVA: 0x000E5C77 File Offset: 0x000E3E77
		// (set) Token: 0x060028BC RID: 10428 RVA: 0x000E5C7F File Offset: 0x000E3E7F
		public virtual float timeToResetJumpMultiplier { get; set; }

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x060028BD RID: 10429 RVA: 0x000E5C88 File Offset: 0x000E3E88
		// (set) Token: 0x060028BE RID: 10430 RVA: 0x000E5C90 File Offset: 0x000E3E90
		public virtual float heightReached { get; set; }

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x060028BF RID: 10431 RVA: 0x000E5C99 File Offset: 0x000E3E99
		// (set) Token: 0x060028C0 RID: 10432 RVA: 0x000E5CA1 File Offset: 0x000E3EA1
		public virtual float lastCeilingContact { get; set; }

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x060028C1 RID: 10433 RVA: 0x000E5CAA File Offset: 0x000E3EAA
		// (set) Token: 0x060028C2 RID: 10434 RVA: 0x000E5CB2 File Offset: 0x000E3EB2
		public virtual bool hasCeilingContact { get; set; }

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x060028C3 RID: 10435 RVA: 0x000E5CBB File Offset: 0x000E3EBB
		// (set) Token: 0x060028C4 RID: 10436 RVA: 0x000E5CC3 File Offset: 0x000E3EC3
		public virtual float groundAngle { get; set; }

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x060028C5 RID: 10437 RVA: 0x000E5CCC File Offset: 0x000E3ECC
		// (set) Token: 0x060028C6 RID: 10438 RVA: 0x000E5CD4 File Offset: 0x000E3ED4
		public virtual bool lockMovement { get; set; }

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x060028C7 RID: 10439 RVA: 0x000E5CDD File Offset: 0x000E3EDD
		// (set) Token: 0x060028C8 RID: 10440 RVA: 0x000E5CE5 File Offset: 0x000E3EE5
		public virtual bool lockRotation { get; set; }

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x060028C9 RID: 10441 RVA: 0x000E5CEE File Offset: 0x000E3EEE
		// (set) Token: 0x060028CA RID: 10442 RVA: 0x000E5CF6 File Offset: 0x000E3EF6
		public virtual bool lockSetMoveSpeed { get; set; }

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x060028CB RID: 10443 RVA: 0x000E5CFF File Offset: 0x000E3EFF
		// (set) Token: 0x060028CC RID: 10444 RVA: 0x000E5D07 File Offset: 0x000E3F07
		protected bool _isStrafing { get; set; }

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x060028CD RID: 10445 RVA: 0x000E5D10 File Offset: 0x000E3F10
		// (set) Token: 0x060028CE RID: 10446 RVA: 0x000E5D18 File Offset: 0x000E3F18
		public virtual bool lockInStrafe { get; set; }

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x060028CF RID: 10447 RVA: 0x000E5D21 File Offset: 0x000E3F21
		// (set) Token: 0x060028D0 RID: 10448 RVA: 0x000E5D29 File Offset: 0x000E3F29
		public virtual bool forceRootMotion { get; set; }

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x060028D1 RID: 10449 RVA: 0x000E5D32 File Offset: 0x000E3F32
		// (set) Token: 0x060028D2 RID: 10450 RVA: 0x000E5D3A File Offset: 0x000E3F3A
		public virtual bool keepDirection { get; set; }

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x060028D3 RID: 10451 RVA: 0x000E5D43 File Offset: 0x000E3F43
		// (set) Token: 0x060028D4 RID: 10452 RVA: 0x000E5D4B File Offset: 0x000E3F4B
		public virtual bool finishStaminaOnSprint { get; set; }

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x060028D5 RID: 10453 RVA: 0x000E5D54 File Offset: 0x000E3F54
		// (set) Token: 0x060028D6 RID: 10454 RVA: 0x000E5D5C File Offset: 0x000E3F5C
		public virtual bool applyingStepOffset { get; set; }

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x060028D7 RID: 10455 RVA: 0x000E5D65 File Offset: 0x000E3F65
		// (set) Token: 0x060028D8 RID: 10456 RVA: 0x000E5D6D File Offset: 0x000E3F6D
		public virtual bool lockAnimMovement { get; set; }

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x060028D9 RID: 10457 RVA: 0x000E5D76 File Offset: 0x000E3F76
		// (set) Token: 0x060028DA RID: 10458 RVA: 0x000E5D7E File Offset: 0x000E3F7E
		public virtual bool lockAnimRotation { get; set; }

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x060028DB RID: 10459 RVA: 0x000E5D87 File Offset: 0x000E3F87
		// (set) Token: 0x060028DC RID: 10460 RVA: 0x000E5D8F File Offset: 0x000E3F8F
		public virtual bool disableSnapToGround { get; set; }

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x060028DD RID: 10461 RVA: 0x000E5D98 File Offset: 0x000E3F98
		// (set) Token: 0x060028DE RID: 10462 RVA: 0x000E5DA0 File Offset: 0x000E3FA0
		public virtual Vector3 lastCharacterAngle { get; set; }

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x060028DF RID: 10463 RVA: 0x000E5DA9 File Offset: 0x000E3FA9
		// (set) Token: 0x060028E0 RID: 10464 RVA: 0x000E5DB1 File Offset: 0x000E3FB1
		public virtual Transform rotateTarget { get; set; }

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x060028E1 RID: 10465 RVA: 0x000E5DBA File Offset: 0x000E3FBA
		// (set) Token: 0x060028E2 RID: 10466 RVA: 0x000E5DC2 File Offset: 0x000E3FC2
		public virtual Vector3 input { get; set; }

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x060028E3 RID: 10467 RVA: 0x000E5DCB File Offset: 0x000E3FCB
		// (set) Token: 0x060028E4 RID: 10468 RVA: 0x000E5DD3 File Offset: 0x000E3FD3
		public virtual Vector3 oldInput { get; set; }

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x060028E5 RID: 10469 RVA: 0x000E5DDC File Offset: 0x000E3FDC
		// (set) Token: 0x060028E6 RID: 10470 RVA: 0x000E5DE4 File Offset: 0x000E3FE4
		public virtual Vector3 colliderCenter { get; set; }

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x060028E7 RID: 10471 RVA: 0x000E5DED File Offset: 0x000E3FED
		// (set) Token: 0x060028E8 RID: 10472 RVA: 0x000E5DF5 File Offset: 0x000E3FF5
		public virtual Vector3 inputSmooth { get; set; }

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x060028E9 RID: 10473 RVA: 0x000E5DFE File Offset: 0x000E3FFE
		// (set) Token: 0x060028EA RID: 10474 RVA: 0x000E5E06 File Offset: 0x000E4006
		public virtual Vector3 moveDirection { get; set; }

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x060028EB RID: 10475 RVA: 0x000E5E0F File Offset: 0x000E400F
		// (set) Token: 0x060028EC RID: 10476 RVA: 0x000E5E17 File Offset: 0x000E4017
		public virtual Vector3 targetVelocity { get; set; }

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x060028ED RID: 10477 RVA: 0x000E5E20 File Offset: 0x000E4020
		// (set) Token: 0x060028EE RID: 10478 RVA: 0x000E5E28 File Offset: 0x000E4028
		public virtual Vector3 externalPlatformVelocity { get; protected set; }

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x060028EF RID: 10479 RVA: 0x000E5E31 File Offset: 0x000E4031
		// (set) Token: 0x060028F0 RID: 10480 RVA: 0x000E5E39 File Offset: 0x000E4039
		public virtual float snapDistance { get; set; }

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x060028F1 RID: 10481 RVA: 0x000E5E42 File Offset: 0x000E4042
		// (set) Token: 0x060028F2 RID: 10482 RVA: 0x000E5E4A File Offset: 0x000E404A
		public virtual bool validSnapPoint { get; set; }

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x060028F3 RID: 10483 RVA: 0x000E5E53 File Offset: 0x000E4053
		public virtual Vector3 rayOrigin
		{
			get
			{
				return base.transform.position + base.transform.up * (this.capsuleHeight * this.stepHeight + this._capsuleCollider.radius);
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x060028F4 RID: 10484 RVA: 0x000E5E8E File Offset: 0x000E408E
		public virtual int baseLayer
		{
			get
			{
				return base.animator.GetLayerIndex("Base Layer");
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x060028F5 RID: 10485 RVA: 0x000E5EA0 File Offset: 0x000E40A0
		public virtual int underBodyLayer
		{
			get
			{
				return base.animator.GetLayerIndex("UnderBody");
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x060028F6 RID: 10486 RVA: 0x000E5EB2 File Offset: 0x000E40B2
		public virtual int rightArmLayer
		{
			get
			{
				return base.animator.GetLayerIndex("RightArm");
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x060028F7 RID: 10487 RVA: 0x000E5EC4 File Offset: 0x000E40C4
		public virtual int leftArmLayer
		{
			get
			{
				return base.animator.GetLayerIndex("LeftArm");
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x060028F8 RID: 10488 RVA: 0x000E5ED6 File Offset: 0x000E40D6
		public virtual int upperBodyLayer
		{
			get
			{
				return base.animator.GetLayerIndex("UpperBody");
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x060028F9 RID: 10489 RVA: 0x000E5EE8 File Offset: 0x000E40E8
		public virtual int fullbodyLayer
		{
			get
			{
				return base.animator.GetLayerIndex("FullBody");
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x060028FA RID: 10490 RVA: 0x000E5EFA File Offset: 0x000E40FA
		// (set) Token: 0x060028FB RID: 10491 RVA: 0x000E5F02 File Offset: 0x000E4102
		public virtual float colliderRadiusDefault { get; protected set; }

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x060028FC RID: 10492 RVA: 0x000E5F0B File Offset: 0x000E410B
		// (set) Token: 0x060028FD RID: 10493 RVA: 0x000E5F13 File Offset: 0x000E4113
		public virtual float colliderHeightDefault { get; protected set; }

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x060028FE RID: 10494 RVA: 0x000E5F1C File Offset: 0x000E411C
		// (set) Token: 0x060028FF RID: 10495 RVA: 0x000E5F24 File Offset: 0x000E4124
		public virtual Vector3 colliderCenterDefault { get; protected set; }

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06002900 RID: 10496 RVA: 0x000E5F2D File Offset: 0x000E412D
		protected virtual bool _canApplyFallDamage
		{
			get
			{
				return !this.blockApplyFallDamage && this.jumpMultiplier <= 1f && !this.customAction;
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06002901 RID: 10497 RVA: 0x000E5F4F File Offset: 0x000E414F
		// (set) Token: 0x06002902 RID: 10498 RVA: 0x000E5F57 File Offset: 0x000E4157
		public virtual bool alwaysWalkByDefault { get; set; }

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06002903 RID: 10499 RVA: 0x000E5F60 File Offset: 0x000E4160
		// (set) Token: 0x06002904 RID: 10500 RVA: 0x000E5F68 File Offset: 0x000E4168
		public virtual bool blockApplyFallDamage { get; set; }

		// Token: 0x06002905 RID: 10501 RVA: 0x000E5F71 File Offset: 0x000E4171
		protected override void OnValidate()
		{
			base.OnValidate();
			if (!Application.isPlaying)
			{
				this.targetHeightRatio = this.stepHeight;
				this.stepHeightRatio = this.stepHeight;
				this.ControlCapsuleHeight();
			}
		}

		// Token: 0x06002906 RID: 10502 RVA: 0x000E5F9E File Offset: 0x000E419E
		protected virtual void Awake()
		{
			this.defaultSpeedMultiplier = 1f;
			this.jumpMultiplier = 1f;
			this.heightReached = base.transform.position.y;
			this.SetCustomFixedTimeStep();
		}

		// Token: 0x06002907 RID: 10503 RVA: 0x000E5FD2 File Offset: 0x000E41D2
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x06002908 RID: 10504 RVA: 0x000E5FDC File Offset: 0x000E41DC
		public override void Init()
		{
			base.Init();
			base.animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
			this._rigidbody = base.GetComponent<Rigidbody>();
			this._capsuleCollider = base.GetComponent<CapsuleCollider>();
			this.colliderCenter = (this.colliderCenterDefault = this._capsuleCollider.center);
			this.colliderRadius = (this.colliderRadiusDefault = this._capsuleCollider.radius);
			this.colliderHeight = (this.colliderHeightDefault = this._capsuleCollider.height);
			Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Physics.IgnoreCollision(this._capsuleCollider, componentsInChildren[i]);
			}
			if (this.fillHealthOnStart)
			{
				this.currentHealth = (float)this.maxHealth;
			}
			this.currentHealthRecoveryDelay = this.healthRecoveryDelay;
			this.currentStamina = this.maxStamina;
			this.ResetJumpMultiplier();
			this.isGrounded = true;
			this.ResetControllerSpeedMultiplier();
			this.freeSpeed.Init();
			this.strafeSpeed.Init();
			this.ControlCapsuleHeight();
		}

		// Token: 0x06002909 RID: 10505 RVA: 0x000E60E0 File Offset: 0x000E42E0
		public virtual void SetCustomFixedTimeStep()
		{
			switch (this.customFixedTimeStep)
			{
			case vThirdPersonMotor.CustomFixedTimeStep.Default:
				break;
			case vThirdPersonMotor.CustomFixedTimeStep.FPS30:
				Time.fixedDeltaTime = 0.03333334f;
				return;
			case vThirdPersonMotor.CustomFixedTimeStep.FPS60:
				Time.fixedDeltaTime = 0.01666667f;
				return;
			case vThirdPersonMotor.CustomFixedTimeStep.FPS75:
				Time.fixedDeltaTime = 0.01333333f;
				return;
			case vThirdPersonMotor.CustomFixedTimeStep.FPS90:
				Time.fixedDeltaTime = 0.01111111f;
				return;
			case vThirdPersonMotor.CustomFixedTimeStep.FPS120:
				Time.fixedDeltaTime = 0.008333334f;
				return;
			case vThirdPersonMotor.CustomFixedTimeStep.FPS144:
				Time.fixedDeltaTime = 0.006944444f;
				break;
			default:
				return;
			}
		}

		// Token: 0x0600290A RID: 10506 RVA: 0x00004F31 File Offset: 0x00003131
		protected virtual void OnEnable()
		{
		}

		// Token: 0x0600290B RID: 10507 RVA: 0x000E6158 File Offset: 0x000E4358
		protected virtual void OnDisable()
		{
			this.SetFullCapsuleHeight();
		}

		// Token: 0x0600290C RID: 10508 RVA: 0x000E6160 File Offset: 0x000E4360
		public virtual void UpdateMotor()
		{
			this.CheckStamina();
			this.CheckGround();
			this.CheckRagdoll();
			this.SlideMovementBehavior();
			this.ControlCapsuleHeight();
			this.ControlJumpBehaviour();
			this.ApplyAirMovement();
			this.ApplyMovement(this.targetVelocity);
			this.ApplyExtraGravity();
			this.StaminaRecovery();
			this.HealthRecovery();
			this.CalculateRotationMagnitude();
		}

		// Token: 0x0600290D RID: 10509 RVA: 0x000E61BC File Offset: 0x000E43BC
		public override void TakeDamage(vDamage damage)
		{
			if (this.currentHealth <= 0f || this.IgnoreDamageRolling())
			{
				if (damage.activeRagdoll && !this.IgnoreDamageActiveRagdollRolling())
				{
					base.onActiveRagdoll.Invoke(damage);
				}
				return;
			}
			if (damage.activeRagdoll && this.IgnoreDamageActiveRagdollRolling())
			{
				damage.activeRagdoll = false;
			}
			base.TakeDamage(damage);
		}

		// Token: 0x0600290E RID: 10510 RVA: 0x000E6219 File Offset: 0x000E4419
		protected virtual bool IgnoreDamageRolling()
		{
			return this.noDamageWhileRolling && this.isRolling;
		}

		// Token: 0x0600290F RID: 10511 RVA: 0x000E622B File Offset: 0x000E442B
		protected virtual bool IgnoreDamageActiveRagdollRolling()
		{
			return this.noActiveRagdollWhileRolling && this.isRolling;
		}

		// Token: 0x06002910 RID: 10512 RVA: 0x000E623D File Offset: 0x000E443D
		protected override void TriggerDamageReaction(vDamage damage)
		{
			if (!this.customAction)
			{
				base.TriggerDamageReaction(damage);
				return;
			}
			if (damage.activeRagdoll)
			{
				base.onActiveRagdoll.Invoke(damage);
			}
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x000E6264 File Offset: 0x000E4464
		public virtual void ReduceStamina(float value, bool accumulative)
		{
			if (this.ignoreStaminaConsumption)
			{
				this.currentStamina = this.maxStamina;
				return;
			}
			if (this.customAction)
			{
				return;
			}
			if (accumulative)
			{
				this.currentStamina -= value * Time.fixedDeltaTime;
			}
			else
			{
				this.currentStamina -= value;
			}
			if (this.currentStamina < 0f)
			{
				this.currentStamina = 0f;
				this.OnStaminaEnd.Invoke();
			}
		}

		// Token: 0x06002912 RID: 10514 RVA: 0x000E62DC File Offset: 0x000E44DC
		public virtual void ChangeStamina(int value)
		{
			if (this.ignoreStaminaConsumption)
			{
				this.currentStamina = this.maxStamina;
				return;
			}
			this.currentStamina += (float)value;
			this.currentStamina = Mathf.Clamp(this.currentStamina, 0f, this.maxStamina);
		}

		// Token: 0x06002913 RID: 10515 RVA: 0x000E6329 File Offset: 0x000E4529
		public virtual void ChangeMaxStamina(int value)
		{
			this.maxStamina += (float)value;
			if (this.maxStamina < 0f)
			{
				this.maxStamina = 0f;
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06002914 RID: 10516 RVA: 0x000E6352 File Offset: 0x000E4552
		// (set) Token: 0x06002915 RID: 10517 RVA: 0x000E635C File Offset: 0x000E455C
		public override bool isDead
		{
			get
			{
				return base.isDead;
			}
			set
			{
				base.isDead = value;
				if (value)
				{
					if (this.isGrounded)
					{
						if (this._rigidbody)
						{
							this._rigidbody.isKinematic = true;
						}
						if (this._capsuleCollider)
						{
							this._capsuleCollider.enabled = false;
							return;
						}
					}
				}
				else if (!this.ragdolled)
				{
					if (this._rigidbody)
					{
						this._rigidbody.isKinematic = false;
					}
					if (this._capsuleCollider)
					{
						this._capsuleCollider.enabled = true;
					}
				}
			}
		}

		// Token: 0x06002916 RID: 10518 RVA: 0x000E63E8 File Offset: 0x000E45E8
		protected virtual void CheckStamina()
		{
			if (!this.useBuiltInStamina)
			{
				return;
			}
			if (this.isSprinting)
			{
				this.currentStaminaRecoveryDelay = 0.25f;
				this.ReduceStamina(this.sprintStamina, true);
			}
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x000E6414 File Offset: 0x000E4614
		public virtual void StaminaRecovery()
		{
			if (!this.useBuiltInStamina)
			{
				return;
			}
			if (this.currentStaminaRecoveryDelay > 0f)
			{
				this.currentStaminaRecoveryDelay -= Time.fixedDeltaTime;
				return;
			}
			if (this.currentStamina > this.maxStamina)
			{
				this.currentStamina = this.maxStamina;
			}
			if (this.currentStamina < this.maxStamina)
			{
				this.currentStamina += this.staminaRecovery;
			}
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06002918 RID: 10520 RVA: 0x000E6488 File Offset: 0x000E4688
		public virtual bool hasMovementInput
		{
			get
			{
				return this.inputSmooth.sqrMagnitude + this.input.sqrMagnitude > 0.1f || (this.input - this.inputSmooth).sqrMagnitude > 0.1f;
			}
		}

		// Token: 0x06002919 RID: 10521 RVA: 0x000E64DC File Offset: 0x000E46DC
		protected virtual void CalculateRotationMagnitude()
		{
			Vector3 vector = base.transform.eulerAngles - this.lastCharacterAngle;
			if ((double)vector.sqrMagnitude < 0.01)
			{
				this.lastCharacterAngle = base.transform.eulerAngles;
				this.rotationMagnitude = 0f;
				return;
			}
			float num = vector.NormalizeAngle().y / (this.isStrafing ? this.strafeSpeed.rotationSpeed : this.freeSpeed.rotationSpeed);
			this.rotationMagnitude = (float)Math.Round((double)num, 2);
			this.lastCharacterAngle = base.transform.eulerAngles;
		}

		// Token: 0x0600291A RID: 10522 RVA: 0x000E657D File Offset: 0x000E477D
		public virtual void SetControllerSpeedMultiplier(float speed)
		{
			this.speedMultiplier = speed;
		}

		// Token: 0x0600291B RID: 10523 RVA: 0x000E6586 File Offset: 0x000E4786
		public virtual void ResetControllerSpeedMultiplier()
		{
			this.speedMultiplier = this.defaultSpeedMultiplier;
		}

		// Token: 0x0600291C RID: 10524 RVA: 0x000E6594 File Offset: 0x000E4794
		public virtual void SetControllerMoveSpeed(vThirdPersonMotor.vMovementSpeed speed)
		{
			if (this.isCrouching)
			{
				this.moveSpeed = Mathf.Lerp(this.moveSpeed, speed.crouchSpeed, speed.movementSmooth * Time.fixedDeltaTime);
				return;
			}
			float num;
			if (this.isSprinting)
			{
				num = speed.sprintSpeed;
			}
			else if (this.isRunning)
			{
				num = speed.runningSpeed;
			}
			else
			{
				num = speed.walkSpeed;
			}
			this.moveSpeed = Mathf.Lerp(this.moveSpeed, num, speed.movementSmooth * Time.fixedDeltaTime);
		}

		// Token: 0x0600291D RID: 10525 RVA: 0x000E6614 File Offset: 0x000E4814
		public virtual void MoveCharacter(Vector3 direction)
		{
			this.inputSmooth = Vector3.Lerp(this.inputSmooth, this.input, (this.isStrafing ? this.strafeSpeed.movementSmooth : this.freeSpeed.movementSmooth) * (this.useRootMotion ? vTime.deltaTime : vTime.fixedDeltaTime));
			this.isOnSlope = this.CheckSlope();
			if (this.MoveCharacterConditions())
			{
				Vector3 vector = ((this.isStrafing || this.moveToDirectionInFree) ? direction : base.transform.forward);
				vector.y = 0f;
				vector = vector.normalized * Mathf.Clamp(direction.magnitude, 0f, 1f);
				Vector3 vector2 = ((this.useRootMotion ? base.animator.rootPosition : this._rigidbody.position) + vector * (this.useRootMotion ? vTime.deltaTime : vTime.fixedDeltaTime) - base.transform.position) / (this.useRootMotion ? vTime.deltaTime : vTime.fixedDeltaTime);
				Vector3 vector3 = (this.useRootMotion ? (base.animator.deltaPosition / vTime.fixedDeltaTime) : (vector2 * (this.moveSpeed * this.speedMultiplier * this.automaticMovementSpeedMultiplier)));
				vector3.y = 0f;
				this.targetVelocity = vector3;
			}
		}

		// Token: 0x0600291E RID: 10526 RVA: 0x000E6788 File Offset: 0x000E4988
		public virtual bool MoveCharacterConditions()
		{
			return this.isGrounded && !this.isSliding && !this.ragdolled && !this.isJumping && !this.isRolling && !this._rigidbody.isKinematic;
		}

		// Token: 0x0600291F RID: 10527 RVA: 0x000E67C4 File Offset: 0x000E49C4
		public virtual void ApplyMovement(Vector3 targetVelocity)
		{
			Vector3 vector = this.externalPlatformVelocity;
			this.externalPlatformVelocity = Vector3.zero;
			this.CheckStopMove(ref targetVelocity);
			if (this.ApplyMovementConditions())
			{
				if (this.isJumping || this.isInAirborne || this.disableSnapToGround)
				{
					targetVelocity.y = this._rigidbody.velocity.y;
				}
				else
				{
					targetVelocity.y = 0f;
				}
				if (!this.isJumping && !this.isInAirborne && this.validSnapPoint && !this.disableSnapToGround)
				{
					targetVelocity = this.ProjectOnGround(targetVelocity);
					this.ApplyGroundMargin(ref targetVelocity);
				}
				if (vector.sqrMagnitude > 1E-06f && this.isGrounded)
				{
					float magnitude = vector.magnitude;
					Vector3 vector2 = ((this.groundHit.normal.sqrMagnitude > 0.5f) ? this.groundHit.normal : Vector3.up);
					vector = Vector3.ProjectOnPlane(vector, vector2);
					if (vector.sqrMagnitude > 1E-06f)
					{
						vector = vector.normalized * magnitude;
					}
				}
				else if (!this.isGrounded)
				{
					vector = Vector3.zero;
				}
				this._rigidbody.velocity = targetVelocity + vector;
			}
		}

		// Token: 0x06002920 RID: 10528 RVA: 0x000E68F5 File Offset: 0x000E4AF5
		public virtual void SetExternalPlatformVelocity(Vector3 velocity)
		{
			this.externalPlatformVelocity = velocity;
		}

		// Token: 0x06002921 RID: 10529 RVA: 0x000E68FE File Offset: 0x000E4AFE
		public virtual bool ApplyMovementConditions()
		{
			return this.isGrounded && !this.isSliding && !this.ragdolled && !this.isJumping && !this._rigidbody.isKinematic;
		}

		// Token: 0x06002922 RID: 10530 RVA: 0x000E6930 File Offset: 0x000E4B30
		private Vector3 ProjectOnGround(Vector3 targetBodyVelocity)
		{
			this.groundAngle = Mathf.Lerp(this.groundAngle, this.GroundAngleFromDirection(), this.groundAngleSmooth * Time.deltaTime);
			if (this.groundAngle < this.slopeLimit)
			{
				float magnitude = targetBodyVelocity.magnitude;
				Vector3 vector = Quaternion.AngleAxis(90f, Vector3.up) * targetBodyVelocity;
				vector.y = 0f;
				Vector3 vector2 = Quaternion.AngleAxis(-this.groundAngle, vector) * targetBodyVelocity;
				this.groundPlaneProjectionWeight = Mathf.Clamp01(this.groundPlaneProjectionWeight + this.groundAngleSmooth * Time.deltaTime);
				targetBodyVelocity = Vector3.Lerp(targetBodyVelocity, vector2.normalized * magnitude, this.groundPlaneProjectionWeight);
			}
			else
			{
				this.groundPlaneProjectionWeight = 0f;
			}
			return targetBodyVelocity;
		}

		// Token: 0x06002923 RID: 10531 RVA: 0x000E69F4 File Offset: 0x000E4BF4
		protected virtual bool CheckSlope()
		{
			Vector3 center = this._capsuleCollider.bounds.center;
			float num = this._capsuleCollider.radius * 0.99f;
			float num2 = this.castLengthGrounded - num;
			if (Physics.SphereCast(center, num, Vector3.down, out this.groundAngleHit, num2, this.groundLayer, QueryTriggerInteraction.Ignore))
			{
				int num3 = (int)Vector3.Angle(this.groundAngleHit.normal, base.transform.up);
				if ((float)num3 > this.slopeLimit && num3 < 85)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002924 RID: 10532 RVA: 0x000E6A80 File Offset: 0x000E4C80
		protected virtual bool CheckStepAhead()
		{
			float num = this.capsuleHeight * this.stepHeight;
			Vector3 vector = base.transform.position + Vector3.up * num;
			Vector3 forward = this._capsuleCollider.transform.forward;
			float num2 = 3f;
			Ray ray = new Ray(vector, forward);
			Vector3 normal = this.groundAngleHit.normal;
			normal.y = 0f;
			normal.Normalize();
			Vector3 vector2 = normal;
			ray.direction = -vector2;
			if (Physics.Raycast(ray, out this.stepAheadHit, num2, this.groundLayer, QueryTriggerInteraction.Ignore))
			{
				if ((int)Vector3.Angle(this.stepAheadHit.normal, base.transform.up) < 85)
				{
					return true;
				}
				if (Physics.Raycast(this.groundAngleHit.point - forward.normalized * 0.1f, Vector3.down, out this.stepAheadHit, num2, this.groundLayer, QueryTriggerInteraction.Ignore) && (float)((int)Vector3.Angle(this.stepAheadHit.normal, base.transform.up)) > this.slopeLimit)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002925 RID: 10533 RVA: 0x000E6BB4 File Offset: 0x000E4DB4
		protected virtual void CheckStopMove(ref Vector3 targetVelocity)
		{
			Vector3 vector = base.transform.position + base.transform.up * this.colliderRadiusDefault;
			Vector3 vector2 = this.moveDirection.normalized;
			vector2 = Vector3.ProjectOnPlane(vector2, this.groundHit.normal);
			float num = 0f;
			float num2 = (this.isStrafing ? this.strafeSpeed.movementSmooth : this.freeSpeed.movementSmooth);
			RaycastHit raycastHit;
			if (this.StopMoveConditions() && this.CheckStopMove(vector2, out raycastHit))
			{
				if (Vector3.Angle(vector2, -raycastHit.normal) < this.slopeLimit)
				{
					float num3 = raycastHit.distance - this.colliderRadiusDefault;
					num = 1f - num3;
				}
				else
				{
					num = -0.01f;
				}
				if (this.debugWindow)
				{
					Debug.DrawLine(vector, raycastHit.point, Color.cyan);
				}
			}
			this.stopMoveWeight = Mathf.Lerp(this.stopMoveWeight, num, num2 * Time.deltaTime);
			this.stopMoveWeight = Mathf.Clamp(this.stopMoveWeight, 0f, 1f);
			targetVelocity = Vector3.LerpUnclamped(targetVelocity, Vector3.zero, this.stopMoveWeight);
		}

		// Token: 0x06002926 RID: 10534 RVA: 0x000E6CEB File Offset: 0x000E4EEB
		public virtual bool StopMoveConditions()
		{
			return this.isGrounded && !this.isJumping && !this.isInAirborne && !this.applyingStepOffset && !this.customAction;
		}

		// Token: 0x06002927 RID: 10535 RVA: 0x000E6D18 File Offset: 0x000E4F18
		protected virtual bool CheckStopMove(Vector3 direction, out RaycastHit hit)
		{
			Vector3 vector = base.transform.position + base.transform.up * this.colliderRadiusDefault;
			float num = this.colliderRadiusDefault + this.stopMoveRayDistance;
			vThirdPersonMotor.StopMoveCheckMethod stopMoveCheckMethod = this.stopMoveCheckMethod;
			if (stopMoveCheckMethod - vThirdPersonMotor.StopMoveCheckMethod.SphereCast <= 1)
			{
				Vector3 vector2 = vector + base.transform.up * (this.capsuleHeight * this.stepHeight);
				Vector3 vector3 = vector + base.transform.up * (this.stopMoveMaxHeight - this._capsuleCollider.radius);
				return Physics.CapsuleCast(vector2, vector3, this._capsuleCollider.radius, direction, out hit, num, this.stopMoveLayer);
			}
			return Physics.Raycast(vector, direction, out hit, num, this.stopMoveLayer);
		}

		// Token: 0x06002928 RID: 10536 RVA: 0x000E6DEC File Offset: 0x000E4FEC
		public virtual void StopCharacterWithLerp()
		{
			this.isSprinting = false;
			this.sprintWeight = 0f;
			this.horizontalSpeed = 0f;
			this.verticalSpeed = 0f;
			this.moveDirection = Vector3.zero;
			this.targetVelocity = Vector3.zero;
			this.input = Vector3.Lerp(this.input, Vector3.zero, 2f * Time.fixedDeltaTime);
			this.inputSmooth = Vector3.Lerp(this.inputSmooth, Vector3.zero, 2f * Time.fixedDeltaTime);
			if (!this._rigidbody.isKinematic)
			{
				this._rigidbody.velocity = Vector3.Lerp(this._rigidbody.velocity, Vector3.zero, 4f * Time.fixedDeltaTime);
			}
			this.inputMagnitude = Mathf.Lerp(this.inputMagnitude, 0f, 2f * Time.fixedDeltaTime);
			this.moveSpeed = Mathf.Lerp(this.moveSpeed, 0f, 2f * Time.fixedDeltaTime);
			base.animator.SetFloat(vAnimatorParameters.InputMagnitude, 0f, 0.2f, Time.fixedDeltaTime);
			base.animator.SetFloat(vAnimatorParameters.InputVertical, 0f, 0.2f, Time.fixedDeltaTime);
			base.animator.SetFloat(vAnimatorParameters.InputHorizontal, 0f, 0.2f, Time.fixedDeltaTime);
			base.animator.SetFloat(vAnimatorParameters.RotationMagnitude, 0f, 0.2f, Time.fixedDeltaTime);
		}

		// Token: 0x06002929 RID: 10537 RVA: 0x000E6F70 File Offset: 0x000E5170
		public virtual void StopCharacter()
		{
			this.isSprinting = false;
			this.sprintWeight = 0f;
			this.horizontalSpeed = 0f;
			this.verticalSpeed = 0f;
			this.moveDirection = Vector3.zero;
			this.targetVelocity = Vector3.zero;
			this.input = Vector3.zero;
			this.inputSmooth = Vector3.zero;
			if (!this._rigidbody.isKinematic)
			{
				this._rigidbody.velocity = Vector3.zero;
			}
			this.inputMagnitude = 0f;
			this.moveSpeed = 0f;
			base.animator.SetFloat(vAnimatorParameters.InputMagnitude, 0f, 0.25f, Time.fixedDeltaTime);
			base.animator.SetFloat(vAnimatorParameters.InputVertical, 0f, 0.25f, Time.fixedDeltaTime);
			base.animator.SetFloat(vAnimatorParameters.InputHorizontal, 0f, 0.25f, Time.fixedDeltaTime);
			base.animator.SetFloat(vAnimatorParameters.RotationMagnitude, 0f, 0.25f, Time.fixedDeltaTime);
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x000E7080 File Offset: 0x000E5280
		public virtual void RotateToPosition(Vector3 position)
		{
			this.RotateToDirection((position - base.transform.position).normalized);
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x000E70AC File Offset: 0x000E52AC
		public virtual void RotateToDirection(Vector3 direction)
		{
			this.RotateToDirection(direction, this.isStrafing ? this.strafeSpeed.rotationSpeed : this.freeSpeed.rotationSpeed);
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x000E70D8 File Offset: 0x000E52D8
		public virtual void RotateToDirection(Vector3 direction, float rotationSpeed)
		{
			if (this.RotateToDirectionConditions())
			{
				direction.y = 0f;
				if (direction.normalized.magnitude == 0f)
				{
					direction = base.transform.forward;
				}
				Vector3 vector = base.transform.rotation.eulerAngles.NormalizeAngle();
				Vector3 vector2 = Quaternion.LookRotation(direction.normalized).eulerAngles.NormalizeAngle();
				vector.y = Mathf.LerpAngle(vector.y, vector2.y, rotationSpeed * Time.fixedDeltaTime);
				Quaternion quaternion = Quaternion.Euler(vector);
				base.transform.rotation = quaternion;
			}
		}

		// Token: 0x0600292D RID: 10541 RVA: 0x000E7186 File Offset: 0x000E5386
		public virtual bool RotateToDirectionConditions()
		{
			return !this.lockAnimRotation && !this.customAction && (this.jumpAndRotate || this.isGrounded) && !this.ragdolled && !this.isSliding;
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x000E71BC File Offset: 0x000E53BC
		protected virtual void ApplyGroundMargin(ref Vector3 velocity)
		{
			float num = base.transform.InverseTransformPoint(this.rayOrigin).y - this.snapDistance;
			this.currentSnapFactor = this.snapFactor;
			this.slopeSlideFactor = Mathf.Clamp(this.slopeSlideFactor, 0f, this.slideSpeed);
			Vector3 vector = base.transform.up * (num / Time.fixedDeltaTime);
			this.CeilingDetection(ref velocity);
			Vector3 vector2 = (this.hasCeilingContact ? vector : (vector * this.currentSnapFactor * Time.fixedDeltaTime));
			Vector3 vector3 = this.groundAngleHit.normal;
			vector3.y = 0f;
			vector3.Normalize();
			vector3 = Vector3.ProjectOnPlane(vector3, this.groundAngleHit.normal);
			if (this.isOnSlope && this.targetHeightRatio <= 0f && (float)this.groundPlane.planeAngle > this.slopeLimit)
			{
				this.slopeSlideFactor += this.slideSpeed * Time.deltaTime;
				vector2 += vector3 * (this.slopeSlideFactor / Time.fixedDeltaTime);
			}
			else
			{
				this.slopeSlideFactor = 0f;
			}
			Vector3 vector4 = Vector3.zero + Vector3.up * base.transform.InverseTransformDirection(vector2).y;
			vector2 = base.transform.TransformDirection(vector4);
			if (vector2.magnitude > this.maxSnapVelocityUp && vector2.y > 0f)
			{
				vector2 = vector2.normalized * this.maxSnapVelocityUp;
			}
			else if (vector2.magnitude > Math.Abs(this.maxSnapVelocityDown))
			{
				vector2 = vector2.normalized * Math.Abs(this.maxSnapVelocityDown);
			}
			velocity += vector2;
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x000E7390 File Offset: 0x000E5590
		protected virtual void CeilingDetection(ref Vector3 velocity)
		{
			if (!this.useCeilingDetection)
			{
				return;
			}
			Ray ray = new Ray
			{
				origin = this._capsuleCollider.bounds.center + base.transform.up * (this._capsuleCollider.height * 0.5f - this._capsuleCollider.radius),
				direction = new Vector3(velocity.x, velocity.y, velocity.z).normalized
			};
			RaycastHit raycastHit;
			if (Physics.SphereCast(ray, this._capsuleCollider.radius * 0.98f, out raycastHit, 0.25f, this.groundLayer, QueryTriggerInteraction.Ignore) && raycastHit.point.y > ray.origin.y)
			{
				Vector3 vector = raycastHit.point - ray.origin - raycastHit.normal;
				vector.y = 0f;
				Vector3 vector2 = velocity;
				vector2.y = 0f;
				Vector3 vector3 = vector2.normalized.AngleFormOtherDirection(vector);
				Vector3 vector4;
				if (vector3.y > 1f)
				{
					vector4 = Quaternion.AngleAxis(-90f, Vector3.up) * -raycastHit.normal;
					vector4.y = 0f;
				}
				else if (vector3.y < -1f)
				{
					vector4 = Quaternion.AngleAxis(90f, Vector3.up) * -raycastHit.normal;
					vector4.y = 0f;
				}
				else
				{
					vector4 = Vector3.zero;
				}
				velocity = vector4.normalized * velocity.magnitude * Mathf.Abs(vector3.y) / 90f;
				this.hasCeilingContact = true;
				this.lastCeilingContact = Time.time + 0.25f;
			}
			if (this.lastCeilingContact < Time.time)
			{
				this.hasCeilingContact = false;
			}
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x000E75A4 File Offset: 0x000E57A4
		protected virtual void ApplyExtraGravity()
		{
			if (this.isDead || this.customAction || this.disableCheckGround || this.isSliding)
			{
				this.applyingStepOffset = false;
				if (!this.isDead)
				{
					this.heightReached = base.transform.position.y;
				}
			}
			if (this.isInAirborne)
			{
				this.verticalVelocity = this._rigidbody.velocity.y;
				if (!this.applyingStepOffset && !this.isJumping && this.extraGravity != 0f)
				{
					this._rigidbody.AddForce(base.transform.up * this.extraGravity * Time.fixedDeltaTime, ForceMode.VelocityChange);
				}
			}
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x000E7660 File Offset: 0x000E5860
		protected virtual void ControlJumpBehaviour()
		{
			if (!this.isJumping)
			{
				return;
			}
			this.jumpCounter -= Time.fixedDeltaTime;
			if (this.jumpCounter <= 0f)
			{
				this.jumpCounter = 0f;
				this.isJumping = false;
			}
			Vector3 velocity = this._rigidbody.velocity;
			velocity.y = this.jumpHeight * this.jumpMultiplier;
			this._rigidbody.velocity = velocity;
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x000E76D3 File Offset: 0x000E58D3
		public virtual void SetJumpMultiplier(float jumpMultiplier)
		{
			this.jumpMultiplier = jumpMultiplier;
		}

		// Token: 0x06002933 RID: 10547 RVA: 0x000E76DC File Offset: 0x000E58DC
		public virtual void SetJumpMultiplier(float jumpMultiplier, float timeToReset = 1f)
		{
			this.jumpMultiplier = jumpMultiplier;
			if (this.timeToResetJumpMultiplier <= 0f)
			{
				this.timeToResetJumpMultiplier = timeToReset;
				base.StartCoroutine(this.ResetJumpMultiplierRoutine());
				return;
			}
			this.timeToResetJumpMultiplier = timeToReset;
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x000E770E File Offset: 0x000E590E
		public virtual void ResetJumpMultiplier()
		{
			base.StopCoroutine("ResetJumpMultiplierRoutine");
			this.timeToResetJumpMultiplier = 0f;
			this.jumpMultiplier = 1f;
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x000E7731 File Offset: 0x000E5931
		protected virtual IEnumerator ResetJumpMultiplierRoutine()
		{
			while (this.timeToResetJumpMultiplier > 0f && this.jumpMultiplier != 1f && (this.isJumping || !this.isGrounded))
			{
				this.timeToResetJumpMultiplier -= Time.fixedDeltaTime;
				yield return null;
			}
			this.timeToResetJumpMultiplier = 0f;
			this.jumpMultiplier = 1f;
			yield break;
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x000E7740 File Offset: 0x000E5940
		public virtual void ApplyAirMovement()
		{
			if ((this.isGrounded && !this.isJumping) || this.isSliding || this.ragdolled || this._rigidbody.isKinematic || this.customAction)
			{
				return;
			}
			if (base.transform.position.y > this.heightReached)
			{
				this.heightReached = base.transform.position.y;
			}
			this.inputSmooth = Vector3.Lerp(this.inputSmooth, this.input, this.airSmooth * Time.fixedDeltaTime);
			if (this.jumpWithRigidbodyForce && !this.isGrounded)
			{
				this._rigidbody.AddForce(this.moveDirection * this.airSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
				return;
			}
			Vector3 moveDirection = this.moveDirection;
			moveDirection.y = 0f;
			moveDirection.x = Mathf.Clamp(this.moveDirection.x, -1f, 1f);
			moveDirection.z = Mathf.Clamp(this.moveDirection.z, -1f, 1f);
			this.moveDirection = moveDirection;
			Vector3 vector = (this._rigidbody.position + this.moveDirection * this.airSpeed * Time.fixedDeltaTime - base.transform.position) / Time.fixedDeltaTime;
			vector.y = this._rigidbody.velocity.y;
			this._rigidbody.velocity = Vector3.Lerp(this._rigidbody.velocity, vector, this.airSmooth * Time.fixedDeltaTime);
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06002937 RID: 10551 RVA: 0x000E78F0 File Offset: 0x000E5AF0
		protected virtual bool jumpFwdCondition
		{
			get
			{
				Vector3 vector = base.transform.position + this._capsuleCollider.center + Vector3.up * -this._capsuleCollider.height * 0.5f;
				Vector3 vector2 = vector + Vector3.up * this._capsuleCollider.height;
				return Physics.CapsuleCastAll(vector, vector2, this._capsuleCollider.radius * 0.5f, base.transform.forward, 0.6f, this.groundLayer).Length == 0;
			}
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x000E798F File Offset: 0x000E5B8F
		public virtual void UseAutoCrouch(bool value)
		{
			this.autoCrouch = value;
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x000E7998 File Offset: 0x000E5B98
		public virtual void AutoCrouch()
		{
			if (this.autoCrouch)
			{
				this.isCrouching = true;
			}
			if (this.autoCrouch && !this.inCrouchArea && this.CanExitCrouch())
			{
				this.autoCrouch = false;
				this.isCrouching = false;
			}
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x000E79D0 File Offset: 0x000E5BD0
		public virtual bool CanExitCrouch()
		{
			if (this.isCrouching)
			{
				float num = this._capsuleCollider.radius * 0.9f;
				Vector3 vector = base.transform.position + Vector3.up * (this.colliderHeight * 0.5f - this.colliderRadius);
				return !Physics.SphereCast(new Ray(vector, Vector3.up), num, out this.groundHit, this.crouchHeadDetect - this.colliderRadius * 0.1f, this.autoCrouchLayer);
			}
			return true;
		}

		// Token: 0x0600293B RID: 10555 RVA: 0x000E7A61 File Offset: 0x000E5C61
		protected virtual void AutoCrouchExit(Collider other)
		{
			if (other.CompareTag("AutoCrouch"))
			{
				this.inCrouchArea = false;
			}
		}

		// Token: 0x0600293C RID: 10556 RVA: 0x000E7A77 File Offset: 0x000E5C77
		protected virtual void CheckForAutoCrouch(Collider other)
		{
			if (other.gameObject.CompareTag("AutoCrouch"))
			{
				this.autoCrouch = true;
				this.inCrouchArea = true;
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x0600293D RID: 10557 RVA: 0x000E7A99 File Offset: 0x000E5C99
		public virtual bool canRollAgain
		{
			get
			{
				return this.isRolling && this.animatorStateInfos.GetCurrentNormalizedTime(0) >= this.timeToRollAgain;
			}
		}

		// Token: 0x0600293E RID: 10558 RVA: 0x000E7ABC File Offset: 0x000E5CBC
		protected virtual void RollBehavior()
		{
			if (!this.isRolling)
			{
				return;
			}
			if (this.rollControl)
			{
				this.inputSmooth = Vector3.Lerp(this.inputSmooth, this.input, (this.isStrafing ? this.strafeSpeed.movementSmooth : this.freeSpeed.movementSmooth) * Time.deltaTime);
			}
			this.RotateToDirection(this.moveDirection, this.rollRotationSpeed);
			Vector3 vector = (this.useRollRootMotion ? new Vector3(base.animator.deltaPosition.x, base.animator.deltaPosition.y, base.animator.deltaPosition.z) : (base.transform.forward * Time.deltaTime)) * ((this.rollSpeed > 0f) ? this.rollSpeed : 1f) / Time.deltaTime * (1f - this.stopMoveWeight);
			if (this.isGrounded)
			{
				vector = this.ProjectOnGround(vector);
				this.ApplyGroundMargin(ref vector);
				if (this.hasCeilingContact)
				{
					this.autoCrouch = true;
				}
			}
			else
			{
				vector.y = this._rigidbody.velocity.y;
				if (this.rollExtraGravity != 0f)
				{
					this._rigidbody.AddForce(base.transform.up * this.rollExtraGravity * Time.fixedDeltaTime, ForceMode.VelocityChange);
				}
			}
			this.targetVelocity = vector;
		}

		// Token: 0x0600293F RID: 10559 RVA: 0x000E7C38 File Offset: 0x000E5E38
		protected virtual void CheckGround()
		{
			this.ControlMaterialPhysics();
			Vector3 vector = -base.transform.up;
			Vector3 rayOrigin = this.rayOrigin;
			float num = ((this.isJumping || !this.isGrounded) ? this.castLengthAirborne : this.castLengthGrounded);
			float num2 = num;
			bool isGrounded = this.isGrounded;
			this.groundPlane.CalculatePlane(base.transform, rayOrigin, Vector3.down, this.castLengthGrounded, this.groundLayer, this.planeSize);
			this.rayHasDetectedHit = Physics.Raycast(rayOrigin, vector, out this.groundHit, num, this.groundLayer, QueryTriggerInteraction.Ignore);
			if (this.rayHasDetectedHit)
			{
				this.validSnapPoint = true;
				float magnitude = (rayOrigin - this.groundHit.point).DotVector(vector).magnitude;
				this.snapDistance = magnitude;
				Vector3 vector2 = base.transform.InverseTransformPoint(this.groundHit.point);
				num2 = Mathf.Max(0f, vector2.y * -1f);
			}
			if (this.useSphereCast)
			{
				RaycastHit raycastHit;
				this.sphereHasDetectedHit = Physics.SphereCast(rayOrigin, this.sphereCastRadius, vector, out raycastHit, num - this.sphereCastRadius, this.groundLayer, QueryTriggerInteraction.Ignore);
				if (this.sphereHasDetectedHit)
				{
					this.groundHit = raycastHit;
					this.validSnapPoint = true;
					this.targetVelocity.y = 0f;
					(this.groundHit.point - rayOrigin).y = 0f;
					Vector3 normal = this.groundHit.normal;
					normal.y = 0f;
					normal.Normalize();
					Vector3 point = this.groundHit.point;
					float magnitude2 = (rayOrigin - point).DotVector(vector).magnitude;
					Vector3 vector3 = base.transform.InverseTransformPoint(point);
					float num3 = Mathf.Max(0f, vector3.y * -1f);
					if (num3 < num2 && !this._isOnSlope)
					{
						this.snapDistance = magnitude2;
						num2 = num3;
					}
				}
			}
			this.groundDistance = num2;
			this.CheckFloorDetected(isGrounded);
			this.UpdateHeightReached();
		}

		// Token: 0x06002940 RID: 10560 RVA: 0x000E7E5C File Offset: 0x000E605C
		protected virtual void CheckFloorDetected(bool floorDetected)
		{
			if (this.customAction || this.disableCheckGround)
			{
				floorDetected = true;
			}
			else if (this.groundDistance <= this.groundMinDistance)
			{
				floorDetected = true;
			}
			else if (this.groundDistance >= this.groundMaxDistance)
			{
				floorDetected = false;
			}
			if (floorDetected != this.isGrounded)
			{
				if (floorDetected && !this.isDead)
				{
					this.CheckFallDamage();
				}
				this.isGrounded = floorDetected;
			}
		}

		// Token: 0x06002941 RID: 10561 RVA: 0x000E7EC4 File Offset: 0x000E60C4
		protected virtual void UpdateHeightReached()
		{
			if (this.isGrounded)
			{
				this.heightReached = base.transform.position.y;
				return;
			}
			if (base.transform.position.y > this.heightReached)
			{
				this.heightReached = base.transform.position.y;
			}
		}

		// Token: 0x06002942 RID: 10562 RVA: 0x000E7F20 File Offset: 0x000E6120
		protected virtual void CheckFallDamage()
		{
			if (this.FallDamageConditions())
			{
				float num = this.heightReached - base.transform.position.y;
				num -= this.fallMinHeight;
				if (num > 0f)
				{
					int num2 = (int)(this.fallDamage * num);
					this.TakeDamage(new vDamage(num2, true));
				}
			}
		}

		// Token: 0x06002943 RID: 10563 RVA: 0x000E7F75 File Offset: 0x000E6175
		public virtual bool FallDamageConditions()
		{
			return (!this.isGrounded && this.verticalVelocity > this.fallMinVerticalVelocity && this._canApplyFallDamage && this.fallMinHeight != 0f) || this.fallDamage != 0f;
		}

		// Token: 0x06002944 RID: 10564 RVA: 0x000E7FB4 File Offset: 0x000E61B4
		protected virtual void ControlMaterialPhysics()
		{
			if (this.changeMaterialPhysics < Time.time)
			{
				if (this.isGrounded && this.input.magnitude < 0.1f)
				{
					if (this.IdlePhysicsMaterial && this._capsuleCollider.material != this.IdlePhysicsMaterial)
					{
						this._capsuleCollider.material = this.IdlePhysicsMaterial;
					}
				}
				else if (this.isGrounded && this.input.magnitude > 0.1f)
				{
					if (this.MovingPhysicsMaterial && this._capsuleCollider.material != this.MovingPhysicsMaterial)
					{
						this._capsuleCollider.material = this.MovingPhysicsMaterial;
					}
				}
				else if (!this.isGrounded && this.AirbornePhysicsMaterial && this._capsuleCollider.material != this.AirbornePhysicsMaterial)
				{
					this._capsuleCollider.material = this.AirbornePhysicsMaterial;
				}
				this.changeMaterialPhysics = Time.time + 1f;
			}
		}

		// Token: 0x06002945 RID: 10565 RVA: 0x000E80D3 File Offset: 0x000E62D3
		public virtual float GroundAngle()
		{
			return Vector3.Angle(this.groundHit.normal, Vector3.up);
		}

		// Token: 0x06002946 RID: 10566 RVA: 0x000E80EC File Offset: 0x000E62EC
		public virtual float GroundAngleFromDirection()
		{
			return Vector3.Angle((this.isStrafing && this.input.magnitude > 0f) ? (base.transform.right * this.input.x + base.transform.forward * this.input.z).normalized : base.transform.forward, this.groundHit.normal) - 90f;
		}

		// Token: 0x06002947 RID: 10567 RVA: 0x000E817C File Offset: 0x000E637C
		protected virtual void AlignWithSurface()
		{
			Ray ray = new Ray(base.transform.position, -base.transform.up);
			Quaternion quaternion = base.transform.rotation;
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, 1.5f, this.groundLayer))
			{
				quaternion = Quaternion.FromToRotation(base.transform.up, raycastHit.normal) * base.transform.localRotation;
			}
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, quaternion, 10f * Time.fixedDeltaTime);
		}

		// Token: 0x06002948 RID: 10568 RVA: 0x000E8220 File Offset: 0x000E6420
		protected virtual void SlideOnSteepSlope()
		{
			if (this.useSlide && this.isGrounded && this.GroundAngle() > this.slopeLimit && !this.disableCheckGround)
			{
				if (this._slidingEnterTime > 0f && !this.isSliding)
				{
					this._slidingEnterTime -= Time.fixedDeltaTime;
					return;
				}
				Vector3 normal = this.groundHit.normal;
				normal.y = 0f;
				Vector3 normalized = Vector3.ProjectOnPlane(normal.normalized, this.groundHit.normal).normalized;
				if (!Physics.Raycast(base.transform.position + Vector3.up * 0.1f, normalized, 0.5f, this.groundLayer))
				{
					this.isSliding = true;
					return;
				}
			}
			else
			{
				this._rotateSlopeEnterTime = this.rotateSlopeEnterTime;
				this._slidingEnterTime = (this.isGrounded ? this.slidingEnterTime : 0f);
				this.isSliding = false;
			}
		}

		// Token: 0x06002949 RID: 10569 RVA: 0x000E832C File Offset: 0x000E652C
		protected virtual void SlideMovementBehavior()
		{
			if (!this.isSliding)
			{
				return;
			}
			Vector3 normal = this.groundHit.normal;
			normal.y = 0f;
			Vector3 normalized = Vector3.ProjectOnPlane(normal.normalized, this.groundHit.normal).normalized;
			if (this.debugWindow)
			{
				Debug.DrawRay(base.transform.position, normalized * this.slideDownVelocity);
			}
			this._rigidbody.velocity = Vector3.Lerp(this._rigidbody.velocity, normalized * this.slideDownVelocity, this.slideDownSmooth * Time.fixedDeltaTime);
			normalized.y = 0f;
			if (this._rotateSlopeEnterTime <= 0f)
			{
				Quaternion quaternion = Quaternion.LookRotation(Vector3.RotateTowards(base.transform.forward, normalized, this.rotateDownSlopeSmooth * Time.fixedDeltaTime, 0f));
				this._rigidbody.MoveRotation(quaternion);
				Vector3 vector = base.transform.InverseTransformDirection(this.moveDirection);
				vector.y = 0f;
				vector.z = 0f;
				vector = base.transform.TransformDirection(vector);
				if (this.debugWindow)
				{
					Debug.DrawRay(base.transform.position, vector * this.slideSidewaysVelocity, Color.blue);
				}
				this._rigidbody.AddForce(vector * this.slideSidewaysVelocity, ForceMode.VelocityChange);
				if (this.debugWindow)
				{
					Debug.DrawRay(base.transform.position, Vector3.ProjectOnPlane(normal.normalized, this.groundHit.normal).normalized, Color.blue);
					Debug.DrawRay(base.transform.position, Quaternion.AngleAxis(90f, this.groundHit.normal) * Vector3.ProjectOnPlane(normal.normalized, this.groundHit.normal).normalized, Color.red);
					Debug.DrawRay(base.transform.position, base.transform.TransformDirection(vector.normalized * 2f), Color.green);
					return;
				}
			}
			else
			{
				this._rotateSlopeEnterTime -= Time.fixedDeltaTime;
			}
		}

		// Token: 0x0600294A RID: 10570 RVA: 0x000E856F File Offset: 0x000E676F
		public virtual bool FullCapsuleHeightConditions()
		{
			return this.disableSnapToGround || this.customAction || this.isDead || (this.isRolling && !this.isGrounded);
		}

		// Token: 0x0600294B RID: 10571 RVA: 0x000E85A0 File Offset: 0x000E67A0
		public virtual void SetFullCapsuleHeight()
		{
			this._capsuleCollider.height = this.capsuleHeight;
			this._capsuleCollider.center = this.capsuleOffset * this.capsuleHeight;
			this._capsuleCollider.radius = this.capsuleThickness / 2f;
		}

		// Token: 0x0600294C RID: 10572 RVA: 0x000E85F4 File Offset: 0x000E67F4
		public virtual void ControlCapsuleHeight()
		{
			if (this.FullCapsuleHeightConditions())
			{
				this.stepHeightRatio = 0f;
			}
			else
			{
				this.stepHeightRatio = Mathf.LerpUnclamped(this.stepHeightRatio, this.targetHeightRatio, this.lerpCapsuleAdjust ? (this.lerpCapsuleSmooth * Time.fixedDeltaTime) : 1f);
			}
			if (this._capsuleCollider == null)
			{
				this._capsuleCollider = base.GetComponent<CapsuleCollider>();
			}
			if (this.isCrouching && !this.isRolling)
			{
				this.heightReduction = Mathf.Lerp(this.heightReduction, this.capsuleHeightReductionCrouched, this.lerpCapsuleSmooth * Time.fixedDeltaTime);
				this.radiusReduction = Mathf.Lerp(this.radiusReduction, this.capsuleRadiusReductionCrouched, this.lerpCapsuleSmooth * Time.fixedDeltaTime);
			}
			else if (this.isRolling)
			{
				this.heightReduction = Mathf.Lerp(this.heightReduction, this.capsuleHeightReductionRolling, this.lerpCapsuleSmooth * Time.fixedDeltaTime);
				this.radiusReduction = Mathf.Lerp(this.radiusReduction, this.capsuleRadiusReductionRolling, this.lerpCapsuleSmooth * Time.fixedDeltaTime);
			}
			else
			{
				this.heightReduction = Mathf.Lerp(this.heightReduction, 0f, this.lerpCapsuleSmooth * Time.fixedDeltaTime);
				this.radiusReduction = Mathf.Lerp(this.radiusReduction, 0f, this.lerpCapsuleSmooth * Time.fixedDeltaTime);
			}
			float num = this.capsuleHeight;
			Vector3 vector = this.capsuleOffset * this.capsuleHeight;
			float num2 = this.capsuleThickness / 2f;
			num /= 1f + this.heightReduction;
			vector /= 1f + this.heightReduction;
			num2 /= 1f + this.radiusReduction;
			vector += new Vector3(0f, this.stepHeightRatio * this.capsuleHeight / 2f, 0f);
			num -= this.capsuleHeight * this.stepHeightRatio;
			if (num / 2f < num2)
			{
				num2 = num / 2f;
			}
			if (num != this._capsuleCollider.height)
			{
				this._capsuleCollider.height = num;
			}
			if (vector != this._capsuleCollider.center)
			{
				this._capsuleCollider.center = vector;
			}
			if (num2 != this._capsuleCollider.radius)
			{
				this._capsuleCollider.radius = num2;
			}
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x000E8846 File Offset: 0x000E6A46
		public virtual void ResetCapsule()
		{
			this.colliderCenter = this.colliderCenterDefault;
			this.colliderRadius = this.colliderRadiusDefault;
			this.colliderHeight = this.colliderHeightDefault;
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x000E886C File Offset: 0x000E6A6C
		public virtual void DisableGravityAndCollision()
		{
			base.animator.SetFloat("InputHorizontal", 0f);
			base.animator.SetFloat("InputVertical", 0f);
			base.animator.SetFloat("VerticalVelocity", 0f);
			this._rigidbody.useGravity = false;
			this._rigidbody.isKinematic = true;
			this._capsuleCollider.isTrigger = true;
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x000E88DC File Offset: 0x000E6ADC
		public virtual void EnableGravityAndCollision()
		{
			this._capsuleCollider.isTrigger = false;
			this._rigidbody.useGravity = true;
			this._rigidbody.isKinematic = false;
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x000E8904 File Offset: 0x000E6B04
		protected virtual void CheckRagdoll()
		{
			if (this.ragdollVelocity == 0f)
			{
				return;
			}
			if (this.verticalVelocity <= this.ragdollVelocity && this.groundDistance <= 0.1f && this._canApplyFallDamage && !this.ragdolled)
			{
				base.onActiveRagdoll.Invoke(null);
			}
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x000E8958 File Offset: 0x000E6B58
		public override void ResetRagdoll()
		{
			this.onDisableRagdoll.Invoke();
			this.verticalVelocity = 0f;
			this.ragdolled = false;
			this._rigidbody.WakeUp();
			this._rigidbody.useGravity = true;
			this._rigidbody.isKinematic = false;
			this._capsuleCollider.isTrigger = false;
			this._capsuleCollider.enabled = true;
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x000E89C0 File Offset: 0x000E6BC0
		public override void EnableRagdoll()
		{
			this.StopCharacter();
			base.animator.SetFloat("InputHorizontal", 0f);
			base.animator.SetFloat("InputVertical", 0f);
			base.animator.SetFloat("InputMagnitude", 0f);
			base.animator.SetFloat("VerticalVelocity", 0f);
			this.ragdolled = true;
			this._capsuleCollider.isTrigger = true;
			this._rigidbody.useGravity = false;
			this._rigidbody.isKinematic = true;
			this.lockAnimMovement = true;
		}

		// Token: 0x14000046 RID: 70
		// (add) Token: 0x06002953 RID: 10579 RVA: 0x000E8A5C File Offset: 0x000E6C5C
		// (remove) Token: 0x06002954 RID: 10580 RVA: 0x000E8A94 File Offset: 0x000E6C94
		public event vThirdPersonMotor.GetDebugDelegate OnDebug;

		// Token: 0x06002955 RID: 10581 RVA: 0x000E8ACC File Offset: 0x000E6CCC
		public virtual string DebugInfo(string additionalText = "")
		{
			string text = string.Empty;
			if (this.debugWindow)
			{
				float smoothDeltaTime = Time.smoothDeltaTime;
				float num = 1f / smoothDeltaTime;
				string[] array = new string[58];
				array[0] = " \nFPS ";
				array[1] = num.ToString("#,##0 fps");
				array[2] = "\nHealth = ";
				array[3] = this.currentHealth.ToString();
				array[4] = "\nInput Vertical = ";
				int num2 = 5;
				Vector3 vector = this.inputSmooth;
				array[num2] = vector.z.ToString("0.0");
				array[6] = "\nInput Horizontal = ";
				int num3 = 7;
				vector = this.inputSmooth;
				array[num3] = vector.x.ToString("0.0");
				array[8] = "\nInput Magnitude = ";
				array[9] = this.inputMagnitude.ToString("0.0");
				array[10] = "\nRotation Magnitude = ";
				array[11] = this.rotationMagnitude.ToString("0.0");
				array[12] = "\nVertical Velocity = ";
				array[13] = this.verticalVelocity.ToString("0.00");
				array[14] = "\nCurrent MoveSpeed = ";
				array[15] = this.moveSpeed.ToString("0.00");
				array[16] = "\nGround Distance = ";
				array[17] = this.groundDistance.ToString("0.00");
				array[18] = "\nGround Angle = ";
				array[19] = this.GroundAngleFromDirection().ToString("0.00");
				array[20] = "\nIs Grounded = ";
				array[21] = this.BoolToRichText(this.isGrounded);
				array[22] = "\nCeiling Contact = ";
				array[23] = this.BoolToRichText(this.hasCeilingContact);
				array[24] = "\nIs Strafing = ";
				array[25] = this.BoolToRichText(this.isStrafing);
				array[26] = "\nIs Trigger = ";
				array[27] = this.BoolToRichText(this._capsuleCollider.isTrigger);
				array[28] = "\nUse Gravity = ";
				array[29] = this.BoolToRichText(this._rigidbody.useGravity);
				array[30] = "\nIs Kinematic = ";
				array[31] = this.BoolToRichText(this._rigidbody.isKinematic);
				array[32] = "\nLock Movement = ";
				array[33] = this.BoolToRichText(this.lockMovement);
				array[34] = "\nLock AnimMov = ";
				array[35] = this.BoolToRichText(this.lockAnimMovement);
				array[36] = "\nLock Rotation = ";
				array[37] = this.BoolToRichText(this.lockRotation);
				array[38] = "\nLock AnimRot = ";
				array[39] = this.BoolToRichText(this.lockAnimRotation);
				array[40] = "\n--- Actions Bools ---\nIs Sliding = ";
				array[41] = this.BoolToRichText(this.isSliding);
				array[42] = "\nIs Sprinting = ";
				array[43] = this.BoolToRichText(this.isSprinting);
				array[44] = "\nIs Crouching = ";
				array[45] = this.BoolToRichText(this.isCrouching);
				array[46] = "\nIs Rolling = ";
				array[47] = this.BoolToRichText(this.isRolling);
				array[48] = "\nIs Jumping = ";
				array[49] = this.BoolToRichText(this.isJumping);
				array[50] = "\nIs Airborne = ";
				array[51] = this.BoolToRichText(this.isInAirborne);
				array[52] = "\nIs Ragdolled = ";
				array[53] = this.BoolToRichText(this.ragdolled);
				array[54] = "\nCustomAction = ";
				array[55] = this.BoolToRichText(this.customAction);
				array[56] = "\n";
				array[57] = additionalText;
				text = string.Concat(array);
			}
			if (this.OnDebug != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				this.OnDebug(ref stringBuilder);
				text += stringBuilder.ToString();
			}
			return text;
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x000E8E43 File Offset: 0x000E7043
		protected virtual string BoolToRichText(bool value)
		{
			if (!value)
			{
				return "<color=red> False </color>";
			}
			return "<color=yellow> True </color>";
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x000E8E54 File Offset: 0x000E7054
		protected virtual void OnDrawGizmos()
		{
			if (Application.isPlaying && this.debugWindow)
			{
				Vector3 vector = base.transform.position + Vector3.up * (this.colliderHeight * 0.5f - this.colliderRadius);
				Ray ray = new Ray(vector, Vector3.up);
				Gizmos.DrawWireSphere(ray.GetPoint(this.crouchHeadDetect - this.colliderRadius * 0.1f), this.colliderRadius * 0.9f);
			}
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x000E8ED8 File Offset: 0x000E70D8
		protected virtual void OnDrawGizmosSelected()
		{
			if (!this._capsuleCollider)
			{
				this._capsuleCollider = base.GetComponent<CapsuleCollider>();
			}
			if (this._capsuleCollider)
			{
				Matrix4x4 matrix = Gizmos.matrix;
				Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, this.groundPlane.planeNormal) * Quaternion.AngleAxis(base.transform.rotation.eulerAngles.y, Vector3.up);
				Gizmos.color = Color.red * 0.8f;
				if (this.isGrounded)
				{
					Gizmos.matrix = Matrix4x4.TRS(this.groundPlane.planeCenter, quaternion, new Vector3(this.planeSize * 2f, 0f, this.planeSize * 2f));
					Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
					Gizmos.matrix = matrix;
					Gizmos.DrawSphere(this.groundPlane.planeCenter, 0.1f);
				}
				Gizmos.matrix = matrix;
				Vector3 vector = base.transform.position + base.transform.up * this._capsuleCollider.radius;
				Vector3 vector2 = base.transform.position + base.transform.up * (this.capsuleHeight * this.stepHeightRatio + this._capsuleCollider.radius);
				Gizmos.matrix = Matrix4x4.TRS(vector, base.transform.rotation, Vector3.one);
				Gizmos.DrawWireSphere(Vector3.zero, this._capsuleCollider.radius);
				Gizmos.matrix = matrix;
				Gizmos.DrawLine(vector + base.transform.right * this._capsuleCollider.radius, vector2 + base.transform.right * this._capsuleCollider.radius);
				Gizmos.DrawLine(vector - base.transform.right * this._capsuleCollider.radius, vector2 - base.transform.right * this._capsuleCollider.radius);
				Gizmos.DrawLine(vector + base.transform.forward * this._capsuleCollider.radius, vector2 + base.transform.forward * this._capsuleCollider.radius);
				Gizmos.DrawLine(vector - base.transform.forward * this._capsuleCollider.radius, vector2 - base.transform.forward * this._capsuleCollider.radius);
			}
		}

		// Token: 0x040028B0 RID: 10416
		[vEditorToolbar("Stamina", false, "", false, false, order = 2)]
		[SerializeField]
		protected float _maxStamina = 200f;

		// Token: 0x040028B1 RID: 10417
		[SerializeField]
		protected float _staminaRecovery = 1.2f;

		// Token: 0x040028B2 RID: 10418
		internal float currentStamina;

		// Token: 0x040028B3 RID: 10419
		internal float currentStaminaRecoveryDelay;

		// Token: 0x040028B4 RID: 10420
		[NonSerialized]
		public bool useBuiltInStamina = true;

		// Token: 0x040028B5 RID: 10421
		[NonSerialized]
		public bool ignoreStaminaConsumption;

		// Token: 0x040028B6 RID: 10422
		[SerializeField]
		protected float _sprintStamina = 30f;

		// Token: 0x040028B7 RID: 10423
		[SerializeField]
		protected float _jumpStamina = 30f;

		// Token: 0x040028B8 RID: 10424
		[SerializeField]
		protected float _rollStamina = 25f;

		// Token: 0x040028B9 RID: 10425
		[vEditorToolbar("Events", false, "", false, false, order = 7)]
		public UnityEvent OnExitGround;

		// Token: 0x040028BA RID: 10426
		public UnityEvent OnGrounded;

		// Token: 0x040028BB RID: 10427
		public UnityEvent OnRoll;

		// Token: 0x040028BC RID: 10428
		public UnityEvent OnJump;

		// Token: 0x040028BD RID: 10429
		public UnityEvent OnStartSprinting;

		// Token: 0x040028BE RID: 10430
		public UnityEvent OnFinishSprinting;

		// Token: 0x040028BF RID: 10431
		public UnityEvent OnFinishSprintingByStamina;

		// Token: 0x040028C0 RID: 10432
		public UnityEvent OnStaminaEnd;

		// Token: 0x040028C1 RID: 10433
		[vEditorToolbar("Crouch", false, "", false, false, order = 3)]
		[Tooltip("Capsule radius reduction while in crouch state")]
		[Range(0f, 1f)]
		public float capsuleHeightReductionCrouched = 0.5f;

		// Token: 0x040028C2 RID: 10434
		[Tooltip("Capsule height reduction while in crouch state")]
		[Range(0f, 1f)]
		public float capsuleRadiusReductionCrouched;

		// Token: 0x040028C3 RID: 10435
		[Tooltip("What objects can make the character auto crouch")]
		public LayerMask autoCrouchLayer = 1;

		// Token: 0x040028C4 RID: 10436
		[Tooltip("[SPHERECAST] ADJUST IN PLAY MODE - White Spherecast put just above the head, this will make the character Auto-Crouch if something hit the sphere.")]
		public float crouchHeadDetect = 0.95f;

		// Token: 0x040028C5 RID: 10437
		[vEditorToolbar("Physics Material", false, "", false, false)]
		public PhysicMaterial idleMaterial;

		// Token: 0x040028C6 RID: 10438
		public PhysicMaterial movingMaterial;

		// Token: 0x040028C7 RID: 10439
		public PhysicMaterial airborneMaterial;

		// Token: 0x040028C8 RID: 10440
		[vEditorToolbar("Locomotion", false, "", false, false, order = 0)]
		[vSeparator("Movement Settings", "")]
		[Tooltip("Multiply the current speed of the controller rigidbody velocity")]
		[SerializeField]
		protected float _speedMultiplier = 1f;

		// Token: 0x040028C9 RID: 10441
		[NonSerialized]
		public float automaticMovementSpeedMultiplier = 1f;

		// Token: 0x040028CA RID: 10442
		[Tooltip("Use this to rotate the character using the World axis, or false to use the camera axis - CHECK for Isometric Camera")]
		public bool rotateByWorld;

		// Token: 0x040028CB RID: 10443
		[Tooltip("The Character always  move to forward direction and rotate to Input direction when in free locomotion.\nUse this to move and rotate the character to Input direction when in free locomotion")]
		public bool moveToDirectionInFree;

		// Token: 0x040028CC RID: 10444
		[vHelpBox("FreeLocomotion: Rotate on any direction regardless of the camera \nStrafeLocomotion: Move always facing forward (extra directional animations)", vHelpBoxAttribute.MessageType.None)]
		[SerializeField]
		[FormerlySerializedAs("locomotionType")]
		protected vThirdPersonMotor.LocomotionType _locomotionType;

		// Token: 0x040028CD RID: 10445
		public vThirdPersonMotor.vMovementSpeed freeSpeed;

		// Token: 0x040028CE RID: 10446
		public vThirdPersonMotor.vMovementSpeed strafeSpeed;

		// Token: 0x040028CF RID: 10447
		[vSeparator("Extra Animation Settings", "")]
		[Tooltip("Use it for debug purposes")]
		public bool disableAnimations;

		// Token: 0x040028D0 RID: 10448
		[Tooltip("Turn off if you have 'in place' animations and use this values above to move the character, or use with root motion as extra speed")]
		[vHelpBox("When 'Use RootMotion' is checked, make sure to reset all speeds to zero to use the original root motion velocity.", vHelpBoxAttribute.MessageType.None)]
		public bool useRootMotion;

		// Token: 0x040028D1 RID: 10449
		[Tooltip("While in Free Locomotion the character will lean to left/right when steering")]
		public bool useLeanMovementAnim = true;

		// Token: 0x040028D2 RID: 10450
		[Tooltip("Smooth value for the Lean Movement animation")]
		[Range(0.01f, 0.1f)]
		public float leanSmooth = 0.05f;

		// Token: 0x040028D3 RID: 10451
		[Tooltip("Check this to use the TurnOnSpot animations while the character is stading still and rotating in place")]
		public bool useTurnOnSpotAnim = true;

		// Token: 0x040028D4 RID: 10452
		public float turnOnSpotSmooth = 0.01f;

		// Token: 0x040028D5 RID: 10453
		[Tooltip("Put your Random Idle animations at the AnimatorController and select a value to randomize, 0 is disable.")]
		public float randomIdleTime;

		// Token: 0x040028D6 RID: 10454
		internal bool ignoreAnimatorMovement;

		// Token: 0x040028D7 RID: 10455
		[vSeparator("Extra Movement Settings", "")]
		[Tooltip("Check This to use run on press button\nIf uncheck your Character will run as long as the RunInput is pressed")]
		public bool useContinuousRun = true;

		// Token: 0x040028D8 RID: 10456
		[Tooltip("Check This to use sprint on press button to your Character run until the stamina finish or movement stops\nIf uncheck your Character will sprint as long as the SprintInput is pressed or the stamina finishes")]
		public bool useContinuousSprint = true;

		// Token: 0x040028D9 RID: 10457
		[Tooltip("Check this to sprint always in free movement")]
		public bool sprintOnlyFree = true;

		// Token: 0x040028DA RID: 10458
		[vHelpBox("Set the FixedTimeStep to match the FPS of your Game, \nEx: If your game aims to run at 30fps, select FPS30 to match the FixedUpdate Physics", vHelpBoxAttribute.MessageType.None)]
		public vThirdPersonMotor.CustomFixedTimeStep customFixedTimeStep = vThirdPersonMotor.CustomFixedTimeStep.FPS60;

		// Token: 0x040028DB RID: 10459
		[vEditorToolbar("Jump / Airborne", false, "", false, false, order = 3)]
		[vHelpBox("Jump only works via Rigidbody Physics, if you want Jump that use only RootMotion make sure to use the AnimatorTag 'CustomAction' ", vHelpBoxAttribute.MessageType.None)]
		[vSeparator("Jump", "")]
		[Tooltip("Use the currently Rigidbody Velocity to influence on the Jump Distance")]
		public bool jumpWithRigidbodyForce;

		// Token: 0x040028DC RID: 10460
		[Tooltip("Rotate or not while airborne")]
		public bool jumpAndRotate = true;

		// Token: 0x040028DD RID: 10461
		[Tooltip("How much time the character will be jumping")]
		public float jumpTimer = 0.3f;

		// Token: 0x040028DE RID: 10462
		[Tooltip("Delay to match the animation anticipation")]
		public float jumpStandingDelay = 0.25f;

		// Token: 0x040028DF RID: 10463
		internal float jumpCounter;

		// Token: 0x040028E0 RID: 10464
		internal bool inJumpStarted;

		// Token: 0x040028E1 RID: 10465
		[Tooltip("Add Extra jump height, if you want to jump only with Root Motion leave the value with 0.")]
		[SerializeField]
		protected float _jumpHeight = 4f;

		// Token: 0x040028E2 RID: 10466
		[vSeparator("Falling", "")]
		[Tooltip("Speed that the character will move while airborne")]
		[SerializeField]
		protected float _airSpeed = 5f;

		// Token: 0x040028E3 RID: 10467
		[Tooltip("Smoothness of the direction while airborne")]
		[SerializeField]
		protected float _airSmooth = 6f;

		// Token: 0x040028E4 RID: 10468
		[Tooltip("Apply extra gravity when the character is not grounded")]
		[SerializeField]
		protected float _extraGravity = -10f;

		// Token: 0x040028E5 RID: 10469
		[Tooltip("Limit of the vertical velocity when Falling")]
		[SerializeField]
		protected float _limitFallVelocity = -15f;

		// Token: 0x040028E6 RID: 10470
		[Tooltip("Turn the Ragdoll On when falling at high speed (check VerticalVelocity) - leave the value with 0 if you don't want this feature")]
		[SerializeField]
		protected float _ragdollVelocity = -15f;

		// Token: 0x040028E7 RID: 10471
		[vSeparator("Fall Damage", "")]
		[SerializeField]
		protected float _fallMinHeight = 6f;

		// Token: 0x040028E8 RID: 10472
		[SerializeField]
		protected float _fallMinVerticalVelocity = -10f;

		// Token: 0x040028E9 RID: 10473
		[SerializeField]
		protected float _fallDamage = 10f;

		// Token: 0x040028EA RID: 10474
		[vEditorToolbar("Roll", false, "", false, false, order = 4)]
		public bool useRollRootMotion = true;

		// Token: 0x040028EB RID: 10475
		[Tooltip("Animation Transition from current animation to Roll")]
		public float rollTransition = 0.25f;

		// Token: 0x040028EC RID: 10476
		[Tooltip("Capsule height reduction while in roll state")]
		[Range(0f, 1f)]
		public float capsuleHeightReductionRolling = 0.5f;

		// Token: 0x040028ED RID: 10477
		[Tooltip("Capsule radius reduction while in roll state")]
		[Range(0f, 1f)]
		public float capsuleRadiusReductionRolling;

		// Token: 0x040028EE RID: 10478
		[Tooltip("Can control the Roll Direction")]
		public bool rollControl = true;

		// Token: 0x040028EF RID: 10479
		[Tooltip("Speed of the Roll Movement")]
		[SerializeField]
		protected float _rollSpeed;

		// Token: 0x040028F0 RID: 10480
		[Tooltip("Speed of the Roll Rotation")]
		[SerializeField]
		protected float _rollRotationSpeed = 20f;

		// Token: 0x040028F1 RID: 10481
		[Tooltip("Extra gravity when rolling, this value can be negative to fall faster OR positive to delay the falling")]
		[SerializeField]
		protected float _rollUseGravityTime = -10f;

		// Token: 0x040028F2 RID: 10482
		[Tooltip("Use the normalized time of the animation to know when you can roll again")]
		[Range(0f, 1f)]
		[SerializeField]
		protected float _timeToRollAgain = 0.75f;

		// Token: 0x040028F3 RID: 10483
		[Tooltip("Ignore all damage while is rolling, include Damage that ignore defense")]
		[SerializeField]
		protected bool _noDamageWhileRolling = true;

		// Token: 0x040028F4 RID: 10484
		[Tooltip("Ignore damage that needs to activate ragdoll")]
		[SerializeField]
		protected bool _noActiveRagdollWhileRolling = true;

		// Token: 0x040028F5 RID: 10485
		[vEditorToolbar("Grounded", false, "", false, false, order = 3)]
		[vSeparator("Ground Detection", "")]
		[SerializeField]
		[vReadOnly(true)]
		protected bool _isGrounded;

		// Token: 0x040028F6 RID: 10486
		[Tooltip("Layers that the character can walk on")]
		public LayerMask groundLayer = 1;

		// Token: 0x040028F7 RID: 10487
		public float groundMinDistance = 0.5f;

		// Token: 0x040028F8 RID: 10488
		public float groundMaxDistance = 0.1f;

		// Token: 0x040028F9 RID: 10489
		public float sphereCastRadius = 0.3f;

		// Token: 0x040028FA RID: 10490
		public float castLengthAirborne = 1.5f;

		// Token: 0x040028FB RID: 10491
		public float castLengthGrounded = 3f;

		// Token: 0x040028FC RID: 10492
		public bool useSphereCast = true;

		// Token: 0x040028FD RID: 10493
		public float planeSize = 0.25f;

		// Token: 0x040028FE RID: 10494
		public float stepHeight = 0.3f;

		// Token: 0x040028FF RID: 10495
		public float groundAngleSmooth = 4f;

		// Token: 0x04002900 RID: 10496
		[vSeparator("Collider Options", "")]
		public bool useCeilingDetection = true;

		// Token: 0x04002901 RID: 10497
		public bool lerpCapsuleAdjust = true;

		// Token: 0x04002902 RID: 10498
		public float lerpCapsuleSmooth = 10f;

		// Token: 0x04002903 RID: 10499
		public Vector3 capsuleOffset = new Vector3(0f, 0.5f, 0f);

		// Token: 0x04002904 RID: 10500
		public float capsuleThickness = 0.75f;

		// Token: 0x04002905 RID: 10501
		public float capsuleHeight = 1.8f;

		// Token: 0x04002906 RID: 10502
		[vSeparator("Snap to Ground", "")]
		public float snapFactor = 15f;

		// Token: 0x04002907 RID: 10503
		public float maxSnapVelocityUp = 5f;

		// Token: 0x04002908 RID: 10504
		public float maxSnapVelocityDown = 15f;

		// Token: 0x04002909 RID: 10505
		public float snapFactorSpeed = 2f;

		// Token: 0x0400290A RID: 10506
		public float slideSpeed = 1f;

		// Token: 0x0400290B RID: 10507
		protected vGroundPlaneCast groundPlane;

		// Token: 0x0400290C RID: 10508
		protected RaycastHit groundAngleHit;

		// Token: 0x0400290D RID: 10509
		protected RaycastHit stepAheadHit;

		// Token: 0x0400290E RID: 10510
		protected float stepHeightRatio;

		// Token: 0x0400290F RID: 10511
		protected float targetHeightRatio;

		// Token: 0x04002910 RID: 10512
		protected float lastTrueSlope;

		// Token: 0x04002911 RID: 10513
		protected float groundPlaneProjectionWeight;

		// Token: 0x04002912 RID: 10514
		protected float slopeSlideFactor;

		// Token: 0x04002913 RID: 10515
		protected float heightReduction;

		// Token: 0x04002914 RID: 10516
		protected float radiusReduction;

		// Token: 0x04002915 RID: 10517
		private bool _isOnSlope;

		// Token: 0x04002916 RID: 10518
		[vSeparator("StopMove", "")]
		public LayerMask stopMoveLayer;

		// Token: 0x04002917 RID: 10519
		[vHelpBox("Character will stop moving, ex: walls - set the layer to nothing to not use", vHelpBoxAttribute.MessageType.None)]
		public float stopMoveRayDistance = 1f;

		// Token: 0x04002918 RID: 10520
		public float stopMoveMaxHeight = 1.6f;

		// Token: 0x04002919 RID: 10521
		public vThirdPersonMotor.StopMoveCheckMethod stopMoveCheckMethod;

		// Token: 0x0400291A RID: 10522
		[vSeparator("Slope", "")]
		[Range(30f, 80f)]
		[SerializeField]
		protected float _slopeLimit = 75f;

		// Token: 0x0400291B RID: 10523
		[vHelpBox("Slide on Slopes is not available yet, coming soon", vHelpBoxAttribute.MessageType.None)]
		[SerializeField]
		protected bool _useSlide = true;

		// Token: 0x0400291C RID: 10524
		[Tooltip("Velocity to slide down when on a slope limit ramp")]
		[Range(0f, 30f)]
		[SerializeField]
		protected float _slideDownVelocity = 10f;

		// Token: 0x0400291D RID: 10525
		[Tooltip("Smooth to slide down the controller")]
		[SerializeField]
		protected float _slideDownSmooth = 2f;

		// Token: 0x0400291E RID: 10526
		[Tooltip("Velocity to slide sideways when on a slope limit ramp")]
		[Range(0f, 1f)]
		[SerializeField]
		protected float _slideSidewaysVelocity = 0.5f;

		// Token: 0x0400291F RID: 10527
		[Range(0f, 1f)]
		[Tooltip("Delay to start sliding once the character is standing on a slope")]
		[SerializeField]
		protected float _SlidingEnterTime = 0.2f;

		// Token: 0x04002920 RID: 10528
		internal float _slidingEnterTime;

		// Token: 0x04002921 RID: 10529
		[Range(0f, 1f)]
		[Tooltip("Delay to rotate once the character started sliding")]
		[SerializeField]
		protected float _RotateSlopeEnterTime = 0.1f;

		// Token: 0x04002922 RID: 10530
		[Tooltip("Smooth to rotate the controller")]
		[SerializeField]
		protected float _rotateDownSlopeSmooth = 8f;

		// Token: 0x04002923 RID: 10531
		internal float _rotateSlopeEnterTime;

		// Token: 0x04002924 RID: 10532
		internal float _stopMoveWeight;

		// Token: 0x04002925 RID: 10533
		internal float _sprintWeight;

		// Token: 0x04002926 RID: 10534
		internal float groundDistance;

		// Token: 0x04002927 RID: 10535
		public RaycastHit groundHit;

		// Token: 0x04002928 RID: 10536
		[vEditorToolbar("Debug", false, "", false, false, order = 9)]
		[Header("--- Debug Info ---")]
		public bool debugWindow;

		// Token: 0x04002929 RID: 10537
		[vReadOnly(true)]
		public float currentSnapFactor;

		// Token: 0x0400292A RID: 10538
		[vReadOnly(true)]
		public bool rayHasDetectedHit;

		// Token: 0x0400292B RID: 10539
		[vReadOnly(true)]
		public bool sphereHasDetectedHit;

		// Token: 0x0400292C RID: 10540
		public vAnimatorStateInfos _animatorStateInfos;

		// Token: 0x0400292F RID: 10543
		[vReadOnly(true)]
		protected bool _isRunning;

		// Token: 0x04002930 RID: 10544
		[vReadOnly(true)]
		protected bool _isSprinting;

		// Token: 0x04002933 RID: 10547
		internal bool isRolling;

		// Token: 0x04002934 RID: 10548
		internal bool isJumping;

		// Token: 0x04002935 RID: 10549
		internal bool isInAirborne;

		// Token: 0x04002936 RID: 10550
		internal bool isTurningOnSpot;

		// Token: 0x04002937 RID: 10551
		internal bool customAction;

		// Token: 0x04002938 RID: 10552
		internal Rigidbody _rigidbody;

		// Token: 0x04002939 RID: 10553
		internal float changeMaterialPhysics;

		// Token: 0x0400293A RID: 10554
		internal CapsuleCollider _capsuleCollider;

		// Token: 0x04002964 RID: 10596
		public RaycastHit stepOffsetHit;

		// Token: 0x04002965 RID: 10597
		public RaycastHit slopeHitInfo;

		// Token: 0x04002966 RID: 10598
		internal AnimatorStateInfo baseLayerInfo;

		// Token: 0x04002967 RID: 10599
		internal AnimatorStateInfo underBodyInfo;

		// Token: 0x04002968 RID: 10600
		internal AnimatorStateInfo rightArmInfo;

		// Token: 0x04002969 RID: 10601
		internal AnimatorStateInfo leftArmInfo;

		// Token: 0x0400296A RID: 10602
		internal AnimatorStateInfo fullBodyInfo;

		// Token: 0x0400296B RID: 10603
		internal AnimatorStateInfo upperBodyInfo;

		// Token: 0x020006A2 RID: 1698
		public enum LocomotionType
		{
			// Token: 0x04002973 RID: 10611
			FreeWithStrafe,
			// Token: 0x04002974 RID: 10612
			OnlyStrafe,
			// Token: 0x04002975 RID: 10613
			OnlyFree
		}

		// Token: 0x020006A3 RID: 1699
		public enum CustomFixedTimeStep
		{
			// Token: 0x04002977 RID: 10615
			Default,
			// Token: 0x04002978 RID: 10616
			FPS30,
			// Token: 0x04002979 RID: 10617
			FPS60,
			// Token: 0x0400297A RID: 10618
			FPS75,
			// Token: 0x0400297B RID: 10619
			FPS90,
			// Token: 0x0400297C RID: 10620
			FPS120,
			// Token: 0x0400297D RID: 10621
			FPS144
		}

		// Token: 0x020006A4 RID: 1700
		public enum StopMoveCheckMethod
		{
			// Token: 0x0400297F RID: 10623
			RayCast,
			// Token: 0x04002980 RID: 10624
			SphereCast,
			// Token: 0x04002981 RID: 10625
			CapsuleCast
		}

		// Token: 0x020006A5 RID: 1701
		// (Invoke) Token: 0x0600295B RID: 10587
		public delegate void GetDebugDelegate(ref StringBuilder stringBuilder);

		// Token: 0x020006A6 RID: 1702
		[Serializable]
		public class vMovementSpeed
		{
			// Token: 0x17000790 RID: 1936
			// (get) Token: 0x0600295E RID: 10590 RVA: 0x000E947D File Offset: 0x000E767D
			// (set) Token: 0x0600295F RID: 10591 RVA: 0x000E9485 File Offset: 0x000E7685
			public float defaultMovementSmooth { get; set; }

			// Token: 0x17000791 RID: 1937
			// (get) Token: 0x06002960 RID: 10592 RVA: 0x000E948E File Offset: 0x000E768E
			// (set) Token: 0x06002961 RID: 10593 RVA: 0x000E9496 File Offset: 0x000E7696
			public float defaultAnimationSmooth { get; set; }

			// Token: 0x17000792 RID: 1938
			// (get) Token: 0x06002962 RID: 10594 RVA: 0x000E949F File Offset: 0x000E769F
			// (set) Token: 0x06002963 RID: 10595 RVA: 0x000E94A7 File Offset: 0x000E76A7
			public float defaultRotationSpeed { get; set; }

			// Token: 0x17000793 RID: 1939
			// (get) Token: 0x06002964 RID: 10596 RVA: 0x000E94B0 File Offset: 0x000E76B0
			// (set) Token: 0x06002965 RID: 10597 RVA: 0x000E94B8 File Offset: 0x000E76B8
			public bool defaultWalkByDefault { get; set; }

			// Token: 0x17000794 RID: 1940
			// (get) Token: 0x06002966 RID: 10598 RVA: 0x000E94C1 File Offset: 0x000E76C1
			// (set) Token: 0x06002967 RID: 10599 RVA: 0x000E94C9 File Offset: 0x000E76C9
			public bool defaultRotateWithCamera { get; set; }

			// Token: 0x17000795 RID: 1941
			// (get) Token: 0x06002968 RID: 10600 RVA: 0x000E94D2 File Offset: 0x000E76D2
			// (set) Token: 0x06002969 RID: 10601 RVA: 0x000E94DA File Offset: 0x000E76DA
			public float defaultWalkSpeed { get; set; }

			// Token: 0x17000796 RID: 1942
			// (get) Token: 0x0600296A RID: 10602 RVA: 0x000E94E3 File Offset: 0x000E76E3
			// (set) Token: 0x0600296B RID: 10603 RVA: 0x000E94EB File Offset: 0x000E76EB
			public float defaultRunningSpeed { get; set; }

			// Token: 0x17000797 RID: 1943
			// (get) Token: 0x0600296C RID: 10604 RVA: 0x000E94F4 File Offset: 0x000E76F4
			// (set) Token: 0x0600296D RID: 10605 RVA: 0x000E94FC File Offset: 0x000E76FC
			public float defaultSprintSpeed { get; set; }

			// Token: 0x17000798 RID: 1944
			// (get) Token: 0x0600296E RID: 10606 RVA: 0x000E9505 File Offset: 0x000E7705
			// (set) Token: 0x0600296F RID: 10607 RVA: 0x000E950D File Offset: 0x000E770D
			public float defaultCrouchSpeed { get; set; }

			// Token: 0x06002970 RID: 10608 RVA: 0x000E9518 File Offset: 0x000E7718
			public void Init()
			{
				this.defaultMovementSmooth = this.movementSmooth;
				this.defaultAnimationSmooth = this.animationSmooth;
				this.defaultRotationSpeed = this.rotationSpeed;
				this.defaultWalkByDefault = this.walkByDefault;
				this.defaultRotateWithCamera = this.rotateWithCamera;
				this.defaultWalkSpeed = this.walkSpeed;
				this.defaultRunningSpeed = this.runningSpeed;
				this.defaultSprintSpeed = this.sprintSpeed;
				this.defaultCrouchSpeed = this.crouchSpeed;
			}

			// Token: 0x06002971 RID: 10609 RVA: 0x000E9594 File Offset: 0x000E7794
			public void ResetToDefault()
			{
				this.movementSmooth = this.defaultMovementSmooth;
				this.animationSmooth = this.defaultAnimationSmooth;
				this.rotationSpeed = this.defaultRotationSpeed;
				this.walkByDefault = this.defaultWalkByDefault;
				this.rotateWithCamera = this.defaultRotateWithCamera;
				this.walkSpeed = this.defaultWalkSpeed;
				this.runningSpeed = this.defaultRunningSpeed;
				this.sprintSpeed = this.defaultSprintSpeed;
				this.crouchSpeed = this.defaultCrouchSpeed;
			}

			// Token: 0x04002982 RID: 10626
			[vHelpBox("Higher means faster/responsive movement, lower means smooth movement", vHelpBoxAttribute.MessageType.None)]
			[Range(1f, 20f)]
			[FormerlySerializedAs("movementSmooth")]
			[SerializeField]
			public float movementSmooth = 6f;

			// Token: 0x04002983 RID: 10627
			[vHelpBox("Lower means faster transitions between animations, higher means slower", vHelpBoxAttribute.MessageType.None)]
			[Range(0f, 1f)]
			[FormerlySerializedAs("animationSmooth")]
			[SerializeField]
			public float animationSmooth = 0.2f;

			// Token: 0x04002984 RID: 10628
			[Tooltip("Rotation speed of the character")]
			[FormerlySerializedAs("rotationSpeed")]
			[SerializeField]
			public float rotationSpeed = 20f;

			// Token: 0x04002985 RID: 10629
			[Tooltip("Character will limit the movement to walk instead of running")]
			[FormerlySerializedAs("walkByDefault")]
			[SerializeField]
			public bool walkByDefault;

			// Token: 0x04002986 RID: 10630
			[Tooltip("Rotate with the Camera forward when standing idle")]
			[FormerlySerializedAs("rotateWithCamera")]
			[SerializeField]
			public bool rotateWithCamera;

			// Token: 0x04002987 RID: 10631
			[Tooltip("Speed to Walk using rigidbody or extra speed if you're using RootMotion")]
			[FormerlySerializedAs("walkSpeed")]
			[SerializeField]
			public float walkSpeed = 2f;

			// Token: 0x04002988 RID: 10632
			[Tooltip("Speed to Run using rigidbody or extra speed if you're using RootMotion")]
			[FormerlySerializedAs("runningSpeed")]
			[SerializeField]
			public float runningSpeed = 4f;

			// Token: 0x04002989 RID: 10633
			[Tooltip("Speed to Sprint using rigidbody or extra speed if you're using RootMotion")]
			[FormerlySerializedAs("sprintSpeed")]
			[SerializeField]
			public float sprintSpeed = 6f;

			// Token: 0x0400298A RID: 10634
			[Tooltip("Speed to Crouch using rigidbody or extra speed if you're using RootMotion")]
			[FormerlySerializedAs("crouchSpeed")]
			[SerializeField]
			public float crouchSpeed = 2f;
		}
	}
}
