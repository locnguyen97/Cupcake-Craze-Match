using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectMoveByDrag : MonoBehaviour
{
    [SerializeField] List<GameObject> particleVFXs;
    [SerializeField] private bool isRandomSprite = false;
    [SerializeField] private List<Sprite> listRandom;

    private Vector3 startPos;
    private Target target;
    private bool isPickup = false;

    private void OnEnable()
    {
        startPos = transform.position;
        if (isRandomSprite)
            GetComponent<SpriteRenderer>().sprite = listRandom[Random.Range(0, listRandom.Count)];
    }

    public void PickUp()
    {
        isPickup = true;
        //transform.rotation = new Quaternion(0,0,0,0);
    }

    public void CheckOnMouseUp()
    {
        //transform.position = startPos;
        isPickup = false;
        if (target)
        {
            GameObject explosion = Instantiate(particleVFXs[Random.Range(0, particleVFXs.Count)], transform.position,
                transform.rotation);
            Destroy(explosion, .75f);
            GameManager.Instance.levels[GameManager.Instance.GetCurrentIndex()].RemoveObject(gameObject);
            gameObject.SetActive(false);
            target.SetSpr(GetComponent<SpriteRenderer>().sprite);
            GameManager.Instance.CheckLevelUp();
        }
        else
        {
            transform.position = startPos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isPickup)
            if (collision.transform.GetComponent<Target>())
            {
                if(collision.transform.GetComponent<Target>().isTrueColor(GetComponent<SpriteRenderer>().sprite.name))
                    target = collision.transform.GetComponent<Target>();
            }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isPickup)
        {
            if (collision.transform.GetComponent<Target>() && target.transform == collision.transform)
            {
                target = null;
            }
        }
    }
}