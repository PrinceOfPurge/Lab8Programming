using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POWERUPS : MonoBehaviour

{
    public GameObject playerObject;
    public enum PowerupType
    {
        TRANSLATE = 0,
        ROTATE,
        SCALE
    };

    [SerializeField]
    public PowerupType PType;

    [SerializeField]
    public float Speed = 10f;

    [SerializeField]
    public Vector3 deltaVector;

    [SerializeField]
    public Vector3 offset; // offset for children object positions

    bool isON = false; // is this powerup active?

    // Utility functions for getting children
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

    // Apply power-up logic
    void ApplyPowerUp(POWERUPS p)
    {
        if (p)
        {
            switch (p.PType)
            {
                case PowerupType.TRANSLATE:
                    transform.Translate(deltaVector * (Time.deltaTime * Speed));
                    break;
                case PowerupType.ROTATE:
                    transform.Rotate(0f,1f,0f, Space.Self);
                    break;
                case PowerupType.SCALE:
                    transform.localScale += deltaVector * (Time.deltaTime * Speed);
                    break;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isON)
        {
            // Apply power-up to the object this script is attached to
            ApplyPowerUp(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            // Player triggered this
            isON = !isON;

            // Create a child object as a power-up thumbnail
            var powerupClone = Instantiate(gameObject, collision.transform);

            // Change the name of the object
            string childName = "PowerupChild";
            powerupClone.name = childName;

            // Remove all power-up component scripts from the clone
            Destroy(powerupClone.GetComponent<POWERUPS>());

            // Determine how many children are already attached to the player
            int numChildren = FindChildrenWithName(childName, collision.gameObject).Count;

            // Set the position of the child based on how many already exist
            powerupClone.transform.localPosition = offset * numChildren;

            // Set the scale to be small
            powerupClone.transform.localScale /= 5f;
        }
        else
        {
            POWERUPS otherPowerup = collision.gameObject.GetComponent<POWERUPS>();
            // Check if the collided object's name contains "REVERSE"
            if (collision.gameObject.name.Contains("REVERSE"))
            {
                Speed = -Speed;
            }
            // TODO: Implement logic for other types of collisions or power-ups
        }
    }
}
