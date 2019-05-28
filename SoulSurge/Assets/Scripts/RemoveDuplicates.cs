using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveDuplicates : MonoBehaviour
{
    private void CollisionEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("DuplicateChecker"))
        {
            Destroy(other.gameObject.transform.parent);
            Debug.Log("Removed duplicate");
        }
        Debug.Log(other);
    }

    private void CollisionStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("DuplicateChecker"))
        {
            Destroy(other.gameObject.transform.parent);
            Debug.Log("Removed duplicate");
        }
    }
}
