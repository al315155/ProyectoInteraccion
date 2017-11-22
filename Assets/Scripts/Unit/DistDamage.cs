using System;

namespace AssemblyCSharp
{
	public class DistDamage : Unit
	{
		static int life = 60;
		static int damage = 50;
		static int velocity = 30;
		static int movement = 6;
		static int agility = 25;
		static int critic = 15;

		public DistDamage () : base (life, damage, velocity, movement, critic, agility)
		{
		}

		public override void Hablity(){
		}
	}
}

