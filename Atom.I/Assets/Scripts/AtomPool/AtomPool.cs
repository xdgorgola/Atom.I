using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Atoms { Hyper, Anti, Atom }
public class AtomPool : MonoBehaviour
{
    /// <summary>
    /// Pool
    /// </summary>
    public static AtomPool Pool { get; private set; }

    /// <summary>
    /// Prefab atomo
    /// </summary>
    public GameObject atomPrefab;
    public GameObject antiAtomPrefab;
    public GameObject hyperAtomPrefab;
    public int initialAtoms = 30;
    public int initialAnti = 20;
    public int initialHyper = 10;
    private List<GameObject> atoms;
    private List<GameObject> antiAtoms;
    private List<GameObject> hyperAtoms;

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

        antiAtoms = new List<GameObject>();
        for (int i = 0; i < initialAnti; i++)
        {
            GameObject anti = Instantiate(antiAtomPrefab);
            anti.SetActive(false);
            antiAtoms.Add(anti);
        }

        hyperAtoms = new List<GameObject>(initialHyper);
        for (int i = 0; i < initialHyper; i++)
        {
            GameObject hyper = Instantiate(hyperAtomPrefab);
            hyper.SetActive(false);
            hyperAtoms.Add(hyper);
        }
    }

    /// <summary>
    /// Obtiene un GameObject del prefab Atomo
    /// </summary>
    /// <returns>GameObject prefab Atomo</returns>
    public GameObject GetAtom(Atoms kind)
    {
        List<GameObject> lista;
        switch (kind)
        {
            case Atoms.Anti:
                lista = antiAtoms;
                break;
            case Atoms.Atom:
                lista = atoms;
                break;
            case Atoms.Hyper:
                lista = hyperAtoms;
                break;
            default:
                return null;
        }
        foreach (GameObject atom in lista)
        {
            if (!atom.activeInHierarchy)
            {
                return atom;
            }
        }
        GameObject prefab;
        switch (kind)
        {
            case Atoms.Anti:
                prefab = antiAtomPrefab;
                break;
            case Atoms.Atom:
                prefab = atomPrefab;
                break;
            case Atoms.Hyper:
                prefab = hyperAtomPrefab;
                break;
            default:
                return null;
        }
        GameObject newAtom = Instantiate(prefab);
        lista.Add(newAtom);
        newAtom.SetActive(false);
        return newAtom;
    }
}
