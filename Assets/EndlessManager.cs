using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor;

public class EndlessManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private GameObject checkpointPrefab;

    [SerializeField]
    private CheckpointBehavior firstLevelCheckpoint;

    // Start is called before the first frame update
    void Start()
    {
        // What do we need to do:
        /**
         * 1. Deserialize ALL levels in the saved_levels folder, make list of TileLists.
         * 2. Shuffle list to random order.
         * 3. For each TileList, put into position.
         *      - keep a counter to keep track of which number we're on
         *      - multiply xposition by counter
         * 
         * So, what are the steps for me.
         * 1. Loop through whole levels folder and make TileList list (shuffled).
         * 2. Create function to place a TileList into a specified position (no plan, just wing it).
         */

        gameManager.clearLevelArea();

        // Get directory of TileLists, and create buttons corresponding to each.
        // Put each button in order in list 'buttons'.
        List<TileList> levels = gameManager.deserializeLevelsDirectory("Saved_Levels/");
        //IListExtensions.Shuffle(levels);

        // each loop:
        // put the level into the tilemaps
        // put down the checkpoint for that level (put information into checkpoint for level).
        for(int i = 0; i < levels.Count; i++)
        {
            Debug.Log("putLevel(TileList, " + i + ")");
            gameManager.putLevel(levels[i], i);
            
            // checkpoint position:
            /*
             * 0 = -.25 (this one should be set automatically in the scene)
             * 1 = 35.25
             * 2 = 70.75
             * 
             * So (aside from 0) 35.25 * i will put the level's
             * checkpoint down.
             */
            if(i > 0)
            {
                float value = 35.25f;
                value += 35.50f * (i-1);



                GameObject newCheckpoint = Instantiate(checkpointPrefab, new Vector3(value, 4.75f, 0f), Quaternion.identity);
                newCheckpoint.GetComponent<CheckpointBehavior>().setJumpHeight(levels[i].playerJumpForce);
                newCheckpoint.GetComponent<CheckpointBehavior>().setMoveSpeed(levels[i].playerSpeed);
            } else
            {
                firstLevelCheckpoint.setJumpHeight(levels[i].playerJumpForce);
                firstLevelCheckpoint.setMoveSpeed(levels[i].playerSpeed);
            }
        }

        float value2 = 35.25f;
        value2 += 35.50f * (levels.Count - 1);
        GameObject finalCheckpoint = Instantiate(checkpointPrefab, new Vector3(value2, 4.75f, 0f), Quaternion.identity);
        finalCheckpoint.GetComponent<CheckpointBehavior>().setEndOfLevel(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
