using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSprite : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlipSpriteXAxis(bool lookingLeft) {
        GetComponentInChildren<SpriteRenderer>().flipX = lookingLeft;

        if (gameObject.name == "brazo delantero idle pixelart" ||
            gameObject.name == "brazo trasero idle pixelart" ||
            gameObject.name == "cabeza idle pixelart") {

            if (lookingLeft)
                this.transform.position += new Vector3(
                    PlayerController.sharedInstance.transform.position.x - 10.0f,
                    this.transform.position.y,
                    this.transform.position.z
                    );
            else
                this.transform.position += new Vector3(
                    PlayerController.sharedInstance.transform.position.x + 10.0f,
                    this.transform.position.y,
                    this.transform.position.z
                    );
        }
    }
}
