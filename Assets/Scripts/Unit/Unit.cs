using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine;

public class Unit {

	public static string HealerDetails = 
		"Un sanador sostiene el equipo gracias a su " +
		"capacidad para curar a sus compañeros.";
	public static string TankDetails = 
		"El papel de un tanque es potenciar la atención" +
		"del equipo enemigo sobre sí mismo, ya que posee" +
		"mayor aguante que el resto de roles.";
	public static string DistDamageDetails = 
		"Una unidad de ataque a distancia es letal, pero " +
		"fácilmente eliminable.";
	public static string MeleDamageDetails = 
		"Daño cuerpo a cuepro moderado y aguante moderado también.";

	private Rol rol;
	private int life;
	private int damage;
	private int velocity;
	private int movement;
	private int critic;
	private int agility;
	private Vector2 position;

	public Unit (int life, int damage, int velocity, int movement, int critic, int agility, Rol rol){
		this.life = life;
		this.damage = damage;
		this.velocity = velocity;
		this.movement = movement;
		this.critic = critic;
		this.agility = agility;
		this.rol = rol;
		//position = null;
	}



	public virtual void Hablity(){
	}

	public Rol UnitRol{
		get{ return rol; }
		set{ rol = value; }
	}

	public Vector2 Position{
		get{ return position; }
		set{ position = value; }
	}

	public int Life{
		get{ return life; }
		set{ life = value; }
	}

	public int Damage{
		get{ return damage; }
		set{ damage = value; }
	}

	public int Velocity{
		get{ return velocity; }
		set{ velocity = value; }
	}

	public int Movement{
		get{ return movement; }
		set{ movement = value; }
	}

	public static List<Unit> GenerateTeam(List<string> members){
		List<Unit> team = new List<Unit> ();
		foreach (string member in members) {
			switch (member) {
			case "Healer":
				team.Add (new Healer ());
				break;
			case "Tank":
				team.Add (new Tank ());
				break;
			case "Distance Damage":
				team.Add (new DistDamage ());
				break;
			case "Mele Damage":
				team.Add (new MeleDamage ());
				break;
			case "Boss":
				team.Add (new Boss ());
				break;
			}
		}
		return team;
	}
}

public enum Rol{ Healer, Tank, Mele, Distance, Boss}