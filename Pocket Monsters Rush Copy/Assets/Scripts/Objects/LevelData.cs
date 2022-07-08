using PathCreation;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public Transform StartPoint;

    public Transform BulletsParent;
    public PathCreator PathCreator;

    public Transform HeroPoint;
    public Transform EnemyPointCam;
    public Transform[] PokemonPoints;

    public Rigidbody EnemyRigidbody;
    public List<PokemonBattle> EnemyPokemons;
    public RagdollPhysics RagdollPhysics;

    public float MaxWallsBrokenIndex = 2;
    public Wall[] Walls;
}
