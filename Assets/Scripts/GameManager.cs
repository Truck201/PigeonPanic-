using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Clases de Palomas")]
    public GameObject pigeonPrefab;
    public GameObject pigeonSpecialPrefab;

    private int pigeonsPerSpawn;
    private float pigeonNormalLifetime;
    private float pigeonSpecialLifetime;
    private float spawnDelayMin;
    private float spawnDelayMax;
    private int missedPigeons = 0;
    private int maxMissedPigeons;

    private List<GameObject> activePigeons = new List<GameObject>();
    private int maxPigeons;
    private bool[] occupiedHoles;

    private float originalSpawnDelayMin;
    private float originalSpawnDelayMax;
    private float originalPigeonNormalLifetime;
    private float originalPigeonSpecialLifetime;
    private int originalPigeonsPerSpawn;

    [Header("Puntos por Paloma")]
    public int pigeonNormalPoints;
    public int pigeonSpecialPoints;

    private int missObjectivePoints;

    [Header("Eggs Prefab")]
    public GameObject eggPrefab;
    public GameObject stainPrefab; // Prefab de la mancha con opacidad

    private float eggSpawnProbability;
    private int eggDestroyPoints;

    [Header("Lugares de Aparición")]
    public Transform[] spawnPoints;

    [Header("Datos de Nivel")]
    public LevelData currentLevelData;

    [HideInInspector]
    public LevelData runtimeLevelData;

    [Header("Datos Compras")]
    public UpgradeData upgradeData;

    [Header("Misses PigeonText")]
    [SerializeField] private TextMeshProUGUI missedPigeonsText;

    [Header("Puntos Requeridos")]
    [SerializeField] private TextMeshProUGUI requiredScoreText;

    private int requiredScore;
    private int scoreLevel;
    public int score;

    public enum SpecialEventType { DoubleTap, MoreTime, Zafari }
    private float specialPigeonProbability;
    private float zafariSpecialProbability;
    private SpecialEventType currentEvent;
    private float eventDuration; // Duración del evento especial
    private float eventTimer = 0f;
    private bool specialEventActive = false;

    [Header("Timer de Juego")]
    public ContadorJuego contadorJuego;
    private bool tiempoYaAñadido = false;

    public static int actualLevel;

    [Header("Texto de Meta Inicio")]
    public GameObject textoMetas;

    private float timerDesaparición = 1.5f;

    [Header("Sounds")]
    public AudioClip successSound;
    public AudioClip cleanStainSound;
    public AudioClip failSound;

    [Header("Texto Score TOTAL")]
    public TextMeshProUGUI scoreText;

    public static int finalScore;

    private CameraShaker cameraShaker;
    private List<GameObject> activeStains = new List<GameObject>();
   
    private float lastShakeTime = 0f;
    private float shakeCooldown = 1.0f;
    private float maxAccel = float.MinValue;
    private float minAccel = float.MaxValue;
    private float deltaThreshold = 2.5f; // Diferencia requerida para considerar un “salto fuerte”
    private float measureDuration = 1.0f; // Tiempo para medir salto
    private float measureTimer = 0f;

    private Vector3 lastAcceleration;

    [Header("Prefab de los Textos Flotantes")]
    public GameObject floatingTextPrefab;

    // COMBO SYSTEM
    [Header("Combos")]
    public Sprite comboSpriteDefault;  // < 15
    public Sprite comboSpriteMedium;   // ≥ 15
    public Sprite comboSpriteHigh;     // ≥ 30

    private int comboCount = 0;
    private float comboTimer = 0f;
    private float comboDuration;
    private bool comboActive = false;
    private int comboCountActive;

    // Sprites animados para el combo
    public GameObject comboEffectDefaultPrefab;
    public GameObject comboEffectMediumPrefab;
    public GameObject comboEffectHighPrefab;

    private GameObject activeComboEffect;

    // UI del combo
    public TextMeshProUGUI comboText;
    public Image comboCircle; // Asigna un Image tipo "Filled - Radial 360" en el inspector

    [Header("Tutorial Egg")]
    public GameObject tutorialEgg;
    public GameObject tutorialEggText;
    public GameObject tutorialStainText;

    private bool hasSpawnedTutorialEgg;
    private bool fistEggTutorial;
    private bool tutorialActive;

    [Header("Tutorial Pigeon")]
    public GameObject tutorialImage;  // Asigna desde el Inspector.
    private GameObject tutorialPigeon;
    private bool hasSpawnedTutorialPigeon = false;

    private bool comboTutorialShown = false;
    public GameObject comboTutorialText;

    void Start()
    {
        if (currentLevelData != null)
        {
            actualLevel = currentLevelData.level;
            requiredScore = currentLevelData.requiredScore;
            spawnDelayMin = currentLevelData.spawnDelayMin;
            spawnDelayMax = currentLevelData.spawnDelayMax; 
            eggSpawnProbability = currentLevelData.eggSpawnProbability;
            specialPigeonProbability = currentLevelData.specialPigeonProbability;
            zafariSpecialProbability = currentLevelData.zafariSpecialProbability;
            maxPigeons = currentLevelData.maxActivePigeons; //
            pigeonsPerSpawn = currentLevelData.pigeonsPerSpawn; //
            pigeonNormalLifetime = currentLevelData.pigeonNormalLifetime;
            pigeonSpecialLifetime = currentLevelData.pigeonSpecialLifetime;
            eventDuration = currentLevelData.eventDuration;  //
            comboDuration = currentLevelData.comboDuration;  //

            eggDestroyPoints = currentLevelData.eggDestroyPoints;
            pigeonNormalPoints = currentLevelData.pigeonNormalPoints; //
            pigeonSpecialPoints = currentLevelData.pigeonSpecialPoints; //

            missObjectivePoints = currentLevelData.missObjectivePoints;
            comboCountActive = currentLevelData.comboCountActive;  //

            maxMissedPigeons = currentLevelData.maxMissedPigeons; 

            // Instanciar LevelData para evitar modificar el original
            if (currentLevelData != null)
            {
                runtimeLevelData = ScriptableObject.Instantiate(currentLevelData);
                ApplyUpgradesToLevel(runtimeLevelData, upgradeData);
                AssignLevelValues(runtimeLevelData);
            }

            originalSpawnDelayMin = spawnDelayMin;
            originalSpawnDelayMax = spawnDelayMax;
            originalPigeonNormalLifetime = pigeonNormalLifetime;
            originalPigeonSpecialLifetime = pigeonSpecialLifetime;
            originalPigeonsPerSpawn = pigeonsPerSpawn;

        }

        UpdateMissedPigeonText();

        if (requiredScoreText != null && currentLevelData != null)
        {
            requiredScoreText.text = $"Meta: {currentLevelData.requiredScore.ToString("D6")}";
        }

        if (currentLevelData != null && currentLevelData.isTutorial)
        {
            tutorialActive = true;
            hasSpawnedTutorialEgg = false;
            fistEggTutorial = false;
            ShowTutorialStep();
        }
        else
        {
            StartCoroutine(SpawnRoutine());
        }

        occupiedHoles = new bool[spawnPoints.Length];
        lastAcceleration = Input.acceleration;
        cameraShaker = Camera.main.GetComponent<CameraShaker>();
    }
    void Update()
    {
      
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

                if (tutorialActive && hasSpawnedTutorialPigeon)
                {
                    if (hit.collider != null && hit.collider.gameObject == tutorialPigeon)
                    {
                        // Correcto: tocó la paloma del tutorial
                        Destroy(tutorialPigeon);
                        tutorialImage.SetActive(false);

                        // Reanudar juego normalmente
                        if (contadorJuego != null)
                            PausarContador(false);

                        StartCoroutine(SpawnRoutine());

                        Debug.Log("Chance");
                    }

                    // Si tocó fuera de la paloma tutorial, no hacer nada
                    return;
                }

                if (tutorialActive && hasSpawnedTutorialEgg)
                {
                    if (hit.collider != null && hit.collider.gameObject == tutorialEgg)
                    {
                        tutorialEggText.SetActive(false);
                        tutorialEgg.SetActive(false);

                        RemovePointsWithStain(transform.position);

                        CheckShakeToClean();
                        return;
                    }
                    return;
                }


                if (hit.collider == null || hit.collider.GetComponent<Pigeon>() == null)
                {
                    Debug.Log("Out of touch");

                    // No tocó una paloma
                    SubtractScore(missObjectivePoints);
                    ResetCombo();
                    UpdateScoreText();

                    SoundController.Instance.PlaySFX(SoundController.Instance.sfxErrarGolpe);

                    if (cameraShaker != null)
                    {
                        StartCoroutine(cameraShaker.Shake(0.2f, 0.2f));
                    }
                    Debug.Log("FALLASTE - Puntos: " + missObjectivePoints);

                    // Mostrar texto flotante "-20"
                    if (floatingTextPrefab != null)
                    {
                        Vector3 spawnPos = Camera.main.ScreenToWorldPoint(touch.position);
                        spawnPos.z = 0; // asegurarse de que esté en 2D
                        spawnPos += Vector3.up * 0.5f;

                        GameObject textObj = Instantiate(floatingTextPrefab, spawnPos, Quaternion.identity);
                        if (textObj.TryGetComponent<FloatingScoreText>(out var floatText))
                        {
                            floatText.SetText("-" + missObjectivePoints);
                            floatText.SetColor(Color.red); // Rojo para indicar pérdida
                        }
                    }
                }
            }
        }

        CheckShakeToClean();

        if (textoMetas != null)
        {
            textoMetas.SetActive(true);
            timerDesaparición -= Time.deltaTime;
            if (timerDesaparición <= 0)
            {
                textoMetas.SetActive(false);

            }
        }

        if (comboActive && !comboTutorialShown && tutorialActive)
        {
            if (tutorialEggText)
            {
                tutorialEggText.SetActive(false);
            }
            Debug.Log(comboActive);
            comboTutorialShown = true;
            comboActive = false; // Detener el combo mientras se muestra el tutorial
            comboTutorialText.SetActive(true); // Mostrar texto/tutorial
            Time.timeScale = 0f; // Pausar juego

            // Esperar toque para continuar
            StartCoroutine(EsperarToqueParaContinuarTutorialCombo());
        }

        if (comboActive)
        {
            comboTimer -= Time.deltaTime;

            if (comboCircle != null)
            {
                comboCircle.fillAmount = comboTimer / comboDuration;
            }
            if (comboTimer <= 0f)
            {
                ResetCombo();
            }
        }
        if (specialEventActive)
        {
            eventTimer -= Time.deltaTime;
            if (eventTimer <= 0f)
            {
                specialEventActive = false;
                tiempoYaAñadido = false; // Permite volver a añadir tiempo si se activa otra vez
                Debug.Log("Evento especial finalizado.");
            }
        }
    }
    void ApplyUpgradesToLevel(LevelData levelData, UpgradeData upgrades)
    {
        if (upgrades.normalPigeonBonus != 0)
            levelData.pigeonNormalPoints += upgrades.normalPigeonBonus;

        Debug.Log("Special Pigeon Normal Point -> " + levelData.pigeonNormalPoints);

        if (upgrades.specialPigeonBonus != 0)
            levelData.pigeonSpecialPoints += upgrades.specialPigeonBonus;

        Debug.Log("Special Pigeon Special Point -> " + levelData.pigeonSpecialPoints);

        if (upgrades.comboDurationBonus != 0f)
            levelData.comboDuration += Mathf.RoundToInt(upgrades.comboDurationBonus);

        Debug.Log("Combo Duration -> " + levelData.comboDuration);

        if (upgrades.comboEffectMultiplier != 0f)
            levelData.comboCountActive = Mathf.RoundToInt(levelData.comboCountActive - upgrades.comboEffectMultiplier);

        Debug.Log("Combo Count Active -> " + levelData.comboCountActive);

        Debug.Log("Mejoras aplicadas al LevelData");
    }

    private void AssignLevelValues(LevelData data)
    {
        
        pigeonNormalPoints = data.pigeonNormalPoints;
        pigeonSpecialPoints = data.pigeonSpecialPoints;

        eventDuration = data.eventDuration;
        comboDuration = data.comboDuration;
        comboCountActive = data.comboCountActive;

        Debug.Log("PPN -> " + pigeonNormalPoints + " PPS -> " + pigeonSpecialPoints + " ED -> " + eventDuration + " CD -> " + comboDuration + " CCA -> " + comboCountActive);

    }

    void AddCombo()
    {
        comboCount++;
        comboTimer = comboDuration;
        comboActive = true;

        if (comboText != null)
        {
            comboText.text = "" + comboCount;
            comboText.gameObject.SetActive(true);
        }

        if (comboCircle != null)
        {
            comboCircle.gameObject.SetActive(true);
            comboCircle.fillAmount = 1f;
        }

        // Evento especial si combo es múltiplo de 15
        if (comboCount > 0 && comboCount % comboCountActive == 0)
        {
            ActivateRandomEvent();
        }

        // Determinar sprite y efecto
        Sprite selectedSprite = comboSpriteDefault;
        GameObject effectPrefab = comboEffectDefaultPrefab;

        if (comboCount >= 30)
        {
            selectedSprite = comboSpriteHigh;
            effectPrefab = comboEffectHighPrefab;
        }
        else if (comboCount >= 15)
        {
            selectedSprite = comboSpriteMedium;
            effectPrefab = comboEffectMediumPrefab;
        }

        if (comboCircle != null)
        {
            comboCircle.sprite = selectedSprite;
        }

        // Instanciar efecto visual del combo
        ShowComboEffect(effectPrefab);
    }

    void ShowComboEffect(GameObject prefab)
    {
        if (activeComboEffect != null)
            Destroy(activeComboEffect);

        if (prefab != null)
        {
            activeComboEffect = Instantiate(prefab, comboCircle.transform.position, Quaternion.identity, comboCircle.transform);
            float offsetX = activeComboEffect.GetComponent<SpriteRenderer>()?.bounds.size.x * 0.245f ?? 0f;
            activeComboEffect.transform.localPosition = new Vector3(offsetX, -1.85f, 0f);

            // Activar BlinkAndScale si existe
            BlinkAndScale blinkScript = activeComboEffect.GetComponent<BlinkAndScale>();
            if (blinkScript != null)
            {
                blinkScript.StartBlinking();
            }
        }
    }

    void ResetCombo()
    {
        comboCount = 0;
        comboActive = false;

        if (comboText != null)
            comboText.gameObject.SetActive(false);

        if (comboCircle != null)
            comboCircle.gameObject.SetActive(false);

        if (comboCircle != null)
            comboCircle.transform.rotation = Quaternion.identity;

        if (activeComboEffect != null)
        {
            Destroy(activeComboEffect);
            activeComboEffect = null;
        }
    }

    void ActivateRandomEvent()
    {
        currentEvent = (SpecialEventType)Random.Range(0, 3);
        specialEventActive = true;
        eventTimer = eventDuration;

        string eventName = "";
        Color textColor = Color.yellow;

        SoundController.Instance.PlaySFX(SoundController.Instance.sfxEventoBonus); // Sound Event

        switch (currentEvent)
        {
            case SpecialEventType.DoubleTap:
                eventName = "Tap x2!";
                textColor = Color.cyan;
                break;
            case SpecialEventType.MoreTime:
                eventName = "++ Time!";
                textColor = Color.yellow;
                if (!tiempoYaAñadido && contadorJuego != null)
                {
                    contadorJuego.AñadirTiempo(10f);
                    tiempoYaAñadido = true;
                }
                break;
            case SpecialEventType.Zafari:
                eventName = "¡Zafari!";
                textColor = new Color(1f, 0.5f, 0.1f);
                break;
        }

        ShowFloatingText(eventName, textColor);
        Debug.Log("Evento especial activado: " + eventName);
    }

    public void PausarContador(bool pausar)
    {
        if (pausar)
        {
            // Pausar tiempo del juego
            Time.timeScale = 0f;

            // Detener lógica de combo
            comboActive = false;

            // Detener el contador si tienes una función para ello
            if (contadorJuego != null)
                contadorJuego.Pausar();

            Debug.Log("Juego pausado (tutorial)");
        }
        else
        {
            // Reanudar tiempo del juego
            Time.timeScale = 1f;

            // Reanudar combo si tenía valor antes
            if (comboCount > 0)
            {
                comboActive = true;
                comboTimer = Mathf.Clamp(comboTimer, 0.1f, comboDuration);
            }

            if (contadorJuego != null)
                contadorJuego.Reanudar();

            Debug.Log("Juego reanudado");
        }
    }


    public bool IsDoubleTapActive()
    {
        return specialEventActive && currentEvent == SpecialEventType.DoubleTap;
    }


    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // Verificar si el contador está activo y en los últimos 14 segundos
            if (contadorJuego != null && contadorJuego.TiempoRestante() <= 30f)
            {
                // Aumentar dificultad
                spawnDelayMin = originalSpawnDelayMin * 0.8f;
                spawnDelayMax = originalSpawnDelayMax * 0.8f;
                pigeonNormalLifetime = originalPigeonNormalLifetime * 0.8f;
                pigeonSpecialLifetime = originalPigeonSpecialLifetime * 0.8f;
                pigeonsPerSpawn = originalPigeonsPerSpawn + 1;
            }
            else
            {
                // Restaurar valores originales
                spawnDelayMin = originalSpawnDelayMin;
                spawnDelayMax = originalSpawnDelayMax;
                pigeonNormalLifetime = originalPigeonNormalLifetime;
                pigeonSpecialLifetime = originalPigeonSpecialLifetime;
                pigeonsPerSpawn = originalPigeonsPerSpawn;
            }

            yield return new WaitForSeconds(Random.Range(spawnDelayMin, spawnDelayMax)); // (0.8f, 1.5f)) (0.9f, 1.6f));

            int spawnCount = pigeonsPerSpawn; // Entre 1 y 4

            for (int i = 0; i < spawnCount; i++)
            {
                activePigeons.RemoveAll(p => p == null);

                if (activePigeons.Count >= maxPigeons)
                    break;

                List<int> freeIndices = new List<int>();
                for (int j = 0; j < occupiedHoles.Length; j++)
                {
                    if (!occupiedHoles[j])
                        freeIndices.Add(j);
                }

                if (freeIndices.Count == 0)
                    break;

                int selectedIndex = freeIndices[Random.Range(0, freeIndices.Count)];
                occupiedHoles[selectedIndex] = true;

                bool spawnEgg = Random.value < eggSpawnProbability; // 10% de probabilidad de huevo 0.1f
                
                if (tutorialActive && !comboActive)
                {
                    spawnEgg = false;
                }

                if (spawnEgg)
                {
                    if (tutorialActive && !hasSpawnedTutorialEgg && !fistEggTutorial)
                    {
                        fistEggTutorial = true;
                        hasSpawnedTutorialEgg = true;

                        
                        tutorialEggText.SetActive(true);
                        tutorialEgg.SetActive(true);
                        tutorialEgg.GetComponent<Egg>().Initialize(this, selectedIndex);

                        if (contadorJuego != null)
                            PausarContador(true); 
                    } else
                    {
                        GameObject newEgg = Instantiate(eggPrefab);
                        newEgg.transform.position = spawnPoints[selectedIndex].position + new Vector3(0, 98f, 0);
                        Egg eggScript = newEgg.GetComponent<Egg>();
                        eggScript.Initialize(this, selectedIndex); // pasamos el manager y el index
                    }
                }
                else
                {
                    float currentSpecialProbability = (specialEventActive && currentEvent == SpecialEventType.Zafari)
                    ? zafariSpecialProbability
                    : specialPigeonProbability;

                    bool isSpecial = Random.value < currentSpecialProbability;

                    GameObject prefabToUse = isSpecial ? pigeonSpecialPrefab : pigeonPrefab;

                    GameObject newPigeon = Instantiate(prefabToUse);
                    Pigeon pigeonScript = newPigeon.GetComponent<Pigeon>();

                    SoundController.Instance.PlaySFX(SoundController.Instance.sfxAparecerPaloma); // aparecer Paloma

                    if (isSpecial)
                    {
                        pigeonScript.Initialize(spawnPoints, selectedIndex, this, true, pigeonNormalLifetime, pigeonSpecialLifetime);
                    }
                    else
                    {
                        pigeonScript.Initialize(spawnPoints, selectedIndex, this, false, pigeonNormalLifetime, pigeonSpecialLifetime);
                    }

                    //pigeonScript.Initialize(spawnPoints, selectedIndex, this, isSpecial);
                    activePigeons.Add(newPigeon);
                }
            }
        }
    }

    public void RemovePointsWithStain(Vector3 position)
    {
        SubtractScore(eggDestroyPoints);
        ResetCombo();
        UpdateScoreText();
        
        SoundController.Instance.PlaySFX(SoundController.Instance.sfxErrarGolpe); // RomperHuevo
        
            
        if (cameraShaker != null) StartCoroutine(cameraShaker.Shake(0.3f, 0.3f));

        Debug.Log("¡Huevo roto!" +  " - " + eggDestroyPoints + " puntos");

        // Mostrar mancha en pantalla
        if (stainPrefab != null)
        {
            GameObject stain = Instantiate(stainPrefab, position, Quaternion.identity);
            stain.transform.position = position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            stain.transform.localPosition = new Vector3(Random.Range(-340f, 340f), Random.Range(-130f, 130f), 0f);

            Debug.Log("Mancha en: " + stain.transform.position);

            SpriteRenderer sr = stain.GetComponent<SpriteRenderer>();
            activeStains.Add(stain);

            if (hasSpawnedTutorialEgg)
            {

                tutorialStainText.SetActive(true);
            }
        }

        // Texto flotante
        if (floatingTextPrefab != null)
        {
            GameObject textObj = Instantiate(floatingTextPrefab, position + Vector3.up * 0.5f, Quaternion.identity);
            if (textObj.TryGetComponent<FloatingScoreText>(out var floatText))
            {
                floatText.SetText("-"+ eggDestroyPoints);
                floatText.SetColor(Color.red);
            }
        }
    }

    private void CheckShakeToClean()
    {
        float currentAccelMagnitude = Input.acceleration.magnitude;
        // Registrar el máximo y mínimo
        if (currentAccelMagnitude > maxAccel) maxAccel = currentAccelMagnitude;
        if (currentAccelMagnitude < minAccel) minAccel = currentAccelMagnitude;

        measureTimer += Time.deltaTime;

        if (hasSpawnedTutorialEgg)
        {
            float delta = maxAccel - minAccel;
            if (delta >= deltaThreshold)
            {
                StartCoroutine(CleanStainsStep());
                Handheld.Vibrate();

                if (contadorJuego != null)
                    PausarContador(false);

                hasSpawnedTutorialEgg = false;
                tutorialStainText.SetActive(false);
            }
        }

        if (measureTimer >= measureDuration)
        {
            float delta = maxAccel - minAccel;

            if (delta >= deltaThreshold && Time.time - lastShakeTime > shakeCooldown)
            {
                lastShakeTime = Time.time;
                StartCoroutine(CleanStainsStep());
                Handheld.Vibrate();
                Debug.Log($"¡Salto detectado! ΔAceleración = {delta:F2}, limpiando manchas.");
            }

            // Reset para siguiente ventana de medida
            measureTimer = 0f;
            maxAccel = float.MinValue;
            minAccel = float.MaxValue;
        }
    }


    IEnumerator CleanStainsStep()
    {
        foreach (GameObject stain in activeStains.ToArray()) // Copia para evitar errores si se eliminan
        {
            if (stain != null)
            {
                var controller = stain.GetComponent<StainController>();
                if (controller != null)
                {
                    controller.IncreaseCleanLevel();

                    if (controller.cleanLevel >= 3)
                        activeStains.Remove(stain); // Elimina de la lista también
                }
            }
        }
        
        SoundController.Instance.PlaySFX(SoundController.Instance.sfxLimpiarMancha);

        yield return null;
    }


    public void FreeHole(int index)
    {
        if (index >= 0 && index < occupiedHoles.Length)
            occupiedHoles[index] = false;
    }

    public void AddPoint(Vector3 position, int amount = 50, string text = "+50", Color? color = null)
    {
        AddCombo();

        int bonusMultiplier = comboCount >= 15 ? 2 : 1;

        AddScore(amount * bonusMultiplier);

        Debug.Log("ACIERTO - Puntos: " + score);
        UpdateScoreText();

        
        SoundController.Instance.PlaySFX(SoundController.Instance.sfxEliminarPaloma);
        

        string comboTextDisplay = "+" + (amount * bonusMultiplier);
        Color comboColor = (bonusMultiplier > 1) ? Color.cyan : (color ?? Color.green);

        if (floatingTextPrefab != null)
        {
            GameObject textObj = Instantiate(floatingTextPrefab, position + Vector3.up * 0.5f, Quaternion.identity);
            if (textObj.TryGetComponent<FloatingScoreText>(out var floatText))
            {
                floatText.SetColor(comboColor);
                floatText.SetText(comboTextDisplay);
            }

        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (currentLevelData != null)
        {
            currentLevelData.scoreLevel = score;
        }
        UpdateScoreText();
    }

    public void SubtractScore(int amount)
    {
        score -= amount;
        if (score < 0) score = 0;
        if (currentLevelData != null)
        {
            currentLevelData.scoreLevel = score;
        }
        UpdateScoreText();

        contadorJuego.RestarTiempo(5f);
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = score.ToString("D6");
    }

    void ShowFloatingText(string message, Color color)
    {
        if (floatingTextPrefab != null)
        {
            Vector3 spawnPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height * 0.65f, 10f));
            GameObject textObj = Instantiate(floatingTextPrefab, spawnPos, Quaternion.identity);

            if (textObj.TryGetComponent<FloatingScoreText>(out var floatText))
            {
                floatText.SetText(message);
                floatText.SetColor(color);
            }
        }
    }

    public void SetLevel(LevelData level)
    {
        currentLevelData = level;

        spawnDelayMin = currentLevelData.spawnDelayMin;
        spawnDelayMax = currentLevelData.spawnDelayMax;
        eggSpawnProbability = currentLevelData.eggSpawnProbability;
        specialPigeonProbability = currentLevelData.specialPigeonProbability;
        zafariSpecialProbability = currentLevelData.zafariSpecialProbability;
        maxPigeons = currentLevelData.maxActivePigeons;
        pigeonsPerSpawn = currentLevelData.pigeonsPerSpawn;
        pigeonNormalLifetime = currentLevelData.pigeonNormalLifetime;
        pigeonSpecialLifetime = currentLevelData.pigeonSpecialLifetime;
        eventDuration = currentLevelData.eventDuration;
        comboDuration = currentLevelData.comboDuration;
    }

    void ShowTutorialStep()
    {
        if (hasSpawnedTutorialPigeon) return;

        int randomIndex = Random.Range(2, spawnPoints.Length - 2);
        Transform spawnPoint = spawnPoints[randomIndex];

        Vector3 position = spawnPoint.position + new Vector3(-2f, 130f, 0);
        tutorialPigeon = Instantiate(pigeonPrefab, position, Quaternion.identity);
        tutorialPigeon.name = "TutorialPigeon";

        Debug.Log(position);

        // Mostrar imagen del tutorial en la posición de la paloma
        if (tutorialImage != null)
        {
            tutorialImage.SetActive(true);
         
            

            // Opción 1: si el sprite está en un hijo llamado "Visual"
            Transform visual = tutorialPigeon.transform.Find("Visual"); // o el nombre real del hijo
            Vector3 worldPosition = visual != null ? visual.position : spawnPoint.position + new Vector3(-2f, 110f, 0);

            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
            screenPos.x = position.x + 50f;  // offset en pantalla en píxeles X
            screenPos.y = position.y - 50f;  // offset en pantalla en píxeles Y

            tutorialImage.transform.position = screenPos;
            tutorialActive = true;
        }

        // Pausar combo y timer
        if (contadorJuego != null)
            PausarContador(true); // Asumimos que hay un método así.

        hasSpawnedTutorialPigeon = true;
        Debug.Log("true has spawned");
    }
    private IEnumerator EsperarToqueParaContinuarTutorialCombo()
    {
        bool esperando = true;

        while (esperando)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                esperando = false;
                if (hasSpawnedTutorialPigeon)
                    hasSpawnedTutorialPigeon = false;
            }

            yield return null;
        }

        comboTutorialText.SetActive(false); // Ocultar texto
        Time.timeScale = 1f; // Reanudar juego
    }

    public void RegisterMissedPigeon()
    {
        missedPigeons++;
        Debug.Log("Palomas falladas: " + missedPigeons);

        UpdateMissedPigeonText();

        if (missedPigeons >= maxMissedPigeons)
        {
            EndGameByFailure();
        }
    }

    private void EndGameByFailure()
    {
        Debug.Log("Fin del juego por fallar demasiadas palomas");

        // Lógica similar a derrota del temporizador
        // Guardar puntaje
        ScoreManager.Instance.AddScore(score); // Asumiendo que tenés un score acá
        SceneManager.LoadScene("Mejoras"); // O la escena de derrota
    }

    private void UpdateMissedPigeonText()
    {
        if (missedPigeonsText != null)
        {
            missedPigeonsText.text = $"{missedPigeons} / {maxMissedPigeons}";
        }
        if (missedPigeons >= maxMissedPigeons - 2)
            missedPigeonsText.color = Color.red;
        else
        {
            Color colorCustom;
            if (ColorUtility.TryParseHtmlString("#763C05", out colorCustom))
            {
                missedPigeonsText.color = colorCustom;
            }
        }
    }

}
