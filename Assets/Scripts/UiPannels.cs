using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Networking;
using AnotherFileBrowser.Windows;

public class UiPannels : MonoBehaviour
{
    [Header("Panels")]
    public GameObject commandPanel;
    public GameObject cresitsPanel;
    public GameObject customizePetPanel;
    public GameObject[] designs;
    [Header("ImgChangers")]
    string path;
    int imgCho;
    public RawImage[] rawImgMover;
    
 private void Start()
{
    for (int i = 0; i < rawImage.Length; i++)
    {
        string imagePath = PlayerPrefs.GetString($"Img_{i}"); // Recupera la ruta de la imagen guardada en PlayerPrefs usando la clave única
        if (!string.IsNullOrEmpty(imagePath))
        {
            StartCoroutine(LoadImage(imagePath, i)); // Carga la imagen
        }
    }
}
private void Update() {
    if(Input.GetKeyDown(KeyCode.F10))
    {
        PlayerPrefs.DeleteAll();
    }
}
     public RawImage[] rawImage;
    public void ActiveCommandPanel()
    {
        commandPanel.SetActive(true);
    }
    public void QuitCommandPanel()
    {
        commandPanel.SetActive(false);
    }
    public void ActiveCreditsPanel()
    {
        cresitsPanel.SetActive(true);
    }
    public void QuitCreditsPanel()
    {
        cresitsPanel.SetActive(false);
    }
    public void ActivePetPanel()
    {
        customizePetPanel.SetActive(true);
    }
    public void QuitPetPanel()
    {
        customizePetPanel.SetActive(false);
    }
    public void ActiveDesign(int designsInt)
    {
        foreach (GameObject allDes in designs)
        {
            allDes.SetActive(false);
        }
        designs[designsInt].SetActive(true);
    }
    public void InActiveAllDesign()
    {
        foreach (GameObject alldes in designs)
        {
            alldes.SetActive(false);
        }
    }

//File BrowserShit
    public void OpenFileBrowser(int ImgCh)
    {
        imgCho = ImgCh;
        var bp = new BrowserProperties();
        bp.filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
        bp.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            //Load image from local path with UWR
            StartCoroutine(LoadImage(path, imgCho));
        });
    }
    IEnumerator LoadImage(string path, int imgCho)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                 PlayerPrefs.SetString($"Img_{imgCho}", path);
                var uwrTexture = DownloadHandlerTexture.GetContent(uwr);
                rawImage[imgCho].texture = uwrTexture;
                rawImgMover[imgCho].texture = uwrTexture;
            }
        }
    }

}
