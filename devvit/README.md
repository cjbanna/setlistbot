# setlistbot

setlistbot replies with concert setlists when someone mentions a show date in a comment or post. It's built for music communities: r/phish, r/gratefuldead and r/kgatlw.

Write `12/31/95` or `1995-12-31` in a comment and setlistbot replies with the songs played that night, the venue, and links to recordings. When a comment has several dates, it replies with a list of the shows, each linked to its full setlist.

This app replaces the Data API bot [u/setlistbot](https://www.reddit.com/user/setlistbot), which has been answering in these communities for years, as part of Reddit's Data API migration.

## Setup for moderators

Install the app, then open its settings for your community:

- **Artist**: whose setlists to look up: Phish, Grateful Dead, or King Gizzard & the Lizard Wizard. The app does nothing until this is set.
- **Only reply when u/setlistbot is mentioned**: when on, the bot only answers comments and posts that include "setlistbot", like `u/setlistbot 12/31/95`. When off, it answers any comment or post with a date.
- **Most setlists in one reply**: the maximum number of shows in one reply (default 25). Fractions round down, values below 1 count as 1, and anything that isn't a number uses 25.

## How it works

- The app reads new comments and posts in the communities it's installed in, looking for dates.
- It never replies to itself or to the legacy u/setlistbot (so the two can't loop on a shared subreddit), and it replies to each comment or post at most once. While both bots remain active, the legacy bot may still reply once to an app comment when no mention is required.
- Edited comments aren't checked again, so a date added in an edit won't get a reply.
- Grateful Dead setlists are included in the app. Phish setlists come from [phish.net](https://phish.net) and King Gizzard setlists from [kglw.net](https://kglw.net).

## Fetch Domains

The following domains are requested for this app:

- `api.phish.net`: Phish setlists, from the public [phish.net API](https://docs.phish.net/). Only the show date is sent.
- `kglw.net`: King Gizzard & the Lizard Wizard setlists, from the public [kglw.net API](https://kglw.net/api/docs.php). Only the show date is sent.

## Terms and privacy

- [Terms and Conditions](https://github.com/cjbanna/setlistbot/blob/main/devvit/TERMS.md)
- [Privacy Policy](https://github.com/cjbanna/setlistbot/blob/main/devvit/PRIVACY.md)

## Development

Requires Node 24.

```sh
npm install
npm test             # unit tests
npm run test:types   # typecheck
npm run dev          # playtest on your test subreddit
npm run launch       # upload and submit for review
```

First time only: create the app at [developers.reddit.com/new](https://developers.reddit.com/new), named `setlistbot-app`, while logged in to Reddit as the same account as `npm run login`. Skip the `npx devvit init` command the wizard shows you: in this folder it would `git init` a nested repo and rewrite `package.json`.

The phish.net API key is a secret, set once with `npx devvit settings set phishNetApiKey`.

The Grateful Dead data is `src/Setlistbot.Infrastructure.GratefulDead/gd-shows.json` from the C# part of this repo, which the Discord bot also uses. The files in `src/testdata` are replies from the original C# bot, so these tests check that the replies haven't changed.
