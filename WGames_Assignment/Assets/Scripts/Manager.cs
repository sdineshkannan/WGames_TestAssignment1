using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Manager : MonoBehaviour
{

    [SerializeField] InputField searchInputField;
    [SerializeField] Transform scrollContentTransform;

    private const string movieCardPath = "Prefabs/MovieCard";
    private const string baseURL = "http://www.omdbapi.com/?apikey=6367da97&s=";

    private List<GameObject> movieCardsList = new List<GameObject>();
    private string prevSearchStr = string.Empty;

    public void OnSearchBtnClick()
    {
        string inputStr = searchInputField.text;

        if(!string.IsNullOrEmpty(inputStr) &&  !string.Equals(inputStr, prevSearchStr))
        {
            prevSearchStr = inputStr;
            StartCoroutine(QueryServerForMovieDetails(searchInputField.text));

            ClearCurrentMovieCards();
        }
            
    }

    void ClearCurrentMovieCards()
    {
        if(movieCardsList.Count > 0)
        {
            foreach (GameObject obj in movieCardsList)
            {
                Destroy(obj);
            }

            movieCardsList.Clear();
        }
    }

    IEnumerator QueryServerForMovieDetails(string _movieName)
    {
        string url = baseURL + _movieName;

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError || request.isNetworkError)
                Debug.LogError("Something went wrong with the connection "+ request.error);

            else if(request.isDone)
            {
                string response = request.downloadHandler.text;
                Debug.Log("response is " + response);

                MovieResultModelClass movieResults = ParseServerResponse(response);
                PopulateMovieCards(movieResults);
            }
        }
    }

    MovieResultModelClass ParseServerResponse(string _response)
    {
        return JsonUtility.FromJson<MovieResultModelClass>(_response);
    }

    void PopulateMovieCards(MovieResultModelClass _movieResults)
    {

        foreach(MovieResultModelClass.MovieDetail movieDetail in _movieResults.Search)
        {
            GameObject movieCard = (GameObject)Instantiate(Resources.Load(movieCardPath));
            movieCard.transform.SetParent(scrollContentTransform);

            movieCard.GetComponent<MovieCard>().movieDetailObj = movieDetail;
            movieCardsList.Add(movieCard);
        }
    }
    
}
