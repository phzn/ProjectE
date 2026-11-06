using System;
using System.Collections.Generic;

namespace ProjectExhibitionist.Interactions
{
	// Token: 0x0200090B RID: 2315
	public sealed class InteractionActionBuffer
	{
		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06003EC8 RID: 16072 RVA: 0x001753CC File Offset: 0x001735CC
		public List<UsableAction> Actions
		{
			get
			{
				return this._actions;
			}
		}

		// Token: 0x06003EC9 RID: 16073 RVA: 0x001753D4 File Offset: 0x001735D4
		public void Begin()
		{
			this._writeIndex = 0;
			this._changed = false;
		}

		// Token: 0x06003ECA RID: 16074 RVA: 0x001753E4 File Offset: 0x001735E4
		public void Add(string key, string label, string description = "")
		{
			if (this._writeIndex == this._pool.Count)
			{
				this._pool.Add(new UsableAction());
			}
			UsableAction usableAction = this._pool[this._writeIndex];
			if (!string.Equals(usableAction.keyDisplayLabel, key, StringComparison.Ordinal) || !string.Equals(usableAction.actionName, label, StringComparison.Ordinal) || !string.Equals(usableAction.description, description, StringComparison.Ordinal))
			{
				this._changed = true;
			}
			usableAction.keyDisplayLabel = key;
			usableAction.actionName = label;
			usableAction.description = description;
			if (this._writeIndex == this._actions.Count)
			{
				this._actions.Add(usableAction);
				this._changed = true;
			}
			this._writeIndex++;
		}

		// Token: 0x06003ECB RID: 16075 RVA: 0x001754A4 File Offset: 0x001736A4
		public bool End()
		{
			if (this._writeIndex < this._actions.Count)
			{
				this._actions.RemoveRange(this._writeIndex, this._actions.Count - this._writeIndex);
				this._changed = true;
			}
			return this._changed;
		}

		// Token: 0x04003ED3 RID: 16083
		private readonly List<UsableAction> _pool = new List<UsableAction>(8);

		// Token: 0x04003ED4 RID: 16084
		private readonly List<UsableAction> _actions = new List<UsableAction>(8);

		// Token: 0x04003ED5 RID: 16085
		private int _writeIndex;

		// Token: 0x04003ED6 RID: 16086
		private bool _changed;
	}
}
