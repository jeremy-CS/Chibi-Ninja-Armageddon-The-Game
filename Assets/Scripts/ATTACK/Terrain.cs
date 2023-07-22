using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{

    private List<GameObject> _projectiles;
    private List<Collider2D> _projectileColliders = new List<Collider2D>();
    private Collider2D _collider;
    private GameObject[] _fullTerrain;

    // Start is called before the first frame update
    void Start()
    {
        _collider = gameObject.GetComponent<Collider2D>();
        _fullTerrain = GameObject.FindGameObjectsWithTag("Terrain");
    }

    // Update is called once per frame
    void Update()
    {
        if (_projectileColliders.Count == 0)
        {
            _projectiles = new List<GameObject>(GameObject.FindGameObjectsWithTag("Projectile")); // find active projectiles in scene

            foreach (GameObject projectile in _projectiles)
            {
                _projectileColliders.Add(projectile.GetComponent<Collider2D>()); // get the colliders of the active projectiles
            }
            _projectiles.Clear();
        }

        if (_projectileColliders.Count > 0)
        {
            foreach (Collider2D projectileCollider in _projectileColliders)
            {
                if (projectileCollider != null)
                {
                    if (_collider.bounds.Intersects(projectileCollider.bounds))
                    {
                        if (projectileCollider.gameObject.GetComponent<Projectile>().GetAOE() > 1)
                            DestroySorroundingTiles(projectileCollider);

                        if (projectileCollider.gameObject.GetComponent<Projectile>().GetFollow())
                        {
                            projectileCollider.gameObject.GetComponent<Projectile>().DestroyProjectileRef();
                        }

                        Destroy(projectileCollider.gameObject);
                        Destroy(this.gameObject);
                    }
                }
                else
                {
                    _projectileColliders.Remove(projectileCollider);
                    break;
                }
            }
        }
    }
    
    private void DestroySorroundingTiles(Collider2D projectile)
    {
        int AOE = projectile.gameObject.GetComponent<Projectile>().GetAOE();
        GameObject[] tilesToDestroy = new GameObject[AOE];

        // Iterate through each tile in the scene to find the n closest tiles
        for (int i = 0; i < _fullTerrain.Length; i++)
        {
            if (_fullTerrain[i] != null && _fullTerrain[i].gameObject != this.gameObject)
            {
                int tileToReplace = -1;
                float largestDifference = 0.0f;

                GameObject currentTile = _fullTerrain[i];
                float distanceToTile = Vector3.Distance(transform.position, currentTile.transform.position);

                for (int n = 0; n < AOE; n++)
                {
                    if (tilesToDestroy[n] == null)
                    {
                        tilesToDestroy[n] = currentTile;
                        break;
                    }
                    else
                    {
                        if (distanceToTile < Vector3.Distance(transform.position, tilesToDestroy[n].transform.position))
                        {
                            if (distanceToTile > largestDifference)
                            {
                                tileToReplace = n;
                            }
                            break;
                        }
                    }
                }

                if (tileToReplace != -1)
                {
                    tilesToDestroy[tileToReplace] = currentTile;
                }
            }

            for(int j = 0; j < AOE; j++)
            {
                Destroy(tilesToDestroy[j]);
            }

        }
    }
}
