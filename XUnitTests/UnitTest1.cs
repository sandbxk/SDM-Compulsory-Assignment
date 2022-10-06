using System.Reflection;
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
        reviews.Add(new Review { Reviewer = 1, Movie = 4, Grade = 2, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 1, Movie = 5, Grade = 2, Date = DateTime.Now });
        
        reviews.Add(new Review { Reviewer = 2, Movie = 1, Grade = 5, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 2, Movie = 3, Grade = 3, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 2, Movie = 4, Grade = 2, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 2, Movie = 5, Grade = 2, Date = DateTime.Now });
        
        reviews.Add(new Review { Reviewer = 3, Movie = 2, Grade = 4, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 3, Movie = 3, Grade = 2, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 3, Movie = 1, Grade = 2, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 3, Movie = 4, Grade = 2, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 3, Movie = 5, Grade = 2, Date = DateTime.Now });
        
        reviews.Add(new Review { Reviewer = 4, Movie = 2, Grade = 4, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 4, Movie = 3, Grade = 5, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 4, Movie = 4, Grade = 5, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 4, Movie = 5, Grade = 2, Date = DateTime.Now });
        
        reviews.Add(new Review { Reviewer = 5, Movie = 3, Grade = 1, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 5, Movie = 4, Grade = 2, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 5, Movie = 5, Grade = 2, Date = DateTime.Now });
        
        reviews.Add(new Review { Reviewer = 7, Movie = 3, Grade = 2, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 7, Movie = 2, Grade = 4, Date = DateTime.Now.Subtract(TimeSpan.FromDays(1)) });
        reviews.Add(new Review { Reviewer = 7, Movie = 1, Grade = 4, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 7, Movie = 4, Grade = 5, Date = DateTime.Now });
        reviews.Add(new Review { Reviewer = 7, Movie = 5, Grade = 5, Date = DateTime.Now });
        

        return reviews;
    }

    [Fact]
    public void TestServiceConstructor()
    {
        //Arrange + Act
        var service = GetMockService();
        
        //Assert
        Assert.IsType<ReviewService>(service);
        Assert.NotNull(service);
    }
    
    
    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 4)]
    [InlineData(3, 5)]
    [InlineData(4, 4)]
    [InlineData(5, 3)]
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
    [InlineData(1, 2.4)]
    [InlineData(2, 3)]
    [InlineData(3, 2.4)]
    [InlineData(4, 4)]
    [InlineData(5, 1.667)]
    public void TestGetAverageRateFromReviewer(int id, double expected)
    {
        //Arrange
        var service = GetMockService();
        
        //Act
        var actual = service.GetAverageRateFromReviewer(id);
        
        //Assert
        Assert.Equal(expected, actual, 3);
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

    [Theory]
    [InlineData(1 , 3)]
    [InlineData(3 , 2.5)]
    [InlineData(10, 0)] // movie may exist even though it does not have any reviews
    public void TestGetAverageRateOfMovie(int movieId, double expectedReviewCount)
    {
        //Arrange
        var service = GetMockService();

        //Act
        var actual = service.GetAverageRateOfMovie(movieId);

        //Assert
        Assert.Equal(expectedReviewCount, actual, 3);
    }

    [Theory]
    [InlineData(3, 5, 1)]
    [InlineData(3, 2, 3)]
    [InlineData(3, 4, 0)]
    [InlineData(10, 5, 0)] // movie may exist even though it does not have any reviews
    public void TestGetNumberOfRates(int movieId, int rate, int expectedCount)
    {
        //Arrange
        var service = GetMockService();

        //Act
        var actual = service.GetNumberOfRates(movieId, rate);

        //Assert
        Assert.Equal(expectedCount, actual);
    }

    [Theory]
    [InlineData(3, 0)]
    [InlineData(3, 6)]
    [InlineData(3, -1)]
    public void TestInvalidRateGetNumberOfRates(int movieId, int rate)
    {
        //Arrange
        var service = GetMockService();

        //Act & Assert
        Assert.Throws<ArgumentException>(() => service.GetNumberOfRates(movieId, rate));
    }


    /**
     * From test reviews.
     * Movie	Times graded as 5
        1			1
        2			1
        3			1
        4			2
        5			1
     */
    [Fact]
    public void TestGetMoviesWithHighestNumberOfTopRates()
    {
        //Arrange
        var service = GetMockService();

        //Act
        var actual = service.GetMoviesWithHighestNumberOfTopRates();
        
        //Assert
        Assert.Equal(new List<int>() {4}, actual);
    }

    [Fact]
    public void TestEmptyGetMoviesWithHighestNumberOfTopRates()
    {
        //Arrange
        Mock<IReviewRepository> mock = new Mock<IReviewRepository>();
        mock.Setup(repository => repository.GetReviews()).Returns(() => new List<Review>(){
            new Review() { Reviewer = 1, Movie = 1, Grade = 1, Date = DateTime.Now },
            new Review() { Reviewer = 2, Movie = 2, Grade = 2, Date = DateTime.Now },
            new Review() { Reviewer = 3, Movie = 3, Grade = 3, Date = DateTime.Now },
            new Review() { Reviewer = 4, Movie = 4, Grade = 4, Date = DateTime.Now },
        });
        
        var service = new ReviewService(mock.Object);

        //Act
        var actual = service.GetMoviesWithHighestNumberOfTopRates();
        
        //Assert
        Assert.Equal(new List<int>(), actual);
    }

    [Fact]
    public void TestGetMostProductiveReviewers()
    {
        //Arrange
        var service = GetMockService();
        
        //Act
        var actual = service.GetMostProductiveReviewers();
        
        //Assert
        Assert.NotNull(actual);
        Assert.Equal(6, actual.Count);
        Assert.Equal(1, actual[0]);
        Assert.Equal(3, actual[1]);
        Assert.Equal(7, actual[2]);
        Assert.Equal(2, actual[3]);
        Assert.Equal(4, actual[4]);
        Assert.Equal(5, actual[5]);

    }
    
    /**
     * From test reviews.
     * Movies        Average rate
        1			    3
        2			    4,25
        3			    2,5
        4			    3
        5			    2,5
     */
    [Theory]
    [InlineData(1, new int[] {2})]
    [InlineData(2, new int[] {2, 1})]
    [InlineData(3, new int[] {2, 1, 4})]
    [InlineData(4, new int[] {2, 1, 4, 3})]
    [InlineData(5, new int[] {2, 1, 4, 3, 5})]
    [InlineData(6, new int[] {2, 1, 4, 3, 5})]
    [InlineData(-1, new int[] {})]
    public void TestGetTopRatedMovies(int amount, int[] expectedValues)
    {
        //Arrange
        var service = GetMockService();
        
        //Act
        var actual = service.GetTopRatedMovies(amount);
        
        //Assert
        Assert.NotNull(actual);
        Assert.Equal(expectedValues.Length, actual.Count);
        for (int i = 0; i < expectedValues.Length; i++)
        {
            Assert.Equal(expectedValues[i], actual[i]);
        }

    }
    
    [Theory]
    [InlineData(1, new int[]{2, 5, 4, 3, 1})]
    [InlineData(2, new int[]{1, 3, 5, 4})]
    [InlineData(3, new int[]{2, 5, 4, 1, 3})]
    [InlineData(4, new int[]{4, 3, 2, 5})]
    [InlineData(5, new int[]{5, 4, 3})]
    [InlineData(7, new int[]{5, 4, 1, 2, 3})]
    public void TestGetTopMoviesByReviewer(int reviewer, int[] movies)
    {
        //Arrange
        var service = GetMockService();

        //Act
        var actual = service.GetTopMoviesByReviewer(reviewer);

        //Assert
        Assert.Equal(movies, actual);

    }
    
    [Fact]
    public void TestGetTopMoviesByReviewerThrowsException()
    {
        //Arrange
        var service = GetMockService();

        //Act & Assert
        Exception e = Assert.Throws<ArgumentException>(() => service.GetTopMoviesByReviewer(9));
        Assert.Equal("Reviewer does not exist", e.Message);
    }
    
    
    [Theory]
    [InlineData(1, new int[] {2, 7, 3, 1} )] //grades: 5, 4, 2, 1
    [InlineData(2, new int[] {1, 4, 3, 7} )] //grades: 5, 4, 4, 4 where reviewer 7's review is one day older
    [InlineData(3, new int[] {4, 2, 7, 3, 1, 5} )] //grades: 5, 3, 2, 2, 2, 1 where reviewer 7's review is one day older
    public void TestGetReviewersByMovie(int movieId, int[] expectedReviewerIds)
    {
        //Arrange
        var service = GetMockService();
        
        //Act
        var actual = service.GetReviewersByMovie(movieId);
        
        //Assert
        Assert.NotNull(actual);
        Assert.Equal(expectedReviewerIds, actual);
    }

    [Fact]
    public void TestGetReviewersByMovieThrowsException()
    {
        //Arrange
        var service = GetMockService();
        
        //Act & Assert
        Exception e = Assert.Throws<ArgumentException>(() => service.GetReviewersByMovie(6));
        Assert.Equal("Movie does not exist", e.Message);
    }
    
    
}
