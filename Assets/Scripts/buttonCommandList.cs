using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class buttonCommandList : MonoBehaviour
{
    [SerializeField] public GameObject TurkishButton, EnglishButton;
    [SerializeField] Slider volumeSlider;
    [SerializeField] TextMeshProUGUI text;
    public string lang = "English";
    public bool volx = true;

    void Start()
    {
        if(PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextScene()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void Language()
    {
        if(TurkishButton.activeSelf)
        {
            TurkishButton.gameObject.SetActive(false);
            EnglishButton.gameObject.SetActive(true);
            lang = "English";
        }
        else
        {
            TurkishButton.gameObject.SetActive(true);
            EnglishButton.gameObject.SetActive(false);
            lang = "Turkish";
        }
    }

    public void changeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
        text.text = ((int)(PlayerPrefs.GetFloat("musicVolume")*100)).ToString();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
 