using System;

namespace AssemblyCSharp
{
	public class Boss : Unit
	{
		static int life = 500;
		static int damage = 30;
		static int velocity = 15;
		static int movement = 8;
		static int agility = 5;
		static int critic = 10;
		static int habilityRange = 5;
		static int attackRange = 2;
		static int minVelocity = 12;
		static int maxVelocity = 18;
		static int habilityCritic = 40;


		public Boss () : base (life, damage, velocity, movement, critic, agility, habilityRange, Rol.Boss, minVelocity, maxVelocity, habilityCritic)
		{
		}

		public override void Hablity(){
		}
	}
}

