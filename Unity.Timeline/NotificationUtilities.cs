using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x0200004F RID: 79
	internal static class NotificationUtilities
	{
		// Token: 0x060002E5 RID: 741 RVA: 0x0000A357 File Offset: 0x00008557
		public static ScriptPlayable<TimeNotificationBehaviour> CreateNotificationsPlayable(PlayableGraph graph, IEnumerable<IMarker> markers, PlayableDirector director)
		{
			return NotificationUtilities.CreateNotificationsPlayable(graph, markers, null, director);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000A362 File Offset: 0x00008562
		public static ScriptPlayable<TimeNotificationBehaviour> CreateNotificationsPlayable(PlayableGraph graph, IEnumerable<IMarker> markers, TimelineAsset timelineAsset)
		{
			return NotificationUtilities.CreateNotificationsPlayable(graph, markers, timelineAsset, null);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000A370 File Offset: 0x00008570
		private static ScriptPlayable<TimeNotificationBehaviour> CreateNotificationsPlayable(PlayableGraph graph, IEnumerable<IMarker> markers, IPlayableAsset asset, PlayableDirector director)
		{
			ScriptPlayable<TimeNotificationBehaviour> scriptPlayable = ScriptPlayable<TimeNotificationBehaviour>.Null;
			DirectorWrapMode directorWrapMode = ((director != null) ? director.extrapolationMode : DirectorWrapMode.None);
			bool flag = false;
			double num = 0.0;
			foreach (IMarker marker in markers)
			{
				INotification notification = marker as INotification;
				if (notification != null)
				{
					if (!flag)
					{
						num = ((director != null) ? director.playableAsset.duration : asset.duration);
						flag = true;
					}
					if (scriptPlayable.Equals(ScriptPlayable<TimeNotificationBehaviour>.Null))
					{
						scriptPlayable = TimeNotificationBehaviour.Create(graph, num, directorWrapMode);
					}
					DiscreteTime discreteTime = (DiscreteTime)marker.time;
					DiscreteTime discreteTime2 = (DiscreteTime)num;
					if (discreteTime >= discreteTime2 && discreteTime <= discreteTime2.OneTickAfter() && discreteTime2 != 0)
					{
						discreteTime = discreteTime2.OneTickBefore();
					}
					INotificationOptionProvider notificationOptionProvider = marker as INotificationOptionProvider;
					if (notificationOptionProvider != null)
					{
						scriptPlayable.GetBehaviour().AddNotification((double)discreteTime, notification, notificationOptionProvider.flags);
					}
					else
					{
						scriptPlayable.GetBehaviour().AddNotification((double)discreteTime, notification, NotificationFlags.Retroactive);
					}
				}
			}
			return scriptPlayable;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000A4B8 File Offset: 0x000086B8
		public static bool TrackTypeSupportsNotifications(Type type)
		{
			TrackBindingTypeAttribute trackBindingTypeAttribute = (TrackBindingTypeAttribute)Attribute.GetCustomAttribute(type, typeof(TrackBindingTypeAttribute));
			return trackBindingTypeAttribute != null && (typeof(Component).IsAssignableFrom(trackBindingTypeAttribute.type) || typeof(GameObject).IsAssignableFrom(trackBindingTypeAttribute.type));
		}
	}
}
