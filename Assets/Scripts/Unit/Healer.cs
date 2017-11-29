using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Healer : Unit
	{
		static int life = 90;
		static int damage = 5;
		static int velocity = 15;
		static int movement = 2;
		static int agility = 25;
		static int critic = 5;

		public Healer () : base (life, damage, velocity, movement, critic, agility, Rol.Healer)
		{
		}

		public override void Hablity(){
		}
	}
}

