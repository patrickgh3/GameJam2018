using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPCSpawner : MonoBehaviour {

    public float spawnTimer;
    public float spawnThreshHold;
    public int direction = (int)Directions.RIGHT;
    public Vector2[] directions = new Vector2[8];
    public string directionString; 
    public int blocked = 0;
    public float spaceRequirement;
    public float spriteSize;
    public GameObject NPCPrefab;
    private string[] collisionLayers = { "Player" };

    // Use this for initialization
    void Start () {
        populateDirections();
        spawnTimer = 0;
        direction = (int)Enum.Parse(typeof(Directions), directionString);
    }

    // Update is called once per frame
    void Update () {
        checkSpawn();
        spawnTimer += Time.deltaTime;
	}

    void checkSpawn()
    {
        Vector2 perpendicular = (Quaternion.Euler(0, 0, 90) * directions[direction]) * spriteSize;
        Debug.DrawRay(transform.position, directions[direction] * (spaceRequirement), Color.red);
        Debug.DrawRay(transform.position - (new Vector3(perpendicular.x, perpendicular.y, transform.position.z)), directions[direction] * (spaceRequirement), Color.red);
        Debug.DrawRay(transform.position + (new Vector3(perpendicular.x, perpendicular.y, transform.position.z)), directions[direction] * (spaceRequirement), Color.red);
        if (spawnTimer > spawnThreshHold)
        {
            spawnTimer = 0;
            if (Physics2D.Raycast(transform.position, directions[direction], spaceRequirement, LayerMask.GetMask(collisionLayers)).collider == null &&
                Physics2D.Raycast(transform.position - (new Vector3(perpendicular.x, perpendicular.y, transform.position.z)), directions[direction],
                    spaceRequirement, LayerMask.GetMask(collisionLayers)).collider == null &&
                Physics2D.Raycast(transform.position - (new Vector3(perpendicular.x, perpendicular.y, transform.position.z)), directions[direction],
                    spaceRequirement, LayerMask.GetMask(collisionLayers)).collider == null)
            {
                GameObject nextNPC = Instantiate(NPCPrefab, transform.position, Quaternion.identity);
                nextNPC.GetComponent<NPC>().direction = direction;
            }
            else
            {
                blocked++;
            }
        }
    }

    void populateDirections()
    {
        directions[(int)Directions.DOWN] = new Vector2(0, -1).normalized;
        directions[(int)Directions.LEFT_DOWN] = new Vector2(-1, -1).normalized;
        directions[(int)Directions.LEFT_UP] = new Vector2(-1, 1).normalized;
        directions[(int)Directions.LEFT] = new Vector2(-1, 0).normalized;
        directions[(int)Directions.RIGHT_DOWN] = new Vector2(1, -1).normalized;
        directions[(int)Directions.RIGHT_UP] = new Vector2(1, 1).normalized;
        directions[(int)Directions.RIGHT] = new Vector2(1, 0).normalized;
        directions[(int)Directions.UP] = new Vector2(0, 1).normalized;
    }
}
