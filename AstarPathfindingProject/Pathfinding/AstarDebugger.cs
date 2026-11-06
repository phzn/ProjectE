using System;
using System.Text;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200006D RID: 109
	[AddComponentMenu("Pathfinding/Pathfinding Debugger")]
	[ExecuteInEditMode]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/astardebugger.html")]
	public class AstarDebugger : VersionedMonoBehaviour
	{
		// Token: 0x0600041C RID: 1052 RVA: 0x0001345C File Offset: 0x0001165C
		public void Start()
		{
			base.useGUILayout = false;
			this.fpsDrops = new float[this.fpsDropCounterSize];
			this.cam = base.GetComponent<Camera>();
			if (this.cam == null)
			{
				this.cam = Camera.main;
			}
			this.graph = new AstarDebugger.GraphPoint[this.graphBufferSize];
			if (Time.unscaledDeltaTime > 0f)
			{
				for (int i = 0; i < this.fpsDrops.Length; i++)
				{
					this.fpsDrops[i] = 1f / Time.unscaledDeltaTime;
				}
			}
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x000134EC File Offset: 0x000116EC
		public void LateUpdate()
		{
			if (!this.show || (!Application.isPlaying && !this.showInEditor))
			{
				return;
			}
			if (Time.unscaledDeltaTime <= 0.0001f)
			{
				return;
			}
			int num = GC.CollectionCount(0);
			if (this.lastCollectNum != (float)num)
			{
				this.lastCollectNum = (float)num;
				this.delta = Time.realtimeSinceStartup - this.lastCollect;
				this.lastCollect = Time.realtimeSinceStartup;
				this.lastDeltaTime = Time.unscaledDeltaTime;
				this.collectAlloc = this.allocMem;
			}
			this.allocMem = (int)GC.GetTotalMemory(false);
			bool flag = this.allocMem < this.peakAlloc;
			this.peakAlloc = ((!flag) ? this.allocMem : this.peakAlloc);
			if (Time.realtimeSinceStartup - this.lastAllocSet > 0.3f || !Application.isPlaying)
			{
				int num2 = this.allocMem - this.lastAllocMemory;
				this.lastAllocMemory = this.allocMem;
				this.lastAllocSet = Time.realtimeSinceStartup;
				this.delayedDeltaTime = Time.unscaledDeltaTime;
				if (num2 >= 0)
				{
					this.allocRate = num2;
				}
			}
			if (Application.isPlaying)
			{
				this.fpsDrops[Time.frameCount % this.fpsDrops.Length] = ((Time.unscaledDeltaTime > 1E-05f) ? (1f / Time.unscaledDeltaTime) : 0f);
				int num3 = Time.frameCount % this.graph.Length;
				this.graph[num3].fps = ((Time.unscaledDeltaTime < 1E-05f) ? (1f / Time.unscaledDeltaTime) : 0f);
				this.graph[num3].collectEvent = flag;
				this.graph[num3].memory = (float)this.allocMem;
			}
			if (Application.isPlaying && this.cam != null && this.showGraph)
			{
				this.graphWidth = (float)this.cam.pixelWidth * 0.8f;
				float num4 = float.PositiveInfinity;
				float num5 = 0f;
				float num6 = float.PositiveInfinity;
				float num7 = 0f;
				for (int i = 0; i < this.graph.Length; i++)
				{
					num4 = Mathf.Min(this.graph[i].memory, num4);
					num5 = Mathf.Max(this.graph[i].memory, num5);
					num6 = Mathf.Min(this.graph[i].fps, num6);
					num7 = Mathf.Max(this.graph[i].fps, num7);
				}
				int num8 = Time.frameCount % this.graph.Length;
				Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(((float)this.cam.pixelWidth - this.graphWidth) / 2f, this.graphOffset, 1f), Quaternion.identity, new Vector3(this.graphWidth, this.graphHeight, 1f));
				for (int j = 0; j < this.graph.Length - 1; j++)
				{
					if (j != num8)
					{
						this.DrawGraphLine(j, matrix4x, (float)j / (float)this.graph.Length, (float)(j + 1) / (float)this.graph.Length, Mathf.InverseLerp(num4, num5, this.graph[j].memory), Mathf.InverseLerp(num4, num5, this.graph[j + 1].memory), Color.blue);
						this.DrawGraphLine(j, matrix4x, (float)j / (float)this.graph.Length, (float)(j + 1) / (float)this.graph.Length, Mathf.InverseLerp(num6, num7, this.graph[j].fps), Mathf.InverseLerp(num6, num7, this.graph[j + 1].fps), Color.green);
					}
				}
			}
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x000138BE File Offset: 0x00011ABE
		private void DrawGraphLine(int index, Matrix4x4 m, float x1, float x2, float y1, float y2, Color color)
		{
			Debug.DrawLine(this.cam.ScreenToWorldPoint(m.MultiplyPoint3x4(new Vector3(x1, y1))), this.cam.ScreenToWorldPoint(m.MultiplyPoint3x4(new Vector3(x2, y2))), color);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x000138FC File Offset: 0x00011AFC
		public void OnGUI()
		{
			if (!this.show || (!Application.isPlaying && !this.showInEditor))
			{
				return;
			}
			if (this.style == null)
			{
				this.style = new GUIStyle();
				this.style.normal.textColor = Color.white;
				this.style.padding = new RectOffset(5, 5, 5, 5);
			}
			if (Time.realtimeSinceStartup - this.lastUpdate > 0.5f || this.cachedText == null || !Application.isPlaying)
			{
				this.lastUpdate = Time.realtimeSinceStartup;
				this.boxRect = new Rect(5f, (float)this.yOffset, 310f, 40f);
				this.text.Length = 0;
				this.text.AppendLine("A* Pathfinding Project Debugger");
				this.text.Append("A* Version: ").Append(AstarPath.Version.ToString());
				if (this.showMemProfile)
				{
					this.boxRect.height = this.boxRect.height + 200f;
					this.text.AppendLine();
					this.text.AppendLine();
					this.text.Append("Currently allocated".PadRight(25));
					this.text.Append(((float)this.allocMem / 1000000f).ToString("0.0 MB"));
					this.text.AppendLine();
					this.text.Append("Peak allocated".PadRight(25));
					this.text.Append(((float)this.peakAlloc / 1000000f).ToString("0.0 MB")).AppendLine();
					this.text.Append("Last collect peak".PadRight(25));
					this.text.Append(((float)this.collectAlloc / 1000000f).ToString("0.0 MB")).AppendLine();
					this.text.Append("Allocation rate".PadRight(25));
					this.text.Append(((float)this.allocRate / 1000000f).ToString("0.0 MB")).AppendLine();
					this.text.Append("Collection frequency".PadRight(25));
					this.text.Append(this.delta.ToString("0.00"));
					this.text.Append("s\n");
					this.text.Append("Last collect fps".PadRight(25));
					this.text.Append((1f / this.lastDeltaTime).ToString("0.0 fps"));
					this.text.Append(" (");
					this.text.Append(this.lastDeltaTime.ToString("0.000 s"));
					this.text.Append(")");
				}
				if (this.showFPS)
				{
					this.text.AppendLine();
					this.text.AppendLine();
					float num = ((this.delayedDeltaTime > 1E-05f) ? (1f / this.delayedDeltaTime) : 0f);
					this.text.Append("FPS".PadRight(25)).Append(num.ToString("0.0 fps"));
					float num2 = float.PositiveInfinity;
					for (int i = 0; i < this.fpsDrops.Length; i++)
					{
						if (this.fpsDrops[i] < num2)
						{
							num2 = this.fpsDrops[i];
						}
					}
					this.text.AppendLine();
					this.text.Append(("Lowest fps (last " + this.fpsDrops.Length.ToString() + ")").PadRight(25)).Append(num2.ToString("0.0"));
				}
				if (this.showPathProfile)
				{
					Object active = AstarPath.active;
					this.text.AppendLine();
					if (active == null)
					{
						this.text.Append("\nNo AstarPath Object In The Scene");
					}
					else
					{
						if (ListPool<Vector3>.GetSize() > this.maxVecPool)
						{
							this.maxVecPool = ListPool<Vector3>.GetSize();
						}
						if (ListPool<GraphNode>.GetSize() > this.maxNodePool)
						{
							this.maxNodePool = ListPool<GraphNode>.GetSize();
						}
						this.text.Append("\nPool Sizes (size/total created)");
						for (int j = 0; j < this.debugTypes.Length; j++)
						{
							this.debugTypes[j].Print(this.text);
						}
					}
				}
				this.cachedText = this.text.ToString();
			}
			if (this.font != null)
			{
				this.style.font = this.font;
				this.style.fontSize = this.fontSize;
			}
			this.boxRect.height = this.style.CalcHeight(new GUIContent(this.cachedText), this.boxRect.width);
			GUI.Box(this.boxRect, "");
			GUI.Label(this.boxRect, this.cachedText, this.style);
			if (this.showGraph)
			{
				float num3 = float.PositiveInfinity;
				float num4 = 0f;
				float num5 = float.PositiveInfinity;
				float num6 = 0f;
				for (int k = 0; k < this.graph.Length; k++)
				{
					num3 = Mathf.Min(this.graph[k].memory, num3);
					num4 = Mathf.Max(this.graph[k].memory, num4);
					num5 = Mathf.Min(this.graph[k].fps, num5);
					num6 = Mathf.Max(this.graph[k].fps, num6);
				}
				GUI.color = Color.blue;
				float num7 = (float)Mathf.RoundToInt(num4 / 100000f);
				GUI.Label(new Rect(5f, (float)Screen.height - AstarMath.MapTo(num3, num4, 0f + this.graphOffset, this.graphHeight + this.graphOffset, num7 * 1000f * 100f) - 10f, 100f, 20f), (num7 / 10f).ToString("0.0 MB"));
				num7 = Mathf.Round(num3 / 100000f);
				GUI.Label(new Rect(5f, (float)Screen.height - AstarMath.MapTo(num3, num4, 0f + this.graphOffset, this.graphHeight + this.graphOffset, num7 * 1000f * 100f) - 10f, 100f, 20f), (num7 / 10f).ToString("0.0 MB"));
				GUI.color = Color.green;
				num7 = Mathf.Round(num6);
				GUI.Label(new Rect(55f, (float)Screen.height - AstarMath.MapTo(num5, num6, 0f + this.graphOffset, this.graphHeight + this.graphOffset, num7) - 10f, 100f, 20f), num7.ToString("0 FPS"));
				num7 = Mathf.Round(num5);
				GUI.Label(new Rect(55f, (float)Screen.height - AstarMath.MapTo(num5, num6, 0f + this.graphOffset, this.graphHeight + this.graphOffset, num7) - 10f, 100f, 20f), num7.ToString("0 FPS"));
			}
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00014090 File Offset: 0x00012290
		public AstarDebugger()
		{
			AstarDebugger.PathTypeDebug[] array = new AstarDebugger.PathTypeDebug[7];
			array[0] = new AstarDebugger.PathTypeDebug("ABPath", () => PathPool.GetSize(typeof(ABPath)), () => PathPool.GetTotalCreated(typeof(ABPath)));
			array[1] = new AstarDebugger.PathTypeDebug("MultiTargetPath", () => PathPool.GetSize(typeof(MultiTargetPath)), () => PathPool.GetTotalCreated(typeof(MultiTargetPath)));
			array[2] = new AstarDebugger.PathTypeDebug("RandomPath", () => PathPool.GetSize(typeof(RandomPath)), () => PathPool.GetTotalCreated(typeof(RandomPath)));
			array[3] = new AstarDebugger.PathTypeDebug("FleePath", () => PathPool.GetSize(typeof(FleePath)), () => PathPool.GetTotalCreated(typeof(FleePath)));
			array[4] = new AstarDebugger.PathTypeDebug("ConstantPath", () => PathPool.GetSize(typeof(ConstantPath)), () => PathPool.GetTotalCreated(typeof(ConstantPath)));
			array[5] = new AstarDebugger.PathTypeDebug("FloodPath", () => PathPool.GetSize(typeof(FloodPath)), () => PathPool.GetTotalCreated(typeof(FloodPath)));
			array[6] = new AstarDebugger.PathTypeDebug("FloodPathTracer", () => PathPool.GetSize(typeof(FloodPathTracer)), () => PathPool.GetTotalCreated(typeof(FloodPathTracer)));
			this.debugTypes = array;
			base..ctor();
		}

		// Token: 0x0400025B RID: 603
		public int yOffset = 5;

		// Token: 0x0400025C RID: 604
		public bool show = true;

		// Token: 0x0400025D RID: 605
		public bool showInEditor;

		// Token: 0x0400025E RID: 606
		public bool showFPS;

		// Token: 0x0400025F RID: 607
		public bool showPathProfile;

		// Token: 0x04000260 RID: 608
		public bool showMemProfile;

		// Token: 0x04000261 RID: 609
		public bool showGraph;

		// Token: 0x04000262 RID: 610
		public int graphBufferSize = 200;

		// Token: 0x04000263 RID: 611
		public Font font;

		// Token: 0x04000264 RID: 612
		public int fontSize = 12;

		// Token: 0x04000265 RID: 613
		private StringBuilder text = new StringBuilder();

		// Token: 0x04000266 RID: 614
		private string cachedText;

		// Token: 0x04000267 RID: 615
		private float lastUpdate = -999f;

		// Token: 0x04000268 RID: 616
		private AstarDebugger.GraphPoint[] graph;

		// Token: 0x04000269 RID: 617
		private float delayedDeltaTime = 1f;

		// Token: 0x0400026A RID: 618
		private float lastCollect;

		// Token: 0x0400026B RID: 619
		private float lastCollectNum;

		// Token: 0x0400026C RID: 620
		private float delta;

		// Token: 0x0400026D RID: 621
		private float lastDeltaTime;

		// Token: 0x0400026E RID: 622
		private int allocRate;

		// Token: 0x0400026F RID: 623
		private int lastAllocMemory;

		// Token: 0x04000270 RID: 624
		private float lastAllocSet = -9999f;

		// Token: 0x04000271 RID: 625
		private int allocMem;

		// Token: 0x04000272 RID: 626
		private int collectAlloc;

		// Token: 0x04000273 RID: 627
		private int peakAlloc;

		// Token: 0x04000274 RID: 628
		private int fpsDropCounterSize = 200;

		// Token: 0x04000275 RID: 629
		private float[] fpsDrops;

		// Token: 0x04000276 RID: 630
		private Rect boxRect;

		// Token: 0x04000277 RID: 631
		private GUIStyle style;

		// Token: 0x04000278 RID: 632
		private Camera cam;

		// Token: 0x04000279 RID: 633
		private float graphWidth = 100f;

		// Token: 0x0400027A RID: 634
		private float graphHeight = 100f;

		// Token: 0x0400027B RID: 635
		private float graphOffset = 50f;

		// Token: 0x0400027C RID: 636
		private int maxVecPool;

		// Token: 0x0400027D RID: 637
		private int maxNodePool;

		// Token: 0x0400027E RID: 638
		private AstarDebugger.PathTypeDebug[] debugTypes;

		// Token: 0x0200006E RID: 110
		private struct GraphPoint
		{
			// Token: 0x0400027F RID: 639
			public float fps;

			// Token: 0x04000280 RID: 640
			public float memory;

			// Token: 0x04000281 RID: 641
			public bool collectEvent;
		}

		// Token: 0x0200006F RID: 111
		private struct PathTypeDebug
		{
			// Token: 0x06000421 RID: 1057 RVA: 0x00014351 File Offset: 0x00012551
			public PathTypeDebug(string name, Func<int> getSize, Func<int> getTotalCreated)
			{
				this.name = name;
				this.getSize = getSize;
				this.getTotalCreated = getTotalCreated;
			}

			// Token: 0x06000422 RID: 1058 RVA: 0x00014368 File Offset: 0x00012568
			public void Print(StringBuilder text)
			{
				int num = this.getTotalCreated();
				if (num > 0)
				{
					text.Append("\n").Append(("  " + this.name).PadRight(25)).Append(this.getSize())
						.Append("/")
						.Append(num);
				}
			}

			// Token: 0x04000282 RID: 642
			private string name;

			// Token: 0x04000283 RID: 643
			private Func<int> getSize;

			// Token: 0x04000284 RID: 644
			private Func<int> getTotalCreated;
		}
	}
}
