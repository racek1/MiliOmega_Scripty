using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Variables")]
    public float baseSpeed;
    [Header("Components")]
    public Player player;
    public Rigidbody2D rigidbody2D;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Transform bulletPos;
    public GameObject bullet;
    public WeaponHolder weaponHolder;
    public Animator weaponAnimator;
    public GameObject pauseMenu;
    public Animator reloadWheelAnimator;
    public GameObject shotgunBullet;

    private Vector2 dir;
    [HideInInspector] public bool canUse = true;
    private float maxWeaponAnimatorSpeed = 5f;
    [HideInInspector] public int stamina;
    [HideInInspector] public int maxStamina = 100;
    private bool isSprinting = false;
    private float speed;
    private KeyCode sprintKey = KeyCode.LeftShift;
    private KeyCode reloadKey = KeyCode.R;
    private KeyCode pauseKey = KeyCode.Escape;
    private bool canSprint = true;
    private WaitForSeconds sprintCooldown = new WaitForSeconds(0.5f);
    void Start()
    {
        speed = baseSpeed;
        stamina = maxStamina;
        StartCoroutine(StaminaLoop());
    }
    void FixedUpdate()
    {
        dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rigidbody2D.velocity = dir * speed;
    }
    void Update()
    {
        if (Input.GetKeyDown(pauseKey) && !player.isChoosingPerk)
        {
            TogglePauseMenu(); 
        }
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
        if (Input.GetKeyDown(reloadKey))
        {
            StartCoroutine(Reload());
        }
        if (dir != Vector2.zero)
        {
            animator.SetBool("running", true);
        }
        else
        {
            animator.SetBool("running",false);
        }
        if (dir.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (dir.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        if (Input.GetKey(sprintKey))
        {
            ToggleSprint();
        }
        if (Input.GetKeyUp(sprintKey))
        {
            CancelSprint();
        }
    }

    private void ToggleSprint()
    {
        if (canSprint)
        {
            if (stamina > 0)
            {
                isSprinting = true;
                speed = baseSpeed * 2f;
                animator.speed = 2;
            }
            else { CancelSprint(); }
        }
    }

    private void CancelSprint()
    {
        isSprinting = false;
        animator.speed = 1;
        speed = baseSpeed;
        if (stamina < 5) { StartCoroutine(StartSprintCooldown()); }
    }

    private IEnumerator StartSprintCooldown()
    {
        canSprint = false;
        yield return sprintCooldown;
        canSprint = true;
    }

    private IEnumerator StaminaLoop()
    {
        WaitForSecondsRealtime waitForSecondsRealtime = new WaitForSecondsRealtime(0.05f);
        while (true)
        {
            if (isSprinting)
            {
                stamina-=3;
            }
            else { stamina+=2; }
            stamina = Mathf.Clamp(stamina, 0, maxStamina);
            yield return waitForSecondsRealtime;
        }
    }

    public void Shoot()
    {
        if (canUse && player.currentWeapon.currentAmmo > 0)
        {
            canUse = false;
            player.currentWeapon.currentAmmo--;
            weaponAnimator.SetTrigger("shoot");
            Toolkit.playSound(player.currentWeapon.name, 0.9f, 1f);
            StartCoroutine(player.ShakeCamera());
            if (1 / maxWeaponAnimatorSpeed > player.CalculateAttackSpeed()) { weaponAnimator.speed = 1 / player.CalculateAttackSpeed(); }
            else { weaponAnimator.speed = maxWeaponAnimatorSpeed; }
            StartCoroutine(StartShootCooldown());
            SpawnBullet();
        }
        else if (player.currentWeapon.currentAmmo == 0)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator StartShootCooldown()
    {
        yield return new WaitForSeconds(player.CalculateAttackSpeed());
        canUse = true;
    }    
    private IEnumerator Reload()
    {
        if (canUse && player.currentWeapon.currentAmmo < player.currentWeapon.magSize)
        {
            canUse = false;
            Toolkit.playSound("Reload");
            weaponAnimator.SetTrigger("reload");
            weaponAnimator.speed = 1 / player.CalculateReloadTime();
            reloadWheelAnimator.SetTrigger("start");
            reloadWheelAnimator.speed = 1 / player.CalculateReloadTime();
            yield return new WaitForSeconds(player.CalculateReloadTime());
            player.currentWeapon.currentAmmo = player.currentWeapon.magSize;
            player.UpdateUI();
            canUse = true;
        }
    }
    private void SpawnBullet()
    {
        Vector2 pos = transform.position + weaponHolder.transform.right * 0.75f;
        if (player.currentWeapon.name == "Shotgun")
        {
            GameObject b = Instantiate(shotgunBullet, pos, Quaternion.Euler(0, 0, weaponHolder.rotationZ));
            Projectile[] shotgunPellets = b.GetComponentsInChildren<Projectile>();
            foreach(Projectile p in shotgunPellets)
            {
                p.piercing = player.currentWeapon.piercing;
                p.damage = player.CalculateDamage();
            }
            Destroy(b, 2f);
        }
        else
        {
            GameObject b = Instantiate(bullet, pos, Quaternion.Euler(0, 0, weaponHolder.rotationZ));
            Projectile p = b.GetComponent<Projectile>();
            p.piercing = player.currentWeapon.piercing;
            p.damage = player.CalculateDamage();
            Destroy(b, 2f);
        }
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);

        if (pauseMenu.activeSelf) //Paused
        {
            PauseGame();
        }
        else //Unpaused
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        player.cursor.enabled = false;
        weaponHolder.enabled = false;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        player.cursor.enabled = true;
        weaponHolder.enabled = true;
    }

    public void QuitToMenu()
    {
        ResumeGame();
        SceneManager.LoadScene("Menu");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Coin")
        {
            player.AddCoin();
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "Hp")
        {
            player.Heal(25);
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "EnemyProjectile")
        {
            Projectile p = col.GetComponent<Projectile>();
            player.TakeDamage(p.damage);
            Destroy(col.gameObject);
        }
    }

}
