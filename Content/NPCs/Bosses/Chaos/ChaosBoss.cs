using CreaturesLude.Content.NPCs.Bosses.Chaos;
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
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

using WackyBag;

using WackyBagTr.NPCs.Behaviors;


namespace CreaturesLude.Content.NPCs.Bosses.Chaos
{
	[AutoloadBossHead]
	public class ChaosBoss : WackyBagTr.NPCs.Behaviors.BehaviorModifiedNPC
	{
		//770 000 000
		// 200 * 1000 * 1000
		// 256 * 1024 * 1024
		public static int LifeMax => 100 * 1000 * 1000;
		public static Func<float, float> SpeedPOWERToSpeedScale = WackyBagTr.Utils.SmoothUp_0_Inf_to_0_Inf;
		
		public float POWER=>2*MathF.Sqrt((float)NPC.damage/ BaseDamage);
		



		public float Stage2Hp = 0.7f;
		public override string BossHeadTexture => BALLTexturePath;
		public static readonly string BALLTexturePath = ("CreaturesLude.Content.NPCs.Bosses.Chaos" + ".BALL").Replace('.', '/');
		
		public static Texture2D BALLTexture => TextureAssets.Npc[ModContent.NPCType<ChaosBoss>()].Value;

		public static void DrawBALL(SpriteBatch spriteBatch,Vector2 Center, float Radius, Color? drawColor, float rotation = 0)
		{
			spriteBatch.Draw(BALLTexture, Center - Main.screenPosition, null, drawColor ?? Color.White, rotation, new Vector2(4, 4), Radius / 4, SpriteEffects.None, 0);

		}

		public static void DrawBALL(Vector2 Center,float Radius,Color? drawColor,float rotation=0) {
			Main.EntitySpriteDraw(BALLTexture, Center - Main.screenPosition, null, drawColor??Color.White, rotation, new Vector2(4, 4), Radius / 4, SpriteEffects.None, 0);

		}
		public int BaseDamage => 500;
		public int Radius { get; protected set; }=16*8;
		public override string Texture => BALLTexturePath;
		public override void SetStaticDefaults()
		{
			/*
			NPCID.Sets.BossBestiaryPriority.Add(Type);
			
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				CustomTexturePath = "ExampleMod/Assets/Textures/Bestiary/MinionBoss_Preview",
				PortraitScale = 0.6f, // Portrait refers to the full picture when clicking on the icon in the bestiary
				PortraitPositionYOverride = 0f,
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			*/
			//NPCID.Sets.ImmuneToAllBuffs[Type]=true;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.BossBestiaryPriority.Add(NPC.type);
			NPCID.Sets.NeedsExpertScaling[Type] = true;
			Main.npcFrameCount[Type] = 1;
			base.SetStaticDefaults();
		}
		public override void SetDefaults()
		{
			Radius = 16 * 8;
			NPC.width = Radius*2;
			NPC.height = Radius * 2;
			NPC.damage = BaseDamage;
			NPC.defense = 1000;
			NPC.lifeMax = LifeMax;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.value = Item.buyPrice(copper:1);
			NPC.SpawnWithHigherTime(30);
			NPC.boss = true;
			NPC.npcSlots = 10f; // Take up open spawn slots, preventing random NPCs from spawning during the fight

			// Default buff immunities should be set in SetStaticDefaults through the NPCID.Sets.ImmuneTo{X} arrays.
			// To dynamically adjust immunities of an active NPC, NPC.buffImmune[] can be changed in AI: NPC.buffImmune[BuffID.OnFire] = true;
			// This approach, however, will not preserve buff immunities. To preserve buff immunities, use the NPC.BecomeImmuneTo and NPC.ClearImmuneToBuffs methods instead, as shown in the ApplySecondStageBuffImmunities method below.

			// Custom AI, 0 is "bound town NPC" AI which slows the NPC down and changes sprite orientation towards the target
			NPC.aiStyle = -1;

			// Custom boss bar
			NPC.BossBar = ModContent.GetInstance<ChaosBossBar>();

			// The following code assigns a music track to the boss in a simple way.
			if (!Main.dedServ)
			{
				Music = MusicID.TheTowers;// .GetMusicSlot(Mod, "Assets/Music/Ropocalypse2");
			}

			base.SetDefaults();
		}
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
		public DamageSpeedEmitPOWER DSEPOWER;
		public POWERAlloc POWERAlloc;
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
		public override (INPCBehavior,Action) CtorBehavior()
		{
			List<INPCBehavior> Behaviors = [];
			List<INPCBehavior> OnBehaviors = [];

			void AddActive(INPCBehavior behavior) {
				Behaviors.Add(behavior);
				OnBehaviors.Add(behavior);
			}
			var BehaviorsBag = new NPCBehaviorsCollection<INPCBehavior>() { 
				Behaviors=Behaviors
			};
			var ChooseTarget = new ChooseTarget(this);
			AddActive(ChooseTarget);


			DamageSpeedEmitPOWER DSEPOWER = new(this);
			AddActive(DSEPOWER);
			this.DSEPOWER = DSEPOWER;

			POWERAlloc POWERAlloc=new(this);
			AddActive(POWERAlloc);
			this.POWERAlloc = POWERAlloc;

			var LaunchProjCircler = new ProjCirclerHandler(this);
			AddActive(LaunchProjCircler);

			var KidManager = new KidManager(this);
			AddActive(KidManager);
			POWERAlloc.AddPowerNeed((()=>KidManager.Making?1<<16:0,(v)=> { }));

			var DoWhenHP = new DoWhenHP<ChaosBoss>(this);
			AddActive(DoWhenHP);



			var GoCloser = new GoCloser(this, DSEPOWER);
			AddActive(GoCloser);

			DoWhenHP.AddAction((e)=>LaunchProjCircler.SpreadProj(), Stage2Hp);

			var LaunchProjProChaser = new LaunchProjProChaser(this,DSEPOWER);
			AddActive(LaunchProjProChaser);
			POWERAlloc.AddPowerNeed((() => 1, (v) => LaunchProjProChaser.Power = v));

			var BeamHandler = new BeamHandler(this,DSEPOWER);
			Behaviors.Add(BeamHandler);
			DoWhenHP.AddAction((e) => { 
				BeamHandler.Activate();
				POWERAlloc.AddPowerNeed((() => 1, (v) => BeamHandler.Power = v));
			}, Stage2Hp);

			var LaunchNormalProj=new LaunchNormalProj(this,DSEPOWER);
			AddActive(LaunchNormalProj);

			POWERAlloc.AddPowerNeed((() => 1, (v) => LaunchNormalProj.Power = v));

			var Damping = new Damping(this, 0.995f);
			AddActive(Damping);

			return (BehaviorsBag,()=> {
				foreach (var item in OnBehaviors)
				{
					item.Activate();
				}
				LaunchNormalProj.AddLauncher();
			}
			);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			DrawBALL(NPC.Center,Radius,Color.White,NPC.rotation);
			return false;//base.PreDraw(spriteBatch, screenPos, drawColor);
		}

