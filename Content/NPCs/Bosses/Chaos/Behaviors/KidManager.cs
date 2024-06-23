using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using WackyBagTr.NPCs.Behaviors;

namespace CreaturesLude.Content.NPCs.Bosses.Chaos.Behaviors
{
	public class KidManager : NPCBehavior<ChaosBoss>
	{
		public KidManager(ChaosBoss modNPC) : base(modNPC)
		{
		}
		public static int SpamCount => 8;
		public static float HpLevel => 0.05f;
		public static float HpDrain => 0.03f;
		public static float HpDrainSpeed => 0.01f;
		public List<int> Kids = [];
		public bool Making = false;

		public int HealRadius => 16 * 64;
		public float HealSpeedPercentage => 0.005f / 60;
		public int HealSpeed => (int)(NPC.life * HealSpeedPercentage);
		

		public override void AI()
		{
			base.AI();

			if (!Making)
			{
				int Heal = HealSpeed;

				foreach (var item in Kids)
				{
					NPC npc = Main.npc[item];
					if (npc.life < npc.lifeMax - Heal && (npc.Center - NPC.Center).Length() < HealRadius)
					{
						//var oldLife = npc.life;
						npc.life += Heal;

						NPC.life -= Heal;
						//var info = new NPC.HitInfo();
						//info.Damage = -HealSpeed;
						//info.Crit = false;
						//info.HitDirection = 0;
						//info.DamageType = DamageClass.Default;

						//npc.StrikeNPC(info);
						//info.Damage = HealSpeed;
						//NPC.StrikeNPC(info);
						NormalBALLDust.DrawDustLine(NPC.Center, npc.Center-NPC.Center, Heal / (1*1024), 30);
						if (NPC.life <= 0) {
							NPC.life = 1;
							if(Main.netMode!=NetmodeID.MultiplayerClient)
								NPC.StrikeInstantKill();
						}
					}
				}

				if (Kids.Count< SpamCount && 1 - ((float)NPC.life /NPC.lifeMax)>Kids.Count*(HpLevel+HpDrain)+HpLevel)
				{
					Making = true;
					NPC.dontTakeDamage = true;
					NPC.life = (int)(NPC.lifeMax*(1 - Kids.Count * (HpLevel + HpDrain) - HpLevel));
				}
			}

			if (Making) {
				if (1 - ((float)NPC.life / NPC.lifeMax) > Kids.Count * (HpLevel + HpDrain) + HpLevel + HpDrain)
				{
					Making = false;
					int id = Terraria.NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<KidChaosBoss>(), 0, 
						0, 
						0, 
						0, 
						( KidChaosBoss.AI3Separator.Build(NPC.whoAmI)), NPC.target);
					Kids.Add(id);
					NPC.dontTakeDamage = false;
				}
				else {
					NPC.life -= (int)(NPC.lifeMax * HpDrainSpeed / 60);
				}
			}
			
		}

		
	}
}
