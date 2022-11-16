using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaBarLocation : MonoBehaviour
{
    Transform startingPosition;
    Transform rightPosition;
    Transform leftPosition;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition.position = this.transform.position;

        leftPosition.position = startingPosition.position;
        rightPosition.position = new Vector2(startingPosition.position.x + 4, startingPosition.position.y);
    }

    void MoveLeft()
    {
        this.transform.position = leftPosition.position;
    }

    void MoveRight()
    {
        this.transform.position = rightPosition.position;
    }
}
