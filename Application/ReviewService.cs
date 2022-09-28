using Application.Interfaces;
using System.Linq;
namespace Application;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    
    public ReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }
    
    public int GetNumberOfReviewsFromReviewer(int reviewer)
    {
        var reviews = _reviewRepository.GetReviews();

        if (!reviews.Select(x => x.Reviewer).Contains(reviewer))
        {
            throw new ArgumentException("Reviewer does not exist");
        }
        
        return reviews.Select(x => x).Where(x => x.Reviewer == reviewer).Count();
    }

    public double GetAverageRateFromReviewer(int reviewer)
    {
        var reviews = _reviewRepository.GetReviews().FindAll(x => x.Reviewer.Equals(reviewer));
      
        if (!reviews.Select(x => x.Reviewer).Contains(reviewer))
        {
            throw new ArgumentException("Reviewer does not exist");
        }
        
        return reviews.Average(x => x.Grade);

    }

    public int GetNumberOfRatesByReviewer(int reviewer, int rate)
    {
        var review = _reviewRepository.GetReviews().FindAll(x => x.Reviewer.Equals(reviewer));
        
        if (rate <= 0 || rate > 5)
        {
            throw new ArgumentException("Rate must be between 1 and 5");
        }

        return review.Count(x => x.Grade == rate);
    }

    public int GetNumberOfReviews(int movie)
    {
        var review = _reviewRepository.GetReviews().FindAll(x => x.Movie == movie);

        return review.Count;
    }

    public double GetAverageRateOfMovie(int movie)
    {
        var review = _reviewRepository.GetReviews().FindAll(x => x.Movie == movie);

        if (review.Count == 0)
            return 0.0;

        return review.Average(x => x.Grade);
    }

    public int GetNumberOfRates(int movie, int rate)
    {
        throw new NotImplementedException();
    }

    public List<int> GetMoviesWithHighestNumberOfTopRates()
    {
        throw new NotImplementedException();
    }

    public List<int> GetMostProductiveReviewers()
    {
        throw new NotImplementedException();
    }

    public List<int> GetTopRatedMovies(int amount)
    {
        throw new NotImplementedException();
    }

    public List<int> GetTopMoviesByReviewer(int reviewer)
    {
        throw new NotImplementedException();
    }

    public List<int> GetReviewersByMovie(int movie)
    {
        throw new NotImplementedException();
    }
}