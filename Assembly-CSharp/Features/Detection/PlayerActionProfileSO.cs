using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012CF RID: 4815
	[CreateAssetMenu(fileName = "New Player Action Profile", menuName = "Detection/Player Action Profile")]
	public class PlayerActionProfileSO : ScriptableObject
	{
		// Token: 0x06009522 RID: 38178 RVA: 0x003D38F0 File Offset: 0x003D1AF0
		public PlayerActionEntry GetAction(string actionId)
		{
			if (string.IsNullOrEmpty(actionId))
			{
				return null;
			}
			return this.Actions.Find((PlayerActionEntry a) => a.ActionId == actionId);
		}

		// Token: 0x06009523 RID: 38179 RVA: 0x003D3930 File Offset: 0x003D1B30
		public string[] GetActionIds()
		{
			if (this.Actions == null)
			{
				return new string[0];
			}
			string[] array = new string[this.Actions.Count];
			for (int i = 0; i < this.Actions.Count; i++)
			{
				array[i] = this.Actions[i].ActionId;
			}
			return array;
		}

		// Token: 0x04009A3D RID: 39485
		public List<PlayerActionEntry> Actions;
	}
}
