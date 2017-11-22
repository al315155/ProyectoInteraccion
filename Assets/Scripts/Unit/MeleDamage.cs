using System;

namespace AssemblyCSharp
{
	public class MeleDamage : Unit
	{
		static int life = 120;
		static int damage = 35;
		static int velocity = 20;
		static int movement = 3;
		static int agility = 20;
		static int critic = 25;

		public MeleDamage () : base (life, damage, velocity, movement, critic, agility)
		{
		}

		public override void Hablity(){
		}
	}
}

