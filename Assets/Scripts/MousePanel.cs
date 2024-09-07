using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MousePanel : MonoBehaviour
{
    GameManager gameManager;

    public Vector2 offset = new Vector3(5, 0, 0);
    private Vector2 mousePos;

    [SerializeField]
    private TextMeshProUGUI location;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.gameOver)
        {
            gameObject.SetActive(false);
        }

        followMouse();
        setLocation();
    }

    // Sets the objects location to an offset of the mouse position
    void followMouse()
    {
        mousePos = Input.mousePosition;

        gameObject.transform.position = new Vector3(mousePos.x + offset.x, mousePos.y + offset.y, 0);
    }

    void setLocation()
    {
        location.text = gameManager.location;
    }

    
}
