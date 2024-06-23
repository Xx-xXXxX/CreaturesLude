using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace CreaturesLude.Content.NPCs.Bosses.Chaos
{
	public class NormalBALLDust:ModDust
	{
		public static void DrawDustLine(Vector2 From, Vector2 Offset, int Count,int time) {
			Vector2 dirNormal = Offset / Offset.Length();
			//Offset-=dirNormal*time*velSpeed/4;
			for (int i = 0; i < Count; i++)
			{
				var rand = Main.rand.NextFloat();
				Vector2 Pos=From+ Offset*rand;
				var dust = Dust.NewDustPerfect(Pos, ModContent.DustType<NormalBALLDust>(), dirNormal * 0, 0, Color.White, 1);
				dust.fadeIn = time;
				//dust.active = true;
			}
		}

		public static float FadeBase => 60;

		public float Effect(Dust dust) {
			if (dust.fadeIn > FadeBase) return 1;
			return dust.fadeIn/FadeBase;
		}

		public override string Texture => ChaosBoss.BALLTexturePath;
		public override void OnSpawn(Dust dust)
		{
			base.OnSpawn(dust);
			dust.noGravity = true;
			//dust.noLight = true;
			dust.color = Color.White;
		}
		public override bool Update(Dust dust)
		{
			//Dust.NewDust(dust.position, 0, 0, ModContent.DustType<BALLDust>(), Main.rand.NextFloatDirection() * 4, Main.rand.NextFloatDirection() * 4);
			dust.fadeIn--;
			if (dust.fadeIn <=0) dust.active = false;
			dust.rotation = Main.rand.NextFloat() * MathF.Tau;
			dust.position += dust.velocity;
			dust.velocity += Main.rand.NextVector2Circular(1f, 1f);
			//WackyBagTr.Utilties.Wacky.DrawLineFrame(dust.position,dust.velocity*60,8,Color.Green);
			return false;
		}
		

		public override bool PreDraw(Dust dust)
		{
			var effect = Effect(dust);
			ChaosBoss.DrawBALL(dust.position, 16* effect, new Color(effect, effect, effect, effect), dust.rotation);
			return false;
		}
	}
}
