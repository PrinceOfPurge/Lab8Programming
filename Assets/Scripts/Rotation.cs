using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float rotationSpeed = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") )
        {
            Debug.Log("Collision!");

            // Start rotating the GameObject
            StartCoroutine(RotateObject());
        }
    }

    private System.Collections.IEnumerator RotateObject()
    {
        while (true)
        {
            // Rotate the GameObject around its Z-axis
            transform.Rotate(0f,0f,1f,Space.Self);

            yield return null;
        }
    }
}
