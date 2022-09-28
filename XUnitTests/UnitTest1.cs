using Application;
using Application.Interfaces;
using Domain;
using Moq;

namespace XUnitTests;

public class ServiceTests
{

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
        Mock<IReviewRepository> mock = new Mock<IReviewRepository>();
        mock.Setup(repository => repository.GetReviews()).Returns(() => CreateTestReviews());
        var service = new ReviewService(mock.Object);
        
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
        Mock<IReviewRepository> mock = new Mock<IReviewRepository>();
        mock.Setup(repository => repository.GetReviews()).Returns(() => CreateTestReviews());
        var service = new ReviewService(mock.Object);
        
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
        Mock<IReviewRepository> mock = new Mock<IReviewRepository>();
        mock.Setup(repository => repository.GetReviews()).Returns(() => CreateTestReviews());
        var service = new ReviewService(mock.Object);
        
        //Act
        var actual = service.GetAverageRateFromReviewer(id);
        
        //Assert
        Assert.Equal(Expetced, actual, 3);
    }
    
    [Fact]
    public void TestGetAverageRateFromReviewerThrowsException()
    {
        //Arrange
        Mock<IReviewRepository> mock = new Mock<IReviewRepository>();
        mock.Setup(repository => repository.GetReviews()).Returns(() => CreateTestReviews());
        var service = new ReviewService(mock.Object);
        
        //Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => service.GetAverageRateFromReviewer(-1));
        Assert.Equal("Reviewer does not exist", exception.Message);
    }

    [Theory]
    [InlineData(7, 1, 0)]  // 1 rate should return 0
    [InlineData(7, 2, 1)]  // 2 rate should return 1
    [InlineData(7, 4, 2)]  // 4 rate should return 2
    public void GetNumberOfRatesByReviewerNoExecpt(int reviewer, int rate, int expectedCount)
    {
        //Arrange
        Mock<IReviewRepository> mock = new Mock<IReviewRepository>();
        mock.Setup(repository => repository.GetReviews()).Returns(() => CreateTestReviews());
        var service = new ReviewService(mock.Object);
        int actual;
    
        actual = service.GetNumberOfRatesByReviewer(reviewer, rate);

        //Assert
        Assert.Equal(expectedCount, actual);
    }

    [Theory]
    [InlineData(7, -2)] // Negative rate should return 0
    [InlineData(7, 0)]  // 0 rate should return 0
    public void GetNumberOfRatesByReviewerExecpt(int reviewer, int rate)
    {
        //Arrange
        Mock<IReviewRepository> mock = new Mock<IReviewRepository>();
        mock.Setup(repository => repository.GetReviews()).Returns(() => CreateTestReviews());
        var service = new ReviewService(mock.Object);

        //Assert
        Assert.Throws<ArgumentException>(() => service.GetNumberOfRatesByReviewer(reviewer, rate));
    }


}