using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyormovement : MonoBehaviour
{
    public Rigidbody rb;
    public float speedC = 1f;
    public Material mt;
    // Функция задания физического движения плат по конвейеру
    void FixedUpdate()
    {
        mt.mainTextureOffset = new Vector2(Time.time * 10 * Time.deltaTime, 0f);
        Vector3 pos = rb.position;
        rb.position += Vector3.back * speedC * Time.fixedDeltaTime;
        rb.MovePosition(pos);
    }
}