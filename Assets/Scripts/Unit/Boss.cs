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

		public Boss () : base (life, damage, velocity, movement, critic, agility)
		{
		}

		public override void Hablity(){
		}
	}
}

