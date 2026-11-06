using System;
using System.Collections.Generic;
using Invector.vEventSystems;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vCharacterController
{
	// Token: 0x020006AF RID: 1711
	[vClassHeader("HEAD TRACK", true, "icon_v2", false, "", iconName = "headTrackIcon")]
	public class vHeadTrack : vMonoBehaviour
	{
		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x0600299A RID: 10650 RVA: 0x000EA4A3 File Offset: 0x000E86A3
		// (set) Token: 0x0600299B RID: 10651 RVA: 0x000EA4AB File Offset: 0x000E86AB
		public virtual bool alwaysFollowCamera
		{
			get
			{
				return this._alwaysFollowCamera;
			}
			set
			{
				this._alwaysFollowCamera = value;
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x0600299C RID: 10652 RVA: 0x000EA4B4 File Offset: 0x000E86B4
		// (set) Token: 0x0600299D RID: 10653 RVA: 0x000EA4BC File Offset: 0x000E86BC
		public virtual Camera cameraMain { get; set; }

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x0600299E RID: 10654 RVA: 0x000EA4C5 File Offset: 0x000E86C5
		// (set) Token: 0x0600299F RID: 10655 RVA: 0x000EA4CD File Offset: 0x000E86CD
		public virtual vLookTarget currentLookTarget { get; set; }

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x060029A0 RID: 10656 RVA: 0x000EA4D6 File Offset: 0x000E86D6
		// (set) Token: 0x060029A1 RID: 10657 RVA: 0x000EA4DE File Offset: 0x000E86DE
		public virtual vLookTarget lastLookTarget { get; set; }

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x060029A2 RID: 10658 RVA: 0x000EA4E7 File Offset: 0x000E86E7
		// (set) Token: 0x060029A3 RID: 10659 RVA: 0x000EA4EF File Offset: 0x000E86EF
		public virtual Quaternion currentLookRotation { get; set; }

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x060029A4 RID: 10660 RVA: 0x000EA4F8 File Offset: 0x000E86F8
		// (set) Token: 0x060029A5 RID: 10661 RVA: 0x000EA500 File Offset: 0x000E8700
		public virtual bool ignoreSmooth { get; set; }

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x060029A6 RID: 10662 RVA: 0x000EA509 File Offset: 0x000E8709
		// (set) Token: 0x060029A7 RID: 10663 RVA: 0x000EA511 File Offset: 0x000E8711
		protected virtual float yRotation { get; set; }

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x060029A8 RID: 10664 RVA: 0x000EA51A File Offset: 0x000E871A
		// (set) Token: 0x060029A9 RID: 10665 RVA: 0x000EA522 File Offset: 0x000E8722
		protected virtual float xRotation { get; set; }

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x060029AA RID: 10666 RVA: 0x000EA52B File Offset: 0x000E872B
		// (set) Token: 0x060029AB RID: 10667 RVA: 0x000EA533 File Offset: 0x000E8733
		protected virtual float _currentHeadWeight { get; set; }

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x060029AC RID: 10668 RVA: 0x000EA53C File Offset: 0x000E873C
		// (set) Token: 0x060029AD RID: 10669 RVA: 0x000EA544 File Offset: 0x000E8744
		protected virtual float _currentBodyWeight { get; set; }

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x060029AE RID: 10670 RVA: 0x000EA54D File Offset: 0x000E874D
		// (set) Token: 0x060029AF RID: 10671 RVA: 0x000EA555 File Offset: 0x000E8755
		protected virtual Animator animator { get; set; }

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x060029B0 RID: 10672 RVA: 0x000EA55E File Offset: 0x000E875E
		// (set) Token: 0x060029B1 RID: 10673 RVA: 0x000EA566 File Offset: 0x000E8766
		protected virtual vIAnimatorStateInfoController animatorStateInfo { get; set; }

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x060029B2 RID: 10674 RVA: 0x000EA56F File Offset: 0x000E876F
		// (set) Token: 0x060029B3 RID: 10675 RVA: 0x000EA577 File Offset: 0x000E8777
		protected virtual float headHeight { get; set; }

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x060029B4 RID: 10676 RVA: 0x000EA580 File Offset: 0x000E8780
		// (set) Token: 0x060029B5 RID: 10677 RVA: 0x000EA588 File Offset: 0x000E8788
		protected virtual Transform simpleTarget { get; set; }

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x060029B6 RID: 10678 RVA: 0x000EA591 File Offset: 0x000E8791
		// (set) Token: 0x060029B7 RID: 10679 RVA: 0x000EA599 File Offset: 0x000E8799
		protected virtual Vector3 temporaryLookPoint { get; set; }

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x060029B8 RID: 10680 RVA: 0x000EA5A2 File Offset: 0x000E87A2
		// (set) Token: 0x060029B9 RID: 10681 RVA: 0x000EA5AA File Offset: 0x000E87AA
		protected virtual float temporaryLookTime { get; set; }

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x060029BA RID: 10682 RVA: 0x000EA5B3 File Offset: 0x000E87B3
		// (set) Token: 0x060029BB RID: 10683 RVA: 0x000EA5BB File Offset: 0x000E87BB
		protected virtual vHeadTrackSensor sensor { get; set; }

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x060029BC RID: 10684 RVA: 0x000EA5C4 File Offset: 0x000E87C4
		// (set) Token: 0x060029BD RID: 10685 RVA: 0x000EA5CC File Offset: 0x000E87CC
		protected virtual float interaction { get; set; }

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x060029BE RID: 10686 RVA: 0x000EA5D5 File Offset: 0x000E87D5
		// (set) Token: 0x060029BF RID: 10687 RVA: 0x000EA5DD File Offset: 0x000E87DD
		protected virtual vICharacter vChar { get; set; }

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x060029C0 RID: 10688 RVA: 0x000EA5E6 File Offset: 0x000E87E6
		// (set) Token: 0x060029C1 RID: 10689 RVA: 0x000EA5EE File Offset: 0x000E87EE
		protected virtual Transform forwardReference { get; set; }

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x060029C2 RID: 10690 RVA: 0x000EA5F7 File Offset: 0x000E87F7
		public virtual float Smooth
		{
			get
			{
				if (!this.ignoreSmooth)
				{
					return this.smooth * Time.deltaTime;
				}
				return 1f;
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x060029C3 RID: 10691 RVA: 0x000EA613 File Offset: 0x000E8813
		// (set) Token: 0x060029C4 RID: 10692 RVA: 0x000EA61B File Offset: 0x000E881B
		protected virtual Vector3 _currentLocalLookPosition { get; set; }

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x060029C5 RID: 10693 RVA: 0x000EA624 File Offset: 0x000E8824
		// (set) Token: 0x060029C6 RID: 10694 RVA: 0x000EA62C File Offset: 0x000E882C
		protected virtual Vector3 _lastLocalLookPosition { get; set; }

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x060029C7 RID: 10695 RVA: 0x000EA635 File Offset: 0x000E8835
		// (set) Token: 0x060029C8 RID: 10696 RVA: 0x000EA642 File Offset: 0x000E8842
		public virtual float currentVerticalLookAngle
		{
			get
			{
				return this._currentLookAngle.x;
			}
			protected set
			{
				this._currentLookAngle.x = value;
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x060029C9 RID: 10697 RVA: 0x000EA650 File Offset: 0x000E8850
		// (set) Token: 0x060029CA RID: 10698 RVA: 0x000EA65D File Offset: 0x000E885D
		public virtual float currentHorizontalLookAngle
		{
			get
			{
				return this._currentLookAngle.y;
			}
			protected set
			{
				this._currentLookAngle.y = value;
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x060029CB RID: 10699 RVA: 0x000EA66B File Offset: 0x000E886B
		// (set) Token: 0x060029CC RID: 10700 RVA: 0x000EA678 File Offset: 0x000E8878
		public virtual float desiredVerticalLookAngle
		{
			get
			{
				return this._desiredlookAngle.x;
			}
			protected set
			{
				this._desiredlookAngle.x = value;
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x060029CD RID: 10701 RVA: 0x000EA686 File Offset: 0x000E8886
		// (set) Token: 0x060029CE RID: 10702 RVA: 0x000EA693 File Offset: 0x000E8893
		public virtual float desiredHorizontalLookAngle
		{
			get
			{
				return this._desiredlookAngle.y;
			}
			protected set
			{
				this._desiredlookAngle.y = value;
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x060029CF RID: 10703 RVA: 0x000EA6A1 File Offset: 0x000E88A1
		// (set) Token: 0x060029D0 RID: 10704 RVA: 0x000EA6AE File Offset: 0x000E88AE
		public virtual float relativeVerticalLookAngle
		{
			get
			{
				return this._relativeLookAngle.x;
			}
			protected set
			{
				this._relativeLookAngle.x = value;
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x060029D1 RID: 10705 RVA: 0x000EA6BC File Offset: 0x000E88BC
		// (set) Token: 0x060029D2 RID: 10706 RVA: 0x000EA6C9 File Offset: 0x000E88C9
		public virtual float relativeHorizontalLookAngle
		{
			get
			{
				return this._relativeLookAngle.y;
			}
			protected set
			{
				this._relativeLookAngle.y = value;
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x060029D3 RID: 10707 RVA: 0x000EA6D7 File Offset: 0x000E88D7
		// (set) Token: 0x060029D4 RID: 10708 RVA: 0x000EA6DF File Offset: 0x000E88DF
		public virtual bool freezeLookPoint
		{
			get
			{
				return this._freezeLookPoint;
			}
			set
			{
				this._freezeLookPoint = value;
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x060029D5 RID: 10709 RVA: 0x000EA6E8 File Offset: 0x000E88E8
		// (set) Token: 0x060029D6 RID: 10710 RVA: 0x000EA715 File Offset: 0x000E8915
		public virtual Vector3 currentLookPosition
		{
			get
			{
				if (!this.freezeLookPoint)
				{
					return base.transform.TransformPoint(this._currentLocalLookPosition);
				}
				return base.transform.TransformPoint(this._lastLocalLookPosition);
			}
			protected set
			{
				this._currentLocalLookPosition = base.transform.InverseTransformPoint(value);
				if (!this.freezeLookPoint)
				{
					this._lastLocalLookPosition = this._currentLocalLookPosition;
				}
			}
		}

		// Token: 0x060029D7 RID: 10711 RVA: 0x000EA740 File Offset: 0x000E8940
		protected virtual void Start()
		{
			if (!this.sensor)
			{
				GameObject gameObject = new GameObject("HeadTrackSensor");
				this.sensor = gameObject.AddComponent<vHeadTrackSensor>();
			}
			vThirdPersonInput component = base.GetComponent<vThirdPersonInput>();
			if (component)
			{
				component.onLateUpdate -= this.UpdateHeadTrack;
				component.onLateUpdate += this.UpdateHeadTrack;
			}
			this.vChar = base.GetComponent<vICharacter>();
			this.sensor.headTrack = this;
			this.cameraMain = Camera.main;
			int num = LayerMask.NameToLayer("HeadTrack");
			this.sensor.transform.parent = base.transform;
			this.sensor.gameObject.layer = num;
			this.sensor.gameObject.tag = base.transform.tag;
			this.animatorStateInfo = base.GetComponent<vIAnimatorStateInfoController>();
			this.Init();
		}

		// Token: 0x060029D8 RID: 10712 RVA: 0x000EA828 File Offset: 0x000E8A28
		public virtual void Init()
		{
			this.currentLookPosition = this.GetLookPoint();
			this._lastLocalLookPosition = this._currentLocalLookPosition;
			if (this.animator == null)
			{
				this.animator = base.GetComponentInChildren<Animator>();
			}
			AnimatorControllerParameter animatorControllerParameter;
			if (this.animator.GetValidParameter(this.currentAngleH, out animatorControllerParameter))
			{
				this.currentLookAngleH_Hash = animatorControllerParameter.nameHash;
			}
			if (this.animator.GetValidParameter(this.currentAngleV, out animatorControllerParameter))
			{
				this.currentLookAngleV_Hash = animatorControllerParameter.nameHash;
			}
			if (this.animator.GetValidParameter(this.desiredLookAngleH, out animatorControllerParameter))
			{
				this.desiredLookAngleH_Hash = animatorControllerParameter.nameHash;
			}
			if (this.animator.GetValidParameter(this.desiredLookAngleV, out animatorControllerParameter))
			{
				this.desiredLookAngleV_Hash = animatorControllerParameter.nameHash;
			}
			if (this.animator.GetValidParameter(this.relativeLookAngleH, out animatorControllerParameter))
			{
				this.relativeLookAngleH_Hash = animatorControllerParameter.nameHash;
			}
			if (this.animator.GetValidParameter(this.relativeLookAngleV, out animatorControllerParameter))
			{
				this.relativeLookAngleV_Hash = animatorControllerParameter.nameHash;
			}
			if (this.autoFindBones)
			{
				this.spine.Clear();
				this.head = this.animator.GetBoneTransform(HumanBodyBones.Head);
				if (this.head)
				{
					if (!this.forwardReference)
					{
						this.forwardReference = new GameObject("FWRF").transform;
					}
					this.forwardReference.SetParent(this.head);
					this.forwardReference.transform.localPosition = Vector3.zero;
					this.forwardReference.transform.rotation = base.transform.rotation;
					Transform boneTransform = this.animator.GetBoneTransform(HumanBodyBones.Hips);
					if (boneTransform)
					{
						Transform parent = this.head;
						int num = 0;
						while (num < 4 && parent.parent && parent.parent.gameObject != boneTransform.gameObject)
						{
							this.spine.Add(parent.parent);
							parent = parent.parent;
							num++;
						}
					}
				}
			}
			if (this.head)
			{
				this.headHeight = Vector3.Distance(base.transform.position, this.head.position);
				this.sensor.transform.position = this.head.transform.position;
			}
			else
			{
				this.headHeight = 1f;
				this.sensor.transform.position = base.transform.position;
			}
			if (this.spine.Count == 0)
			{
				Debug.Log("Headtrack Spines missing");
			}
			this.spine.Reverse();
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x060029D9 RID: 10713 RVA: 0x000EAAC6 File Offset: 0x000E8CC6
		protected virtual Vector3 headPoint
		{
			get
			{
				return base.transform.position + base.transform.up * this.headHeight;
			}
		}

		// Token: 0x060029DA RID: 10714 RVA: 0x000EAAF0 File Offset: 0x000E8CF0
		public virtual void UpdateHeadTrack()
		{
			if (this.animator == null || !this.animator.enabled)
			{
				return;
			}
			if (this.vChar != null && this.vChar.currentHealth > 0f && this.animator != null && !this.vChar.ragdolled)
			{
				this.onInitUpdate.Invoke();
				if (!this.freezeLookPoint)
				{
					this.currentLookPosition = this.GetLookPoint();
				}
				this.SetLookAtPosition(this.currentLookPosition, this._currentHeadWeight, this._currentBodyWeight);
				this.UpdateAngles();
				this.onFinishUpdate.Invoke();
			}
		}

		// Token: 0x060029DB RID: 10715 RVA: 0x000EAB98 File Offset: 0x000E8D98
		public virtual void SetLookAtPosition(Vector3 point, float headWeight, float spineWeight)
		{
			Quaternion quaternion = Quaternion.LookRotation(point - this.headPoint);
			this.currentLookRotation = quaternion;
			Vector3 vector = quaternion.eulerAngles - base.transform.rotation.eulerAngles;
			float num = this.NormalizeAngle(vector.y);
			float num2 = this.NormalizeAngle(vector.x);
			Vector3 vector2 = (this.considerHeadAnimationForward ? (this.forwardReference.eulerAngles - base.transform.eulerAngles) : Vector3.zero);
			this.currentVerticalLookAngle = Mathf.Clamp(Mathf.Lerp(this.currentVerticalLookAngle, num2 - vector2.NormalizeAngle().x + Quaternion.Euler(this.offsetSpine + this.defaultOffsetSpine).eulerAngles.NormalizeAngle().x, this.Smooth), this.verticalAngleLimit.x, this.verticalAngleLimit.y);
			this.currentHorizontalLookAngle = Mathf.Clamp(Mathf.Lerp(this.currentHorizontalLookAngle, num - vector2.NormalizeAngle().y + Quaternion.Euler(this.offsetSpine + this.defaultOffsetSpine).eulerAngles.NormalizeAngle().y, this.Smooth), this.horizontalAngleLimit.x, this.horizontalAngleLimit.y);
			float num3 = this.NormalizeAngle(this.currentVerticalLookAngle);
			float num4 = this.NormalizeAngle(this.currentHorizontalLookAngle);
			foreach (Transform transform in this.spine)
			{
				Quaternion quaternion2 = Quaternion.AngleAxis(num4 * spineWeight / (float)this.spine.Count, transform.InverseTransformDirection(base.transform.up));
				transform.rotation *= quaternion2;
				Quaternion quaternion3 = Quaternion.AngleAxis(num3 * spineWeight / (float)this.spine.Count, transform.InverseTransformDirection(base.transform.TransformDirection(this.upDownAxis)));
				transform.rotation *= quaternion3;
			}
			if (this.head)
			{
				float num5 = this.NormalizeAngle(this.currentVerticalLookAngle - num3 * spineWeight + Quaternion.Euler(this.offsetHead + this.defaultOffsetHead).eulerAngles.NormalizeAngle().x);
				Quaternion quaternion4 = Quaternion.AngleAxis(this.NormalizeAngle(this.currentHorizontalLookAngle - num4 * spineWeight + Quaternion.Euler(this.offsetHead + this.defaultOffsetHead).eulerAngles.NormalizeAngle().y) * headWeight, this.head.InverseTransformDirection(base.transform.up));
				this.head.rotation *= quaternion4;
				Quaternion quaternion5 = Quaternion.AngleAxis(num5 * headWeight, this.head.InverseTransformDirection(base.transform.TransformDirection(this.upDownAxis)));
				this.head.rotation *= quaternion5;
			}
		}

		// Token: 0x060029DC RID: 10716 RVA: 0x000EAEEC File Offset: 0x000E90EC
		public virtual void UpdateAngles()
		{
			if (this.useCurrentAngle)
			{
				if (this.currentLookAngleH_Hash != -1)
				{
					this.animator.SetFloat(this.currentLookAngleH_Hash, this.currentHorizontalLookAngle);
				}
				if (this.currentLookAngleV_Hash != -1)
				{
					this.animator.SetFloat(this.currentLookAngleV_Hash, this.currentVerticalLookAngle);
				}
			}
			if (this.useDesiredLookAngle)
			{
				if (this.desiredLookAngleH_Hash != -1)
				{
					this.animator.SetFloat(this.desiredLookAngleH_Hash, this.desiredHorizontalLookAngle);
				}
				if (this.desiredLookAngleV_Hash != -1)
				{
					this.animator.SetFloat(this.desiredLookAngleV_Hash, this.desiredVerticalLookAngle);
				}
			}
			if (this.useRelativeLookAngle)
			{
				if (this.relativeLookAngleH_Hash != -1)
				{
					this.animator.SetFloat(this.relativeLookAngleH_Hash, this.relativeHorizontalLookAngle);
				}
				if (this.relativeLookAngleV_Hash != -1)
				{
					this.animator.SetFloat(this.relativeLookAngleV_Hash, this.relativeVerticalLookAngle);
				}
			}
		}

		// Token: 0x060029DD RID: 10717 RVA: 0x000EAFD4 File Offset: 0x000E91D4
		public virtual void RotateSpine(float angleX, float angleY)
		{
			float num = this.NormalizeAngle(angleX);
			float num2 = this.NormalizeAngle(angleY);
			foreach (Transform transform in this.spine)
			{
				Quaternion quaternion = Quaternion.AngleAxis(num2 / (float)this.spine.Count, transform.InverseTransformDirection(base.transform.up));
				transform.rotation *= quaternion;
				Quaternion quaternion2 = Quaternion.AngleAxis(num / (float)this.spine.Count, transform.InverseTransformDirection(base.transform.TransformDirection(this.upDownAxis)));
				transform.rotation *= quaternion2;
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x060029DE RID: 10718 RVA: 0x000EB0B0 File Offset: 0x000E92B0
		// (set) Token: 0x060029DF RID: 10719 RVA: 0x000EB0B8 File Offset: 0x000E92B8
		public virtual Vector3 desiredLookDirection { get; protected set; }

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x060029E0 RID: 10720 RVA: 0x000EB0C1 File Offset: 0x000E92C1
		// (set) Token: 0x060029E1 RID: 10721 RVA: 0x000EB0C9 File Offset: 0x000E92C9
		public virtual Vector3 LookDirection { get; protected set; }

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x060029E2 RID: 10722 RVA: 0x000EB0D4 File Offset: 0x000E92D4
		protected virtual bool lookConditions
		{
			get
			{
				if (!this.cameraMain)
				{
					this.cameraMain = Camera.main;
				}
				return (this.head != null && this.followCamera && this.cameraMain != null) || (!this.followCamera && (this.currentLookTarget || this.simpleTarget)) || this.temporaryLookTime > 0f;
			}
		}

		// Token: 0x060029E3 RID: 10723 RVA: 0x000EB150 File Offset: 0x000E9350
		protected virtual Vector3 GetLookPoint()
		{
			if (this.animator == null)
			{
				return Vector3.zero;
			}
			int num = 100;
			if (this.lookConditions && !this.IgnoreHeadTrack())
			{
				this.desiredLookDirection = base.transform.forward;
				if (this.temporaryLookTime <= 0f)
				{
					Vector3 vector = this.headPoint + base.transform.forward * (float)num;
					if (this.followCamera)
					{
						vector = this.cameraMain.transform.position + this.cameraMain.transform.forward * (float)num;
					}
					this.desiredLookDirection = vector - this.headPoint;
					if ((this.followCamera && !this.alwaysFollowCamera) || !this.followCamera)
					{
						if (this.simpleTarget != null)
						{
							this.desiredLookDirection = this.simpleTarget.position - this.headPoint;
							if (this.currentLookTarget && this.currentLookTarget == this.lastLookTarget)
							{
								this.currentLookTarget.ExitLook(this);
								this.lastLookTarget = null;
							}
						}
						else if (this.currentLookTarget != null && (this.currentLookTarget.ignoreHeadTrackAngle || this.TargetIsOnRange(this.currentLookTarget.lookPoint - this.headPoint)) && this.currentLookTarget.IsVisible(this.headPoint, this.obstacleLayer, false))
						{
							this.desiredLookDirection = this.currentLookTarget.lookPoint - this.headPoint;
							if (this.currentLookTarget != this.lastLookTarget)
							{
								this.currentLookTarget.EnterLook(this);
								this.lastLookTarget = this.currentLookTarget;
							}
						}
						else if (this.currentLookTarget && this.currentLookTarget == this.lastLookTarget)
						{
							this.currentLookTarget.ExitLook(this);
							this.lastLookTarget = null;
						}
					}
				}
				else
				{
					this.desiredLookDirection = this.temporaryLookPoint - this.headPoint;
					this.temporaryLookTime -= Time.deltaTime;
					if (this.currentLookTarget && this.currentLookTarget == this.lastLookTarget)
					{
						this.currentLookTarget.ExitLook(this);
						this.lastLookTarget = null;
					}
				}
				Vector2 targetAngle = this.GetTargetAngle(this.desiredLookDirection);
				if (this.cancelTrackOutOfAngle && (this.lastLookTarget == null || !this.lastLookTarget.ignoreHeadTrackAngle))
				{
					if (this.TargetIsOnRange(this.desiredLookDirection))
					{
						if (this.animator.GetBool("IsStrafing") && !this.IsAnimatorTag("Upperbody Pose"))
						{
							this.SmoothValues(this.strafeHeadWeight, this.strafeBodyWeight, targetAngle.x, targetAngle.y);
						}
						else if (this.animator.GetBool("IsStrafing") && this.IsAnimatorTag("Upperbody Pose"))
						{
							this.SmoothValues(this.aimingHeadWeight, this.aimingBodyWeight, targetAngle.x, targetAngle.y);
						}
						else
						{
							this.SmoothValues(this.freeHeadWeight, this.freeBodyWeight, targetAngle.x, targetAngle.y);
						}
					}
					else
					{
						this.SmoothValues(0f, 0f, 0f, 0f);
					}
				}
				else if (this.animator.GetBool("IsStrafing") && !this.IsAnimatorTag("Upperbody Pose"))
				{
					this.SmoothValues(this.strafeHeadWeight, this.strafeBodyWeight, targetAngle.x, targetAngle.y);
				}
				else if (this.animator.GetBool("IsStrafing") && this.IsAnimatorTag("Upperbody Pose"))
				{
					this.SmoothValues(this.aimingHeadWeight, this.aimingBodyWeight, targetAngle.x, targetAngle.y);
				}
				else
				{
					this.SmoothValues(this.freeHeadWeight, this.freeBodyWeight, targetAngle.x, targetAngle.y);
				}
				if (this.targetsInArea.Count > 1)
				{
					this.SortTargets();
				}
			}
			else
			{
				this.SmoothValues(0f, 0f, 0f, 0f);
				if (this.targetsInArea.Count > 1)
				{
					this.SortTargets();
				}
			}
			Quaternion quaternion = Quaternion.AngleAxis(this.yRotation, base.transform.up);
			Quaternion quaternion2 = Quaternion.AngleAxis(this.xRotation, base.transform.right);
			Vector3 vector2 = quaternion * quaternion2 * base.transform.forward;
			this.LookDirection = vector2;
			this._desiredlookAngle = this.GetTargetAngle(this.cameraMain.transform.forward);
			this._relativeLookAngle = -(this.GetTargetAngle(vector2) - this._desiredlookAngle);
			return this.headPoint + vector2 * (float)num;
		}

		// Token: 0x060029E4 RID: 10724 RVA: 0x000EB654 File Offset: 0x000E9854
		protected virtual Vector2 GetTargetAngle(Vector3 direction)
		{
			if (direction.magnitude == 0f)
			{
				return Vector2.zero;
			}
			Vector3 vector = Quaternion.LookRotation(direction, Vector3.up).eulerAngles - base.transform.eulerAngles;
			return new Vector2(vector.NormalizeAngle().x, vector.NormalizeAngle().y);
		}

		// Token: 0x060029E5 RID: 10725 RVA: 0x000EB6B4 File Offset: 0x000E98B4
		protected virtual bool TargetIsOnRange(Vector3 direction)
		{
			Vector2 targetAngle = this.GetTargetAngle(direction);
			return targetAngle.x >= this.verticalAngleLimit.x && targetAngle.x <= this.verticalAngleLimit.y && targetAngle.y >= this.horizontalAngleLimit.x && targetAngle.y <= this.horizontalAngleLimit.y;
		}

		// Token: 0x060029E6 RID: 10726 RVA: 0x000EB71A File Offset: 0x000E991A
		public virtual void SetAlwaysFollowCamera(bool value)
		{
			this.alwaysFollowCamera = value;
		}

		// Token: 0x060029E7 RID: 10727 RVA: 0x000EB723 File Offset: 0x000E9923
		public virtual void SetLookTarget(vLookTarget target, bool priority = false)
		{
			if (!this.targetsInArea.Contains(target))
			{
				this.targetsInArea.Add(target);
			}
			if (priority)
			{
				this.currentLookTarget = target;
			}
		}

		// Token: 0x060029E8 RID: 10728 RVA: 0x000EB749 File Offset: 0x000E9949
		public virtual void SetLookTarget(Transform target)
		{
			this.simpleTarget = target;
		}

		// Token: 0x060029E9 RID: 10729 RVA: 0x000EB752 File Offset: 0x000E9952
		public virtual void SetTemporaryLookPoint(Vector3 point, float time = 1f)
		{
			this.temporaryLookPoint = point;
			this.temporaryLookTime = time;
		}

		// Token: 0x060029EA RID: 10730 RVA: 0x000EB762 File Offset: 0x000E9962
		public virtual void RemoveLookTarget(vLookTarget target)
		{
			if (this.targetsInArea.Contains(target))
			{
				this.targetsInArea.Remove(target);
			}
			if (this.currentLookTarget == target)
			{
				this.currentLookTarget = null;
			}
		}

		// Token: 0x060029EB RID: 10731 RVA: 0x000EB794 File Offset: 0x000E9994
		public virtual void RemoveLookTarget(Transform target)
		{
			if (this.simpleTarget == target)
			{
				this.simpleTarget = null;
			}
		}

		// Token: 0x060029EC RID: 10732 RVA: 0x000EB7AB File Offset: 0x000E99AB
		protected virtual float NormalizeAngle(float angle)
		{
			if (angle > 180f)
			{
				angle -= 360f;
			}
			else if (angle < -180f)
			{
				angle += 360f;
			}
			return angle;
		}

		// Token: 0x060029ED RID: 10733 RVA: 0x000EB7D2 File Offset: 0x000E99D2
		protected virtual void ResetValues()
		{
			this._currentHeadWeight = 0f;
			this._currentBodyWeight = 0f;
			this.yRotation = 0f;
			this.xRotation = 0f;
		}

		// Token: 0x060029EE RID: 10734 RVA: 0x000EB800 File Offset: 0x000E9A00
		protected virtual void SmoothValues(float _headWeight = 0f, float _bodyWeight = 0f, float _x = 0f, float _y = 0f)
		{
			this._currentHeadWeight = Mathf.Lerp(this._currentHeadWeight, _headWeight, this.Smooth);
			this._currentBodyWeight = Mathf.Lerp(this._currentBodyWeight, _bodyWeight, this.Smooth);
			this.yRotation = Mathf.Lerp(this.yRotation, _y, this.Smooth);
			this.xRotation = Mathf.Lerp(this.xRotation, _x, this.Smooth);
			this.yRotation = Mathf.Clamp(this.yRotation, this.horizontalAngleLimit.x, this.horizontalAngleLimit.y);
			this.xRotation = Mathf.Clamp(this.xRotation, this.verticalAngleLimit.x, this.verticalAngleLimit.y);
		}

		// Token: 0x060029EF RID: 10735 RVA: 0x000EB8BC File Offset: 0x000E9ABC
		protected virtual void SortTargets()
		{
			this.interaction += Time.deltaTime;
			if (this.interaction > this.updateTargetInteraction)
			{
				this.interaction -= this.updateTargetInteraction;
				if (this.targetsInArea == null || this.targetsInArea.Count < 2)
				{
					if (this.targetsInArea != null && this.targetsInArea.Count > 0)
					{
						this.currentLookTarget = this.targetsInArea[0];
					}
					return;
				}
				for (int i = this.targetsInArea.Count - 1; i >= 0; i--)
				{
					if (this.targetsInArea[i] == null)
					{
						this.targetsInArea.RemoveAt(i);
					}
				}
				this.targetsInArea.Sort((vLookTarget c1, vLookTarget c2) => Vector3.Distance(base.transform.position, (c1 != null) ? c1.transform.position : (Vector3.one * float.PositiveInfinity)).CompareTo(Vector3.Distance(base.transform.position, (c2 != null) ? c2.transform.position : (Vector3.one * float.PositiveInfinity))));
				if (this.targetsInArea.Count > 0)
				{
					this.currentLookTarget = this.targetsInArea[0];
				}
			}
		}

		// Token: 0x060029F0 RID: 10736 RVA: 0x000EB9B0 File Offset: 0x000E9BB0
		public virtual void OnDetect(Collider other)
		{
			if (this.tagsToDetect.Contains(other.gameObject.tag) && other.GetComponent<vLookTarget>() != null)
			{
				this.currentLookTarget = other.GetComponent<vLookTarget>();
				vHeadTrack componentInParent = other.GetComponentInParent<vHeadTrack>();
				if (!this.targetsInArea.Contains(this.currentLookTarget) && (componentInParent == null || componentInParent != this))
				{
					this.targetsInArea.Add(this.currentLookTarget);
					this.SortTargets();
					this.currentLookTarget = this.targetsInArea[0];
				}
			}
		}

		// Token: 0x060029F1 RID: 10737 RVA: 0x000EBA44 File Offset: 0x000E9C44
		public virtual void OnLost(Collider other)
		{
			if (this.tagsToDetect.Contains(other.gameObject.tag) && other.GetComponentInParent<vLookTarget>() != null)
			{
				vLookTarget componentInParent = other.GetComponentInParent<vLookTarget>();
				if (this.targetsInArea.Contains(componentInParent))
				{
					this.targetsInArea.Remove(componentInParent);
					if (componentInParent == this.lastLookTarget)
					{
						componentInParent.ExitLook(this);
					}
				}
				this.SortTargets();
				if (this.targetsInArea.Count > 0)
				{
					this.currentLookTarget = this.targetsInArea[0];
					return;
				}
				this.currentLookTarget = null;
			}
		}

		// Token: 0x060029F2 RID: 10738 RVA: 0x000EBADC File Offset: 0x000E9CDC
		public virtual bool IgnoreHeadTrack()
		{
			return this.animatorIgnoreTags.Exists((string tag) => this.IsAnimatorTag(tag));
		}

		// Token: 0x060029F3 RID: 10739 RVA: 0x000EBAFA File Offset: 0x000E9CFA
		public virtual bool IsAnimatorTag(string tag)
		{
			return !(this.animator == null) && (this.animatorStateInfo.isValid() && this.animatorStateInfo.animatorStateInfos.HasTag(tag));
		}

		// Token: 0x040029C0 RID: 10688
		[vEditorToolbar("Settings", false, "", false, false)]
		[vHelpBox("If your character is not looking up/down, try changing the axis", vHelpBoxAttribute.MessageType.Info)]
		public Vector3 upDownAxis = Vector3.right;

		// Token: 0x040029C1 RID: 10689
		[Header("Head & Body Weight")]
		public float strafeHeadWeight = 0.6f;

		// Token: 0x040029C2 RID: 10690
		public float strafeBodyWeight = 0.6f;

		// Token: 0x040029C3 RID: 10691
		public float aimingHeadWeight = 0.8f;

		// Token: 0x040029C4 RID: 10692
		public float aimingBodyWeight = 0.8f;

		// Token: 0x040029C5 RID: 10693
		public float freeHeadWeight = 0.6f;

		// Token: 0x040029C6 RID: 10694
		public float freeBodyWeight = 0.6f;

		// Token: 0x040029C7 RID: 10695
		[SerializeField]
		protected float smooth = 10f;

		// Token: 0x040029C8 RID: 10696
		[Header("Default Offsets ")]
		[SerializeField]
		protected Vector2 defaultOffsetSpine;

		// Token: 0x040029C9 RID: 10697
		[SerializeField]
		protected Vector2 defaultOffsetHead;

		// Token: 0x040029CA RID: 10698
		[vReadOnly(true)]
		public Vector2 offsetSpine;

		// Token: 0x040029CB RID: 10699
		[vReadOnly(true)]
		public Vector2 offsetHead;

		// Token: 0x040029CC RID: 10700
		[Header("Tracking")]
		[Tooltip("Follow the Camera Forward")]
		public bool followCamera = true;

		// Token: 0x040029CD RID: 10701
		public bool _freezeLookPoint;

		// Token: 0x040029CE RID: 10702
		[vHideInInspector("followCamera", false)]
		[Tooltip("Force to follow camera")]
		[SerializeField]
		protected bool _alwaysFollowCamera;

		// Token: 0x040029CF RID: 10703
		[Tooltip("Ignore the Limits and continue to follow the camera")]
		public bool cancelTrackOutOfAngle = true;

		// Token: 0x040029D0 RID: 10704
		[Tooltip("Considerer the head animation forward while tracking, try it to see different results")]
		public bool considerHeadAnimationForward;

		// Token: 0x040029D1 RID: 10705
		[Header("Limits")]
		[vMinMax(minLimit = -180f, maxLimit = 180f)]
		public Vector2 horizontalAngleLimit = new Vector2(-100f, 100f);

		// Token: 0x040029D2 RID: 10706
		[vMinMax(minLimit = -90f, maxLimit = 90f)]
		public Vector2 verticalAngleLimit = new Vector2(-80f, 80f);

		// Token: 0x040029D3 RID: 10707
		[vHelpBox("Animations with vAnimatorTag Behavior will ignore the HeadTrack while is being played", vHelpBoxAttribute.MessageType.None)]
		[Header("Ignore AnimatorTags")]
		public List<string> animatorIgnoreTags = new List<string> { "Attack", "LockMovement", "CustomAction", "IsEquipping", "IgnoreHeadtrack" };

		// Token: 0x040029D4 RID: 10708
		[vEditorToolbar("Bones", false, "", false, false)]
		[vHelpBox("Auto Find Bones using Humanoid", vHelpBoxAttribute.MessageType.None)]
		public bool autoFindBones = true;

		// Token: 0x040029D5 RID: 10709
		public Transform head;

		// Token: 0x040029D6 RID: 10710
		public List<Transform> spine = new List<Transform>();

		// Token: 0x040029D7 RID: 10711
		[vEditorToolbar("Detection", false, "", false, false)]
		public float updateTargetInteraction = 1f;

		// Token: 0x040029D8 RID: 10712
		public float distanceToDetect = 10f;

		// Token: 0x040029D9 RID: 10713
		public LayerMask obstacleLayer = 1;

		// Token: 0x040029DA RID: 10714
		[vHelpBox("Gameobjects Tags to detect", vHelpBoxAttribute.MessageType.None)]
		public List<string> tagsToDetect = new List<string> { "LookAt" };

		// Token: 0x040029DB RID: 10715
		[vEditorToolbar("HeadTrack Angles", false, "", false, false)]
		[vHelpBox("Angle between character forward and camera forward", vHelpBoxAttribute.MessageType.None)]
		[SerializeField]
		[vReadOnly(false)]
		protected Vector2 _desiredlookAngle;

		// Token: 0x040029DC RID: 10716
		[vHelpBox("Angle between head forward and character forward", vHelpBoxAttribute.MessageType.None)]
		[SerializeField]
		[vReadOnly(false)]
		protected Vector2 _currentLookAngle;

		// Token: 0x040029DD RID: 10717
		[vHelpBox("Angle between head forward and camera forward", vHelpBoxAttribute.MessageType.None)]
		[SerializeField]
		[vReadOnly(false)]
		protected Vector2 _relativeLookAngle;

		// Token: 0x040029DE RID: 10718
		[vSeparator("Optinal Animator paramenters", "")]
		[vHelpBox("This is an optional to use and update paramenters in the animator using the look angles of the headtrack", vHelpBoxAttribute.MessageType.None)]
		[Tooltip("When enable this. The headtrack will update the angles relative to the character forward and camera forward")]
		public bool useDesiredLookAngle;

		// Token: 0x040029DF RID: 10719
		[vHideInInspector("useDesiredLookAngle", false)]
		public string desiredLookAngleH = "HorizontalLookAngle";

		// Token: 0x040029E0 RID: 10720
		[vHideInInspector("useDesiredLookAngle", false)]
		public string desiredLookAngleV = "VerticalLookAngle";

		// Token: 0x040029E1 RID: 10721
		[Tooltip("When enable this. The headtrack will update the angles relative to the Head forward and Character forward")]
		public bool useCurrentAngle;

		// Token: 0x040029E2 RID: 10722
		[vHideInInspector("useCurrentAngle", false)]
		public string currentAngleH = "CurrentHorizontalLookAngle";

		// Token: 0x040029E3 RID: 10723
		[vHideInInspector("useCurrentAngle", false)]
		public string currentAngleV = "CurrentVerticalLookAngle";

		// Token: 0x040029E4 RID: 10724
		[Tooltip("When enable this. The headtrack will update the angles relative to the character head forward and camera forward")]
		public bool useRelativeLookAngle;

		// Token: 0x040029E5 RID: 10725
		[vHideInInspector("useRelativeLookAngle", false)]
		public string relativeLookAngleH = "HorizontalLookAngleRelative";

		// Token: 0x040029E6 RID: 10726
		[vHideInInspector("useRelativeLookAngle", false)]
		public string relativeLookAngleV = "VerticalLookAngleRelative";

		// Token: 0x040029E7 RID: 10727
		internal UnityEvent onInitUpdate = new UnityEvent();

		// Token: 0x040029E8 RID: 10728
		internal UnityEvent onFinishUpdate = new UnityEvent();

		// Token: 0x040029E9 RID: 10729
		internal List<vLookTarget> targetsInArea = new List<vLookTarget>();

		// Token: 0x040029FD RID: 10749
		protected int currentLookAngleH_Hash = -1;

		// Token: 0x040029FE RID: 10750
		protected int currentLookAngleV_Hash = -1;

		// Token: 0x040029FF RID: 10751
		protected int desiredLookAngleH_Hash = -1;

		// Token: 0x04002A00 RID: 10752
		protected int desiredLookAngleV_Hash = -1;

		// Token: 0x04002A01 RID: 10753
		protected int relativeLookAngleH_Hash = -1;

		// Token: 0x04002A02 RID: 10754
		protected int relativeLookAngleV_Hash = -1;
	}
}
