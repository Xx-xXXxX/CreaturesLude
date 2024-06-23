using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WackyBagTr.NPCs.Behaviors;

namespace CreaturesLude.Content.NPCs.Bosses.Chaos.Behaviors.Kids
{
	public class SetAttackVel : NPCBehavior<KidChaosBoss>
	{
		public SetAttackVel(KidChaosBoss modNPC) : base(modNPC)
		{
		}

		public override void AI()
		{
			base.AI();
			var tar = ModNPC.Target;
			if (tar.Invalid) ModNPC.TargetVel= Vector2.Zero;
			else ModNPC.TargetVel = WackyBagTr.Utilties.Calculates.PredictWithVelDirect(tar.Center - NPC.Center + ModNPC.OffsetPos, tar.Velocity, ModNPC.Speed);
		}
	}
}
