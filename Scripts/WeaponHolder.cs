using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public float speed;
    public GameObject weapon;
    public GameObject myPlayer;
    [HideInInspector] public float rotationZ;

    private bool isAnimating = false;

    public bool IsAnimating
    {
        get { return isAnimating; }
        set { isAnimating = value; }
    }

    void Update()
    {
        /*
        //Rotation
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle,Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation,rotation,speed * Time.deltaTime);
        //Flipping
        
        if (angle >= -90 && angle < 90) //No flip
        {
            weapon.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (angle >= 90 && angle < 180 || angle >= -180 && angle < -90)//Flip
        {
            weapon.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        */
        if (!isAnimating)
        {
            RotateTowardsMouse();
        }
    }

    public void RotateTowardsMouse()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        if (rotationZ < -90 || rotationZ > 90)
        {
            if (myPlayer.transform.eulerAngles.y == 0)
            {
                transform.localRotation = Quaternion.Euler(180, 0, -rotationZ);
            }
            else if (myPlayer.transform.eulerAngles.y == 180)
            {
                transform.localRotation = Quaternion.Euler(180, 180, -rotationZ);
            }
        }
    }
}
