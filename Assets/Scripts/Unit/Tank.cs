using System;

namespace AssemblyCSharp
{
	public class Tank : Unit
	{
		static int life = 200;
		static int damage = 20;
		static int velocity = 10;
		static int movement = 2;
		static int agility = 10;
		static int critic = 10;

		public Tank () : base (life, damage, velocity, movement, critic, agility)
		{
		}

		public override void Hablity(){
		}
	}
}

