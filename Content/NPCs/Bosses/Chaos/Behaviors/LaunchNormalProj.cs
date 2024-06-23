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

using WackyBagTr.NPCs.Behaviors;

namespace CreaturesLude.Content.NPCs.Bosses.Chaos.Behaviors
{
	public class LaunchNormalProj : ServerLoopDo<ChaosBoss>
	{
		public void AddDelayLaunch(int timeleft) {

			this.Add(()=> {
				timeleft--;
				if (timeleft <= 0) {
					AddLauncher();
					return false;
				}
				return true;
			});
		}

		public void AddLauncher()
		{
			this.Add(Main.rand.NextFromList(Launchers)());
		
		}
		public float Power;

		public Func< Func< bool>>[] Launchers ;

		public LaunchNormalProj(ChaosBoss modNPC,DamageSpeedEmitPOWER POWER) : base(modNPC)
		{
			Launchers = [
				()=>{
					int countTotal=2;
					int count=-countTotal;
					int interval=0;
					int intervalTotal=25;
					const float AngleDel=MathHelper.Pi/180*20;
					return ()=>{
						interval+=WackyBagTr.Utils.RandIntoInt(Power*POWER.EmitPOWER);
						if(interval>=intervalTotal){
							float Speed=24f*ChaosBoss.SpeedPOWERToSpeedScale( Power*POWER.SpeedPOWER);
							int Damage=(int)(250*Power*POWER.DamagePOWER);
							float damping=0.98f;
							float accelerate=Speed*(1-damping);
							var target=NPC.GetTargetData();
							if(!target.Invalid){

								float tardir=WackyBagTr.Utilties.Calculates.PredictWithVel(target.Center-NPC.Center,target.Velocity,Speed)??((target.Center-NPC.Center).ToRotation());

								interval-=intervalTotal;
								if(Main.netMode!=NetmodeID.MultiplayerClient){
									Projectile.NewProjectile(
										NPC.GetSource_FromAI(),
										NPC.Center,
										(tardir+count*AngleDel).ToRotationVector2()*Speed/2,
										ModContent.ProjectileType<NormalProj>(),
										Damage,
										0,-1,damping,AngleDel/60,accelerate
										);
									Projectile.NewProjectile(
										NPC.GetSource_FromAI(),
										NPC.Center,
										(tardir-count*AngleDel).ToRotationVector2()*Speed/2,
										ModContent.ProjectileType<NormalProj>(),
										Damage,
										0,-1,damping,-AngleDel/60,accelerate
										);
								}
								count++;
								if(count>countTotal){
									AddDelayLaunch(60);
									return false;
								}
								else return true;
							}

						}
						return true;
					};
				},
				()=>{
					int countTotal=8;
					int count=0;
					int interval=0;
					int intervalTotal=20;
					float AngleDel=MathHelper.Pi/180*30;
					return ()=>{
						interval+=WackyBagTr.Utils.RandIntoInt(Power*POWER.EmitPOWER);
						if(interval>=intervalTotal){
							float Speed=24f*ChaosBoss.SpeedPOWERToSpeedScale( Power*POWER.SpeedPOWER);
							int Damage=(int)(250*Power*POWER.DamagePOWER);
							float damping=0.98f;
							float accelerate=Speed*(1-damping);
							var target=NPC.GetTargetData();
							if(!target.Invalid){
								float tardir=WackyBagTr.Utilties.Calculates.PredictWithVel(target.Center-NPC.Center,target.Velocity,Speed)??((target.Center-NPC.Center).ToRotation());
								interval-=intervalTotal;

								Projectile.NewProjectile(
										NPC.GetSource_FromAI(),
										NPC.Center,
										(tardir+Main.rand.NextFloatDirection()*AngleDel).ToRotationVector2()*Speed/2,
										ModContent.ProjectileType<NormalProj>(),
										Damage,
										0,-1,damping,AngleDel/60*Main.rand.NextFloatDirection(),accelerate
										);
								count++;

								if(count>countTotal){
									AddDelayLaunch(60);
									return false;
								}
								else return true;
							}

						}
						return true;
					};
				},
				()=>{
					int countTotal=32;
					int count=0;
					int interval=0;
					int intervalTotal=2;
					float AngleDel=MathHelper.Pi*2;
					return ()=>{
						interval+=WackyBagTr.Utils.RandIntoInt(Power*POWER.EmitPOWER);
						if(interval>=intervalTotal){
							float Speed=(WackyBag.Utils.Calculate.StdGaussian()*0.5f+1f)*32f*ChaosBoss.SpeedPOWERToSpeedScale( Power*POWER.SpeedPOWER);
							int Damage=(int)(250*Power*POWER.DamagePOWER);
							float damping=0.99f;
							float accelerate=Speed*(1-damping);
							var target=NPC.GetTargetData();
							if(!target.Invalid){
								//float tardir=WackyBagTr.Utilties.Calculates.PredictWithVel(target.Center-NPC.Center,target.Velocity,Speed)??((target.Center-NPC.Center).ToRotation());
								interval-=intervalTotal;

								Projectile.NewProjectile(
										NPC.GetSource_FromAI(),
										NPC.Center,
										(Main.rand.NextFloat()*AngleDel).ToRotationVector2()*Speed/8,
										ModContent.ProjectileType<NormalProj>(),
										Damage,
										0,-1,damping,AngleDel/36/60*WackyBag.Utils.Calculate.StdGaussian(),accelerate
										);
								count++;

								if(count>countTotal){
									AddDelayLaunch(120);
									return false;
								}
								else return true;
							}

						}
						return true;
					};
				}
			];
		}
	}
}
