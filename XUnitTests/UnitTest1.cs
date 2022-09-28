using Application;
using Application.Interfaces;
using Domain;
using Moq;

namespace XUnitTests;

public class ServiceTests
{
    private IReviewService GetMockService()
    {
        Mock<IReviewRepository> mock = new Mock<IReviewRepository>();
        mock.Setup(repository => repository.GetReviews()).Returns(() => CreateTestReviews());
        return new ReviewService(mock.Object);
    }

    private List<Review> CreateTestReviews()
    {
        var reviews = new List<Review>();
        //                           Reviewer ID   Movie ID   Rating     Date
        reviews.Add(new Review { Reviewer = 1, Movie = 1, Grade = 1, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 1, Movie = 2, Grade = 5, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 1, Movie = 3, Grade = 2, Date = DateTime.Now });
        
        reviews.Add(new Review { Reviewer = 2, Movie = 1, Grade = 5, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 2, Movie = 3, Grade = 3, Date = DateTime.Now });
        
        reviews.Add(new Review { Reviewer = 3, Movie = 2, Grade = 4, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 3, Movie = 3, Grade = 2, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 3, Movie = 1, Grade = 2, Date = DateTime.Now });
        
        reviews.Add(new Review { Reviewer = 4, Movie = 2, Grade = 4, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 4, Movie = 3, Grade = 5, Date = DateTime.Now });
        
        reviews.Add(new Review { Reviewer = 5, Movie = 3, Grade = 1, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 7, Movie = 3, Grade = 2, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 7, Movie = 2, Grade = 4, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 7, Movie = 1, Grade = 4, Date = DateTime.Now });

        return reviews;
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    [InlineData(4, 2)]
    [InlineData(5, 1)]
    public void TestGetNumberOfReviewsFromReviewer(int reviewerId, int expectedCount)
    {   
        //Arrange
        var service = GetMockService();
        
        //Act
        var actual = service.GetNumberOfReviewsFromReviewer(reviewerId);
    
        //Assert
        Assert.Equal(expectedCount, actual);    
    }
    
    [Theory]
    [InlineData(6)]
    [InlineData(-1)]
    [InlineData(0)]
    public void TestGetNumberOfReviewsFromReviwerThrowsException(int id)
    {
        //Arrange
        var service = GetMockService();
        
        //Act & Assert
        Assert.Throws<ArgumentException>(() => service.GetNumberOfReviewsFromReviewer(id));
    }

    [Theory]
    [InlineData(1, 2.667)]
    [InlineData(2, 4)]
    [InlineData(3, 2.667)]
    [InlineData(4, 4.5)]
    [InlineData(5, 1)]
    public void TestGetAverageRateFromReviewer(int id, double Expetced)
    {
        //Arrange
        var service = GetMockService();
        
        //Act
        var actual = service.GetAverageRateFromReviewer(id);
        
        //Assert
        Assert.Equal(Expetced, actual, 3);
    }
    
    [Fact]
    public void TestGetAverageRateFromReviewerThrowsException()
    {
        //Arrange
        var service = GetMockService();
        
        //Act & Assert
        Assert.Throws<ArgumentException>(() => service.GetAverageRateFromReviewer(-1));
    }

    [Theory]
    [InlineData(7, 1, 0)]  // 1 rate should return 0
    [InlineData(7, 2, 1)]  // 2 rate should return 1
    [InlineData(7, 4, 2)]  // 4 rate should return 2
    public void GetNumberOfRatesByReviewerNoExecpt(int reviewer, int rate, int expectedCount)
    {
        //Arrange
        var service = GetMockService();
    
        //Act
        int actual = service.GetNumberOfRatesByReviewer(reviewer, rate);

        //Assert
        Assert.Equal(expectedCount, actual);
    }

    [Theory]
    [InlineData(7, -2)] // Negative rate should return 0
    [InlineData(7, 0)]  // 0 rate should return 0
    public void GetNumberOfRatesByReviewerExecpt(int reviewer, int rate)
    {
        //Arrange
        var service = GetMockService();

        //Act & Assert
        Assert.Throws<ArgumentException>(() => GetMockService().GetNumberOfRatesByReviewer(reviewer, rate));
    }

    [Theory]
    [InlineData(1 , 4)]
    [InlineData(3 , 6)]
    [InlineData(10, 0)] // movie may exist even though it does not have any reviews
    public void TestGetNumberOfReviews(int movieId, int expectedReviewCount) 
    {
        //Arrange
        var service = GetMockService();

        //Act
        var actual = service.GetNumberOfReviews(movieId);

        //Assert
        Assert.Equal(expectedReviewCount, actual);
    }

        //Assert
        Assert.Throws<ArgumentException>(() => service.GetNumberOfRatesByReviewer(reviewer, rate));
    }


}