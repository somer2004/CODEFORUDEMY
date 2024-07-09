using UnityEngine;


public class Manager : MonoBehaviour
{
    public GameObject currentCube;
    public GameObject lastCube;
    public GameObject finish, game;

    public int level;
    public bool done;

    private void Start()
    {
        game.SetActive(true);
        finish.SetActive(false);
        newBlock();
    }
    private void Update()
    {
        if (done)
        {
            return;
        }
        var time = Mathf.Abs(Time.realtimeSinceStartup % 2f - 1f);
        var pos1 = lastCube.transform.position + Vector3.up * 10f;
        var pos2 = pos1 + ((level % 2 == 0) ? Vector3.left : Vector3.forward) * 120;
        if (level % 2 == 0)
        {
            currentCube.transform.position = Vector3.Lerp(pos2, pos1, time);
        }
        else
        {
            currentCube.transform.position = Vector3.Lerp(pos1, pos2, time);
        }
        if (Input.GetMouseButtonDown(0))
        {
            newBlock();
        }
    }
    private void newBlock()
    {
        if (lastCube != null)
        {
            currentCube.transform.position = new Vector3(Mathf.Round(currentCube.transform.position.x),
                currentCube.transform.position.y,
                Mathf.Round(currentCube.transform.position.z));
            currentCube.transform.localScale = new Vector3(lastCube.transform.localScale.x - Mathf.Abs(currentCube.transform.position.x - lastCube.transform.position.x),
                lastCube.transform.localScale.y,
                lastCube.transform.localScale.z - Mathf.Abs(currentCube.transform.position.z - lastCube.transform.position.z)
                );
            currentCube.transform.position = Vector3.Lerp(currentCube.transform.position, lastCube.transform.position, .5f) + Vector3.up * 5f   ;
        }
        var x = currentCube.transform.localScale.x;
        var z = currentCube.transform.localScale.z;
        if (x <= 0 || z <= 0)
        {
            done = true;
            game.SetActive(false);
            finish.SetActive(true);
            return;
        }
        lastCube = currentCube;
        currentCube = Instantiate(lastCube);
        currentCube.name = level + "";
        currentCube.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB((level / 100f) % 1f, 1f, 1f));
        level++;
        Camera.main.transform.position = currentCube.transform.position + Vector3.one * 100f;
        Camera.main.transform.LookAt(currentCube.transform.position);
    }
}