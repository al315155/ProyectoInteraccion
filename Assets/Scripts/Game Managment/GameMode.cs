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
	[Serializable]
	public class GameMode
	{
		private string title;
		private string explanation;
		private GameType type;
		private int members;

		public GameMode (string title, string explanation, GameType type, int members)
		{
			this.title = title;
			this.explanation = explanation;
			this.type = type;
			this.members = members;
		}

		public string Title{
			set{ title = value; }
			get{ return title; }
		}

		public string Explanation{
			set{ explanation = value; }
			get{ return explanation; }
		}

		public GameType Type{
			set{ type = value; }
			get{ return type; }
		}

		public int Members{
			set{ members = value; }
			get{ return members; }
		}

		public List<string> GetBasicTeam(){
			List<string> units = new List<string> ();
			if (this.type.Equals (GameType.Team_vs_Team) || this.type.Equals (GameType.One_Kill)) {
				//Los miembros son 6
				units.Add ("Healer");
				units.Add ("Tank");
				units.Add ("Tank");
				units.Add ("Distance Damage");
				units.Add ("Mele Damage");
				units.Add ("Mele Damage");
			} else {
				units.Add ("Boss");
			}
			return units;
		}

		public List<string> GetQLearningTeam(){
			// Un miembro de cada rol para QLearning
			List<string> units = new List<string> ();

			units.Add ("Healer");
			units.Add ("Tank");
			units.Add ("Distance Damage");
			units.Add ("Mele Damage");

			return units;
		}
	}

	public enum GameType{
		Team_vs_Team, One_Kill, Boss_Fight
	}
}

