using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SelectController : MonoBehaviour
{
    public GameObject cube;
    public LayerMask layer;
    public LayerMask layerMask;
    public List<GameObject> players;
    private Camera _cam;
    private GameObject _cubeselection;
    private RaycastHit _hit;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && players.Count>0)
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit agentTarget, 1000f, layer))
            {
                foreach (var element in players)
                    element.GetComponent<NavMeshAgent>().SetDestination(agentTarget.point);
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            foreach (var element in players)
                if (element != null)
                    element.transform.GetChild(0).gameObject.SetActive(false);

            players.Clear();
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out _hit, 1000f, layer))
            {
                _cubeselection = Instantiate(cube, new Vector3(_hit.point.x, 1, _hit.point.z), Quaternion.identity);
            }
        }

        if (_cubeselection)
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitDrag, 1000f, layer))
            {
                float xScale = hitDrag.point.x - _hit.point.x;
                float zScale = hitDrag.point.z - _hit.point.z;

                if (xScale < 0f && zScale < 0f)
                    _cubeselection.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
                else if (xScale < 0f)
                    _cubeselection.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                else if (zScale < 0f)
                    _cubeselection.transform.localRotation = Quaternion.Euler(new Vector3(180, 0, 0));
                else
                    _cubeselection.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                _cubeselection.transform.localScale = new Vector3(Mathf.Abs(xScale), 1, Mathf.Abs(zScale));
            }
        }

        if (Input.GetMouseButtonUp(0) && _cubeselection)
        {
            RaycastHit[] hits = Physics.BoxCastAll(
                _cubeselection.transform.position,
                _cubeselection.transform.localScale,
                Vector3.up,
                Quaternion.identity, 0, layerMask);

            foreach (var element in hits)
            {
                if (element.collider.CompareTag("Enemy")) continue;
                players.Add(element.transform.gameObject);
                element.transform.GetChild(0).gameObject.SetActive(true);
            }
            Destroy(_cubeselection);
        }

    }
}
