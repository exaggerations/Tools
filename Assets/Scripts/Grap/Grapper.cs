using System.ComponentModel;
using UnityEngine;

public class Grapper : MonoBehaviour
{
    private GameObject selectObject;
    private Camera _main;

    // Start is called before the first frame update
    void Start()
    {
        _main = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HandleHitOnObject();
        HandleNotHitOnObject();
    }

    private RaycastHit? CastRay()
    {
       Ray ray = _main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Default")))
        {
            return hit;
        }
        return null;
    }

    private void HandleHitOnObject()
    {
        if ((Input.GetMouseButtonDown(0)))
        {
            if(selectObject==null)
            {
                RaycastHit hit =(UnityEngine.RaycastHit)CastRay();

                if (hit.collider != null)
                {
                    if(!hit.collider.CompareTag("Grapable"))
                    {
                        Debug.Log("Object is not grappable.");
                        return;
                    }

                    selectObject = hit.collider.gameObject;
                    Cursor.visible = false;
                }
            }
            else
            {
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectObject.transform.position).z);

                Vector3 worldPos = _main.ScreenToWorldPoint(position);

                selectObject.transform.position = new Vector3(worldPos.x, worldPos.y, worldPos.z);
                selectObject = null;
                Cursor.visible = true;
            }
        }
    }

    private void HandleNotHitOnObject()
    {
        if (selectObject != null)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectObject.transform.position).z);

            Vector3 worldPos = _main.ScreenToWorldPoint(position);

            selectObject.transform.position = new Vector3(worldPos.x, worldPos.y+0.25f , worldPos.z);

            if(Input.GetMouseButtonDown(1))
            {
              selectObject.transform.rotation = Quaternion.Euler(new Vector3(selectObject.transform.eulerAngles.x,selectObject.transform.eulerAngles.y+90f,selectObject.transform.eulerAngles.z));
            }
        }
    }
}
