using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class Player : MonoBehaviour
{
    [Header("Variables")]
    public int maxHp;
    [Header("Components")]
    public Material whiteMat;
    public Material defaultMat;
    public SpriteRenderer spriteRenderer;
    public Animator messageTextAnimator;
    public TextMeshProUGUI messageText;
    public Image hpBar;
    public SpriteRenderer weaponSpriteRenderer;
    public TextMeshProUGUI ammoText;
    public SpriteRenderer cursor;
    public TextMeshProUGUI killsText;
    public Animator killIconAnimator;
    public Image staminaBar;
    public Camera camera;
    public Sprite cursor_normal;
    public Sprite cursor_firing;
    public Image[] perkIcons = new Image[3];
    public TextMeshProUGUI[] perkNames = new TextMeshProUGUI[3];
    public TextMeshProUGUI[] perkDescriptions = new TextMeshProUGUI[3];
    public TextMeshProUGUI[] perkLevels = new TextMeshProUGUI[3];
    public Button[] perkSelectButton = new Button[3];
    public GameObject perkMenu;
    public GameObject hand2;
    public TextMeshProUGUI coinText;
    public SceneController sceneController;

    [HideInInspector] public Weapon currentWeapon;
    private bool canFlash = true;
    private int hp;
    private WaitForSeconds flashDuration = new WaitForSeconds(0.1f);
    private int kills = 0;
    private PlayerController playerController;
    private float defaultCameraZoom;
    private bool isCameraShaking = false;
    [HideInInspector] public List<Perk> perks = new List<Perk>();
    private List<Perk> randomPerks = new List<Perk>();
    [HideInInspector] public bool isChoosingPerk = false;

    void Start()
    {
        hp = maxHp;
        defaultCameraZoom = camera.orthographicSize;
        playerController = this.GetComponent<PlayerController>();
        EquipWeapon(PlayerPrefs.GetString("equippedWeapon","Pistol"));
        SceneManager.LoadScene("Arena",LoadSceneMode.Additive);
        Cursor.visible = false;
    }
    void Update()
    {
        cursor.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursor.transform.position = new Vector3(cursor.transform.position.x,cursor.transform.position.y,0);
        UpdateUI();
        if (Input.GetMouseButtonDown(0))
        {
            cursor.sprite = cursor_firing;
        }
        if (Input.GetMouseButtonUp(0))
        {
            cursor.sprite = cursor_normal;
        }
    }

    public void TakeDamage(int value)
    {
        hp -= value;
        Toolkit.playSound("Hurt", 0.8f, 1f);
        StartCoroutine(Flash());
        if (hp <= 0)
        {
            Die();
        }
    }

    public void Heal(int value)
    {
        hp += value;
        hp = Mathf.Clamp(hp,0,maxHp);
    }

    private void Die()
    {
        PlayerPrefs.SetInt("lastGameKills",kills);
        PlayerPrefs.SetInt("lastGameWave", GameObject.Find("GameController").GetComponent<GameController>().wave);
        Toolkit.spawnEffect("Vanish", this.transform.position);
        spriteRenderer.enabled = false;
        cursor.gameObject.SetActive(false);
        Destroy(this.transform.GetChild(1));
        playerController.weaponHolder.gameObject.SetActive(false);
        sceneController.GotoScene("GameOver");
    }

    private void EquipWeapon(string weaponName)
    {
        Weapon weapon = WeaponDatabase.GetWeapon(weaponName);
        currentWeapon = weapon;
        weaponSpriteRenderer.sprite = weapon.GetSprite();
        Vector2 handPos = new Vector2(0,0);
        switch (weaponName)
        {
            case "Pistol":
                handPos = new Vector2(0.25f, -0.1875f);
                break;
            case "Smg":
                handPos = new Vector2(1.125f, -0.0625f);
                break;
            case "Ar":
                handPos = new Vector2(1.25f,0.045f);
                break;
            case "Shotgun":
                handPos = new Vector2(1.125f, -0.06625f);
                break;
            case "SniperRifle":
                handPos = new Vector2(1.125f, -0.06625f);
                break;
            case "Revolver":
                handPos = new Vector2(0.25f, -0.1875f);
                break;
        }
        hand2.transform.localPosition = handPos;
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

    public void UpdateUI()
    {
        hpBar.fillAmount = (float)hp / (float)maxHp;
        ammoText.text = currentWeapon.currentAmmo + "/" + currentWeapon.magSize;
        killsText.text = kills.ToString();
        coinText.text = PlayerPrefs.GetInt("Coins",0).ToString();
        staminaBar.fillAmount = (float)playerController.stamina / (float)playerController.maxStamina;
    }

    public void ShowMessage(string text)
    {
        messageTextAnimator.SetTrigger("show");
        messageText.text = text;
    }

    public int Kills
    {
        get { return kills; }
        set { kills = value; killIconAnimator.SetTrigger("start"); }
    }

    public void AddPerk(Perk perk)
    {
        perks.Add(perk);
        switch (perk.name)
        {
            case "Bigger mag":
                currentWeapon.magSize = CalculateMagSize();
                break;
        }
    }

    public void GeneratePerks()
    {
        perkMenu.SetActive(true);
        isChoosingPerk = true;
        randomPerks = PerkDatabase.GetPerks();
        for (int i = 0; i < randomPerks.Count; i++)
        {
            perkSelectButton[i].gameObject.SetActive(true);
            perkLevels[i].text = "Lvl: " + GetPerkCount(randomPerks[i].name) + "/" + randomPerks[i].maxLevel;
            perkIcons[i].sprite = randomPerks[i].GetSprite();
            perkNames[i].text = randomPerks[i].name;
            perkDescriptions[i].text = randomPerks[i].description;
        }
        playerController.PauseGame();
    }

    private void ResetPerkCards()
    {
        for (int i = 0; i < 3; i++)
        {
            perkLevels[i].text = "";
            perkIcons[i].sprite = Resources.Load<Sprite>("Sprites/null");
            perkNames[i].text = "Max";
            perkDescriptions[i].text = "";
            perkSelectButton[i].gameObject.SetActive(false);
        }
    }

    public void SelectPerk(int index)
    {
        ResetPerkCards();
        perkMenu.SetActive(false);
        isChoosingPerk = false;
        playerController.ResumeGame();
        AddPerk(randomPerks[index-1]);
    }

    public IEnumerator ShakeCamera()
    {
        if (!isCameraShaking)
        {
            isCameraShaking = true;
            WaitForEndOfFrame delay = new WaitForEndOfFrame();
            float zoom = 1f;
            int frames = 15;
            float zoomPerFrame = (float)zoom / (float)frames / 2;
            for (int i = 0; i < frames / 2; i++)
            {
                camera.orthographicSize += zoomPerFrame;
                yield return delay;
            }
            for (int i = 0; i < frames / 2; i++)
            {
                camera.orthographicSize -= zoomPerFrame;
                yield return delay;
            }
            camera.orthographicSize = defaultCameraZoom;
            isCameraShaking = false;
        }
    }

    public int GetPerkCount(string perkName)
    {
        int count = 0;
        foreach (Perk p in perks)
        {
            if (p.name.Equals(perkName)) { count++; }
        }
        return count;
    }

    public void PlaySound(string soundName)
    {
        Toolkit.playSound(soundName);
    }

    public int CalculateDamage()
    {
        float perkAmount = GetPerkCount("Gnome slayer");
        float multiplier = perkAmount / 5;
        return Mathf.RoundToInt(currentWeapon.damage + currentWeapon.damage * multiplier);
    }

    public float CalculateAttackSpeed()
    {
        float perkAmount = GetPerkCount("Quick trigger");
        float multiplier = perkAmount / 10;
        return currentWeapon.attackSpeed - currentWeapon.attackSpeed * multiplier;
    }

    public float CalculateReloadTime()
    {
        float perkAmount = GetPerkCount("Fast hands");
        float multiplier = perkAmount / 10;
        return currentWeapon.reloadTime - currentWeapon.reloadTime * multiplier;
    }

    public int CalculateMagSize()
    {
        float perkAmount = GetPerkCount("Bigger mag");
        float multiplier = perkAmount / 5;
        return Mathf.RoundToInt(currentWeapon.magSize + WeaponDatabase.GetWeapon(currentWeapon.name).magSize * multiplier);
    }

    public void AddCoin()
    {
        PlayerPrefs.SetInt("Coins",PlayerPrefs.GetInt("Coins",0) + 1);
    }
}