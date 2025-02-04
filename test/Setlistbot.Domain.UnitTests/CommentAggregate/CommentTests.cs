using Setlistbot.Domain.CommentAggregate;

namespace Setlistbot.Domain.UnitTests.CommentAggregate
{
    public sealed class CommentTests
    {
        [Fact]
        public void NewComment_ValidParameters_CreatesComment()
        {
            // Arrange
            var id = "1";
            var author = "Author";
            var body = "This is a comment.";
            var permalink = "http://example.com";
            var artistId = "ArtistId";

            // Act
            var comment = Comment.NewComment(id, author, body, permalink, artistId);

            // Assert
            Assert.Equal(id, comment.Id);
            Assert.Equal(author, comment.Author);
            Assert.Equal(body, comment.Body);
            Assert.Equal(permalink, comment.Permalink);
            Assert.Equal(artistId, comment.ArtistId);
            Assert.Equal(string.Empty, comment.Reply);
        }

        [Fact]
        public void HasMentionOf_ContainsText_ReturnsTrue()
        {
            // Arrange
            var comment = Comment.NewComment(
                "1",
                "Author",
                "This is a comment mentioning something.",
                "http://example.com",
                "ArtistId"
            );

            // Act
            var result = comment.HasMentionOf("mentioning");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasMentionOf_DoesNotContainText_ReturnsFalse()
        {
            // Arrange
            var comment = Comment.NewComment(
                "1",
                "Author",
                "This is a comment.",
                "http://example.com",
                "ArtistId"
            );

            // Act
            var result = comment.HasMentionOf("mentioning");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SetReply_ValidReply_SetsReply()
        {
            // Arrange
            var comment = Comment.NewComment(
                "1",
                "Author",
                "This is a comment.",
                "http://example.com",
                "ArtistId"
            );
            var reply = "This is a reply.";

            // Act
            comment.SetReply(reply);

            // Assert
            Assert.Equal(reply, comment.Reply);
        }

        [Fact]
        public void SetReply_EmptyReply_ExpectException()
        {
            // Arrange
            var comment = Comment.NewComment(
                "1",
                "Author",
                "This is a comment.",
                "http://example.com",
                "ArtistId"
            );
            var reply = string.Empty;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => comment.SetReply(reply));
        }

        [Fact]
        public void Dates_ValidBody_ReturnsDates()
        {
            // Arrange
            var comment = Comment.NewComment(
                "1",
                "Author",
                "This is a comment mentioning 2023-10-01 and 2023-10-02.",
                "http://example.com",
                "ArtistId"
            );

            // Act
            var result = comment.Dates;

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(new DateOnly(2023, 10, 1), result);
            Assert.Contains(new DateOnly(2023, 10, 2), result);
        }

        [Fact]
        public void Dates_NoDatesInBody_ReturnsEmptyList()
        {
            // Arrange
            var comment = Comment.NewComment(
                "1",
                "Author",
                "This is a comment without dates.",
                "http://example.com",
                "ArtistId"
            );

            // Act
            var result = comment.Dates;

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ParentId_ValidId_ReturnsParentId()
        {
            // Arrange
            var comment = Comment.NewComment(
                "1",
                "Author",
                "This is a comment.",
                "http://example.com",
                "ArtistId"
            );

            // Act
            var result = comment.ParentId;

            // Assert
            Assert.Equal("t1_1", result);
        }
    }
}
