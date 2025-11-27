using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
public class playerMovements : MonoBehaviour
{
    [Header("Movement Settings")]
    public int frameRate = 60;
    public float playerSpeed = 10f;
    public int facingDirection = 1;
    public Animator anim;

    // flags
    private bool canMove = true;
    private bool canAttack = true;
    public bool isDead = false;
    private bool isCharging = false;
    private bool canChargeAtttack = true;


    // HP
    public int HP = 100;

    // normal attack
    public float attackCooldown = 3f;
    private float lastAttackTime = 0f;

    //for charging attack
    public float chargeAttackCooldown = 3f;
    private float chargeLastAttackTime;


    // movement lock during attack
    public float attackLockDuration = 0.5f;
    private float movementLockEndTime = 0f;

    //Player Prefs(Paused)
    //displaying the prefs
    [Header("E drag ang text ari choy")]
    public GameObject showHeaderTexts;
    public GameObject objectTexts;


    private bool isPaused = false;
    public TMP_Text CatText;
    public TMP_Text PotionText;
    public TMP_Text HeartText;
    public TMP_Text GemText;


    [Header("Attack Damage")]
    public int damage = 20;


    [Header("Interacting Settings")]
    public float interactRange = 2f;

    void Start()
    {

        showHeaderTexts.SetActive(false);
        Application.targetFrameRate = frameRate;
        anim.SetBool("Alive", true);
        //normal attack
        lastAttackTime = -1f * attackCooldown;

        //charged attack
        chargeLastAttackTime = -1f * chargeAttackCooldown;
    }

    void Update()
    {
        Application.targetFrameRate = frameRate;

        checkPaused();
        chargingAttack();
        attack();
        OnMove();
        Death();

        //interacting gameObjects
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryInteract();
        }
    }

    void OnMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        anim.SetFloat("horizontal", Mathf.Abs(horizontal));
        anim.SetFloat("vertical", Mathf.Abs(vertical));

        if (!canMove)
            return;

        if ((horizontal > 0 && transform.localScale.x < 0)
        || (horizontal < 0 && transform.localScale.x > 0))
        {
            Flip();
        }

        Vector2 position = transform.position;
        position.x += playerSpeed * horizontal * Time.deltaTime;
        position.y += playerSpeed * vertical * Time.deltaTime;
        transform.position = position;


    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(
            transform.localScale.x * -1,
            transform.localScale.y,
            transform.localScale.z
        );
    }

    void attack()
    {
        if (!canAttack) return;


        if (Time.time >= lastAttackTime + attackCooldown)
        {

            //normal attack - Pressing J choy
            if (Input.GetKeyDown(KeyCode.J))
            {
                Debug.Log("ATTACK");
                anim.SetTrigger("ATTACK");

                // detect enemy in front
                Vector2 attackPos = transform.position + new Vector3(facingDirection * 1f, 0, 0); // 1f = attack range
                Collider2D hitEnemy = Physics2D.OverlapCircle(attackPos, 0.5f, LayerMask.GetMask("Enemy"));
                if (hitEnemy != null)
                {
                    enemyAI enemy = hitEnemy.GetComponent<enemyAI>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damage);
                    }
                }

                lastAttackTime = Time.time;
                // lock movement
                canMove = false;
                movementLockEndTime = Time.time + attackLockDuration;
            }

        }

        if (!canMove && Time.time >= movementLockEndTime)
        {
            canMove = true;
            canAttack = true;
        }
    }


    void chargingAttack()
    {
        if (!canChargeAtttack)
            return;


        if (Time.time >= chargeLastAttackTime + chargeAttackCooldown)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                anim.SetBool("charging", true);
                isCharging = true;
                canMove = false;
                canAttack = false;
            }

            // Release charging (release K)
            if (isCharging && Input.GetKeyUp(KeyCode.K))
            {

                anim.SetBool("charging", false);
                anim.SetTrigger("chargeAttack");

                isCharging = false;
                canAttack = false;
                canMove = false;

                chargeLastAttackTime = Time.time;
                movementLockEndTime = Time.time + 1f; //1 sec is the duration sa attack-lock aron mo fit sa C.Attach animation
            }
        }

        if (!canAttack && !canMove && Time.time >= movementLockEndTime && !isCharging) {
            canMove = true;
            canAttack = true;
        }
    }

    void Death()
    {

        if (isDead && HP > 0)
        {
            anim.SetBool("Alive", true);
            isDead = false;

            canMove = true;
            canAttack = true;
            canChargeAtttack = true;
        }

        if (isDead) return;

        if (HP <= 0)
        {
            anim.SetBool("Alive", false);
            anim.SetTrigger("Death");
            canMove = false;
            canAttack = false;
            isDead = true;
            canChargeAtttack = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    //interact objects
    void TryInteract()
    {
        float interactRange = 1.5f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange);

        foreach (Collider2D hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
                return;
            }
        }

        Debug.Log("No interactable object nearby.");
    }

    //interact consumables
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICollectible collectible = collision.GetComponent<ICollectible>();
        if (collectible != null)
        {
            collectible.Collect();
        }
    }


    public void TakeDamage(int damage)
    {
        if (isDead) return;

        HP -= damage;
        Debug.Log("Player takes " + damage + " damage! HP left: " + HP);

        if (HP <= 0)
        {
            HP = 0;
            Death();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Vector2 attackPos = transform.position + new Vector3(facingDirection * 1f, 0, 0);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos, 0.5f);


        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 1f); // same as interactRange
    }

    //for pausing/displaying the texts of player prefs
    private void checkPaused()
    {


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                UpdateTexts();
                isPaused = true;
                Time.timeScale = 0f;
                showHeaderTexts.SetActive(true);
                objectTexts.SetActive(true);

            }

            else
            {
                isPaused = false;
                Time.timeScale = 1f;
                showHeaderTexts.SetActive(false);
                objectTexts.SetActive(false);
            }


        }
    }
    void UpdateTexts()
    {
        CatText.text = "Cat eaten: " + UnityEngine.PlayerPrefs.GetInt("EatCatCount", 0);
        PotionText.text = "Potion Drank: " + UnityEngine.PlayerPrefs.GetInt("GreenBottleCount", 0);
        HeartText.text = "Heart Broked: " + UnityEngine.PlayerPrefs.GetInt("HeartCount", 0);
        GemText.text = "Gem Collected: " + UnityEngine.PlayerPrefs.GetInt("GemCount", 0);
    }
}