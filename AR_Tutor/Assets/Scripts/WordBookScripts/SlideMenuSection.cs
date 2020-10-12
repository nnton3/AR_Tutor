using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideMenuSection : MonoBehaviour
{
    public float Speed = 10;
    public float UpIndex = 100;
    private bool Move;
    private Vector2 MoveTarget;

    private void Start()
    {
        MoveTarget = new Vector2(transform.localPosition.x, transform.localPosition.y + UpIndex);
    }

    private void Update()
    {
        if (Move)
        {
            if (Vector2.Distance(transform.localPosition, MoveTarget) > 0.05f)
            {
                transform.localPosition = Vector2.Lerp(transform.localPosition, MoveTarget, Speed * Time.deltaTime);
            }
            else
            {
                Move = false;
            }
        }    
    }

    public void MoveUp()
    {
        MoveTarget = new Vector2(transform.localPosition.x, transform.localPosition.y + UpIndex);
        Move = true;
    }

    public void MoveDown()
    {
        MoveTarget = new Vector2(transform.localPosition.x, transform.localPosition.y - UpIndex);
        Move = true;
    }
}
