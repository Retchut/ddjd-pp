using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : Entity
{
    // Scene Object References
    protected EnemyController enemyController;
    protected GameObject player;

    // enemy health par prefab
    [SerializeField]
    private GameObject enemyCanvasPrefab;
    private Vector3 canvasOffsets = new Vector3(0, 1.0f, 0.7f);

    protected int hp;
    private GameObject enemyUI;
    private Slider hpBar;


    [SerializeField]
    private GameObject[] collectables;

    [SerializeField]
    protected float[] collectablesDropChances;

    protected abstract void SetFields();

    // Fetches reference to the player model on the first frame
    void Start()
    {
        Debug.Assert(collectables.Length == collectablesDropChances.Length);
        player = GameObject.FindGameObjectWithTag("PlayerModel");
        SetFields();
        CreateEnemyUI();
    }

    // Sets a reference to the EnemyController script component of the Enemy Controller GameObject
    public void SetEnemyController(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public Vector3 GetCanvasOffsets(){
        return canvasOffsets;
    }

    private void CreateEnemyUI(){
        Vector3 pos = transform.position + canvasOffsets;
        Quaternion rot = Quaternion.Euler(75, 0, 0);
        enemyUI = Instantiate(enemyCanvasPrefab, pos, rot);
        enemyUI.transform.SetParent(transform, true);
        hpBar = enemyUI.transform.GetChild(0).GetComponent<Slider>();
        SetMaxHealth();
    }

    private void SetMaxHealth(){
        hpBar.maxValue = hp;
        hpBar.value = hp;
    }

    private void UpdateHealth(){
        hpBar.value = hp;
    }

    // Input gathering
    void Update()
    {
        moveDirection = GetMoveDirection();
    }

    // Physics Calculations
    void FixedUpdate()
    {
        Move();
    }

    protected override void Die() {
        for (int i = 0; i < collectables.Length; i++) {
            int quantity = (int)collectablesDropChances[i] + (Random.Range(0.0f, 1.0f) < collectablesDropChances[i] - (int)collectablesDropChances[i] ? 1 : 0);
            for (int c = 0; c < quantity; c++)
                Instantiate(collectables[i], transform.position, Quaternion.identity);
        }
        enemyController.RemoveEnemy(gameObject);
    }

    // Handles collisions between this and other object instances
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            hp--;
            UpdateHealth();
            if(hp == 0) Die();
        }
    }
}
