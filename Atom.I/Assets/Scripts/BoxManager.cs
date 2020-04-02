using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public static BoxManager Instance { get; private set; }

    public static GameObject Player
    {
        get { return Instance.player; }
    }
    public static GameObject Box
    {
        get { return Instance.box; }
    }
    public static RayBox Raybox
    {
        get { return Instance.rayBox; }
    }
    public static BoxAtomContainer Container
    {
        get { return Instance.container; }
    }
    public static LifeSystem Life
    {
        get { return Instance.life; }
    }

    [SerializeField]
    private GameObject player = null;
    [SerializeField]
    private GameObject box = null;

    private RayBox rayBox;
    private BoxAtomContainer container;
    private LifeSystem life;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Instancia de box manager ya existe, borrando esta...", gameObject);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        if (player == null || box == null)
        {
            Debug.LogWarning("Player o Box nulos!");
            return;
        }

        rayBox = box.GetComponent<RayBox>();
        container = box.GetComponent<BoxAtomContainer>();

        life = player.GetComponent<LifeSystem>();
    }
}
