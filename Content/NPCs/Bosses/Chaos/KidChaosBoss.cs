using CreaturesLude.Content.NPCs.Bosses.Chaos.Behaviors;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.LootSimulation.LootSimulatorConditionSetterTypes;
using Terraria.ID;

using WackyBag.Structures;
using WackyBag.Utils;

using WackyBagTr;
using WackyBagTr.NPCs.Behaviors;

using CreaturesLude.Content.NPCs.Bosses.Chaos.Behaviors.Kids;
using System.IO;
using WackyBagTr.Structures;

namespace CreaturesLude.Content.NPCs.Bosses.Chaos
{
    public class KidChaosBoss : BehaviorModifiedNPC
    {
		public int Radius { get; protected set; } = 16 * 4;
		//public static readonly string BALLTexturePath = ("CreaturesLude.Content.NPCs.Bosses.Chaos" + ".BALL").Replace('.', '/');
		public override string Texture => ChaosBoss.BALLTexturePath;
		public override string BossHeadTexture => ChaosBoss.BALLTexturePath;


		public float OffsetRadius {
			get => NPC.ai[0];
			set=>NPC.ai[0] = value;
		}

		public float OffsetDirection {
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}

		public Vector2 OffsetPos => OffsetDirection.ToRotationVector2() * OffsetRadius;


		public float Damping => 0.99f;
		public float SpeedBase =>28f;
		public float Speed => SpeedBase;//ChaosBoss.SpeedPOWERToSpeedScale(Owner.DSEPOWER.SpeedPOWER) * SpeedBase;

		public NPCAimedTarget Target =>Owner.NPC.GetTargetData();
		public Vector2 TargetVel
		{
			get;set;
		}

		public readonly static FloatAsIntSeparator AI3Separator=new((0,200));
        public int OwnerId => AI3Separator.Get(NPC.ai[3], 0);

        public ChaosBoss Owner => (Main.npc[OwnerId].ModNPC as ChaosBoss)!;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();

			NPCID.Sets.ImmuneToAllBuffs[Type] = true;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.NeedsExpertScaling[Type] = true;
		}

		public override void SetDefaults()
		{
			NPC.width = Radius * 2;
			NPC.height = Radius * 2;
			NPC.damage = 200;
			NPC.defense = 500;
			NPC.lifeMax = (int)(ChaosBoss.LifeMax*KidManager.HpDrain);
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.value = Item.buyPrice(copper: 1);
			NPC.boss = true;
			NPC.npcSlots = 10f;
			base.SetDefaults();
			
		}

		public override (INPCBehavior, Action) CtorBehavior()
        {
			List<INPCBehavior> Behaviors = [];
			List<INPCBehavior> OnBehaviors = [];

			List<INPCBehavior> AttackBehaviors = [];

			void AddActive(INPCBehavior behavior)
			{
				Behaviors.Add(behavior);
				OnBehaviors.Add(behavior);
			}
			void AddAttackActive(INPCBehavior behavior)
			{
				AttackBehaviors.Add(behavior);
				OnBehaviors.Add(behavior);
			}
			var BehaviorsBag = new NPCBehaviorsCollection<INPCBehavior>()
			{
				Behaviors = Behaviors
			};
			var AttackBehaviorsBag = new NPCBehaviorsCollection<INPCBehavior>() { 
				Behaviors=AttackBehaviors
			};
			AddActive(AttackBehaviorsBag);

			Spin Spin = new Spin(this);
			AddActive(Spin);

			SetAttackVel SetAttackVel = new SetAttackVel(this);
			AddAttackActive(SetAttackVel);
			
			GoHeal GoHeal = new GoHeal(this,AttackBehaviorsBag);
			AddActive(GoHeal);

			var Move = new Move(this);
			AddActive(Move);
			var Damping = new Damping(this, this.Damping);
			AddActive(Damping);

			return (BehaviorsBag, () => {
				foreach (var item in OnBehaviors)
				{
					item.Activate();
				}
				//LaunchNormalProj.AddLauncher();
			}
			);
		}

		public override void ModifyIncomingHit( ref NPC.HitModifiers modifiers)
		{
			base.ModifyIncomingHit( ref modifiers);
			modifiers.FinalDamage*=0.25f;
		}

		public override bool CheckDead()
		{
			base.CheckDead();
			return false;
		}
		public override bool CheckActive()
		{
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			ChaosBoss.DrawBALL(NPC.Center, Radius, Color.White, NPC.rotation);
			return false;
		}

		public override void AI()
		{
			//for (int i = 0; i < NPC.maxBuffs; i++)
			//{
			//	NPC.buffTime[i] = 0;
			//	NPC.buffType[i] = 0;
			//}
			if (Owner == null || Owner.NPC.active == false) {
				NPCBehavior.Warp()?.Pause();
				NPCBehavior.Dispose();
				NPC.active = false;
			}
			base.AI();
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(NPC.rotation);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			NPC.rotation = reader.ReadSingle();
			base.ReceiveExtraAI(reader);
		}
	}
}
