using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

using WackyBagTr;
using WackyBagTr.NPCs.Behaviors;

namespace CreaturesLude.Content.NPCs.Bosses.Chaos.Behaviors
{
	public class ChooseTarget : NPCBehavior<ChaosBoss>
	{
		public ChooseTarget(ChaosBoss modNPC) : base(modNPC)
		{

		}

		public override void AI()
		{
			base.AI();
			Player? target = null;
			foreach (var plr in Main.player)
			{
				if (plr.active&&!plr.dead&&!plr.ghost) {
					if (target==null || plr.statLife>target.statLife) {
						target = plr;
					}
				}
			}
			if (target != null)
			{
				NPC.target = target.whoAmI;
			}
			else {
				ModNPC.NPCBehavior.Warp()?.Pause();
				ModNPC.NPCBehavior.Dispose();
				NPC.active = false;
			}
		}
	}
}
