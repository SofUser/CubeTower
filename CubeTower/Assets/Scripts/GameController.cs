using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    private int prevCountMaxHorizontal, maxX = 0, maxY = 0, maxZ = 0, maxHor = 0, Prov;
    public int maxVer = 0;
    private float camMoveToYPosition, camMoveSpeed = 2f;
    public float cubeChangePlaceSpeed = 0.5f;
    public bool AnimScoreStart;
    private bool IsLose, firstCube, NumbPos;
    public GameObject allCubes, restartButton, vfx, vfxLevelUp, Score; 
    public GameObject[] cubesToCreate;
    public GameObject[] canvasStartPage;
    private Transform mainCam;
    public Transform cubeToPlace;
    private Color toCameraColor;
    public Color[] bgColors;
    private Coroutine showCubePlace;
    private Rigidbody allCubesRb;
    private CubePos nowCube = new CubePos(0, 1, 0);
    public Text scoreTxt;
    public AudioSource Stars, PlusCube;
    private List<Vector3> allCubesPositions = new List<Vector3> {
        new Vector3(0, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector3(0, -1, 0),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, -1),
        new Vector3(1, 0, 1),
        new Vector3(-1, 0, 1),
        new Vector3(1, 0, -1),
        new Vector3(-1, 0, -1),
    };
    private List<GameObject> posibleCubesToCreate = new List<GameObject>();
    

    private void Start() {
        if (PlayerPrefs.GetInt("score") < 5)
            posibleCubesToCreate.Add(cubesToCreate[0]);
        else if (PlayerPrefs.GetInt("score") < 10)
            AddPossibleCubes(2);
        else if (PlayerPrefs.GetInt("score") < 20)
            AddPossibleCubes(3);
        else if (PlayerPrefs.GetInt("score") < 30)
            AddPossibleCubes(4);
        else if (PlayerPrefs.GetInt("score") < 50)
            AddPossibleCubes(5);
        else if (PlayerPrefs.GetInt("score") < 70)
            AddPossibleCubes(6);
        else if (PlayerPrefs.GetInt("score") < 100)
            AddPossibleCubes(7);
        else AddPossibleCubes(10);

        scoreTxt.text = "<size=60>Best: </size>" + PlayerPrefs.GetInt("score") + "\n<size=60>New: </size>1";
        toCameraColor = Camera.main.backgroundColor;
        mainCam = Camera.main.transform;
        camMoveToYPosition = 5f + nowCube.y - 1f;

        allCubesRb = allCubes.GetComponent<Rigidbody>();
        showCubePlace = StartCoroutine(ShowCubePlace());

        
    } 

    private void Update() {
        if((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && cubeToPlace != null && allCubes != null && !EventSystem.current.IsPointerOverGameObject()) {
#if !UNITY_EDITOR
            if(Input.GetTouch(0).phase != TouchPhase.Began)
                return;
#endif

            if (!firstCube) {
                firstCube = true;
                foreach(GameObject obj in canvasStartPage)
                    Destroy(obj);
            }

            GameObject createCube = null;
            if(posibleCubesToCreate.Count == 1)
                createCube = posibleCubesToCreate[0];
            else
                createCube = posibleCubesToCreate[UnityEngine.Random.Range(0, posibleCubesToCreate.Count)];

            GameObject newCube = Instantiate(
                createCube,
                cubeToPlace.position,
                Quaternion.identity) as GameObject;

            newCube.transform.SetParent(allCubes.transform);
            nowCube.setVector(cubeToPlace.position);
            allCubesPositions.Add(nowCube.getVector());
            Score.SetActive(true);

            Instantiate (vfx, newCube.transform.position, Quaternion.identity);

            AnimScoreStart = true;
            
            allCubesRb.isKinematic = true;
            allCubesRb.isKinematic = false;

            SpawnPositions();
            MoveCameraChangeBg();
        }

        mainCam.localPosition = Vector3.MoveTowards(mainCam.localPosition,
            new Vector3(mainCam.localPosition.x, camMoveToYPosition, mainCam.localPosition.z),
            camMoveSpeed * Time.deltaTime);

        if(Camera.main.backgroundColor != toCameraColor)
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, toCameraColor, Time.deltaTime / 1.5f);
        
        if (!IsLose && allCubesRb.velocity.magnitude > 0.06f) {
            restartButton.SetActive(true);
            Destroy(cubeToPlace.gameObject);
            StopCoroutine(showCubePlace);
            mainCam.localPosition += new Vector3(0f, 0f, -maxVer);
            IsLose = true;
        } 
    }

    IEnumerator ShowCubePlace() {
        while(true) {
            SpawnPositions();

            yield return new WaitForSeconds(cubeChangePlaceSpeed);
        }
    }
    private void SpawnPositions() {
        List<Vector3> positions = new List<Vector3>();
        if(IsPositionEmpty(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z)) && nowCube.x + 1 != cubeToPlace.position.x) {
            positions.Add(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z));
            NumbPos = true;
        }
        if(IsPositionEmpty(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z)) && nowCube.x - 1 != cubeToPlace.position.x) {
            positions.Add(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z));
            NumbPos = true;
        }
        if(IsPositionEmpty(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z)) && nowCube.y + 1 != cubeToPlace.position.y) {
            positions.Add(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z));
            NumbPos = true;
        }
        if(IsPositionEmpty(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z)) && nowCube.y - 1 != cubeToPlace.position.y) {
            positions.Add(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z));
            NumbPos = true;
        }
        if(IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1)) && nowCube.z + 1 != cubeToPlace.position.z) {
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1));
            NumbPos = true;
        }
        if(IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1)) && nowCube.z - 1 != cubeToPlace.position.z) {
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1));
            NumbPos = true;
        }

        if (NumbPos == true && positions.Count > 1) {
            cubeToPlace.position = positions[UnityEngine.Random.Range(0 , positions.Count)];
            NumbPos = false;  
        }
        else if (NumbPos == false) {
            IsLose = true;
            Destroy(cubeToPlace.gameObject);
            StopCoroutine(showCubePlace);
            restartButton.SetActive(true);
            Score.SetActive(false);
            mainCam.localPosition += new Vector3(0f, 0f, -maxVer);
        }
        else {
            cubeToPlace.position = positions[0];
            NumbPos = false;
            }
        
    }
    private bool IsPositionEmpty(Vector3 targetPos) {
        if(targetPos.y == 0)
            return false;

        foreach(Vector3 pos in allCubesPositions) {
            if(pos.x == targetPos.x && pos.y == targetPos.y && pos.z == targetPos.z)
            return false;
        }
        return true;
    }

    private void MoveCameraChangeBg() {
        

        foreach(Vector3 pos in allCubesPositions) {
            if (Mathf.Abs(Convert.ToInt32(pos.x)) > maxX)
                maxX = Convert.ToInt32(pos.x);

            if (Convert.ToInt32(pos.y) > maxY) {
                maxY = Convert.ToInt32(pos.y);
                maxVer++;
            }

            if (Mathf.Abs(Convert.ToInt32(pos.z)) > maxZ)
                maxZ = Convert.ToInt32(pos.z);
        }

        if(PlayerPrefs.GetInt("score") < maxY)
            PlayerPrefs.SetInt("score", maxY);

        scoreTxt.text = "<size=60>Best: </size>" + PlayerPrefs.GetInt("score") + "\n<size=60>New: </size>" + maxY;

        camMoveToYPosition = 5f + nowCube.y - 1f;

        maxHor = maxX > maxZ ? maxX : maxZ;
        if (maxHor != 1) {
            if(maxHor % 1 == 0 && prevCountMaxHorizontal != maxHor) {
                mainCam.localPosition += new Vector3(0, 0, -1f);
                prevCountMaxHorizontal = maxHor;
            }
        }

        if(maxY >= 100) { 
            toCameraColor = bgColors[6];
        }
        else if(maxY >= 70) {
            toCameraColor = bgColors[5];
        }
        else if(maxY >= 50) {
            toCameraColor = bgColors[4];
        }
        else if(maxY >= 30) {
            toCameraColor = bgColors[3];
        }
        else if(maxY >= 20) {
            toCameraColor = bgColors[2];
        }  
        else if(maxY >= 10) {
            toCameraColor = bgColors[1];
        }
        else if(maxY >= 5) {
            toCameraColor = bgColors[0];
        }
        
        if ((maxY == 5 && Prov == 0) || (maxY == 10 && Prov == 1) || (maxY == 20 && Prov == 2) || (maxY == 30 && Prov == 3) || (maxY == 50 && Prov == 4) || (maxY == 70 && Prov == 5) || (maxY == 100 && Prov == 6)) {
            Instantiate (vfxLevelUp, cubeToPlace.transform.position, Quaternion.identity);
            Prov++;
            if(PlayerPrefs.GetString("music") != "No") 
                Stars.Play();    
        }
        else {
            if(PlayerPrefs.GetString("music") != "No") 
                PlusCube.Play();
        } 
        
    }
    private void AddPossibleCubes(int till) {
        for(int i = 0; i < till; i++)
            posibleCubesToCreate.Add(cubesToCreate[i]);
    }
}

struct CubePos {
    public int x, y, z;

    public CubePos (int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 getVector() {
        return new Vector3(x, y, z);
    }
    public void setVector(Vector3 pos) {
        x = Convert.ToInt32(pos.x);
        y = Convert.ToInt32(pos.y);
        z = Convert.ToInt32(pos.z);
    }
}