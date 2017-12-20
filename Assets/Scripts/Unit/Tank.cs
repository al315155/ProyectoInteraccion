using System;

namespace AssemblyCSharp
{
	public class Tank : Unit
	{
		static int life = 400;
		static int damage = 20;
		static int velocity = 10;
		static int movement = 1;
		static int agility = 5;
		static int critic = 10;
		static int habilityRange = 5;
		static int attackRange = 1;
		static int minVelocity = 0;
		static int maxVelocity = 20;
		static int habilityCritic = -1;


		public Tank () : base (life, damage, velocity, movement, critic, agility, habilityRange, attackRange, Rol.Tank, minVelocity, maxVelocity, habilityCritic)
		{
		}

		public override void Hablity(){
		}
	}
}

