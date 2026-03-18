using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    Puzzle,
    Searchable,
    Weapon,
    Escape,
    Power,

}
public class InteractiveTile : MonoBehaviour
{
    //makes tiles interactive
    [SerializeField] private Renderer rend;
    [SerializeField] private Collider tileCollider;
    [SerializeField] private GameObject player;

    [SerializeField] public TileType tileType;
    private Color interactiveColor = Color.magenta;

    void Start()
    {

        if (tileCollider == null)
        {
            tileCollider = GetComponent<Collider>();
            //tileCollider.isTrigger = true;
        }

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        if (rend == null)
        {
            rend = GetComponent<Renderer>();
        }

        TileSelector tile = GetComponent<TileSelector>();

        if (tile != null)
        {
            switch (tileType)
            {
                case TileType.Puzzle:
                    interactiveColor = Color.white;
                    break;
                case TileType.Searchable:
                    interactiveColor = Color.yellow;
                    break;
                case TileType.Weapon:
                    interactiveColor = Color.red;
                    break;
                case TileType.Escape:
                    interactiveColor = Color.green;
                    break;
                case TileType.Power:
                    interactiveColor = Color.blue;
                    break;
            }

            tile.Highlight(interactiveColor);
        }
    }
}
