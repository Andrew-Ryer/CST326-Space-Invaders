using UnityEngine;

// Create a grid of 5 rows and 11 columns to place enemy prefabs
// Control enemy movement logic 
// Allow enemies to shoot
// Speed up with less enemies
// Able to reset enemy positions (still need to replace destroyed prefabs)
public class EnemyArray : MonoBehaviour
{
    [Header("Invaders")]
    public Invader[] prefabs = new Invader[5];
    public AnimationCurve speed = new AnimationCurve();
    private Vector3 direction = Vector3.right;
    private Vector3 initialPosition;

    [Header("Grid")]
    public int rows = 5;
    public int columns = 11;
    
    [Header("Step Movement")]
    public float stepTime = 1f;
    public float stepSize = 0.5f;       // how far the grid moves each tick
    public float baseStepTime = 1f;     // starting tick rate
    public float minStepTime = 0.08f;   // cap

    private float currentStepTime = -1f;

    [Header("Missiles")]
    public GameObject missilePrefab;
    public float missileSpawnRate = 1f;

    private void Awake()
    {
        initialPosition = transform.position;

        CreateInvaderGrid();
    }

    private void CreateInvaderGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            float width = 2f * (columns - 1);
            float height = 2f * (rows - 1);

            Vector2 centerOffset = new Vector2(-width * 0.5f, -height * 0.5f);
            Vector3 rowPosition = new Vector3(centerOffset.x, (2f * i) + centerOffset.y, 0f);

            for (int j = 0; j < columns; j++)
            {
                // Create a new invader and parent it to this transform
                Invader invader = Instantiate(prefabs[i], transform);

                // Calculate and set the position of the invader in the row
                Vector3 position = rowPosition;
                position.x += 2f * j;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        ScheduleStep(baseStepTime);
        //InvokeRepeating(nameof(Step), stepTime, stepTime);
        InvokeRepeating(nameof(MissileAttack), missileSpawnRate, missileSpawnRate);
    }
    
    void ScheduleStep(float newTime)
    {
        newTime = Mathf.Max(minStepTime, newTime);

        if (Mathf.Approximately(newTime, currentStepTime))
        {
            return;
        }

        currentStepTime = newTime;
        CancelInvoke(nameof(Step));
        InvokeRepeating(nameof(Step), currentStepTime, currentStepTime);
    }

    private void MissileAttack()
    {
        int amountAlive = GetAliveCount();

        // No missiles should spawn when no invaders are alive
        if (amountAlive == 0) {
            return;
        }

        foreach (Transform invader in transform)
        {
            // Any invaders that are killed cannot shoot missiles
            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }

            // Random chance to spawn a missile based upon how many invaders are
            // alive (the more invaders alive the lower the chance)
            if (Random.value < (1f / amountAlive))
            {
                GameObject shot = Instantiate(missilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }

    // Use Step instead of Update
    private void Step()
    {
        int totalCount = rows * columns;
        int amountAlive = GetAliveCount();
        int amountKilled = totalCount - amountAlive;
        float percentKilled = amountKilled / (float)totalCount;

        float curveSpeed = speed.Evaluate(percentKilled);
        curveSpeed = Mathf.Max(0.1f, curveSpeed);

        // Faster curveSpeed => smaller time between steps
        float desiredStepTime = baseStepTime / curveSpeed;
        ScheduleStep(desiredStepTime);

        // Animate + move (keep your existing code below)
        foreach (Transform t in transform)
        {
            if (!t.gameObject.activeInHierarchy) continue;

            Invader inv = t.GetComponent<Invader>();
            if (inv != null) inv.StepAnimation();
        }

        float stepDistance = stepSize; // keep distance consistent (classic feel)

        // Edge protection (reverse + down, then step)
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        bool willHitEdge = false;

        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy) continue;

            float nextX = invader.position.x + (stepDistance * direction.x);

            if (direction == Vector3.right && nextX >= (rightEdge.x - 1f))
            {
                willHitEdge = true;
                break;
            }
            else if (direction == Vector3.left && nextX <= (leftEdge.x + 1f))
            {
                willHitEdge = true;
                break;
            }
        }

        if (willHitEdge)
        {
            AdvanceRow(); // flips direction + moves down
        }

        // discrete step move
        transform.position += direction * stepDistance;
    }

    private void AdvanceRow()
    {
        // Flip the direction the invaders are moving
        direction = new Vector3(-direction.x, 0f, 0f);

        // Move the entire grid of invaders down a row
        Vector3 position = transform.position;
        position.y -= 1f;
        transform.position = position;
    }
    
    void OnEnable()
    {
        Player.onPlayerDied += ResetInvaders;
    }

    void ResetInvaders()
    {
        direction = Vector3.right;
        transform.position = initialPosition;

        foreach (Transform invader in transform) {
            invader.gameObject.SetActive(true);
        }
    }

    public int GetAliveCount()
    {
        int count = 0;

        foreach (Transform invader in transform)
        {
            if (invader.gameObject.activeSelf) {
                count++;
            }
        }

        return count;
    }
}
