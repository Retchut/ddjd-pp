using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorsController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Transform roomSpawnPoint;

    [SerializeField]
    private GameObject[] rooms;

    [SerializeField]
    private float[] roomDropRate;

    [SerializeField]
    private int doorCost;
    [SerializeField]
    private GameObject doorcanvas;


    // Distance from mid one door to dead center
    private float midDoorDistanceDeadCenter = 4.95f;
    private bool open = false;
    private float corridorLength = 30.0f;


    private Transform door1;
    private Rigidbody door1Rb;

    private Transform door2;
    private Rigidbody door2Rb;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(rooms.Length == roomDropRate.Length);
        Debug.Assert(roomDropRate.Sum() == 1.0f);

        player = GameObject.FindGameObjectWithTag("Player");

        door1 = transform.GetChild(1);
        door1Rb = door1.GetComponent<Rigidbody>();

        door2 = transform.GetChild(2);
        door2Rb = door2.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!open)
        {
            Vector3 doorsDeadCenterOnFloor = new Vector3(transform.position.x, player.transform.position.y, transform.position.z) + transform.forward * midDoorDistanceDeadCenter;
            float distanceToDoor = Vector3.Distance(player.transform.position, doorsDeadCenterOnFloor);

            if (distanceToDoor < 10.0f)
            {
                if (Input.GetKeyDown("e"))
                {
                    if (player.GetComponent<Player>().haveEnoughCoins(doorCost)) {
                        player.GetComponent<Player>().RemoveCoins(doorCost);
                        open = true;
                        OpenDoors();

                        float currentDropRate = 0.0f;
                        float drop = Random.Range(0.0f, 1.0f);
                        for (int i = 0; i < rooms.Length; i++) {
                            currentDropRate += roomDropRate[i];
                            if (drop < currentDropRate) {
                                Instantiate(rooms[i], roomSpawnPoint.position + -roomSpawnPoint.right * corridorLength, Quaternion.Euler(0, roomSpawnPoint.localEulerAngles.y + 135f, 0)).transform.parent = roomSpawnPoint;
                                break;
                            }
                        }
                    }
                }
            }
        }

        // Clamp Door Rotation Values
        door1.localEulerAngles = Vector3.up * Mathf.Clamp(door1.localEulerAngles.y, 0f, 85f);
        door2.localEulerAngles = Vector3.up * Mathf.Clamp(door2.localEulerAngles.y, 95f, 180f);
    }

    void OpenDoors()
    {
        doorcanvas.SetActive(false);
        float toque = 850.0f;
        door1Rb.constraints &= ~RigidbodyConstraints.FreezeRotationY;
        door1Rb.AddTorque(Vector3.up * toque * door1Rb.mass);


        door2Rb.constraints &= ~RigidbodyConstraints.FreezeRotationY;
        door2Rb.AddTorque(Vector3.down * toque * door1Rb.mass);
    }
}
