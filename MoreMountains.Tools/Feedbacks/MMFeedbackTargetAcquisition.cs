using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001C0 RID: 448
	[Serializable]
	public class MMFeedbackTargetAcquisition
	{
		// Token: 0x06000AA0 RID: 2720 RVA: 0x0002FE98 File Offset: 0x0002E098
		public static MMF_ReferenceHolder GetReferenceHolder(MMFeedbackTargetAcquisition settings, MMF_Player owner, int currentFeedbackIndex)
		{
			switch (settings.Mode)
			{
			case MMFeedbackTargetAcquisition.Modes.FirstReferenceHolder:
				return owner.GetFeedbackOfType<MMF_ReferenceHolder>(MMF_Player.AccessMethods.First, currentFeedbackIndex);
			case MMFeedbackTargetAcquisition.Modes.PreviousReferenceHolder:
				return owner.GetFeedbackOfType<MMF_ReferenceHolder>(MMF_Player.AccessMethods.Previous, currentFeedbackIndex);
			case MMFeedbackTargetAcquisition.Modes.ClosestReferenceHolder:
				return owner.GetFeedbackOfType<MMF_ReferenceHolder>(MMF_Player.AccessMethods.Closest, currentFeedbackIndex);
			case MMFeedbackTargetAcquisition.Modes.NextReferenceHolder:
				return owner.GetFeedbackOfType<MMF_ReferenceHolder>(MMF_Player.AccessMethods.Next, currentFeedbackIndex);
			case MMFeedbackTargetAcquisition.Modes.LastReferenceHolder:
				return owner.GetFeedbackOfType<MMF_ReferenceHolder>(MMF_Player.AccessMethods.Last, currentFeedbackIndex);
			default:
				return null;
			}
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0002FEF8 File Offset: 0x0002E0F8
		public static GameObject FindAutomatedTargetGameObject(MMFeedbackTargetAcquisition settings, MMF_Player owner, int currentFeedbackIndex)
		{
			if (owner.FeedbacksList[currentFeedbackIndex].ForcedReferenceHolder != null)
			{
				return owner.FeedbacksList[currentFeedbackIndex].ForcedReferenceHolder.GameObjectReference;
			}
			MMFeedbackTargetAcquisition._referenceHolder = MMFeedbackTargetAcquisition.GetReferenceHolder(settings, owner, currentFeedbackIndex);
			switch (settings.Mode)
			{
			case MMFeedbackTargetAcquisition.Modes.Self:
				return owner.gameObject;
			case MMFeedbackTargetAcquisition.Modes.AnyChild:
				return owner.transform.GetChild(0).gameObject;
			case MMFeedbackTargetAcquisition.Modes.ChildAtIndex:
				return owner.transform.GetChild(settings.ChildIndex).gameObject;
			case MMFeedbackTargetAcquisition.Modes.Parent:
				return owner.transform.parent.gameObject;
			case MMFeedbackTargetAcquisition.Modes.FirstReferenceHolder:
			case MMFeedbackTargetAcquisition.Modes.PreviousReferenceHolder:
			case MMFeedbackTargetAcquisition.Modes.ClosestReferenceHolder:
			case MMFeedbackTargetAcquisition.Modes.NextReferenceHolder:
			case MMFeedbackTargetAcquisition.Modes.LastReferenceHolder:
			{
				MMF_ReferenceHolder referenceHolder = MMFeedbackTargetAcquisition._referenceHolder;
				if (referenceHolder == null)
				{
					return null;
				}
				return referenceHolder.GameObjectReference;
			}
			default:
				return null;
			}
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0002FFC4 File Offset: 0x0002E1C4
		public static T FindAutomatedTarget<T>(MMFeedbackTargetAcquisition settings, MMF_Player owner, int currentFeedbackIndex)
		{
			if (owner.FeedbacksList[currentFeedbackIndex].ForcedReferenceHolder != null)
			{
				return owner.FeedbacksList[currentFeedbackIndex].ForcedReferenceHolder.GameObjectReference.GetComponent<T>();
			}
			MMFeedbackTargetAcquisition._referenceHolder = MMFeedbackTargetAcquisition.GetReferenceHolder(settings, owner, currentFeedbackIndex);
			switch (settings.Mode)
			{
			case MMFeedbackTargetAcquisition.Modes.Self:
				return owner.GetComponent<T>();
			case MMFeedbackTargetAcquisition.Modes.AnyChild:
			{
				for (int i = 0; i < owner.transform.childCount; i++)
				{
					if (owner.transform.GetChild(i).GetComponent<T>() != null)
					{
						return owner.transform.GetChild(i).GetComponent<T>();
					}
				}
				return owner.GetComponentInChildren<T>();
			}
			case MMFeedbackTargetAcquisition.Modes.ChildAtIndex:
				return owner.transform.GetChild(settings.ChildIndex).gameObject.GetComponent<T>();
			case MMFeedbackTargetAcquisition.Modes.Parent:
				return owner.transform.parent.GetComponentInParent<T>();
			case MMFeedbackTargetAcquisition.Modes.FirstReferenceHolder:
			case MMFeedbackTargetAcquisition.Modes.PreviousReferenceHolder:
			case MMFeedbackTargetAcquisition.Modes.ClosestReferenceHolder:
			case MMFeedbackTargetAcquisition.Modes.NextReferenceHolder:
			case MMFeedbackTargetAcquisition.Modes.LastReferenceHolder:
				if (MMFeedbackTargetAcquisition._referenceHolder == null)
				{
					return default(T);
				}
				return MMFeedbackTargetAcquisition._referenceHolder.GameObjectReference.GetComponent<T>();
			default:
				return default(T);
			}
		}

		// Token: 0x04000CB0 RID: 3248
		[Tooltip("the selected mode for target acquisition\nNone : nothing will happen\nSelf : the target will be picked on the MMF Player's game object\nAnyChild : the target will be picked on any of the MMF Player's child objects\nChildAtIndex : the target will be picked on the child at index X of the MMF Player\nParent : the target will be picked on the first parent where a matching target is found\nVarious reference holders : the target will be picked on the specified reference holder in the list (either the first one, previous : first one found before this feedback in the list, closest in any direction from this feedback, the next one found, or the last one in the list)")]
		public MMFeedbackTargetAcquisition.Modes Mode;

		// Token: 0x04000CB1 RID: 3249
		[MMFEnumCondition("Mode", new int[] { 3 })]
		public int ChildIndex;

		// Token: 0x04000CB2 RID: 3250
		private static MMF_ReferenceHolder _referenceHolder;

		// Token: 0x02000491 RID: 1169
		public enum Modes
		{
			// Token: 0x04002147 RID: 8519
			None,
			// Token: 0x04002148 RID: 8520
			Self,
			// Token: 0x04002149 RID: 8521
			AnyChild,
			// Token: 0x0400214A RID: 8522
			ChildAtIndex,
			// Token: 0x0400214B RID: 8523
			Parent,
			// Token: 0x0400214C RID: 8524
			FirstReferenceHolder,
			// Token: 0x0400214D RID: 8525
			PreviousReferenceHolder,
			// Token: 0x0400214E RID: 8526
			ClosestReferenceHolder,
			// Token: 0x0400214F RID: 8527
			NextReferenceHolder,
			// Token: 0x04002150 RID: 8528
			LastReferenceHolder
		}
	}
}
