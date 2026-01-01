using Setlistbot.Domain.PostAggregate;

namespace Setlistbot.Domain.UnitTests.PostAggregate
{
    public class PostTests
    {
        [Fact]
        public void NewPost_ValidParameters_CreatesPost()
        {
            // Arrange
            var id = "1";
            var author = "Author";
            var title = "This is a post.";
            var selfText = "This is the self text.";
            var permalink = "http://example.com";
            var artistId = "ArtistId";

            // Act
            var post = Post.NewPost(id, author, title, selfText, permalink, artistId);

            // Assert
            Assert.Equal(id, post.Id);
            Assert.Equal(author, post.Author);
            Assert.Equal(title, post.Title);
            Assert.Equal(selfText, post.SelfText);
            Assert.Equal(permalink, post.Permalink);
            Assert.Equal(artistId, post.ArtistId);
        }

        [Fact]
        public void HasMentionOf_ContainsText_ReturnsTrue()
        {
            // Arrange
            var post = Post.NewPost(
                "1",
                "Author",
                "This is a post mentioning something.",
                "This is the self text.",
                "http://example.com",
                "ArtistId"
            );

            // Act
            var result = post.HasMentionOf("mentioning");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasMentionOf_DoesNotContainText_ReturnsFalse()
        {
            // Arrange
            var post = Post.NewPost(
                "1",
                "Author",
                "This is a post.",
                "This is the self text.",
                "http://example.com",
                "ArtistId"
            );

            // Act
            var result = post.HasMentionOf("mentioning");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SetReply_ValidParameters_SetsReply()
        {
            // Arrange
            var post = Post.NewPost(
                "1",
                "Author",
                "This is a post.",
                "This is the self text.",
                "http://example.com",
                "ArtistId"
            );
            var reply = "This is a reply.";

            // Act
            post.SetReply(reply);

            // Assert
            Assert.Equal(reply, post.Reply);
        }

        [Fact]
        public void SetReply_EmptyReply_ThrowsArgumentException()
        {
            // Arrange
            var post = Post.NewPost(
                "1",
                "Author",
                "This is a post.",
                "This is the self text.",
                "http://example.com",
                "ArtistId"
            );

            // Act & Assert
            Assert.Throws<ArgumentException>(() => post.SetReply(string.Empty));
        }

        [Fact]
        public void SetReply_NullReply_ThrowsArgumentException()
        {
            // Arrange
            var post = Post.NewPost(
                "1",
                "Author",
                "This is a post.",
                "This is the self text.",
                "http://example.com",
                "ArtistId"
            );

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => post.SetReply(null!));
        }

        [Fact]
        public void Dates_ValidPost_ReturnsDates()
        {
            // Arrange
            var post = Post.NewPost(
                "1",
                "Author",
                "This is a post mentioning 2023-01-01.",
                "This is the self text mentioning 2023-02-02.",
                "http://example.com",
                "ArtistId"
            );

            // Act
            var result = post.Dates;

            // Assert
            Assert.Contains(new DateOnly(2023, 1, 1), result);
            Assert.Contains(new DateOnly(2023, 2, 2), result);
        }

        [Fact]
        public void Dates_NoDates_ReturnsEmptyCollection()
        {
            // Arrange
            var post = Post.NewPost(
                "1",
                "Author",
                "This is a post.",
                "This is the self text.",
                "http://example.com",
                "ArtistId"
            );

            // Act
            var result = post.Dates;

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ParentId_ValidPost_ReturnsParentId()
        {
            // Arrange
            var post = Post.NewPost(
                "1",
                "Author",
                "This is a post.",
                "This is the self text.",
                "http://example.com",
                "ArtistId"
            );

            // Act
            var result = post.ParentId;

            // Assert
            Assert.Equal("t3_1", result);
        }
    }
}
