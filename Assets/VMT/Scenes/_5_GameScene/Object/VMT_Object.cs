using UnityEngine;

public class VMT_Object : MonoBehaviour
{
    public enum Shapes {Box,Ball,Pipe}
    public Shapes Shape;

    public enum Colors { White, Red, Green, Blue }
    public Colors ApperanceColor;

    public int Size;

    bool isDying = false;
    float dissolveTimer = 0;

    public void ChangeColor(Colors colorToChange)
    {
        ApperanceColor = colorToChange;

        if (ApperanceColor == Colors.White)
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
        }
        else if (ApperanceColor == Colors.Red)
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        }
        else if (ApperanceColor == Colors.Green)
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
        }
        else if (ApperanceColor == Colors.Blue)
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
        }
    }
    public void ChangeSize(int sizeToChange)
    {
        Size = sizeToChange;
        transform.localScale = new Vector3(sizeToChange, sizeToChange, sizeToChange);
    }

    public void Update()
    {
        //extra for shader
        if(isDying)
        {
            dissolveTimer += Time.deltaTime;
            GetComponent<MeshRenderer>().material.SetFloat("_Height", (2 - dissolveTimer) * 10);
            if (dissolveTimer >= 2)
            {
                Destroy(gameObject);
            }
        }
    }
    public void Kill()
    {
        isDying = true;
    }
}
