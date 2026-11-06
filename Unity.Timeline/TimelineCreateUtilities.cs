using System;
using System.Collections.Generic;

namespace UnityEngine.Timeline
{
	// Token: 0x02000051 RID: 81
	internal static class TimelineCreateUtilities
	{
		// Token: 0x060002ED RID: 749 RVA: 0x0000A6A8 File Offset: 0x000088A8
		public static string GenerateUniqueActorName(List<ScriptableObject> tracks, string name)
		{
			if (!tracks.Exists((ScriptableObject x) => x != null && x.name == name))
			{
				return name;
			}
			int num = 0;
			string text = name;
			if (!string.IsNullOrEmpty(name) && name[name.Length - 1] == ')')
			{
				int num2 = name.LastIndexOf('(');
				if (num2 > 0 && int.TryParse(name.Substring(num2 + 1, name.Length - num2 - 2), out num))
				{
					num++;
					text = name.Substring(0, num2);
				}
			}
			text = text.TrimEnd();
			for (int i = num; i < num + 5000; i++)
			{
				if (i > 0)
				{
					string result = string.Format("{0} ({1})", text, i);
					if (!tracks.Exists((ScriptableObject x) => x != null && x.name == result))
					{
						return result;
					}
				}
			}
			return name;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000A7BE File Offset: 0x000089BE
		public static void SaveAssetIntoObject(Object childAsset, Object masterAsset)
		{
			if (childAsset == null || masterAsset == null)
			{
				return;
			}
			if ((masterAsset.hideFlags & HideFlags.DontSave) != HideFlags.None)
			{
				childAsset.hideFlags |= HideFlags.DontSave;
				return;
			}
			childAsset.hideFlags |= HideFlags.HideInHierarchy;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000A7FC File Offset: 0x000089FC
		public static void RemoveAssetFromObject(Object childAsset, Object masterAsset)
		{
			if (!(childAsset == null))
			{
				masterAsset == null;
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000A810 File Offset: 0x00008A10
		public static AnimationClip CreateAnimationClipForTrack(string name, TrackAsset track, bool isLegacy)
		{
			TimelineAsset timelineAsset = ((track != null) ? track.timelineAsset : null);
			HideFlags hideFlags = ((track != null) ? track.hideFlags : HideFlags.None);
			AnimationClip animationClip = new AnimationClip();
			animationClip.legacy = isLegacy;
			animationClip.name = name;
			animationClip.frameRate = ((timelineAsset == null) ? ((float)TimelineAsset.EditorSettings.kDefaultFrameRate) : ((float)timelineAsset.editorSettings.frameRate));
			TimelineCreateUtilities.SaveAssetIntoObject(animationClip, timelineAsset);
			animationClip.hideFlags = hideFlags & ~HideFlags.HideInHierarchy;
			return animationClip;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000A88C File Offset: 0x00008A8C
		public static bool ValidateParentTrack(TrackAsset parent, Type childType)
		{
			if (childType == null || !typeof(TrackAsset).IsAssignableFrom(childType))
			{
				return false;
			}
			if (parent == null)
			{
				return true;
			}
			if (parent is ILayerable && !parent.isSubTrack && parent.GetType() == childType)
			{
				return true;
			}
			SupportsChildTracksAttribute supportsChildTracksAttribute = Attribute.GetCustomAttribute(parent.GetType(), typeof(SupportsChildTracksAttribute)) as SupportsChildTracksAttribute;
			if (supportsChildTracksAttribute == null)
			{
				return false;
			}
			if (supportsChildTracksAttribute.childType == null)
			{
				return true;
			}
			if (childType == supportsChildTracksAttribute.childType)
			{
				int num = 0;
				TrackAsset trackAsset = parent;
				while (trackAsset != null && trackAsset.isSubTrack)
				{
					num++;
					trackAsset = trackAsset.parent as TrackAsset;
				}
				return num < supportsChildTracksAttribute.levels;
			}
			return false;
		}
	}
}
