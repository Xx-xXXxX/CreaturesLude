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
	public class SpamBALLDust:ModDust
	{
		
		public override string Texture => ChaosBoss.BALLTexturePath;
		public override void OnSpawn(Dust dust)
		{
			base.OnSpawn(dust);
			dust.noGravity = true;
			dust.noLight = true; 
			dust.color = Color.White;
		}
		public override bool Update(Dust dust)
		{
			//Main.NewText($"here:{dust.fadeIn}");
			dust.fadeIn-=1/60f;
			if (dust.fadeIn <=0) dust.active = false;
			float d = WackyBagTr.Utils.SmoothUp_0_Inf_to_0_1(dust.fadeIn/8);
			dust.alpha = (int)(255 * (1-d));
			if (Main.rand.NextFloat()<d) {
				var newdust=Dust.NewDustPerfect(dust.position, ModContent.DustType<SpamBALLDust>());
				newdust.fadeIn = dust.fadeIn*0.8f;
				dust.fadeIn -= 0.2f;
				dust.velocity -= newdust.velocity;
			}
			dust.rotation=Main.rand.NextFloat()*MathF.Tau;
			dust.position += dust.velocity;
			return false;
		}
		public override bool PreDraw(Dust dust)
		{
			ChaosBoss.DrawBALL(dust.position, dust.fadeIn*8, dust.color with { A= (byte)(255 -dust.alpha) },dust.rotation);
			return false;
		}
	}
}
