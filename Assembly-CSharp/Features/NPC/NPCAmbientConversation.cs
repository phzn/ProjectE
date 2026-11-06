using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000ECC RID: 3788
	public sealed class NPCAmbientConversation
	{
		// Token: 0x17001281 RID: 4737
		// (get) Token: 0x06007173 RID: 29043 RVA: 0x002E1883 File Offset: 0x002DFA83
		public int MemberCount
		{
			get
			{
				return this.members.Count;
			}
		}

		// Token: 0x17001282 RID: 4738
		// (get) Token: 0x06007174 RID: 29044 RVA: 0x002E1890 File Offset: 0x002DFA90
		public NPCAmbientLife Speaker
		{
			get
			{
				return this.speaker;
			}
		}

		// Token: 0x06007175 RID: 29045 RVA: 0x002E1898 File Offset: 0x002DFA98
		public NPCAmbientConversation(int seed)
		{
			this.random = new Random(seed);
		}

		// Token: 0x06007176 RID: 29046 RVA: 0x002E18BF File Offset: 0x002DFABF
		public void Join(NPCAmbientLife member)
		{
			if (!this.members.Contains(member))
			{
				this.members.Add(member);
			}
		}

		// Token: 0x06007177 RID: 29047 RVA: 0x002E18DB File Offset: 0x002DFADB
		public void Leave(NPCAmbientLife member)
		{
			this.members.Remove(member);
			if (this.speaker == member)
			{
				this.previousSpeaker = this.speaker;
				this.speaker = null;
				this.remaining = 0.5f;
			}
		}

		// Token: 0x06007178 RID: 29048 RVA: 0x002E1914 File Offset: 0x002DFB14
		public void Clear()
		{
			while (this.members.Count > 0)
			{
				this.members[this.members.Count - 1].End();
			}
			this.speaker = (this.previousSpeaker = null);
			this.remaining = 0f;
			this.frame = -1;
		}

		// Token: 0x06007179 RID: 29049 RVA: 0x002E1970 File Offset: 0x002DFB70
		public void Tick(float delta)
		{
			if (this.frame == Time.frameCount)
			{
				return;
			}
			this.frame = Time.frameCount;
			for (int i = this.members.Count - 1; i >= 0; i--)
			{
				if (!this.members[i].IsSocialAvailable)
				{
					this.members[i].End();
				}
			}
			if (this.members.Count < 2)
			{
				this.speaker = null;
				this.remaining = 0f;
				return;
			}
			this.remaining -= delta;
			if (this.remaining > 0f)
			{
				return;
			}
			if (this.speaker != null)
			{
				this.previousSpeaker = this.speaker;
				this.speaker = null;
				this.remaining = 0.55f + (float)this.random.NextDouble() * 1.15f;
				return;
			}
			int num = this.random.Next(this.members.Count);
			for (int j = 0; j < this.members.Count; j++)
			{
				NPCAmbientLife npcambientLife = this.members[(num + j) % this.members.Count];
				if (npcambientLife != this.previousSpeaker && this.Partner(npcambientLife) != null)
				{
					this.speaker = npcambientLife;
					break;
				}
			}
			this.variant = this.random.Next(2);
			Vector2 speakingTurn = NPCAmbientLifeSettings.Current.speakingTurn;
			this.remaining = Mathf.Lerp(speakingTurn.x, speakingTurn.y, (float)this.random.NextDouble());
		}

		// Token: 0x0600717A RID: 29050 RVA: 0x002E1AF0 File Offset: 0x002DFCF0
		private NPCAmbientLife Partner(NPCAmbientLife member)
		{
			float conversationDistance = NPCAmbientLifeSettings.Current.conversationDistance;
			NPCAmbientLife npcambientLife = null;
			float num = conversationDistance * conversationDistance;
			foreach (NPCAmbientLife npcambientLife2 in this.members)
			{
				if (npcambientLife2 != member && npcambientLife2.IsSocialAvailable)
				{
					float sqrMagnitude = (npcambientLife2.Root.position - member.Root.position).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						npcambientLife = npcambientLife2;
						num = sqrMagnitude;
					}
				}
			}
			return npcambientLife;
		}

		// Token: 0x0600717B RID: 29051 RVA: 0x002E1B88 File Offset: 0x002DFD88
		public void Present(NPCAmbientLife member, float delta)
		{
			NPCAmbientLife npcambientLife = ((this.speaker != null && this.speaker != member && this.speaker.IsSocialAvailable) ? this.speaker : this.Partner(member));
			float conversationDistance = NPCAmbientLifeSettings.Current.conversationDistance;
			if (npcambientLife != null && (npcambientLife.Root.position - member.Root.position).sqrMagnitude > conversationDistance * conversationDistance)
			{
				npcambientLife = null;
			}
			member.PresentConversation(npcambientLife, this.speaker == member, this.variant, delta);
		}

		// Token: 0x04007454 RID: 29780
		private readonly List<NPCAmbientLife> members = new List<NPCAmbientLife>(4);

		// Token: 0x04007455 RID: 29781
		private readonly Random random;

		// Token: 0x04007456 RID: 29782
		private NPCAmbientLife speaker;

		// Token: 0x04007457 RID: 29783
		private NPCAmbientLife previousSpeaker;

		// Token: 0x04007458 RID: 29784
		private int frame = -1;

		// Token: 0x04007459 RID: 29785
		private int variant;

		// Token: 0x0400745A RID: 29786
		private float remaining;
	}
}
