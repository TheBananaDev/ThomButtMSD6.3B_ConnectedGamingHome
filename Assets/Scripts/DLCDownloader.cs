using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Storage;
using Firebase.Extensions;
using UnityEngine.Networking;

public class DLCDownloader : MonoBehaviour
{
    public UIManager ui;

    private const string url1 = "gs://connectedgamingassignment.appspot.com/Background/background1.jpg";
    private const string url2 = "gs://connectedgamingassignment.appspot.com/Background/background2.jpg";
    private const string url3 = "gs://connectedgamingassignment.appspot.com/Background/background3.png";

    public bool bkg1Purchased;
    public bool bkg2Purchased;
    public bool bkg3Purchased;

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<UIManager>();
        bkg1Purchased = false;
        bkg2Purchased = false;
        bkg3Purchased = false;
    }

    public void DownloadImage(int bckId)
    {
        //Gets the references in firebase storage
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference bkg1Ref = storage.GetReferenceFromUrl(url1);
        StorageReference bkg2Ref = storage.GetReferenceFromUrl(url2);
        StorageReference bkg3Ref = storage.GetReferenceFromUrl(url3);

        const long maxAllowedSize = 1 * 1024 * 1024;
        switch (bckId)
        {
            case 1:
                bkg1Ref.GetBytesAsync(maxAllowedSize).ContinueWithOnMainThread(task1 => {
                    if (task1.IsFaulted || task1.IsCanceled)
                    {
                        Debug.LogException(task1.Exception);
                        // Uh-oh, an error occurred!
                    }
                    else
                    {
                        //Turns the image into a byte stream so it can be processed
                        byte[] fileContents = task1.Result;
                        Debug.Log("Finished downloading!");

                        //Converts the byte stream to a Texture2D component and displays it
                        Texture2D texture = new Texture2D(1920, 1080);
                        texture.LoadImage(fileContents);
                        Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, 1920f, 1080f), new Vector2(0, 0), 100f);
                        ui.background1.sprite = sprite;
                        StartCoroutine(UpdateImages(bckId));
                    }
                });
                break;
            case 2:
                bkg2Ref.GetBytesAsync(maxAllowedSize).ContinueWithOnMainThread(task2 => {
                    if (task2.IsFaulted || task2.IsCanceled)
                    {
                        Debug.LogException(task2.Exception);
                        // Uh-oh, an error occurred!
                    }
                    else
                    {
                        //Turns the image into a byte stream so it can be processed
                        byte[] fileContents = task2.Result;
                        Debug.Log("Finished downloading!");

                        //Converts the byte stream to a Texture2D component and displays it
                        Texture2D texture = new Texture2D(1920, 1080);
                        texture.LoadImage(fileContents);
                        Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, 1920f, 1080f), new Vector2(0, 0), 100f);
                        ui.background2.sprite = sprite;
                        StartCoroutine(UpdateImages(bckId));
                    }
                });
                break;
            case 3:
                bkg3Ref.GetBytesAsync(maxAllowedSize).ContinueWithOnMainThread(task3 => {
                    if (task3.IsFaulted || task3.IsCanceled)
                    {
                        Debug.LogException(task3.Exception);
                        // Uh-oh, an error occurred!
                    }
                    else
                    {
                        //Turns the image into a byte stream so it can be processed
                        byte[] fileContents = task3.Result;
                        Debug.Log("Finished downloading!");

                        //Converts the byte stream to a Texture2D component and displays it
                        Texture2D texture = new Texture2D(1920, 1080);
                        texture.LoadImage(fileContents);
                        Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, 1920f, 1080f), new Vector2(0, 0), 100f);
                        ui.background3.sprite = sprite;
                        StartCoroutine(UpdateImages(bckId));
                    }
                });
                break;
            default:
                Debug.Log("Unsupported background value");
                break;
        }
    }

    private IEnumerator UpdateImages(int bkgID)
    {
        UnityWebRequest request;
        switch (bkgID)
        {
            case 1:
                request = UnityWebRequestTexture.GetTexture(url1);
                yield return request.SendWebRequest();
                if (ui.background1.sprite == null)
                {
                    Debug.Log("Sprite not set");
                    ui.UpdateDLC(bkgID, false);
                }
                else
                {
                    ui.UpdateDLC(bkgID, true);
                    bkg1Purchased = true;
                }
                break;
            case 2:
                request = UnityWebRequestTexture.GetTexture(url2);
                yield return request.SendWebRequest();
                if (ui.background2.sprite == null)
                {
                    Debug.Log("Sprite not set");
                    ui.UpdateDLC(bkgID, false);
                }
                else
                {
                    ui.UpdateDLC(bkgID, true);
                    bkg2Purchased = true;
                }
                break;
            case 3:
                request = UnityWebRequestTexture.GetTexture(url3);
                yield return request.SendWebRequest();
                if (ui.background3.sprite == null)
                {
                    Debug.Log("Sprite not set");
                    ui.UpdateDLC(bkgID, false);
                }
                else
                {
                    ui.UpdateDLC(bkgID, true);
                    bkg3Purchased = true;
                }
                break;
            default:
                Debug.Log("Invalid background update");
                yield return null;
                break;
        }
    }
}
