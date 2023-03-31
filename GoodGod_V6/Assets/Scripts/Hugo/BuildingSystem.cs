using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class BuildingSystem : MonoBehaviour
{

    public static BuildingSystem current;

    public TempleScript templeScript;

    public GridLayout gridLayout;

    public Tilemap MainTileMap;
    public Tilemap TempTilemap;

    private static Dictionary<tile_type,TileBase> tile_bases = new Dictionary<tile_type,TileBase>();

    private BuildingScript next_building;

    private Vector3 previous_pos;
    private BoundsInt previous_area;


    public GameObject WoodCutterCamp_prefab;
    public GameObject GarthererCamp_prefab;

    private bool test_building_type_switch = false;

    private void Test_before_instanciate(GameObject GB_to_place)
    {

        if (next_building != null)
        {
            Destroy(next_building.gameObject);
            next_building = null;
        }

        next_building = Instantiate(GB_to_place,Vector3.zero,Quaternion.identity).GetComponent<BuildingScript>();
        follow_buiding();

    }

    private void follow_buiding()
    {

        ClearArea();

        next_building.area.position = gridLayout.WorldToCell(next_building.gameObject.transform.position);
        next_building.area.position -= new Vector3Int ((int)Mathf.Floor(next_building.area.size.x * 0.5f), (int)Mathf.Floor(next_building.area.size.y * 0.5f), 0);
        BoundsInt building_area = next_building.area;

        Debug.Log(next_building.area.position);


        TileBase[] base_array = GetTilesBlock(building_area, MainTileMap);

        int size = base_array.Length;
        TileBase[]tile_array = new TileBase[size];
        
        for(int i = 0; i< base_array.Length; i++)
        {
            if (base_array[i] == tile_bases[tile_type.White])
            {
                //Debug.Log("white");
                tile_array[i] = tile_bases[tile_type.Green];
            }
            else
            {
                //Debug.Log("no");
                FillTiles(tile_array, tile_type.Red);
                break;
            }
        }
        TempTilemap.SetTilesBlock(building_area, tile_array);
        //Debug.Log(tilemap.GetTilesBlock(building_area)[0] == tile_bases[tile_type.White]);
        //Set_Tiles_Block(building_area, tile_type.Green, tilemap);
        previous_area = building_area;

    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, TempTilemap);

        foreach(var b in baseArray)
        {

            if(b != tile_bases[tile_type.Green])
            {

                return false;
            }

        }

        return true;

    }

    public void TakeArea(BoundsInt area)
    {

        Set_Tiles_Block(area, tile_type.Orange, MainTileMap);

    }


    private void Awake()
    {
        current = this;


    }


    void Start()
    {
        string tile_path = @"Tiles\";
        tile_bases.Add(tile_type.Empty, null);
        tile_bases.Add(tile_type.White, Resources.Load<TileBase>(path:tile_path + "White"));
        tile_bases.Add(tile_type.Green, Resources.Load<TileBase>(path: tile_path + "Green"));
        tile_bases.Add(tile_type.Orange, Resources.Load<TileBase>(path: tile_path + "Orange"));
        tile_bases.Add(tile_type.Red, Resources.Load<TileBase>(path: tile_path + "Red"));


        Test_before_instanciate(WoodCutterCamp_prefab);


    }


    void Update()
    {
        
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3Int cellpos = new Vector3Int();
        if (Physics.Raycast(ray, out hit, 100))
        {
            cellpos = gridLayout.LocalToCell(hit.point);

            //print("hitground");

            //print(cellpos);


            if (previous_pos != cellpos)
            {
                next_building.transform.localPosition = gridLayout.CellToLocalInterpolated(cellpos + new Vector3(0.5f, 0.5f, 0f));
                previous_pos = cellpos;
                follow_buiding();
            }

        }
        


        if (Input.GetMouseButtonDown(0))
        {

            /*
            if (CanTakeArea(next_building.area))
            {
                if (test_building_type_switch == false)
                {
                    templeScript._spawn_building(next_building.transform.localPosition, "WoodcutterCamp");
                    TakeArea(next_building.area);
                }
                else
                {
                    templeScript._spawn_building(next_building.transform.localPosition, "GarthererCamp");
                    TakeArea(next_building.area);
                }
            }
            */



        }

        if(Input.GetKeyDown("s"))
        {
            test_building_type_switch = !test_building_type_switch;

            if(test_building_type_switch)Test_before_instanciate(GarthererCamp_prefab);
            else Test_before_instanciate(WoodCutterCamp_prefab);

            follow_buiding();

        }

        

    }

    public enum tile_type
    {
        
        Empty,
        White,
        Green,
        Orange,
        Red

    }


    private void ClearArea()
    {
        TileBase[] to_clear = new TileBase[previous_area.size.x * previous_area.size.y * previous_area.size.z];
        FillTiles(to_clear, tile_type.Empty);

        //Set_Tiles_Block(previous_area, tile_type.White, tilemap);
        TempTilemap.SetTilesBlock(previous_area, to_clear);

    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {

        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach(var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, z:0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;

    }

    private static void Set_Tiles_Block(BoundsInt area, tile_type type, Tilemap tilemap)
    {

        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tile_array = new TileBase[size];
        FillTiles(tile_array, type);
        tilemap.SetTilesBlock(area, tile_array);
    }

    private static void FillTiles(TileBase[]arr, tile_type type)
    {

        for (int i = 0; i < arr.Length;i++)
        {
            arr[i] = tile_bases[type];
        }

    }





}
