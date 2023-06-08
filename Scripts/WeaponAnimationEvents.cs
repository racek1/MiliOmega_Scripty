using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationEvents : MonoBehaviour
{
    private PlayerController playerController;
    private Player player;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        player = GameObject.Find("Player").GetComponent<Player>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    public void SetCanUse()
    {
        playerController.canUse = true;
    }

    public void SetFiringSprite()
    {
        spriteRenderer.sprite = player.currentWeapon.GetFiringSprite();
    }

    public void SetNormalSprite()
    {
        spriteRenderer.sprite = player.currentWeapon.GetSprite();
    }
}
