using System;
using System.Linq;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000D4 RID: 212
	public static class ShaderUtils
	{
		// Token: 0x060006A9 RID: 1705 RVA: 0x00027D18 File Offset: 0x00025F18
		public static string GetShaderPath(ShaderPathID id)
		{
			int num = (int)id;
			int num2 = ShaderUtils.s_ShaderPaths.Length;
			if (num2 > 0 && num >= 0 && num < num2)
			{
				return ShaderUtils.s_ShaderPaths[num];
			}
			Debug.LogError(string.Concat(new string[]
			{
				"Trying to access universal shader path out of bounds: (",
				id.ToString(),
				": ",
				num.ToString(),
				")"
			}));
			return "";
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00027D8C File Offset: 0x00025F8C
		public static ShaderPathID GetEnumFromPath(string path)
		{
			return (ShaderPathID)Array.FindIndex<string>(ShaderUtils.s_ShaderPaths, (string m) => m == path);
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00027DBC File Offset: 0x00025FBC
		public static bool IsLWShader(Shader shader)
		{
			return ShaderUtils.s_ShaderPaths.Contains(shader.name);
		}

		// Token: 0x04000552 RID: 1362
		private static readonly string[] s_ShaderPaths = new string[]
		{
			"Universal Render Pipeline/Lit", "Universal Render Pipeline/Simple Lit", "Universal Render Pipeline/Unlit", "Universal Render Pipeline/Terrain/Lit", "Universal Render Pipeline/Particles/Lit", "Universal Render Pipeline/Particles/Simple Lit", "Universal Render Pipeline/Particles/Unlit", "Universal Render Pipeline/Baked Lit", "Universal Render Pipeline/Nature/SpeedTree7", "Universal Render Pipeline/Nature/SpeedTree7 Billboard",
			"Universal Render Pipeline/Nature/SpeedTree8_PBRLit"
		};
	}
}
