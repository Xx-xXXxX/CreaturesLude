using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

using WackyBagTr.NPCs.Behaviors;

namespace CreaturesLude.Content.NPCs.Bosses.Chaos.Behaviors.Kids
{
    public class Move : NPCBehavior<KidChaosBoss>
    {
        public Move(KidChaosBoss modNPC) : base(modNPC)
        {
            //WackyBag.Utils.Calculate.StdGaussian();
        }

		public float StraitMoveEffect
		{
            get;set;
		}
		public float RotateEffect => 1 - StraitMoveEffect;


		public float AccelerateBase =>ModNPC.Speed*(1f-Damping);
		public float RotateBase => 0.10f;
		public float Damping => ModNPC.Damping;
		public float Accelerate =>   AccelerateBase * StraitMoveEffect;
		public float Rotate =>  RotateBase * RotateEffect;


		public override void AI()
        {
            base.AI();

            var target = ModNPC.Target;
            if (target.Invalid)
            {
                return;
            }
            var tarvel = ModNPC.TargetVel;
			var v = WackyBagTr.Utilties.Calculates.AngleCos(tarvel, NPC.velocity);
			var targetEffect = (NPC.velocity != Vector2.Zero && tarvel != Vector2.Zero) ? (  v*v*v/ 2 + 0.5f) * 0.8f + 0.1f : 0.1f;
			StraitMoveEffect = StraitMoveEffect * 0.90f + targetEffect * 0.10f;

			NPC.velocity += NPC.rotation.ToRotationVector2() * Accelerate;

			NPC.rotation = Terraria.Utils.AngleTowards(NPC.rotation, tarvel.ToRotation(), Rotate);

			//WackyBagTr.Utilties.Wacky.DrawTextFrame(NPC.BottomRight,$"MoveEffect:{StraitMoveEffect},tar:{targetEffect}");
			//NPC.velocity = NPC.velocity * 0.95f + ModNPC.TargetVel * 0.05f;

		}
	}
}
