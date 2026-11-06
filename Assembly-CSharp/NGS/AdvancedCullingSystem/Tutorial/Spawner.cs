using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Tutorial
{
	// Token: 0x02000431 RID: 1073
	public class Spawner : MonoBehaviour
	{
		// Token: 0x06001B15 RID: 6933 RVA: 0x000A8E50 File Offset: 0x000A7050
		private void Update()
		{
			this._timer += Time.deltaTime;
			if (this._timer > this._delay)
			{
				this.Spawn();
				this._timer = 0f;
				this._counter++;
				if (this._counter > this._maxInstances)
				{
					base.enabled = false;
				}
			}
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x000A8EB4 File Offset: 0x000A70B4
		private void Spawn()
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this._prefab);
			gameObject.transform.position = base.transform.position;
			gameObject.transform.rotation = Random.rotation;
			gameObject.transform.localScale = Vector3.one * Random.Range(1f, 2.5f);
		}

		// Token: 0x04001BEB RID: 7147
		[SerializeField]
		private GameObject _prefab;

		// Token: 0x04001BEC RID: 7148
		[SerializeField]
		private int _maxInstances = 2000;

		// Token: 0x04001BED RID: 7149
		[SerializeField]
		private float _delay = 0.02f;

		// Token: 0x04001BEE RID: 7150
		private int _counter;

		// Token: 0x04001BEF RID: 7151
		private float _timer;
	}
}
