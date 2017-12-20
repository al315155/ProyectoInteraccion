using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Healer : Unit
	{
		static int life = 180;
		static int damage = 5;
		static int velocity = 15;
		static int movement = 2;
		static int agility = 15;
		static int critic = 5;
		static int habilityRange = 5;
		static int attackRange = 1;
		static int minVelocity = 5;
		static int maxVelocity = 25;
		static int habilityCritic = 20;


		public Healer () : base (life, damage, velocity, movement, critic, agility, habilityRange, attackRange, Rol.Healer, minVelocity, maxVelocity, habilityCritic)
		{
		}

		public override void Hablity(){
			
		}

	}
}

