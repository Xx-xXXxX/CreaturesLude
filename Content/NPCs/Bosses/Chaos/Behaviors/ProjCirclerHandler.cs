using CreaturesLude.Content.NPCs.Bosses.Chaos.Projectiles;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using WackyBag.Structures.Collections;
using WackyBag.Utils;

using WackyBagTr.NPCs.Behaviors;

namespace CreaturesLude.Content.NPCs.Bosses.Chaos.Behaviors
{
	public class ProjCirclerHandler : NPCBehavior<ChaosBoss>
	{
		public ProjCirclerHandler(ChaosBoss modNPC) : base(modNPC)
		{
		}

		public static int ProjCirclerId => ModContent.ProjectileType<ProjCircler>();
		public int ProjTotalCountBase = 96;
		public int ProjTotalCount => (int)(ProjTotalCountBase* ModNPC.POWER);
		public int ProjCount =>(int)(ProjCirclers.Count*ModNPC.POWER);
		public readonly UnorderedList<int> ProjCirclers = [];
		public int LaunchInterval =2;
		public int LaunchTime = 0;
		public int Damage = 250;

		public float CircleRadius=16*192;
		public float CircleRadiusDeviation = 16 * 32;

		/// <summary>
		/// get radius which spread proj in even area
		/// </summary>
		/// <returns></returns>
		public float GetRadius() {
			float MinR = CircleRadius - CircleRadiusDeviation;
			float MaxR = CircleRadius + CircleRadiusDeviation;
			float rand = Main.rand.NextFloat();
			float result = MathF.Sqrt(rand*(MaxR*MaxR+MinR*MinR)+MinR*MinR);
			return result;
			//return CircleRadius + CircleRadiusDeviation * Main.rand.NextFloatDirection();
		}

		public (float radius, float direction, int rotateDir) GetProjMoveState() {
			return (GetRadius(),
						MathF.PI * 2 * Main.rand.NextFloat(),
						Main.rand.NextBool() ? 0 : 1);
		}

		public override void Activate()
		{
			base.Activate();
		}

		public override void AI()
		{
			base.AI();
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{

				LaunchTime += 1;
				if (LaunchTime >= LaunchInterval) {
					LaunchTime = 0;
					if (ProjCount < ProjTotalCount)
					{
						
						var (radius, direction, rotateDir) = GetProjMoveState();
						//int rotateDirection = Main.rand.NextBool() ? 0 : 1;
						int projId = Projectile.NewProjectile(
							NPC.GetSource_FromAI(),
							NPC.Center,
							Vector2.Zero,
							ProjCirclerId,
							Damage, 0.0f, -1,
							radius,
							direction,
							(ProjCircler.AI2Separator.Build(NPC.whoAmI, rotateDir,(int)(Main.rand.NextFloatDirection()*128))));
						ProjCirclers.Add(projId);
						//ProjCount++;
					}

					if (ProjCount > ProjTotalCount) {
						int index=Main.rand.Next(ProjCount);
						int id=ProjCirclers[index];
						ProjCirclers.Remove(index);
						Main.projectile[id].Kill();
						//ProjCount--;
					} 
				}
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			foreach (var projId in ProjCirclers)
			{
				Main.projectile[projId.Value].Kill();
			}
		}


		public void SpreadProj() {

			CircleRadius *= 0.75f;
			CircleRadiusDeviation = CircleRadius*0.75f;
			if (Main.netMode == NetmodeID.MultiplayerClient) return;
			foreach (var item in ProjCirclers)
			{
				var Proj = Main.projectile[item.Value];
				Proj.ai[0] = GetRadius();
			}
		}
	}
}
