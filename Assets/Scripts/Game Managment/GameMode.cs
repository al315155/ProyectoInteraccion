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

		public List<string> GetEnemyTeam(){
			List<string> enemies = new List<string> ();
			if (this.type.Equals (GameType.Team_vs_Team) || this.type.Equals (GameType.One_Kill)) {
				//Los miembros son 6
				enemies.Add ("Healer");
				enemies.Add ("Tank");
				enemies.Add ("Tank");
				enemies.Add ("Distance Damage");
				enemies.Add ("Mele Damage");
				enemies.Add ("Mele Damage");
			} else {
				enemies.Add ("Boss");
			}
			return enemies;
		}

		public Vector2[] GetReferences(int dimension, string team){
			int sideMargin, topMargin, botMargin;
			CalculateMargins (dimension, out sideMargin, out topMargin, out botMargin);

			Vector2 topLeft;
			Vector2 botRight;

			if (team.Equals ("Player")) {
				topLeft = new Vector2 (sideMargin, topMargin);
				botRight = new Vector2 ((int) Math.Sqrt(dimension) - sideMargin, (int) Math.Sqrt(dimension) / 2 - botMargin);
			} else {
				topLeft = new Vector2 (sideMargin, (int) Math.Sqrt(dimension) / 2 + botMargin);
				botRight = new Vector2 ((int) Math.Sqrt(dimension) - sideMargin, (int) Math.Sqrt(dimension) - topMargin);
			}
			return new Vector2[]{ topLeft, botRight };
		}

		private void CalculateMargins(int dimension, out int sideMargin, out int topMargin, out int botMargin){

			int normalMargin = (int) Math.Sqrt(dimension) * 20 / 100;
			//Debug.Log ("normal margin" + normalMargin);
			sideMargin = normalMargin;
			topMargin = normalMargin / 2;
			botMargin = normalMargin;
		}
	}

	public enum GameType{
		Team_vs_Team, One_Kill, Boss_Fight
	}
}

