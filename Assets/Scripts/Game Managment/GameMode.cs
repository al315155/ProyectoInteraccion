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

//		public List<Unit> GetEnemyTeam(){
//			List<Unit> enemies = new List<Unit> ();
//			if (this.type.Equals (GameType.Team_vs_Team) || this.type.Equals (GameType.One_Kill)) {
//				//Los miembros son 6
//				enemies.Add (new Healer ());
//				enemies.Add (new Tank ());
//				enemies.Add (new Tank ());
//				enemies.Add (new DistDamage ());
//				enemies.Add (new MeleDamage ());
//				enemies.Add (new MeleDamage ());
//			} else {
//				enemies.Add (new Boss ());
//			}
//
//			return enemies;
//		}

		public Vector2[] GetReferences(int dimension, string team){
			int sideMargin, topMargin, botMargin;
			CalculateMargins (dimension, out sideMargin, out topMargin, out botMargin);

			Vector2 topLeft;
			Vector2 botRight;

			if (team.Equals ("Enemy")) {
				topLeft = new Vector2 (sideMargin, topMargin);
				botRight = new Vector2 (dimension / 2 - sideMargin, dimension / 4 - botMargin);
			} else {
				topLeft = new Vector2 (sideMargin, dimension / 4 + botMargin);
				botRight = new Vector2 (dimension / 2 - sideMargin, dimension / 2 - topMargin);
			}
			return new Vector2[]{ topLeft, botRight };
		}

		private void CalculateMargins(int dimension, out int sideMargin, out int topMargin, out int botMargin){
			int normalMargin = dimension / 2 * 20 / 100;
			sideMargin = topMargin = normalMargin;
			botMargin = 2 * normalMargin;
		}
	}

	public enum GameType{
		Team_vs_Team, One_Kill, Boss_Fight
	}
}

