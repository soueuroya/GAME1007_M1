using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridHolderScript : MonoBehaviour
{
    [SerializeField]
    int rows, counterRow;

    [SerializeField]
    int cols, counterCol;

    [SerializeField]
    float tileSize;

    [SerializeField]
    int resourcesCount, resourcesCounter;

    [SerializeField]
    GameObject reference;

    private GameObject[,] grid;

    [SerializeField]
    Color maxColor;

    [SerializeField]
    Color mediumColor;

    [SerializeField]
    Color quarterColor;

    [SerializeField]
    Color minimumColor;

    [SerializeField]
    Color goldColor;

    [SerializeField]
    Color tinColor;

    [SerializeField]
    Color copperColor;

    [SerializeField]
    Animator toogleAnimation;

    bool extractMode;

    [SerializeField]
    Text scansLeft, extractsLeft, goldValue, tinValue, copperValue;

    bool isGeneratingGrid, isGeneratingResources, toDestroy, isInitializingGrid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new GameObject[rows, cols]; // Initialize grid with number of rows and columns
        isInitializingGrid = true;
        isGeneratingGrid = false;
        isGeneratingResources = false;
        toDestroy = false;
    }

    void Update()
    {
        if (isInitializingGrid)
        {
            for (int i = 0; i < rows/2; i++) // Create half of a line of the grid per update, this doesn't slow down too much
            {
                if (counterRow < rows)
                {
                    if (counterCol < cols)
                    {
                        float posX = counterCol * tileSize; // calculate position X based on the size of the tile and COLUMN number
                        float posY = counterRow * -tileSize;// calculate position Y based on the size of the tile and ROW number
                        grid[counterRow, counterCol] = Instantiate(reference, transform);// Instantiate tile in the world using prefab and storing its reference in the grid
                        TileScript ts = grid[counterRow, counterCol].GetComponent<TileScript>();
                        grid[counterRow, counterCol].transform.localPosition = new Vector2(posX, posY);// moving tile in the world using positions calculated
                        ts.x = counterRow;// storing the position X in the tile for later reference when clicked
                        ts.y = counterCol;// storing the position Y in the tile for later reference when clicked
                        ts.ghs = this;// referencing this script to the new tile for later reference when clicked
                        ts.gameObject.SetActive(false);
                        ts.value.text = Random.Range(250, 312).ToString();
                        counterCol++;
                    }
                    else
                    {
                        counterCol = 0;
                        counterRow++;
                    }
                }
                else
                {
                    isInitializingGrid = false;
                }
            }
        }
        else if (isGeneratingGrid)
        {
            for (int i = 0; i < rows / 2; i++) // Create half of a line of the grid per update, this doesn't slow down too much
            {
                if (counterRow < rows)
                {
                    if (counterCol < cols)
                    {
                        TileScript ts = grid[counterRow, counterCol].GetComponent<TileScript>();
                        ts.gameObject.SetActive(true);
                        int r = Random.Range(0, 10); // generating random number 0 - 10
                        if (r > 7)      // ~0.3%
                        {
                            ts.value.color = goldColor;   // setting label as GOLD color
                        }
                        else if (r > 3) // ~0.4%
                        {
                            ts.value.color = tinColor;    // setting label as TIN color
                        }
                        else            // ~0.3%
                        {
                            ts.value.color = copperColor; // setting label as COPPER color
                        }
                        counterCol++;
                    }
                    else
                    {
                        counterCol = 0;
                        counterRow++;
                    }
                }
                else
                {
                    isGeneratingGrid = false;
                    resourcesCounter = 0;
                    isGeneratingResources = true;
                }
            }
        }
        else if (isGeneratingResources)
        {
            //Generate resources using resourcesCount as maximum.\
            resourcesCounter++;
            if (resourcesCounter < resourcesCount)
            {
                GenerateSpecialResources();
            }
            else
            {
                isGeneratingResources = false;
            }
        }
        else if (toDestroy)
        {
            for (int i = 0; i < rows / 2; i++) // hide half of a line of the grid per update, this doesn't slow down too much the FPS
            {
                if (counterRow < rows)
                {
                    if (counterCol < cols)
                    {
                        TileScript ts = grid[counterRow, counterCol].GetComponent<TileScript>();
                        grid[counterRow, counterCol].GetComponent<Image>().color = minimumColor;
                        grid[counterRow, counterCol].gameObject.SetActive(false);
                        ts.value.gameObject.SetActive(false); // turn off label on the area
                        ts.value.text = Random.Range(250, 312).ToString();
                        counterCol++;
                    }
                    else
                    {
                        counterCol = 0;
                        counterRow++;
                    }
                }
                else
                {
                    toDestroy = false;
                }
            }
        }
    }

    public void ReGenerate()
    {
        //Initialize at extract mode
        extractMode = true;
        toogleAnimation.ResetTrigger("Scan");
        toogleAnimation.SetTrigger("Extract");

        //Set extract and scan times
        extractsLeft.text = "x3";
        scansLeft.text = "x6";

        //Reset counters
        counterRow = 0;
        counterCol = 0;

        //Flag for update to generate grid
        isGeneratingGrid = true;
    }

    public void DestroyGrid()
    {
        counterRow = 0;
        counterCol = 0;
        toDestroy = true;
    }

    public void ScanSelected() // Turn off extract mode
    {
        extractMode = false;
    }

    public void ExtractSelected() // Turn on extract mode
    {
        extractMode = true;
    }

    public void TileSelected(int x, int y) // When tile is selected (Recieves position X and Y)
    {
        if (extractMode) // If in extract mode
        {
            int extracts = int.Parse(extractsLeft.text.Substring(1));   // Using label to keep track of extracts left
            if (extracts > 0)                                           // If we have any extract available
            {
                extracts--;                                             // remove one extract
                extractsLeft.text = "x" + extracts;                     // update label
                ExtractTile(x, y);                                      // extracting selected tile
            }
        }
        else                                                            // If in scan mode
        {
            int scans = int.Parse(scansLeft.text.Substring(1));         // Using label to keep track of scans left
            if (scans > 0)                                              // If we have any scan available
            {
                if (!grid[x, y].GetComponent<TileScript>().value.gameObject.activeSelf) // if the area is not already scanned
                {
                    scans--;                                                // remove one scan
                    scansLeft.text = "x" + scans;                           // update label
                    ScanTile(x, y);                                         // scan selected tile
                }
            }
        }
    }
    private void ExtractTile(int x, int y)                              // start extraction of tile
    {
        for (int i = x-1; i < x+2; i++)                                 //the area of extraction starts from x - 1 and goes to x + 2 (excluded)
        {
            for (int j = y-1; j < y+2; j++)                             //the area of extraction starts from y - 1 and goes to y + 2 (excluded)
            {
                if (i >= 0 && j >= 0 && i < rows && j < cols)           //If the area is valid
                {
                    int ammount;
                    if (grid[i, j].GetComponent<Image>().color == maxColor)                 // If we had a MAX color area
                    {
                        grid[i, j].GetComponent<Image>().color = mediumColor;               // we will set the area as MEDIUM
                        ammount = (int.Parse(grid[i, j].GetComponent<TileScript>().value.text)) / 2;  // set the ammount to increase inventory
                        grid[i, j].GetComponent<TileScript>().value.text = ammount.ToString();          // set the label as 1000
                    }
                    else if (grid[i, j].GetComponent<Image>().color == mediumColor)         // If we had a MEDIUM color area
                    {
                        grid[i, j].GetComponent<Image>().color = quarterColor;              // we will set the area as QUARTER
                        ammount = (int.Parse(grid[i, j].GetComponent<TileScript>().value.text)) / 2;  // set the ammount to increase inventory
                        grid[i, j].GetComponent<TileScript>().value.text = ammount.ToString();          // set the label as 1000
                    }
                    else if (grid[i, j].GetComponent<Image>().color == quarterColor)        // If we had a QUARTER color area
                    {
                        grid[i, j].GetComponent<Image>().color = minimumColor;              // we will set the area as MINIMUM
                        ammount = (int.Parse(grid[i, j].GetComponent<TileScript>().value.text)) / 2;  // set the ammount to increase inventory
                        grid[i, j].GetComponent<TileScript>().value.text = ammount.ToString();          // set the label as 1000
                    }
                    else
                    {
                        ammount = int.Parse(grid[i, j].GetComponent<TileScript>().value.text);
                    }

                    if (grid[i, j].GetComponent<TileScript>().value.color == goldColor) // If the label is GOLD color
                    {
                        goldValue.text = (int.Parse(goldValue.text) + ammount).ToString(); // increase the GOLD
                    }
                    else if (grid[i, j].GetComponent<TileScript>().value.color == tinColor) // if the label is TIN color
                    {
                        tinValue.text = (int.Parse(tinValue.text) + ammount).ToString();       // increase the TIN
                    }
                    else
                    {
                        copperValue.text = (int.Parse(copperValue.text) + ammount).ToString(); // increase COPPER
                    }
                }
            }
        }
    }

    private void ScanTile(int x, int y) // scan selected tile (receives position x and y)
    {
        for (int i = x - 1; i < x + 2; i++)                  //the area of scan starts from x - 1 and goes to x + 2 (excluded)
        {
            for (int j = y - 1; j < y + 2; j++)              //the area of scan starts from y - 1 and goes to y + 2 (excluded)
            {
                if (i >= 0 && j >= 0 && i < rows && j < cols)//If the area is valid
                {
                    grid[i, j].GetComponent<TileScript>().value.gameObject.SetActive(true); // turn on label on the area
                }
            }
        }
    }

    private void GenerateSpecialResources() //  Generate one resource
    {
        int x = Random.Range(0, rows); // random row
        int y = Random.Range(0, cols); // random column

        if (grid[x, y].GetComponent<Image>().color == maxColor          //random area cannot override MAX resource
          //|| grid[x, y].GetComponent<Image>().color == mediumColor    //random area cannot override MEDIUM resource
          //|| grid[x, y].GetComponent<Image>().color == quarterColor  //random area cannot override QUARTER resource
        )
        {
            GenerateSpecialResources();                          // If cannot place resource, call the function again
        }
        else
        {
            for (int i = x - 2; i < x + 3; i++)                  //the area of resources starts from x - 2 and goes to x + 3 (excluded)
            {
                for (int j = y - 2; j < y + 3; j++)              //the area of scan starts from y - 2 and goes to y + 3 (excluded)
                {
                    if (i >= 0 && j >= 0 && i < rows && j < cols)//If the area is valid
                    {
                        if ((i == x - 2) || (j == y - 2) || (i == x + 2) || (j == y + 2)) // if distance from selected is 2
                        {
                            if (grid[i, j].GetComponent<Image>().color != maxColor && grid[i, j].GetComponent<Image>().color != mediumColor)// cannot override max ou medium spots
                            {
                                grid[i, j].GetComponent<Image>().color = quarterColor; // MAX COLOR at selected position
                                grid[i, j].GetComponent<TileScript>().value.text = Random.Range(500, 625).ToString();
                            }
                        }
                        else if ((i == x - 1) || (j == y - 1) || (i == x + 1) || (j == y + 1)) // if distance from selected is 1
                        {
                            if (grid[i, j].GetComponent<Image>().color != maxColor) // cannot override max spots
                            {
                                grid[i, j].GetComponent<Image>().color = mediumColor; // MAX COLOR at selected position
                                grid[i, j].GetComponent<TileScript>().value.text = Random.Range(1000, 1250).ToString();
                            }
                        }
                        else // spot selected
                        {
                            grid[i, j].GetComponent<Image>().color = maxColor; // MAX COLOR at selected position
                            grid[i, j].GetComponent<TileScript>().value.text = Random.Range(2000, 2500).ToString();
                        }
                    }
                }
            }
        }
    }
}

