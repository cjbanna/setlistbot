# Domain

```mermaid
flowchart LR

    ShowManagement[Show Management]
    Show

    Show --- Ratings
    Show --- Attendance
    Show --- Discussion
    ShowManagement --- Submissions
    ShowManagement --- Band
    ShowManagement --- Show
    Show --- Band
    Album --- Band
    Band --- Artist
    Membership --- Attendance
    Membership --- Ratings
    Membership --- Discussion
    Membership --- Messaging
    Membership --- Auth
    Notifications --- Submissions
    Messaging --- Notifications
    Email --- Notifications
    Discussion --- Notifications
```

### Show Management

- Members can create and edit shows
- Can see a full history of changes to shows

### Membership

- Can manage Member aspects like name, avatar, roles, etc.
- Add and remove moderators
- Manage email notification settings

### Discussion

- Members can comment on shows
- Members can vote on comments
- Members can flag inappropriate comments
- Moderators can remove comments

### Ratings

- Members can rate shows

### Attendance

- Members can mark attendance for shows

### Submissions

- Members can submit new shows or edits to existing shows
- Submissions are reviewed by moderators
- Moderators can approve or reject submissions
- Moderators and members can comment on submissions

### Notifications

- Members can receive notifications for submissions changes such as approved submissions, rejected submissions, and comments on submissions
- Member can receive notifications when a moderator sends them a message

### Email

- Sends emails for notifications

### Messaging

- Members can send messages to moderators

### Band

- Moderators can add and edit band names and performers

### Artist

- Moderators can add and edit artist names

### Album

- Moderators can add and edit albums

# Bounded Contexts

```mermaid
flowchart

  Membership
  Music
  Submissions

  Membership --- Auth
  Membership --- Music
  Membership --- Email
  Submissions --- Music
  Submissions --- Membership

  subgraph email[External]
    Email
  end

  subgraph auth[External]
    Auth
  end
```

## Music

- Shows
- Bands
- Artists
- Albums

## Membership

## Submissions

## Auth

## Email

<!-- ```mermaid
flowchart LR

  subgraph membership[Membership]
    Membership
    Messaging
    Notifications
  end

  subgraph show[Show]
    ShowManagement[Show Management]
    Bands
    Ratings
    Attendance
    Discussion
  end

  Notifications

  subgraph email[External]
    Email
  end

  subgraph auth[External]
    Auth
  end

  Membership --- Auth
  Membership --- ShowManagement
  Membership --- Ratings
  Membership --- Attendance
  Membership --- Discussion
  Membership --- Messaging
  Membership --- Notifications
  Notifications --- Email
  ShowManagement --- Bands
  ShowManagement --- Discussion
  ShowManagement --- Notifications

``` -->

# Aggregates

## Membership Bounded Context

```mermaid
classDiagram

  class MemberAggregate {
    string Id
    string Name
    string? Avatar
    string[] Roles
    string CreatedOn
    New()
    AddRole(role)
    RemoveRole(role)
    ChageName(name)
    ChangeAvatar(avatar)
  }
```

```mermaid
classDiagram

  class AttendanceAggregate {
     string Id
     string ShowId
     string MemberId
     string CreatedOn
     NewAttendance()
    }
```

```mermaid
classDiagram

  class RatingAggregate {
    string Id
    string ShowId
    string MemberId
    int Rating
    string CreatedOn
    New(member, show, rating)
  }
```

## Music Bounded Context

```mermaid
classDiagram

  class ShowAggregate {
    string Id
    string BandId
    string Date
    string Venue
    string City
    string State
    string Country
    string Tour
    Set[] Sets
    string CreatedOn
    string ModifiedOn
    string CreatedBy
    string ModifiedBy
    New(moderator)
    New(submission)
    UpdateFromModerator(moderator)
    UpdateFromSubmission(submission)
  }

  class Set {
    string Name
    int Position
    Song[] Songs
    Add(name)
    Add(name, songs)
  }

  class Song {
    string Name
    int Position
    string Transition
    string Annotation
    string[] Teases
    dictionary Metadata
    New()
    Update()
  }
```

```mermaid
classDiagram

  class CommentAggregate {
    string Id
    string ShowId
    string Text
    string MemberId
    string MemberName
    string? Avatar
    string CreatedOn
    string ModifiedOn
    string ModifiedBy
    bool Removed
    New(member, text)
    Remove(member)
    Edit(member, text)
  }

  class CommentVote {
    string Id
    string ShowCommentId
    string MemberId
    string CreatedOn
    Vote? Vote
    Upvote(member)
    Downvote(member)
    RemoveVote(member)
  }

  class Vote {
    <<enumeration>>
    Up
    Down
  }
```

```mermaid
classDiagram

  class BandAggregate {
    string Id
    string Name
    Performer[] Performers
    string CreatedOn
    string ModifiedOn
    string CreatedBy
    string ModifiedBy
    New()

  }

  class Performer {
    string Id
    string Name
    string[] Instruments
    string CreatedOn
    New()
  }
```

```mermaid
classDiagram

  class AlbumAggregate {
    string Id
    string Name
    string ReleaseDate
    string ArtistId
    Song[] Songs
    New()
    AddSong(song)
  }

  class Song {
    string Id
    string Name
    New()
  }
```

## Submissions Bounded Context

```mermaid
classDiagram

  class SubmissionAggregate {
    string Id
    Show Show
    Comment[] Comments
    Status Status
    string StatusChangedOn
    string? StatusChangedBy
    string CreatedOn
    string ModifiedOn
    string CreatedBy
    string ModifiedBy
    Submit(member, show)
    Approve(moderator)
    Reject(moderator)
    AddComment(member, text)
  }

  class Status {
    <<enumeration>>
    Pending
    Approved
    Rejected
  }

    class Show {
    string Id
    string BandId
    string Date
    string Venue
    string City
    string State
    string Country
    string Tour
    Set[] Sets
    string CreatedOn
    string ModifiedOn
    string CreatedBy
    string ModifiedBy
    New()
  }

  class Set {
    string Name
    int Position
    Song[] Songs
    Add(name, position)
  }

  class Song {
    string Name
    int Position
    string Transition
    dictionary Metadata
    string[] Teases
    New()
    Update()
  }
```

```mermaid
classDiagram

  class CommentAggregate {
    string Id
    string Text
    string MemberId
    string MemberName
    string? Avatar
    string CreatedOn
    string ModifiedOn
    string ModifiedBy
    bool Removed
    New(member, text)
    Remove(member)
    Edit(member, text)
  }
```

# Projections

- Show with ratings, attendance, and comments
- Shows by rating
- Shows by band
- All songs
- Shows by song
- All venues
- Shows by venue
- All cities
- Shows by city
- All countries
- Shows by country
- All states
- Shows by state
- All tours
- Shows by tour
- Shows by date (month and day: "On this date...")
- Attendance by member
- Ratings by member
