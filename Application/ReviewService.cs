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
        if (rate <= 0 || rate >= 6)
            throw new ArgumentException();

        var review = _reviewRepository.GetReviews().FindAll(x => x.Movie == movie && x.Grade == rate);

        return review.Count;
    }
  
    public List<int> GetMoviesWithHighestNumberOfTopRates()
    {
        var reviews = _reviewRepository.GetReviews().FindAll(x => x.Grade == 5);
    
        if (reviews.Count == 0)
            return new List<int>();

        List<(int id, int occurences)> idOccur = new();

        foreach (var unique in reviews.DistinctBy(x => x.Movie))
        {
            idOccur.Add((unique.Movie, reviews.Count(x => x.Movie == unique.Movie)));    
        }

        int maxOccurence = idOccur.Max(x => x.occurences);

        return idOccur.Where(x => x.occurences == maxOccurence).Select(x => x.id).ToList();
    }

    public List<int> GetMostProductiveReviewers()
    {
        var reviews = _reviewRepository.GetReviews();
        
        List<(int id, int occurences)> idOccur = new();
        
        foreach (var unique in reviews.DistinctBy(review => review.Reviewer))
        {
            idOccur.Add((unique.Reviewer, reviews.Count(x => x.Reviewer == unique.Reviewer)));    
        }
        
        idOccur.Sort((tuple, valueTuple) => valueTuple.occurences.CompareTo(tuple.occurences));
        
        return idOccur.Select(x => x.id).ToList();
    }

    public List<int> GetTopRatedMovies(int amount)
    {
        var reviews = _reviewRepository.GetReviews();
        
        List<(int id, double average)> idAverage = new();
        
        foreach (var unique in reviews.DistinctBy(review => review.Movie))
        {
            idAverage.Add((unique.Movie, reviews.Where(x => x.Movie == unique.Movie).Average(x => x.Grade)));    
        }
        
        idAverage.Sort((tuple, valueTuple) => valueTuple.average.CompareTo(tuple.average));
        
        return idAverage.Select(x => x.id).Take(amount).ToList();
    }

    public List<int> GetTopMoviesByReviewer(int reviewer)
    {
        var reviews = _reviewRepository.GetReviews().FindAll(x => x.Reviewer == reviewer);
        if (reviews.Count == 0)
            throw new ArgumentException("Reviewer does not exist");

        reviews.Sort((x, y) =>
        { 
            var ret = y.Grade.CompareTo(x.Grade);
            if (ret == 0) ret = y.Date.CompareTo(x.Date);
            return ret;
        });
        
        return reviews.Select(x => x.Movie).ToList();
    }

    public List<int> GetReviewersByMovie(int movie)
    {
        var reviews = _reviewRepository.GetReviews().FindAll(x => x.Movie == movie);
        if (reviews.Count == 0)
            throw new ArgumentException("Movie does not exist");
        

        reviews.Sort((x, y) =>
        { 
            var ret = y.Grade.CompareTo(x.Grade);
            if (ret == 0) ret = y.Date.CompareTo(x.Date);
            return ret;
        });

        return reviews.Select(x => x.Reviewer).ToList();
    }
}