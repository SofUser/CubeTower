using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionBestLogo : MonoBehaviour
{
        public GameObject[] CollBestLogo;
    void Start()
    {
        if(PlayerPrefs.GetInt("score") >= 5) { 
            CollBestLogo[0].SetActive(false);
            CollBestLogo[7].SetActive(true);

            if(PlayerPrefs.GetInt("score") >= 10) {
                CollBestLogo[1].SetActive(false);
                CollBestLogo[8].SetActive(true);

                if(PlayerPrefs.GetInt("score") >= 20) {
                    CollBestLogo[2].SetActive(false);
                    CollBestLogo[9].SetActive(true);
                
                    if(PlayerPrefs.GetInt("score") >= 30) {
                        CollBestLogo[3].SetActive(false);
                        CollBestLogo[10].SetActive(true);
                    
                        if(PlayerPrefs.GetInt("score") >= 50) {
                            CollBestLogo[4].SetActive(false);
                            CollBestLogo[11].SetActive(true);
                        
                            if(PlayerPrefs.GetInt("score") >= 70) {
                                CollBestLogo[5].SetActive(false);
                                CollBestLogo[12].SetActive(true);
                            
                                if(PlayerPrefs.GetInt("score") >= 100) {
                                    CollBestLogo[6].SetActive(false);
                                    CollBestLogo[13].SetActive(true);
    }   }   }   }   }   }   }   }
}
