using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorManager : MonoBehaviour
{
    List<GameObject> cubes = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        //Set cursor to invisible
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Let this circle move with the cursor
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = new Vector3(pos.x, pos.y, -1);


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Add cube to cubes, change the cursor color and start coroutine
        cubes.Add(collision.gameObject);
        changeCursorColor();
        StopAllCoroutines();
        StartCoroutine("Timer");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //when the cursor is not hover on cube, remove the cube from the list, change the cursor color and stop coroutine
        cubes.Remove(collision.gameObject);
        changeCursorColor();
        StopAllCoroutines();
    }

    private void changeCursorColor()
    {
        //change the cursor color based on the size of cubes
        if (cubes.Count == 0)
        {
            //the cursor does not stay on any cube, so the cursor color is black
            this.GetComponent<SpriteRenderer>().color = Color.black;
        }
        else
        {
            //set the cursor color based the multiple cubes
            Color temp = new Color(0, 0, 0);
            foreach (GameObject cube in cubes)
            {
                //get the color of each cube
                Color cubeColor = cube.GetComponent<SpriteRenderer>().color;
                //if the color channel is not same, add colors together
                if (cubeColor.r * temp.r + cubeColor.g * temp.g + cubeColor.b * temp.b == 0)
                {
                    temp += cubeColor;
                }
                else
                {
                    // if color have same color channel, then calculate the average(Red + Green = Yellow, Yellow + Red = Orange)
                    temp = temp / 2 + cubeColor / 2;
                }
            }
            this.GetComponent<SpriteRenderer>().color = temp;
        }

    }

    private IEnumerator Timer()
    {
        //Set a 3s coroutine
        float waittime = 3.0f;
        float time = 0f;
        while (waittime >= time)
        {
            time += Time.deltaTime;
            yield return null;
        }
        //when the timer is finished, change the color of cubes
        changeCubeColor();
    }

    private void changeCubeColor()
    {
        //get a random color and set to cubes
        Color randomColor = Random.ColorHSV();

        foreach (GameObject cube in cubes)
        {
            //get the parent of the cube and change all children color
            Transform parent = cube.transform.parent;

            foreach (Transform child in parent)
            {
                child.GetComponent<SpriteRenderer>().color = randomColor;
            }
        }
        //change the cursor color after the color of cube been modified
        changeCursorColor();
    }
}
