using System;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x02000721 RID: 1825
	[vClassHeader("Trigger Ladder Action", false, "icon_v2", false, "")]
	public class vTriggerLadderAction : vMonoBehaviour
	{
		// Token: 0x04002FAD RID: 12205
		[vEditorToolbar("Settings", false, "", false, false)]
		[Header("Trigger Action Options")]
		[Tooltip("Automatically execute the action without the need to press a Button")]
		public bool autoAction;

		// Token: 0x04002FAE RID: 12206
		[Header("Enter")]
		[Tooltip("Trigger an Animation - Use the exactly same name of the AnimationState you want to trigger")]
		public string playAnimation;

		// Token: 0x04002FAF RID: 12207
		[Header("Exit")]
		[Tooltip("Trigger an Animation - Use the exactly same name of the AnimationState you want to trigger")]
		public string exitAnimation;

		// Token: 0x04002FB0 RID: 12208
		[Tooltip("Use this to limit the trigger to active if forward of character is close to this forward")]
		public bool activeFromForward;

		// Token: 0x04002FB1 RID: 12209
		[Tooltip("Rotate Character for this rotation when active")]
		public bool useTriggerRotation;

		// Token: 0x04002FB2 RID: 12210
		[Tooltip("Target Character parent, used to movable ladders to set character child of target, keep empty if ladder is static")]
		public Transform targetCharacterParent;

		// Token: 0x04002FB3 RID: 12211
		[vEditorToolbar("MatchTarget", false, "", false, false)]
		[Tooltip("Use a transform to help the character climb any height, take a look at the Example Scene ClimbUp, StepUp, JumpOver objects.")]
		public Transform matchTarget;

		// Token: 0x04002FB4 RID: 12212
		[Tooltip("Use a empty gameObject as a reference for the character to exit")]
		public Transform exitMatchTarget;

		// Token: 0x04002FB5 RID: 12213
		public AnimationCurve enterPositionXZCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04002FB6 RID: 12214
		public AnimationCurve enterPositionYCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04002FB7 RID: 12215
		public AnimationCurve exitPositionXZCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04002FB8 RID: 12216
		public AnimationCurve exitPositionYCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04002FB9 RID: 12217
		public AnimationCurve enterRotationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04002FBA RID: 12218
		public AnimationCurve exitRotationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04002FBB RID: 12219
		public UnityEvent OnDoAction;

		// Token: 0x04002FBC RID: 12220
		public UnityEvent OnPlayerEnter;

		// Token: 0x04002FBD RID: 12221
		public UnityEvent OnPlayerStay;

		// Token: 0x04002FBE RID: 12222
		public UnityEvent OnPlayerExit;
	}
}
