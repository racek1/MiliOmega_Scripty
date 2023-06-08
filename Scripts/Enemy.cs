using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Variables")]
    public int maxHp;
    public float speed;
    public int damage;
    public float attackRange;
    public Behaviour behaviour;
    [Header("Components")]
    public Material whiteMat;
    public Material defaultMat;
    public SpriteRenderer spriteRenderer;
    public GameObject hurtEffect;
    public GameObject coin;
    public GameObject hpObj;
    public GameObject? projectile;
    public Sprite? shootSprite;
    public Animator animator;

    private int hp;
    private WaitForSeconds flashDuration = new WaitForSeconds(0.1f);
    private bool canFlash = true;
    private Player player;
    private delegate void doBehaviour();
    private doBehaviour? DoBehaviour;
    private float distanceToPlayer = 9999;
    private WaitForSeconds damageDelay = new WaitForSeconds(1f);
    private GameController gameController;
    private MapGenerator mapGenerator;
    private bool canAttack = true;
    private WaitForSeconds attackCooldown = new WaitForSeconds(3);
    private bool canMove = true;
    private WaitForSeconds shootAnimationDuration = new WaitForSeconds(1);
    private bool alive = true;

    public enum Behaviour
    {
        FIGHTER, RANGED
    }
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        mapGenerator = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
        hp = maxHp;
        switch (behaviour)
        {
            case Behaviour.FIGHTER:
                DoBehaviour = FighterBehaviour;
                break;
            case Behaviour.RANGED:
                DoBehaviour = RangedBehaviour;
                break;
        }
        StartCoroutine(Attack());
    }

    private IEnumerator StartAttackCooldown()
    {
        canAttack = false;
        yield return attackCooldown;
        canAttack = true;
    }

    private void FighterBehaviour()
    {
        transform.position = Vector2.MoveTowards(this.transform.position,player.transform.position,speed * Time.deltaTime);
    }
    private void RangedBehaviour()
    {
        if (distanceToPlayer > 5 && canMove)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else if (distanceToPlayer < 2)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, -speed * Time.deltaTime);
        }
        if (canAttack && distanceToPlayer < 8)
        {
            ShootProjectile();
        }
    }

    void Update()
    {
        distanceToPlayer = Vector2.Distance(this.transform.position, player.transform.position);
        if (player.transform.position.x < this.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        if (DoBehaviour != null)
        {
            DoBehaviour();
        }
        if (Mathf.Abs(this.transform.position.x) > mapGenerator.size / 2 || (Mathf.Abs(this.transform.position.y) > mapGenerator.size / 2))
        {
            this.transform.position = Vector2.zero;
        }
    }

    public void TakeDamage(int value)
    {
        hp -= value;
        Toolkit.playSound("Hurt",0.8f,1f);
        Toolkit.spawnDamageNumber(value,new Vector2(this.transform.position.x + Random.Range(-0.25f,0.25f),this.transform.position.y + 0.25f));
        StartCoroutine(Flash());
        Instantiate(hurtEffect,this.transform.position,Quaternion.identity);
        if (hp <= 0)
        {
            Die();
        }
    }

    private void ShootProjectile()
    {
        StartCoroutine(StartAttackCooldown());
        StartCoroutine(ShootAnimation());

        Vector2 dir = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);

        Vector2 pos = this.transform.position;  
        GameObject b = Instantiate(projectile, pos, Quaternion.identity);
        Projectile p = b.GetComponent<Projectile>();
        p.transform.right = dir;
        p.damage = damage;
        Destroy(b, 20f);
    }

    private IEnumerator ShootAnimation()
    {
        canMove = false;
        animator.enabled = false;
        spriteRenderer.sprite = shootSprite;
        yield return shootAnimationDuration;
        animator.enabled = true;
        canMove = true;
        
    }

    private IEnumerator Flash()
    {
        if (canFlash)
        {
            canFlash = false;
            spriteRenderer.material = whiteMat;
            yield return flashDuration;
            spriteRenderer.material = defaultMat;
            canFlash = true;
        }
    }

    public void Die()
    {
        if (alive)
        {
            alive = false;
            gameController.RemoveEnemyFromList();
            player.Kills++;
            DropItem();
            Toolkit.spawnEffect("Vanish",this.transform.position);
            Destroy(this.gameObject);
        }
    }

    private void DropItem()
    {
        if (Random.Range(0, 5) == 0)
        {
            Instantiate(coin, this.transform.position, Quaternion.identity);
        }
        else if (Random.Range(0, 10) == 0)
        {
            Instantiate(hpObj, this.transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Projectile")
        {
            Projectile projectile = col.gameObject.GetComponent<Projectile>();
            TakeDamage(projectile.damage);
            if (!projectile.piercing)
            {
                Destroy(projectile.gameObject);
            }
        }
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            if (distanceToPlayer <= attackRange)
            {
                player.TakeDamage(damage);
            }
            yield return damageDelay;
        }
    }
}
