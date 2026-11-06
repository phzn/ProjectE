using System;
using UnityEngine;

namespace ProjectExhibitionist.BenchSeating
{
	// Token: 0x02000B25 RID: 2853
	[CreateAssetMenu(menuName = "Project Exhibitionist/Bench animations", fileName = "BenchAnimations")]
	public sealed class BenchAnimationSet : ScriptableObject
	{
		// Token: 0x060050F4 RID: 20724 RVA: 0x001F5C9F File Offset: 0x001F3E9F
		public bool IsValid(bool female, bool npcHands = false)
		{
			return this.GetSitDown(female, npcHands) != null && this.GetIdle(female, npcHands) != null && this.GetStandUp(female, npcHands) != null;
		}

		// Token: 0x060050F5 RID: 20725 RVA: 0x001F5CD1 File Offset: 0x001F3ED1
		public AnimationClip GetSitDown(bool female, bool npcHands = false)
		{
			if (!npcHands)
			{
				if (!female)
				{
					return this.maleSitDown;
				}
				return this.femaleSitDown;
			}
			else
			{
				if (!female)
				{
					return this.npcMaleSitDown;
				}
				return this.npcFemaleSitDown;
			}
		}

		// Token: 0x060050F6 RID: 20726 RVA: 0x001F5CF7 File Offset: 0x001F3EF7
		public AnimationClip GetIdle(bool female, bool npcHands = false)
		{
			if (!npcHands)
			{
				if (!female)
				{
					return this.maleIdle;
				}
				return this.femaleIdle;
			}
			else
			{
				if (!female)
				{
					return this.npcMaleIdle;
				}
				return this.npcFemaleIdle;
			}
		}

		// Token: 0x060050F7 RID: 20727 RVA: 0x001F5D1D File Offset: 0x001F3F1D
		public AnimationClip GetStandUp(bool female, bool npcHands = false)
		{
			if (!npcHands)
			{
				if (!female)
				{
					return this.maleStandUp;
				}
				return this.femaleStandUp;
			}
			else
			{
				if (!female)
				{
					return this.npcMaleStandUp;
				}
				return this.npcFemaleStandUp;
			}
		}

		// Token: 0x060050F8 RID: 20728 RVA: 0x001F5D43 File Offset: 0x001F3F43
		public BenchMotionTrack GetEntrance(bool female)
		{
			if (!female)
			{
				return this.maleEntrance;
			}
			return this.femaleEntrance;
		}

		// Token: 0x060050F9 RID: 20729 RVA: 0x001F5D55 File Offset: 0x001F3F55
		public BenchMotionTrack GetExit(bool female)
		{
			if (!female)
			{
				return this.maleExit;
			}
			return this.femaleExit;
		}

		// Token: 0x060050FA RID: 20730 RVA: 0x001F5D68 File Offset: 0x001F3F68
		public bool HasAuthoredMotion(bool female)
		{
			return this.motionVersion >= 2 && this.GetEntrance(female) != null && this.GetExit(female) != null && this.GetEntrance(female).Position(0f).sqrMagnitude > 0.01f && this.GetExit(female).Position(this.GetExit(female).duration).sqrMagnitude > 0.01f;
		}

		// Token: 0x0400518E RID: 20878
		public AnimationClip femaleSitDown;

		// Token: 0x0400518F RID: 20879
		public AnimationClip femaleIdle;

		// Token: 0x04005190 RID: 20880
		public AnimationClip femaleStandUp;

		// Token: 0x04005191 RID: 20881
		public AnimationClip maleSitDown;

		// Token: 0x04005192 RID: 20882
		public AnimationClip maleIdle;

		// Token: 0x04005193 RID: 20883
		public AnimationClip maleStandUp;

		// Token: 0x04005194 RID: 20884
		[Header("NPC hands (same body motion, calibrated for the NPC avatar)")]
		public AnimationClip npcFemaleSitDown;

		// Token: 0x04005195 RID: 20885
		public AnimationClip npcFemaleIdle;

		// Token: 0x04005196 RID: 20886
		public AnimationClip npcFemaleStandUp;

		// Token: 0x04005197 RID: 20887
		public AnimationClip npcMaleSitDown;

		// Token: 0x04005198 RID: 20888
		public AnimationClip npcMaleIdle;

		// Token: 0x04005199 RID: 20889
		public AnimationClip npcMaleStandUp;

		// Token: 0x0400519A RID: 20890
		public BenchMotionTrack femaleEntrance = new BenchMotionTrack();

		// Token: 0x0400519B RID: 20891
		public BenchMotionTrack femaleExit = new BenchMotionTrack();

		// Token: 0x0400519C RID: 20892
		public BenchMotionTrack maleEntrance = new BenchMotionTrack();

		// Token: 0x0400519D RID: 20893
		public BenchMotionTrack maleExit = new BenchMotionTrack();

		// Token: 0x0400519E RID: 20894
		[HideInInspector]
		public int motionVersion;

		// Token: 0x0400519F RID: 20895
		[Min(0f)]
		public float blendDuration = 0.15f;
	}
}
