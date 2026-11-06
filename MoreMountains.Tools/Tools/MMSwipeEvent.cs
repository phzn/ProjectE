using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000189 RID: 393
	public struct MMSwipeEvent
	{
		// Token: 0x0600093B RID: 2363 RVA: 0x0002AA7E File Offset: 0x00028C7E
		public MMSwipeEvent(MMPossibleSwipeDirections direction, float angle, float length, Vector2 origin, Vector2 destination, float swipeDuration)
		{
			this.SwipeDirection = direction;
			this.SwipeAngle = angle;
			this.SwipeLength = length;
			this.SwipeOrigin = origin;
			this.SwipeDestination = destination;
			this.SwipeDuration = swipeDuration;
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0002AAB0 File Offset: 0x00028CB0
		public static void Trigger(MMPossibleSwipeDirections direction, float angle, float length, Vector2 origin, Vector2 destination, float swipeDuration)
		{
			MMSwipeEvent.e.SwipeDirection = direction;
			MMSwipeEvent.e.SwipeAngle = angle;
			MMSwipeEvent.e.SwipeLength = length;
			MMSwipeEvent.e.SwipeOrigin = origin;
			MMSwipeEvent.e.SwipeDestination = destination;
			MMSwipeEvent.e.SwipeDuration = swipeDuration;
			MMEventManager.TriggerEvent<MMSwipeEvent>(MMSwipeEvent.e);
		}

		// Token: 0x04000AC7 RID: 2759
		public MMPossibleSwipeDirections SwipeDirection;

		// Token: 0x04000AC8 RID: 2760
		public float SwipeAngle;

		// Token: 0x04000AC9 RID: 2761
		public float SwipeLength;

		// Token: 0x04000ACA RID: 2762
		public Vector2 SwipeOrigin;

		// Token: 0x04000ACB RID: 2763
		public Vector2 SwipeDestination;

		// Token: 0x04000ACC RID: 2764
		public float SwipeDuration;

		// Token: 0x04000ACD RID: 2765
		private static MMSwipeEvent e;
	}
}
