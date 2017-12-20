using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AssemblyCSharp;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace AssemblyCSharp
{
	public class MeleDamage : Unit
	{
		static int life = 240;
		static int damage = 35;
		static int velocity = 20;
		static int movement = 3;
		static int agility = 10;
		static int critic = 25;
		static int habilityRange = 3;
		static int attackRange = 1;
		static int minVelocity = 10;
		static int maxVelocity = 30;		
		static int habilityCritic = 30;

		public MeleDamage () : base (life, damage, velocity, movement, critic, agility, habilityRange, attackRange, Rol.Mele, minVelocity, maxVelocity, habilityCritic)
		{
		}
	}
}

