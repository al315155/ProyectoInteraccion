using System;

namespace AssemblyCSharp
{
	public class DistDamage : Unit
	{
		static int life = 60;
		static int damage = 50;
		static int velocity = 30;
		static int movement = 4;
		static int agility = 25;
		static int critic = 15;

		public DistDamage () : base (life, damage, velocity, movement, critic, agility, Rol.Distance)
		{
		}

		public override void Hablity(){
		}
	}
}

