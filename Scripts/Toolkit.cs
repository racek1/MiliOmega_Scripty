using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Toolkit : MonoBehaviour
{
    public static void spawnEffect(string name,Vector2 pos)
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Effects/" + name),pos,Quaternion.identity);
    }

    public static void playSound(string name, float pitchMax, float pitchMin)
    {
        GameObject soundClone = Instantiate(Resources.Load<GameObject>("Prefabs/Sound"),Vector2.zero,Quaternion.identity);
        AudioSource audioSource = soundClone.GetComponent<AudioSource>();
        
        audioSource.clip = Resources.Load<AudioClip>("Sounds/" + name);
        audioSource.pitch = Random.Range(pitchMin,pitchMax);
        audioSource.Play();

        Destroy(soundClone,audioSource.clip.length + 0.1f);
    }

    public static void playSound(string name)
    {
        GameObject soundClone = Instantiate(Resources.Load<GameObject>("Prefabs/Sound"),Vector2.zero,Quaternion.identity);
        AudioSource audioSource = soundClone.GetComponent<AudioSource>();
        
        audioSource.clip = Resources.Load<AudioClip>("Sounds/" + name);
        audioSource.Play();

        Destroy(soundClone,audioSource.clip.length + 0.1f);
    }

    public static void spawnDamageNumber(int damage, Vector2 pos)
    {
        GameObject damageTextObj = Resources.Load<GameObject>("Prefabs/DamageNumber");
        damageTextObj.transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();
        GameObject d = Instantiate(damageTextObj,pos,Quaternion.identity);
        Destroy(d,0.5f);
    }
}
