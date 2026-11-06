using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ProjectExhibitionist.Core.Rendering
{
	// Token: 0x02000A93 RID: 2707
	public static class GameplayVignetteMixer
	{
		// Token: 0x06004D1A RID: 19738 RVA: 0x001DECEC File Offset: 0x001DCEEC
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Reset()
		{
			foreach (KeyValuePair<Vignette, GameplayVignetteMixer.State> keyValuePair in GameplayVignetteMixer.States)
			{
				if (keyValuePair.Key != null)
				{
					GameplayVignetteMixer.Restore(keyValuePair.Key, keyValuePair.Value);
				}
			}
			GameplayVignetteMixer.States.Clear();
			GameplayVignetteMixer._postProcessingEnabled = true;
		}

		// Token: 0x06004D1B RID: 19739 RVA: 0x001DED6C File Offset: 0x001DCF6C
		public static bool BaselineActive(Vignette vignette)
		{
			GameplayVignetteMixer.State state;
			if (!GameplayVignetteMixer.States.TryGetValue(vignette, out state))
			{
				return vignette.active;
			}
			return state.active;
		}

		// Token: 0x06004D1C RID: 19740 RVA: 0x001DED98 File Offset: 0x001DCF98
		public static void SetPostProcessingEnabled(bool enabled)
		{
			GameplayVignetteMixer._postProcessingEnabled = enabled;
			foreach (KeyValuePair<Vignette, GameplayVignetteMixer.State> keyValuePair in GameplayVignetteMixer.States)
			{
				if (keyValuePair.Key != null)
				{
					GameplayVignetteMixer.Apply(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		// Token: 0x06004D1D RID: 19741 RVA: 0x001DEE0C File Offset: 0x001DD00C
		public static void Set(Vignette vignette, Object source, int priority, float intensity, Color? color = null, float? smoothness = null)
		{
			if (vignette == null || source == null)
			{
				return;
			}
			GameplayVignetteMixer.State state;
			if (!GameplayVignetteMixer.States.TryGetValue(vignette, out state))
			{
				state = new GameplayVignetteMixer.State
				{
					active = ScenePostFxQualityService.AuthoredActive(vignette),
					intensity = vignette.intensity.value,
					intensityOverride = vignette.intensity.overrideState,
					color = vignette.color.value,
					colorOverride = vignette.color.overrideState,
					smoothness = vignette.smoothness.value,
					smoothnessOverride = vignette.smoothness.overrideState
				};
				GameplayVignetteMixer.States.Add(vignette, state);
			}
			state.sources[source] = new GameplayVignetteMixer.Contribution
			{
				priority = priority,
				intensity = Mathf.Clamp01(intensity),
				color = (color ?? state.color),
				smoothness = (smoothness ?? state.smoothness)
			};
			GameplayVignetteMixer.Apply(vignette, state);
		}

		// Token: 0x06004D1E RID: 19742 RVA: 0x001DEF38 File Offset: 0x001DD138
		public static void Release(Vignette vignette, Object source)
		{
			GameplayVignetteMixer.State state;
			if (vignette == null || source == null || !GameplayVignetteMixer.States.TryGetValue(vignette, out state))
			{
				return;
			}
			state.sources.Remove(source);
			if (state.sources.Count == 0 || vignette == null)
			{
				if (vignette != null)
				{
					GameplayVignetteMixer.Restore(vignette, state);
				}
				GameplayVignetteMixer.States.Remove(vignette);
				return;
			}
			GameplayVignetteMixer.Apply(vignette, state);
		}

		// Token: 0x06004D1F RID: 19743 RVA: 0x001DEFA4 File Offset: 0x001DD1A4
		private static void Apply(Vignette vignette, GameplayVignetteMixer.State state)
		{
			GameplayVignetteMixer.Contribution contribution = default(GameplayVignetteMixer.Contribution);
			bool flag = false;
			foreach (KeyValuePair<Object, GameplayVignetteMixer.Contribution> keyValuePair in state.sources)
			{
				if (!(keyValuePair.Key == null))
				{
					GameplayVignetteMixer.Contribution value = keyValuePair.Value;
					if (!flag || value.priority > contribution.priority || (value.priority == contribution.priority && value.intensity > contribution.intensity))
					{
						contribution = value;
						flag = true;
					}
				}
			}
			if (!flag)
			{
				GameplayVignetteMixer.Restore(vignette, state);
				return;
			}
			vignette.active = GameplayVignetteMixer._postProcessingEnabled;
			vignette.intensity.Override(contribution.intensity);
			vignette.color.Override(contribution.color);
			vignette.smoothness.Override(contribution.smoothness);
		}

		// Token: 0x06004D20 RID: 19744 RVA: 0x001DF090 File Offset: 0x001DD290
		private static void Restore(Vignette vignette, GameplayVignetteMixer.State state)
		{
			vignette.active = GameplayVignetteMixer._postProcessingEnabled && state.active;
			vignette.intensity.value = state.intensity;
			vignette.intensity.overrideState = state.intensityOverride;
			vignette.color.value = state.color;
			vignette.color.overrideState = state.colorOverride;
			vignette.smoothness.value = state.smoothness;
			vignette.smoothness.overrideState = state.smoothnessOverride;
		}

		// Token: 0x04004E8C RID: 20108
		public const int EmotionPriority = 10;

		// Token: 0x04004E8D RID: 20109
		public const int OrgasmPriority = 20;

		// Token: 0x04004E8E RID: 20110
		public const int WearablePriority = 30;

		// Token: 0x04004E8F RID: 20111
		private static readonly Dictionary<Vignette, GameplayVignetteMixer.State> States = new Dictionary<Vignette, GameplayVignetteMixer.State>(GameplayVignetteMixer.ComponentReferenceComparer.Instance);

		// Token: 0x04004E90 RID: 20112
		private static bool _postProcessingEnabled = true;

		// Token: 0x02000A94 RID: 2708
		private struct Contribution
		{
			// Token: 0x04004E91 RID: 20113
			public int priority;

			// Token: 0x04004E92 RID: 20114
			public float intensity;

			// Token: 0x04004E93 RID: 20115
			public Color color;

			// Token: 0x04004E94 RID: 20116
			public float smoothness;
		}

		// Token: 0x02000A95 RID: 2709
		private sealed class State
		{
			// Token: 0x04004E95 RID: 20117
			public bool active;

			// Token: 0x04004E96 RID: 20118
			public bool intensityOverride;

			// Token: 0x04004E97 RID: 20119
			public bool colorOverride;

			// Token: 0x04004E98 RID: 20120
			public bool smoothnessOverride;

			// Token: 0x04004E99 RID: 20121
			public float intensity;

			// Token: 0x04004E9A RID: 20122
			public float smoothness;

			// Token: 0x04004E9B RID: 20123
			public Color color;

			// Token: 0x04004E9C RID: 20124
			public readonly Dictionary<Object, GameplayVignetteMixer.Contribution> sources = new Dictionary<Object, GameplayVignetteMixer.Contribution>();
		}

		// Token: 0x02000A96 RID: 2710
		internal sealed class ComponentReferenceComparer : IEqualityComparer<VolumeComponent>
		{
			// Token: 0x06004D23 RID: 19747 RVA: 0x001D57BF File Offset: 0x001D39BF
			public bool Equals(VolumeComponent x, VolumeComponent y)
			{
				return x == y;
			}

			// Token: 0x06004D24 RID: 19748 RVA: 0x001D57C5 File Offset: 0x001D39C5
			public int GetHashCode(VolumeComponent component)
			{
				return RuntimeHelpers.GetHashCode(component);
			}

			// Token: 0x04004E9D RID: 20125
			public static readonly GameplayVignetteMixer.ComponentReferenceComparer Instance = new GameplayVignetteMixer.ComponentReferenceComparer();
		}
	}
}
