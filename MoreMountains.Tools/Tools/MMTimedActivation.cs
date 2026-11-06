using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MoreMountains.Tools
{
	// Token: 0x02000012 RID: 18
	[AddComponentMenu("More Mountains/Tools/Activation/MMTimedActivation")]
	public class MMTimedActivation : MonoBehaviour
	{
		// Token: 0x06000046 RID: 70 RVA: 0x00002BEB File Offset: 0x00000DEB
		protected virtual void Awake()
		{
			if (this.ActivationMode == MMTimedActivation.ActivationModes.Awake)
			{
				this.StartChangeState();
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002BFB File Offset: 0x00000DFB
		public virtual void TriggerSequence()
		{
			this.StartChangeState();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002C03 File Offset: 0x00000E03
		protected virtual void Start()
		{
			if (this.ActivationMode == MMTimedActivation.ActivationModes.Start)
			{
				this.StartChangeState();
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002C14 File Offset: 0x00000E14
		protected virtual void OnEnable()
		{
			if (this.ActivationMode == MMTimedActivation.ActivationModes.OnEnable)
			{
				this.StartChangeState();
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002C25 File Offset: 0x00000E25
		protected virtual void OnTriggerEnter(Collider collider)
		{
			if (this.ActivationMode == MMTimedActivation.ActivationModes.OnTriggerEnter && this.CorrectTagOrLayer(collider.gameObject))
			{
				this.StartChangeState();
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002C44 File Offset: 0x00000E44
		protected virtual void OnTriggerExit(Collider collider)
		{
			if (this.ActivationMode == MMTimedActivation.ActivationModes.OnTriggerExit && this.CorrectTagOrLayer(collider.gameObject))
			{
				this.StartChangeState();
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002C63 File Offset: 0x00000E63
		protected virtual void OnTriggerEnter2D(Collider2D collider)
		{
			if (this.ActivationMode == MMTimedActivation.ActivationModes.OnTriggerEnter2D && this.CorrectTagOrLayer(collider.gameObject))
			{
				this.StartChangeState();
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002C82 File Offset: 0x00000E82
		protected virtual void OnTriggerExit2D(Collider2D collider)
		{
			if (this.ActivationMode == MMTimedActivation.ActivationModes.OnTriggerExit2D && this.CorrectTagOrLayer(collider.gameObject))
			{
				this.StartChangeState();
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002CA4 File Offset: 0x00000EA4
		protected virtual bool CorrectTagOrLayer(GameObject target)
		{
			switch (this.TriggerMode)
			{
			case MMTimedActivation.TriggerModes.None:
				return true;
			case MMTimedActivation.TriggerModes.Tag:
				return target.CompareTag(this.TargetTriggerTag);
			case MMTimedActivation.TriggerModes.Layer:
				return ((1 << target.layer) & this.TargetTriggerLayer) != 0;
			default:
				return false;
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002CF9 File Offset: 0x00000EF9
		protected virtual void StartChangeState()
		{
			base.StartCoroutine(this.TimedActivationSequence());
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002D08 File Offset: 0x00000F08
		protected virtual IEnumerator TimedActivationSequence()
		{
			if (this.DelayMode == MMTimedActivation.DelayModes.Time)
			{
				yield return MMCoroutine.WaitFor(this.TimeBeforeStateChange);
			}
			else
			{
				yield return base.StartCoroutine(MMCoroutine.WaitForFrames(this.FrameCount));
			}
			this.StateChange();
			this.Activate();
			yield break;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002D17 File Offset: 0x00000F17
		protected virtual void Activate()
		{
			if (this.TimedActions != null)
			{
				this.TimedActions.Invoke();
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002D2C File Offset: 0x00000F2C
		protected virtual void StateChange()
		{
			foreach (GameObject gameObject in this.TargetGameObjects)
			{
				switch (this.TimeDestructionMode)
				{
				case MMTimedActivation.TimedStatusChange.Enable:
					gameObject.SetActive(true);
					break;
				case MMTimedActivation.TimedStatusChange.Disable:
					gameObject.SetActive(false);
					break;
				case MMTimedActivation.TimedStatusChange.Destroy:
					Object.Destroy(gameObject);
					break;
				}
			}
			foreach (MonoBehaviour monoBehaviour in this.TargetBehaviours)
			{
				switch (this.TimeDestructionMode)
				{
				case MMTimedActivation.TimedStatusChange.Enable:
					monoBehaviour.enabled = true;
					break;
				case MMTimedActivation.TimedStatusChange.Disable:
					monoBehaviour.enabled = false;
					break;
				case MMTimedActivation.TimedStatusChange.Destroy:
					Object.Destroy(monoBehaviour);
					break;
				}
			}
		}

		// Token: 0x0400004E RID: 78
		[Header("Trigger Mode")]
		public MMTimedActivation.ActivationModes ActivationMode = MMTimedActivation.ActivationModes.Start;

		// Token: 0x0400004F RID: 79
		[MMEnumCondition("ActivationMode", new int[] { 3, 4 })]
		public MMTimedActivation.TriggerModes TriggerMode;

		// Token: 0x04000050 RID: 80
		[MMEnumCondition("TriggerMode", new int[] { 2 })]
		public LayerMask TargetTriggerLayer;

		// Token: 0x04000051 RID: 81
		[MMEnumCondition("TriggerMode", new int[] { 1 })]
		public string TargetTriggerTag;

		// Token: 0x04000052 RID: 82
		[Header("Delay")]
		public MMTimedActivation.DelayModes DelayMode;

		// Token: 0x04000053 RID: 83
		[MMEnumCondition("DelayMode", new int[] { 0 })]
		public float TimeBeforeStateChange = 2f;

		// Token: 0x04000054 RID: 84
		[MMEnumCondition("DelayMode", new int[] { 1 })]
		public int FrameCount = 1;

		// Token: 0x04000055 RID: 85
		[Header("Timed Activation")]
		public List<GameObject> TargetGameObjects;

		// Token: 0x04000056 RID: 86
		public List<MonoBehaviour> TargetBehaviours;

		// Token: 0x04000057 RID: 87
		public MMTimedActivation.TimedStatusChange TimeDestructionMode = MMTimedActivation.TimedStatusChange.Disable;

		// Token: 0x04000058 RID: 88
		[Header("Actions")]
		public UnityEvent TimedActions;

		// Token: 0x0200039D RID: 925
		public enum TimedStatusChange
		{
			// Token: 0x04001D47 RID: 7495
			Enable,
			// Token: 0x04001D48 RID: 7496
			Disable,
			// Token: 0x04001D49 RID: 7497
			Destroy
		}

		// Token: 0x0200039E RID: 926
		public enum ActivationModes
		{
			// Token: 0x04001D4B RID: 7499
			Awake,
			// Token: 0x04001D4C RID: 7500
			Start,
			// Token: 0x04001D4D RID: 7501
			OnEnable,
			// Token: 0x04001D4E RID: 7502
			OnTriggerEnter,
			// Token: 0x04001D4F RID: 7503
			OnTriggerExit,
			// Token: 0x04001D50 RID: 7504
			OnTriggerEnter2D,
			// Token: 0x04001D51 RID: 7505
			OnTriggerExit2D,
			// Token: 0x04001D52 RID: 7506
			Script
		}

		// Token: 0x0200039F RID: 927
		public enum TriggerModes
		{
			// Token: 0x04001D54 RID: 7508
			None,
			// Token: 0x04001D55 RID: 7509
			Tag,
			// Token: 0x04001D56 RID: 7510
			Layer
		}

		// Token: 0x020003A0 RID: 928
		public enum DelayModes
		{
			// Token: 0x04001D58 RID: 7512
			Time,
			// Token: 0x04001D59 RID: 7513
			Frames
		}
	}
}
