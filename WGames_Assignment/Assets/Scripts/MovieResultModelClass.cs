using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovieResultModelClass
{
    public string totalResults;
    public string Response;

    public List<MovieDetail> Search;

    [System.Serializable]
    public class MovieDetail
    {
        public string Title;
        public string imdbID;
        public string Poster;
        public string Type;
        public string Year;
    }
}
