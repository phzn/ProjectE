using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x0200071C RID: 1820
	[vClassHeader("Trigger Generic Action", true, "icon_v2", false, "", openClose = true, iconName = "triggerIcon")]
	public class vTriggerGenericAction : vMonoBehaviour
	{
		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06002F41 RID: 12097 RVA: 0x0010B364 File Offset: 0x00109564
		// (set) Token: 0x06002F42 RID: 12098 RVA: 0x0010B36C File Offset: 0x0010956C
		public bool CanDoAction
		{
			get
			{
				return this.canDoAction;
			}
			set
			{
				this.canDoAction = value;
			}
		}

		// Token: 0x06002F43 RID: 12099 RVA: 0x0010B378 File Offset: 0x00109578
		protected virtual void Start()
		{
			base.gameObject.tag = this.actionTag;
			base.gameObject.layer = LayerMask.NameToLayer("Triggers");
			this._collider = base.GetComponent<Collider>();
			this._collider.isTrigger = true;
			if (this.disableOnStart)
			{
				base.enabled = false;
			}
		}

		// Token: 0x06002F44 RID: 12100 RVA: 0x0010B3D2 File Offset: 0x001095D2
		public virtual IEnumerator OnPressActionDelay(GameObject obj)
		{
			yield return new WaitForSeconds(this.onPressActionDelay);
			this.OnPressActionInput.Invoke();
			if (obj)
			{
				this.onPressActionInputWithTarget.Invoke(obj);
			}
			yield break;
		}

		// Token: 0x06002F45 RID: 12101 RVA: 0x0010B3E8 File Offset: 0x001095E8
		public void UpdateButtonTimer(float value)
		{
			if (value != this.currentButtonTimer)
			{
				this.currentButtonTimer = value;
				this.OnUpdateButtonTimer.Invoke(value);
			}
		}

		// Token: 0x04002F74 RID: 12148
		[vEditorToolbar("Input", false, "", false, false, order = 1)]
		public vTriggerGenericAction.InputType inputType;

		// Token: 0x04002F75 RID: 12149
		[Tooltip("Input to make the action")]
		public GenericInput actionInput = new GenericInput("E", "A", "A");

		// Token: 0x04002F76 RID: 12150
		[vHelpBox("Time you have to hold the button *Only for GetButtonTimer*", vHelpBoxAttribute.MessageType.None)]
		public float buttonTimer = 3f;

		// Token: 0x04002F77 RID: 12151
		[vHelpBox("Add delay to start the input count *Only for GetButtonTimer*", vHelpBoxAttribute.MessageType.None)]
		public float inputDelay = 0.1f;

		// Token: 0x04002F78 RID: 12152
		[vHelpBox("*Only for GetButtonTimer* \n\n<b>TRUE: </b> Play the animation while you're holding the button \n<b>FALSE: </b>Play the animation after you finish holding the button", vHelpBoxAttribute.MessageType.None)]
		public bool playAnimationWhileHoldingButton = true;

		// Token: 0x04002F79 RID: 12153
		[vHelpBox("Time to press the button twice *Only for GetDoubleButton*", vHelpBoxAttribute.MessageType.None)]
		public float doubleButtomTime = 0.25f;

		// Token: 0x04002F7A RID: 12154
		[vEditorToolbar("Trigger", false, "", false, false, order = 2)]
		[SerializeField]
		protected bool canDoAction = true;

		// Token: 0x04002F7B RID: 12155
		public string actionName = "Action";

		// Token: 0x04002F7C RID: 12156
		public string actionTag = "Action";

		// Token: 0x04002F7D RID: 12157
		[vHelpBox("Disable this trigger OnStart", vHelpBoxAttribute.MessageType.None)]
		public bool disableOnStart;

		// Token: 0x04002F7E RID: 12158
		[vHelpBox("Disable the Player's Capsule Collider Collision, useful for animations with closer interactions", vHelpBoxAttribute.MessageType.None)]
		public bool disableCollision;

		// Token: 0x04002F7F RID: 12159
		[vHelpBox("Disable the Player's Rigidbody Gravity, useful for on air animations", vHelpBoxAttribute.MessageType.None)]
		public bool disableGravity;

		// Token: 0x04002F80 RID: 12160
		[vHelpBox("It will only use the trigger if the forward of the character is close to the forward of this transform", vHelpBoxAttribute.MessageType.None)]
		public bool activeFromForward;

		// Token: 0x04002F81 RID: 12161
		[vHelpBox("Max angle between character forward and trigger forward to active trigger", vHelpBoxAttribute.MessageType.None)]
		[Range(5f, 180f)]
		public float forwardAngle = 55f;

		// Token: 0x04002F82 RID: 12162
		[vHelpBox("Rotate Character to the Forward Rotation of this Trigger", vHelpBoxAttribute.MessageType.None)]
		public bool useTriggerRotation;

		// Token: 0x04002F83 RID: 12163
		[vHelpBox("Destroy this Trigger after pressing the Input or AutoAction or finishing the Action", vHelpBoxAttribute.MessageType.None)]
		public bool destroyAfter;

		// Token: 0x04002F84 RID: 12164
		[vHideInInspector("destroyAfter", false)]
		public float destroyDelay;

		// Token: 0x04002F85 RID: 12165
		[vHelpBox("Change your CameraState to a Custom State while playing the animation", vHelpBoxAttribute.MessageType.None)]
		public string customCameraState;

		// Token: 0x04002F86 RID: 12166
		[vEditorToolbar("Animation", false, "", false, false, order = 2)]
		[vHelpBox("Trigger a Animation - Use the exactly same name of the AnimationState you want to trigger, don't forget to add a vAnimatorTag to your State", vHelpBoxAttribute.MessageType.None)]
		public string playAnimation;

		// Token: 0x04002F87 RID: 12167
		public float crossFadeTransition = 0.25f;

		// Token: 0x04002F88 RID: 12168
		public int animatorLayer;

		// Token: 0x04002F89 RID: 12169
		[vHelpBox("Check the Exit Time of your animation (if it doesn't loop) and insert here. \n\nFor example if your Exit Time is 0.82 you need to insert 0.82\n\nAlways check with the Debug of the GenericAction if your animation is finishing correctly, otherwise the controller won't reset to the default physics and collision.", vHelpBoxAttribute.MessageType.Warning)]
		[Tooltip("You can use this to make a persistent action, and finish the action calling FinishAction method of the vGenericAction  component in your character")]
		public bool endActionManualy;

		// Token: 0x04002F8A RID: 12170
		[vHideInInspector("endActionManualy", false, invertValue = true)]
		public float endExitTimeAnimation = 0.8f;

		// Token: 0x04002F8B RID: 12171
		[vHelpBox("Use a ActionState value to apply special conditions for your AnimatorController transitions", vHelpBoxAttribute.MessageType.None)]
		public int animatorActionState;

		// Token: 0x04002F8C RID: 12172
		[vHelpBox("Reset the ActionState parameter to 0 after playing the animation", vHelpBoxAttribute.MessageType.None)]
		public bool resetAnimatorActionState = true;

		// Token: 0x04002F8D RID: 12173
		[vHelpBox("Use a empty transform as reference for the MatchTarget", vHelpBoxAttribute.MessageType.None)]
		public Transform matchTarget;

		// Token: 0x04002F8E RID: 12174
		[vHelpBox("Select the bone you want to use as reference to the Match Target", vHelpBoxAttribute.MessageType.None)]
		public AvatarTarget avatarTarget;

		// Token: 0x04002F8F RID: 12175
		[Header("Curve Match target system")]
		public bool useLocalX;

		// Token: 0x04002F90 RID: 12176
		public bool useLocalZ = true;

		// Token: 0x04002F91 RID: 12177
		public AnimationCurve matchPositionXZCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04002F92 RID: 12178
		public AnimationCurve matchPositionYCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04002F93 RID: 12179
		public AnimationCurve matchRotationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04002F94 RID: 12180
		[vEditorToolbar("Events", false, "", false, false, order = 3)]
		[Tooltip("Delay to run the OnDoAction Event")]
		[FormerlySerializedAs("onDoActionDelay")]
		public float onPressActionDelay;

		// Token: 0x04002F95 RID: 12181
		[Header("--- INPUT EVENTS ---")]
		[FormerlySerializedAs("OnDoAction")]
		public UnityEvent OnPressActionInput;

		// Token: 0x04002F96 RID: 12182
		public OnDoActionWithTarget onPressActionInputWithTarget;

		// Token: 0x04002F97 RID: 12183
		[Header("--- ONLY FOR GET BUTTON TIMER ---")]
		public UnityEvent OnCancelActionInput;

		// Token: 0x04002F98 RID: 12184
		public UnityEvent OnFinishActionInput;

		// Token: 0x04002F99 RID: 12185
		public vTriggerGenericAction.OnUpdateValue OnUpdateButtonTimer;

		// Token: 0x04002F9A RID: 12186
		[Header("--- ANIMATION EVENTS ---")]
		public UnityEvent OnStartAnimation;

		// Token: 0x04002F9B RID: 12187
		public UnityEvent OnEndAnimation;

		// Token: 0x04002F9C RID: 12188
		[Header("--- PLAYER AND TRIGGER DETECTION ---")]
		public OnDoActionWithTarget OnPlayerEnter;

		// Token: 0x04002F9D RID: 12189
		public OnDoActionWithTarget OnPlayerStay;

		// Token: 0x04002F9E RID: 12190
		public OnDoActionWithTarget OnPlayerExit;

		// Token: 0x04002F9F RID: 12191
		[Header("--- ACTION VALIDATION  ---")]
		public OnDoActionWithTarget OnValidate;

		// Token: 0x04002FA0 RID: 12192
		public OnDoActionWithTarget OnInvalidate;

		// Token: 0x04002FA1 RID: 12193
		public UnityEvent OnCancelAction;

		// Token: 0x04002FA2 RID: 12194
		private float currentButtonTimer;

		// Token: 0x04002FA3 RID: 12195
		internal Collider _collider;

		// Token: 0x0200071D RID: 1821
		public enum InputType
		{
			// Token: 0x04002FA5 RID: 12197
			GetButtonDown,
			// Token: 0x04002FA6 RID: 12198
			GetDoubleButton,
			// Token: 0x04002FA7 RID: 12199
			GetButtonTimer,
			// Token: 0x04002FA8 RID: 12200
			AutoAction
		}

		// Token: 0x0200071E RID: 1822
		[Serializable]
		public class OnUpdateValue : UnityEvent<float>
		{
		}
	}
}
