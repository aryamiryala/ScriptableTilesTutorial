using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; 

#if UNITY_EDITOR
using UnityEditor; 
#endif


public class DirtTile : Tile
{
    // array of sprites
    public Sprite[] dirts; 

    //tile base has two specific functions we need to override: RefreshTile and GetTileData

    //determines which Tiles in the vicinity are updated when this Tile is added to the Tilemap, allows us to check what piece the tile is
    public override void RefreshTile(Vector3Int position, ITilemap tilemap){
        // check neighbors of current tiles
        for(int y = -1; y <= 1; y++){
            for(int x = -1; x <= 1; x++){
                //update location
                Vector3Int location = new Vector3Int(position.x + x, position.y + y, position.z);

                // make sure the tile exists 
                if(HasDirtTile(tilemap, location)){
                    //refresh location if it does 
                    tilemap.RefreshTile(location);

                }

            }

        }

    }
    //check to see if the tile exists 
    private bool HasDirtTile(ITilemap tilemap, Vector3Int position){
        return tilemap.GetTile(position) == this; 
    }

    //determines how the tile looks, count how many neighbors it has
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData){
        // add vector to position to check ALL the neighbors
        int mask = HasDirtTile(tilemap, position + new Vector3Int(0, 1, 0)) ? 1 : 0;
        mask += HasDirtTile(tilemap, position + new Vector3Int(1, 0, 0)) ? 2 : 0;
        mask += HasDirtTile(tilemap, position + new Vector3Int(0, -1, 0)) ? 4 : 0;
        mask += HasDirtTile(tilemap, position + new Vector3Int(-1, 0, 0)) ? 8 : 0;

        // determines how many neighbors we have based on the mask
        int index = GetIndex((byte)mask);
        if(index >= 0 && index < dirts.Length){
            tileData.sprite = dirts[index]; 
            tileData.color = Color.white; 
            tileData.flags = TileFlags.LockTransform; 
            tileData.colliderType = ColliderType.None;



        }


    }
    private int GetIndex(byte mask){
        switch(mask){
            case 0: return 0;
            case 6: return 1; 
            case 14: return 2; 
            case 12: return 3; 
            case 7: return 4; 
            case 13: return 5; 
            case 3: return 6; 
            case 11: return 7; 
            case 9: return 8; 
            case 15: return 0; 
        }
        return -1; 
    }
    #if UNITY_EDITOR
    [MenuItem("Assets/DirtTile")]
    public static void CreateDirtTiles(){
        string path = EditorUtility.SaveFilePanelInProject("Save Dirt Tile", "New Dirt Tile", "Asset", "Save Dirt Tile", "Assets");
        if(path == ""){
            return; 
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<DirtTile>(), path);
    }
    #endif

 
 
}

