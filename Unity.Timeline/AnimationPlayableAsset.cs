using System;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000007 RID: 7
	[NotKeyable]
	[Serializable]
	public class AnimationPlayableAsset : PlayableAsset, ITimelineClipAsset, IPropertyPreview, ISerializationCallbackReceiver
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000018 RID: 24 RVA: 0x0000244F File Offset: 0x0000064F
		// (set) Token: 0x06000019 RID: 25 RVA: 0x00002457 File Offset: 0x00000657
		public Vector3 position
		{
			get
			{
				return this.m_Position;
			}
			set
			{
				this.m_Position = value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002460 File Offset: 0x00000660
		// (set) Token: 0x0600001B RID: 27 RVA: 0x0000246D File Offset: 0x0000066D
		public Quaternion rotation
		{
			get
			{
				return Quaternion.Euler(this.m_EulerAngles);
			}
			set
			{
				this.m_EulerAngles = value.eulerAngles;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001C RID: 28 RVA: 0x0000247C File Offset: 0x0000067C
		// (set) Token: 0x0600001D RID: 29 RVA: 0x00002484 File Offset: 0x00000684
		public Vector3 eulerAngles
		{
			get
			{
				return this.m_EulerAngles;
			}
			set
			{
				this.m_EulerAngles = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001E RID: 30 RVA: 0x0000248D File Offset: 0x0000068D
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002495 File Offset: 0x00000695
		public bool useTrackMatchFields
		{
			get
			{
				return this.m_UseTrackMatchFields;
			}
			set
			{
				this.m_UseTrackMatchFields = value;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000020 RID: 32 RVA: 0x0000249E File Offset: 0x0000069E
		// (set) Token: 0x06000021 RID: 33 RVA: 0x000024A6 File Offset: 0x000006A6
		public MatchTargetFields matchTargetFields
		{
			get
			{
				return this.m_MatchTargetFields;
			}
			set
			{
				this.m_MatchTargetFields = value;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000024AF File Offset: 0x000006AF
		// (set) Token: 0x06000023 RID: 35 RVA: 0x000024B7 File Offset: 0x000006B7
		public bool removeStartOffset
		{
			get
			{
				return this.m_RemoveStartOffset;
			}
			set
			{
				this.m_RemoveStartOffset = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000024C0 File Offset: 0x000006C0
		// (set) Token: 0x06000025 RID: 37 RVA: 0x000024C8 File Offset: 0x000006C8
		public bool applyFootIK
		{
			get
			{
				return this.m_ApplyFootIK;
			}
			set
			{
				this.m_ApplyFootIK = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000024D1 File Offset: 0x000006D1
		// (set) Token: 0x06000027 RID: 39 RVA: 0x000024D9 File Offset: 0x000006D9
		public AnimationPlayableAsset.LoopMode loop
		{
			get
			{
				return this.m_Loop;
			}
			set
			{
				this.m_Loop = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000028 RID: 40 RVA: 0x000024E2 File Offset: 0x000006E2
		internal bool hasRootTransforms
		{
			get
			{
				return this.m_Clip != null && AnimationPlayableAsset.HasRootTransforms(this.m_Clip);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000024FF File Offset: 0x000006FF
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00002507 File Offset: 0x00000707
		internal AppliedOffsetMode appliedOffsetMode { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002510 File Offset: 0x00000710
		// (set) Token: 0x0600002C RID: 44 RVA: 0x00002518 File Offset: 0x00000718
		public AnimationClip clip
		{
			get
			{
				return this.m_Clip;
			}
			set
			{
				if (value != null)
				{
					base.name = "AnimationPlayableAsset of " + value.name;
				}
				this.m_Clip = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002540 File Offset: 0x00000740
		public override double duration
		{
			get
			{
				double animationClipLength = TimeUtility.GetAnimationClipLength(this.clip);
				if (animationClipLength < 1.401298464324817E-45)
				{
					return base.duration;
				}
				return animationClipLength;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600002E RID: 46 RVA: 0x0000256D File Offset: 0x0000076D
		public override IEnumerable<PlayableBinding> outputs
		{
			get
			{
				yield return AnimationPlayableBinding.Create(base.name, this);
				yield break;
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000257D File Offset: 0x0000077D
		public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
		{
			return AnimationPlayableAsset.CreatePlayable(graph, this.m_Clip, this.position, this.eulerAngles, this.removeStartOffset, this.appliedOffsetMode, this.applyFootIK, this.m_Loop);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000025B0 File Offset: 0x000007B0
		internal static Playable CreatePlayable(PlayableGraph graph, AnimationClip clip, Vector3 positionOffset, Vector3 eulerOffset, bool removeStartOffset, AppliedOffsetMode mode, bool applyFootIK, AnimationPlayableAsset.LoopMode loop)
		{
			if (clip == null || clip.legacy)
			{
				return Playable.Null;
			}
			AnimationClipPlayable animationClipPlayable = AnimationClipPlayable.Create(graph, clip);
			animationClipPlayable.SetRemoveStartOffset(removeStartOffset);
			animationClipPlayable.SetApplyFootIK(applyFootIK);
			animationClipPlayable.SetOverrideLoopTime(loop > AnimationPlayableAsset.LoopMode.UseSourceAsset);
			animationClipPlayable.SetLoopTime(loop == AnimationPlayableAsset.LoopMode.On);
			Playable playable = animationClipPlayable;
			if (AnimationPlayableAsset.ShouldApplyScaleRemove(mode))
			{
				AnimationRemoveScalePlayable animationRemoveScalePlayable = AnimationRemoveScalePlayable.Create(graph, 1);
				graph.Connect<Playable, AnimationRemoveScalePlayable>(playable, 0, animationRemoveScalePlayable, 0);
				animationRemoveScalePlayable.SetInputWeight(0, 1f);
				playable = animationRemoveScalePlayable;
			}
			if (AnimationPlayableAsset.ShouldApplyOffset(mode, clip))
			{
				AnimationOffsetPlayable animationOffsetPlayable = AnimationOffsetPlayable.Create(graph, positionOffset, Quaternion.Euler(eulerOffset), 1);
				graph.Connect<Playable, AnimationOffsetPlayable>(playable, 0, animationOffsetPlayable, 0);
				animationOffsetPlayable.SetInputWeight(0, 1f);
				playable = animationOffsetPlayable;
			}
			return playable;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002676 File Offset: 0x00000876
		private static bool ShouldApplyOffset(AppliedOffsetMode mode, AnimationClip clip)
		{
			return mode != AppliedOffsetMode.NoRootTransform && mode != AppliedOffsetMode.SceneOffsetLegacy && AnimationPlayableAsset.HasRootTransforms(clip);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002687 File Offset: 0x00000887
		private static bool ShouldApplyScaleRemove(AppliedOffsetMode mode)
		{
			return mode == AppliedOffsetMode.SceneOffsetLegacyEditor || mode == AppliedOffsetMode.SceneOffsetLegacy || mode == AppliedOffsetMode.TransformOffsetLegacy;
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002698 File Offset: 0x00000898
		public ClipCaps clipCaps
		{
			get
			{
				ClipCaps clipCaps = ClipCaps.Extrapolation | ClipCaps.SpeedMultiplier | ClipCaps.Blending;
				if (this.m_Clip != null && this.m_Loop != AnimationPlayableAsset.LoopMode.Off && (this.m_Loop != AnimationPlayableAsset.LoopMode.UseSourceAsset || this.m_Clip.isLooping))
				{
					clipCaps |= ClipCaps.Looping;
				}
				if (this.m_Clip != null && !this.m_Clip.empty)
				{
					clipCaps |= ClipCaps.ClipIn;
				}
				return clipCaps;
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000026F8 File Offset: 0x000008F8
		public void ResetOffsets()
		{
			this.position = Vector3.zero;
			this.eulerAngles = Vector3.zero;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002710 File Offset: 0x00000910
		public void GatherProperties(PlayableDirector director, IPropertyCollector driver)
		{
			driver.AddFromClip(this.m_Clip);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000271E File Offset: 0x0000091E
		internal static bool HasRootTransforms(AnimationClip clip)
		{
			return !(clip == null) && !clip.empty && (clip.hasRootMotion || clip.hasGenericRootTransform || clip.hasMotionCurves || clip.hasRootCurves);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002753 File Offset: 0x00000953
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			this.m_Version = AnimationPlayableAsset.k_LatestVersion;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002760 File Offset: 0x00000960
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (this.m_Version < AnimationPlayableAsset.k_LatestVersion)
			{
				this.OnUpgradeFromVersion(this.m_Version);
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000277B File Offset: 0x0000097B
		private void OnUpgradeFromVersion(int oldVersion)
		{
			if (oldVersion < 1)
			{
				AnimationPlayableAsset.AnimationPlayableAssetUpgrade.ConvertRotationToEuler(this);
			}
		}

		// Token: 0x04000009 RID: 9
		[SerializeField]
		private AnimationClip m_Clip;

		// Token: 0x0400000A RID: 10
		[SerializeField]
		private Vector3 m_Position = Vector3.zero;

		// Token: 0x0400000B RID: 11
		[SerializeField]
		private Vector3 m_EulerAngles = Vector3.zero;

		// Token: 0x0400000C RID: 12
		[SerializeField]
		private bool m_UseTrackMatchFields = true;

		// Token: 0x0400000D RID: 13
		[SerializeField]
		private MatchTargetFields m_MatchTargetFields = MatchTargetFieldConstants.All;

		// Token: 0x0400000E RID: 14
		[SerializeField]
		private bool m_RemoveStartOffset = true;

		// Token: 0x0400000F RID: 15
		[SerializeField]
		private bool m_ApplyFootIK = true;

		// Token: 0x04000010 RID: 16
		[SerializeField]
		private AnimationPlayableAsset.LoopMode m_Loop;

		// Token: 0x04000012 RID: 18
		private static readonly int k_LatestVersion = 1;

		// Token: 0x04000013 RID: 19
		[SerializeField]
		[HideInInspector]
		private int m_Version;

		// Token: 0x04000014 RID: 20
		[SerializeField]
		[Obsolete("Use m_RotationEuler Instead", false)]
		[HideInInspector]
		private Quaternion m_Rotation = Quaternion.identity;

		// Token: 0x0200005A RID: 90
		public enum LoopMode
		{
			// Token: 0x0400011B RID: 283
			[Tooltip("Use the loop time setting from the source AnimationClip.")]
			UseSourceAsset,
			// Token: 0x0400011C RID: 284
			[Tooltip("The source AnimationClip loops during playback.")]
			On,
			// Token: 0x0400011D RID: 285
			[Tooltip("The source AnimationClip does not loop during playback.")]
			Off
		}

		// Token: 0x0200005B RID: 91
		private enum Versions
		{
			// Token: 0x0400011F RID: 287
			Initial,
			// Token: 0x04000120 RID: 288
			RotationAsEuler
		}

		// Token: 0x0200005C RID: 92
		private static class AnimationPlayableAssetUpgrade
		{
			// Token: 0x06000310 RID: 784 RVA: 0x0000B1D8 File Offset: 0x000093D8
			public static void ConvertRotationToEuler(AnimationPlayableAsset asset)
			{
				asset.m_EulerAngles = asset.m_Rotation.eulerAngles;
			}
		}
	}
}
