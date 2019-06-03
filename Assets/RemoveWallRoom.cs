using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveWallRoom : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            if (collision.transform.parent.position == transform.parent.position)
            {
                Debug.Log("Removed room: " + collision.transform.parent.name + " because it collides with " + transform.parent);
                Destroy(collision.transform.parent.gameObject);
            }
        }
    }
}
  