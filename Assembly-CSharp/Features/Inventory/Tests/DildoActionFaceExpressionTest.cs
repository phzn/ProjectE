using System;
using Features.Inventory.Actions;
using UnityEngine;

namespace Features.Inventory.Tests
{
	// Token: 0x02001162 RID: 4450
	public class DildoActionFaceExpressionTest : MonoBehaviour
	{
		// Token: 0x0600878D RID: 34701 RVA: 0x003776FC File Offset: 0x003758FC
		private void Start()
		{
			if (this.enableTestLogs)
			{
				Debug.Log("[DildoActionFaceExpressionTest] Starting face expression test...");
				if (this.testAction != null)
				{
					Debug.Log("[DildoActionFaceExpressionTest] Test action: " + this.testAction.name);
					Debug.Log(string.Format("[DildoActionFaceExpressionTest] FaceExpressionIndex: {0}", this.testAction.FaceExpressionIndex));
					Debug.Log(string.Format("[DildoActionFaceExpressionTest] DisableFaceLayer: {0}", this.testAction.DisableFaceLayer));
				}
				else
				{
					Debug.LogWarning("[DildoActionFaceExpressionTest] No test action assigned!");
				}
				if (this.testAction != null)
				{
					Debug.Log(string.Format("[DildoActionFaceExpressionTest] Setting test FaceExpressionIndex to: {0}", this.testFaceExpressionIndex));
				}
			}
		}

		// Token: 0x0600878E RID: 34702 RVA: 0x003777B8 File Offset: 0x003759B8
		[ContextMenu("Test Face Expression Application")]
		public void TestFaceExpressionApplication()
		{
			if (this.executor == null || this.testDildoTransform == null)
			{
				Debug.LogWarning("[DildoActionFaceExpressionTest] Missing executor or dildo transform for test!");
				return;
			}
			if (this.testAction == null)
			{
				Debug.LogWarning("[DildoActionFaceExpressionTest] No test action assigned!");
				return;
			}
			if (this.enableTestLogs)
			{
				Debug.Log("[DildoActionFaceExpressionTest] Executing test action with face expression...");
				Debug.Log("[DildoActionFaceExpressionTest] Action: " + this.testAction.DisplayName);
				Debug.Log(string.Format("[DildoActionFaceExpressionTest] FaceExpressionIndex: {0}", this.testAction.FaceExpressionIndex));
				Debug.Log(string.Format("[DildoActionFaceExpressionTest] DisableFaceLayer: {0}", this.testAction.DisableFaceLayer));
			}
			this.executor.Execute(this.testAction, this.testDildoTransform);
		}

		// Token: 0x0600878F RID: 34703 RVA: 0x00377888 File Offset: 0x00375A88
		[ContextMenu("Print Current Face Expression")]
		public void PrintCurrentFaceExpression()
		{
			Animator animator = this.FindPlayerAnimator();
			if (animator != null)
			{
				int integer = animator.GetInteger("FaceExpressionIndex");
				Debug.Log(string.Format("[DildoActionFaceExpressionTest] Current FaceExpressionIndex: {0}", integer));
				return;
			}
			Debug.LogWarning("[DildoActionFaceExpressionTest] Player animator not found!");
		}

		// Token: 0x06008790 RID: 34704 RVA: 0x003778D4 File Offset: 0x00375AD4
		private Animator FindPlayerAnimator()
		{
			if (this.executor != null)
			{
				return this.executor.GetComponentInChildren<Animator>();
			}
			foreach (Animator animator in Object.FindObjectsOfType<Animator>())
			{
				if (animator.gameObject.name.Contains("Player") || animator.gameObject.name.Contains("Ciel"))
				{
					return animator;
				}
			}
			return null;
		}

		// Token: 0x06008791 RID: 34705 RVA: 0x00377944 File Offset: 0x00375B44
		private void OnValidate()
		{
			this.testFaceExpressionIndex = Mathf.Clamp(this.testFaceExpressionIndex, -1, 10);
		}

		// Token: 0x04008A25 RID: 35365
		[Header("Test Configuration")]
		[Tooltip("DildoActionDefinition to test with face expressions")]
		[SerializeField]
		private DildoActionDefinition testAction;

		// Token: 0x04008A26 RID: 35366
		[Tooltip("Test face expression index to apply")]
		[SerializeField]
		private int testFaceExpressionIndex = 2;

		// Token: 0x04008A27 RID: 35367
		[Tooltip("Enable debug logs for testing")]
		[SerializeField]
		private bool enableTestLogs = true;

		// Token: 0x04008A28 RID: 35368
		[Header("References")]
		[Tooltip("DildoActionExecutor to test")]
		[SerializeField]
		private DildoActionExecutor executor;

		// Token: 0x04008A29 RID: 35369
		[Tooltip("Test dildo transform")]
		[SerializeField]
		private Transform testDildoTransform;
	}
}
