using Domain;

namespace Application.Interfaces;

public interface IReviewRepository
{
    List<Review> GetReviews();
    Review GetReview(int id);
    void AddReview(Review review);
    void UpdateReview(Review review);
    void DeleteReview(int id);
    
    
}