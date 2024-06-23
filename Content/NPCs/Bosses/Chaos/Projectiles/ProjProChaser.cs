using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

using WackyBag.Structures;
using WackyBag.Utils;

using WackyBagTr.Structures;

namespace CreaturesLude.Content.NPCs.Bosses.Chaos.Projectiles
{
	public class ProjProChaser:BALLProj
	{

		public override Color Color => new Color(255,127,127,255);
		public static readonly FloatAsIntSeparator AI2Separater = new((0,256),(0,256));
		//public int AI2Int => BitOperate.ToInt(Projectile.ai[2]);
		public float ExtraPredict {
			get => AI2Separater.Get(Projectile.ai[2],1)/128f;
		}

		public float StraitMoveEffect {
			get => Projectile.ai[0];
			set=>Projectile.ai[0] = value;
		}
		public float RotateEffect => 1 - StraitMoveEffect;
		public override int Radius => 32;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.timeLeft = 60*30;
			Projectile.penetrate = -1;
		}

		public float SpeedEffect => Projectile.ai[1];

		public int OwnerId =>AI2Separater.Get(Projectile.ai[2],0);
		public NPC Owner=>Main.npc[OwnerId];
		public Vector2 TargetVelocity {
			get {
				if (Owner.active) {
					Vector2? targetPos=null;
					Vector2 targetVel=default;
					if (Owner.HasPlayerTarget)
					{
						var playerId = Owner.target;
						Player plr = Main.player[playerId];
						targetPos = plr.Center;
						targetVel = plr.velocity;
					}
					else if(Owner.HasNPCTarget){
						var npcId = Owner.target - 300;
						var npc = Main.npc[npcId];
						targetPos = npc.Center;
						targetVel = npc.velocity;
					}
					if (targetPos != null) {
						return
							WackyBagTr.Utilties.Calculates.PredictWithVelDirect(targetPos.Value-Projectile.Center,targetVel* ExtraPredict, MaxSpeed);
					}
				}
				return Vector2.Zero;
			}
		}
		//public float Accelerte = 0.1f;
		public float MaxSpeed => AccelerateBase / (1 - Damping)*0.9f;
		public float AccelerateBase => 0.5f;
		public float RotateBase => 0.03f;
		public float Damping => 0.98f;
		public float Accelerate=>ChaosBoss.SpeedPOWERToSpeedScale(SpeedEffect)* AccelerateBase * StraitMoveEffect;
		public float Rotate=> ChaosBoss.SpeedPOWERToSpeedScale(SpeedEffect) * RotateBase * RotateEffect;


		public override void AI()
		{
			base.AI();
			if (!Owner.active) {
				Projectile.Kill();
				return;
			}
			var tarvel = TargetVelocity;
			
			var targetEffect = (Projectile.velocity!=Vector2.Zero&&tarvel!=Vector2.Zero) ?(WackyBagTr.Utilties.Calculates.AngleCos(tarvel, Projectile.velocity)/2+0.5f) * 0.8f + 0.1f : 0;
			StraitMoveEffect = StraitMoveEffect * 0.90f + targetEffect * 0.10f;


			Projectile.velocity += Projectile.rotation.ToRotationVector2() * Accelerate;

			Projectile.rotation = Terraria.Utils.AngleTowards(Projectile.rotation, tarvel.ToRotation(), Rotate);
			//Projectile.velocity += Projectile.rotation.ToRotationVector2() * Accelerate;

			Projectile.velocity *= Damping;
			/*
			WackyBagTr.Utilties.Wacky.DrawLineFrame(Projectile.Center, tarvel * 60, color:Color.Green);
			WackyBagTr.Utilties.Wacky.DrawLineFrame(Projectile.Center, Projectile.velocity*60, color: Color.Red);
			WackyBagTr.Utilties.Wacky.DrawLineFrame(Projectile.Center, Projectile.rotation.ToRotationVector2() * Accelerate * 60*60/2,color:Color.Black);
			*/
		}
		public override void OnSpawn(IEntitySource source)
		{
			base.OnSpawn(source);
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.rotation);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.rotation=reader.ReadSingle();
			base.ReceiveExtraAI(reader);
		}
	}
}
