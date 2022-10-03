namespace Application.Interfaces;

public interface IReviewService
{

    
    /// On input N, what are the number of reviews from reviewer N?
    int GetNumberOfReviewsFromReviewer(int reviewer);
    /// On input N, what is the average rate that reviewer N had given?
    double GetAverageRateFromReviewer(int reviewer);
    /// On input N and R, how many times has reviewer N given rate R?
    int GetNumberOfRatesByReviewer(int reviewer, int rate);
    /// On input N, how many have reviewed movie N?
    int GetNumberOfReviews(int movie);
    /// On input N, what is the average rate the movie N had received?
    double GetAverageRateOfMovie(int movie);
    /// On input N and R, how many times had movie N received rate R?
    int GetNumberOfRates(int movie, int rate);

    /// What is the id(s) of the movie(s) with the highest number of top rates (5)?
    List<int> GetMoviesWithHighestNumberOfTopRates();

    /// What reviewer(s) had done most reviews?
    List<int> GetMostProductiveReviewers();
    
    
    /// <summary>
    /// On input N, what is top N of movies? The score of a movie is its average rate.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    List<int> GetTopRatedMovies(int reviewer);

    /// <summary>
    /// On input N, what are the movies that reviewer N has reviewed? The list should
    /// be sorted decreasing by rate first, and date secondly.
    /// </summary>
    /// <param name="reviewer"></param>
    /// <returns></returns>
    List<int> GetTopMoviesByReviewer(int reviewer);
    
    /// <summary>
    /// On input N, who are the reviewers that have reviewed movie N?
    /// The list should be sorted decreasing by rate first, and date secondly.
    /// </summary>
    /// <param name="movie"></param>
    /// <returns></returns>
    List<int> GetReviewersByMovie(int movie);

}