		public override void AI()
		{
			//Main.NewText($"{(NPCBehavior as NPCBehaviorsCollection<INPCBehavior>).Behaviors}");
			POWERAlloc.POWER = POWER;
			//for (int i = 0; i < NPC.maxBuffs; i++)
			//{
			//	NPC.buffTime[i] = 0;
			//	NPC.buffType[i] = 0;
			//}
			base.AI();
		}

		public override bool CheckActive()
		{
			return false;//base.CheckActive();
		}

		public override void OnKill()
		{
			base.OnKill();
			//for (int i = 0; i < 16; i++)
			//{
			//	Dust.NewDustPerfect(NPC.Center, ModContent.DustType<SpamBALLDust>())
			//		.fadeIn = 20;
			//}
		}

		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
		{
			//Main.NewText($"ApplyDifficultyAndPlayerScaling,{numPlayers},{balance},{bossAdjustment},hp:{NPC.lifeMax},power:{NPC.strengthMultiplier}");
			//base.ApplyDifficultyAndPlayerScaling(numPlayers, balance, bossAdjustment);
			NPC.lifeMax = (int)(NPC.lifeMax * balance * bossAdjustment);
			//POWER *= balance * bossAdjustment;
			
			//if (Main.expertMode) {
			//	if (Main.masterMode) {
			//		var e = new NPCStrengthHelper();

			//	}
			//}
		}

		public override void OnSpawn(IEntitySource source)
		{
			base.OnSpawn(source);
			//POWER *= NPC.strengthMultiplier;

			Main.NewText($"strenth: {NPC.strengthMultiplier} ,hp:{NPC.lifeMax},damage:{NPC.damage}");
		}

		public override bool CanHitNPC(NPC target)
		{
			return false;//base.CanHitNPC(target);
		}
		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			return false;//base.CanHitPlayer(target, ref cooldownSlot);
		}

		public override void BossHeadSlot(ref int index)
		{
			base.BossHeadSlot(ref index);
			//index = -1;
			
		}
		public override void BossHeadRotation(ref float rotation)
		{
			base.BossHeadRotation(ref rotation);

			WackyBagTr.Utilties.Wacky.DrawInMap(BALLTexture, NPC.Center, null, Color.White, 0, new Vector2(4, 4), Radius / 4 * Vector2.One, SpriteEffects.None, 0);
		}
	}
}
