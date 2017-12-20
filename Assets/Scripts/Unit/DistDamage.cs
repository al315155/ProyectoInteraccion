using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AssemblyCSharp;
using System;

namespace AssemblyCSharp
{
	public class DistDamage : Unit
	{
		static int life = 120;
		static int damage = 50;
		static int velocity = 30;
		static int movement = 4;
		static int agility = 20;
		static int critic = 15;
		static int habilityRange = 6;
		static int attackRange = 4;
		static int minVelocity = 20;
		static int maxVelocity = 40;
		static int habilityCritic = -1;


		public DistDamage () : base (life, damage, velocity, movement, critic, agility, habilityRange, attackRange, Rol.Distance, minVelocity, maxVelocity, habilityCritic)
		{
		}

		public override void Hablity(){
		}
	}



}

