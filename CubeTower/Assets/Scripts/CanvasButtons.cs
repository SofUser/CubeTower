using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour
{
    public Sprite musicOn, musicOff;

    public void Start() {
        if(PlayerPrefs.GetString("music") == "No" && gameObject.name == "Music") 
            GetComponent<Image>().sprite = musicOff;
    }
    public void RestartGame() {
        if(PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadVK() {
        if(PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();

        Application.OpenURL("https://vk.com/id183412513");
    }
    public void LoadShop() {
        if(PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();

        SceneManager.LoadScene("Collection");
    }
    public void CloseCollection() {
        if(PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();

        SceneManager.LoadScene("1 Work");
    }
    public void MusicWork() {
        //Сейчас музыка выключена и её нужно включить
        if(PlayerPrefs.GetString("music") == "No") {
            GetComponent<AudioSource>().Play();
            PlayerPrefs.SetString("music", "Yes");
            GetComponent<Image>().sprite = musicOn;
        }
        else {
            PlayerPrefs.SetString("music", "No");
            GetComponent<Image>().sprite = musicOff;
        }
    }
}
