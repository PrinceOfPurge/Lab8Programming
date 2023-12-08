using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class POWER : MonoBehaviour
{
    public enum PowerupType
    {
        TRANSLATE = 0,
        ROTATE,
        SCALE
    };

    public PowerupType PType;
    public float Speed = 10f;
    public Vector3 deltaVector;
    public Vector3 offset;

    bool isON = false;

    public List<GameObject> GetChildren(GameObject obj)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in obj.transform)
        {
            children.Add(child.gameObject);
        }
        return children;
    }

    public List<GameObject> GetAllChildren(GameObject obj)
    {
        List<GameObject> children = GetChildren(obj);
        for (var i = 0; i < children.Count; i++)
        {
            List<GameObject> moreChildren = GetChildren(children[i]);
            for (var j = 0; j < moreChildren.Count; j++)
            {
                children.Add(moreChildren[j]);
            }
        }
        return children;
    }

    public List<GameObject> FindChildrenWithName(string nam, GameObject obj)
    {
        List<GameObject> children = GetAllChildren(obj);
        List<GameObject> results = new List<GameObject>();
        for (var i = 0; i < children.Count; i++)
        {
            if (children[i].name == nam)
                results.Add(children[i].gameObject);
        }
        return results;
    }

    void ApplyPowerUp(Transform playerTransform)
    {
        if (playerTransform)
        {
            switch (PType)
            {
                case PowerupType.TRANSLATE:
                    // Do something here with translation
                    playerTransform.Translate(deltaVector * (Speed * Time.deltaTime));
                    break;
                case PowerupType.ROTATE:
                    // Do something here with rotation
                    playerTransform.Rotate(deltaVector * (Speed * Time.deltaTime));
                    break;
                case PowerupType.SCALE:
                    // Do something here with scaling
                    playerTransform.localScale += deltaVector * (Speed * Time.deltaTime);
                    break;
            }
        }
    }

    void FixedUpdate()
    {
        if (isON)
        {
            ApplyPowerUp(transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            isON = !isON;
            var obj = Instantiate(this, collision.gameObject.transform.position, Quaternion.identity);
            string childname = "CHILD";
            obj.name = childname;
            Destroy(obj.GetComponent<POWERUP>());
            int numChildren = FindChildrenWithName(childname, collision.gameObject).Count;
            obj.transform.localPosition = offset * numChildren;
            obj.transform.localScale /= 5f;
        }
        else
        {
            POWERUP other = collision.gameObject.GetComponent<POWERUP>();
            if (collision.gameObject.name.Contains("REVERSE"))
            {
                Speed = -Speed;
            }
        }
    }
}
