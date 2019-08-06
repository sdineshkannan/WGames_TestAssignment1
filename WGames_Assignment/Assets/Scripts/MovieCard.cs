using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MovieCard : MonoBehaviour
{
    [SerializeField] RawImage posterImage;
    [SerializeField] Text title;
    [SerializeField] Text type;

    public MovieResultModelClass.MovieDetail movieDetailObj { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        SetMovieDetails();
    }

    void SetMovieDetails()
    {
        if(movieDetailObj != null)
        {
            string titleText = string.Format("{0} ({1})", movieDetailObj.Title, movieDetailObj.Year);
            title.text = titleText;
            type.text = movieDetailObj.Type;

            StartCoroutine(DownloadPoster());
        }
    }

    IEnumerator DownloadPoster()
    {
        string url = movieDetailObj.Poster;

        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError || request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                posterImage.texture = texture;
            }
        }
    }

    public void OnCardClick()
    {
        if(movieDetailObj != null)
        {
            Debug.Log("IMDB ID is " + movieDetailObj.imdbID);
        }
    }
}
