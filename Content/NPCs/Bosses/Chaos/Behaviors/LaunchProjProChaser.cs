using CreaturesLude.Content.NPCs.Bosses.Chaos.Projectiles;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using WackyBag.Utils;

using WackyBagTr.NPCs.Behaviors;

namespace CreaturesLude.Content.NPCs.Bosses.Chaos.Behaviors
{
	public class LaunchProjProChaser : NPCBehavior<ChaosBoss>
	{
		public float GivenPower=1;

		public float Power;
		public int IntervalBase = 6 * 60;
		public float DamageBase = 250;
		public int Interval => IntervalBase;//(int)(IntervalBase / (Power * DSEPOWER.EmitPOWER));
		public float Damage => DamageBase * Power*DSEPOWER.DamagePOWER;

		public DamageSpeedEmitPOWER DSEPOWER { get; }

		public int clock = 0;

		public LaunchProjProChaser(ChaosBoss modNPC,DamageSpeedEmitPOWER DSEPOWER) : base(modNPC)
		{
			this.DSEPOWER = DSEPOWER;
		}

		public override void AI()
		{
			base.AI();
			if (Main.netMode==NetmodeID.MultiplayerClient) { return; }
			//clock++;
			clock += WackyBagTr.Utils.RandIntoInt(Power * DSEPOWER.DamagePOWER);
			//Main.NewText($"{clock}/{Interval}");

			if (clock>=Interval){
				Projectile.NewProjectile(NPC.GetSource_FromAI(),NPC.Center,Main.rand.NextVector2Circular(1,1),ModContent.ProjectileType<ProjProChaser>(),(int)Damage,0,-1,1, Power * DSEPOWER.SpeedPOWER,
					ProjProChaser.AI2Separater.Build(NPC.whoAmI,Main.rand.Next(256)));
				clock = 0;
			}
		}
	}
}
