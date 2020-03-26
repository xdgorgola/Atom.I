using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomPool : MonoBehaviour
{
    /// <summary>
    /// Pool
    /// </summary>
    public static AtomPool Pool { get; private set; }
    /// <summary>
    /// Devuelve un atomo
    /// </summary>
    public static GameObject FreeAtom
    {
        get { return Pool.GetAtom(); }
    }

    /// <summary>
    /// Prefab atomo
    /// </summary>
    public GameObject atomPrefab;
    public int initialAtoms = 30;
    private List<GameObject> atoms;

    private void Awake()
    {
        if (Pool != null && Pool != this)
        {
            Debug.LogWarning("La pool de atomos ya existe! Eliminando este objeto...", gameObject);
            Destroy(gameObject);
        }
        Pool = this;

        atoms = new List<GameObject>(initialAtoms);
        for (int i = 0; i < initialAtoms; i++)
        {
            GameObject atom = Instantiate(atomPrefab);
            atom.SetActive(false);
            atoms.Add(atom);
        }
    }

    /// <summary>
    /// Obtiene un GameObject del prefab Atomo
    /// </summary>
    /// <returns>GameObject prefab Atomo</returns>
    private GameObject GetAtom()
    {
        foreach (GameObject atom in atoms)
        {
            if (!atom.activeInHierarchy)
            {
                return atom;
            }
        }
        GameObject newAtom = Instantiate(atomPrefab);
        atoms.Add(newAtom);
        newAtom.SetActive(false);
        return newAtom;
    }
}
