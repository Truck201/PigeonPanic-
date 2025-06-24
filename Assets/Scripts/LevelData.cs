using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("Level")]
    public int level;

    [Header("Puntos Requeridos para Completar Nivel")]
    public int requiredScore;

    [Header("Configuración de Spawn")]
    public float spawnDelayMin = 0.9f;
    public float spawnDelayMax = 1.6f;

    [Header("Probabilidades de Aparición")]
    [Range(0f, 1f)]
    public float eggSpawnProbability = 0.1f;

    [Range(0f, 1f)]
    public float specialPigeonProbability = 0.15f;

    [Range(0f, 1f)]
    public float zafariSpecialProbability = 0.4f;

    [Header("Dificultad por Nivel")]
    [Range(1, 10)]
    public int maxActivePigeons = 4;

    [Range(1, 5)]
    public int pigeonsPerSpawn = 1;

    [Header("Duración de Palomas")]
    public float pigeonNormalLifetime = 1.7f;
    public float pigeonSpecialLifetime = 1.7f;

    [Header("Cada Cuenta de Combos")]
    public int comboCountActive;

    [Header("Duración del Combo")]
    public int comboDuration;

    [Header("Duración del Evento")]
    public int eventDuration;

    [Header("Puntos Totales de Nivel")]
    public int scoreLevel;

    [Header("Palomas Fallidas")]
    public int maxMissedPigeons;

    [Header("Puntos de las Palomas Normales")]
    public int pigeonNormalPoints;

    [Header("Puntos de las Palomas Especiales")]
    public int pigeonSpecialPoints;

    [Header("Penalización de Romper Huevos")]
    public int eggDestroyPoints;

    [Header("Penalización de Fallar Toque")]
    public int missObjectivePoints;


    [Header("Is Tutorial")]
    public bool isTutorial;
}
