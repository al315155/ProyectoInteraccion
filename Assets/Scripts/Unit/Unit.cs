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
	private int currentLife;
	private int damage;
	private int velocity;
	private int movement;
	private int critic;
	private int agility;
	private Vector2 position;
	private int habilityRange;
	private int attackRange;
	private int minVelocity;
	private int maxVelocity;
	private int habilityCritic;
	private bool focused;
	private int focusedCount;

	private float[,] matrix;

	public float[,] Matrix{
		set{ matrix = value; }
		get{ return matrix; }
	}

	public Unit (int life, int damage, int velocity, int movement, int critic, int agility, int habilityRange, Rol rol, int minVelocity, int maxVelocity, int habilityCritic){
		this.life = life;
		this.damage = damage;
		this.velocity = velocity;
		this.movement = movement;
		this.critic = critic;
		this.agility = agility;
		this.rol = rol;
		currentLife = life;
		this.habilityRange = habilityRange;
		this.minVelocity = minVelocity;
		this.maxVelocity = maxVelocity;
		this.habilityCritic = habilityCritic;

		focused = false;
		focusedCount = 0;

		RandomizeVelocity ();
	}

	public Unit (int life, int damage, int velocity, int movement, int critic, int agility, int habilityRange, int attackRange, Rol rol, int minVelocity, int maxVelocity, int habilityCritic){
		this.life = life;
		this.damage = damage;
		this.velocity = velocity;
		this.movement = movement;
		this.critic = critic;
		this.agility = agility;
		this.rol = rol;
		currentLife = life;
		this.attackRange = attackRange;
		this.habilityRange = habilityRange;
		this.minVelocity = minVelocity;
		this.maxVelocity = maxVelocity;
		this.habilityCritic = habilityCritic;


		focused = false;
		focusedCount = 0;

		RandomizeVelocity ();

	}


	private void RandomizeVelocity(){

		velocity = Random.Range (minVelocity, maxVelocity);

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

	public int CurrentLife{
		get{ return currentLife; }
		set{ currentLife = value; }
	}

	public int Damage{
		get{ return damage; }
		set{ damage = value; }
	}

	public int Critic{
		get{ return critic; }
		set{ critic = value; }
	}

	public int Agility{
		get{ return agility; }
		set{ agility = value; }
	}

	public int Velocity{
		get{ return velocity; }
		set{ velocity = value; }
	}

	public int Movement{
		get{ return movement; }
		set{ movement = value; }
	}

	public int AttackRange{
		get{ return attackRange; }
		set{ attackRange = value; }
	}

	public int HabilityRange{
		get{ return habilityRange; }
		set{ habilityRange = value; }
	}

	public int HabilityCritic{
		get{ return habilityCritic; }
		set{ habilityCritic = value; }
	}

	public bool Focused{
		get{ return focused; }
		set{ focused = value; }
	}

	public int FocusedCount{
		get{ return focusedCount; }
		set{ focusedCount = value; }
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

	public Vector2 GetArea(Unit other){

		Vector2 range = new Vector2 (-1, -1);

		switch (other.UnitRol) {
		case Rol.Tank:
			range = new Vector2 (5, 10);
			break;
		case Rol.Distance:
			range = new Vector2 (20, 25);
			break;

		case Rol.Healer:
			range = new Vector2 (15, 20);
			break;

		case Rol.Mele:
			range = new Vector2 (10, 15);
			break;

		}
		return range;
	}


	public int GetHeal(Unit other){

		switch (other.UnitRol) {
		case Rol.Tank:
			return 15;
		case Rol.Distance:
			return 20;
		case Rol.Healer:
			return 30;			
		case Rol.Mele:
			return 25;
		}
		return -1;
	}
}

public enum Rol{ Healer, Tank, Mele, Distance, Boss}