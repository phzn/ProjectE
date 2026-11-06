using System;
using System.Collections.Generic;
using Features.DebugTools;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.GachaMachine
{
	// Token: 0x020011DD RID: 4573
	[DisallowMultipleComponent]
	public sealed class GachaClawRig : MonoBehaviour
	{
		// Token: 0x1700167F RID: 5759
		// (get) Token: 0x06008CD6 RID: 36054 RVA: 0x00397F90 File Offset: 0x00396190
		public GachaMachineSide Side
		{
			get
			{
				return this.side;
			}
		}

		// Token: 0x17001680 RID: 5760
		// (get) Token: 0x06008CD7 RID: 36055 RVA: 0x00397F98 File Offset: 0x00396198
		public bool ControlEnabled
		{
			get
			{
				return this._controlEnabled;
			}
		}

		// Token: 0x17001681 RID: 5761
		// (get) Token: 0x06008CD8 RID: 36056 RVA: 0x00397FA0 File Offset: 0x003961A0
		public float CurrentDrop
		{
			get
			{
				return this._drop;
			}
		}

		// Token: 0x17001682 RID: 5762
		// (get) Token: 0x06008CD9 RID: 36057 RVA: 0x00397FA8 File Offset: 0x003961A8
		public float CurrentDrop01
		{
			get
			{
				if (this.maximumDrop <= 0f)
				{
					return 0f;
				}
				return this._drop / this.maximumDrop;
			}
		}

		// Token: 0x17001683 RID: 5763
		// (get) Token: 0x06008CDA RID: 36058 RVA: 0x00397FCA File Offset: 0x003961CA
		public float Closure01
		{
			get
			{
				return this._closure;
			}
		}

		// Token: 0x17001684 RID: 5764
		// (get) Token: 0x06008CDB RID: 36059 RVA: 0x00397FD2 File Offset: 0x003961D2
		public bool IsClosed
		{
			get
			{
				return this._closure >= 0.98f;
			}
		}

		// Token: 0x17001685 RID: 5765
		// (get) Token: 0x06008CDC RID: 36060 RVA: 0x00397FE4 File Offset: 0x003961E4
		public float LeftFingerClosure01
		{
			get
			{
				return this._leftFingerClosure;
			}
		}

		// Token: 0x17001686 RID: 5766
		// (get) Token: 0x06008CDD RID: 36061 RVA: 0x00397FEC File Offset: 0x003961EC
		public float RightFingerClosure01
		{
			get
			{
				return this._rightFingerClosure;
			}
		}

		// Token: 0x17001687 RID: 5767
		// (get) Token: 0x06008CDE RID: 36062 RVA: 0x00397FF4 File Offset: 0x003961F4
		public bool BothFingersBlocked
		{
			get
			{
				return this._leftFingerBlocked && this._rightFingerBlocked;
			}
		}

		// Token: 0x17001688 RID: 5768
		// (get) Token: 0x06008CDF RID: 36063 RVA: 0x00398006 File Offset: 0x00396206
		public Transform ClawMotor
		{
			get
			{
				return this.clawMotor;
			}
		}

		// Token: 0x17001689 RID: 5769
		// (get) Token: 0x06008CE0 RID: 36064 RVA: 0x0039800E File Offset: 0x0039620E
		public Transform LeftFingerBone
		{
			get
			{
				return this.leftFingerBone;
			}
		}

		// Token: 0x1700168A RID: 5770
		// (get) Token: 0x06008CE1 RID: 36065 RVA: 0x00398016 File Offset: 0x00396216
		public Transform RightFingerBone
		{
			get
			{
				return this.rightFingerBone;
			}
		}

		// Token: 0x1700168B RID: 5771
		// (get) Token: 0x06008CE2 RID: 36066 RVA: 0x0039801E File Offset: 0x0039621E
		public GachaClawGrabber Grabber
		{
			get
			{
				return this.grabber;
			}
		}

		// Token: 0x1700168C RID: 5772
		// (get) Token: 0x06008CE3 RID: 36067 RVA: 0x00398028 File Offset: 0x00396228
		public bool IsConfigured
		{
			get
			{
				return this.beam != null && this.trolleyMotor != null && this.upperArmBone != null && this.lowerArmBone != null && this.clawMotor != null;
			}
		}

		// Token: 0x1700168D RID: 5773
		// (get) Token: 0x06008CE4 RID: 36068 RVA: 0x0039807B File Offset: 0x0039627B
		public bool FingersConfigured
		{
			get
			{
				return this.leftFingerBone != null && this.rightFingerBone != null;
			}
		}

		// Token: 0x06008CE5 RID: 36069 RVA: 0x0039809C File Offset: 0x0039629C
		private void Awake()
		{
			ControllerContextBindings.Set(this.grabInput, "A");
			ControllerContextBindings.Set(this.lowerInput, "LT");
			ControllerContextBindings.Set(this.raiseInput, "RT");
			this.ResolveFingerBones();
			this.EnsureGrabber();
			this.CaptureBindPose();
			this.ApplyFingerPose();
			this.UpdateCable();
		}

		// Token: 0x06008CE6 RID: 36070 RVA: 0x003980F7 File Offset: 0x003962F7
		private void OnDisable()
		{
			this._controlEnabled = false;
			if (this.grabber != null)
			{
				this.grabber.SetControlEnabled(false);
				this.grabber.ClearSession();
			}
			if (this.resetRigOnDisable)
			{
				this.ResetToRest();
			}
		}

		// Token: 0x06008CE7 RID: 36071 RVA: 0x00398134 File Offset: 0x00396334
		private void Update()
		{
			if (!this._controlEnabled || !this.IsConfigured)
			{
				return;
			}
			if (FlyCamera.IsActive)
			{
				return;
			}
			float num = (string.IsNullOrEmpty(this.horizontalAxis) ? 0f : (-GameplayInput.GetAxisRaw(this.horizontalAxis)));
			float num2 = (string.IsNullOrEmpty(this.depthAxis) ? 0f : GameplayInput.GetAxisRaw(this.depthAxis));
			if (this.invertHorizontal)
			{
				num = -num;
			}
			if (this.invertDepth)
			{
				num2 = -num2;
			}
			float num3 = 0f;
			if (this.lowerInput != null && this.lowerInput.GetButton())
			{
				num3 += 1f;
			}
			if (this.raiseInput != null && this.raiseInput.GetButton())
			{
				num3 -= 1f;
			}
			this.TickMovement(num, num2, num3, Time.deltaTime);
			if (this.grabInput != null && this.grabInput.GetButtonDown())
			{
				this.SetClosureTarget((this._closureTarget < 0.5f) ? 1f : 0f);
			}
			this.TickClosure(Time.deltaTime);
		}

		// Token: 0x06008CE8 RID: 36072 RVA: 0x0039823E File Offset: 0x0039643E
		private void LateUpdate()
		{
			this.UpdateCable();
		}

		// Token: 0x06008CE9 RID: 36073 RVA: 0x00398248 File Offset: 0x00396448
		public void SetControlEnabled(bool enabled)
		{
			if (this._controlEnabled == enabled)
			{
				return;
			}
			if (enabled && !this.IsConfigured)
			{
				Debug.LogError("[GachaClawRig] '" + base.name + "' is missing one or more rig bones.", this);
				return;
			}
			this.CaptureBindPose();
			if (enabled && !this.BindPoseIsInsideLimits())
			{
				Debug.LogError(string.Format("[GachaClawRig] {0} bind pose falls outside its configured ", this.side) + string.Format("travel limits (beamX={0:0.000}, ", this._beamBindLocalPosition.x) + string.Format("motorZ={0:0.000}).", this._trolleyBindLocalPosition.z), this);
				return;
			}
			bool controlEnabled = this._controlEnabled;
			this._controlEnabled = enabled;
			this.EnsureGrabber();
			if (this.grabber != null)
			{
				this.grabber.SetControlEnabled(enabled);
			}
			if (controlEnabled && !enabled && this.resetRigOnControlDisable)
			{
				this.ResetToRest();
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[GachaClawRig] {0} control {1}.", this.side, enabled ? "enabled" : "disabled"), this);
			}
		}

		// Token: 0x06008CEA RID: 36074 RVA: 0x00398360 File Offset: 0x00396560
		public void TickMovement(float horizontal, float depth, float vertical, float deltaTime)
		{
			if (!this.IsConfigured || deltaTime <= 0f)
			{
				return;
			}
			this.CaptureBindPose();
			Vector3 localPosition = this.beam.localPosition;
			localPosition.x = Mathf.Clamp(localPosition.x + Mathf.Clamp(horizontal, -1f, 1f) * this.horizontalSpeed * deltaTime, this.beamLocalXLimits.x, this.beamLocalXLimits.y);
			this.beam.localPosition = localPosition;
			Vector3 localPosition2 = this.trolleyMotor.localPosition;
			localPosition2.z = Mathf.Clamp(localPosition2.z + Mathf.Clamp(depth, -1f, 1f) * this.depthSpeed * deltaTime, this.trolleyLocalZLimits.x, this.trolleyLocalZLimits.y);
			this.trolleyMotor.localPosition = localPosition2;
			float num = Mathf.Clamp(this._drop + Mathf.Clamp(vertical, -1f, 1f) * this.verticalSpeed * deltaTime, 0f, this.maximumDrop);
			if (num > this._drop && this.stopFingersOnContact)
			{
				num = this.ClampDropAgainstCiel(num);
			}
			this._drop = num;
			this.ApplyArmStaging(this._drop);
			this.UpdateCable();
		}

		// Token: 0x06008CEB RID: 36075 RVA: 0x003984A0 File Offset: 0x003966A0
		private void ApplyArmStaging(float drop)
		{
			float num = Mathf.Min(drop, this.upperArmStageTravel);
			float num2 = Mathf.Max(0f, drop - num);
			Vector3 upperArmBindLocalPosition = this._upperArmBindLocalPosition;
			upperArmBindLocalPosition.y += num;
			this.upperArmBone.localPosition = upperArmBindLocalPosition;
			Vector3 lowerArmBindLocalPosition = this._lowerArmBindLocalPosition;
			lowerArmBindLocalPosition.y += num2;
			this.lowerArmBone.localPosition = lowerArmBindLocalPosition;
		}

		// Token: 0x06008CEC RID: 36076 RVA: 0x00398508 File Offset: 0x00396708
		private float ClampDropAgainstCiel(float requestedDrop)
		{
			if (this.grabber == null || !this.grabber.HasCielSession)
			{
				return requestedDrop;
			}
			if ((this._leftFingerColliders == null || this._leftFingerColliders.Length == 0) && (this._rightFingerColliders == null || this._rightFingerColliders.Length == 0) && this._palmCollider == null)
			{
				return requestedDrop;
			}
			float num = this._drop;
			float num2 = this._drop;
			while (num2 < requestedDrop - 0.0001f)
			{
				num2 = Mathf.Min(num2 + 0.01f, requestedDrop);
				if (this.ClawOverlapsCielAtDrop(num2))
				{
					break;
				}
				num = num2;
			}
			this.ApplyArmStaging(this._drop);
			return num;
		}

		// Token: 0x06008CED RID: 36077 RVA: 0x003985A4 File Offset: 0x003967A4
		private bool ClawOverlapsCielAtDrop(float candidateDrop)
		{
			this.ApplyArmStaging(candidateDrop);
			return this.FingerShapesOverlapCiel(this._leftFingerColliders) || this.FingerShapesOverlapCiel(this._rightFingerColliders) || (this._palmCollider != null && this.BoxOverlapsCiel(this._palmCollider));
		}

		// Token: 0x06008CEE RID: 36078 RVA: 0x003985F4 File Offset: 0x003967F4
		private bool FingerShapesOverlapCiel(BoxCollider[] shapes)
		{
			if (shapes == null)
			{
				return false;
			}
			foreach (BoxCollider boxCollider in shapes)
			{
				if (boxCollider != null && this.BoxOverlapsCiel(boxCollider))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008CEF RID: 36079 RVA: 0x00398630 File Offset: 0x00396830
		private bool BoxOverlapsCiel(BoxCollider box)
		{
			Transform transform = box.transform;
			Vector3 vector = transform.TransformPoint(box.center);
			Vector3 lossyScale = transform.lossyScale;
			Vector3 vector2 = Vector3.Scale(box.size * 0.5f, new Vector3(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.y), Mathf.Abs(lossyScale.z)));
			int num = Physics.OverlapBoxNonAlloc(vector, vector2, this._fingerOverlapBuffer, transform.rotation, -1, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num; i++)
			{
				Collider collider = this._fingerOverlapBuffer[i];
				if (!(collider == null) && !collider.isTrigger && this.grabber != null && this.grabber.IsSessionCielCollider(collider))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008CF0 RID: 36080 RVA: 0x003986F8 File Offset: 0x003968F8
		private void CaptureBindPose(bool force)
		{
			if ((!force && this._bindPoseCaptured) || !this.IsConfigured)
			{
				return;
			}
			this.ResolveFingerBones();
			this.ResolveFingerColliders();
			this._beamBindLocalPosition = this.beam.localPosition;
			this._trolleyBindLocalPosition = this.trolleyMotor.localPosition;
			this._upperArmBindLocalPosition = this.upperArmBone.localPosition;
			this._lowerArmBindLocalPosition = this.lowerArmBone.localPosition;
			if (this.leftFingerBone != null)
			{
				this._leftFingerBindLocalRotation = this.leftFingerBone.localRotation;
			}
			if (this.rightFingerBone != null)
			{
				this._rightFingerBindLocalRotation = this.rightFingerBone.localRotation;
			}
			this._drop = 0f;
			this.ResetClosureState();
			this._bindPoseCaptured = true;
		}

		// Token: 0x06008CF1 RID: 36081 RVA: 0x003987BF File Offset: 0x003969BF
		public void CaptureBindPose()
		{
			this.CaptureBindPose(false);
		}

		// Token: 0x06008CF2 RID: 36082 RVA: 0x003987C8 File Offset: 0x003969C8
		[ContextMenu("Capture Current Rig As New Bind Pose")]
		private void ForceCaptureCurrentAsBindPose()
		{
			this.CaptureBindPose(true);
		}

		// Token: 0x06008CF3 RID: 36083 RVA: 0x003987D4 File Offset: 0x003969D4
		[ContextMenu("Reset Claw To Rest")]
		public void ResetToRest()
		{
			if (!this._bindPoseCaptured || !this.IsConfigured)
			{
				return;
			}
			this.beam.localPosition = this._beamBindLocalPosition;
			this.trolleyMotor.localPosition = this._trolleyBindLocalPosition;
			this.upperArmBone.localPosition = this._upperArmBindLocalPosition;
			this.lowerArmBone.localPosition = this._lowerArmBindLocalPosition;
			this._drop = 0f;
			this.ResetClosureState();
			this.ApplyFingerPose();
			if (this.grabber != null)
			{
				this.grabber.Release();
			}
			this.UpdateCable();
		}

		// Token: 0x06008CF4 RID: 36084 RVA: 0x0039886C File Offset: 0x00396A6C
		private void ResetClosureState()
		{
			this._closure = 0f;
			this._closureTarget = 0f;
			this._leftFingerClosure = 0f;
			this._rightFingerClosure = 0f;
			this._leftFingerBlocked = false;
			this._rightFingerBlocked = false;
		}

		// Token: 0x06008CF5 RID: 36085 RVA: 0x003988A8 File Offset: 0x00396AA8
		public void SetClosureTarget(float closure01)
		{
			this._closureTarget = Mathf.Clamp01(closure01);
			if (this._closureTarget < 0.5f && this.grabber != null)
			{
				this.grabber.Release();
			}
		}

		// Token: 0x06008CF6 RID: 36086 RVA: 0x003988DC File Offset: 0x00396ADC
		public void TickClosure(float deltaTime)
		{
			if (!this.FingersConfigured || deltaTime <= 0f)
			{
				return;
			}
			this._closure = Mathf.MoveTowards(this._closure, this._closureTarget, this.fingerClosureSpeed * deltaTime);
			this.UpdateFingerClosures(this.fingerClosureSpeed * deltaTime);
			this.ApplyFingerPose();
			if (this.grabber != null)
			{
				this.grabber.SetGripBlocked(this.BothFingersBlocked && this._closureTarget >= 0.5f);
				this.grabber.SetClosure01(this._closure);
			}
		}

		// Token: 0x06008CF7 RID: 36087 RVA: 0x00398974 File Offset: 0x00396B74
		private void UpdateFingerClosures(float maximumAdvance)
		{
			this._leftFingerClosure = this.AdvanceFinger(this._leftFingerClosure, this.leftFingerBone, this._leftFingerColliders, true, maximumAdvance, ref this._leftFingerBlocked);
			this._rightFingerClosure = this.AdvanceFinger(this._rightFingerClosure, this.rightFingerBone, this._rightFingerColliders, false, maximumAdvance, ref this._rightFingerBlocked);
		}

		// Token: 0x06008CF8 RID: 36088 RVA: 0x003989D0 File Offset: 0x00396BD0
		private float AdvanceFinger(float current, Transform bone, BoxCollider[] shapes, bool isLeftFinger, float maximumAdvance, ref bool blocked)
		{
			float num = Mathf.Min(this._closure, current + Mathf.Max(0f, maximumAdvance));
			if (num <= current + 0.0001f)
			{
				blocked = false;
				return Mathf.Min(current, num);
			}
			if (!this.stopFingersOnContact || bone == null || shapes == null || shapes.Length == 0)
			{
				blocked = false;
				return num;
			}
			float num2 = ((this.fingerClosedAngle > 0.01f) ? (this.fingerContactStepDegrees / this.fingerClosedAngle) : 1f);
			if (blocked)
			{
				if (this.FingerWouldOverlap(bone, shapes, Mathf.Min(current + num2 * 0.5f, num), isLeftFinger))
				{
					return current;
				}
				blocked = false;
			}
			while (current < num - 0.0001f)
			{
				float num3 = Mathf.Min(current + num2, num);
				if (this.FingerWouldOverlap(bone, shapes, num3, isLeftFinger))
				{
					blocked = true;
					break;
				}
				current = num3;
			}
			return current;
		}

		// Token: 0x06008CF9 RID: 36089 RVA: 0x00398AA0 File Offset: 0x00396CA0
		private bool FingerWouldOverlap(Transform bone, BoxCollider[] shapes, float closure01, bool isLeftFinger)
		{
			Quaternion localRotation = bone.localRotation;
			this.ApplySingleFingerPose(bone, closure01, isLeftFinger);
			bool flag = false;
			int num = 0;
			while (num < shapes.Length && !flag)
			{
				BoxCollider boxCollider = shapes[num];
				if (!(boxCollider == null))
				{
					Transform transform = boxCollider.transform;
					Vector3 vector = transform.TransformPoint(boxCollider.center);
					Vector3 lossyScale = transform.lossyScale;
					Vector3 vector2 = Vector3.Scale(boxCollider.size * 0.5f, new Vector3(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.y), Mathf.Abs(lossyScale.z)));
					int num2 = Physics.OverlapBoxNonAlloc(vector, vector2, this._fingerOverlapBuffer, transform.rotation, -1, QueryTriggerInteraction.Ignore);
					for (int i = 0; i < num2; i++)
					{
						Collider collider = this._fingerOverlapBuffer[i];
						if (!(collider == null) && !collider.isTrigger && (!(this.clawMotor != null) || !collider.transform.IsChildOf(this.clawMotor)))
						{
							flag = true;
							break;
						}
					}
				}
				num++;
			}
			bone.localRotation = localRotation;
			return flag;
		}

		// Token: 0x06008CFA RID: 36090 RVA: 0x00398BBA File Offset: 0x00396DBA
		public void ConfigureGrabSession(GachaMachineController controller, Transform playerRoot, Animator playerAnimator)
		{
			this.EnsureGrabber();
			if (this.grabber != null)
			{
				this.grabber.ConfigureSession(controller, playerRoot, playerAnimator);
			}
		}

		// Token: 0x06008CFB RID: 36091 RVA: 0x00398BDE File Offset: 0x00396DDE
		public void ClearGrabSession()
		{
			if (this.grabber != null)
			{
				this.grabber.ClearSession();
			}
		}

		// Token: 0x06008CFC RID: 36092 RVA: 0x00398BF9 File Offset: 0x00396DF9
		private void ApplyFingerPose()
		{
			if (!this.FingersConfigured || !this._bindPoseCaptured)
			{
				return;
			}
			this.ApplySingleFingerPose(this.leftFingerBone, this._leftFingerClosure, true);
			this.ApplySingleFingerPose(this.rightFingerBone, this._rightFingerClosure, false);
		}

		// Token: 0x06008CFD RID: 36093 RVA: 0x00398C34 File Offset: 0x00396E34
		private void ApplySingleFingerPose(Transform bone, float closure01, bool isLeftFinger)
		{
			if (bone == null || !this._bindPoseCaptured)
			{
				return;
			}
			Vector3 vector = ((this.fingerHingeAxis.sqrMagnitude > 0.0001f) ? this.fingerHingeAxis.normalized : Vector3.forward);
			float num = this.fingerClosedAngle * Mathf.Clamp01(closure01);
			float num2 = (this.invertFingerRotation ? (-1f) : 1f);
			if (isLeftFinger)
			{
				bone.localRotation = Quaternion.AngleAxis(-num * num2, vector) * this._leftFingerBindLocalRotation;
				return;
			}
			bone.localRotation = Quaternion.AngleAxis(num * num2, vector) * this._rightFingerBindLocalRotation;
		}

		// Token: 0x06008CFE RID: 36094 RVA: 0x00398CD4 File Offset: 0x00396ED4
		private void ResolveFingerColliders()
		{
			if (this.leftFingerBone != null && (this._leftFingerColliders == null || this._leftFingerColliders.Length == 0))
			{
				this._leftFingerColliders = GachaClawRig.CollectFingerShapes(this.leftFingerBone);
			}
			if (this.rightFingerBone != null && (this._rightFingerColliders == null || this._rightFingerColliders.Length == 0))
			{
				this._rightFingerColliders = GachaClawRig.CollectFingerShapes(this.rightFingerBone);
			}
			if (this._palmCollider == null && this.clawMotor != null)
			{
				Transform transform = this.clawMotor.Find("GachaClawPalmCollider");
				if (transform != null)
				{
					this._palmCollider = transform.GetComponent<BoxCollider>();
				}
			}
		}

		// Token: 0x06008CFF RID: 36095 RVA: 0x00398D84 File Offset: 0x00396F84
		private static BoxCollider[] CollectFingerShapes(Transform fingerBone)
		{
			List<BoxCollider> list = new List<BoxCollider>(4);
			foreach (BoxCollider boxCollider in fingerBone.GetComponentsInChildren<BoxCollider>(true))
			{
				if (boxCollider != null && boxCollider.gameObject.name.StartsWith("GachaFingerCollider", StringComparison.Ordinal))
				{
					list.Add(boxCollider);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06008D00 RID: 36096 RVA: 0x00398DE0 File Offset: 0x00396FE0
		private void ResolveFingerBones()
		{
			if (this.clawMotor == null || this.FingersConfigured)
			{
				return;
			}
			string text = ((this.side == GachaMachineSide.Left) ? "L" : "R");
			if (this.leftFingerBone == null)
			{
				this.leftFingerBone = GachaClawRig.FindDescendantExact(this.clawMotor, "GMClawLeft_" + text);
			}
			if (this.rightFingerBone == null)
			{
				this.rightFingerBone = GachaClawRig.FindDescendantExact(this.clawMotor, "GMClawRight_" + text);
			}
		}

		// Token: 0x06008D01 RID: 36097 RVA: 0x00398E70 File Offset: 0x00397070
		private void EnsureGrabber()
		{
			if (this.grabber == null)
			{
				this.grabber = base.GetComponent<GachaClawGrabber>();
			}
			if (this.grabber == null && Application.isPlaying)
			{
				this.grabber = base.gameObject.AddComponent<GachaClawGrabber>();
			}
			if (this.grabber != null)
			{
				this.grabber.Bind(this, this.clawMotor);
			}
		}

		// Token: 0x06008D02 RID: 36098 RVA: 0x00398EE0 File Offset: 0x003970E0
		private static Transform FindDescendantExact(Transform root, string exactName)
		{
			if (root == null)
			{
				return null;
			}
			Transform[] componentsInChildren = root.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null && componentsInChildren[i].name == exactName)
				{
					return componentsInChildren[i];
				}
			}
			return null;
		}

		// Token: 0x06008D03 RID: 36099 RVA: 0x00398F30 File Offset: 0x00397130
		private void UpdateCable()
		{
			if (this.cableRenderer == null)
			{
				return;
			}
			Transform transform = ((this.cableTopAnchor != null) ? this.cableTopAnchor : this.upperArmBone);
			Transform transform2 = ((this.cableBottomAnchor != null) ? this.cableBottomAnchor : this.clawMotor);
			if (transform == null || transform2 == null)
			{
				this.cableRenderer.enabled = false;
				return;
			}
			this.cableRenderer.enabled = this._drop > this.cableRevealDrop + 0.001f;
			if (!this.cableRenderer.enabled)
			{
				return;
			}
			this.cableRenderer.positionCount = 2;
			if (this.cableRenderer.useWorldSpace)
			{
				this.cableRenderer.SetPosition(0, transform.position);
				this.cableRenderer.SetPosition(1, transform2.position);
				return;
			}
			Transform transform3 = this.cableRenderer.transform;
			this.cableRenderer.SetPosition(0, transform3.InverseTransformPoint(transform.position));
			this.cableRenderer.SetPosition(1, transform3.InverseTransformPoint(transform2.position));
		}

		// Token: 0x06008D04 RID: 36100 RVA: 0x0039904C File Offset: 0x0039724C
		private void OnValidate()
		{
			if (this.beamLocalXLimits.x > this.beamLocalXLimits.y)
			{
				this.beamLocalXLimits = new Vector2(this.beamLocalXLimits.y, this.beamLocalXLimits.x);
			}
			if (this.trolleyLocalZLimits.x > this.trolleyLocalZLimits.y)
			{
				this.trolleyLocalZLimits = new Vector2(this.trolleyLocalZLimits.y, this.trolleyLocalZLimits.x);
			}
			this.maximumDrop = Mathf.Max(0f, this.maximumDrop);
			this.upperArmStageTravel = Mathf.Clamp(this.upperArmStageTravel, 0f, this.maximumDrop);
			this.cableRevealDrop = Mathf.Clamp(this.cableRevealDrop, 0f, this.maximumDrop);
			this.horizontalSpeed = Mathf.Max(0.01f, this.horizontalSpeed);
			this.depthSpeed = Mathf.Max(0.01f, this.depthSpeed);
			this.verticalSpeed = Mathf.Max(0.01f, this.verticalSpeed);
			this.fingerClosedAngle = Mathf.Clamp(this.fingerClosedAngle, 0f, 60f);
			this.fingerClosureSpeed = Mathf.Max(0.01f, this.fingerClosureSpeed);
			if (this.fingerHingeAxis.sqrMagnitude < 0.0001f)
			{
				this.fingerHingeAxis = Vector3.forward;
			}
		}

		// Token: 0x06008D05 RID: 36101 RVA: 0x003991AC File Offset: 0x003973AC
		private bool BindPoseIsInsideLimits()
		{
			return this._beamBindLocalPosition.x >= this.beamLocalXLimits.x - 0.0001f && this._beamBindLocalPosition.x <= this.beamLocalXLimits.y + 0.0001f && this._trolleyBindLocalPosition.z >= this.trolleyLocalZLimits.x - 0.0001f && this._trolleyBindLocalPosition.z <= this.trolleyLocalZLimits.y + 0.0001f;
		}

		// Token: 0x04008FBF RID: 36799
		[Header("Identity")]
		[SerializeField]
		private GachaMachineSide side;

		// Token: 0x04008FC0 RID: 36800
		[Header("Rig Bones")]
		[SerializeField]
		private Transform beam;

		// Token: 0x04008FC1 RID: 36801
		[SerializeField]
		private Transform trolleyMotor;

		// Token: 0x04008FC2 RID: 36802
		[SerializeField]
		private Transform upperArmBone;

		// Token: 0x04008FC3 RID: 36803
		[SerializeField]
		private Transform lowerArmBone;

		// Token: 0x04008FC4 RID: 36804
		[SerializeField]
		private Transform clawMotor;

		// Token: 0x04008FC5 RID: 36805
		[Header("Claw Fingers")]
		[Tooltip("Skeleton hinge GMClawLeft_*; resolved below clawMotor when empty.")]
		[SerializeField]
		private Transform leftFingerBone;

		// Token: 0x04008FC6 RID: 36806
		[Tooltip("Skeleton hinge GMClawRight_*; resolved below clawMotor when empty.")]
		[SerializeField]
		private Transform rightFingerBone;

		// Token: 0x04008FC7 RID: 36807
		[Tooltip("Hinge axis expressed in GMClawMotor_* local space.")]
		[SerializeField]
		private Vector3 fingerHingeAxis = Vector3.forward;

		// Token: 0x04008FC8 RID: 36808
		[SerializeField]
		[Range(0f, 60f)]
		private float fingerClosedAngle = 44f;

		// Token: 0x04008FC9 RID: 36809
		[Tooltip("Full open-to-closed travel per second.")]
		[SerializeField]
		[Min(0.01f)]
		private float fingerClosureSpeed = 2.8f;

		// Token: 0x04008FCA RID: 36810
		[Tooltip("The two imported claws are mirrored; the right bay needs the opposite hinge sign or its fingers swing open instead of shut.")]
		[SerializeField]
		private bool invertFingerRotation;

		// Token: 0x04008FCB RID: 36811
		[Tooltip("Fingers with authored GachaFingerCollider_* shapes stop when they press into a plush, Ciel or the cabinet instead of clipping through.")]
		[SerializeField]
		private bool stopFingersOnContact = true;

		// Token: 0x04008FCC RID: 36812
		[SerializeField]
		[Range(0.5f, 6f)]
		private float fingerContactStepDegrees = 2.5f;

		// Token: 0x04008FCD RID: 36813
		[Tooltip("Toggle claw open/closed. Press once to close and again to release.")]
		[SerializeField]
		private GenericInput grabInput = new GenericInput("F", "X", "X");

		// Token: 0x04008FCE RID: 36814
		[Tooltip("Physical acquisition and pull driver. Created at runtime when omitted.")]
		[SerializeField]
		private GachaClawGrabber grabber;

		// Token: 0x04008FCF RID: 36815
		[Header("Cable")]
		[SerializeField]
		private LineRenderer cableRenderer;

		// Token: 0x04008FD0 RID: 36816
		[Tooltip("Defaults to GMArm01_* when empty.")]
		[SerializeField]
		private Transform cableTopAnchor;

		// Token: 0x04008FD1 RID: 36817
		[Tooltip("Defaults to GMClawMotor_* when empty.")]
		[SerializeField]
		private Transform cableBottomAnchor;

		// Token: 0x04008FD2 RID: 36818
		[Tooltip("The two rigid tubes overlap safely until roughly 0.28 m. Reveal the procedural cable only beyond that mechanical travel.")]
		[SerializeField]
		[Min(0f)]
		private float cableRevealDrop = 0.28f;

		// Token: 0x04008FD3 RID: 36819
		[Header("Travel Limits (authored local coordinates)")]
		[Tooltip("Absolute local X limits for GMBeam_*. The two bays intentionally use different values.")]
		[SerializeField]
		private Vector2 beamLocalXLimits = new Vector2(-0.67f, -0.23f);

		// Token: 0x04008FD4 RID: 36820
		[Tooltip("Absolute local Z limits for GMMotor_*. The imported motor starts at the front limit.")]
		[SerializeField]
		private Vector2 trolleyLocalZLimits = new Vector2(-0.655f, 0.655f);

		// Token: 0x04008FD5 RID: 36821
		[Tooltip("Total positive local-Y travel across GMArm01_* and GMArm02_*. +Y points down because the beam is rotated 180 degrees on X.")]
		[SerializeField]
		[Min(0f)]
		private float maximumDrop = 0.48f;

		// Token: 0x04008FD6 RID: 36822
		[Tooltip("First telescopic stage on GMArm01_*. The rest of maximumDrop is applied to GMArm02_*.")]
		[SerializeField]
		[Min(0f)]
		private float upperArmStageTravel = 0.1f;

		// Token: 0x04008FD7 RID: 36823
		[Header("Movement")]
		[SerializeField]
		[Min(0.01f)]
		private float horizontalSpeed = 0.38f;

		// Token: 0x04008FD8 RID: 36824
		[SerializeField]
		[Min(0.01f)]
		private float depthSpeed = 0.48f;

		// Token: 0x04008FD9 RID: 36825
		[SerializeField]
		[Min(0.01f)]
		private float verticalSpeed = 0.34f;

		// Token: 0x04008FDA RID: 36826
		[SerializeField]
		private string horizontalAxis = "Horizontal";

		// Token: 0x04008FDB RID: 36827
		[SerializeField]
		private string depthAxis = "Vertical";

		// Token: 0x04008FDC RID: 36828
		[Tooltip("Additional inversion after correcting the imported rig's camera-facing X.")]
		[SerializeField]
		private bool invertHorizontal;

		// Token: 0x04008FDD RID: 36829
		[SerializeField]
		private bool invertDepth;

		// Token: 0x04008FDE RID: 36830
		[SerializeField]
		private GenericInput lowerInput = new GenericInput("Space", "LeftTrigger", "LeftTrigger");

		// Token: 0x04008FDF RID: 36831
		[SerializeField]
		private GenericInput raiseInput = new GenericInput("LeftShift", "RightTrigger", "RightTrigger");

		// Token: 0x04008FE0 RID: 36832
		[Header("Lifecycle")]
		[Tooltip("Returns this claw to its imported rest pose whenever the player gives up control.")]
		[SerializeField]
		private bool resetRigOnControlDisable = true;

		// Token: 0x04008FE1 RID: 36833
		[Tooltip("Restores the imported bind pose when this component is disabled.")]
		[SerializeField]
		private bool resetRigOnDisable = true;

		// Token: 0x04008FE2 RID: 36834
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04008FE3 RID: 36835
		private Vector3 _beamBindLocalPosition;

		// Token: 0x04008FE4 RID: 36836
		private Vector3 _trolleyBindLocalPosition;

		// Token: 0x04008FE5 RID: 36837
		private Vector3 _upperArmBindLocalPosition;

		// Token: 0x04008FE6 RID: 36838
		private Vector3 _lowerArmBindLocalPosition;

		// Token: 0x04008FE7 RID: 36839
		private Quaternion _leftFingerBindLocalRotation;

		// Token: 0x04008FE8 RID: 36840
		private Quaternion _rightFingerBindLocalRotation;

		// Token: 0x04008FE9 RID: 36841
		private bool _bindPoseCaptured;

		// Token: 0x04008FEA RID: 36842
		private bool _controlEnabled;

		// Token: 0x04008FEB RID: 36843
		private float _drop;

		// Token: 0x04008FEC RID: 36844
		private float _closure;

		// Token: 0x04008FED RID: 36845
		private float _closureTarget;

		// Token: 0x04008FEE RID: 36846
		private float _leftFingerClosure;

		// Token: 0x04008FEF RID: 36847
		private float _rightFingerClosure;

		// Token: 0x04008FF0 RID: 36848
		private bool _leftFingerBlocked;

		// Token: 0x04008FF1 RID: 36849
		private bool _rightFingerBlocked;

		// Token: 0x04008FF2 RID: 36850
		private BoxCollider[] _leftFingerColliders = Array.Empty<BoxCollider>();

		// Token: 0x04008FF3 RID: 36851
		private BoxCollider[] _rightFingerColliders = Array.Empty<BoxCollider>();

		// Token: 0x04008FF4 RID: 36852
		private BoxCollider _palmCollider;

		// Token: 0x04008FF5 RID: 36853
		private readonly Collider[] _fingerOverlapBuffer = new Collider[24];
	}
}
